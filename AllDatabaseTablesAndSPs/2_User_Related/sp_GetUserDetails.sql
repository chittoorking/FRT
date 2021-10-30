CREATE PROCEDURE sp_GetUserDetails
@UserId int
AS
BEGIN
select DateOfRegistration,FirstName,LastName,RoleName from Users U inner join Roles R on U.RoleId=R.RoleId where UserId=@UserId;
END
