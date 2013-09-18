using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using Facebook;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Mvc.Facebook;
using Microsoft.AspNet.Mvc.Facebook.Client;
using Newtonsoft.Json;
using fastforward.Database;
using fastforward.Models;
using fastforward.Service;
using fastforward.Helpers;


namespace fastforward.Controllers
{
    public class HomeController : Controller
    {
        [FacebookAuthorize("email", "user_location", "user_education_history", "friends_education_history")]
        public async Task<ActionResult> Index(FacebookContext context)
        {
            using (var ctx = new FastForwardContext())
            {
                if (ModelState.IsValid)
                {
                    var user = await context.Client.GetCurrentUserAsync<MyAppUser>();
                    CookieHelper.SetCookie(AppConstants.UserIdCookieName, user.Id, Int32.MaxValue);
                    return View(user);
                    //var loc = user.Location.Name.Split(',');
                    //var state = loc[1].Substring(1);
                    //if (ctx.LocalColleges.Any(x => x.State == state))
                    //{
                    //    return View(user);
                    //}
                    //else
                    //{
                    //    return View("ErrorLoc");
                    //}
                }
            }

            return View("Error");
        }

        // This action will handle the redirects from FacebookAuthorizeFilter when 
        // the app doesn't have all the required permissions specified in the FacebookAuthorizeAttribute.
        // The path to this action is defined under appSettings (in Web.config) with the key 'Facebook:AuthorizationRedirectPath'.
        public ActionResult Permissions(FacebookRedirectContext context)
        {
            if (ModelState.IsValid)
            {
                return View(context);
            }

            return View("Error");
        }

        public ActionResult Survey()
        {
            using (var ctx = new FastForwardContext())
            {

                var survey = ctx.Surveys.FirstOrDefault();
                foreach (var question in survey.Questions)
                {
                    question.Answers = question.Answers.ToList();
                    question.Answers.Reverse();
                }

                return View("Survey", survey);

            }
        }

