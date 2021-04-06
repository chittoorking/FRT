using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ELearningPortalMSAzureV1.ConfigurationData;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.Resources;
using ELearningPortalMSAzureV1.ViewModels;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ELearningPortalMSAzureV1.BusinessComponent
{
    public class TeacherBusinessComponent
    {
        string connectionString;
        CommonBusinessFunctions commonBusinessFunctions;
        AppSettings AppSettings;
        public TeacherBusinessComponent(IConfiguration config, AppSettings AppSettings)
        {
            GetDBConnectionString objGetDBConnectionString = new GetDBConnectionString(config);
            connectionString = objGetDBConnectionString.connectionString;
            commonBusinessFunctions = new CommonBusinessFunctions(connectionString, AppSettings);
        }

        public List<Course> GetAllCoursesOfTeacher(int CourseTeacherId)
        {
            List<Course> courseList = new List<Course>();
            List<Course> ActiveCourseList = new List<Course>();
            List<Course> InactiveCourseList = new List<Course>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetCoursesOfTeacher, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseTeacherId, SqlDbType.Int).Value = CourseTeacherId;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    ActiveCourseList = commonBusinessFunctions.AssignCourseToModel(dset);
                    con.Close();
                }
                ActiveCourseList.ForEach(x => x.IsActiveCourse = true);
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetInactiveCoursesOfTeacher, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseTeacherId, SqlDbType.Int).Value = CourseTeacherId;
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = cmd;
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    InactiveCourseList = commonBusinessFunctions.AssignCourseToModel(dset);
                    con.Close();
                }
                InactiveCourseList.ForEach(x => x.IsActiveCourse = false);
            }
            courseList = InactiveCourseList.Union(ActiveCourseList).ToList();

            return courseList;
        }
        public int getTeacherIdFromUserId(int UserId)
        {
            return commonBusinessFunctions.getRoleBasedIdFromUserId(UserId, 3);
        }
        public int InsertUpdateCourse(Course data, int DMLOperation)
        {
            if (DMLOperation == Constants.DMLOperations.DMLInsert)
            {
                data.CourseId = -1;
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_InsertUpdateCourse, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = data.CourseId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseCode, SqlDbType.VarChar).Value = data.CourseCode;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseName, SqlDbType.VarChar).Value = data.CourseName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseDuration, SqlDbType.VarChar).Value = data.CourseDuration;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseDescription, SqlDbType.VarChar).Value = data.CourseDescription;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseTeacherId, SqlDbType.Int).Value = data.CourseTeacherId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CreateUpdateDate, SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    try
                    {
                        data.CourseId = Convert.ToInt32(cmd.ExecuteScalar());
                        if (data.CourseId > 0 && DMLOperation == Constants.DMLOperations.DMLInsert)
                        {
                            //For Updating photo a separate action method needs to be called that will update the photo
                            var UpladedFilePath = commonBusinessFunctions.UploadFilePhoto(data.CourseImageFile, Constants.Locations.FileUploadLocation);
                            UplaodCourseFilePhoto(data.CourseImageFile, UpladedFilePath, data.CourseId);
                        }
                    }
                    catch (Exception ex)
                    {
                        data.CourseId = -3;
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
                return data.CourseId;
            }
        }

        public bool IsTeacherProfileComplete(int UserId)
        {
            var isComplete = commonBusinessFunctions.IsRoleSpecificProfileComplete(UserId, 3);
            return isComplete;
        }
        public void SaveTeacherProfile(TeacherDetail td)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_UpdateTeacherProfile, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = td.UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Currentprofession, SqlDbType.VarChar).Value = td.Currentprofession;
                    cmd.Parameters.Add(Constants.DBConstants.Param_SubjectsOfInterests, SqlDbType.VarChar).Value = td.SubjectsOfInterests;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Experience, SqlDbType.VarChar).Value = td.Experience;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentCity, SqlDbType.VarChar).Value = td.CurrentCity;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentState, SqlDbType.VarChar).Value = td.CurrentState;
                    cmd.Parameters.Add(Constants.DBConstants.Param_HighestQualification, SqlDbType.VarChar).Value = td.HighestQualification;
                    cmd.Parameters.Add(Constants.DBConstants.Param_AboutMeDescription, SqlDbType.VarChar).Value = td.AboutMeDescription;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        var UpladedFilePath = commonBusinessFunctions.UploadFilePhoto(td.TeacherImageFile, Constants.Locations.UserImageUploadImageLocation);
                        UplaodTeacherPhoto(td.TeacherImageFile, UpladedFilePath, td.UserId);
                    }
                    catch (Exception ex)
                    {
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
            }
        }
        public void UplaodTeacherPhoto(IFormFile FileData, string UploadedPath, int UserId)
        {
            string TableName = Constants.DBTableNames.UserImageFile;
            int FileId = commonBusinessFunctions.InsertUpdateFilePhoto(FileData, TableName, Constants.DMLOperations.DMLInsert, UploadedPath, UserId);
        }
        public TeacherDetail GetTeacherDetails(int UserId)
        {
            TeacherDetail teacher = new TeacherDetail();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_GetTeacherDetails, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = UserId;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        teacher.TeacherId = Convert.ToInt32(reader[Constants.DBConstants.TeacherId]);
                        teacher.Currentprofession = reader[Constants.DBConstants.Currentprofession].ToString();
                        teacher.SubjectsOfInterests = reader[Constants.DBConstants.SubjectsOfInterests].ToString();
                        teacher.Experience = reader[Constants.DBConstants.Experience].ToString();
                        teacher.CurrentCity = reader[Constants.DBConstants.CurrentCity].ToString();
                        teacher.CurrentState = reader[Constants.DBConstants.CurrentState].ToString();
                        teacher.HighestQualification = reader[Constants.DBConstants.HighestQualification].ToString();
                        teacher.AboutMeDescription = reader[Constants.DBConstants.AboutMeDescription].ToString();
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
                            teacher.TeacherImageFile = commonBusinessFunctions.GetSetFilePhotoToIFormFile(imageFileModel);
                        }
                        else
                        {
                            teacher.TeacherImageFile = commonBusinessFunctions.SetDefaultPhotoToIFormFile(Constants.DefaultImageNames.MissingUserPhoto, Constants.Locations.UserImageUploadImageLocation);
                        }
                        //For getting and setting the image of the Teacher End
                    }
                }
            }
            return teacher;
        }
        public void UpdateTeacherProfile(TeacherDetail td)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_UpdateTeacherProfile, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_UserId, SqlDbType.Int).Value = td.UserId;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Currentprofession, SqlDbType.VarChar).Value = td.Currentprofession;
                    cmd.Parameters.Add(Constants.DBConstants.Param_SubjectsOfInterests, SqlDbType.VarChar).Value = td.SubjectsOfInterests;
                    cmd.Parameters.Add(Constants.DBConstants.Param_Experience, SqlDbType.VarChar).Value = td.Experience;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentCity, SqlDbType.VarChar).Value = td.CurrentCity;
                    cmd.Parameters.Add(Constants.DBConstants.Param_CurrentState, SqlDbType.VarChar).Value = td.CurrentState;
                    cmd.Parameters.Add(Constants.DBConstants.Param_HighestQualification, SqlDbType.VarChar).Value = td.HighestQualification;
                    cmd.Parameters.Add(Constants.DBConstants.Param_AboutMeDescription, SqlDbType.VarChar).Value = td.AboutMeDescription;
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
        public void UpdateTeacherProfilePhoto(TeacherDetail td)
        {
            var UpladedFilePath = commonBusinessFunctions.UploadFilePhoto(td.TeacherImageFile, Constants.Locations.UserImageUploadImageLocation);
            string TableName = Constants.DBTableNames.UserImageFile;
            int FileId = commonBusinessFunctions.InsertUpdateFilePhoto(td.TeacherImageFile, TableName, Constants.DMLOperations.DMLUpdate, UpladedFilePath, td.UserId);
        }
        public List<TutorialFile> GetTutorialFilesForCourse(int CourseId)
        {
            return commonBusinessFunctions.GetTutorialFilesForCourse(CourseId);
        }
        public List<TutorialFile> GetTutorialFilesForCourseCloud(int CourseId)
        {
            return commonBusinessFunctions.GetTutorialFilesForCourseCloud(CourseId);
        }
        public List<TutorialFile> GetTutorialFileNamesForCourseCloud(int CourseId)
        {
            return commonBusinessFunctions.GetTutorialFileNamesForCourseCloud(CourseId);
        }
        public int InsertUploadTutorial(TeacherUploadVideoViewModel vm)
        {
            vm.TutorialFile.FileId = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_InsertUploadTutorial, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_TutorialName, SqlDbType.VarChar).Value = vm.TutorialFile.TutorialName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileName, SqlDbType.VarChar).Value = vm.TutorialFile.FileName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileType, SqlDbType.VarChar).Value = vm.TutorialFile.FileType;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileLocation, SqlDbType.VarChar).Value = vm.TutorialFile.FileLocation;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileUploadDateTime, SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = vm.TutorialFile.CourseId;
                    try
                    {
                        vm.TutorialFile.FileId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        vm.TutorialFile.FileId = -3;
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
            }
            return vm.TutorialFile.FileId;
        }
        public int InsertUploadTutorialCloud(TeacherUploadVideoViewModel vm)
        {
            vm.TutorialFile.FileId = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Constants.DBConstants.StoredProcedure_InsertUploadTutorialCloud, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Constants.DBConstants.Param_TutorialName, SqlDbType.VarChar).Value = vm.TutorialFile.TutorialName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileName, SqlDbType.VarChar).Value = vm.TutorialFile.FileName;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileType, SqlDbType.VarChar).Value = vm.TutorialFile.FileType;
                    cmd.Parameters.Add(Constants.DBConstants.Param_FileUploadDateTime, SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    cmd.Parameters.Add(Constants.DBConstants.Param_CourseId, SqlDbType.Int).Value = vm.TutorialFile.CourseId;
                    try
                    {
                        vm.TutorialFile.FileId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        vm.TutorialFile.FileId = -3;
                        commonBusinessFunctions.LogException(ex);
                    }
                    con.Close();
                }
            }
            return vm.TutorialFile.FileId;
        }
        public string UploadTutorialFile(IFormFile FileData)
        {
            bool Uploaded = false;
            var path = string.Empty;
            if (FileData != null)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), Constants.Locations.FileUploadLocation, FileData.FileName);
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
        public string UploadTutorialFileToCloud(IFormFile FileData, AppSettings AppSettings)//1-- Working for Text document but not video files
        {
            //Here Azure Cloud will be connected and uplaoaded
            int returnValue = 0;
            string connectionString = AppSettings.StorageConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("acepcontainer");
            container.CreateIfNotExistsAsync();
            container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            CloudBlockBlob blockblobObj = container.GetBlockBlobReference(FileData.FileName);

            using (var filestream = FileData.OpenReadStream())
            {
                blockblobObj.UploadFromStreamAsync(filestream);
                returnValue = 1;
            }

            // blockblobObj.UploadFromFileAsync(@"C:\Subha_Deb_497290\Study\Dot_Net_Study\8_Dot_Net_Core\Resources\"+FileData.FileName); //UploadFromFileAsynch was working.
            return String.Empty;
        }
        public string UploadTutorialFileToCloud2(IFormFile FileData, AppSettings AppSettings)//1-- Working for Text document but not video files
        {
            //Here Azure Cloud will be connected and uplaoaded
            int returnValue = 0;
            string connectionString = AppSettings.StorageConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("myblobcontainer");
            container.CreateIfNotExistsAsync();
            container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            CloudBlockBlob blockblobObj = container.GetBlockBlobReference(FileData.FileName);

            using (var filestream = FileData.OpenReadStream())
            {
                blockblobObj.UploadFromStreamAsync(filestream);
                returnValue = 1;
            }

            // blockblobObj.UploadFromFileAsync(@"C:\Subha_Deb_497290\Study\Dot_Net_Study\8_Dot_Net_Core\Resources\"+FileData.FileName); //UploadFromFileAsynch was working.
            return String.Empty;
        }
        public int UploadTutorialFileToCloudMain(IFormFile FileData, AppSettings AppSettings)//1-- Working for Text document but not video files
        {
            //Here Azure Cloud will be connected and uplaoaded
            int RetValue = 0;
            try
            {
                string connectionString = AppSettings.StorageConnectionString;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(AppSettings.AzureStorageAccountContainer);
                container.CreateIfNotExistsAsync();
                container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                CloudBlockBlob blockblobObj = container.GetBlockBlobReference(FileData.FileName);
                blockblobObj.UploadFromFileAsync(Constants.Locations.TutorialFileToUploadLocation + FileData.FileName); //UploadFromFileAsynch was working
                RetValue = 1;
            }
            catch
            {
                RetValue = -1;
            }
            return RetValue;
        }
        public void UplaodCourseFilePhoto(IFormFile FileData, string UploadedPath, int CourseId)
        {
            string TableName = Constants.DBTableNames.CourseImageFile;
            int FileId = commonBusinessFunctions.InsertUpdateFilePhoto(FileData, TableName, Constants.DMLOperations.DMLInsert, UploadedPath, CourseId);
        }
        public Course GetCourseDetails(int CourseId)
        {
            return commonBusinessFunctions.GetCourseDetails(CourseId);
        }
        public bool ValidateUpdateUserPassword(UserLoginDetails userlogin, string Operation)
        {
            return commonBusinessFunctions.ValidateUpdateUserPassword(userlogin, Operation);
        }
        public List<CourseRating> GetCourseRatingComments(int CourseId)
        {
            return commonBusinessFunctions.GetCourseRatingComments(CourseId);
        }
        public string GetURLForTutorialFile(string FileName)
        {
            return commonBusinessFunctions.GetURLForTutorialFile(FileName);
        }
    }
}

