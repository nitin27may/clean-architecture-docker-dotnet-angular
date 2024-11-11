USE [master]
GO

-- Create a new database
IF NOT EXISTS (SELECT *
FROM sys.databases
WHERE name = 'Contacts')
BEGIN
	CREATE DATABASE Contacts;
END;
GO

ALTER DATABASE [Contacts] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
	EXEC [Contacts].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Contacts] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Contacts] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Contacts] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Contacts] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Contacts] SET ARITHABORT OFF 
GO
ALTER DATABASE [Contacts] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Contacts] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Contacts] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Contacts] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Contacts] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Contacts] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Contacts] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Contacts] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Contacts] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Contacts] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Contacts] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Contacts] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Contacts] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Contacts] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Contacts] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Contacts] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Contacts] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Contacts] SET RECOVERY FULL 
GO
ALTER DATABASE [Contacts] SET  MULTI_USER 
GO
ALTER DATABASE [Contacts] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Contacts] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Contacts] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Contacts] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Contacts] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Contacts] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Contacts] SET QUERY_STORE = ON
GO
ALTER DATABASE [Contacts] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Contacts]
GO
/****** Object:  Table [dbo].[ActivityLog]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'ActivityLog')
BEGIN
	CREATE TABLE [dbo].[ActivityLog]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[UserId] [uniqueidentifier] NOT NULL,
		[Activity] [nvarchar](250) NOT NULL,
		[Endpoint] [nvarchar](250) NOT NULL,
		[HttpMethod] [nvarchar](10) NOT NULL,
		[Timestamp] [datetimeoffset](7) NOT NULL,
		[IpAddress] [nvarchar](45) NULL,
		[UserAgent] [nvarchar](250) NULL,
		PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END;
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'Contacts')
BEGIN
	CREATE TABLE [dbo].[Contacts]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[FirstName] [nvarchar](100) NOT NULL,
		[LastName] [nvarchar](100) NOT NULL,
		[DateOfBirth] [date] NULL,
		[CountryCode] [INT] NOT NULL,
		[Mobile] [bigint] NOT NULL,
		[Email] [nvarchar](255) NOT NULL,
		[City] [nvarchar](100) NOT NULL,
		[PostalCode] [nvarchar](10) NOT NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT [dbo].[Contacts]
		([Id], [FirstName], [LastName], [DateOfBirth], [CountryCode], [Mobile], [Email], [City], [PostalCode], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'340a2588-1f74-4a0b-8e4b-137ce0329c50', N'Nitin 1', N'Singh', CAST(N'1986-08-21' AS Date), 91, 6475305663, N'nitin27may@gmail.com', N'Whitby', N'L1N9G9', CAST(N'2024-08-25T03:48:35.3938849+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Contacts]
		([Id], [FirstName], [LastName], [DateOfBirth], [CountryCode], [Mobile], [Email], [City], [PostalCode], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'8076b4c8-98e3-43c2-abab-cffb73ffba03', N'Nitin', N'Singh', CAST(N'2024-09-05' AS Date), 91, 9833366734, N'nitin27may234@gmail.com', N'Whitby', N'L1N9G9', CAST(N'2024-09-05T14:21:49.2766085+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

END;
GO
/****** Object:  Table [dbo].[Operations]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'Operations')
BEGIN
	CREATE TABLE [dbo].[Operations]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](250) NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT [dbo].[Operations]
		([Id], [Name], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', N'Delete', N'Delete', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Operations]
		([Id], [Name], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'dce8d805-df41-4549-be7b-6ed5647b09c3', N'Update', N'Update', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Operations]
		([Id], [Name], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'7493f274-5007-4e17-9840-88c9a096422f', N'Read', N'Read', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Operations]
		([Id], [Name], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'09be3f29-6429-4089-a2a9-a17efe46cd7b', N'Write', N'Write', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

END;
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'Pages')
BEGIN
	CREATE TABLE [dbo].[Pages]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Url] [nvarchar](250) NOT NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]


	INSERT [dbo].[Pages]
		([Id], [Name], [Url], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', N'Contacts', N'/contacts', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Pages]
		([Id], [Name], [Url], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'c4943131-a642-4352-9725-e44ba5972b4b', N'Users', N'/users', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

END;
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'Permissions')
BEGIN
	CREATE TABLE [dbo].[Permissions]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[PageId] [uniqueidentifier] NOT NULL,
		[OperationId] [uniqueidentifier] NOT NULL,
		[Description] [nvarchar](250) NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'd35daa4e-fd02-4934-98d2-5b06e9b694b8', N'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', N'09be3f29-6429-4089-a2a9-a17efe46cd7b', N'Contacts Write', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'82755e66-b743-46e2-b612-efd2db6bcc75', N'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', N'dce8d805-df41-4549-be7b-6ed5647b09c3', N'Contacts Update', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'a6657401-0d2b-4bf9-9314-26be98878822', N'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', N'7493f274-5007-4e17-9840-88c9a096422f', N'Contacts Read', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'd5282999-a12a-45ad-b7d0-2becc27d5e92', N'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', N'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', N'Contacts Delete', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'c94c23ad-59d4-4f80-91ee-39316140cb17', N'c4943131-a642-4352-9725-e44ba5972b4b', N'09be3f29-6429-4089-a2a9-a17efe46cd7b', N'User Create', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'c94c23ad-59d4-4f80-91ee-39316140cb17', N'c4943131-a642-4352-9725-e44ba5972b4b', N'dce8d805-df41-4549-be7b-6ed5647b09c3', N'User Update', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'82755e66-b743-46e2-b612-efd2db6bcc75', N'c4943131-a642-4352-9725-e44ba5972b4b', N'7493f274-5007-4e17-9840-88c9a096422f', N'User Read', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[Permissions]
		([Id], [PageId], [OperationId], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'82755e66-b743-46e2-b612-efd2db6bcc75', N'c4943131-a642-4352-9725-e44ba5972b4b', N'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', N'User Delete', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
END;
GO
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'RolePermissions')
BEGIN
	CREATE TABLE [dbo].[RolePermissions]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[RoleId] [uniqueidentifier] NOT NULL,
		[PermissionId] [uniqueidentifier] NOT NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

-- Admin Role Permissions
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77730bc6-60f4-42df-913d-6d558f1871ce', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'a6657401-0d2b-4bf9-9314-26be98878822', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733d', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'd35daa4e-fd02-4934-98d2-5b06e9b694b8',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733f', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'82755e66-b743-46e2-b612-efd2db6bcc75',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'd5282999-a12a-45ad-b7d0-2becc27d5e92',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'c94c23ad-59d4-4f80-91ee-39316140cb17',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
		INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'7fbbc09d-b2e2-46fe-92d5-cec881f16708',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
		INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'1611852e-1dc5-4f26-9204-746edcab50db',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
		INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'e84cb3ca-6a28-4eb6-91ca-447fa2cd91d4',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)	


---- Editor
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77730bc6-60f4-42df-913d-6d558f1871ce', N'104102f5-e0ec-4739-8fda-f05552b677c3', N'a6657401-0d2b-4bf9-9314-26be98878822', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733d', N'104102f5-e0ec-4739-8fda-f05552b677c3', N'd35daa4e-fd02-4934-98d2-5b06e9b694b8',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733f', N'104102f5-e0ec-4739-8fda-f05552b677c3', N'82755e66-b743-46e2-b612-efd2db6bcc75',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'104102f5-e0ec-4739-8fda-f05552b677c3', N'c94c23ad-59d4-4f80-91ee-39316140cb17',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
		
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'104102f5-e0ec-4739-8fda-f05552b677c3', N'1611852e-1dc5-4f26-9204-746edcab50db',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'104102f5-e0ec-4739-8fda-f05552b677c3', N'e84cb3ca-6a28-4eb6-91ca-447fa2cd91d4',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)	

---- Reader
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77730bc6-60f4-42df-913d-6d558f1871ce', N'3a07551f-7473-44a6-a664-e6c7c834902b', N'a6657401-0d2b-4bf9-9314-26be98878822', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)
	
	INSERT [dbo].[RolePermissions]
		([Id], [RoleId], [PermissionId],  [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'77b5e37b-40d7-4f37-a66a-7c4649d7733g', N'3a07551f-7473-44a6-a664-e6c7c834902b', N'1611852e-1dc5-4f26-9204-746edcab50db',  CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

END;
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2024-09-05 3:01:34 PM ******/														
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'Roles')
BEGIN
	CREATE TABLE [dbo].[Roles]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](250) NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]


	INSERT [dbo].[Roles]
		([Id], [Name], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', N'Admin', N'This is Admin Role', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Roles]
		([Id], [Name], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'3a07551f-7473-44a6-a664-e6c7c834902b', N'Reader', N'This is a Reader role', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[Roles]
		([Id], [Name], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'104102f5-e0ec-4739-8fda-f05552b677c3', N'Editor ', N'This is editor role', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

END;
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'UserRoles')
BEGIN
	CREATE TABLE [dbo].[UserRoles]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[UserId] [uniqueidentifier] NOT NULL,
		[RoleId] [uniqueidentifier] NOT NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[Createdby] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL
	) ON [PRIMARY]

	INSERT [dbo].[UserRoles]
		([Id], [UserId], [RoleId], [CreatedOn], [Createdby], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'7a74994f-e940-4132-9907-4e0de3b27f11', N'26402b6c-ebdd-44c3-9188-659a134819cb', N'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), N'26402b6c-ebdd-44c3-9188-659a134819cb', NULL, NULL)

	INSERT [dbo].[UserRoles]
		([Id], [UserId], [RoleId], [CreatedOn], [Createdby], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'fd3d4995-f8e5-48fb-8747-c816b26063e6', N'424ffb80-05bf-43f8-8814-2772a5de2543', N'3a07551f-7473-44a6-a664-e6c7c834902b', CAST(N'2024-09-05T10:48:17.5898885-04:00' AS DateTimeOffset), N'424ffb80-05bf-43f8-8814-2772a5de2543', NULL, NULL)

	INSERT [dbo].[UserRoles]
		([Id], [UserId], [RoleId], [CreatedOn], [Createdby], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'd9f6ab7e-e2a0-4138-9a24-cbccec759116', N'3aa35df1-2578-4ed3-a93b-8b8eb955499e', N'104102f5-e0ec-4739-8fda-f05552b677c3', CAST(N'2024-08-28T20:29:02.8859184+00:00' AS DateTimeOffset), N'3aa35df1-2578-4ed3-a93b-8b8eb955499e', NULL, NULL)

END;
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024-09-05 3:01:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'Users')
BEGIN
	CREATE TABLE [dbo].[Users]
	(
		[Id] [uniqueidentifier] NOT NULL,
		[FirstName] [nvarchar](250) NOT NULL,
		[LastName] [nvarchar](250) NOT NULL,
		[UserName] [nvarchar](100) NOT NULL,
		[Email] [nvarchar](200) NOT NULL,
		[Mobile] [int] NOT NULL,
		[Password] [nvarchar](max) NOT NULL,
		[CreatedOn] [datetimeoffset](7) NOT NULL,
		[Createdby] [uniqueidentifier] NULL,
		[UpdatedOn] [datetimeoffset](7) NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	INSERT [dbo].[Users]
		([Id], [FirstName], [LastName], [UserName], [Email], [Mobile], [Password], [CreatedOn], [Createdby], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'424ffb80-05bf-43f8-8814-2772a5de2543', N'Sachin', N'Singh', N'sachin@gmail.com', N'sachin@gmail.com', 9833364, N'AQAAAAIAAYagAAAAEC1iNqNI7oqJKNcpJ+kYreWvBzjMxE/FWhfoDXzP5CoV60u6JHm5PwHIb3w7K7lWxw==', CAST(N'2024-09-05T14:48:17.0399044+00:00' AS DateTimeOffset), N'00000000-0000-0000-0000-000000000000', NULL, NULL)

	INSERT [dbo].[Users]
		([Id], [FirstName], [LastName], [UserName], [Email], [Mobile], [Password], [CreatedOn], [Createdby], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'26402b6c-ebdd-44c3-9188-659a134819cb', N'Nitin', N'Singh', N'nitin27may@gmail.com', N'nitin27may@gmail.com', 9833364, N'AQAAAAIAAYagAAAAEC1iNqNI7oqJKNcpJ+kYreWvBzjMxE/FWhfoDXzP5CoV60u6JHm5PwHIb3w7K7lWxw==', CAST(N'2024-08-24T02:56:23.6635113+00:00' AS DateTimeOffset), NULL, CAST(N'2024-09-04T15:36:10.1171041+00:00' AS DateTimeOffset), NULL)

	INSERT [dbo].[Users]
		([Id], [FirstName], [LastName], [UserName], [Email], [Mobile], [Password], [CreatedOn], [Createdby], [UpdatedOn], [UpdatedBy])
	VALUES
		(N'3aa35df1-2578-4ed3-a93b-8b8eb955499e', N'Vikram', N'Singh', N'vikram@gmail.com', N'vikram@gmail.com', 9833364, N'AQAAAAIAAYagAAAAEC1iNqNI7oqJKNcpJ+kYreWvBzjMxE/FWhfoDXzP5CoV60u6JHm5PwHIb3w7K7lWxw==', CAST(N'2024-08-28T20:29:02.2362893+00:00' AS DateTimeOffset), N'00000000-0000-0000-0000-000000000000', NULL, NULL)

END;
GO



ALTER TABLE [dbo].[ActivityLog] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Contacts] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Operations] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Pages] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DEFAULT_Users_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_Operations] FOREIGN KEY([OperationId])
REFERENCES [dbo].[Operations] ([Id])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_Operations]
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_Pages] FOREIGN KEY([PageId])
REFERENCES [dbo].[Pages] ([Id])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_Pages]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([Id])
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Permissions]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Roles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_RoleUsers_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_RoleUsers_Roles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_RoleUsers_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_RoleUsers_Users]
GO
USE [master]
GO
ALTER DATABASE [Contacts] SET  READ_WRITE 
GO
