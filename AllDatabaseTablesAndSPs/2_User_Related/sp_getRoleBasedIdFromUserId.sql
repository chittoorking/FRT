ALTER PROCEDURE sp_getRoleBasedIdFromUserId
@UserId int,
@RoleId int
AS
BEGIN
DECLARE @id INT = -1
IF @RoleId=2
BEGIN
select @id = LearnerId from LearnerDetails where UserId=@UserId
END
ELSE IF @RoleId = 3 
BEGIN
select @id = TeacherId from TeacherDetails where UserId=@UserId
END
ELSE IF @RoleId = 4 
BEGIN
select @id = AdminId from AdminDetails where UserId=@UserId
END
SELECT @id
END