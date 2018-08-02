BEGIN TRAN;

CREATE TABLE [CompanyGroup]
(
[Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
[GroupName] NVARCHAR(30),
[Description] NVARCHAR(100),
[Company_Id] BIGINT NOT NULL,
[IsDeleted] BIT,
[DeletedOn] DATETIME2
)

ALTER TABLE [CompanyGroup]
	ADD CONSTRAINT FK_CompanyGroup_Company FOREIGN KEY ([Company_Id]) REFERENCES [Company]([Id])

COMMIT;