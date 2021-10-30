alter procedure sp_GetEnrolledCoursesOfLearner
@LearnerId int
AS
BEGIN
select C.CourseId, CourseCode,CourseName,CourseDuration,CourseDescription,FileName,FileType,FileLocation,FileUploadDateTime from Course C 
INNER JOIN Enrollment E ON C.CourseId=E.CourseId 
LEFT JOIN CourseImageFile CIF on C.CourseId=CIF.CourseId
WHERE C.IsActiveCourse=1 AND C.IsDisabledCourse=0 AND E.LearnerId=@LearnerId
END