        [HttpPost]
        public ActionResult Survey(Survey survey)
        {
            using (var ctx = new FastForwardContext())
            {
                var dbSurvey = ctx.Surveys.FirstOrDefault();
                var resultDict = new Dictionary<string, int>();
                for (var i = 1; i < 11; i++)
                {
                    resultDict.Add(i.ToString(), 0);
                }

                for (var i = 0; i < dbSurvey.Questions.Count(); i++)
                {
                    dbSurvey.Questions[i].Result = survey.Questions != null ? survey.Questions[i].Result : 0;
                    dbSurvey.Questions[i].Answers = dbSurvey.Questions[i].Answers.ToList();
                    var question = dbSurvey.Questions[i];
                    var indexA = question.GroupIdA;
                    var indexB = question.GroupIdB;
                    var answer = question.Answers.Last().AnswerId - survey.Questions[i].Result;
                    resultDict[indexA.ToString()] += answer;
                    resultDict[indexB.ToString()] += answer;
                }

                var sortedResults = (from entry in resultDict orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                var top1 = Convert.ToInt32(sortedResults.ElementAt(0).Key);
                var top2 = Convert.ToInt32(sortedResults.ElementAt(1).Key);
                var top3 = Convert.ToInt32(sortedResults.ElementAt(2).Key);
                var groupIds = new List<int> {top1, top2, top3};

                var jsonResults = JsonConvert.SerializeObject(groupIds);
                CookieHelper.SetCookie(AppConstants.ResultsCookieName, jsonResults, AppConstants.DefaultCookieLifetimeInMinutes);
                return RedirectToAction("Result", new { calculate = true });
            }
        }

        public ActionResult Result(bool calculate = false)
        {
            var cookie = CookieHelper.GetCookie(AppConstants.ResultsCookieName);
            var model = new Result();
            model.Calculate = calculate;
            const int numberOfCareersToFetch = 9;
            using (var ctx = new FastForwardContext())
            {
                var allCareers = ctx.Careers.Where(c => c != null).ToList();

                if (cookie.Length > 0)
                {
                    model.HasTakenSurvey = true;
                    var cookieResults = cookie.TrimStart('[').TrimEnd(']');
                    var cookieList = cookieResults.Split(',');
                    var groupIds = cookieList.Select(int.Parse).ToList();
                    model.TopCareers = allCareers.Where(c => c.GroupId == groupIds[0] || c.GroupId == groupIds[1] || c.GroupId == groupIds[2]).ToList();
                }
                else
                {
                    model.TopCareers =
                        allCareers.Take(numberOfCareersToFetch).OrderBy(c => Guid.NewGuid()).ToList();
                    model.HasTakenSurvey = false;
                }

                model.RemainingCareers = allCareers.Except(model.TopCareers).ToList();
            }
            return View(model);
        }

        [FacebookAuthorize("email", "user_location", "user_education_history", "friends_education_history")]
        public async Task<ActionResult> Timeline(FacebookContext context, int careerId)
        {
            using (var ctx = new FastForwardContext())
            {
                var events = ctx.Events.Where(x => x.CareerId == careerId).OrderBy(x => x.Index).ToList();
                var timeline = new Timeline();
                timeline.Events = events;
                timeline.Career = ctx.Careers.FirstOrDefault(x => x.CareerId == careerId);
                var user = await context.Client.GetCurrentUserAsync<MyAppUser>();
                if (user.Name.Length > 22)
                {
                    user.Name = user.Name.Split(' ')[0];
                }

                var random = new Random(unchecked((int)(DateTime.Now.Ticks)));
                var len = user.Friends.Data.Count;
                var friends = user.Friends.Data.OrderBy(x => random.Next(len));
                if (len < events.Count)
                {
                    for (var i = 0; i < events.Count - len; i++)
                    {
                        user.Friends.Data.Add(user.Friends.Data[i % len]);
                    }
                }
                timeline.User = user;
                timeline.Friends = friends.ToList();

                var hasState = false;

                if (user.Location != null)
                {
                    var loc = user.Location.Name.Split(',');
                    var state = loc[1].Substring(1);
                    if (ctx.LocalColleges.Any(c => c.State == state))
                    {
                        var locals = ctx.LocalColleges.Where(x => x.State == state);
                        timeline.LocalColleges = locals.ToList();

                        foreach (var ev in timeline.Events.Where(ev => ev.TextContent.Contains("%localcollege%")))
                        {
                            ev.TextContent = ev.TextContent.Replace("%localcollege%", timeline.LocalColleges[0].College);
                        }

                        hasState = true;
                        ViewBag.HasLocation = true;
                    }
                }

                if(! hasState)
                {
                    timeline.Events = timeline.Events.Where(ev => !ev.TextContent.Contains("%localcollege%")).ToList();
                    ViewBag.HasLocation = false;
                }

                timeline.RelatedOccupations = CareerService.GetRelatedOccupations(ctx, careerId).ToList();

                var jsonCareerId = JsonConvert.SerializeObject(careerId);
                CookieHelper.SetCookie(AppConstants.CareerIdCookieName, jsonCareerId, AppConstants.DefaultCookieLifetimeInMinutes);

                return View("Timeline", timeline);
            }

        }

        [FacebookAuthorize("email", "user_location", "user_education_history", "friends_education_history")]
        public ActionResult Network(FacebookContext context)
        {
            ViewBag.AccessToken = context.AccessToken;
            return View();
        }

        public ActionResult Search(string searchText)
        {
            var model = new CareerSearchModel();
            if (CookieHelper.CookieExists(AppConstants.CareerIdCookieName))
            {
                var careerId = Convert.ToInt32(CookieHelper.GetCookie(AppConstants.CareerIdCookieName));
                model.RelatedOccupations = CareerService.GetRelatedOccupations(careerId);
            }

            model.SearchText = searchText;

            if (!String.IsNullOrEmpty(model.SearchText))
            {
                using (var ctx = new CareersEntities())
                {
                    var careers = ctx.spGetCareerListByName(model.SearchText);
                    model.SearchResults = careers.ToList();
                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult DoCareerSearch(string searchText)
        {
            var results = new List<spGetCareerListByName_Result>();
            using (var ctx = new CareersEntities())
            {
                var careers = ctx.spGetCareerListByName(searchText);
                results = careers.ToList();
            }

            return Json(new AjaxResponse { success = true, data = results });
        }

        public ActionResult SearchResultDetails(string careerId, string searchText)
        {
            var model = new CareerSearchResultDetailsModel();
            model.SearchText = searchText;

            using (var ctx = new CareersEntities())
            {
                var description = ctx.spGetCareerInfoBasic(careerId);
                var tasks = ctx.spGetCareerInfoCareerTasks(careerId);
                var knowledge = ctx.spGetCareerInfoKnowledge(careerId);
                var skills = ctx.spGetCareerInfoSkills(careerId);
                var abilities = ctx.spGetCareerInfoAbilities(careerId);
                var activities = ctx.spGetCareerInfoWorkActivities(careerId);
                var interestes = ctx.spGetCareerInfoInterests(careerId);
                var styles = ctx.spGetCareerInfoWorkStyles(careerId);

                model.DescriptionDetails = description.FirstOrDefault();
                model.Tasks = tasks.ToList();
                model.KnowledgeItems = knowledge.ToList();
                model.Skills = skills.ToList();
                model.Abilities = abilities.ToList();
                model.Activities = activities.ToList();
                model.Interests = interestes.ToList();
                model.Styles = styles.ToList();
            }

            if (model.DescriptionDetails == null)
            {
                return View("Search");
            }

            model.CareerName = model.DescriptionDetails.title;
            return View(model);
        }

        public ActionResult Learn()
        {
            var model = new LearnModel();
            List<Video> videos;

            using (var ctx = new FastForwardContext())
            {
                videos = ctx.Videos.OrderBy(v => v.DisplayOrder).ToList();
            }

            model.Videos = videos.Select(v => new VideoModel
                                                  {
                                                      VideoId = v.VideoId,
                                                      Name = v.Name,
                                                      Description = v.Description,
                                                      Video = VideoHelper.GetVideo(v.VideoId)
                                                  }).ToList();

            return View(model);
        }

        public ActionResult CollegeConnections(string accessToken)
        {
            CollegeConnections model;

            if(CacheHelper.ExistsInCache(AppConstants.CollegeConnectionsCacheKey))
            {
                model = (CollegeConnections)CacheHelper.GetItemFromCache(AppConstants.CollegeConnectionsCacheKey);
            }
            else
            {
                model = new CollegeConnections();
                var client = new FacebookClient(accessToken);
                const int DefaultNumOuterLevelsToRetrieve = 8;
                const int NumberOfFeaturedCollegeEducatedFriendsToRetrieve = 16;
                dynamic result = client.Get("/me");
                string data = result.ToString();
                var myFacebookUser = JsonConvert.DeserializeObject<FacebookUser>(data);
                string firstName = myFacebookUser.First_Name;
                string gender = myFacebookUser.Gender;
                string thirdPersonPronoun = gender == "male" ? "his" : "her";

                var facebookService = new FacebookService(client);
                model.NumberOfFriendsWhoWentToCollege = facebookService.GetNumberOfCollegeEducatedFriends();
                model.HasAtLeastMinimumFeaturedCollegeFriends = (model.NumberOfFriendsWhoWentToCollege -
                                                                 NumberOfFeaturedCollegeEducatedFriendsToRetrieve) > 0;

                model.FeaturedCollegeEducatedFriends = facebookService.GetCollegeEducatedFriends(NumberOfFeaturedCollegeEducatedFriendsToRetrieve).ToList();

                model.NumberOfAdditionalFriendsWhoWentToCollege = model.NumberOfFriendsWhoWentToCollege -
                                                                  NumberOfFeaturedCollegeEducatedFriendsToRetrieve;

                model.FriendsTopSchools = facebookService.GetFriendsCollegeEducationCollection(DefaultNumOuterLevelsToRetrieve).ToList();

                model.TopMajorsWithUsers = facebookService.GetFriendsTopCollegeMajors(DefaultNumOuterLevelsToRetrieve).ToList();

                model.TopCollegeLocationsWithUsers = facebookService.GetFriendsTopCollegeLocations(DefaultNumOuterLevelsToRetrieve).ToList();



                var topCollegesByGender = facebookService.GetTopCollegesByGender(DefaultNumOuterLevelsToRetrieve).ToList();
                model.FemaleFriendsCollege = topCollegesByGender.FirstOrDefault(cg => cg.Key == "female").Value;
                model.MaleFriendsCollege = topCollegesByGender.FirstOrDefault(cg => cg.Key == "male").Value;


                var topMajorsByGender = facebookService.GetTopMajorsByGender(DefaultNumOuterLevelsToRetrieve);
                model.FemaleFriendsMajors = topMajorsByGender.FirstOrDefault(mg => mg.Key == "female").Value;
                model.MaleFriendsMajors = topMajorsByGender.FirstOrDefault(mg => mg.Key == "male").Value;

                CacheHelper.AddItemToCache(AppConstants.CollegeConnectionsCacheKey, model);
            }   

            return PartialView("CollegeConnections", model);
        }

       
    }
}
