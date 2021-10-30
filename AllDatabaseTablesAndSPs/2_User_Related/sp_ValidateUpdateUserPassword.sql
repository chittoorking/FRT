ALTER procedure sp_ValidateUpdateUserPassword
@Operation VARCHAR(20),
@UserId int,
@Password VARCHAR(30)
AS
BEGIN
---Return 1 for Valid and 0 for Invalid
---For Update also the same will be returned(1 If update is Successful)
DECLARE @IsValid BIT = 0
IF(@Operation='Validate')
BEGIN
IF EXISTS(SELECT 1 FROM UserLoginDetails WHERE UserId=@UserId AND Password=@Password)
BEGIN
SELECT @IsValid = 1
END
ELSE
BEGIN
SELECT @IsValid = 0
END
END
ELSE IF(@Operation='Update')
BEGIN
IF EXISTS(SELECT 1 FROM UserLoginDetails WHERE UserId=@UserId)
BEGIN
UPDATE UserLoginDetails
SET Password=@Password
WHERE UserId=@UserId
SELECT @IsValid = 1
END
END
SELECT @IsValid
END