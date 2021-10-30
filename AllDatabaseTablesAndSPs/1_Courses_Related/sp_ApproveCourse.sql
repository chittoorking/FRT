ALTER procedure sp_ApproveCourse 
@CourseId int,
@UserId int
AS
BEGIN
DECLARE @IsSuccess BIT = 0
--First Check if the User Is Admin
IF EXISTS(SELECT 1 FROM Users WHERE UserId=@UserId AND RoleId=4)
BEGIN
IF NOT EXISTS(SELECT 1 FROM Course where CourseId=@CourseId AND IsActiveCourse=1)
BEGIN
UPDATE Course
SET IsActiveCourse=1
WHERE CourseId=@CourseId
SET @IsSuccess = 1
END
END
SELECT @IsSuccess
END