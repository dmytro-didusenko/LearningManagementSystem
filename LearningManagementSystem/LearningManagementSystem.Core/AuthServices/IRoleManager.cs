namespace LearningManagementSystem.Core.AuthServices
{
    public interface IRoleManager
    {
        public Task<string> CreateRole(string roleName);
        public Task<IEnumerable<string>> GetRoles();

    }
}
