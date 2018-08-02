BEGIN TRAN;

CREATE TABLE [CompanyGroupModulePermission]
(
	[Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
	[CompanyGroup_Id] BIGINT NOT NULL,
	[Module_Id] BIGINT NOT NULL,
	[View_Right] BIT,
	[Add_Right] BIT,
	[Edit_Right] BIT,
	[Delete_Right] BIT,
	[IsDeleted] BIT,
	[DeletedOn] DATETIME2
)

ALTER TABLE [CompanyGroupModulePermission]
	ADD CONSTRAINT FK_CompanyGroupModulePermission_CompanyGroup FOREIGN KEY ([CompanyGroup_Id]) REFERENCES [CompanyGroup]([Id])

ALTER TABLE [CompanyGroupModulePermission]
	ADD CONSTRAINT FK_CompanyGroupModulePermission_Module FOREIGN KEY ([Module_Id]) REFERENCES [Module]([Id])

COMMIT;