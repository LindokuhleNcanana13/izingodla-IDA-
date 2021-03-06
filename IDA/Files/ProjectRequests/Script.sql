USE [master]
GO
/****** Object:  Database [IdaDB]    Script Date: 2020/09/01 08:31:10 ******/
CREATE DATABASE [IdaDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IdaDB', FILENAME = N'C:\Users\User1\IdaDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'IdaDB_log', FILENAME = N'C:\Users\User1\IdaDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [IdaDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IdaDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IdaDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IdaDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IdaDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IdaDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IdaDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [IdaDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [IdaDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IdaDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IdaDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IdaDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IdaDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IdaDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IdaDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IdaDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IdaDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [IdaDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IdaDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IdaDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IdaDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IdaDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IdaDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IdaDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IdaDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [IdaDB] SET  MULTI_USER 
GO
ALTER DATABASE [IdaDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IdaDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IdaDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IdaDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [IdaDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [IdaDB] SET QUERY_STORE = OFF
GO
USE [IdaDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [IdaDB]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 2020/09/01 08:31:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[AdminId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Surname] [varchar](50) NULL,
	[Position] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[PhoneNo] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[IdNumber] [varchar](50) NULL,
 CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED 
(
	[AdminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AllProjects]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AllProjects](
	[AllProjId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Date] [date] NULL,
	[ProjectId] [int] NULL,
	[Location] [varchar](70) NULL,
 CONSTRAINT [PK_AllProjects] PRIMARY KEY CLUSTERED 
(
	[AllProjId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[PhoneNo] [varchar](50) NOT NULL,
	[Company] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmpId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[PhoneNo] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[IDNumber] [varchar](50) NOT NULL,
	[Salary] [varchar](50) NOT NULL,
	[Position] [varchar](50) NOT NULL,
	[HireDate] [date] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpMeeting]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpMeeting](
	[EmpMeetId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[MeetingId] [int] NULL,
	[ClientId] [int] NULL,
 CONSTRAINT [PK_EmpMeeting] PRIMARY KEY CLUSTERED 
(
	[EmpMeetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leave]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leave](
	[LeaveId] [int] IDENTITY(1,1) NOT NULL,
	[AppDate] [datetime] NOT NULL,
	[Duration] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[DateApproved] [datetime] NULL,
	[EmpId] [int] NULL,
	[ApplicantName] [varchar](50) NULL,
	[ReasonForApplying] [varchar](70) NULL,
	[Fromdate] [date] NULL,
	[ToDate] [date] NULL,
	[NoOfDays] [int] NULL,
	[Comment] [varchar](200) NULL,
	[FeedMessage] [varchar](500) NULL,
 CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED 
(
	[LeaveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meeting]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meeting](
	[MeetingId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [varchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ThemeColor] [varchar](50) NOT NULL,
	[EmpId] [int] NULL,
	[ClientId] [int] NULL,
	[NewClientEmail] [nvarchar](200) NULL,
	[TDate] [datetime] NOT NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED 
(
	[MeetingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[Date_Started] [date] NOT NULL,
	[Date_Concluded] [date] NULL,
	[ClientId] [int] NOT NULL,
	[EmpId] [int] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project_Request]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_Request](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[DateRequested] [date] NULL,
	[ClientId] [int] NULL,
	[FilePath] [varchar](200) NULL,
	[Description] [varchar](200) NULL,
 CONSTRAINT [PK_Project_Request] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[ReportId] [int] IDENTITY(1,1) NOT NULL,
	[WorkDescription] [varchar](50) NULL,
	[HoursWorked] [int] NULL,
	[TargetedHours] [int] NULL,
	[EmpId] [int] NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stage]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stage](
	[StageId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[TaskId] [int] NOT NULL,
 CONSTRAINT [PK_Stage] PRIMARY KEY CLUSTERED 
(
	[StageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[TaskDescription] [varchar](50) NOT NULL,
	[StageId] [int] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[EmpId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[AdminId] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddNewClientRecord]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AddNewClientRecord]  
(  
   @Name varchar (50),  
   @Surname varchar (50),
   @Email varchar(50),
   @Password varchar (50),  
   @Company varchar(50),
   @PhoneNo nvarchar (50),
   @ClientId int 
)  
as  
begin  
   Insert into Client values(@Name,@Surname,@Email,@PhoneNo,@Company,@Password)  
   Set @ClientId = (Select top 1 ClientId from Client where Email = @Email)
    Insert into [dbo].[User] values(@Email,@Password,0,@ClientId,0)   
End
GO
/****** Object:  StoredProcedure [dbo].[AddNewEmployeeRecord]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AddNewEmployeeRecord]  
(  
   @Name varchar (50),  
   @Surname varchar (50),  
   @Gender nvarchar (50),
   @Email varchar(50),
   @Position varchar (50),  
   @Address varchar (50),  
   @HireDate nvarchar (50),
   @Salary varchar(50),
   @IDNumber varchar (50),  
   @PhoneNo nvarchar (50),
   @EmpId int 
)  
as  
begin  
   Insert into Employee values(@Name,@Surname,@Email,@PhoneNo,@Address,@Gender,@IDNumber,@Salary,@Position,@HireDate)  
   Set @EmpId = (Select  EmpId from Employee where Email = @Email)
    Insert into [dbo].[User] values(@Email,@IDNumber,@EmpId,0,0)   
End
GO
/****** Object:  StoredProcedure [dbo].[AddNewReport]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[AddNewReport]
(
	@ReportId int,
	@WorkDescription varchar(50),
	@HoursWorked int,
	@TargetedHours int,
	@EmpId	int
)
as
begin
	INSERT into Report values(@WorkDescription,@HoursWorked,@TargetedHours,@EmpId)
end
GO
/****** Object:  StoredProcedure [dbo].[AddToAllProjects]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[AddToAllProjects]
(
	@AllProjId int,
	@Name varchar(50),
	@Date date,
	@Location varchar(50),
	@ProjectId int
)
as
begin
	insert into AllProjects values(@Name,@Date,0,@Location)
end
GO
/****** Object:  StoredProcedure [dbo].[getClient]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[getClient]  
as  
begin  
   select * from Client
End
GO
/****** Object:  StoredProcedure [dbo].[getEmployee]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[getEmployee]  
as  
begin  
   select * from Employee
End
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeByEmail]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create Procedure [dbo].[GetEmployeeByEmail]  

as  
begin  
   select * from Employee
End
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeById]    Script Date: 2020/09/01 08:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[GetEmployeeById]  
(
	@EmpId int
)
as  
begin  
   select * from Employee where EmpId=@EmpId
End
GO
USE [master]
GO
ALTER DATABASE [IdaDB] SET  READ_WRITE 
GO
