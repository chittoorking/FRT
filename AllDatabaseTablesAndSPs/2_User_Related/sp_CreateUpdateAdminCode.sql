ALTER procedure sp_CreateUpdateAdminCode
@Email VARCHAR(30)
AS
--This procedure will Create or Update Admin Code for an Admin, mapped by EmailId.
--The Procedure can be executed simply by typing "EXEC sp_CreateUpdateAdminCode '<UserEmail>'"
--If the User will already have a code created, that code will be updated with a new random code on executing the procedure.
BEGIN
DECLARE @GeneratedCode VARCHAR(6) = convert(numeric(6,0),rand() * 899999) + 100000
DECLARE @CurrentDateTime DATETIME = getdate()
IF EXISTS(SELECT 1 FROM AdminCode WHERE Email=@Email)
BEGIN
UPDATE AdminCode 
SET AdminCodeNumber=@GeneratedCode,
CreateUpdateDate=@CurrentDateTime
WHERE Email=@Email
END
ELSE
BEGIN
INSERT INTO AdminCode(Email,AdminCodeNumber,CreateUpdateDate)
VALUES(@Email,@GeneratedCode,@CurrentDateTime)
END
SELECT Email,AdminCodeNumber from AdminCode WHERE Email=@Email
END


--SELECT * FROM AdminCode
--EXEC sp_CreateUpdateAdminCode 'abcd@z.com'