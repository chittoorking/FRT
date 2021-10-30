ALTER PROCEDURE sp_IsRoleSpecificProfileComplete
@UserId int,
@RoleId int
AS
BEGIN
DECLARE @IsProfileUpdated BIT = 0
IF @RoleId = 2
BEGIN
SELECT @IsProfileUpdated = IsProfileUpdated from LearnerDetails WHERE Userid=@UserId
END
ELSE IF @RoleId=3
BEGIN
SELECT @IsProfileUpdated = IsProfileUpdated from TeacherDetails WHERE Userid=@UserId
END
ELSE IF @RoleId=4
BEGIN
SELECT @IsProfileUpdated = IsProfileUpdated from AdminDetails WHERE Userid=@UserId
END
SELECT @IsProfileUpdated as IsProfileUpdated
END