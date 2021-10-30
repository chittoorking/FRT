ALTER PROCEDURE sp_GetCourseAverageRating
@CourseId int
AS
BEGIN
DECLARE @AverageRating Decimal(10,2)
IF EXISTS(SELECT 1 FROM CourseRating Where CourseId=@CourseId)
BEGIN
DECLARE @TotalRating Decimal(10,2)
DECLARE @Count INT
SELECT @TotalRating = CAST(SUM(Rating) AS DECIMAL(10,2)) from CourseRating Where CourseId=@CourseId
SELECT @Count = COUNT(Rating) FROM CourseRating where CourseId = @CourseId
SELECT @AverageRating = @TotalRating / @Count
END
ELSE
BEGIN
SELECT @AverageRating = -1.00
END
SELECT @AverageRating
END