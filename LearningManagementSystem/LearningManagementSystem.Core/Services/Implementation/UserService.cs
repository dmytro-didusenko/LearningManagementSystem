using System.Linq.Expressions;
using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<UserModel>> AddAsync(UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            //TODO: change this
            var userExist = _context.Users.FirstOrDefaultAsync(f =>
                f.UserName.Equals(model.UserName) || f.Email.Equals(model.Email));

            if (userExist is not null)
            {
                return new Response<UserModel>()
                {
                    IsSuccessful = false,
                    Error = "User with such credentials is already exist!"
                };
            }

            await _context.AddAsync(_mapper.Map<User>(model));
            await _context.SaveChangesAsync();
            _logger.LogInformation("New user has been added");

            return new Response<UserModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public async Task Update(Guid id, UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var userExist = await _context.Users.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (userExist is null)
            {
                throw new Exception($"User id:{id} does not exist!");
            }

            _context.Users.Update(_mapper.Map<User>(model));
            await _context.SaveChangesAsync();
            _logger.LogInformation("User {0} has been added", model.Id);
        }

        public async Task Remove(UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var user = await _context.Users.SingleOrDefaultAsync(i => i.Id.Equals(
                model.Id));

            if (user is null)
            {
                //TODO: change this
                _logger.LogInformation("User with id:{0} does not exist", model.Id);
                throw new Exception("User is not exist");
            }

            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {0} has changed status to inactive", model.Id);
        }

        public async Task<Response<UserModel>> GetById(Guid id)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
            if (entity is null)
            {
                return new Response<UserModel>()
                {
                    IsSuccessful = false,
                    Error = $"User with id:{id} does not exist"
                };
            }

            var model = _mapper.Map<UserModel>(entity);
            _logger.LogInformation("Get user by id:{0}", model.Id);
            return new Response<UserModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _mapper.Map<IEnumerable<UserModel>>(_context.Users.AsEnumerable());
        }
    }
}
