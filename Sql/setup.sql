CREATE DATABASE Fabricam;
GO

USE Fabricam;
GO

CREATE TABLE dbo.LeadStatus (
  LeadStatusId int NOT NULL,
  LeadStatusDesc nvarchar(20) NOT NULL,
  CONSTRAINT PK_LeadStatus PRIMARY KEY CLUSTERED (
    LeadStatusId ASC
  )
)
GO

INSERT into dbo.LeadStatus (LeadStatusId, LeadStatusDesc) VALUES (1, 'Created')
INSERT into dbo.LeadStatus (LeadStatusId, LeadStatusDesc) VALUES (2, 'CheckedOut')
INSERT into dbo.LeadStatus (LeadStatusId, LeadStatusDesc) VALUES (3, 'Completed')
GO

CREATE TABLE dbo.Lead (
  LeadId int IDENTITY(1,1) NOT NULL,
  LeadStatusId int NOT NULL,
  CheckedOutBy int NULL,
  Title nvarchar(30) NULL,
  FirstName nvarchar(50) NOT NULL,
  LastName nvarchar(50) NOT NULL,
  Gender nvarchar(10) NULL,
  Address nvarchar(100) NULL,
  City nvarchar(100) NULL,
  State nvarchar(50) NULL,
  PostalCode nvarchar(10) NULL,
  Country nvarchar(5) NOT NULL,
  Email nvarchar(200) NULL,
  Phone nvarchar(15) NULL,
  Cell nvarchar(15) NULL,
  CONSTRAINT PK_Lead PRIMARY KEY CLUSTERED (
    LeadId ASC
  )
)
GO

ALTER TABLE dbo.Lead WITH CHECK
ADD CONSTRAINT FK_Lead_LeadStatus FOREIGN KEY(LeadStatusId)
REFERENCES dbo.LeadStatus (LeadStatusId)
GO

CREATE PROCEDURE dbo.LeadGetNext (
  @CheckedOutBy int
) AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @LeadResults TABLE (
    LeadId int NOT NULL,
    LeadStatusId int NOT NULL,
    CheckedOutBy int NULL,
    Title nvarchar(30) NULL,
    FirstName nvarchar(50) NOT NULL,
    LastName nvarchar(50) NOT NULL,
    Gender nvarchar(10) NULL,
    Address nvarchar(100) NULL,
    City nvarchar(100) NULL,
    State nvarchar(50) NULL,
    PostalCode nvarchar(10) NULL,
    Country nvarchar(5) NOT NULL,
    Email nvarchar(200) NULL,
    Phone nvarchar(15) NULL,
    Cell nvarchar(15) NULL
  )

  BEGIN TRANSACTION T1

  -- FRAGILE: LeadStatus.CheckedOut == 2
  UPDATE dbo.Lead
  SET LeadStatusId = 2, CheckedOutBy = @CheckedOutBy
  OUTPUT inserted.LeadId, inserted.LeadStatusId, inserted.CheckedOutBy,
    inserted.Title, inserted.FirstName, inserted.LastName, inserted.Gender,
    inserted.Address, inserted.City, inserted.State, inserted.PostalCode, inserted.Country,
    inserted.Email, inserted.Phone, inserted.Cell
  INTO @LeadResults
  FROM dbo.Lead
  WHERE LeadId = (
    SELECT TOP 1 LeadId
    FROM dbo.Lead WITH (UPDLOCK)
    WHERE LeadStatusId = 1
    ORDER BY LeadId -- TODO: filter by tenant, sort by business data
  )

  COMMIT TRANSACTION T1

  SELECT LeadId, LeadStatusId, CheckedOutBy,
  Title, FirstName, LastName, Gender,
  Address, City, State, PostalCode, Country,
  Email, Phone, Cell
  FROM @LeadResults
END
GO
