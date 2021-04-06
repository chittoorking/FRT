using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Areas.Guest.ViewModels;
using ELearningPortalMSAzureV1.Areas.Learner.ViewModels;
using ELearningPortalMSAzureV1.BusinessComponent;
using ELearningPortalMSAzureV1.ConfigurationData;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.Resources;
using ELearningPortalMSAzureV1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ELearningPortalMSAzureV1.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public readonly IConfiguration configuration;

        //Copied from Blog for Azure appconfig connection.
        private AppSettings AppSettings { get; set; }

        HomePageBusinessComponent homePageBusinessComponent;
        LearningBusinessComponent learningBusinessComponent;
        TeacherBusinessComponent teacherBusinessComponent;

        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public HomeController(IConfiguration config, IOptions<AppSettings> settings)
        {
            this.configuration = config;
            AppSettings = settings.Value;
            homePageBusinessComponent = new HomePageBusinessComponent(config, AppSettings);
            learningBusinessComponent = new LearningBusinessComponent(config, AppSettings);
            teacherBusinessComponent = new TeacherBusinessComponent(config, AppSettings);
        }

        [Route("Index")]
        [Route("~/")]
        public IActionResult Index()
        {
            CommonViewModel vm = new CommonViewModel();
            vm.ScreenContent1 = ScreenContents.IndexPage.Content1;
            return View(vm);
        }
        [Route("Login")]
        public IActionResult Login()
        {
            HttpContext.Session.SetSessionData("UserDetails", null);
            string result = "Test";
            ViewData["result"] = result;
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserLoginDetails userlogin)
        {
            if(ModelState.IsValid)
            {
             if(userlogin.UserLoginValue.Contains("@"))
                {
                    userlogin.UserLoginType = "Email";
                }
               else
                {
                    userlogin.UserLoginType = "Phone";
                }
                userlogin = homePageBusinessComponent.ValidateLogin(userlogin);

                if (userlogin.UserId > 0)
                {
                   var user = homePageBusinessComponent.GetUserDetails(userlogin.UserId);
                    HttpContext.Session.SetSessionData("UserDetails", user);
                    return RedirectToAction("HomePage");
                }
            }
           return View(userlogin);
        }
        [Route("HomePage")]
        public IActionResult HomePage()
        {
           var userData = HttpContext.Session.GetSessionData<Users>("UserDetails");
            ViewData["result"] = userData.FirstName;

            if (userData.UserRole == "Learner")
            {
                return RedirectToAction("Home", "Learner", new { area = "Learner"});
            }
            if (userData.UserRole == "Teacher")
            {
                return RedirectToAction("Home", "Teacher", new { area = "Teacher" });
            }
            if (userData.UserRole == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin", new { area = "Admin" });
            }
            if (userData.UserRole == "Guest")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(userData);
        }

        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }


        [Route("SignUp")]
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var userId = homePageBusinessComponent.SaveUserDetails(vm);
                if (userId > 0)
                {
                    var user = homePageBusinessComponent.GetUserDetails(userId);
                    HttpContext.Session.SetSessionData("UserDetails", user);
                    return RedirectToAction("HomePage");
                }
            }
            if(vm.UserRole== "Admin")
            {
                return RedirectToAction("AdminSignUp", "Admin", new { area = "Admin" });
            }
            
            return View();
        }

        [Route("AboutUs")]
        public IActionResult AboutUs()
        {
            CommonViewModel vm = new CommonViewModel();
            vm.Users = new Users();//For avoiding Null Reference Exception
            vm.ScreenContent1 = ScreenContents.AboutUsPage.Content1;
            return PartialView("_AboutUs", vm);
        }
        [Route("SignOut")]
        public IActionResult SignOut()
        {
            ClearSessions();
            return RedirectToAction("Index");
        }
        private void ClearSessions()
        {
            HttpContext.Session.SetSessionData("UserDetails", null);
            HttpContext.Session.SetSessionData("isLearnerProfileComplete", null);
            HttpContext.Session.SetSessionData("TeacherId", null);
            HttpContext.Session.SetSessionData("LearnerId", null);
            HttpContext.Session.SetSessionData("AdminId", null);
            HttpContext.Session.SetSessionData("isAdminProfileComplete", null);
            HttpContext.Session.SetSessionData("isTeacherProfileComplete", null);

        }
        [Route("IsEmailAddressUnique")]
        [HttpGet]
        public int IsEmailAddressUnique(string EmailAddress)
        {
            //1 Means Unique, 2 Means Not-Unique, -1 Means Invalid operation
            if (EmailAddress != null)
            {
                return homePageBusinessComponent.IsUserIdentifierUnique(EmailAddress);
            }
            else
            {
                return -1;
            }
        }
        [Route("IsPhoneNumberUnique")]
        [HttpGet]
        public int IsPhoneNumberUnique(string Phone)
        {
            //1 Means Unique, 2 Means Not-Unique, -1 Means Invalid operation
            if (Phone != null)
            {
                return homePageBusinessComponent.IsUserIdentifierUnique(Phone);
            }
            else
            {
                return -1;
            }
        }
        [Route("Courses")]
        public IActionResult Courses(string search_Query)
        {
            GuestCourseViewModel vm = new GuestCourseViewModel();
            vm.AvailableCourseList = new List<Course>();
            vm.SearchedCourseList = new List<Course>();
            if (search_Query is null)//Page is loaded Fresh
            {
                vm.IsSearcheClicked = false;
                vm.AvailableCourseList = homePageBusinessComponent.GetAllCourses();
            }
            else
            {
                vm.IsSearcheClicked = true;
                vm.search_Query = search_Query;
                vm.SearchedCourseList = homePageBusinessComponent.GetCourseSearchResults(search_Query);
            }
            return View(vm);
        }
        [Route("CoursePage")]
        public IActionResult CoursePage(int CourseId)
        {
            GuestCourseViewModel vm = new GuestCourseViewModel();
            vm.selectedCourseDetails = homePageBusinessComponent.GetCourseDetails(CourseId);
            return View(vm);
        }
        [Route("CourseRatings")]
        public IActionResult CourseRatings(int CourseId)
        {
            CourseRatingsPageViewModel vm = new CourseRatingsPageViewModel();
            vm.courseRatingsList = homePageBusinessComponent.GetCourseRatingComments(CourseId);
            vm.Users = new Users();
            vm.Users.UserRole = "Guest";
            return PartialView("_CourseRatings", vm);
        }
        [Route("Donate")]
        public IActionResult Donate()
        {
            CommonViewModel vm = new CommonViewModel();
            vm.ScreenContent1 = ScreenContents.DonatePage.Content1;
            return View(vm);
        }

        
    }
}