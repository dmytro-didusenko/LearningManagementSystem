using System.Collections.Concurrent;

namespace LearningManagementSystem.API.SignalRServices
{
    public class UserConnectionService : IUserConnectionService
    {
        private ConcurrentDictionary<Guid, List<string>> _userConnections = new();

        public void AddUserConnection(Guid userID, string connectionId)
        {
            if (_userConnections.ContainsKey(userID))
            {
                _userConnections[userID].Add(connectionId);
                return;
            }
            _userConnections.TryAdd(userID, new List<string> { connectionId });
        }

        public List<string> GetUserConnections(Guid userID)
        {            
            if (UserIsConnected(userID))
            {
               return _userConnections[userID];
            }

            return Array.Empty<string>().ToList();
        }

        public void RemoveAllUserConnection(Guid userID)
        {
            if (_userConnections.ContainsKey(userID))
            {
                _userConnections.TryRemove(new KeyValuePair<Guid, List<string>>(userID, _userConnections[userID]));

                return;
            }
        }

        public void RemoveUserConnection(string connectionId)
        {       
            var userIDByConnectionId = _userConnections.FirstOrDefault(u => u.Value.Contains(connectionId));
            if (userIDByConnectionId.Value is not null && userIDByConnectionId.Value.Any())
            {
                _userConnections[userIDByConnectionId.Key].Remove(connectionId);
            }
        }

        public bool UserIsConnected(Guid userID)
        {
            return _userConnections.ContainsKey(userID);
        }
    }
}
