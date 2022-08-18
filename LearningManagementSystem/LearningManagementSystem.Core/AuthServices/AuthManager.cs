using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Auth;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LearningManagementSystem.Core.AuthServices
{
    public class AuthManager : IAuthManager
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AuthManager> _logger;
        private readonly IJwtHandler _jwtHandler;
        private readonly IConfiguration _cfg;

        public AuthManager(AppDbContext db,
            ILogger<AuthManager> logger,
            IJwtHandler jwtHandler,
            IConfiguration cfg)
        {
            _db = db;
            _logger = logger;
            _jwtHandler = jwtHandler;
            _cfg = cfg;
        }

        public async Task<Response<bool>> RegisterAsync(RegisterModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var userExist = await _db.Users.FirstOrDefaultAsync(f => f.UserName.Equals(model.UserName) || f.Email.Equals(model.Email));
            if (userExist is not null)
            {
                return Response<bool>.GetError(ErrorCode.BadRequest, "User with such data already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Birthday = model.Birthday,
                PasswordHash = hashedPassword
            };
            await AddToRoleAsync(user, "Student");

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return Response<bool>.GetSuccess(true);
        }

        public async Task<Response<AuthResponse>> SignInAsync(SignInModel model, string ipAddress)
        {
            ArgumentNullException.ThrowIfNull(model);

            var user = await _db.Users
                .Include(i => i.Role)
                .FirstOrDefaultAsync(f => f.UserName.Equals(model.UserName));

            var isCorrectPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (user is null || !isCorrectPassword)
            {
                return Response<AuthResponse>.GetError(ErrorCode.BadRequest, "Wrong username or password!");
            }

            var refreshToken = _jwtHandler.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from user
            RemoveOldRefreshTokens(user);

            // save changes to db
            _db.Update(user);
            _db.SaveChanges();

            var token = _jwtHandler.GenerateToken(user);
            return Response<AuthResponse>.GetSuccess(new AuthResponse
            {
                Id = user.Id,
                Token = token,
                Username = user.UserName,
                Role = user.Role.RoleName,
                RefreshToken = refreshToken.Token
            });
        }

        public Task<AuthResponse> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                RevokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                _db.Update(user);
                _db.SaveChanges();
            }

            if (!refreshToken.IsActive)
                throw new Exception("Invalid token");

            var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);
            RemoveOldRefreshTokens(user);

            _db.Update(user);
            _db.SaveChanges();

            var jwtToken = _jwtHandler.GenerateToken(user);

            return Task.FromResult(new AuthResponse
            {
                Id = user.Id,
                Token = jwtToken,
                RefreshToken = newRefreshToken.Token
            });
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var user = GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new Exception("Invalid token");

            RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _db.Update(user);
            _db.SaveChanges();
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(roleName);

            var role = await _db.Roles.FirstOrDefaultAsync(f => f.RoleName.Equals(roleName));
            if (role is null)
            {
                throw new BadRequestException("Role does not exist");
            }
            user.Role = role;
        }

        public async Task<AuthUserModel> GetUserById(Guid id)
        {
            var user = await _db.Users.Include(i => i.Role).FirstOrDefaultAsync(f => f.Id.Equals(id));
            return new AuthUserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role.RoleName
            };
        }

        #region Helpers

        private User GetUserByRefreshToken(string token)
        {
            var user = _db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                throw new Exception("Invalid token");

            return user;
        }

        private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtHandler.GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void RemoveOldRefreshTokens(User user)
        {
            var ttl = double.Parse(_cfg["RefreshTokenTTL"]);
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(ttl) <= DateTime.UtcNow);
        }

        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    RevokeRefreshToken(childToken, ipAddress, reason);
                else
                    RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }
        #endregion
    }
}
