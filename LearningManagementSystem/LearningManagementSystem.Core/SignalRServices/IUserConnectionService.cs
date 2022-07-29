namespace LearningManagementSystem.API.SignalRServices
{
    public interface IUserConnectionService
    {
        public void AddUserConnection(Guid userID, string connectionId);

        public void RemoveUserConnection(string connectionId);

        public void RemoveAllUserConnection(Guid userID);

        public bool UserIsConnected(Guid userID);

        public List<string> GetUserConnections(Guid userID);

    }
}
