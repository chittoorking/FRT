CREATE PROCEDURE sp_GetCourseRatingOfUser
@CourseId INT,
@UserId INT
AS
BEGIN
SELECT CourseRatingId,CourseId,Comments,Rating,CommentDateTime,RatedByUserId FROM CourseRating
Where CourseId=@CourseId and RatedByUserId=@UserId
END
