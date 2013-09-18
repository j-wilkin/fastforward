using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class FastForwardContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<fastforward.Models.fastforwardContext>());

        public DbSet<fastforward.Models.Survey> Surveys{ get; set; }

        public DbSet<fastforward.Models.Question> Questions{ get; set; }

        public DbSet<fastforward.Models.Answer> Answers{ get; set; }
        public DbSet<fastforward.Models.Career> Careers { get; set; }
        public DbSet<fastforward.Models.CareerGroup> CareerGroups { get; set; }
        public DbSet<fastforward.Models.Event> Events { get; set; }
        public DbSet<fastforward.Models.LocalCollege> LocalColleges { get; set; }
        public DbSet<fastforward.Models.RelatedOccupation> RelatedOccupations { get; set; }
        public DbSet<fastforward.Models.Video> Videos { get; set; }
    }
}