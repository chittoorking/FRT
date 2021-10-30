ALTER PROCEDURE sp_GetAllCourses
AS
BEGIN
SELECT C.CourseId, CourseCode,CourseName,CourseDuration,CourseDescription,FileName,FileType,FileLocation,FileUploadDateTime FROM Course C
left join CourseImageFile CIF on C.CourseId=CIF.CourseId
WHERE C.IsActiveCourse=1 AND C.IsDisabledCourse=0
END