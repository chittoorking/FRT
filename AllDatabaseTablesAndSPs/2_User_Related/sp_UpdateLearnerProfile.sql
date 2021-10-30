ALTER PROCEDURE sp_UpdateLearnerProfile
@UserId INT,
@Currentprofession VARCHAR(30),
@Experience VARCHAR(30),
@CurrentCity VARCHAR(30),
@CurrentState Varchar(50),
@HighestQualification Varchar(100),
@AboutMeDescription Varchar(500)
AS
BEGIN
IF EXISTS(SELECT LearnerId from LearnerDetails where UserId= @UserId)
BEGIN
UPDATE LearnerDetails SET
Currentprofession=@Currentprofession,
Experience = @Experience,
CurrentCity = @CurrentCity,
CurrentState = @CurrentState,
HighestQualification = @HighestQualification,
IsProfileUpdated = 1,
AboutMeDescription = @AboutMeDescription
Where UserId=@UserId
END
END
