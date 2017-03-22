using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BinarySurveyPOC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            // this will also have contact info.
            // this will show how to use it
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string surveyQuestion, Models.Coordinates coords, int durationMinutes = 5, double distanceKm = 2)
        {
            using (var ctx = new Service.SurveyContext())
            {
                ctx.Surveys.Add(new Models.Survey()
                {
                    AddDate = DateTime.UtcNow,
                    SurveyQuestion = surveyQuestion,
                    ExpiritionDate = DateTime.UtcNow.AddMinutes(durationMinutes),
                    Location = Helper.Geo.getCircle(coords, distanceKm)
                });
                var t = ctx.SaveChanges();
                // ping
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.SurveyHub>();
                hubContext.Clients.Group("All").checkForNewSurveys();
            }
            return RedirectToAction("Index");
        }

        public async Task<PartialViewResult> Surveys(Models.Coordinates coords)
        {
            using (var ctx = new Service.SurveyContext())
            {
                return PartialView("_SurveyList", await ctx.GetSurveysAsync(coords));
            }
        }
    }
}