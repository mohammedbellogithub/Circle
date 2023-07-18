IF OBJECT_ID('[sp_get_businessById]', 'P') IS NOT NULL
	Drop Procedure [sp_get_businessById]
GO
CREATE Procedure [dbo].[sp_get_businessById]
	@Id uniqueIdentifier = NULL,
	@userId uniqueIdentifier =NULL
As
select TOP 1 b.* from Business b left join AppUsers au on b.UserAccountId = au.Id
where b.Id = @Id 
and au.Id = @userId
and b.IsDeleted <> 1;
