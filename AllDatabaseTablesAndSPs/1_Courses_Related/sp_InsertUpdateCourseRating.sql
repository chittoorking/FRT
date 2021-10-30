ALTER PROCEDURE sp_InsertUpdateCourseRating
@DMLOperation int,
@CourseId int,
@Comments VARCHAR(400),
@Rating int,
@CommentDateTime Datetime,
@RatedByUserId INT
AS
BEGIN
DECLARE @CourseRatingId int
IF(@DMLOperation = 2)
BEGIN
IF NOT EXISTS(SELECT 1 FROM CourseRating where CourseId = @CourseId and RatedByUserId=@RatedByUserId)
BEGIN
INSERT INTO CourseRating(CourseId,Comments,Rating,CommentDateTime,RatedByUserId)
VALUES (@CourseId,@Comments,@Rating,@CommentDateTime,@RatedByUserId)
SELECT @CourseRatingId = SCOPE_IDENTITY()
END
ELSE
BEGIN
SELECT @CourseRatingId = -1
END
END
ELSE IF(@DMLOperation = 3)
BEGIN
IF EXISTS(SELECT 1 FROM CourseRating where CourseId = @CourseId and RatedByUserId=@RatedByUserId)
BEGIN
UPDATE CourseRating
SET Comments=@Comments,
Rating = @Rating,
CommentDateTime = @CommentDateTime
where CourseId = @CourseId and RatedByUserId=@RatedByUserId
SELECT @CourseRatingId = CourseRatingId FROM CourseRating where CourseId = @CourseId and RatedByUserId=@RatedByUserId
END
ELSE
BEGIN
SELECT @CourseRatingId = -1
END
END
SELECT @CourseRatingId
END
