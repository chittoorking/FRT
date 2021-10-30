ALTER PROCEDURE sp_UpdateTeacherProfile
@UserId INT,
@Currentprofession VARCHAR(30),
@SubjectsOfInterests VARCHAR(100),
@Experience VARCHAR(30),
@CurrentCity VARCHAR(30),
@CurrentState Varchar(50),
@HighestQualification Varchar(100),
@AboutMeDescription Varchar(500)
AS
BEGIN
IF EXISTS(SELECT TeacherId from TeacherDetails where UserId= @UserId)
BEGIN
UPDATE TeacherDetails SET
Currentprofession=@Currentprofession,
SubjectsOfInterests=@SubjectsOfInterests,
Experience = @Experience,
CurrentCity = @CurrentCity,
CurrentState = @CurrentState,
HighestQualification = @HighestQualification,
IsProfileUpdated = 1,
AboutMeDescription=@AboutMeDescription
Where UserId=@UserId
END
END
