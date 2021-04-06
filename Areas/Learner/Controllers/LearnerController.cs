using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Area("Learner")]
    [Route("Learner")]
    public class LearnerController : Controller
    {
        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public readonly IConfiguration configuration;

        //Copied from Blog for Azure appconfig connection.
        private AppSettings AppSettings { get; set; }

        HomePageBusinessComponent homePageBusinessComponent;
        LearningBusinessComponent learningBusinessComponent;
        TeacherBusinessComponent teacherBusinessComponent;

        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public LearnerController(IConfiguration config, IOptions<AppSettings> settings)
        {
            this.configuration = config;
            AppSettings = settings.Value;
            homePageBusinessComponent = new HomePageBusinessComponent(config, AppSettings);
            learningBusinessComponent = new LearningBusinessComponent(config, AppSettings);
            teacherBusinessComponent = new TeacherBusinessComponent(config, AppSettings);
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Home")]
        public IActionResult Home()
        {
            LearnerHomePageViewModel vm = new LearnerHomePageViewModel();
            vm.CourseList = learningBusinessComponent.GetAllCourses();
            vm.Users  = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if(vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            setSessionLearnerId(vm.Users.UserId, true);
            SetIsLearnerProfileComplete(vm.Users.UserId);
            var isComplete = HttpContext.Session.GetSessionData<bool>("isLearnerProfileComplete");
            vm.IsLearnerProfileComplete = isComplete;
            return View(vm);
        }

        [Route("EnrollCourse")]
        public IActionResult EnrollCourse(int CourseId)
        {
            LearnerHomePageViewModel vm = new LearnerHomePageViewModel();
            vm.CourseList = learningBusinessComponent.GetAllCourses();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            var LearnerId = learningBusinessComponent.getLearnerIdFromUserId(vm.Users.UserId);
            int EnrollmentID = learningBusinessComponent.EnrollCourse(CourseId, LearnerId);
            vm.Enrollment = new Enrollment();
            if(EnrollmentID>0)
            {
                vm.Enrollment.CourseName = vm.CourseList.Where(x => x.CourseId == CourseId).FirstOrDefault().CourseName;
                vm.Enrollment.EnrollmentID = EnrollmentID;
                vm.Enrollment.CourseId = CourseId;
            }
            return View(vm);
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
            vm.DashboardHeader = "Learner Dashboard";
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Learner", ActionMethod = "Profile", DisplayName = "View Profile" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Learner", ActionMethod = "EditProfile", DisplayName = "Edit Profile" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Learner", ActionMethod = "Courses", DisplayName = "Visit Courses" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Learner", ActionMethod = "FindCourses", DisplayName = "Find Course" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Learner", ActionMethod = "CancelEnrollment", DisplayName = "Cancel Enrollment" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Learner", ActionMethod = "ChangePassword", DisplayName = "Change Password" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Home", ActionMethod = "SignOut", DisplayName = "Sign Out" });

            return PartialView("~/Views/Shared/CommonViews/_Dashboard.cshtml", vm);
        }
        [Route("Courses")]
        public IActionResult Courses()
        {
            LearnerCoursesPageViewModel vm = new LearnerCoursesPageViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            int LearnerId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("LearnerId"));
            vm.EnrolledCourseList = learningBusinessComponent.GetEnrolledCoursesOfLearner(LearnerId);
            vm.AvailableCourseList = learningBusinessComponent.GetAllCourses();
            var enrolledCourseIdLists = (from Course in vm.EnrolledCourseList select new { Course.CourseId }).ToArray();//only try it
            foreach (var v in enrolledCourseIdLists)
            {
                vm.AvailableCourseList.RemoveAll(x => x.CourseId == v.CourseId);
            }
            return View(vm);
        }
        [Route("CoursePage")]
        public IActionResult CoursePage(int CourseId)
        {
            CoursePageViewModel vm = new CoursePageViewModel();
            vm.Course = learningBusinessComponent.GetCourseDetails(CourseId);
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            int LearnerId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("LearnerId"));
            vm.EnrolledCourseList = learningBusinessComponent.GetEnrolledCoursesOfLearner(LearnerId);
            vm.AvailableCourseList = learningBusinessComponent.GetAllCourses();
            var enrolledCourseIdLists = (from Course in vm.EnrolledCourseList select new { Course.CourseId }).ToArray();//only try it
            foreach (var v in enrolledCourseIdLists)
            {
                vm.AvailableCourseList.RemoveAll(x => x.CourseId == v.CourseId);
            }
            return View(vm);
        }
        [Route("LearningPage")]
        public IActionResult LearningPage(int CourseId)
        {
            LearningPageViewModel vm = new LearningPageViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            int LearnerId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("LearnerId"));
            var EnrolledCourses = learningBusinessComponent.GetEnrolledCoursesOfLearner(LearnerId);
            vm.CurrentCourse = EnrolledCourses.SingleOrDefault(x => x.CourseId == CourseId);
            if(vm.CurrentCourse.CourseId > 0)
            {
                EnrolledCourses.Remove(vm.CurrentCourse);
                vm.OtherEnrolledCourses = EnrolledCourses;
            }
            //vm.TutorialFiles = teacherBusinessComponent.GetTutorialFilesForCourse(CourseId);
            vm.TutorialFiles = teacherBusinessComponent.GetTutorialFilesForCourseCloud(CourseId);
            return View(vm);
        }
        [Route("LearnerRatingForm")]
        public IActionResult LearnerRatingForm(int CourseId)
        {
            LearnerRatingFormViewModel vm = new LearnerRatingFormViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.CourseRating = learningBusinessComponent.GetUserCourseRating(CourseId,vm.Users.UserId);
            vm.CurrentCourse = learningBusinessComponent.GetCourseDetails(CourseId);
            return View(vm);
        }
        [Route("LearnerRatingFormSubmit")]
        public IActionResult LearnerRatingFormSubmit(LearnerRatingFormViewModel vm)
        {
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            int CourseRatingId = 0;
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            var dbCourseRating = learningBusinessComponent.GetUserCourseRating(vm.CourseRating.CourseId, vm.Users.UserId);
            if(string.IsNullOrEmpty(dbCourseRating.Comments))//For insert operation
            {
                CourseRatingId = learningBusinessComponent.InsertUpdateCourseRating(vm.CourseRating,Constants.DMLOperations.DMLInsert);
            }
            else//For update operation
            {
                CourseRatingId = learningBusinessComponent.InsertUpdateCourseRating(vm.CourseRating, Constants.DMLOperations.DMLUpdate);
            }
            return RedirectToAction("LearnerRatingForm", new { CourseId = vm.CourseRating.CourseId});
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            var userData = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (userData is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            LearnerProfileViewModel lpvm = new LearnerProfileViewModel();
            lpvm.Users = userData;
            var data = HttpContext.Session.GetSessionData<object>("isLearnerProfileComplete");
            if(data != null && (bool)data)
            {
                lpvm.IsLearnerProfileComplete = (bool)data;
                lpvm.LearnerDetails = learningBusinessComponent.GetLearnerDetails(lpvm.Users.UserId);
            }

            return View(lpvm);
        }
        [HttpPost]
        [Route("SaveLearnerProfile")]
        public IActionResult SaveLearnerProfile(LearnerProfileViewModel updateModel)
        {
            if(updateModel.Users.UserId>0)
            {
                //Setting the learner details value because during page get, value for LearnerDetails.UserId was not set
                updateModel.LearnerDetails.UserId = updateModel.Users.UserId;
            }

            learningBusinessComponent.SaveLearnerProfile(updateModel.LearnerDetails);
            SetIsLearnerProfileComplete(updateModel.LearnerDetails.UserId,true);
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
            LearnerProfileViewModel lpvm = new LearnerProfileViewModel();
            lpvm.Users = userData;
            var data = HttpContext.Session.GetSessionData<object>("isLearnerProfileComplete");
            if (data != null && (bool)data)
            {
                lpvm.IsLearnerProfileComplete = (bool)data;
                lpvm.LearnerDetails = learningBusinessComponent.GetLearnerDetails(lpvm.Users.UserId);
            }
            return View(lpvm);
        }

        [HttpPost]
        [Route("UpdateLearnerProfile")]
        public IActionResult UpdateLearnerProfile(LearnerProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                //Setting the learner details value because during page get, value for LearnerDetails.UserId was not set
                updateModel.LearnerDetails.UserId = updateModel.Users.UserId;
            }

            learningBusinessComponent.UpdateLearnerProfile(updateModel.LearnerDetails);
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
            LearnerProfileViewModel lpvm = new LearnerProfileViewModel();
            lpvm.Users = userData;
            var data = HttpContext.Session.GetSessionData<object>("isLearnerProfileComplete");
            if (data != null && (bool)data)
            {
                lpvm.IsLearnerProfileComplete = (bool)data;
                lpvm.LearnerDetails = learningBusinessComponent.GetLearnerDetails(lpvm.Users.UserId);
            }
            return View(lpvm);
        }
        [HttpPost]
        [Route("EditPhotoSave")]
        public IActionResult EditPhotoSave(LearnerProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                //Setting the learner details value because during page get, value for LearnerDetails.UserId was not set
                updateModel.LearnerDetails.UserId = updateModel.Users.UserId;
            }
            learningBusinessComponent.UpdateLearnerProfilePhoto(updateModel.LearnerDetails);
            return RedirectToAction("Profile");
        }
        public void SetIsLearnerProfileComplete(int UserId, bool getUpdatedData=false)
        {
            var data = HttpContext.Session.GetSessionData<object>("isLearnerProfileComplete");

            if (data == null || getUpdatedData)
            {
                var v = learningBusinessComponent.IsLearnerProfileComplete(UserId);
                HttpContext.Session.SetSessionData("isLearnerProfileComplete", v);
            }
        }
        private void setSessionLearnerId(int UserId, bool getUpdatedData = false)
        {
            var data = HttpContext.Session.GetSessionData<object>("TeacherId");
            if (data == null || getUpdatedData)
            {
                int learnerId = learningBusinessComponent.getLearnerIdFromUserId(UserId);
                HttpContext.Session.SetSessionData("LearnerId", learnerId);
            }
        }
        [Route("CancelEnrollment")]
        public IActionResult CancelEnrollment()
        {
            LearnerCoursesPageViewModel vm = new LearnerCoursesPageViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            int LearnerId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("LearnerId"));
            vm.EnrolledCourseList = learningBusinessComponent.GetEnrolledCoursesOfLearner(LearnerId);
            vm.AvailableCourseList = learningBusinessComponent.GetAllCourses();
            var enrolledCourseIdLists = (from Course in vm.EnrolledCourseList select new { Course.CourseId }).ToArray();//only try it
            foreach (var v in enrolledCourseIdLists)
            {
                vm.AvailableCourseList.RemoveAll(x => x.CourseId == v.CourseId);
            }
            return View(vm);
        }
        [Route("CancelEnrollmentClicked")]
        public int CancelEnrollmentClicked(int CourseId)
        {
            int Success = 0;
            int LearnerId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("LearnerId"));
            Success = 1; //learningBusinessComponent.CancellEnrollment(CourseId, LearnerId);
            return Success;
        }
        [Route("FindCourses")]
        public IActionResult FindCourses(string search_Query)
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
            if(search_Query is null)//Page is loaded Fresh
            {
                vm.IsSearcheClicked = false;
                vm.EnrolledCourseList = learningBusinessComponent.GetEnrolledCoursesOfLearner(LearnerId);
                vm.AvailableCourseList = learningBusinessComponent.GetAllCourses();
                var enrolledCourseIdLists = (from Course in vm.EnrolledCourseList select new { Course.CourseId }).ToArray();//only try it
                foreach (var v in enrolledCourseIdLists)
                {
                    vm.AvailableCourseList.RemoveAll(x => x.CourseId == v.CourseId);
                }
            }
            else
            {
                vm.IsSearcheClicked = true;
                vm.search_Query = search_Query;
                vm.SearchedCourseList = learningBusinessComponent.GetCourseSearchResults(search_Query);
            }

            
            return View(vm);
        }
        //SubhaDeb: 3/3/2019: Currently It(FindCoursesSearchResults) is of No use will delete it after a week
        [Route("FindCoursesSearchResults")]
        public IActionResult FindCoursesSearchResults(string search_Query)
        {
            FindCourseViewModel vm = new FindCourseViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.SearchedCourseList = learningBusinessComponent.GetCourseSearchResults(search_Query);

            int LearnerId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("LearnerId"));
            vm.EnrolledCourseList = learningBusinessComponent.GetEnrolledCoursesOfLearner(LearnerId);
            vm.AvailableCourseList = learningBusinessComponent.GetAllCourses();
            var enrolledCourseIdLists = (from Course in vm.EnrolledCourseList select new { Course.CourseId }).ToArray();
            foreach (var v in enrolledCourseIdLists)
            {
                vm.AvailableCourseList.RemoveAll(x => x.CourseId == v.CourseId);
            }
            return View(vm);
        }
        [Route("AboutUs")]
        public IActionResult AboutUs()
        {
            CommonViewModel vm = new CommonViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.ScreenContent1 = ScreenContents.AboutUsPage.Content1;
            return PartialView("_AboutUs", vm);
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
        [Route("Donate")]
        public IActionResult Donate()
        {
            CommonViewModel vm = new CommonViewModel();
            vm.ScreenContent1 = ScreenContents.DonatePage.Content1;
            return View(vm);
        }
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            ChangePasswordViewModel vm = new ChangePasswordViewModel();
            vm.ControllerName = "Learner";
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
            var isValid = learningBusinessComponent.ValidateUpdateUserPassword(loginDetails,"Validate");
            return isValid;
        }
        [Route("ChangePasswordSave")]
        public IActionResult ChangePasswordSave(ChangePasswordViewModel vm)
        {
            var isSaved = learningBusinessComponent.ValidateUpdateUserPassword(vm.UserLoginDetails, "Update");
            if(isSaved)
            {
                return RedirectToAction("Profile");
            }
            return RedirectToAction("ChangePassword");
        }

    }
}