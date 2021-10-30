ALTER PROCEDURE sp_ValidateAdminSignUpCode
@Email VARCHAR(30),
@AdminCodeNumber VARCHAR(6)
AS
BEGIN
DECLARE @IsValid BIT = 0
IF EXISTS(SELECT 1 FROM AdminCode WHERE Email=@Email AND AdminCodeNumber=@AdminCodeNumber)
BEGIN
SELECT @IsValid=1
END
ELSE
BEGIN
SELECT @IsValid=0
END
SELECT @IsValid
END