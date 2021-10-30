ALTER PROCEDURE sp_GetTeacherDetails
@UserId int
AS
BEGIN
select TeacherId,Currentprofession,SubjectsOfInterests,Experience,CurrentCity,CurrentState,HighestQualification,AboutMeDescription,
FileName,FileType,FileLocation,FileUploadDateTime
 from TeacherDetails TD 
left join UserImageFile UI on TD.UserId=UI.UserId
where TD.UserId=@UserId;
END