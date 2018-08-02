BEGIN TRAN;

CREATE TABLE [CompanyGroupCompanyUser]
(
	[Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
	[CompanyGroup_Id] BIGINT NOT NULL,
	[CompanyUser_Id] BIGINT NOT NULL,
	[IsDeleted] BIT,
	[DeletedOn] DATETIME2
)

ALTER TABLE [CompanyGroupCompanyUser]
	ADD CONSTRAINT FK_CompanyGroupCompanyUser_CompanyGroup FOREIGN KEY ([CompanyGroup_Id]) REFERENCES [CompanyGroup]([Id])

ALTER TABLE [CompanyGroupCompanyUser]
	ADD CONSTRAINT FK_CompanyGroupCompanyUser_CompanyUser FOREIGN KEY ([CompanyUser_Id]) REFERENCES [CompanyUser]([Id])

COMMIT;
