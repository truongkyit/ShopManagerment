 USE master;
 Go
 DROP DATABASE MyShop;
 Go
 IF(db_id(N'MyShop') IS NULL)
 CREATE DATABASE MyShop
 GO

 USE MyShop
 GO

/****** Object:  Table [dbo].[Category]    Script Date: 20/04/2020 3:44:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from sysobjects where name='Category' and xtype='U')
CREATE TABLE [dbo].[Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [text] NULL,
	[Name_Slug] varchar(255) NULL,
	[CreatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[DeletedAt] [DATETIME] NULL
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 20/04/2020 3:44:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from sysobjects where name='Product' and xtype='U')
CREATE TABLE [dbo].[Product](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NULL,
	[Name] [text] NULL,
	[Description] [text] NULL,
	[Name_Slug] varchar(255) NULL,
	[SKU] [text] NOT NULL,
	[Price] [float] NULL,
	[Quantity] [int] NULL,
	[ImagePath] [text] NULL,
	[CreatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[DeletedAt] [DATETIME] NULL
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

if not exists (select * from sysobjects where name='Order' and xtype='U')
CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] varchar(50) NOT NULL,
	[Address] varchar(255) NOT NULL,
	[TotalAmount] [float] NOT NULL DEFAULT 0,
	[CreatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[DeletedAt] [DATETIME] NULL,
	[ClientID] [int] NULL,
	[ManagerID] [int] NULL,
	[StatusID] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists (select * from sysobjects where name='OrderProduct')
CREATE TABLE [dbo].[OrderProduct](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Name] varchar(255) NOT NULL,
	[OrderID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Amount] [float] NOT NULL,
	[TotalAmount] [float] NOT NULL,
	[CreatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[DeletedAt] [DATETIME] NULL,
	CONSTRAINT UC_OrderProduct UNIQUE (ProductID,OrderID),
 CONSTRAINT [PK_OrderProduct] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists (select * from sysobjects where name='Client')
CREATE TABLE [dbo].[Client](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] varchar(255) NOT NULL,
	[PhoneNumber] varchar(12) NOT NULL,
	[CreatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[DeletedAt] [DATETIME] NULL,
	UNIQUE(PhoneNumber),
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists (select * from sysobjects where name='User')
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] varchar(255) NOT NULL,
	[Password] varchar(255) NOT NULL,
	[RoleID] [int] NOT NULL,
	[Name] varchar(255) NOT NULL,
	[CreatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[DeletedAt] [DATETIME] NULL
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists (select * from sysobjects where name='Role')
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] varchar(255) NOT NULL,
	[RoleID] [int] NOT NULL,
	[CreatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedAt] [DATETIME] NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[DeletedAt] [DATETIME] NULL,
	UNIQUE (RoleID),
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists (select * from sysobjects where name='OrderStatus')
CREATE TABLE [dbo].[OrderStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EnumKey] varchar(255) NOT NULL,
	[Value] [int] NOT NULL,
	[Description] nvarchar(255) NULL,
	[DeletedAt] [DATETIME] NULL,
	UNIQUE (Value),
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_Product_Category')
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CatID])
REFERENCES [dbo].[Category] ([ID])
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_Product_Category')
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO




if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_User_Role')
ALTER TABLE [dbo].[User] WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_User_Role')
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO



if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_Order_Client')
ALTER TABLE [dbo].[Order] WITH CHECK ADD  CONSTRAINT [FK_Order_Client] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ID])
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_Order_Client')
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Order_Client]
GO


if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_OrderProduct_Order')
ALTER TABLE [dbo].[OrderProduct] WITH CHECK ADD  CONSTRAINT [FK_OrderProduct_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([ID])
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_OrderProduct_Order')
ALTER TABLE [dbo].[OrderProduct] CHECK CONSTRAINT [FK_OrderProduct_Order]
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_OrderProduct_Product')
ALTER TABLE [dbo].[OrderProduct] WITH CHECK ADD  CONSTRAINT [FK_OrderProduct_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_OrderProduct_Product')
ALTER TABLE [dbo].[OrderProduct] CHECK CONSTRAINT FK_OrderProduct_Product
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_Order_OrderStatus')
ALTER TABLE [dbo].[Order] WITH CHECK ADD  CONSTRAINT [FK_Order_OrderStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[OrderStatus] ([ID])
GO

if not exists (select * from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_Order_OrderStatus')
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT FK_Order_OrderStatus
GO


INSERT INTO [dbo].[Role] (NAME,RoleID) VALUES ('SUPPER_ADMIN',1)
INSERT INTO [dbo].[Role] (NAME,RoleID) VALUES ('MANAGER',2)

INSERT INTO [dbo].[OrderStatus] (EnumKey,Value,Description) VALUES ('New',1,N'Đơn hàng mới được tạo')
INSERT INTO [dbo].[OrderStatus] (EnumKey,Value,Description) VALUES ('Completed',2,N'Đơn hàng đã được thanh toán')
INSERT INTO [dbo].[OrderStatus] (EnumKey,Value,Description) VALUES ('Cancelled',3,N'Đơn hàng đã hủy')
INSERT INTO [dbo].[OrderStatus] (EnumKey,Value,Description) VALUES ('Shipping',4,N'Đơn hàng thanh toán và đã giao')

INSERT INTO [dbo].[User] (UserName,Password,RoleID,Name) VALUES ('Admin','1000:imUqFBg9UArqcboJjOkhKdEGPc9f83O7:cPw9m+hhVTPqmY9UR2HjI6L0XE6DNsau',1,'Admin');
INSERT INTO [dbo].[User] (UserName,Password,RoleID,Name) VALUES ('ManagerOne','1000:imUqFBg9UArqcboJjOkhKdEGPc9f83O7:cPw9m+hhVTPqmY9UR2HjI6L0XE6DNsau',2,'ManagerOne');
INSERT INTO [dbo].[User] (UserName,Password,RoleID,Name) VALUES ('ManagerTwo','1000:imUqFBg9UArqcboJjOkhKdEGPc9f83O7:cPw9m+hhVTPqmY9UR2HjI6L0XE6DNsau',2,'ManagerTwo');