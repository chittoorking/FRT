CREATE PROCEDURE sp_GetCoursesForAdminApproval
AS
BEGIN
SELECT C.CourseId, CourseCode,CourseName,CourseDuration,CourseDescription,FileName,FileType,FileLocation,FileUploadDateTime FROM Course C
left join CourseImageFile CIF on C.CourseId=CIF.CourseId
WHERE C.IsDisabledCourse=0 AND C.IsActiveCourse <> 1
END