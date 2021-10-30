CREATE PROCEDURE sp_GetAdminDetails
@UserId int
AS
BEGIN
select AdminId,CurrentCity,CurrentState,HighestQualification,FileName,FileType,FileLocation,FileUploadDateTime
 from AdminDetails TD 
left join UserImageFile UI on TD.UserId=UI.UserId
where TD.UserId=@UserId;
END