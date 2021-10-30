alter procedure sp_GetTutorialFilesForCourse
@CourseId int
AS
BEGIN
SELECT FileName,TutorialName,FileType,FileLocation from TutorialFile TF inner join CourseTutorialFileMapping CM on TF.FileId=CM.FileId
where CourseId=@CourseId order by CM.SequenceId
END