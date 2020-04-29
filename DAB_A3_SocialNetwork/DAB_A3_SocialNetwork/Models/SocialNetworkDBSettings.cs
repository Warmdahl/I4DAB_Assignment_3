﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAB_A3_SocialNetwork.Models
{
    public class SocialNetworkDBSettings : ISocialNetworkDBSettings
    {
        public string UsersCollectionName { get; set; }
        public string PostsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISocialNetworkDBSettings
    {
        string UsersCollectionName { get; set; }
        string PostsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}