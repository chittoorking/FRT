ALTER PROCEDURE sp_GetCoursesOfTeacher
@CourseTeacherId INT
AS
BEGIN
SELECT C.CourseId, CourseCode,CourseName,CourseDuration,CourseDescription,FileName,FileType,FileLocation,FileUploadDateTime FROM Course C
LEFT JOIN CourseImageFile CIF on C.CourseId=CIF.CourseId
where C.IsActiveCourse=1 AND C.IsDisabledCourse=0 AND C.CourseTeacherId=@CourseTeacherId
END