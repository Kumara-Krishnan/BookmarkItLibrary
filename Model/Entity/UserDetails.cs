using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model.Entity
{
    public sealed class UserDetails
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string PocketUserId { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public string CreationTime { get; set; }

        public string UpdatedTime { get; set; }

        public long LastSyncedTime { get; set; }

        public UserDetails() { }

        public UserDetails(string userName, long lastSyncedTime = 0)
        {
            Id = userName;
            UserName = userName;
            LastSyncedTime = lastSyncedTime;
        }
    }
}
