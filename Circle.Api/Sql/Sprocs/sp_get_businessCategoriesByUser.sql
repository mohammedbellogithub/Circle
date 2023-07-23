IF OBJECT_ID('[sp_get_businessCategoriesByUser]', 'P') IS NOT NULL
	Drop Procedure [sp_get_businessCategoriesByUser]
GO
CREATE Procedure [dbo].[sp_get_businessCategoriesByUser]
	@userId UNIQUEIDENTIFIER = Null,
	@businessId UNIQUEIDENTIFIER = NULL
As

select bc.* from BusinessCategory bc 
left join Business b on bc.BusinessId = b.Id
left join AppUsers au on b.UserAccountId = au.Id 

where au.Id = @userId
AND (@businessId IS NULL OR bc.BusinessId = @businessId)