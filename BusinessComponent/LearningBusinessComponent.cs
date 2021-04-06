using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.ConfigurationData;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.Resources;
using ELearningPortalMSAzureV1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;

namespace ELearningPortalMSAzureV1.BusinessComponent
{
    public class LearningBusinessComponent
    {

        string connectionString;
        CommonBusinessFunctions commonBusinessFunctions;
        AppSettings AppSettings;
        public LearningBusinessComponent(IConfiguration config, AppSettings AppSettings)
        {
            GetDBConnectionString objGetDBConnectionString = new GetDBConnectionString(config);
            connectionString = objGetDBConnectionString.connectionString;
            commonBusinessFunctions = new CommonBusinessFunctions(connectionString, AppSettings);
        }

        public List<Course> GetAllCourses()
        {
            return commonBusinessFunctions.GetAllCourses();
        }
        public int EnrollCourse(int CourseId, int LearnerId)
        {
            int EnrollmentID = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_EnrollCourse, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_LearnerId, SqlDbType.VarChar).Value = LearnerId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_EnrollmentDate, SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    try
                    {
                        EnrollmentID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EnrollmentID = -3;
                        commonBusinessFunctions.LogException(ex);
                    }

                    con.Close();
                }
            }

            return EnrollmentID;
        }
        public bool IsLearnerProfileComplete(int UserId)
        {
            var isComplete = commonBusinessFunctions.IsRoleSpecificProfileComplete(UserId, 2);
            return isComplete;
        }
        public void SaveLearnerProfile(LearnerDetails ld)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_UpdateLearnerProfile, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = ld.UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Currentprofession, SqlDbType.VarChar).Value = ld.Currentprofession;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Experience, SqlDbType.VarChar).Value = ld.Experience;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentCity, SqlDbType.VarChar).Value = ld.CurrentCity;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentState, SqlDbType.VarChar).Value = ld.CurrentState;
                    cmd.Parameters.Add(Constants.DBConstants.Param_HighestQualification, SqlDbType.VarChar).Value = ld.HighestQualification;
                    cmd.Parameters.Add(Constants.DBConstants.Param_AboutMeDescription, SqlDbType.VarChar).Value = ld.AboutMeDescription;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        var UpladedFilePath = commonBusinessFunctions.UploadFilePhoto(ld.LearnerImageFile, Constants.Locations.UserImageUploadImageLocation);
                        UplaodLearnerPhoto(ld.LearnerImageFile, UpladedFilePath, ld.UserId);
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }


            }
        }

        public void UpdateLearnerProfile(LearnerDetails ld)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_UpdateLearnerProfile, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = ld.UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Currentprofession, SqlDbType.VarChar).Value = ld.Currentprofession;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Experience, SqlDbType.VarChar).Value = ld.Experience;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentCity, SqlDbType.VarChar).Value = ld.CurrentCity;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentState, SqlDbType.VarChar).Value = ld.CurrentState;
                    cmd.Parameters.Add(Constants.DBConstants.Param_HighestQualification, SqlDbType.VarChar).Value = ld.HighestQualification;
                    cmd.Parameters.Add(Constants.DBConstants.Param_AboutMeDescription, SqlDbType.VarChar).Value = ld.AboutMeDescription;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
            }
        }


        public void UpdateLearnerProfilePhoto(LearnerDetails ld)
        {
            var UpladedFilePath = commonBusinessFunctions.UploadFilePhoto(ld.LearnerImageFile, Constants.Locations.UserImageUploadImageLocation);
            string TableName = Constants.DBTableNames.UserImageFile;
            int FileId = commonBusinessFunctions.InsertUpdateFilePhoto(ld.LearnerImageFile, TableName, Constants.DMLOperations.DMLUpdate, UpladedFilePath, ld.UserId);
        }


        public void UplaodLearnerPhoto(IFormFile FileData, string UploadedPath, int UserId)
        {
            string TableName = Constants.DBTableNames.UserImageFile;
            int FileId = commonBusinessFunctions.InsertUpdateFilePhoto(FileData, TableName, Constants.DMLOperations.DMLInsert, UploadedPath, UserId);
        }
        public LearnerDetails GetLearnerDetails(int UserId)
        {
            LearnerDetails learner = new LearnerDetails();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetLearnerDetails, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = UserId;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        learner.LearnerId = Convert.ToInt32(reader[Constants.DBConstants.LearnerId]);
                        learner.Currentprofession = reader[Constants.DBConstants.Currentprofession].ToString();
                        learner.Experience = reader[Constants.DBConstants.Experience].ToString();
                        learner.CurrentCity = reader[Constants.DBConstants.CurrentCity].ToString();
                        learner.CurrentState = reader[Constants.DBConstants.CurrentState].ToString();
                        learner.HighestQualification = reader[Constants.DBConstants.HighestQualification].ToString();
                        learner.AboutMeDescription = reader[Constants.DBConstants.AboutMeDescription].ToString();

                        //For getting and setting the image of the Learner Start
                        var imageFileModel = new ImageFileModel();
                        imageFileModel.FileName = reader[Constants.DBConstants.FileName] is null ? string.Empty : reader[Constants.DBConstants.FileName].ToString();
                        imageFileModel.FileType = reader[Constants.DBConstants.FileType] is null ? string.Empty : reader[Constants.DBConstants.FileType].ToString();
                        imageFileModel.FileLocation = reader[Constants.DBConstants.FileLocation] is null ? string.Empty : reader[Constants.DBConstants.FileLocation].ToString();
                        //Condtional query for DateTime was not working.
                        if (reader[Constants.DBConstants.FileUploadDateTime] is DBNull)
                        {
                            imageFileModel.FileUploadDateTime = null;
                        }
                        else
                        {
                            imageFileModel.FileUploadDateTime = Convert.ToDateTime(reader[Constants.DBConstants.FileUploadDateTime]);
                        }
                        if (imageFileModel.FileName != string.Empty)
                        {
                            learner.LearnerImageFile = commonBusinessFunctions.GetSetFilePhotoToIFormFile(imageFileModel);
                        }
                        else
                        {
                            learner.LearnerImageFile = commonBusinessFunctions.SetDefaultPhotoToIFormFile(Constants.DefaultImageNames.MissingUserPhoto, Constants.Locations.UserImageUploadImageLocation);
                        }
                        //For getting and setting the image of the Learner End
                    }
                }
            }
            return learner;
        }
        public List<Course> GetEnrolledCoursesOfLearner(int LearnerId)
        {
            List<Course> courseList = new List<Course>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetEnrolledCoursesOfLearner, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_LearnerId, SqlDbType.Int).Value = LearnerId;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    courseList = commonBusinessFunctions.AssignCourseToModel(dset);
                    con.Close();
                }
            }

            return courseList;
        }
        public int getLearnerIdFromUserId(int UserId)
        {
            return commonBusinessFunctions.getRoleBasedIdFromUserId(UserId, 2);
        }

        public Course GetCourseDetails(int CourseId)
        {
            return commonBusinessFunctions.GetCourseDetails(CourseId);
        }

        public int CancellEnrollment(int CourseId, int LearnerId)
        {
            int Success = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_CancelEnrollment, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_LearnerId, SqlDbType.VarChar).Value = LearnerId;
                    try
                    {
                        Success = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Success = -3;
                        commonBusinessFunctions.LogException(ex);
                    }

                    con.Close();
                }
            }

            return Success;
        }
        public CourseRating GetUserCourseRating(int CourseId, int UserId)
        {
            CourseRating rating = new CourseRating();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetCourseRatingOfUser, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = UserId;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        rating.CourseRatingId = Convert.ToInt32(reader[Constants.DBConstants.CourseRatingId]);
                        rating.CourseId = Convert.ToInt32(reader[Constants.DBConstants.CourseId]);
                        rating.Comments = reader[Constants.DBConstants.Comments].ToString();
                        rating.Rating = Convert.ToInt32(reader[Constants.DBConstants.Rating]);
                        rating.CommentDateTime = Convert.ToDateTime(reader[Constants.DBConstants.CommentDateTime]);
                        rating.RatedByUserId = Convert.ToInt32(reader[Constants.DBConstants.RatedByUserId]);
                    }
                }
            }
            return rating;
        }
        public int InsertUpdateCourseRating(CourseRating rating, int DMLOperation)
        {
            int CourseRatingId = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_InsertUpdateCourseRating, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_DMLOperation, SqlDbType.Int).Value = DMLOperation;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = rating.CourseId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Comments, SqlDbType.VarChar).Value = rating.Comments;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Rating, SqlDbType.Int).Value = rating.Rating;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CommentDateTime, SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    cmd.Parameters.Add(Constants.DBConstants.Param_RatedByUserId, SqlDbType.Int).Value = rating.RatedByUserId;
                    try
                    {
                        CourseRatingId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        CourseRatingId = -3;
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
                return CourseRatingId;
            }
        }
        public List<CourseRating> GetCourseRatingComments(int CourseId)
        {
            return commonBusinessFunctions.GetCourseRatingComments(CourseId);
        }
        public List<Course> GetCourseSearchResults(string search_Query)
        {
            return commonBusinessFunctions.GetCourseSearchResults(search_Query);
        }
        public bool ValidateUpdateUserPassword(UserLoginDetails userlogin, string Operation)
        {
            return commonBusinessFunctions.ValidateUpdateUserPassword(userlogin, Operation);
        }
        public string GetURLForTutorialFile(string FileName)
        {
            return commonBusinessFunctions.GetURLForTutorialFile(FileName);
        }
    }
}
