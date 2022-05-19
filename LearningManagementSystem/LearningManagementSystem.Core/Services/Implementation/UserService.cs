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

            var userExist = await _context.Users.FirstOrDefaultAsync(f =>
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

        public async Task UpdateAsync(Guid id, UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var userExist = await _context.Users.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(id));

            if (userExist is null)
            {
                throw new Exception($"User id:{id} does not exist!");
            }

            model.Id = id;
            _context.Users.Update(_mapper.Map<User>(model));
            await _context.SaveChangesAsync();
            _logger.LogInformation("User[id]:{0} has been updated", model.Id);
        }

        public async Task RemoveAsync(UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var user = await _context.Users.SingleOrDefaultAsync(i => i.Id.Equals(
                model.Id));

            if (user is null)
            {
                _logger.LogInformation("User with id:{0} does not exist", model.Id);
                throw new Exception("User is not exist");
            }

            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {0} has changed status to inactive", model.Id);
        }

        public async Task<UserModel> GetByIdAsync(Guid id)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
            if (entity is null)
            {
                throw new Exception("User does not exist!");
            }

            var model = _mapper.Map<UserModel>(entity);
            _logger.LogInformation("Get user by id:{0}", model.Id);
            return model;
        }

        public async Task<IEnumerable<UserModel>> GetByFilterAsync(UserQueryModel? query = null)
        {
            var queryable = _context.Users.AsQueryable();

            if (query is null)
            {
                return _mapper.Map<List<UserModel>>(await queryable.ToListAsync());
            }

            if (query.UserName is not null)
            {
                queryable = queryable.Where(i => i.UserName.Contains(query.UserName));
            }

            if (query.FirstName is not null)
            {
                queryable = queryable.Where(i => i.FirstName.Contains(query.FirstName));
            }

            if (query.LastName is not null)
            {
                queryable = queryable.Where(i => i.LastName.Contains(query.LastName));
            }

            if (query.BirthdayLessThan is not null)
            {
                queryable = queryable.Where(i => i.Birthday < query.BirthdayLessThan);
            }

            if (query.BirthdayGreaterThan is not null)
            {
                queryable = queryable.Where(i => i.Birthday > query.BirthdayGreaterThan);
            }

            var res = await queryable.ToListAsync();

            return _mapper.Map<List<UserModel>>(res);
        }
    }
}
