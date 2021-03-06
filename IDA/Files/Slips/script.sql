USE [master]
GO
/****** Object:  Database [IdaDB]    Script Date: 2020/12/18 07:48:30 ******/
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
ALTER DATABASE [IdaDB] SET  ENABLE_BROKER 
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
/****** Object:  Table [dbo].[Admin]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  Table [dbo].[AssignedPM]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignedPM](
	[AssignedPMID] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[ProjectId] [int] NULL,
	[DateAssigned] [date] NULL,
	[EmpPosition] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[AssignedPMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Assignment]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assignment](
	[AssignmentId] [int] IDENTITY(1,1) NOT NULL,
	[TaskId] [int] NULL,
	[SubTaskId] [int] NULL,
	[ProjectId] [int] NULL,
	[EmpId] [int] NULL,
	[DateAssigned] [datetime] NULL,
	[ScheduleTime] [varchar](100) NULL,
	[Stage] [int] NULL,
 CONSTRAINT [PK_Assignment] PRIMARY KEY CLUSTERED 
(
	[AssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 2020/12/18 07:48:30 ******/
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
	[DateRegistered] [datetime] NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientComment]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientComment](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
 CONSTRAINT [PK_ClientComment] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Creditor]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Creditor](
	[CreditorId] [int] IDENTITY(1,1) NOT NULL,
	[CreditorName] [varchar](50) NULL,
	[Amount] [float] NULL,
	[DateRecorded] [datetime] NULL,
 CONSTRAINT [PK_Creditor] PRIMARY KEY CLUSTERED 
(
	[CreditorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Debtor]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Debtor](
	[DebtorId] [int] IDENTITY(1,1) NOT NULL,
	[DebtorName] [varchar](50) NULL,
	[Amount] [float] NULL,
	[DateRecorded] [datetime] NULL,
 CONSTRAINT [PK_Debtor] PRIMARY KEY CLUSTERED 
(
	[DebtorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  Table [dbo].[EmpMeeting]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  Table [dbo].[Expenditure]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenditure](
	[ExpenseId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[EmpId] [int] NULL,
	[Description] [nvarchar](100) NULL,
	[DateAdded] [datetime] NULL,
	[Amount] [float] NULL,
 CONSTRAINT [PK_Expenditure] PRIMARY KEY CLUSTERED 
(
	[ExpenseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[FeedbackId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NULL,
	[Subject] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[Reason] [varchar](100) NULL,
	[description] [varchar](100) NULL,
	[MeetingId] [varchar](100) NULL,
 CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
(
	[FeedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InepVariance]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InepVariance](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Month] [date] NULL,
	[MonthVarianceReason] [nvarchar](max) NULL,
	[MonthCorrectiveAction] [nvarchar](max) NULL,
	[Year] [date] NULL,
	[YearVarianceReason] [nvarchar](max) NULL,
	[YearCorrectiveAction] [nvarchar](max) NULL,
	[OtherComments] [nvarchar](max) NULL,
	[ProjectId] [int] NULL,
	[ReportNumber] [int] NULL,
	[MunicipalityApproval] [float] NULL,
	[Pre_Engineering] [float] NULL,
	[Design] [float] NULL,
	[Procurement] [float] NULL,
	[Construction] [float] NULL,
	[CloseUp] [float] NULL,
 CONSTRAINT [PK_InepVariance] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceTbl]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceTbl](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [varchar](10) NULL,
	[Amount] [float] NULL,
	[EmpId] [int] NULL,
	[DateDue] [datetime] NULL,
	[Invoicedate] [datetime] NULL,
	[StatusIvo] [varchar](50) NULL,
	[NoteMessage] [varchar](max) NULL,
	[ProjectId] [int] NULL,
	[Item] [nvarchar](50) NULL,
	[SubTaskId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leave]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  Table [dbo].[LogisticBooking]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogisticBooking](
	[LogisticBookingId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[ProjectId] [int] NULL,
	[ReasonForBooking] [nvarchar](max) NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[VehicleId] [int] NULL,
	[Status] [varchar](50) NULL,
	[BookingDate] [datetime] NULL,
	[DateAttended] [datetime] NULL,
	[Salary] [float] NULL,
 CONSTRAINT [PK_LogisticBooking] PRIMARY KEY CLUSTERED 
(
	[LogisticBookingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meeting]    Script Date: 2020/12/18 07:48:30 ******/
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
	[MeetingNo] [varchar](100) NULL,
 CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED 
(
	[MeetingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[Date_Started] [datetime] NULL,
	[Date_Concluded] [datetime] NULL,
	[ClientId] [int] NULL,
	[EmpId] [int] NULL,
	[FilePath] [varchar](100) NULL,
	[Description] [varchar](500) NULL,
	[DateRequested] [date] NULL,
	[ProjectType] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Feedback] [nvarchar](max) NULL,
	[AdvertDate] [datetime] NULL,
	[BriefingDate] [datetime] NULL,
	[SubmitionDate] [datetime] NULL,
	[ProjectNumber] [varchar](50) NULL,
	[ProjectT] [varchar](50) NULL,
	[ContractNumber] [varchar](50) NULL,
	[ContractorType] [varchar](50) NULL,
	[SourceOfFunding] [varchar](50) NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectReport]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectReport](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[ReportDate] [datetime] NULL,
	[Description] [nvarchar](100) NULL,
	[ProjectId] [int] NULL,
 CONSTRAINT [PK_ProjectReport] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  Table [dbo].[Salary]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Salary](
	[SalaryId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[DatePaid] [datetime] NULL,
	[GrossAmount] [float] NULL,
	[HoursWorked] [int] NULL,
	[HourRate] [float] NULL,
	[NetAmount] [float] NULL,
	[OverTimeHours] [int] NULL,
 CONSTRAINT [PK_Salary] PRIMARY KEY CLUSTERED 
(
	[SalaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubTask]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubTask](
	[SubTaskId] [int] IDENTITY(1,1) NOT NULL,
	[TaskId] [int] NULL,
	[SubName] [nvarchar](max) NULL,
	[NoOfDays] [int] NULL,
 CONSTRAINT [PK_SubTask] PRIMARY KEY CLUSTERED 
(
	[SubTaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[TaskDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskSchedule]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskSchedule](
	[ScheduleId] [int] NOT NULL,
	[ScheduleTime] [varchar](50) NULL,
	[SubTaskId] [int] NULL,
 CONSTRAINT [PK_TaskSchedule] PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_UserActivation]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_UserActivation](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[ActivationCode] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Tbl_UserActivation] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[LogisticBookId] [int] NULL,
	[DateRecorded] [datetime] NULL,
	[item] [nvarchar](100) NULL,
	[Price] [varchar](50) NULL,
	[SlipPath] [nvarchar](100) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  Table [dbo].[Vehicle]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[VehicleId] [int] IDENTITY(1,1) NOT NULL,
	[VehicleName] [varchar](50) NULL,
	[BrandName] [varchar](50) NULL,
	[Model] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[DateRegisterd] [datetime] NULL,
 CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
(
	[VehicleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssignedPM]  WITH CHECK ADD FOREIGN KEY([EmpId])
REFERENCES [dbo].[Employee] ([EmpId])
GO
ALTER TABLE [dbo].[AssignedPM]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([ProjectId])
GO
ALTER TABLE [dbo].[InvoiceTbl]  WITH CHECK ADD FOREIGN KEY([EmpId])
REFERENCES [dbo].[Employee] ([EmpId])
GO
/****** Object:  StoredProcedure [dbo].[AddNewClientRecord]    Script Date: 2020/12/18 07:48:30 ******/
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
   @ClientId int, 
   @DateRegistered DateTime
)  
as  
begin  
   Insert into Client values(@Name,@Surname,@Email,@PhoneNo,@Company,@Password,@DateRegistered)  
   Set @ClientId = (Select top 1 ClientId from Client where Email = @Email)
    Insert into [dbo].[User] values(@Email,@Password,0,@ClientId,0)   
End
GO
/****** Object:  StoredProcedure [dbo].[AddNewEmployeeRecord]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  StoredProcedure [dbo].[AddNewReport]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  StoredProcedure [dbo].[AddToAllProjects]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  StoredProcedure [dbo].[getClient]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  StoredProcedure [dbo].[GetDataForMonthly]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetDataForMonthly]
-- Add the parameters for the stored procedure here

AS

  select [LogisticBooking].Salary,
  [LogisticBooking].BookingDate
  
  from [LogisticBooking],Vehicle
  where Vehicle.VehicleId = [LogisticBooking].VehicleId and
  MONTH(BookingDate)=10 and YEAR(BookingDate) = 2020
GO
/****** Object:  StoredProcedure [dbo].[getEmployee]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  StoredProcedure [dbo].[GetEmployeeByEmail]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  StoredProcedure [dbo].[GetEmployeeById]    Script Date: 2020/12/18 07:48:30 ******/
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
/****** Object:  StoredProcedure [dbo].[SqlQueryNotificationStoredProcedure-69325560-825d-4233-9434-f8fb06af79ef]    Script Date: 2020/12/18 07:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SqlQueryNotificationStoredProcedure-69325560-825d-4233-9434-f8fb06af79ef] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-69325560-825d-4233-9434-f8fb06af79ef]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-69325560-825d-4233-9434-f8fb06af79ef] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-69325560-825d-4233-9434-f8fb06af79ef') > 0)   DROP SERVICE [SqlQueryNotificationService-69325560-825d-4233-9434-f8fb06af79ef]; if (OBJECT_ID('SqlQueryNotificationService-69325560-825d-4233-9434-f8fb06af79ef', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-69325560-825d-4233-9434-f8fb06af79ef]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-69325560-825d-4233-9434-f8fb06af79ef]; END COMMIT TRANSACTION; END
GO
USE [master]
GO
ALTER DATABASE [IdaDB] SET  READ_WRITE 
GO
