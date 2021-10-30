
USE SubhaDB



create table TestTable
(id int,
col1 varchar(100),
col2 varchar(100))

create table TestTable2
(id int identity,
col varchar(30))

INSERT INTO TestTable2 VALUES('bob');
SELECT SCOPE_IDENTITY()

select * from TestTable2

insert into TestTable values(101,'Hey Your Data is there ','Hey Your Data is there')
insert into TestTable values(102,'EFG','EFGH')

select * from TestTable
select col1 from TestTable where id=101


DROP TABLE Students

CREATE TABLE Students(
StudentId INT PRIMARY KEY IDENTITY,
DateOfRegistration DATETIME,
UserName VARCHAR(30),
Password VARCHAR(30),
FirstName VARCHAR(30),
LastName VARCHAR(30),
)

INSERT INTO Students(DateOfRegistration,UserName,Password,FirstName,LastName) 
VALUES('01/23/2019','testuser1','testuser1','Test','User')


SELECT StudentId FROM Students where UserName='testuser1' and Password='testuser1'

select DateOfRegistration,UserName,FirstName,LastName from Students where StudentId=1

exec sp_GetCoursesOfTeacher 1

SELECT * FROM Users

--Drop table Course
SELECT * FROM Course
SELECT * FROM Enrollment
SELECT CourseId, CouseCode,CourseName,CourseDuration,CourseDescription FROM Course
INSERT INTO COURSE(CouseCode,CourseName,CourseDuration,CourseDescription,CourseTeacherId,CreateUpdateDate) 
VALUES('SS-110','Machine Learning','12 Hours','One of the trending couses of the market',1,'2/3/2019')

INSERT INTO COURSE(CouseCode,CourseName,CourseDuration,CourseDescription,CourseTeacherId,CreateUpdateDate) 
VALUES('EC-110','Sensors and Sensor Circuit Design','9 Hours','One of the trending electronic couses of the market',1,'2/3/2019')

SELECT * FROM TutorialFile
INSERT INTO TutorialFile(TutorialName,FileName,FileType,FileLocation,FileUploadDateTime) 
VALUES('Learn ABC','LoManliya1.mp4','video/mp4','C:\Subha_Deb_497290\Study\Dot_Net_Study\8_Dot_Net_Core\Projects\DotNetCore2_CRUD\DotNetCore2_CRUD\wwwroot/MyFiles\LoManliya1.mp4','2019-01-21 19:42:50.000')


INSERT INTO TutorialFile(TutorialName,FileName,FileType,FileLocation,FileUploadDateTime) 
VALUES('Learn DEF','LoManliya2.mp4','video/mp4','C:\Subha_Deb_497290\Study\Dot_Net_Study\8_Dot_Net_Core\Projects\DotNetCore2_CRUD\DotNetCore2_CRUD\wwwroot/MyFiles\LoManliya2.mp4','2019-01-27 19:42:50.000')

SELECT * FROM CourseTutorialFileMapping

INSERT INTO CourseTutorialFileMapping(CourseId,FileId,SequenceId) VALUES(1,1,1)

INSERT INTO CourseTutorialFileMapping(CourseId,FileId,SequenceId) VALUES(1,2,2)

select * from Course C inner join CourseTutorialFileMapping M on C.CourseId = M.CourseID
inner join TutorialFile on M.FileId=M.FileId


INSERT INTO LearnerDetails(Learner)

INSERT INTO LearnerDetails(Currentprofession,Experience,CurrentCity,CurrentState,HighestQualification)
VALUES('Developer','Years','Mumbai','Maharashtra','M Tech')


select * from LearnerDetails
Update LearnerDetails set Currentprofession='Enginner',Experience='5 Years',
CurrentCity='Dubai',CurrentState='Dubai',HighestQualification='MBA',IsProfileUpdated=1
Where LearnerId=1

select * from TeacherDetails

select 
S.name as [Schema], 
o.name as [Object], 
o.type_desc as [Object_Type], 
C.text as [Object_Definition]
from sys.all_objects O inner join sys.schemas S on O.schema_id = S.schema_id
inner join sys.syscomments C on O.object_id = C.id
where S.schema_id not in (3,4) -- avoid searching in sys and INFORMATION_SCHEMA schemas
and C.text like '%course%'---PUT THE STRING TO SEARCH HERE
order by [Schema]



select * from CourseRating
delete from CourseRating where CourseRatingId = 7


insert into CourseRating Values(1,'This was a great and thorough course that goes over many of the key aspects of AngularJS for people that want to learn the language, or even for those that want to strengthen their understanding',4,'1/04/2019',1)
insert into CourseRating Values(1,'Very clear and concise. The author takes his time in explaining confusing topics. Its easy to follow and informative',3,'2/22/2019',2)
insert into CourseRating Values(1,'Very well detailed explanation of topics so far. Very good coverage of topics',3,'2/14/2019',3)
insert into CourseRating Values(1,'Very clear, help to understand easily angularjs',2,'1/06/2019',4)
insert into CourseRating Values(1,'Tried learning outside,but never got concepts clearly.Here each concept is explained nicely with examples.Getting more confidence and able to develop my personal projects using angularJs',5,'2/13/2019',5)
insert into CourseRating Values(4,'Excelent course, covers many aspects of python. Would be great to put the python 2 videos appart.',2,'1/08/2019',1)

select * from CodingGuideLines


select convert(numeric(6,0),rand() * 899999) + 100000