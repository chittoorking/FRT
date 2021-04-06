using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.ConfigurationData;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.Resources;
using ELearningPortalMSAzureV1.ViewModels;
using Microsoft.Extensions.Configuration;

namespace ELearningPortalMSAzureV1.BusinessComponent
{
    //Use -3 for SQL Excepiton
    public class HomePageBusinessComponent
    {
        string connectionString;
        CommonBusinessFunctions commonBusinessFunctions;
        AdminBusinessComponent adminBusinessComponent;
        AppSettings AppSettings;
        public HomePageBusinessComponent(IConfiguration config, AppSettings AppSettings)
        {
            GetDBConnectionString objGetDBConnectionString = new GetDBConnectionString(config);
            connectionString = objGetDBConnectionString.connectionString;
            commonBusinessFunctions = new CommonBusinessFunctions(connectionString,AppSettings);
            adminBusinessComponent = new AdminBusinessComponent(config,AppSettings);
        }
        public UserLoginDetails ValidateLogin(UserLoginDetails userlogin)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_ValidateLogin, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_userLoginValue, SqlDbType.VarChar).Value = userlogin.UserLoginValue;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Password, SqlDbType.VarChar).Value = userlogin.Password;
                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            userlogin.UserId = Convert.ToInt32(reader[0]);
                            userlogin.LoginMessage = reader[1].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        userlogin.UserId = -3;
                        userlogin.LoginMessage = "An Unexpected Error has occured";
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
            }
            return userlogin;
        }
            
        public Users GetUserDetails(int userId)
        {
            Users user = new Users();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetUserDetails, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = userId;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        user.UserId = userId;
                        user.DateOfRegistration = Convert.ToDateTime(reader[Constants.DBConstants.DateOfRegistration]);
                        user.FirstName = reader[Constants.DBConstants.FirstName].ToString();
                        user.LastName = reader[Constants.DBConstants.LastName].ToString();
                        user.UserRole = reader[Constants.DBConstants.RoleName].ToString();
                        user.UserFullName = user.FirstName + " " + user.LastName;
                    }
                }                
            }
            return user;
        }

        public int SaveUserDetails(SignUpViewModel data)
        {
            int RetrunValue = -1;
            var UserRoleId = commonBusinessFunctions.GetRoleIdFromRoleName(data.UserRole);
            Users user = new Users();
            UserLoginDetails loginDetails = new UserLoginDetails();
            if(UserRoleId == 2)
            {
                user = data.UserLearner;
                loginDetails = data.UserLearnerLoginDetails;
            }
            else if(UserRoleId == 3)
            {
                user = data.UserTeacher;
                loginDetails = data.UserTeacherLoginDetails;
            }
            else if (UserRoleId == 4)
            {
                user = data.UserAdmin;
                loginDetails = data.UserAdminLoginDetails;
                var isValid = adminBusinessComponent.ValidateAdminSignUpCode(data.UserAdmin.Email, data.UserAdminLoginDetails.AdminCodeNumber);
                if(!isValid)
                {
                    return -1;
                }
            }
            user.UserRoleId = UserRoleId;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_InsertUser, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_DateOfRegistration, SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    cmd.Parameters.Add(Constants.DBConstants.Param_FirstName, SqlDbType.VarChar).Value = user.FirstName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_LastName, SqlDbType.VarChar).Value = user.LastName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Email, SqlDbType.VarChar).Value = user.Email;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Phone, SqlDbType.VarChar).Value = user.Phone;
                    cmd.Parameters.Add(Constants.DBConstants.Param_RoleId, SqlDbType.Int).Value = user.UserRoleId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Password, SqlDbType.VarChar).Value = loginDetails.Password;
                    cmd.Parameters.Add(Constants.DBConstants.Param_SecurityQuestion, SqlDbType.VarChar).Value = loginDetails.SecurityQuestion;
                    cmd.Parameters.Add(Constants.DBConstants.Param_SecurityAnswer, SqlDbType.VarChar).Value = loginDetails.SecurityAnswer;
                    try
                    {
                        user.UserId = Convert.ToInt32(cmd.ExecuteScalar());
                        loginDetails.UserId = user.UserId;
                        RetrunValue = user.UserId;
                    }
                    catch(Exception ex)
                    {
                        user.UserId = -3;
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
                return RetrunValue;
            }
        }
        public int IsUserIdentifierUnique(string Identifier)
        {
            var identifierType = string.Empty;
            if(Identifier.Contains("@"))
            {
                identifierType = "Email";
            }
            else
            {
                identifierType = "Phone";
            }
            int isUnique = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_IsUserIdentifierUnique, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_IdentifierType, SqlDbType.VarChar).Value = identifierType;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Identifier, SqlDbType.VarChar).Value = Identifier;
                    try
                    {
                        isUnique = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }
                }
            }
            return isUnique;
        }
        public List<Course> GetAllCourses()
        {
            return commonBusinessFunctions.GetAllCourses();
        }
        public Course GetCourseDetails(int CourseId)
        {
            return commonBusinessFunctions.GetCourseDetails(CourseId);
        }
        public List<Course> GetCourseSearchResults(string search_Query)
        {
            return commonBusinessFunctions.GetCourseSearchResults(search_Query);
        }
        public List<CourseRating> GetCourseRatingComments(int CourseId)
        {
            return commonBusinessFunctions.GetCourseRatingComments(CourseId);
        }
    }
}
