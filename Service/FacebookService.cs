using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using Facebook;
using Newtonsoft.Json;
using fastforward.Models;

namespace fastforward.Service
{
    public class FacebookService
    {
        private FacebookClient _facebookClient;

        public const string UserEducationCacheKey = "UserEducationCache_{0}";

        public FacebookService(FacebookClient client)
        {
            _facebookClient = client;
        }

        public UserEducationCollection GetFriendsEducationCollection()
        {
            string cacheKey = string.Format(UserEducationCacheKey, _facebookClient.AccessToken);

            UserEducationCollection userEducationCollection;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                userEducationCollection = (UserEducationCollection)HttpContext.Current.Cache[cacheKey];
            }
            else
            {
                var query = "SELECT uid, name, pic_square, sex, hometown_location, current_location, education FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1 = me())";
                dynamic parameters = new ExpandoObject();
                parameters.q = query;
                dynamic result = _facebookClient.Get("/fql", parameters);
                string data = result.ToString();
                userEducationCollection = JsonConvert.DeserializeObject<UserEducationCollection>(data);
                HttpContext.Current.Cache.Insert(cacheKey, userEducationCollection, null, DateTime.Now.AddMinutes(30),
                                                 TimeSpan.Zero);
            }

            return userEducationCollection;
        }

