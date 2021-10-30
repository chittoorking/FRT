--select * from Users
--drop table Users
CREATE TABLE Users(
UserId INT PRIMARY KEY IDENTITY,
DateOfRegistration DATETIME,
FirstName VARCHAR(30),
LastName VARCHAR(30),
Email VARCHAR(30),
Phone VARCHAR(15),
RoleId INT
)

--drop table UserLoginDetails
--select * from UserLoginDetails
CREATE TABLE UserLoginDetails(
UserId INT PRIMARY KEY,
Password VARCHAR(30),
SecurityQuestion VARCHAR(100),
SecurityAnswer VARCHAR(100),
)

select * from Users U INNER JOIN UserLoginDetails D on U.UserId=D.UserId
--SELECT * from AdminCode
--DROP TABLE AdminCode
CREATE TABLE AdminCode(
AdminCodeId INT IDENTITY,
Email VARCHAR(30),
AdminCodeNumber VARCHAR(6),
CreateUpdateDate DATETIME
)

update Users set Phone='657568767' where UserId=7

--drop table LearnerDetails
--select * from LearnerDetails
CREATE TABLE LearnerDetails
(
LearnerId INT IDENTITY,
UserId int,
Currentprofession varchar(100),
Experience Varchar(100),
CurrentCity Varchar(50),
CurrentState Varchar(50),
HighestQualification Varchar(100),
IsProfileUpdated BIT,
AboutMeDescription VARCHAR(500)
)

--ALTER TABLE LearnerDetails Add AboutMeDescription VARCHAR(500) 


--select * from TeacherDetails
CREATE TABLE TeacherDetails
(
TeacherId INT IDENTITY,
UserId int,
Currentprofession varchar(100),
SubjectsOfInterests varchar(100),
Experience Varchar(100),
CurrentCity Varchar(50),
CurrentState Varchar(50),
HighestQualification Varchar(100),
IsProfileUpdated BIT,
AboutMeDescription VARCHAR(500) 
)

--ALTER TABLE TeacherDetails Add AboutMeDescription VARCHAR(500) 

--select * from AdminDetails
CREATE TABLE AdminDetails
(
AdminId INT IDENTITY,
UserId int,
CurrentCity Varchar(50),
CurrentState Varchar(50),
HighestQualification Varchar(100),
IsProfileUpdated BIT
)


--drop table UserImageFile
--select * from UserImageFile
CREATE TABLE UserImageFile
(
ImageFileId int IDENTITY NOT NULL,
UserId int,
FileName NVARCHAR(50) NOT NULL,
FileType NVARCHAR(20) NOT NULL,
FileLocation NVARCHAR(200) NOT NULL,
FileUploadDateTime DATETIME NOT NULL
)


CREATE TABLE Roles
(
RoleId INT PRIMARY KEY,
RoleName VARCHAR(30)
)

INSERT INTO Roles Values(1,'Guest')
INSERT INTO Roles Values(2,'Learner')
INSERT INTO Roles Values(3,'Teacher')
INSERT INTO Roles Values(4,'Admin')

--drop table Course
--select * from Course
CREATE TABLE Course
(
CourseId INT IDENTITY,
CourseCode VARCHAR(10),
CourseName VARCHAR(40),
CourseDuration VARCHAR(50),
CourseDescription VARCHAR(1000),
CourseTeacherId INT,
CreateUpdateDate DATETIME,
IsActiveCourse BIT,
IsDisabledCourse BIT
)

--ALTER TABLE Course ALTER COLUMN CourseName VARCHAR(80)
--SELECT * FROM Course where CourseId=5
--UPDATE Course SET IsActiveCourse=0 where CourseId=5
--UPDATE Course SET IsActiveCourse=0 where CourseId=3
--ALTER TABLE Course RENAME COLUMN CouseCode TO CourseCode EXEC sp_RENAME 'Course.CouseCode', 'CourseCode', 'COLUMN'
--ALTER TABLE Course Add IsActiveCourse BIT
--ALTER TABLE Course Add IsDisabledCourse BIT

