
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Packaging.Ionic.Zip;

namespace LearningManagementSystem.Core.AuthServices
{
    public class RoleManager : IRoleManager
    {
        private readonly AppDbContext _db;

        public RoleManager(AppDbContext db)
        {
            _db = db;
        }

        public async Task<string> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new BadRequestException("Incorrect role name");
            }

            var roleExist = await _db.Roles.FirstOrDefaultAsync(f => f.RoleName.Equals(roleName));
            if (roleExist is not null)
            {
                return roleName;
            }

            var role = new Role()
            {
                RoleName = roleName
            };
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();
            return roleName;
        }

        public async Task<IEnumerable<string>> GetRoles()
        {
            return await _db.Roles.Select(s => s.RoleName).ToListAsync();
        }
    }
}
