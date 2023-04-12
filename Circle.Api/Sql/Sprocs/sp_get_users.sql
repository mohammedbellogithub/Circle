IF OBJECT_ID('sp_get_users', 'P') IS NOT NULL
	Drop Procedure sp_get_users
GO

CREATE PROCEDURE [dbo].[sp_get_users]
	@Keyword VARCHAR(150) = Null,
	@RoleName VARCHAR(150) = Null,
	@PageIndex [INT] = 1,
	@PageSize [INT] = 10
AS

BEGIN
	WITH
		UserList
		AS
		(
			SELECT iu.Id, iu.LastName, iu.FirstName, iu.UserName Username, iu.PhoneNumber, iu.Activated, 
				iu.CreatedOn, iu.ModifiedOn, iu.Email, iu.IsPasswordDefault, iu.Department,
				iu.CreatedBy, iu.ModifiedBy, iu.LastLoginDate, iu.Gender,
				ir.Id RoleId, ir.[Name] RoleName
				FROM [AppUsers] iu
				LEFT JOIN [AppUserRoles] iur
				ON iu.Id = iur.UserId 
				LEFT JOIN [AppRoles] ir
				ON iur.RoleId = ir.Id
			WHERE (@Keyword IS NULL OR ((iu.LastName  like '%'+ @Keyword + '%')
			OR (iu.FirstName  LIKE '%'+ @Keyword + '%')
			OR (iu.PhoneNumber  LIKE '%'+ @Keyword + '%')
			OR (iu.UserName LIKE '%'+ @Keyword + '%')
			OR (iu.Email LIKE '%'+ @Keyword + '%') 
			OR (iu.Department LIKE '%'+ @Keyword + '%') 
			OR (ir.[Name]  = @RoleName) 
			OR (iu.StaffNo LIKE '%'+ @Keyword + '%'))) 
			AND iu.IsDeleted <> 1
			AND  ir.[Name] <> 'SYS_ADMIN'
		),
		Counts
		AS
		(
			SELECT Count(*) TotalCount
			FROM UserList
		)
		SELECT u.*,
			ROW_NUMBER() OVER(ORDER BY u.ModifiedOn DESC) as RowNo,
			c.TotalCount
		FROM UserList u, Counts c
		ORDER BY u.ModifiedOn DESC, u.CreatedOn DESC
		OFFSET ((@PageIndex -1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY
END