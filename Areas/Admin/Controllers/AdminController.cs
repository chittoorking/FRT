using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ELearningPortalMSAzureV1.BusinessComponent;
using ELearningPortalMSAzureV1.ConfigurationData;
using ELearningPortalMSAzureV1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ELearningPortalMSAzureV1.ViewModels;
using ELearningPortalMSAzureV1.Areas.Admin.ViewModels;

namespace ELearningPortalMSAzureV1.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public readonly IConfiguration configuration;

        //Copied from Blog for Azure appconfig connection.
        private AppSettings AppSettings { get; set; }

        HomePageBusinessComponent homePageBusinessComponent;
        AdminBusinessComponent adminBusinessComponent;
        LearningBusinessComponent learningBusinessComponent;

        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public AdminController(IConfiguration config, IOptions<AppSettings> settings)
        {
            this.configuration = config;
            AppSettings = settings.Value;
            homePageBusinessComponent = new HomePageBusinessComponent(config, AppSettings);
            adminBusinessComponent = new AdminBusinessComponent(config, AppSettings);
            learningBusinessComponent = new LearningBusinessComponent(config, AppSettings);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("AdminSignUp")]
        public IActionResult AdminSignUp()
        {
            return View();
        }

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            DashboardViewModel vm = new DashboardViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.DashboardHeader = "Admin Dashboard";
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Admin", ActionMethod = "Profile", DisplayName = "View Profile" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Admin", ActionMethod = "EditProfile", DisplayName = "Edit Profile" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Admin", ActionMethod = "Courses", DisplayName = "Visit Courses" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Admin", ActionMethod = "FindCourses", DisplayName = "Find Course" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Admin", ActionMethod = "CancelEnrollment", DisplayName = "Cancel Enrollment" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Admin", ActionMethod = "ChangePassword", DisplayName = "Change Password" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Home", ActionMethod = "SignOut", DisplayName = "Sign Out" });

            return PartialView("~/Views/Shared/CommonViews/_Dashboard.cshtml", vm);
        }
        [HttpPost]
        [Route("SaveAdminProfile")]
        public IActionResult SaveAdminProfile(AdminProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                updateModel.AdminDetails.UserId = updateModel.Users.UserId;
            }
            adminBusinessComponent.SaveAdminProfile(updateModel.AdminDetails);
            SetIsAdminProfileComplete(updateModel.Users.UserId, true);
            return RedirectToAction("Profile");
        }
        [Route("EditProfile")]
        public IActionResult EditProfile()
        {
            var userData = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (userData is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            AdminProfileViewModel vm = new AdminProfileViewModel();
            vm.Users = userData;
            var data = HttpContext.Session.GetSessionData<object>("isAdminProfileComplete");
            if (data != null && (bool)data)
            {
                vm.IsAdminProfileComplete = (bool)data;
                vm.AdminDetails = adminBusinessComponent.GetAdminDetails(vm.Users.UserId);
            }
            return View(vm);
        }

        [HttpPost]
        [Route("UpdateAdminProfile")]
        public IActionResult UpdateAdminProfile(AdminProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                //Setting the learner details value because during page get, value for LearnerDetails.UserId was not set
                updateModel.AdminDetails.UserId = updateModel.Users.UserId;
            }

            adminBusinessComponent.UpdateAdminProfile(updateModel.AdminDetails);
            return RedirectToAction("Profile");
        }

        [Route("EditPhoto")]
        public IActionResult EditPhoto()
        {
            var userData = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (userData is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            AdminProfileViewModel vm = new AdminProfileViewModel();
            vm.Users = userData;
            var data = HttpContext.Session.GetSessionData<object>("isAdminProfileComplete");
            if (data != null && (bool)data)
            {
                vm.IsAdminProfileComplete = (bool)data;
                vm.AdminDetails = adminBusinessComponent.GetAdminDetails(vm.Users.UserId);
            }
            return View(vm);
        }
        [HttpPost]
        [Route("EditPhotoSave")]
        public IActionResult EditPhotoSave(AdminProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                //Setting the learner details value because during page get, value for LearnerDetails.UserId was not set
                updateModel.AdminDetails.UserId = updateModel.Users.UserId;
            }
            adminBusinessComponent.UpdateAdminProfilePhoto(updateModel.AdminDetails);
            return RedirectToAction("Profile");
        }
        [Route("Profile")]
        public IActionResult Profile()
        {
            AdminProfileViewModel vm = new AdminProfileViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            var data = HttpContext.Session.GetSessionData<object>("isAdminProfileComplete");
            if (data != null && (bool)data)
            {
                vm.IsAdminProfileComplete = (bool)data;
                vm.AdminDetails = adminBusinessComponent.GetAdminDetails(vm.Users.UserId);
            }
            return View(vm);
        }
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            ChangePasswordViewModel vm = new ChangePasswordViewModel();
            vm.ControllerName = "Admin";
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            return PartialView("~/Views/Shared/CommonViews/_ChangePassword.cshtml", vm);
        }
        [Route("ValidatePassword")]
        public bool ValidatePassword(string Password)
        {
            var User = HttpContext.Session.GetSessionData<Users>("UserDetails");
            UserLoginDetails loginDetails = new UserLoginDetails();
            loginDetails.UserId = User.UserId;
            loginDetails.Password = Password;
            var isValid = adminBusinessComponent.ValidateUpdateUserPassword(loginDetails, "Validate");
            return isValid;
        }
        [Route("ChangePasswordSave")]
        public IActionResult ChangePasswordSave(ChangePasswordViewModel vm)
        {
            var isSaved = adminBusinessComponent.ValidateUpdateUserPassword(vm.UserLoginDetails, "Update");
            if (isSaved)
            {
                return RedirectToAction("Profile");
            }
            return RedirectToAction("ChangePassword");
        }
        [Route("Courses")]
        public IActionResult Courses(string search_Query)
        {
            FindCourseViewModel vm = new FindCourseViewModel();
            vm.EnrolledCourseList = new List<Course>();
            vm.AvailableCourseList = new List<Course>();
            vm.SearchedCourseList = new List<Course>();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            int LearnerId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("LearnerId"));
            if (search_Query is null)//Page is loaded Fresh
            {
                vm.IsSearcheClicked = false;
                vm.AvailableCourseList = adminBusinessComponent.GetAllCourses();
            }
            else
            {
                vm.IsSearcheClicked = true;
                vm.search_Query = search_Query;
                vm.SearchedCourseList = adminBusinessComponent.GetCourseSearchResults(search_Query);
            }
            return View(vm);
        }
        [Route("CourseRatings")]
        public IActionResult CourseRatings(int CourseId)
        {
            CourseRatingsPageViewModel vm = new CourseRatingsPageViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.courseRatingsList = learningBusinessComponent.GetCourseRatingComments(CourseId);
            return PartialView("_CourseRatings", vm);
        }
        [Route("ApproveCourses")]
        public IActionResult ApproveCourses()
        {
            CourseApprovalViewModel vm = new CourseApprovalViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.CourseListForAdminApproval = adminBusinessComponent.GetCoursesForAdminApproval();
            return View(vm);
        }
        [Route("ApproveCourse")]
        public IActionResult ApproveCourse(int CourseId)
        {
            var Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            var Success = adminBusinessComponent.ApproveCourse(CourseId, Users.UserId);
            return RedirectToAction("ApproveCourses");
        }


        [Route("DisableCourses")]
        public IActionResult DisableCourses()
        {
            CourseApprovalViewModel vm = new CourseApprovalViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.AllCourseList = adminBusinessComponent.GetAllCourses();
            return View(vm);
        }
        [Route("DisableCourse")]
        public IActionResult DisableCourse(int CourseId)
        {
            var Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            var Success = adminBusinessComponent.ApproveCourse(CourseId, Users.UserId);
            return RedirectToAction("ApproveCourses");
        }
        [Route("TutorialsPage")]
        public IActionResult TutorialsPage(int CourseId)
        {
            TutorialsPageViewModel vm = new TutorialsPageViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            //vm.TutorialFiles = adminBusinessComponent.GetTutorialFilesForCourse(CourseId);
            vm.TutorialFiles = adminBusinessComponent.GetTutorialFilesForCourseCloud(CourseId);
            vm.CurrentCourse = adminBusinessComponent.GetCourseDetails(CourseId);
            vm.ControllerName = "Admin";
            return PartialView("~/Views/Shared/CommonViews/_TutorialsPage.cshtml", vm);
        }
        public void setSessionAdminId(int UserId, bool getUpdatedData = false)
        {
            var data = HttpContext.Session.GetSessionData<object>("AdminId");

            if (data == null || getUpdatedData)
            {
                int teacherId = adminBusinessComponent.getAdminIdFromUserId(UserId);
                HttpContext.Session.SetSessionData("AdminId", teacherId);
            }
        }
        public void SetIsAdminProfileComplete(int UserId, bool getUpdatedData = false)
        {
            var data = HttpContext.Session.GetSessionData<object>("isAdminProfileComplete");

            if (data == null || getUpdatedData)
            {
                var v = adminBusinessComponent.IsAdminProfileComplete(UserId);
                HttpContext.Session.SetSessionData("isAdminProfileComplete", v);
            }
        }



    }
}