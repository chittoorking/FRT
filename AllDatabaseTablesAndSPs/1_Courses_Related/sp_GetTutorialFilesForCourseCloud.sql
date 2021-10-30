ALTER procedure sp_GetTutorialFilesForCourseCloud
@CourseId int
AS
BEGIN
SELECT FileName,TutorialName,FileType from TutorialFileCloud TF inner join CourseTutorialFileMapping CM on TF.FileId=CM.FileId
where CourseId=@CourseId order by TF.FileName
END
