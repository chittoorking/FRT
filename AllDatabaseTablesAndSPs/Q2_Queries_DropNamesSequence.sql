---Queries All at one if drop table
------------------------------------------------For Course and Mapping Tables------------------------

drop table Course
drop table Enrollment
drop table TutorialFile
drop table CourseImageFile
drop table CourseTutorialFileMapping
drop table CourseRating

--All Course Fetch Procedures
--sp_GetAllCourses,sp_GetCourseDetails,sp_GetCoursesOfTeacher,sp_GetEnrolledCoursesOfLearner,sp_GetCourseSearchResults
--sp_GetCoursesForAdminApproval,sp_GetInactiveCoursesOfTeacherUserImageFile


--All Tables for Users(If date entry deleted from one table-deletion in other tables:)
--AdminDetails,Enrollment,LearnerDetails,Students,TeacherDetails,UserLoginDetails,Users

--All Procedures Using TutorialFile
--sp_GetTutorialFilesForCourse,sp_InsertUploadTutorial

select * from Users
select * from Course
select * from AdminDetails
select * from TeacherDetails
select * from CourseTutorialFileMapping--3-The Complete Machine Learning Course with Python
select * from TutorialFileCloud
select * from CourseRating

select * from TutorialFileCloud
--delete from TutorialFileCloud
--delete from Enrollment
--delete from CourseTutorialFileMapping
select * from CourseRating

--update TutorialFileCloud set TutorialName='Python4  Programming Tutorial- Variables.mp4' where FileId=6


exec sp_GetTutorialFilesForCourseCloud 3
exec sp_GetCourseRatingComments 1
select EnrollmentID from Enrollment where CourseId=1 and LearnerId=@LearnerId
select * from Enrollment
select * from TutorialFile

select R.CourseRatingId,R.Comments,R.Rating,R.CommentDateTime, (U.FirstName + ' ' + U.LastName) AS RatedByUser from CourseRating R
left join Users U on R.RatedByUserId = U.UserId

select * from sys.procedures order by name

--FEB16--Next updates on Course.
--1. Change the length of CourseName on Course table to 80 from 40.

--FEB24--Next Update on LearnerDetails/TeacherDetails
--1. Add AboutMeDescription column of size(500)

select * from Enrollment

sp_GetLearnerDetails--Done
sp_GetTeacherDetails--Done
sp_UpdateLearnerProfile--Done--Coded
sp_UpdateTeacherProfile----Done







select 
S.name as [Schema], 
o.name as [Object], 
o.type_desc as [Object_Type], 
C.text as [Object_Definition]
from sys.all_objects O inner join sys.schemas S on O.schema_id = S.schema_id
inner join sys.syscomments C on O.object_id = C.id
where S.schema_id not in (3,4) -- avoid searching in sys and INFORMATION_SCHEMA schemas
and C.text like '%TutorialFile%'---PUT THE STRING TO SEARCH HERE
order by [Schema]