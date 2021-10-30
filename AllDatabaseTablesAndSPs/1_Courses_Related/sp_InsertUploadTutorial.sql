create procedure sp_InsertUploadTutorial
@TutorialName varchar(50),
@FileName varchar(100),
@FileType varchar(40),
@FileLocation varchar(400),
@FileUploadDateTime datetime,
@CourseId int
AS
BEGIN
DECLARE @FileId int
DECLARE @MaxSequence int
insert into TutorialFile(TutorialName,FileName,FileType,FileLocation,FileUploadDateTime)
values(@TutorialName,@FileName,@FileType,@FileLocation,@FileUploadDateTime)
SELECT @FileId = SCOPE_IDENTITY()
IF @FileId > 0
BEGIN
select * from CourseTutorialFileMapping
SELECT @MaxSequence = MAX(SequenceId) from CourseTutorialFileMapping where CourseId=@CourseId
insert into CourseTutorialFileMapping(CourseId,FileId,SequenceId)
values(@CourseId,@FileId,(@MaxSequence+1))
END
SELECT @FileId
END
