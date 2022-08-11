using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Auth;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BCrypt.Net;
using LearningManagementSystem.API.Utils;
using LearningManagementSystem.Core.Exceptions;

namespace LearningManagementSystem.Core.AuthServices
{
    public class UserManager : IUserManager
    {
        private readonly AppDbContext _db;
        private readonly ILogger<UserManager> _logger;
        private readonly JwtHandler _jwtHandler;

        public UserManager(AppDbContext db, ILogger<UserManager> logger, JwtHandler jwtHandler)
        {
            _db = db;
            _logger = logger;
            _jwtHandler = jwtHandler;
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


        public async Task<Response<AuthResponse>> SignInAsync(SignInModel model)
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

            var token = _jwtHandler.GenerateToken(user);
            return Response<AuthResponse>.GetSuccess(new AuthResponse
            {
                Id = user.Id,
                Token = token,
                Username = user.UserName,
                Role = user.Role.RoleName
            });
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
            var user = await _db.Users.Include(i=>i.Role).FirstOrDefaultAsync(f => f.Id.Equals(id));
            return new AuthUserModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role.RoleName
            };
        }
    }
}
