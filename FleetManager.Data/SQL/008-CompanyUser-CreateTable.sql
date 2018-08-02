CREATE TABLE [CompanyUser]
(
	[Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
	[Company_Id] BIGINT NOT NULL,
	[User_Id] BIGINT NOT NULL,
	[IsAdmin] BIT
)

ALTER TABLE [CompanyUser]
	ADD CONSTRAINT FK_CompanyUser_Company FOREIGN KEY ([Company_Id]) REFERENCES Company(Id)

ALTER TABLE [CompanyUser]
	ADD CONSTRAINT FK_CompanyUser_User FOREIGN KEY ([User_Id]) REFERENCES [User](Id)