ALTER PROCEDURE sp_ValidateLogin
    @userLoginValue VARCHAR(30),
    @password varchar(30)
AS
BEGIN
--Return UserId as -2 for Incorrect UserName, UserId as -1 for Invalid Passord
SET NOCOUNT ON
DECLARE @UserId int
DECLARE @Message varchar(30)
SELECT @UserId = UserId FROM Users where Email=@userLoginValue or Phone=@userLoginValue
IF @UserId IS NOT NULL
BEGIN
SELECT @Message = FirstName + ' ' + LastName FROM Users where UserId=@UserId
IF NOT EXISTS(SELECT * FROM UserLoginDetails WHERE UserId = @UserId AND password = @password)
BEGIN
SELECT @UserId = -1
SELECT @Message = 'Invalid Password'
END
END
ELSE
  BEGIN
SELECT @UserId = -2
SELECT @Message = 'Invalid User'
  END  
END
SELECT @UserId as UserId,@Message as Message