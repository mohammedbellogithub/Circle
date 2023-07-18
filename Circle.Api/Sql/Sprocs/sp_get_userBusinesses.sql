IF OBJECT_ID('[sp_get_userBusinesses]', 'P') IS NOT NULL
	Drop Procedure [sp_get_userBusinesses]
GO
CREATE Procedure [dbo].[sp_get_userBusinesses]
	@userId uniqueIdentifier =NULL
As
select b.* from Business b left join AppUsers au on b.UserAccountId = au.Id
and au.Id = @userId
and b.IsDeleted <> 1;
