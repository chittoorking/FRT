ALTER PROCEDURE sp_EnrollCourse
@CourseId int,
@LearnerId int,
@EnrollmentDate DATETIME
AS
--Demo to execute the proc
--exec sp_EnrollCourse 1,3,'2019-01-27 23:08:52.000'
BEGIN
SET NOCOUNT ON 
DECLARE @EnrollmentID int
IF(NOT EXISTS(select EnrollmentID from Enrollment where CourseId=@CourseId and LearnerId=@LearnerId))
BEGIN
insert into Enrollment(CourseId,LearnerId,EnrollmentDate)
VALUES(@CourseId,@LearnerId,@EnrollmentDate)
SELECT @EnrollmentID = SCOPE_IDENTITY()
IF @EnrollmentID IS NULL
BEGIN
SELECT @EnrollmentID = -1;
END
END
ELSE
BEGIN
SELECT @EnrollmentID = EnrollmentID from Enrollment where CourseId=@CourseId and LearnerId=@LearnerId
END
SELECT @EnrollmentID as EnrollmentID
END
