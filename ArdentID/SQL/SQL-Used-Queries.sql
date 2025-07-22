SELECT TOP (1000) [Id]
      ,[Email]
      ,[IsEmailConfirmed]
      ,[PasswordHash]
      ,[SecurityStamp]
      ,[GivenName]
      ,[FamilyName]
      ,[Status]
      ,[CreatedAtUtc]
      ,[UpdatedAtUtc]
      ,[LastLoginAtUtc]
      ,[AccessFailedCount]
      ,[LockoutEndUtc]
  FROM [ArdentID_Dev].[dbo].[Users]

-- Replace 'YourTableName' with your actual table name
DELETE FROM [ArdentID_Dev].[dbo].[Users];