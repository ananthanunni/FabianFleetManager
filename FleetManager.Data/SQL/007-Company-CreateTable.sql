CREATE TABLE [Company]
(
	[Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
	[ShortName] NVARCHAR(20),
	[FullName] NVARCHAR(100),
	[Address1] NVARCHAR(100),
	[Address2] NVARCHAR(100),
	[Address3] NVARCHAR(100),
	[VAT] INT,
	[Email] NVARCHAR(20),
	[Person] NVARCHAR(100),
	[Contact] NVARCHAR(100),
	[Phone] VARCHAR(20),
	[CreatedOn] DATETIME2,
	[IsDeleted] BIT,
	[DeletedOn] DateTime2
)