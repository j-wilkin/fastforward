using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class FacebookBaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class UserEducationCollection
    {
        public List<UserEducation> Data { get; set; }
    }

    public class UserEducation
    {
        public long UID { get; set; }
        public string Name { get; set; }
        public string Pic { get; set; }
        public string Pic_Square { get; set; }
        public string Sex { get; set; }
        public Location Hometown_Location { get; set; }
        public Location Current_Location { get; set; }

        public List<SchoolDetails> Education { get; set; }
    }

    public class SchoolDetails
    {
        public School School { get; set; }
        public Year Year { get; set; }
        public string Type { get; set; }
        public Degree Degree { get; set; }
        public List<Concentration> Concentration { get; set; }
    }

    public class CollegeAttendee
    {
        public int Count { get; set; }
        public long CollegeId { get; set; }
        public List<FacebookUser> Attendees { get; set; }
    }

    public class FacebookUser
    {
        public long UID { get; set; }
        public string Name { get; set; }
        public string Pic_Square { get; set; }
        public string Sex { get; set; }
        public string First_Name { get; set; }
        public string Gender { get; set; }
    }

    public class School : FacebookBaseEntity
    {
    }

    //public class Location : FacebookBaseEntity
    //{
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string Country { get; set; }
    //    public string Zip { get; set; }
    //}

    public class Year : FacebookBaseEntity
    {
    }

    public class Concentration : FacebookBaseEntity
    {
    }

    public class Degree : FacebookBaseEntity
    {
    }
}