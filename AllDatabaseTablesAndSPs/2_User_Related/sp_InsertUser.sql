ALTER PROCEDURE sp_InsertUser
@DateOfRegistration DATETIME,
@FirstName VARCHAR(30),
@LastName VARCHAR(30),
@Email VARCHAR(30),
@Phone VARCHAR(15),
@RoleId int,
@Password VARCHAR(30),
@SecurityQuestion VARCHAR(100),
@SecurityAnswer	VARCHAR(100)
AS
--Demo to execute the proc
--exec sp_InsertUser '01/30/2018','TestProce','LNV','abc2@test','88676665777',2,'abcd','What?','yes'
BEGIN
SET NOCOUNT ON 
DECLARE @UserId int
insert into Users(DateOfRegistration,FirstName,LastName,Email,Phone,RoleId)
VALUES(@DateOfRegistration,@FirstName,@LastName,@Email,@Phone,@RoleId)
SELECT @UserId = SCOPE_IDENTITY()
IF @UserId IS NOT NULL
BEGIN
insert into UserLoginDetails(UserId,Password,SecurityQuestion,SecurityAnswer)
VALUES(@UserId,@Password,@SecurityQuestion,@SecurityAnswer)
END
ELSE
BEGIN
SELECT @UserId = -1;
END
IF (@UserId > 0 AND @RoleId=2)
BEGIN
INSERT INTO LearnerDetails(UserId) VALUES(@UserId)
END
ELSE IF(@UserId > 0 AND @RoleId=3)
BEGIN
INSERT INTO TeacherDetails(UserId) VALUES(@UserId)
END
ELSE IF(@UserId > 0 AND @RoleId=4)
BEGIN
INSERT INTO AdminDetails(UserId) VALUES(@UserId)
IF EXISTS(Select 1 FROM AdminCode WHERE Email=@Email)
BEGIN
DELETE FROM AdminCode WHERE Email=@Email
END
END
END
SELECT @UserId as UserId