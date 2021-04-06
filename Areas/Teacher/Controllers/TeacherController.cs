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
using ELearningPortalMSAzureV1.Resources;
using Microsoft.AspNetCore.Http;

namespace ELearningPortalMSAzureV1.Controllers
{
    [Area("Teacher")]
    [Route("Teacher")]
    public class TeacherController : Controller
    {
        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public readonly IConfiguration configuration;

        //Copied from Blog for Azure appconfig connection.
        private AppSettings AppSettings { get; set; }

        HomePageBusinessComponent homePageBusinessComponent;
        TeacherBusinessComponent teacherBusinessComponent;
        LearningBusinessComponent learningBusinessComponent;

        //Copied from Muted Video(CRUD Operations in ASP.NET Core MVC)
        public TeacherController(IConfiguration config, IOptions<AppSettings> settings)
        {
            this.configuration = config;
            AppSettings = settings.Value;
            homePageBusinessComponent = new HomePageBusinessComponent(config, AppSettings);
            teacherBusinessComponent = new TeacherBusinessComponent(config, AppSettings);
            learningBusinessComponent = new LearningBusinessComponent(config, AppSettings);
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Home")]
        public IActionResult Home()
        {
            CommonViewModel vm = new CommonViewModel();
            vm.ScreenContent1 = ScreenContents.TeacherHomePage.Content1;
            vm.ScreenContent2 = ScreenContents.TeacherHomePage.Content2;
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            if (vm.Users.UserId>0)
            {
                SetIsTeacherProfileComplete(vm.Users.UserId);
                setSessionTeacherId(vm.Users.UserId, true);
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
            vm.DashboardHeader = "Teacher Dashboard";
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Teacher", ActionMethod = "Profile", DisplayName = "View Profile" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Teacher", ActionMethod = "EditProfile", DisplayName = "Edit Profile" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Teacher", ActionMethod = "AddTutorial", DisplayName = "Your Courses" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Teacher", ActionMethod = "TeacherAddCourse", DisplayName = "Add Course" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Teacher", ActionMethod = "TeacherUpdateCourse", DisplayName = "Update Course" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Teacher", ActionMethod = "TeacherUploadVideo", DisplayName = "Upload Tutorials" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Teacher", ActionMethod = "ChangePassword", DisplayName = "Change Password" });
            vm.ActionDetailsList.Add(new ActionDetails() { ControllerName = "Home", ActionMethod = "SignOut", DisplayName = "Sign Out" });

            return PartialView("~/Views/Shared/CommonViews/_Dashboard.cshtml", vm);
        }
        [Route("Profile")]
        public IActionResult Profile()
        {
            TeacherProfileViewModel vm = new TeacherProfileViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            var data = HttpContext.Session.GetSessionData<object>("isTeacherProfileComplete");
            if (data != null && (bool)data)
            {
                vm.IsTeacherProfileComplete = (bool)data;
                vm.TeacherDetails = teacherBusinessComponent.GetTeacherDetails(vm.Users.UserId);
            }
            return View(vm);
        }

        [HttpPost]
        [Route("SaveTeacherProfile")]
        public IActionResult SaveTeacherProfile(TeacherProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                updateModel.TeacherDetails.UserId = updateModel.Users.UserId;
            }
            teacherBusinessComponent.SaveTeacherProfile(updateModel.TeacherDetails);
            SetIsTeacherProfileComplete(updateModel.Users.UserId, true);
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
            TeacherProfileViewModel tpvm = new TeacherProfileViewModel();
            tpvm.Users = userData;
            var data = HttpContext.Session.GetSessionData<object>("isTeacherProfileComplete");
            if (data != null && (bool)data)
            {
                tpvm.IsTeacherProfileComplete = (bool)data;
                tpvm.TeacherDetails = teacherBusinessComponent.GetTeacherDetails(tpvm.Users.UserId);
            }
            return View(tpvm);
        }

        [HttpPost]
        [Route("UpdateTeacherProfile")]
        public IActionResult UpdateTeacherProfile(TeacherProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                //Setting the learner details value because during page get, value for LearnerDetails.UserId was not set
                updateModel.TeacherDetails.UserId = updateModel.Users.UserId;
            }

            teacherBusinessComponent.UpdateTeacherProfile(updateModel.TeacherDetails);
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
            TeacherProfileViewModel tpvm = new TeacherProfileViewModel();
            tpvm.Users = userData;
            var data = HttpContext.Session.GetSessionData<object>("isTeacherProfileComplete");
            if (data != null && (bool)data)
            {
                tpvm.IsTeacherProfileComplete = (bool)data;
                tpvm.TeacherDetails = teacherBusinessComponent.GetTeacherDetails(tpvm.Users.UserId);
            }
            return View(tpvm);
        }
        [HttpPost]
        [Route("EditPhotoSave")]
        public IActionResult EditPhotoSave(TeacherProfileViewModel updateModel)
        {
            if (updateModel.Users.UserId > 0)
            {
                //Setting the learner details value because during page get, value for LearnerDetails.UserId was not set
                updateModel.TeacherDetails.UserId = updateModel.Users.UserId;
            }
            teacherBusinessComponent.UpdateTeacherProfilePhoto(updateModel.TeacherDetails);
            return RedirectToAction("Profile");
        }