select EnrollmentID from Enrollment where CourseId=5 and LearnerId=1
--select * from Enrollment
--drop table Enrollment
CREATE TABLE Enrollment
(
EnrollmentID INT IDENTITY,
CourseId INT,
LearnerId INT,
EnrollmentDate DATETIME,
)

--delete from TutorialFile where FileId=13
--drop table TutorialFile
select * from TutorialFile
CREATE TABLE TutorialFile
(
FileId int IDENTITY NOT NULL,
TutorialName Varchar(100) NOT NULL,
FileName NVARCHAR(100) NOT NULL,
FileType NVARCHAR(20) NOT NULL,
FileLocation NVARCHAR(200) NOT NULL,
FileUploadDateTime DATETIME NOT NULL
)

--ALTER TABLE TutorialFile ALTER COLUMN TutorialName VARCHAR(100)
--ALTER TABLE TutorialFile ALTER COLUMN FileName VARCHAR(100)

select * from TutorialFileCloud
CREATE TABLE TutorialFileCloud
(
FileId int IDENTITY NOT NULL,
TutorialName Varchar(100) NOT NULL,
FileName NVARCHAR(100) NOT NULL,
FileType NVARCHAR(20) NOT NULL,
FileUploadDateTime DATETIME NOT NULL
)

--ALTER TABLE TutorialFileCloud ALTER COLUMN TutorialName VARCHAR(100)
--ALTER TABLE TutorialFileCloud ALTER COLUMN FileName VARCHAR(100)


select LEN('Machine Learning5_Introduction to Deep Learning What Is Deep Learning.mp4')

--drop table CourseImageFile
--select * from CourseImageFile
CREATE TABLE CourseImageFile
(
ImageFileId int IDENTITY NOT NULL,
CourseId int,
FileName NVARCHAR(50) NOT NULL,
FileType NVARCHAR(20) NOT NULL,
FileLocation NVARCHAR(200) NOT NULL,
FileUploadDateTime DATETIME NOT NULL
)




-- drop table CourseTutorialFileMapping
--select * from CourseTutorialFileMapping
CREATE TABLE CourseTutorialFileMapping
(
CTMappingId INT IDENTITY,
CourseId INT,
FileId INT,
SequenceId INT
)

--DROP TABLE CourseRating
--Select * From CourseRating
--RatedUserId cannot be set as unique bcz One User can rate multiple courses
CREATE TABLE CourseRating
(
CourseRatingId INT Identity,
CourseId INT,
Comments VARCHAR(400),
Rating INT,
CommentDateTime DATETIME,
RatedByUserId INT
Constraint UQ_CourseRating_CourseId_RatedByUserId Unique(CourseId,RatedByUserId)
)


--select * from ExceptionLogging
--drop table ExceptionLogging
CREATE TABLE ExceptionLogging(  
    Logid BIGINT IDENTITY NOT NULL,  
    ExceptionMsg VARCHAR(200) NULL,  
    ExceptionType VARCHAR(100) NULL,  
    ExceptionSource NVARCHAR(max) NULL,  
    Logdate DATETIME NULL)




--Drop table CodingGuideLines
--Select * from CodingGuideLines
CREATE TABLE CodingGuideLines(
GuidelineId int Identity,
Title varchar(20),
Details VARCHAR(200))

insert into CodingGuideLines(Title,Details) values('Roles','[1:Guest][2:Learner][3:Teacher][4:Admin]')
insert into CodingGuideLines(Title,Details) values('ReturnValues','[1/2/..:Id];[-1:No Results];[-3:SQL Exceptoin]')
insert into CodingGuideLines(Title,Details) values('DMLOperations','[1:Select],[2:Insert],[3:Update],[4:Delete]')
insert into CodingGuideLines(Title,Details) values('FileLocations','[FileUploadLocation:wwwroot/UploadedFiles]')
