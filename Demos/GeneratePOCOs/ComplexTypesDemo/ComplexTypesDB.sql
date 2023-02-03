USE [master]
GO

IF EXISTS(select * from sys.databases where name='ComplexTypesDB')
DROP DATABASE ComplexTypesDB
GO

CREATE DATABASE ComplexTypesDB
GO

USE [ComplexTypesDB]
GO

CREATE TABLE [ComplexTypesDB].[dbo].[Customers](
	[CustomerId] [int] NOT NULL,
	[CustomerName] [nvarchar](50) NOT NULL,
	[ShippingAddress_Street] [nvarchar](50) NULL,
	[ShippingAddress_City] [nvarchar](50) NULL,
	[ShippingAddress_ZipCode] [nvarchar](50) NULL,
	[BillingAddress_Street] [nvarchar](50) NULL,
	[BillingAddress_City] [nvarchar](50) NULL,
	[BillingAddress_ZipCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [ComplexTypesDB].[dbo].[Users](
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Address_Street] [nvarchar](50) NULL,
	[Address_City] [nvarchar](50) NULL,
	[Address_ZipCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
