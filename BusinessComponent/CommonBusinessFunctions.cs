using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.ConfigurationData;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ELearningPortalMSAzureV1.BusinessComponent
{
    public class CommonBusinessFunctions
    {
        string connectionString;
        AppSettings AppSettings;
        public CommonBusinessFunctions(string connectionString, AppSettings AppSettings)
        {
            this.connectionString = connectionString;
            this.AppSettings = AppSettings;
        }

        public int GetRoleIdFromRoleName(string RoleName)
        {
            if (RoleName == "Guest")
            {
                return Constants.Roles.RoleIdGuest;
            }
            else if (RoleName == "Learner")
            {
                return Constants.Roles.RoleIdLearner;
            }
            else if (RoleName == "Teacher")
            {
                return Constants.Roles.RoleIdTeacher;
            }
            else if (RoleName == "Admin")
            {
                return Constants.Roles.RoleIdAdmin;
            }
            else
            {
                return -1;
            }
        }
        public void LogException(Exception ex)
        {
            var ret = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string insertCommand = "insert into [ExceptionLogging] values(@ExceptionMsg,@ExceptionType ,@ExceptionSource,@Logdate)";
                using (SqlCommand cmd = new SqlCommand(insertCommand, con))
                {
                    con.Open();
                    cmd.Parameters.Add("@ExceptionMsg", SqlDbType.VarChar).Value = ex.Message.ToString();
                    cmd.Parameters.Add("@ExceptionType", SqlDbType.VarChar).Value = ex.GetType().Name.ToString();
                    cmd.Parameters.Add("@ExceptionSource", SqlDbType.NVarChar).Value = ex.StackTrace.ToString();
                    cmd.Parameters.Add("@Logdate", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    cmd.CommandType = CommandType.Text;
                    ret = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public int getRoleBasedIdFromUserId(int UserId, int RoleId)
        {
            var Id = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_getRoleBasedIdFromUserId, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_RoleId, SqlDbType.Int).Value = RoleId;
                    try
                    {
                        Id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Id = -3;
                        LogException(ex);
                    }
                }
            }
            return Id;
        }
        public bool IsRoleSpecificProfileComplete(int UserId, int RoleId)
        {
            var isComplete = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_IsRoleSpecificProfileComplete, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_RoleId, SqlDbType.Int).Value = RoleId;
                    try
                    {
                        isComplete = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                    }
                }
            }

            return isComplete;
        }
        public int InsertUpdateFilePhoto(IFormFile formFile, string TableName, int DMLOperation, string UploadedPath, int? OptionalIntParam1 = null, int? OptionalIntParam2 = null, string OptionalStrParam1 = null, string OptionalStrParam2 = null)
        {
            int FileId = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_InsertUpdateFilePhoto, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_TableName, SqlDbType.VarChar).Value = TableName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_DMLOperation, SqlDbType.Int).Value = DMLOperation;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileName, SqlDbType.VarChar).Value = formFile.FileName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileType, SqlDbType.VarChar).Value = formFile.ContentType;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileLocation, SqlDbType.VarChar).Value = UploadedPath;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileUploadDateTime, SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    cmd.Parameters.Add(Constants.DBConstants.Param_OptionalInt1, SqlDbType.Int).Value = OptionalIntParam1;
                    cmd.Parameters.Add(Constants.DBConstants.Param_OptionalInt2, SqlDbType.Int).Value = OptionalIntParam2;
                    cmd.Parameters.Add(Constants.DBConstants.Param_OptionalVarchar1, SqlDbType.Int).Value = OptionalStrParam2;
                    cmd.Parameters.Add(Constants.DBConstants.Param_OptionalVarchar2, SqlDbType.Int).Value = OptionalStrParam2;
                    try
                    {
                        FileId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        FileId = -3;
                        LogException(ex);
                    }
                    con.Close();
                }
            }
            return FileId;
        }
        public string UploadFilePhoto(IFormFile FileData,string UploadLocation)
        {
            bool Uploaded = false;
            var path = string.Empty;
            if (FileData != null)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), UploadLocation, FileData.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    FileData.CopyTo(stream);
                    stream.Flush();
                    Uploaded = true;
                }
            }
            if (Uploaded)
            {
                return path;
            }
            else
            {
                return string.Empty;
            }
        }
        public IFormFile GetSetFilePhotoToIFormFile(ImageFileModel imageFileModel)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.Locations.FileUploadLocation, imageFileModel.FileName);
            var stream = new FileStream(path, FileMode.OpenOrCreate);
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = imageFileModel.FileType
            };
            stream.Close();
            return file;
        }
        public IFormFile SetDefaultPhotoToIFormFile(string DefaultImageName, string Location)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), Location, DefaultImageName);
            var stream = new FileStream(path, FileMode.OpenOrCreate);
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = Constants.DefaultImageNames.ImageFileType
            };
            stream.Close();
            return file;
        }
        public List<Course> GetAllCourses()
        {
            List<Course> courseList = new List<Course>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetAllCourses, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    courseList = AssignCourseToModel(dset);
                    con.Close();
                }
            }
            return courseList;
        }
        public List<Course> AssignCourseToModel(DataSet dataSet)
        {
            List<Course> courseList = new List<Course>();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow courseRow in dataTable.AsEnumerable())
            {
                Course course = new Course();
                course.CourseId = courseRow.ItemArray[0] is null ? 0 : Convert.ToInt32(courseRow.ItemArray[0]);
                course.CourseCode = courseRow.ItemArray[1] is null ? string.Empty : courseRow.ItemArray[1].ToString();
                course.CourseName = courseRow.ItemArray[2] is null ? string.Empty : courseRow.ItemArray[2].ToString();
                course.CourseDuration = courseRow.ItemArray[3] is null ? string.Empty : courseRow.ItemArray[3].ToString();
                course.CourseDescription = courseRow.ItemArray[4] is null ? string.Empty : courseRow.ItemArray[4].ToString();
                var Rating = GetCourseAverageRating(course.CourseId);
                course.AverageCourseRating = Rating > 0 ? Rating : 0;
                //For getting and setting the image of the Course Start
                //FileName,FileType,FileLocation,FileUploadDateTime
                var imageFileModel = new ImageFileModel();
                imageFileModel.FileName = courseRow.ItemArray[5] is null ? string.Empty : courseRow.ItemArray[5].ToString();
                imageFileModel.FileType = courseRow.ItemArray[6] is null ? string.Empty : courseRow.ItemArray[6].ToString();
                imageFileModel.FileLocation = courseRow.ItemArray[7] is null ? string.Empty : courseRow.ItemArray[7].ToString();
                //Condtional query for DateTime was not working.
                if (courseRow.ItemArray[7] is DBNull)
                {
                    imageFileModel.FileUploadDateTime = null;
                }
                else
                {
                    imageFileModel.FileUploadDateTime = Convert.ToDateTime(courseRow.ItemArray[8]);
                }
                if (imageFileModel.FileName != string.Empty)
                {
                    course.CourseImageFile = GetSetFilePhotoToIFormFile(imageFileModel);
                }
                else
                {
                    course.CourseImageFile = SetDefaultPhotoToIFormFile(Constants.DefaultImageNames.NoImageCoursePhoto, Constants.Locations.FileUploadLocation);
                }
                //For getting and setting the image of the Course End
                courseList.Add(course);
            }
            return courseList;
        }
        public Course GetCourseDetails(int CourseId)
        {
            Course course = new Course();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetCourseDetails, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        course.CourseId = Convert.ToInt32(reader[Constants.DBConstants.CourseId]);
                        course.CourseCode = reader[Constants.DBConstants.CourseCode].ToString();
                        course.CourseName = reader[Constants.DBConstants.CourseName].ToString();
                        course.CourseDuration = reader[Constants.DBConstants.CourseDuration].ToString();
                        course.CourseDescription = reader[Constants.DBConstants.CourseDescription].ToString();
                        var Rating = GetCourseAverageRating(course.CourseId);
                        course.AverageCourseRating = Rating > 0 ? Rating : 0;
                        var imageFileModel = new ImageFileModel();
                        imageFileModel.FileName = reader[Constants.DBConstants.FileName].ToString();
                        imageFileModel.FileType = reader[Constants.DBConstants.FileType].ToString();
                        imageFileModel.FileLocation = reader[Constants.DBConstants.FileLocation].ToString();
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
                            course.CourseImageFile = GetSetFilePhotoToIFormFile(imageFileModel);
                        }
                        else
                        {
                            course.CourseImageFile = SetDefaultPhotoToIFormFile(Constants.DefaultImageNames.NoImageCoursePhoto, Constants.Locations.FileUploadLocation);
                        }

                    }
                }
            }
            return course;
        }
        public decimal GetCourseAverageRating(int CourseId)
        {
            decimal CourseRating = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetCourseAverageRating, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    try
                    {
                        CourseRating = Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        CourseRating = -3;
                        LogException(ex);
                    }
                }
            }
            return CourseRating;
        }
        public string GetNameInitialsFromFullName(string fullName)
        {
            StringBuilder sb = new StringBuilder();
            var names = fullName.Split(" ");
            foreach(var name in names)
            {
                sb.Append(name[0]);
            }
            return sb.ToString();
        }
        public List<CourseRating> GetCourseRatingComments(int CourseId)
        {
            List<CourseRating> ratingList = new List<CourseRating>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetCourseRatingComments, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    if (dset.Tables.Count > 0)
                    {
                        DataTable dataTable = dset.Tables[0];
                        foreach (DataRow ratingRow in dataTable.AsEnumerable())
                        {
                            CourseRating rating = new CourseRating();
                            rating.CourseRatingId = ratingRow.ItemArray[0] is DBNull ? 0 : Convert.ToInt32(ratingRow.ItemArray[0]);
                            rating.Comments = ratingRow.ItemArray[1] is DBNull ? string.Empty : ratingRow.ItemArray[1].ToString();
                            rating.Rating = ratingRow.ItemArray[2] is DBNull ? 0 : Convert.ToInt32(ratingRow.ItemArray[2]);
                            rating.CommentDateTime = ratingRow.ItemArray[3] is DBNull ? new DateTime() : Convert.ToDateTime(ratingRow.ItemArray[3]);
                            rating.RatedByUserFullName = ratingRow.ItemArray[4] is DBNull ? "Anonymous" : ratingRow.ItemArray[4].ToString();
                            rating.RatedByUserNameInitials = rating.RatedByUserFullName != string.Empty ? GetNameInitialsFromFullName(rating.RatedByUserFullName) : string.Empty;
                            ratingList.Add(rating);
                        }
                    }
                    con.Close();
                }
            }
            return ratingList;
        }
        public List<Course> GetCourseSearchResults(string search_Query)
        {
            
            //Separating the Search Query to Probable Search Indexes Start
            var SplittedWordsArray = search_Query.Split(" ");
            List<string> queryList = new List<string>();
            queryList.Add(string.Empty);
            queryList.Add(string.Empty);
            queryList.Add(string.Empty);
            queryList.Add(string.Empty);
            queryList.Add(string.Empty);
            for(int i = 0;i < SplittedWordsArray.Length; i++)
            {
                queryList[i] = SplittedWordsArray[i];
            }

            //If the seach query will be of less than four words the last two search index will be the end trimmed value of the first two
            if(SplittedWordsArray.Length <= 3 && SplittedWordsArray.Length >= 2)
            {
                queryList[3] = SplittedWordsArray[0].Remove(SplittedWordsArray[0].Length - 1);
                queryList[4] = SplittedWordsArray[1].Remove(SplittedWordsArray[1].Length - 1);
            }

            //Separating the Search Query to Probable Search Indexes End
            List<Course> courseList = new List<Course>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetCourseSearchResults, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_search_Query1, SqlDbType.VarChar).Value = queryList[0];
                    cmd.Parameters.Add(Constants.DBConstants.Param_search_Query2, SqlDbType.VarChar).Value = string.IsNullOrEmpty(queryList[1]) ? null : queryList[1];
                    cmd.Parameters.Add(Constants.DBConstants.Param_search_Query3, SqlDbType.VarChar).Value = string.IsNullOrEmpty(queryList[2]) ? null : queryList[2];
                    cmd.Parameters.Add(Constants.DBConstants.Param_search_Query4, SqlDbType.VarChar).Value = string.IsNullOrEmpty(queryList[3]) ? null : queryList[3];
                    cmd.Parameters.Add(Constants.DBConstants.Param_search_Query5, SqlDbType.VarChar).Value = string.IsNullOrEmpty(queryList[4]) ? null : queryList[4];
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    courseList = AssignCourseToModel(dset);
                    con.Close();
                }
            }
            return courseList;
        }
        public bool ValidateUpdateUserPassword(UserLoginDetails userlogin,String Operation)
        {
            bool IsValid = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_ValidateUpdateUserPassword, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Operation, SqlDbType.VarChar).Value = Operation;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.VarChar).Value = userlogin.UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Password, SqlDbType.VarChar).Value = userlogin.Password;
                    try
                    {
                        IsValid = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                    }
                    con.Close();
                }
            }
            return IsValid;
        }
        public List<TutorialFile> GetTutorialFilesForCourse(int CourseId)
        {
            List<TutorialFile> tutorialFiles = new List<TutorialFile>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetTutorialFilesForCourse, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    tutorialFiles = AssignTutorialFilesToModel(dset);
                    con.Close();
                }
            }
            return tutorialFiles;
        }
        public List<TutorialFile> GetTutorialFilesForCourseCloud(int CourseId)
        {
            List<TutorialFile> tutorialFiles = new List<TutorialFile>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetTutorialFilesForCourseCloud, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    tutorialFiles = AssignTutorialFilesToModelCloud(dset);
                    con.Close();
                }
            }
            return tutorialFiles;
        }
        private List<TutorialFile> AssignTutorialFilesToModel(DataSet dataSet)
        {
            List<TutorialFile> tutorialFiles = new List<TutorialFile>();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow tutorialRow in dataTable.AsEnumerable())
            {
                TutorialFile file = new TutorialFile();
                file.FileName = tutorialRow.ItemArray[0] is null ? string.Empty : tutorialRow.ItemArray[0].ToString();
                file.TutorialName = tutorialRow.ItemArray[1] is null ? string.Empty : tutorialRow.ItemArray[1].ToString();
                file.FileType = tutorialRow.ItemArray[2] is null ? string.Empty : tutorialRow.ItemArray[2].ToString();
                file.FileLocation = tutorialRow.ItemArray[3] is null ? string.Empty : tutorialRow.ItemArray[3].ToString();
                tutorialFiles.Add(file);
            }
            return tutorialFiles;
        }
        private List<TutorialFile> AssignTutorialFilesToModelCloud(DataSet dataSet)
        {
            List<TutorialFile> tutorialFiles = new List<TutorialFile>();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow tutorialRow in dataTable.AsEnumerable())
            {
                TutorialFile file = new TutorialFile();
                file.FileName = tutorialRow.ItemArray[0] is null ? string.Empty : tutorialRow.ItemArray[0].ToString();
                file.TutorialName = tutorialRow.ItemArray[1] is null ? string.Empty : tutorialRow.ItemArray[1].ToString();
                file.FileType = tutorialRow.ItemArray[2] is null ? string.Empty : tutorialRow.ItemArray[2].ToString();
                file.FileLocation = GetURLForTutorialFile(file.FileName);
                tutorialFiles.Add(file);
            }
            return tutorialFiles;
        }
        private List<TutorialFile> AssignTutorialFilesNamesToModelCloud(DataSet dataSet)
        {
            List<TutorialFile> tutorialFiles = new List<TutorialFile>();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow tutorialRow in dataTable.AsEnumerable())
            {
                TutorialFile file = new TutorialFile();
                file.FileName = tutorialRow.ItemArray[0] is null ? string.Empty : tutorialRow.ItemArray[0].ToString();
                file.TutorialName = tutorialRow.ItemArray[1] is null ? string.Empty : tutorialRow.ItemArray[1].ToString();
                file.FileType = tutorialRow.ItemArray[2] is null ? string.Empty : tutorialRow.ItemArray[2].ToString();
                tutorialFiles.Add(file);
            }
            return tutorialFiles;
        }
        public List<TutorialFile> GetTutorialFileNamesForCourseCloud(int CourseId)
        {
            List<TutorialFile> tutorialFiles = new List<TutorialFile>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetTutorialFilesForCourseCloud, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = CourseId;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    tutorialFiles = AssignTutorialFilesNamesToModelCloud(dset);
                    con.Close();
                }
            }
            return tutorialFiles;
        }
        public string GetURLForTutorialFile(string FileName)
        {
            string connectionString = AppSettings.StorageConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(AppSettings.AzureStorageAccountContainer);
            container.CreateIfNotExistsAsync();
            container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            var blockblobObj = container.GetBlockBlobReference(FileName);
            var shareAccessSig = blockblobObj.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1)
            });
            //blockblobObj.UploadFromFileAsync(Constants.Locations.TutorialFileToUploadLocation + FileName); //UploadFromFileAsynch was working
            var UrlToBePlayed = string.Format("{0}{1}", blockblobObj.Uri, shareAccessSig);
            return UrlToBePlayed;
        }

    }
}
