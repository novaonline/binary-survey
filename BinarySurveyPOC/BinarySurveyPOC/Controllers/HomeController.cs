using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult Vote(int id)
        {
            using (var ctx = new Service.SurveyContext())
            {
                //var hasVoted = ctx.SurveyResponses.Any(x=>x.SurveyId)
                var model = ctx.Surveys.Find(id);
                if (model == null)
                {
                    return HttpNotFound();
                }
                ViewBag.SurveyId = id;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Vote([System.Web.Http.FromUri]int id, [System.Web.Http.FromBody]bool surveyResponse)
        {
            using (var ctx = new Service.SurveyContext())
            {
                var model = ctx.SurveyResponses.Add(new Models.SurveyResponse()
                {
                    AddDate = DateTime.UtcNow,
                    Response = surveyResponse,
                    SurveyId = id
                });
                ctx.SaveChanges();
                if (model.SurveyResponseID <= 0)
                {
                    return PartialView("_SurveyChart");
                }

                // todo, just group them.. but since this is a binary survey...
                var surveyResponses = ctx.SurveyResponses.Where(x => x.SurveyId == id);

                var surveyYes = surveyResponses.Count(y => y.Response == true);
                var surveyNo = surveyResponses.Count(x => x.Response == false);

                var summaryModel = new Models.SurveyResponsesSummary() { One = surveyYes, SurveyId = id, Zero = surveyNo };

                // ping
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.SurveyHub>();
                hubContext.Clients.Group("survey-response-" + id).surveyResponse(new { zero = summaryModel.Zero, one = summaryModel.One });

                return PartialView("_SurveyChart", summaryModel);
            }
        }

        public async Task<ActionResult> Surveys(Models.Coordinates coords)
        {
            using (var ctx = new Service.SurveyContext())
            {
                var survey = await ctx.GetSurveysAsync(coords);
                //var bodystring = RenderViewToString(this.ControllerContext, "_SurveyList", survey);
                //return Json(new { surveys = survey, body = bodystring },JsonRequestBehavior.AllowGet);
                return PartialView("_SurveyList", survey);
            }
        }


        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}