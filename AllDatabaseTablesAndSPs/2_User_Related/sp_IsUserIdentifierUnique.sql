ALTER procedure sp_IsUserIdentifierUnique
@IdentifierType varchar(15),
@Identifier Varchar(20)
AS
BEGIN
DECLARE @IsUnique BIT = 1
IF @identifierType='Email'
BEGIN
IF EXISTS(SELECT 1 FROM Users WHERE Email=@Identifier)
select @IsUnique = 0
END

IF @identifierType='Phone'
BEGIN
IF EXISTS(SELECT 1 FROM Users WHERE Phone=@Identifier)
select @IsUnique = 0
END
SELECT @IsUnique
END