        [Route("AddTutorial")]
        public IActionResult AddTutorial()
        {
            AddTutorialViewModel vm = new AddTutorialViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.TeacherId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("TeacherId"));
            vm.CourseList = teacherBusinessComponent.GetAllCoursesOfTeacher(vm.TeacherId);
            return View(vm);
        }
        [Route("TeacherAddCourse")]
        public IActionResult TeacherAddCourse()
        {
            TeacherAddUpdateCourseViewModel vm = new TeacherAddUpdateCourseViewModel();
            vm.Users= HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.TeacherId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("TeacherId"));
            return View(vm);
        }
        [HttpPost]
        [Route("TeacherAddCourse")]
        public IActionResult TeacherAddCourse(TeacherAddUpdateCourseViewModel vm)
        {
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.TeacherId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("TeacherId"));
            vm.Course.CourseTeacherId = vm.TeacherId;
            var courseId = teacherBusinessComponent.InsertUpdateCourse(vm.Course, Constants.DMLOperations.DMLInsert);
            if (courseId > 0)
            {
                return RedirectToAction("AddTutorial");
            }
            else
            {
                return View(vm);
            }
        }

        [Route("TeacherUpdateCourse")]
        public IActionResult TeacherUpdateCourse(int CourseId)
        {
            TeacherAddUpdateCourseViewModel vm = new TeacherAddUpdateCourseViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.Course = teacherBusinessComponent.GetCourseDetails(CourseId);
            return View(vm);
        }


        [HttpPost]
        [Route("TeacherUpdateCourse")]
        public IActionResult TeacherUpdateCourse(TeacherAddUpdateCourseViewModel vm)
        {
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.TeacherId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("TeacherId"));
            vm.Course.CourseTeacherId = vm.TeacherId;
            var courseId = teacherBusinessComponent.InsertUpdateCourse(vm.Course, Constants.DMLOperations.DMLUpdate);
            if (courseId > 0)
            {
                return RedirectToAction("AddTutorial");
            }
            else
            {
                return View(vm);
            }
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
            //vm.TutorialFiles = teacherBusinessComponent.GetTutorialFilesForCourse(CourseId);
            vm.TutorialFiles = teacherBusinessComponent.GetTutorialFilesForCourseCloud(CourseId);
            vm.CurrentCourse = teacherBusinessComponent.GetCourseDetails(CourseId);
            vm.ControllerName = "Teacher";
            return PartialView("~/Views/Shared/CommonViews/_TutorialsPage.cshtml", vm);
        }

        [Route("TeacherUploadVideo")]
        public IActionResult TeacherUploadVideo()
        {
            TeacherUploadVideoViewModel vm = new TeacherUploadVideoViewModel();
            vm.Users = HttpContext.Session.GetSessionData<Users>("UserDetails");
            if (vm.Users is null)
            {
                return RedirectToAction("SignOut", "Home");
            }
            vm.TeacherId = Convert.ToInt32(HttpContext.Session.GetSessionData<object>("TeacherId"));
            vm.CourseList = teacherBusinessComponent.GetAllCoursesOfTeacher(vm.TeacherId);
            vm.TutorialFile = new TutorialFile();
            vm.CourseIdOnPost = HttpContext.Session.GetSessionData<string>("SessionCourseIdOnPost");
            vm.UploadedTutorialNameOnPost = HttpContext.Session.GetSessionData<string>("SessionUploadedTutorialNameOnPost");
            ClearUpladedVideoSession();
            return View(vm);
        }
        [HttpPost]
        [Route("TeacherUploadVideo")]
        public IActionResult TeacherUploadVideo(TeacherUploadVideoViewModel viewModel, IFormFile FileData)
        {
            if (FileData != null)
            {
                viewModel.TutorialFile.FileName = FileData.FileName;
                viewModel.TutorialFile.FileType = FileData.ContentType;
                // var UploadedFilePath = teacherBusinessComponent.UploadTutorialFile(FileData);
                int Uploaded = teacherBusinessComponent.UploadTutorialFileToCloudMain(FileData,AppSettings); ///-------
                //if (!string.IsNullOrEmpty(UploadedFilePath))
                //{
                //    viewModel.TutorialFile.FileLocation = UploadedFilePath;
                //    int fileId = teacherBusinessComponent.InsertUploadTutorial(viewModel);
                //}
                if (Uploaded > 0) //////----
                {
                    int fileId = teacherBusinessComponent.InsertUploadTutorialCloud(viewModel);
                    HttpContext.Session.SetSessionData("SessionCourseIdOnPost", viewModel.TutorialFile.CourseId.ToString());
                    HttpContext.Session.SetSessionData("SessionUploadedTutorialNameOnPost", viewModel.TutorialFile.TutorialName);
                }
            }
            return RedirectToAction("TeacherUploadVideo");
        }

        [Route("GetTutorialFilesForCourseId")]
        public List<TutorialFile> GetTutorialFilesForCourseId(int CourseId)
        {
            List<TutorialFile> tutorialFiles = new List<TutorialFile>();
            if (CourseId > 0)
            {
                //tutorialFiles = teacherBusinessComponent.GetTutorialFilesForCourse(CourseId);
                tutorialFiles = teacherBusinessComponent.GetTutorialFileNamesForCourseCloud(CourseId);
                //tutorialFiles.Add(new TutorialFile() { FileName = "Angular 2 Tutorial - 1 - Introduction.mp4",FileType= "video / mp4", TutorialName= "Angular Part 1 - Intro" });
            }
            return tutorialFiles;
        }

        [Route("GetCourseSelectPopup")]
        public IActionResult GetCourseSelectPopup()
        {
            var userData = HttpContext.Session.GetSessionData<Users>("UserDetails");
            return View(userData);
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
            return PartialView("_AboutUs",vm);
        }


        public void setSessionTeacherId(int UserId, bool getUpdatedData = false)
        {
            var data = HttpContext.Session.GetSessionData<object>("TeacherId");

            if (data == null || getUpdatedData)
            {
                int teacherId = teacherBusinessComponent.getTeacherIdFromUserId(UserId);
                HttpContext.Session.SetSessionData("TeacherId", teacherId);
            }
        }
        public void SetIsTeacherProfileComplete(int UserId, bool getUpdatedData = false)
        {
            var data = HttpContext.Session.GetSessionData<object>("isTeacherProfileComplete");

            if (data == null || getUpdatedData)
            {
                var v = teacherBusinessComponent.IsTeacherProfileComplete(UserId);
                HttpContext.Session.SetSessionData("isTeacherProfileComplete", v);
            }
        }
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            ChangePasswordViewModel vm = new ChangePasswordViewModel();
            vm.ControllerName = "Teacher";
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
            var isValid = teacherBusinessComponent.ValidateUpdateUserPassword(loginDetails, "Validate");
            return isValid;
        }
        [Route("ChangePasswordSave")]
        public IActionResult ChangePasswordSave(ChangePasswordViewModel vm)
        {
            var isSaved = teacherBusinessComponent.ValidateUpdateUserPassword(vm.UserLoginDetails, "Update");
            if (isSaved)
            {
                return RedirectToAction("Profile");
            }
            return RedirectToAction("ChangePassword");
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
            vm.courseRatingsList = teacherBusinessComponent.GetCourseRatingComments(CourseId);
            return PartialView("_CourseRatings", vm);
        }
        private void ClearUpladedVideoSession()
        {
            HttpContext.Session.SetSessionData("SessionCourseIdOnPost", null);
            HttpContext.Session.SetSessionData("SessionUploadedTutorialNameOnPost", null);
        }

    }
}