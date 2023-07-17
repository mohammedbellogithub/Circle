IF OBJECT_ID('[sp_get_user_profile]', 'P') IS NOT NULL
	Drop Procedure [sp_get_user_profile]
GO
CREATE Procedure [dbo].[sp_get_user_profile]
	@Id uniqueIdentifier = NULL
As
select TOP 1
	up.Id,
	ProfileName,
	Email,
	UserName,
	FirstName,
	LastName,
	MiddleName,
	PhoneNumber,
	CreatedOn,
	Bio,
	IsVerified,
	ProfilePictureUrl,
	BannerPictureUrl,
	au.Id as UserAccountId,
	Location
from UserProfile up left join AppUsers au on up.UserAccountId = au.Id
where au.IsDeleted <> 1 and au.Id=@Id;
