USE [master]
GO
/****** Object:  Database [PLANNER]    Script Date: 11/04/2019 10:15:36 ******/
CREATE DATABASE [PLANNER]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PLANNER', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PLANNER.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PLANNER_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PLANNER_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [PLANNER] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PLANNER].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PLANNER] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PLANNER] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PLANNER] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PLANNER] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PLANNER] SET ARITHABORT OFF 
GO
ALTER DATABASE [PLANNER] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PLANNER] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PLANNER] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PLANNER] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PLANNER] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PLANNER] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PLANNER] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PLANNER] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PLANNER] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PLANNER] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PLANNER] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PLANNER] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PLANNER] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PLANNER] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PLANNER] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PLANNER] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PLANNER] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PLANNER] SET RECOVERY FULL 
GO
ALTER DATABASE [PLANNER] SET  MULTI_USER 
GO
ALTER DATABASE [PLANNER] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PLANNER] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PLANNER] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PLANNER] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PLANNER] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PLANNER', N'ON'
GO
ALTER DATABASE [PLANNER] SET QUERY_STORE = OFF
GO
USE [PLANNER]
GO
/****** Object:  UserDefinedTableType [dbo].[ListId]    Script Date: 11/04/2019 10:15:37 ******/
CREATE TYPE [dbo].[ListId] AS TABLE(
	[id] [int] NOT NULL
)
GO
/****** Object:  Table [dbo].[users]    Script Date: 11/04/2019 10:15:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](20) NOT NULL,
	[register_date] [datetime] NOT NULL,
	[last_changed_date] [datetime] NOT NULL,
	[can_create_plan] [bit] NOT NULL,
	[removed] [bit] NOT NULL,
 CONSTRAINT [pk_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[plans]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[plans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](20) NOT NULL,
	[id_type] [int] NOT NULL,
	[id_user] [int] NOT NULL,
	[id_status] [int] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[description] [varchar](50) NULL,
	[cost] [decimal](10, 2) NULL,
 CONSTRAINT [pk_plans] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[plan_history]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[plan_history](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_plan] [int] NOT NULL,
	[id_plan_status] [int] NOT NULL,
	[date] [datetime] NOT NULL,
 CONSTRAINT [pk_plan_history] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[concluded_plans_in_time]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[concluded_plans_in_time] AS
(SELECT  p.id_user, p.id, ph.id_plan, MAX(ph.date) AS date FROM [dbo].[plans] AS p
INNER JOIN [dbo].[plan_history] AS ph
ON p.id = ph.id_plan
WHERE ph.id_plan_status = 3 AND DATEDIFF(DAY, p.end_date, ph.date) <= 1 AND (SELECT can_create_plan FROM users WHERE id = p.id_user) = 0
GROUP BY p.id, ph.id_plan, p.id_user)
GO
/****** Object:  UserDefinedFunction [dbo].[FivePlanUser]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FivePlanUser]() RETURNS TABLE
AS
	RETURN (SELECT * FROM concluded_plans_in_time)
GO
/****** Object:  View [dbo].[concluded_plans_in_time_report]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[concluded_plans_in_time_report] AS
(SELECT  p.id_user, p.id, ph.id_plan, MAX(ph.date) AS date FROM [dbo].[plans] AS p
INNER JOIN [dbo].[plan_history] AS ph
ON p.id = ph.id_plan
WHERE ph.id_plan_status = 3 AND DATEDIFF(DAY, p.end_date, ph.date) <= 1
GROUP BY p.id, ph.id_plan, p.id_user)
GO
/****** Object:  View [dbo].[concluded_plans_out_time_report]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[concluded_plans_out_time_report] AS
(SELECT  p.id_user, p.id, ph.id_plan, MAX(ph.date) AS date FROM [dbo].[plans] AS p
INNER JOIN [dbo].[plan_history] AS ph
ON p.id = ph.id_plan
WHERE ph.id_plan_status = 3 AND DATEDIFF(DAY, p.end_date, ph.date) >= 1
GROUP BY p.id, ph.id_plan, p.id_user)
GO
/****** Object:  View [dbo].[report]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[report] AS
	(SELECT
		p.id_user,
		u.name,
		(SELECT COUNT(*) FROM plans WHERE id_user=p.id_user AND id_status = 2) AS Plans_Opened,
		(SELECT COUNT(*) FROM plans WHERE id_user=p.id_user AND id_status IN (4,5)) AS Plans_Suspended_Canceled,
		(SELECT COUNT(*) FROM concluded_plans_in_time_report WHERE id_user = p.id_user) AS Plans_Concluded_In_Time,
		(SELECT COUNT(*) FROM concluded_plans_out_time_report WHERE id_user = p.id_user) AS Plans_Concluded_Out_Time
	FROM users u INNER JOIN plans p
	ON u.id = p.id_user
	GROUP BY p.id_user, u.name)
GO
/****** Object:  Table [dbo].[plan_interested_user]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[plan_interested_user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_plan] [int] NOT NULL,
	[id_user] [int] NOT NULL,
 CONSTRAINT [pk_plan_interested_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[report_with_stakeholders]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[report_with_stakeholders] AS
(SELECT id, p.name AS plan_name, (SELECT name FROM users WHERE id = p.id_user) AS responsible, p.start_date, p.end_date, x.name AS stakeholders 
	FROM plans p INNER JOIN
(SELECT id_plan, id_user, u.name FROM plan_interested_user pu INNER JOIN users u ON pu.id_user = u.id) x
ON p.id = x.id_plan)
GO
/****** Object:  Table [dbo].[plan_status]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[plan_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](20) NOT NULL,
 CONSTRAINT [pk_plan_status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[plan_type]    Script Date: 11/04/2019 10:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[plan_type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](20) NOT NULL,
 CONSTRAINT [pk_plan_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_history]    Script Date: 11/04/2019 10:15:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_history](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [int] NOT NULL,
	[status] [bit] NULL,
	[create_new_plan] [bit] NULL,
	[date] [datetime] NOT NULL,
 CONSTRAINT [pk_user_history] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[plans] ADD  DEFAULT ((0)) FOR [id_status]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT ((1)) FOR [can_create_plan]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT ((0)) FOR [removed]
GO
ALTER TABLE [dbo].[plan_history]  WITH CHECK ADD  CONSTRAINT [fk_plans] FOREIGN KEY([id_plan])
REFERENCES [dbo].[plans] ([id])
GO
ALTER TABLE [dbo].[plan_history] CHECK CONSTRAINT [fk_plans]
GO
ALTER TABLE [dbo].[plan_interested_user]  WITH CHECK ADD  CONSTRAINT [fk_plans_plan_interested_user] FOREIGN KEY([id_plan])
REFERENCES [dbo].[plans] ([id])
GO
ALTER TABLE [dbo].[plan_interested_user] CHECK CONSTRAINT [fk_plans_plan_interested_user]
GO
ALTER TABLE [dbo].[plan_interested_user]  WITH CHECK ADD  CONSTRAINT [fk_user_plan_interested_user] FOREIGN KEY([id_user])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[plan_interested_user] CHECK CONSTRAINT [fk_user_plan_interested_user]
GO
ALTER TABLE [dbo].[plans]  WITH CHECK ADD  CONSTRAINT [fk_status] FOREIGN KEY([id_status])
REFERENCES [dbo].[plan_status] ([id])
GO
ALTER TABLE [dbo].[plans] CHECK CONSTRAINT [fk_status]
GO
ALTER TABLE [dbo].[plans]  WITH CHECK ADD  CONSTRAINT [fk_type] FOREIGN KEY([id_type])
REFERENCES [dbo].[plan_type] ([id])
GO
ALTER TABLE [dbo].[plans] CHECK CONSTRAINT [fk_type]
GO
ALTER TABLE [dbo].[plans]  WITH CHECK ADD  CONSTRAINT [fk_user] FOREIGN KEY([id_user])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[plans] CHECK CONSTRAINT [fk_user]
GO
ALTER TABLE [dbo].[user_history]  WITH CHECK ADD  CONSTRAINT [fk_user_history] FOREIGN KEY([id_user])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[user_history] CHECK CONSTRAINT [fk_user_history]
GO
/****** Object:  StoredProcedure [dbo].[CheckLatePlans]    Script Date: 11/04/2019 10:15:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckLatePlans]
AS
BEGIN
	DECLARE cursor_tab_users CURSOR FOR SELECT id FROM users

	DECLARE @id INT

	OPEN cursor_tab_users

		FETCH NEXT FROM cursor_tab_users INTO @id

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (SELECT COUNT(*) FROM plans WHERE id_user = @id AND id_status <> 3 AND DATEDIFF(MONTH, end_date, GETDATE()) >= 1) >= 1
				BEGIN
					UPDATE users
					SET can_create_plan = 0
					WHERE id = @id
				END
			ELSE IF (SELECT COUNT(*) FROM plans WHERE id_user = @id AND id_status <> 3 AND DATEDIFF(DAY, end_date, GETDATE()) >= 1) >= 2
				BEGIN
					UPDATE users
					SET can_create_plan = 0
					WHERE id = @id
				END

			FETCH NEXT FROM cursor_tab_users INTO @id
		END
	CLOSE cursor_tab_users
END;
GO
/****** Object:  StoredProcedure [dbo].[filter_report]    Script Date: 11/04/2019 10:15:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[filter_report]
@id_list AS ListId READONLY
AS
BEGIN
	SELECT a.id, a.plan_name, a.responsible, a.start_date, a.end_date, a.Stakeholders 
		FROM (SELECT  id,
				 plan_name,
				 responsible,
				 start_date,
				 end_date,
				 COALESCE(
					(SELECT CAST(stakeholders AS VARCHAR(10)) + ',' AS [text()]
					FROM report_with_stakeholders AS x
					WHERE x.id  = y.id
					AND   x.plan_name = y.plan_name
					AND   x.start_date = y.start_date
					AND   x.end_date = y.end_date
					ORDER BY id
					FOR XML PATH(''), TYPE).value('.[1]', 'VARCHAR(MAX)'), '') AS Stakeholders
				 FROM report_with_stakeholders AS y) AS a
		 INNER JOIN @id_list AS b
		 ON a.id = b.id
		 GROUP BY a.id, a.plan_name, a.responsible, a.start_date, a.end_date, a.Stakeholders;
END;
GO
USE [master]
GO
ALTER DATABASE [PLANNER] SET  READ_WRITE 
GO