        public int GetNumberOfCollegeEducatedFriends()
        {
            var friendsEducation = GetFriendsEducationCollection();
            var collegeEducatedFriends =
                friendsEducation.Data.Where(
                    e => e.Education.Exists(i => i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school"));

            return collegeEducatedFriends.Select(cef => cef.UID).Distinct().Count();
        }

        public int GetNumberOfCollegesInNetwork()
        {
            var collegesWithFriends = GetFriendsCollegeEducationCollection(Int32.MaxValue);
            return collegesWithFriends.Count();
        }

        public int GetNumberOfMajorsInNetwork()
        {
            var majorsWithFriends = GetFriendsTopCollegeMajors(Int32.MaxValue);
            return majorsWithFriends.Count();
        }

        public IEnumerable<FacebookUser> GetCollegeEducatedFriends(int numberOfFriends)
        {
            var friendsEducation = GetFriendsEducationCollection();
            var collegeEducatedFriends =
                friendsEducation.Data.Where(
                    e => e.Education.Exists(i => i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school"));


            return collegeEducatedFriends.Take(numberOfFriends).Select(
                f => new FacebookUser { Name = f.Name, Pic_Square = f.Pic_Square, UID = f.UID }).ToList();
        }

        public IEnumerable<KeyValuePair<string, CollegeAttendee>> GetFriendsCollegeEducationCollectionWithSchoolDetails(int numberOfFriends)
        {
            var friendsEducation = GetFriendsEducationCollection();
            var collegeEducatedFriends =
                friendsEducation.Data.Where(
                    e => e.Education.Exists(i => i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school"));


            var collegesDict = new Dictionary<string, CollegeAttendee>();

            foreach (var friend in collegeEducatedFriends)
            {
                var facebookUser = new FacebookUser
                {
                    Name = friend.Name,
                    Pic_Square = friend.Pic_Square,
                    UID = friend.UID
                };

                foreach (var school in friend.Education.Where(i => i.Type != null && (i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school")))
                {
                    string schoolName = school.School.Name;

                    if (collegesDict.ContainsKey(schoolName))
                    {
                        if (!collegesDict[schoolName].Attendees.Exists(x => x.UID == facebookUser.UID))
                        {
                            collegesDict[schoolName].Count++;
                            collegesDict[schoolName].Attendees.Add(facebookUser);
                        }
                    }
                    else
                    {
                        var collegeAttendee = new CollegeAttendee
                        {
                            Count = 1,
                            CollegeId = school.School.Id,
                            Attendees = new List<FacebookUser> { facebookUser }
                        };

                        collegesDict.Add(school.School.Name, collegeAttendee);
                    }
                }
            }

            return collegesDict.OrderByDescending(x => x.Value.Count).Take(numberOfFriends).ToList();
        }

        /// <summary>
        /// Get a collection of colleges and their associated users
        /// </summary>
        /// <param name="numberOfFriends"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, List<FacebookUser>>> GetFriendsCollegeEducationCollection(int numberOfFriends)
        {
            var friendsEducation = GetFriendsEducationCollection();
            var collegeEducatedFriends =
                friendsEducation.Data.Where(
                    e => e.Education.Exists(i => i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school"));


            var collegesDict = new Dictionary<string, List<FacebookUser>>();

            foreach (var friend in collegeEducatedFriends)
            {
                var facebookUser = new FacebookUser
                {
                    Name = friend.Name,
                    Pic_Square = friend.Pic_Square,
                    UID = friend.UID
                };

                foreach (var school in friend.Education.Where(i => i.Type != null && (i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school")))
                {
                    string schoolName = school.School.Name;

                    if (collegesDict.ContainsKey(schoolName))
                    {
                        if (!collegesDict[schoolName].Exists(x => x.UID == facebookUser.UID))
                        {
                            collegesDict[schoolName].Add(facebookUser);
                        }
                    }
                    else
                    {
                        collegesDict.Add(schoolName, new List<FacebookUser> { facebookUser });
                    }
                }
            }

            return collegesDict.OrderByDescending(x => x.Value.Count).Take(numberOfFriends).ToList();
        }

        public IEnumerable<KeyValuePair<string, List<FacebookUser>>> GetFriendsTopCollegeLocations(int numberOfLocations)
        {
            // Get ALL college education collection
            var collegeFriendsInSchools = GetFriendsCollegeEducationCollectionWithSchoolDetails(Int32.MaxValue);

            // Collection of CollegeIDs and their Locations
            var collegeLocationHash = new Dictionary<string, string>();

            // Collection of locations (City, State) and the FacebookUsers who went to school in this city
            var locationHash = new Dictionary<string, List<FacebookUser>>();

            foreach (KeyValuePair<string, CollegeAttendee> schoolWithAttendees in collegeFriendsInSchools)
            {
                string collegeId = schoolWithAttendees.Value.CollegeId.ToString();

                if (collegeLocationHash.ContainsKey(collegeId))
                {
                    // We have retrieved this college's location already
                    string location = collegeLocationHash[collegeId];
                    if (locationHash.ContainsKey(location))
                    {
                        // Transfer attendees from this school
                        if (locationHash[location] == null)
                        {
                            locationHash[location] = schoolWithAttendees.Value.Attendees;
                        }
                        else
                        {
                            foreach (FacebookUser facebookUser in schoolWithAttendees.Value.Attendees)
                            {
                                //locationHash[location].Add(facebookUser);
                                if (!locationHash[location].Exists(x => x.UID == facebookUser.UID))
                                {
                                    locationHash[location].Add(facebookUser);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new DataException("Location hash missing expected key");
                    }
                }
                else
                {
                    dynamic result = _facebookClient.Get(collegeId);
                    if (result.location != null)
                    {
                        string location = result.location.city + ", " + result.location.state;

                        // Add this college's location to the hash
                        collegeLocationHash.Add(collegeId, location);

                        if (!locationHash.ContainsKey(location))
                        {
                            locationHash.Add(location, null);
                        }

                        // Transfer attendees from this school
                        if (locationHash[location] == null)
                        {
                            locationHash[location] = schoolWithAttendees.Value.Attendees;
                        }
                        else
                        {
                            foreach (FacebookUser facebookUser in schoolWithAttendees.Value.Attendees)
                            {
                                if (!locationHash[location].Exists(x => x.UID == facebookUser.UID))
                                {
                                    locationHash[location].Add(facebookUser);
                                }
                            }
                        }
                    }
                }
            }


            return locationHash.OrderByDescending(x => x.Value.Count()).Take(numberOfLocations).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfMajors"></param>
        /// <returns>Collection of Majors with their associated users</returns>
        public IEnumerable<KeyValuePair<string, List<FacebookUser>>> GetFriendsTopCollegeMajors(int numberOfMajors)
        {
            var friendsEducation = GetFriendsEducationCollection();
            var collegeEducatedFriends =
                friendsEducation.Data.Where(
                    e => e.Education.Exists(i => i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school"));

            var majorsDict = new Dictionary<string, List<FacebookUser>>();

            foreach (var friend in collegeEducatedFriends)
            {
                var facebookUser = new FacebookUser
                {
                    Name = friend.Name,
                    Pic_Square = friend.Pic_Square,
                    UID = friend.UID
                };

                foreach (var school in friend.Education.Where(i => i.Type != null && (i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school")))
                {

                    if (school.Concentration != null)
                    {
                        foreach (var concentration in school.Concentration)
                        {
                            string majorName = concentration.Name;

                            if (majorsDict.ContainsKey(majorName))
                            {

                                if (majorsDict[majorName] == null)
                                {
                                    majorsDict[majorName] = new List<FacebookUser>
                                                                {
                                                                    facebookUser
                                                                };
                                }
                                else
                                {
                                    //majorsDict[majorName].Add(facebookUser);
                                    if (!majorsDict[majorName].Exists(x => x.UID == facebookUser.UID))
                                    {
                                        majorsDict[majorName].Add(facebookUser);
                                    }
                                }

                            }
                            else
                            {
                                majorsDict.Add(majorName, new List<FacebookUser> { facebookUser });
                            }
                        }
                    }
                }
            }

            return majorsDict.OrderByDescending(x => x.Value.Count).Take(numberOfMajors).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfColleges"></param>
        /// <returns>Collection of Sexes (male and female) with associated list of Colleges and their associated users</returns>
        public List<KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>> GetTopCollegesByGender(int numberOfColleges)
        {
            var friendsEducation = GetFriendsEducationCollection();
            var collegeEducatedFriends =
                friendsEducation.Data.Where(
                    e => e.Education.Exists(i => i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school"));

            var femaleCollegeDict = new Dictionary<string, List<FacebookUser>>();
            var maleCollegeDict = new Dictionary<string, List<FacebookUser>>();

            foreach (var friend in collegeEducatedFriends)
            {
                var sex = friend.Sex.ToLower();

                var facebookUser = new FacebookUser
                {
                    Name = friend.Name,
                    Pic_Square = friend.Pic_Square,
                    UID = friend.UID
                };

                foreach (var school in friend.Education.Where(i => i.Type != null && (i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school")))
                {
                    string schoolName = school.School.Name;

                    switch (sex)
                    {
                        case "female":
                            if (femaleCollegeDict.ContainsKey(schoolName))
                            {
                                //femaleCollegeDict[schoolName].Add(facebookUser);
                                if (!femaleCollegeDict[schoolName].Exists(x => x.UID == facebookUser.UID))
                                {
                                    femaleCollegeDict[schoolName].Add(facebookUser);
                                }
                            }
                            else
                            {
                                femaleCollegeDict.Add(schoolName, new List<FacebookUser> { facebookUser });
                            }
                            break;
                        case "male":
                            if (maleCollegeDict.ContainsKey(schoolName))
                            {
                                //maleCollegeDict[schoolName].Add(facebookUser);
                                if (!maleCollegeDict[schoolName].Exists(x => x.UID == facebookUser.UID))
                                {
                                    maleCollegeDict[schoolName].Add(facebookUser);
                                }
                            }
                            else
                            {
                                maleCollegeDict.Add(schoolName, new List<FacebookUser> { facebookUser });
                            }
                            break;
                    }
                }
            }

            var femaleTopColleges =
                femaleCollegeDict.OrderByDescending(x => x.Value.Count).Take(numberOfColleges).ToList();

            var maleTopColleges = maleCollegeDict.OrderByDescending(x => x.Value.Count).Take(numberOfColleges).ToList();


            var ret = new List<KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>>
                          {
                              new KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>("female",
                                                                                                       femaleTopColleges),
                              new KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>("male",
                                                                                                       maleTopColleges)
                          };
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfColleges"></param>
        /// <returns>Collection of Sexes (male and female) with associated list of Majors and their associated users</returns>
        public List<KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>> GetTopMajorsByGender(int numberOfColleges)
        {
            var friendsEducation = GetFriendsEducationCollection();
            var collegeEducatedFriends =
                friendsEducation.Data.Where(
                    e => e.Education.Exists(i => i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school"));

            var femaleMajorDict = new Dictionary<string, List<FacebookUser>>();
            var maleMajorDict = new Dictionary<string, List<FacebookUser>>();

            foreach (var friend in collegeEducatedFriends)
            {
                var sex = friend.Sex.ToLower();

                var facebookUser = new FacebookUser
                {
                    Name = friend.Name,
                    Pic_Square = friend.Pic_Square,
                    UID = friend.UID
                };

                foreach (var school in friend.Education.Where(i => i.Type != null && (i.Type.ToLower() == "college" || i.Type.ToLower() == "graduate school")))
                {
                    if (school.Concentration != null)
                    {
                        foreach (var concentration in school.Concentration)
                        {
                            string majorName = concentration.Name;

                            switch (sex)
                            {
                                case "female":
                                    if (femaleMajorDict.ContainsKey(majorName))
                                    {
                                        //femaleMajorDict[majorName].Add(facebookUser);
                                        if (!femaleMajorDict[majorName].Exists(x => x.UID == facebookUser.UID))
                                        {
                                            femaleMajorDict[majorName].Add(facebookUser);
                                        }
                                    }
                                    else
                                    {
                                        femaleMajorDict.Add(majorName, new List<FacebookUser> { facebookUser });
                                    }
                                    break;
                                case "male":
                                    if (maleMajorDict.ContainsKey(majorName))
                                    {
                                        //maleMajorDict[majorName].Add(facebookUser);
                                        if (!maleMajorDict[majorName].Exists(x => x.UID == facebookUser.UID))
                                        {
                                            maleMajorDict[majorName].Add(facebookUser);
                                        }
                                    }
                                    else
                                    {
                                        maleMajorDict.Add(majorName, new List<FacebookUser> { facebookUser });
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            var femaleTopMajors =
                femaleMajorDict.OrderByDescending(x => x.Value.Count).Take(numberOfColleges).ToList();

            var maleTopMajors = maleMajorDict.OrderByDescending(x => x.Value.Count).Take(numberOfColleges).ToList();


            var ret = new List<KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>>
                          {
                              new KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>("female",
                                                                                                       femaleTopMajors),
                              new KeyValuePair<string, List<KeyValuePair<string, List<FacebookUser>>>>("male",
                                                                                                       maleTopMajors)
                          };
            return ret;
        }
    }
}