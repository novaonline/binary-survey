using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BinarySurveyPOC.Models;
using System.Data.Entity.Spatial;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BinarySurveyPOC.Helper;
using System.IO;

namespace BinarySurveyPOC.Service
{
    public class SurveyContext : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }

        public SurveyContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new SurveyDBInitializer());
        }

        public Task<List<Survey>> GetSurveysAsync(Models.Coordinates coords)
        {
            return Database.SqlQuery<Survey>("dbo.usp_surveys_get_by_lat_lng @lat, @lng", new SqlParameter("lat", coords.Lat), new SqlParameter("lng", coords.Lng)).ToListAsync();
        }

    }

    public class SurveyDBInitializer : DropCreateDatabaseAlways<SurveyContext>
    {
        protected override void Seed(SurveyContext context)
        {
            IList<Survey> defaultSurvey = new List<Survey>();
            defaultSurvey.Add(new Survey() { AddDate = DateTime.UtcNow.AddMinutes(-30), Location = Geo.getCircle(50.445211, -104.618894, 2), SurveyQuestion = "Do you like this app?" });
            defaultSurvey.Add(new Survey() { AddDate = DateTime.UtcNow.AddMinutes(-20), Location = Geo.getCircle(50.444479, -104.625281, 1), SurveyQuestion = "Do you like the books at the University?" });
            defaultSurvey.Add(new Survey() { AddDate = DateTime.UtcNow.AddMinutes(-10), Location = Geo.getCircle(50.431123, -104.557593, 2), SurveyQuestion = "Was your experience at subway good?" });
            context.Surveys.AddRange(defaultSurvey);
            var sqlFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "SQL/*.sql");
            foreach(string file in sqlFiles)
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file));
            }
            // execute the following sql files

            base.Seed(context);
        }

    }
}