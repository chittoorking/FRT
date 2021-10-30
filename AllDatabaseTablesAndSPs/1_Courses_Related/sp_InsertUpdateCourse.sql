ALTER PROCEDURE sp_InsertUpdateCourse
@CourseId Int = 0, 
@CourseCode VARCHAR(10),
@CourseName VARCHAR(40),
@CourseDuration VARCHAR(30),
@CourseDescription VARCHAR(1000),
@CourseTeacherId INT,
@CreateUpdateDate DATETIME
AS
BEGIN
SET NOCOUNT ON 
IF @CourseId < 1
BEGIN
insert into Course(CourseCode,CourseName,CourseDuration,CourseDescription,CourseTeacherId,CreateUpdateDate,IsActiveCourse,IsDisabledCourse)
VALUES(@CourseCode,@CourseName,@CourseDuration,@CourseDescription,@CourseTeacherId,@CreateUpdateDate,0,0)
SELECT @CourseId = SCOPE_IDENTITY()
END
ELSE
BEGIN
UPDATE Course SET
CourseCode=@CourseCode,
CourseName = @CourseName,
CourseDuration=@CourseDuration,
CourseDescription=@CourseDescription,
CourseTeacherId=@CourseTeacherId,
CreateUpdateDate=@CreateUpdateDate
WHERE CourseId = @CourseId
END
END
SELECT @CourseId as CourseId