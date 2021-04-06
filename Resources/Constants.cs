using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.Resources
{
    public static class Constants
    {
        public static class Roles
        {
            public static int RoleIdGuest = 1;
            public static int RoleIdLearner = 2;
            public static int RoleIdTeacher = 3;
            public static int RoleIdAdmin = 4;
        }
        public static class DMLOperations
        {
            public static int DMLSelect = 1;
            public static int DMLInsert = 2;
            public static int DMLUpdate = 3;
            public static int DMLDelete = 4;
        }
        public static class Locations
        {
            public static string FileUploadLocation = "wwwroot/UploadedFiles";
            public static string UserImageUploadImageLocation = "wwwroot/Images/UserImage";
            public static string TutorialFileToUploadLocation = @"C:\Subha_Deb_497290\Study\Dot_Net_Study\8_Dot_Net_Core\Resources\Upload Tutorials\";
        }
        public static class DBTableNames
        {
            public const string CourseImageFile = "CourseImageFile";
            public const string UserImageFile = "UserImageFile";
        }
        public static class DefaultImageNames
        {
            public const string NoImageCoursePhoto = "NoImageCoursePhoto.jpg";
            public const string MissingUserPhoto = "MissingPhoto.png";
            public const string ImageFileType = "image/jpeg";
            public const string CoursePhotoSrcPath = "/UploadedFiles/";
            public const string UserPhotoSrcPath = "/Images/UserImage/";
        }
        public static class DBConstants
        {
            public const string StoredProcedure_ValidateLogin = "sp_ValidateLogin";
            public const string Param_userLoginValue = "@userLoginValue";
            public const string Param_Password = "@password";
            public const string UserId = "UserId";
            public const string Message = "UserId";

            public const string StoredProcedure_GetUserDetails = "sp_GetUserDetails";
            public const string Param_UserId = "@UserId";

            public const string StoredProcedure_InsertUser = "sp_InsertUser";
            public const string Param_DateOfRegistration = "@DateOfRegistration";
            public const string Param_FirstName = "@FirstName";
            public const string Param_LastName = "@LastName";
            public const string Param_Email = "@Email";
            public const string Param_Phone = "@Phone";
            public const string Param_RoleId = "@RoleId";
            public const string Param_SecurityQuestion = "@SecurityQuestion";
            public const string Param_SecurityAnswer = "@SecurityAnswer";
            public const string DateOfRegistration = "DateOfRegistration";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string RoleName = "RoleName";

            public const string StoredProcedure_GetAllCourses = "sp_GetAllCourses";
            public const string FileName = "FileName";
            public const string FileType = "FileType";
            public const string FileLocation = "FileLocation";

            public const string StoredProcedure_EnrollCourse = "sp_EnrollCourse";
            public const string Param_CourseId = "@CourseId";
            public const string Param_LearnerId = "@LearnerId";
            public const string Param_EnrollmentDate = "@EnrollmentDate";

            public const string StoredProcedure_IsRoleSpecificProfileComplete = "sp_IsRoleSpecificProfileComplete";

            public const string StoredProcedure_UpdateLearnerProfile = "sp_UpdateLearnerProfile";
            public const string Param_Currentprofession = "@Currentprofession";
            public const string Param_Experience = "@Experience";
            public const string Param_CurrentCity = "@CurrentCity";
            public const string Param_CurrentState = "@CurrentState";
            public const string Param_HighestQualification = "@HighestQualification";
            public const string Param_AboutMeDescription = "@AboutMeDescription";

            public const string StoredProcedure_GetLearnerDetails = "sp_GetLearnerDetails";
            public const string LearnerId = "LearnerId";
            public const string Currentprofession = "Currentprofession";
            public const string Experience = "Experience";
            public const string CurrentCity = "CurrentCity";
            public const string CurrentState = "CurrentState";
            public const string HighestQualification = "HighestQualification";
            public const string AboutMeDescription = "AboutMeDescription";

            public const string StoredProcedure_IsUserIdentifierUnique = "sp_IsUserIdentifierUnique";
            public const string Param_IdentifierType = "@IdentifierType";
            public const string Param_Identifier = "@Identifier";

            public const string StoredProcedure_GetCoursesOfTeacher = "sp_GetCoursesOfTeacher";
            public const string Param_CourseTeacherId = "@CourseTeacherId";

            public const string StoredProcedure_getRoleBasedIdFromUserId = "sp_getRoleBasedIdFromUserId";

            public const string StoredProcedure_InsertUpdateCourse = "sp_InsertUpdateCourse";
            public const string Param_CourseCode = "@CourseCode";
            public const string Param_CourseName = "@CourseName";
            public const string Param_CourseDuration = "@CourseDuration";
            public const string Param_CourseDescription = "@CourseDescription";
            public const string Param_CreateUpdateDate = "@CreateUpdateDate";

            public const string StoredProcedure_GetTeacherDetails = "sp_GetTeacherDetails";
            public const string SubjectsOfInterests = "SubjectsOfInterests";
            public const string TeacherId = "TeacherId";

            public const string StoredProcedure_UpdateTeacherProfile = "sp_UpdateTeacherProfile";
            public const string Param_SubjectsOfInterests = "@SubjectsOfInterests";

            public const string StoredProcedure_GetTutorialFilesForCourse = "sp_GetTutorialFilesForCourse";

            public const string StoredProcedure_InsertUploadTutorial = "sp_InsertUploadTutorial";
            public const string Param_TutorialName = "@TutorialName";
            public const string Param_FileName = "@FileName";
            public const string Param_FileType = "@FileType";
            public const string Param_FileLocation = "@FileLocation";
            public const string Param_FileUploadDateTime = "@FileUploadDateTime";

            public const string StoredProcedure_GetEnrolledCoursesOfLearner = "sp_GetEnrolledCoursesOfLearner";


            public const string StoredProcedure_InsertUpdateFilePhoto = "sp_InsertUpdateFilePhoto";
            public const string Param_TableName = "@TableName";
            public const string Param_DMLOperation = "@DMLOperation";
            public const string Param_OptionalInt1 = "@OptionalIntParam1";
            public const string Param_OptionalInt2 = "@OptionalIntParam2";
            public const string Param_OptionalVarchar1 = "@OptionalVarcharParam1";
            public const string Param_OptionalVarchar2 = "@OptionalVarcharParam2";

            public const string StoredProcedure_GetCourseDetails = "sp_GetCourseDetails";
            public const string CourseId = "CourseId";
            public const string CourseCode = "CourseCode";
            public const string CourseName = "CourseName";
            public const string CourseDuration = "CourseDuration";
            public const string CourseDescription = "CourseDescription";
            public const string FileUploadDateTime = "FileUploadDateTime";

            public const string StoredProcedure_CancelEnrollment = "sp_CancelEnrollment";

            public const string StoredProcedure_GetCourseAverageRating = "sp_GetCourseAverageRating";

            public const string StoredProcedure_GetCourseRatingOfUser = "sp_GetCourseRatingOfUser";
            public const string CourseRatingId = "CourseRatingId";
            public const string Comments = "Comments";
            public const string Rating = "Rating";
            public const string CommentDateTime = "CommentDateTime";
            public const string RatedByUserId = "RatedByUserId";

            public const string StoredProcedure_InsertUpdateCourseRating = "sp_InsertUpdateCourseRating";
            public const string Param_Comments = "@Comments";
            public const string Param_Rating = "@Rating";
            public const string Param_CommentDateTime = "@CommentDateTime";
            public const string Param_RatedByUserId = "@RatedByUserId";

            public const string StoredProcedure_GetCourseRatingComments = "sp_GetCourseRatingComments";

            public const string StoredProcedure_GetCourseSearchResults = "sp_GetCourseSearchResults";
            public const string Param_search_Query1 = "@search_Query1";
            public const string Param_search_Query2 = "@search_Query2";
            public const string Param_search_Query3 = "@search_Query3";
            public const string Param_search_Query4 = "@search_Query4";
            public const string Param_search_Query5 = "@search_Query5";

            public const string StoredProcedure_ValidateUpdateUserPassword = "sp_ValidateUpdateUserPassword";
            public const string Param_Operation = "@Operation";

            public const string StoredProcedure_ValidateAdminSignUpCode = "sp_ValidateAdminSignUpCode";
            public const string Param_AdminCodeNumber = "@AdminCodeNumber";

            public const string StoredProcedure_GetCoursesForAdminApproval = "sp_GetCoursesForAdminApproval";
            public const string StoredProcedure_ApproveCourse = "sp_ApproveCourse";
            public const string StoredProcedure_DisableCourse = "sp_DisableCourse";

            public const string StoredProcedure_GetInactiveCoursesOfTeacher = "sp_GetInactiveCoursesOfTeacher";

            public const string StoredProcedure_GetAdminDetails = "sp_GetAdminDetails";
            public const string AdminId = "AdminId";

            public const string StoredProcedure_UpdateAdminProfile = "sp_UpdateAdminProfile";

            public const string StoredProcedure_InsertUploadTutorialCloud = "sp_InsertUploadTutorialCloud";
            public const string StoredProcedure_GetTutorialFilesForCourseCloud = "sp_GetTutorialFilesForCourseCloud";
        }
    }
}

