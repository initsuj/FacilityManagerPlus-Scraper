CREATE DATABASE MitekShipping;
GO

USE MitekShipping;
GO

DROP TABLE IF EXISTS [Yamaha].[ASNItem]
GO

DROP SCHEMA IF EXISTS Yamaha
GO

CREATE SCHEMA Yamaha
GO

/*
CREATE TABLE [Yamaha].[ASNItem]
(
    [Id] [int] NOT NULL IDENTITY PRIMARY KEY,
    [CreatedAt] [datetime] NOT NULL DEFAULT sysutcdatetime(),
    [UpdatedAt] [datetime] NULL,
    [DeletedAt] [datetime] NULL,
    [ASNId] [char](10) NOT NULL,
    [PartId] [nvarchar](30) NOT NULL,
    [SerialId] [bigint] NOT NULL CHECK(SerialId BETWEEN 0 AND 9999999999999),
    [PurchaseOrder] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    [EmployeeId] [int] NOT NULL CHECK(EmployeeId BETWEEN 0 AND 99999)
) ON [PRIMARY]
GO
*/
