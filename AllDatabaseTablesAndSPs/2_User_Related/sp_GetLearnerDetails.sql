ALTER PROCEDURE sp_GetLearnerDetails
@UserId int
AS
--DEMO to Execute 
--exec sp_GetLearnerDetails 9
BEGIN
select LearnerId,Currentprofession,Experience,CurrentCity,CurrentState,HighestQualification,AboutMeDescription,FileName,FileType,FileLocation,FileUploadDateTime 
from LearnerDetails LD 
left join UserImageFile UI on LD.UserId=UI.UserId
where LD.UserId=@UserId;
END