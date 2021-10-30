create procedure sp_CancelEnrollment
@CourseId int,
@LearnerId int
AS
BEGIN
Declare @Success bit = 0
IF(EXISTS(select EnrollmentID from Enrollment where CourseId=@CourseId and LearnerId=@LearnerId))
BEGIN
DELETE FROM Enrollment where CourseId=@CourseId and LearnerId=@LearnerId
SELECT @Success = 1
END
SELECT @Success
END