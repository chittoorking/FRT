ALTER PROCEDURE sp_UpdateAdminProfile
@UserId INT,
@CurrentCity VARCHAR(30),
@CurrentState Varchar(50),
@HighestQualification Varchar(100)
AS
BEGIN
IF EXISTS(SELECT AdminId from AdminDetails where UserId= @UserId)
BEGIN
UPDATE AdminDetails SET
CurrentCity = @CurrentCity,
CurrentState = @CurrentState,
HighestQualification = @HighestQualification,
IsProfileUpdated = 1
Where UserId=@UserId
END
END