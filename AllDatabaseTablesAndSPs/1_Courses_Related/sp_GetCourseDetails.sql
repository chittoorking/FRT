CREATE PROCEDURE sp_GetCourseDetails
@CourseId int
AS
BEGIN
SELECT C.CourseId, CourseCode,CourseName,CourseDuration,CourseDescription,FileName,FileType,FileLocation,FileUploadDateTime FROM Course C
left join CourseImageFile CIF on C.CourseId=CIF.CourseId
WHERE C.CourseId=@CourseId
END