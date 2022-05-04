using System.Linq.Expressions;
using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contexes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            await _context.AddAsync(_mapper.Map<User>(model));
            await _context.SaveChangesAsync();
        }

        public async Task Update(UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            _context.Users.Update(_mapper.Map<User>(model));
            await _context.SaveChangesAsync();
        }

        public async Task Remove(UserModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            _context.Users.Remove(_mapper.Map<User>(model));
            await _context.SaveChangesAsync();
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _mapper.Map<IEnumerable<UserModel>>(_context.Users.AsEnumerable());
        }
    }
}
