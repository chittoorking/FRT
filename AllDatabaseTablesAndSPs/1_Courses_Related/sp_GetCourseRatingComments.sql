ALTER procedure sp_GetCourseRatingComments
@CourseId int
AS
BEGIN
IF EXISTS(select 1 from CourseRating where CourseId = @CourseId)
BEGIN
select R.CourseRatingId,R.Comments,R.Rating,R.CommentDateTime, (U.FirstName + ' ' + U.LastName) AS RatedByUser from CourseRating R
left join Users U on R.RatedByUserId = U.UserId
WHERE CourseId = @CourseId
END
END