using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAB_A3_SocialNetwork.Models
{
    public class SocialNetworkDBSettings : ISocialNetworkDBSettings
    {
        public string UsersCollectionName { get; set; }
        public string PostsCollectionName { get; set; }
        public string CirclesCollectionName { get; set; }
        public string FollowlistCollectionName { get; set; }
        public string BlacklistCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISocialNetworkDBSettings
    {
        string UsersCollectionName { get; set; }
        string PostsCollectionName { get; set; }
        string CirclesCollectionName { get; set; }
        string FollowlistCollectionName { get; set; }
        string BlacklistCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
