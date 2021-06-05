using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Interfaces
{
    public interface IUserConnection
    {
        void KeepUserConnection(int userId, string connectionId);
        void RemoveUserConnection(string connectionId);
        List<string> GetUserConnections(int userId);  
    }

    public class UserConnection : IUserConnection
    {
        private static Dictionary<int, List<string>> userConnectionMap = new Dictionary<int, List<string>>();
        private static string userConnectionMapLocker = string.Empty;
        public List<string> GetUserConnections(int userId)
        {
            var conn = new List<string>();
            lock (userConnectionMapLocker)
            {
                conn = userConnectionMap[userId];
            }
            return conn;
        }

        public void KeepUserConnection(int userId, string connectionId)
        {
            lock (userConnectionMapLocker)
            {
                if (!userConnectionMap.ContainsKey(userId))
                {
                    userConnectionMap[userId] = new List<string>();
                }
                userConnectionMap[userId].Add(connectionId);
            }
        }

        public void RemoveUserConnection(string connectionId)
        {
            lock (userConnectionMapLocker)
            {
                foreach (var userId in userConnectionMap.Keys)
                {
                    if (userConnectionMap.ContainsKey(userId))
                    {
                        if (userConnectionMap[userId].Contains(connectionId))
                        {
                            userConnectionMap[userId].Remove(connectionId);
                            break;
                        }
                    }
                }
            }
        }


    }
}
