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
    public class AdminBusinessComponent
    {
        string connectionString;
        CommonBusinessFunctions commonBusinessFunctions;
        AppSettings AppSettings;
        public AdminBusinessComponent(IConfiguration config, AppSettings AppSettings)
        {
            GetDBConnectionString objGetDBConnectionString = new GetDBConnectionString(config);
            connectionString = objGetDBConnectionString.connectionString;
            commonBusinessFunctions = new CommonBusinessFunctions(connectionString, AppSettings);
        }
        public bool ValidateAdminSignUpCode(string Email, string AdminCodeNumber)
        {
            bool isValid = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_ValidateAdminSignUpCode, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Email, SqlDbType.VarChar).Value = Email;
                    cmd.Parameters.Add(Constants.DBConstants.Param_AdminCodeNumber, SqlDbType.VarChar).Value = AdminCodeNumber;
                    try
                    {
                        isValid = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
            }
            return isValid;
        }
        public List<Course> GetAllCourses()
        {
            return commonBusinessFunctions.GetAllCourses();
        }
        public List<Course> GetCourseSearchResults(string search_Query)
        {
            return commonBusinessFunctions.GetCourseSearchResults(search_Query);
        }
        public List<Course> GetCoursesForAdminApproval()
        {
            List<Course> courseList = new List<Course>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetCoursesForAdminApproval, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
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
        public List<TutorialFile> GetTutorialFilesForCourse(int CourseId)
        {
            return commonBusinessFunctions.GetTutorialFilesForCourse(CourseId);
        }
        public List<TutorialFile> GetTutorialFilesForCourseCloud(int CourseId)
        {
            return commonBusinessFunctions.GetTutorialFilesForCourseCloud(CourseId);
        }
        public Course GetCourseDetails(int CourseId)
        {
            return commonBusinessFunctions.GetCourseDetails(CourseId);
        }
        public bool ApproveCourse(int CourseId, int UserId)
        {
            bool IsSuccess = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_ApproveCourse, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.VarChar).Value = UserId;
                    try
                    {
                        IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }

                    con.Close();
                }
            }
            return IsSuccess;
        }
        public bool DisableCourse(int CourseId, int UserId)
        {
            bool IsSuccess = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_DisableCourse, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.VarChar).Value = UserId;
                    try
                    {
                        IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }

                    con.Close();
                }
            }
            return IsSuccess;
        }
        public int getAdminIdFromUserId(int UserId)
        {
            return commonBusinessFunctions.getRoleBasedIdFromUserId(UserId, 4);
        }
        public bool IsAdminProfileComplete(int UserId)
        {
            var isComplete = commonBusinessFunctions.IsRoleSpecificProfileComplete(UserId, 4);
            return isComplete;
        }
        public AdminDetails GetAdminDetails(int UserId)
        {
            AdminDetails admin = new AdminDetails();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetAdminDetails, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = UserId;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        admin.AdminId = Convert.ToInt32(reader[Constants.DBConstants.AdminId]);
                        admin.CurrentCity = reader[Constants.DBConstants.CurrentCity].ToString();
                        admin.CurrentState = reader[Constants.DBConstants.CurrentState].ToString();
                        admin.HighestQualification = reader[Constants.DBConstants.HighestQualification].ToString();
                        //For getting and setting the image of the Teacher Start
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
                            admin.AdminImageFile = commonBusinessFunctions.GetSetFilePhotoToIFormFile(imageFileModel);
                        }
                        else
                        {
                            admin.AdminImageFile = commonBusinessFunctions.SetDefaultPhotoToIFormFile(Constants.DefaultImageNames.MissingUserPhoto, Constants.Locations.UserImageUploadImageLocation);
                        }
                        //For getting and setting the image of the Teacher End
                    }
                }
            }
            return admin;
        }
        public void SaveAdminProfile(AdminDetails ad)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_UpdateAdminProfile, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = ad.UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentCity, SqlDbType.VarChar).Value = ad.CurrentCity;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentState, SqlDbType.VarChar).Value = ad.CurrentState;
                    cmd.Parameters.Add(Constants.DBConstants.Param_HighestQualification, SqlDbType.VarChar).Value = ad.HighestQualification;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        var UpladedFilePath = commonBusinessFunctions.UploadFilePhoto(ad.AdminImageFile, Constants.Locations.UserImageUploadImageLocation);
                        UplaodAdminPhoto(ad.AdminImageFile, UpladedFilePath, ad.UserId);
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
            }
        }
        public void UplaodAdminPhoto(IFormFile FileData, string UploadedPath, int UserId)
        {
            string TableName = Constants.DBTableNames.UserImageFile;
            int FileId = commonBusinessFunctions.InsertUpdateFilePhoto(FileData, TableName, Constants.DMLOperations.DMLInsert, UploadedPath, UserId);
        }
        public void UpdateAdminProfile(AdminDetails ad)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_UpdateAdminProfile, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = ad.UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentCity, SqlDbType.VarChar).Value = ad.CurrentCity;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentState, SqlDbType.VarChar).Value = ad.CurrentState;
                    cmd.Parameters.Add(Constants.DBConstants.Param_HighestQualification, SqlDbType.VarChar).Value = ad.HighestQualification;
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
       
        public void UpdateAdminProfilePhoto(AdminDetails ad)
        {
            var UpladedFilePath = commonBusinessFunctions.UploadFilePhoto(ad.AdminImageFile, Constants.Locations.UserImageUploadImageLocation);
            string TableName = Constants.DBTableNames.UserImageFile;
            int FileId = commonBusinessFunctions.InsertUpdateFilePhoto(ad.AdminImageFile, TableName, Constants.DMLOperations.DMLUpdate, UpladedFilePath, ad.UserId);
        }
        public bool ValidateUpdateUserPassword(UserLoginDetails userlogin, string Operation)
        {
            return commonBusinessFunctions.ValidateUpdateUserPassword(userlogin, Operation);
        }

    }
}
