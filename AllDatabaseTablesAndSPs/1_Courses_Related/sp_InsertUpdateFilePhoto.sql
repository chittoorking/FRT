ALTER procedure sp_InsertUpdateFilePhoto
@TableName varchar(20),
@DMLOperation int,
@FileName nvarchar(100),
@FileType nvarchar(40),
@FileLocation nvarchar(400),
@FileUploadDateTime nvarchar(100),
@OptionalIntParam1 varchar(50) = NULL,
@OptionalIntParam2 varchar(50) = NULL,
@OptionalVarcharParam1 varchar(50) = NULL,
@OptionalVarcharParam2 varchar(50) = NULL
AS
BEGIN
DECLARE @FileId int
IF(@TableName = 'CourseImageFile')
BEGIN
IF(@DMLOperation=2)--DMLInsert
BEGIN
--@OptionalIntParam1 should pass the CourseId
insert into CourseImageFile(CourseId,FileName,FileType,FileLocation,FileUploadDateTime)
values(@OptionalIntParam1,@FileName,@FileType,@FileLocation,@FileUploadDateTime)
SELECT @FileId = SCOPE_IDENTITY()
END
END
IF(@TableName = 'UserImageFile')
BEGIN
IF(@DMLOperation=2)--DMLInsert
BEGIN
--@OptionalIntParam1 should pass the UserId
insert into UserImageFile(UserId,FileName,FileType,FileLocation,FileUploadDateTime)
values(@OptionalIntParam1,@FileName,@FileType,@FileLocation,@FileUploadDateTime)
SELECT @FileId = SCOPE_IDENTITY()
END
ELSE IF(@DMLOperation=3)--DMLUpdate
BEGIN
IF EXISTS(SELECT ImageFileId from UserImageFile Where UserId=@OptionalIntParam1)
BEGIN
--@OptionalIntParam1 should pass the UserId
UPDATE UserImageFile SET
FileName = @FileName,
FileType = @FileType,
FileLocation = @FileLocation,
FileUploadDateTime = @FileUploadDateTime
Where UserId = @OptionalIntParam1
SELECT @FileId = ImageFileId from UserImageFile Where UserId=@OptionalIntParam1
END
END
END
SELECT @FileId
END