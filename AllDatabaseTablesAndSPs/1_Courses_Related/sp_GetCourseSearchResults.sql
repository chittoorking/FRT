ALTER PROCEDURE sp_GetCourseSearchResults
@search_Query1 VARCHAR(30),
@search_Query2 VARCHAR(30) = NULL,
@search_Query3 VARCHAR(30) = NULL,
@search_Query4 VARCHAR(30) = NULL,
@search_Query5 VARCHAR(30) = NULL
AS
BEGIN
SELECT C.CourseId, CourseCode,CourseName,CourseDuration,CourseDescription,FileName,FileType,FileLocation,FileUploadDateTime FROM Course C
left join CourseImageFile CIF on C.CourseId=CIF.CourseId
Where (C.IsActiveCourse=1 AND C.IsDisabledCourse=0) AND
(C.CourseName like '%'+@search_Query1+'%' OR C.CourseName like '%'+@search_Query2+'%' OR C.CourseName like '%'+@search_Query3+'%'
OR C.CourseName like '%'+@search_Query4+'%' OR C.CourseName like '%'+@search_Query5+'%')
END