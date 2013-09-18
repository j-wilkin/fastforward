using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class CollegeConnections
    {
        public int NumberOfFriendsWhoWentToCollege { get; set; }
        public bool HasAtLeastMinimumFeaturedCollegeFriends { get; set; }
        public int NumberOfAdditionalFriendsWhoWentToCollege { get; set; }
        public List<FacebookUser> FeaturedCollegeEducatedFriends { get; set; }
        public List<KeyValuePair<string, List<FacebookUser>>> FriendsTopSchools { get; set; }
        public List<KeyValuePair<string, List<FacebookUser>>> TopMajorsWithUsers { get; set; }
        public List<KeyValuePair<string, List<FacebookUser>>> TopCollegeLocationsWithUsers { get; set; }
        public List<KeyValuePair<string, List<FacebookUser>>> FemaleFriendsCollege { get; set; }
        public List<KeyValuePair<string, List<FacebookUser>>> MaleFriendsCollege { get; set; }
        public List<KeyValuePair<string, List<FacebookUser>>> FemaleFriendsMajors { get; set; }
        public List<KeyValuePair<string, List<FacebookUser>>> MaleFriendsMajors { get; set; }
    }
}