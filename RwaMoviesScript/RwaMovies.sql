USE [master]
GO
/****** Object:  Database [RwaMovies]    Script Date: 6/26/2023 6:35:18 PM ******/
CREATE DATABASE [RwaMovies]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RwaMovies', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\RwaMovies.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RwaMovies_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\RwaMovies_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [RwaMovies] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RwaMovies].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RwaMovies] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RwaMovies] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RwaMovies] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RwaMovies] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RwaMovies] SET ARITHABORT OFF 
GO
ALTER DATABASE [RwaMovies] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [RwaMovies] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RwaMovies] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RwaMovies] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RwaMovies] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RwaMovies] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RwaMovies] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RwaMovies] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RwaMovies] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RwaMovies] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RwaMovies] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RwaMovies] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RwaMovies] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RwaMovies] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RwaMovies] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RwaMovies] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RwaMovies] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RwaMovies] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RwaMovies] SET  MULTI_USER 
GO
ALTER DATABASE [RwaMovies] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RwaMovies] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RwaMovies] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RwaMovies] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RwaMovies] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RwaMovies] SET QUERY_STORE = OFF
GO
USE [RwaMovies]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [char](2) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genre]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genre](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[Id] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ReceiverEmail] [nvarchar](256) NOT NULL,
	[Subject] [nvarchar](256) NULL,
	[Body] [nvarchar](1024) NOT NULL,
	[SentAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[DeletedAt] [datetime2](7) NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PwdHash] [nvarchar](256) NOT NULL,
	[PwdSalt] [nvarchar](256) NOT NULL,
	[Phone] [nvarchar](256) NULL,
	[IsConfirmed] [bit] NOT NULL,
	[SecurityToken] [nvarchar](256) NULL,
	[CountryOfResidenceId] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Video]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Video](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[GenreId] [int] NOT NULL,
	[TotalSeconds] [int] NOT NULL,
	[StreamingUrl] [nvarchar](256) NULL,
	[ImageId] [int] NULL,
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VideoTag]    Script Date: 6/26/2023 6:35:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoTag](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VideoId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_VideoTag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (1, N'HR', N'Croatia')
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (6, N'SB', N'Serbia')
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (7, N'AB', N'Albania')
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (8, N'JP', N'Japan')
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (9, N'BH', N'Bosnia and Hercegovina')
INSERT [dbo].[Country] ([Id], [Code], [Name]) VALUES (10, N'VA', N'Vatican')
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
SET IDENTITY_INSERT [dbo].[Genre] ON 

INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (1, N'Action', N'"Action" is a film genre characterized by energy, fast pacing, and a substantial amount of physical stunts and activities. These films often feature dynamic pursuits, fights, battles, races, or rescues that create adrenaline-fueled viewing experiences.')
INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (2, N'Comedy', N'"Comedy" is a genre of film that primarily focuses on humor to entertain its audience. The main goal is to provoke laughter through amusing storylines, witty dialogue, slapstick routines, or absurd situations.')
INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (3, N'Thriller', N'"Thriller" is a genre of film that primarily aims to evoke excitement, suspense, surprise, anticipation, and anxiety in its audience. Thrillers often involve a crime, mystery, or conspiracy that must be solved, with the protagonist in a race against time. The narrative is usually driven by a constant threat or danger, and unexpected plot twists. High stakes, tense situations, and a captivating plot are key elements of this genre. Thrillers can cross over with other genres like action, crime, horror, or science fiction, and they are designed to keep audiences at the edge of their seats.')
INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (7, N'Romance', N'Romance" is a film genre characterized by a focus on passion, emotion, and the romantic love between characters. The storyline often revolves around the romantic relationships and challenges faced by the protagonists, leading to emotionally satisfying and optimistic endings. These films explore themes like love at first sight, forbidden love, love triangles, and sacrificial love.')
INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (8, N'Western', N'"Western" is a genre of film that typically encapsulates the mythology of the American Old West. Set in the late 19th century, these films often feature rugged cowboys, lawmen, outlaws, Native Americans, and settlers. Common themes include the taming of the wilderness, the establishment of law and order, and conflicts between farmers and ranchers or white settlers and Native Americans.')
INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (20, N'Horror', N'The primary purpose of horror films is to scare, shock, or frighten the audience. They often involve an evil force, event, or character of some kind, commonly of supernatural origin. Key elements include suspense, fear, surprise, and mystery. Sub-genres include slasher films, monster movies, and psychological horror.')
INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (29, N'Cartoon', N'Cartoon" is a genre of film primarily characterized by animation and drawn characters. These films can range from simple hand-drawn designs to complex computer-generated imagery. Though often perceived as being designed for children, the cartoon genre can cater to all age groups with a variety of themes, including comedy, fantasy, adventure, and even more adult themes in some cases. Storylines can be whimsical, serious, or educational, often personifying animals or inanimate objects. ')
INSERT [dbo].[Genre] ([Id], [Name], [Description]) VALUES (30, N'Fantasy', N'Fantasy films contain elements that are impossible or improbable in the real world, usually involving magic, supernatural events, mythology, folklore, or exotic fantasy worlds. These films ask the audience to suspend their disbelief and to journey into realms where the rules of the real world don''t apply.')
SET IDENTITY_INSERT [dbo].[Genre] OFF
GO
INSERT [dbo].[Image] ([Id], [Content]) VALUES (1, N'https://media.istockphoto.com/id/1250025148/photo/finland-lake-nature-landscape-forest-wilderness.jpg?s=612x612&w=0&k=20&c=blGYp39VCvlbAhuP2XicHX9FLSlQMi1RRwNZQlFhuXU=')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (2, N'https://assets-prd.ignimgs.com/2022/03/14/sonic-2-poster-and-game-side-by-side-1647281169671.jpg')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (3, N'https://images.pushsquare.com/b17e68747f0f7/mortal-kombat-2021-movie-review-1.large.jpg')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (4, N'https://store-images.s-microsoft.com/image/apps.2678.14492077886571533.a48e9a4a-99a7-44a9-9d97-9e8e72220a7c.6b98c506-61b7-4126-80b6-449f2ff0fb96?mode=scale&q=90&h=1080&w=1920&background=%23FFFFFF')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (5, N'https://i.pcmag.com/imagery/reviews/05cItXL96l4LE9n02WfDR0h-5.fit_scale.size_1028x578.v1582751026.png')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (6, N'https://images.unsplash.com/photo-1626814026160-2237a95fc5a0?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1170&q=80')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (7, N'https://www.hollywoodreporter.com/wp-content/uploads/2016/06/similar_posters-main-image-h_2016.jpg?w=2000&h=1126&crop=1')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (8, N'https://www.looper.com/img/gallery/why-do-movie-posters-reverse-names/intro-1582575430.jpg')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (9, N'https://assets.website-files.com/60e60017e111d52aa5f88776/642c87f8663a5d441ea7c202_Social-Reveal-B-16_9-p-1080.jpg')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (10, N'https://i.kym-cdn.com/entries/icons/original/000/022/048/Revenge_of_the_Sith.jpg')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (11, N'https://e0.pxfuel.com/wallpapers/490/652/desktop-wallpaper-the-matrix-poster-the-matrix-movies-neo-keanu-reeves-flare.jpg')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (12, N'https://thehoganreviews.files.wordpress.com/2021/12/the-matrix-reloaded.jpg')
INSERT [dbo].[Image] ([Id], [Content]) VALUES (13, N'https://www.slantmagazine.com/wp-content/uploads/2003/11/matrixrevolutions.jpg')
GO
SET IDENTITY_INSERT [dbo].[Notification] ON 

INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (1, CAST(N'2023-06-22T18:30:25.2656107' AS DateTime2), N'avuksan@example.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.1217554' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (2, CAST(N'2023-06-22T19:58:19.5877253' AS DateTime2), N'mniksic@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.1591013' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (3, CAST(N'2023-06-22T20:00:12.8988670' AS DateTime2), N'pperich@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.1709674' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (4, CAST(N'2023-06-23T09:59:12.3657803' AS DateTime2), N'testich@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.1841652' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (5, CAST(N'2023-06-23T10:29:17.2810397' AS DateTime2), N'vversich@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.1989904' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (6, CAST(N'2023-06-24T11:37:10.8929761' AS DateTime2), N'1234@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.2111154' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (7, CAST(N'2023-06-25T11:28:37.6330955' AS DateTime2), N'tomo11@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.2267412' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (8, CAST(N'2023-06-25T11:31:35.0140596' AS DateTime2), N'per@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-25T13:42:39.2506433' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (9, CAST(N'2023-06-25T15:35:57.1408396' AS DateTime2), N'ante.vuksan94@gmail.com', N'testing', N'Info about sending mail', CAST(N'2023-06-25T13:42:39.2707171' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (11, CAST(N'2023-06-25T16:25:04.1367686' AS DateTime2), N'perica@gmail.com', N'Testing', N'12313', CAST(N'2023-06-25T14:30:21.5856473' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (12, CAST(N'2023-06-25T16:29:00.4479024' AS DateTime2), N'perica12312@gmail.com', N'Testing', N'12313', CAST(N'2023-06-25T14:30:21.6206223' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (13, CAST(N'2023-06-25T16:29:08.4000601' AS DateTime2), N'perica12312@gmail.com', N'Test122131ing', N'adsadasd', CAST(N'2023-06-25T14:30:21.6322490' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (14, CAST(N'2023-06-25T16:39:31.7941957' AS DateTime2), N'avuksna@example.com', N'asdas', N'adsad', CAST(N'2023-06-25T14:42:19.0462433' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (15, CAST(N'2023-06-25T16:43:43.3732836' AS DateTime2), N'avuksna@example.com', N'cvxcvxc', N'xcvxvcxvx', CAST(N'2023-06-25T14:45:22.3854312' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (16, CAST(N'2023-06-25T16:43:45.3685881' AS DateTime2), N'avuksna@example.com', N'cvxcvxc', N'xcvxvcxvx', CAST(N'2023-06-25T14:45:22.3947270' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (17, CAST(N'2023-06-25T16:49:50.8719551' AS DateTime2), N'1232@example.com', N'string', N'string', CAST(N'2023-06-25T14:49:55.1081860' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (18, CAST(N'2023-06-25T18:39:51.8730513' AS DateTime2), N'pperica@example.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', CAST(N'2023-06-26T10:53:53.8649556' AS DateTime2))
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (19, CAST(N'2023-06-26T12:37:07.6711540' AS DateTime2), N'ddaric@example.com', N'test', N'test', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (20, CAST(N'2023-06-26T15:27:55.9503275' AS DateTime2), N'ddanich@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (21, CAST(N'2023-06-26T15:31:36.8442813' AS DateTime2), N'krmpotich@example.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (22, CAST(N'2023-06-26T15:53:13.7417017' AS DateTime2), N'demiro@example.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (23, CAST(N'2023-06-26T16:25:37.0812720' AS DateTime2), N'mmarckich@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (24, CAST(N'2023-06-26T16:31:19.9918212' AS DateTime2), N'bozic@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (25, CAST(N'2023-06-26T16:42:09.8148728' AS DateTime2), N'dom@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (26, CAST(N'2023-06-26T16:56:58.4447695' AS DateTime2), N'ivich@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (27, CAST(N'2023-06-26T16:57:34.9596465' AS DateTime2), N'bbrunich@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (28, CAST(N'2023-06-26T17:00:26.0688324' AS DateTime2), N'lea@gmail.com', N'Account registration RWAMovies app', N'Folow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (29, CAST(N'2023-06-26T17:09:11.4235484' AS DateTime2), N'tanja@gmail.com', N'Account registration RWAMovies app', N'Follow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (30, CAST(N'2023-06-26T17:13:09.5796100' AS DateTime2), N'nedo@gmail.com', N'Account registration RWAMovies app', N'Follow this URL to validate your account: DUMMY URL', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (31, CAST(N'2023-06-26T17:17:31.3968102' AS DateTime2), N'red@gmail.com', N'Account registration RWAMovies app', N'User Successfuly created', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (32, CAST(N'2023-06-26T17:21:04.5388409' AS DateTime2), N'mili@gmail.com', N'Account registration RWAMovies app', N'User Successfuly created', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (33, CAST(N'2023-06-26T17:26:42.1740134' AS DateTime2), N'test1@gamil.com', N'Account registration RWAMovies app', N'User Successfuly created', NULL)
INSERT [dbo].[Notification] ([Id], [CreatedAt], [ReceiverEmail], [Subject], [Body], [SentAt]) VALUES (34, CAST(N'2023-06-26T17:30:55.9941578' AS DateTime2), N'dora@gmail.com', N'Account registration RWAMovies app', N'User Successfuly created', NULL)
SET IDENTITY_INSERT [dbo].[Notification] OFF
GO
SET IDENTITY_INSERT [dbo].[Tag] ON 

INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1, N'Suspense')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (2, N'Water')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (3, N'Testing')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (4, N'reset')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (5, N' industrial')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (8, N'Adding')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (9, N' Materials')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (17, N'Yeah')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (44, N'liu kang')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (45, N'raiden')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (47, N'liu kang')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (48, N'raiden')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (49, N'Sub zero')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (51, N' relaxing')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (52, N'mine')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (53, N' relaxing')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (54, N'jazz')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (55, N'music')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (56, N'jazz')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (57, N'music')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (58, N'Homemade')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (67, N'Summer Time')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (68, N'Carrot')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1069, N'Interesting')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1070, N'Summer time')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1073, N'NanoMachines')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1074, N'Fights')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1075, N'Neo')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1076, N'Machines')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1077, N'Fighting')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1080, N'Matrix 2')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1081, N'The Architect')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1083, N'Final part')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1084, N' battle')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1085, N' revolutions')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1086, N'Final part')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1087, N' battle')
INSERT [dbo].[Tag] ([Id], [Name]) VALUES (1088, N' revolutions')
SET IDENTITY_INSERT [dbo].[Tag] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (1, CAST(N'2023-06-22T18:30:25.1233363' AS DateTime2), NULL, N'avuksan', N'Ante', N'Vuksan', N'avuksan@example.com', N'9whR6ILKe8W0dRmw9WK1ULCbdaqmamhqc+AlCBM7AFw=', N'Vrzn7ZkVrw5mYZWBQkPxFA==', N'091 55 33 964', 1, N'zTQ6B5Wu9CgDQtBbAglDT3RAORwsvv+IjjkPUQY4AJk=', 8)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (4, CAST(N'2023-06-22T20:00:12.8924939' AS DateTime2), NULL, N'pperich', N'Perica', N'Perich', N'pperich@gmail.com', N'RKgAV7RSm8Dqofu3B8+4MjO5oQSBR7/30mDvwYb3ips=', N'x08zwrO9MLQfaYSqImP3Lw==', N'091 68 69 881', 0, N'L3XknizFqW1Yi3Uu+g54tJRTYQRq+tmMfr+Xr/9kgVM=', 6)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (7, CAST(N'2023-06-23T09:44:16.4397793' AS DateTime2), NULL, N'mick', N'Micho', N'Micki', N'mick@gmail.com', N'Br4LbK86RCrg+4QAzi+udM8KHAmyqCpIP3Ua6zBOB4w=', N'GbCPAc/o/j5w0LoZvAqjyg==', N'093 11 33 333', 0, N'p/oGwlbWP08OhtAqDB9nmIsj2jbrysVwd7+FaCrabmA=', 9)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (9, CAST(N'2023-06-23T09:59:11.8245100' AS DateTime2), CAST(N'2023-06-23T08:51:47.2912573' AS DateTime2), N'ttestich', N'Testo', N'Testic', N'testich@gmail.com', N'Fn44RqcHCQzhDLUQcIBQIxcfrRW3c5dM4RuPBQ0GwOQ=', N'5tn7UZpw8/Isez8iQcf1oA==', N'092 11 22 112', 0, N'YoOe1F8rjuxVeSOBelSVGZgn3SMOyouLURGOKcigUyw=', 7)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (10, CAST(N'2023-06-23T10:29:12.6614847' AS DateTime2), CAST(N'2023-06-23T08:48:22.6708363' AS DateTime2), N'vversich', N'Verso', N'Versich', N'vversich@gmail.com', N'qvWT1L4AOtoUjhKDV98D37UjOdazAikypk5nE8ZrKgU=', N'8nujtr1ZNjIXB4OvvQb0hg==', N'091 23 33 111', 1, N'BeskThiSDbAO9a1AryHBvpNPa9czI72sIjSUut8xWFY=', 6)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (11, CAST(N'2023-06-24T11:34:50.5690410' AS DateTime2), NULL, N'1234Daric', N'Dario', N'Darich', N'123darich@gmail.com', N'rVvCjbfz71wXihz2zW5/RHEuqsGMf5MSRdm40UQqPz4=', N'EU9psyidV/E8S7ipPOrBpw==', N'092 11 22 331', 0, N'UUFkwqh9GV1dEsxjRSoVS56p6XRpc6y7m6+uUtY1284=', 9)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (12, CAST(N'2023-06-24T11:37:08.5613587' AS DateTime2), NULL, N'1234', N'Dario', N'Darich', N'1234@gmail.com', N'eHneQQ9z38IRzcaWu3NnTmg5DDO8Xg1Qm2GbRlo07bk=', N'Cq3dUn4GSfpMKCrkK3XmiQ==', N'981 11 22 312', 0, N'4Ga9X0WUxTtStkEl0flwRksShNErcqvfXBGfjZn7MwY=', 8)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (13, CAST(N'2023-06-25T11:28:37.1483285' AS DateTime2), NULL, N'tomo111', N'Tomislav', N'Tomek', N'tomo11@gmail.com', N'3twTXGGeO75HXkdYhaNo10HEVZZs+ZWjpohpvFwOiFE=', N'vumM7KXAQDDH4N65zhL0oA==', N'092 11 44 111', 0, N'MnkTC4ceh9PwOlt0SfusZeLkvlZd4mujZ6wXv3Cc5+M=', 6)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (14, CAST(N'2023-06-25T11:31:34.7004749' AS DateTime2), NULL, N'petar', N'Petar', N'Petrich', N'per@gmail.com', N'KRK+9NWrLB+HqNRbLEnkLEws53QUF8YEFlnlGiAoYG0=', N'nCeu3FEKT8kcXzWYsUVcSw==', N'093 11 22 333', 1, N'3Sorw7eeKNKcevHrujGsYaYkGnS/xScoI2oG/3eCpmg=', 8)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (15, CAST(N'2023-06-25T18:39:51.5952639' AS DateTime2), CAST(N'2023-06-25T16:55:17.6502223' AS DateTime2), N'vida', N'Perica', N'Perich', N'pperica@example.com', N'QFu6omFXGLi+Npc8grcNJbmGIddPv10HGjmf4Mfoqp4=', N'7Ph0HHZTurcxUboia6VKwA==', N'092 11 33 115', 1, N'WVQyDmYxDYXfzR0lQFfokSxJ4LetcS2KGiwW5xQwLHQ=', 1)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (16, CAST(N'2023-06-26T15:27:55.6986736' AS DateTime2), NULL, N'ddanich', N'Danko', N'Danich', N'ddanich@gmail.com', N'4M8j/rj9aybJYh5G1o8JaFwNZavav2nJjg7TFDXTm98=', N'HSE5iqccS3lYnGEkxQJxPA==', N'091 22 11 441', 0, N'pqwb4AEKwOIEUh2tItn8H4i4n/dlb/KDPsq/8PxviRI=', 7)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (17, CAST(N'2023-06-26T15:31:36.8307465' AS DateTime2), NULL, N'krmpotich', N'Militca', N'Krmopitch', N'krmpotich@example.com', N'O6P31+QUEHNzdGXCm8Qk0QEDxF99Z9Uahl6EjwtU64M=', N'GugxsG3JuYCxc2f39Il2Vw==', N'098 22 55 112', 0, N'GCXQo0CCVTm8TbBPgwYCoKhXvYTOWqmxdQgqNB3uvNg=', 9)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (18, CAST(N'2023-06-26T15:53:13.2219784' AS DateTime2), NULL, N'demiro', N'Robert', N'DeMiro', N'demiro@example.com', N'pmWVqtruiT+BC4KJu9cIeWroK7LfNvpH5KnSYWeGgW0=', N'ZFhUStPtacZpq/OjBLY96g==', N'092 11 44 112', 1, N'vN1eHQQzYO1KFBNpzRAYetkNc9jklgiCI2UAiUSkCgs=', 7)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (19, CAST(N'2023-06-26T16:25:36.6006880' AS DateTime2), CAST(N'2023-06-26T14:25:55.4779901' AS DateTime2), N'mmarkich', N'Marko', N'Markich', N'mmarckich@gmail.com', N'E9/6i4DNExBbgEo6f2+WFUKaJqh4pFpjrKiW0bjhIdg=', N'a4lT4MH67Rp64nNaIG5WRQ==', N'099 46 11 987', 0, N'4ckmhTwSNCud32QkmCCJjJeHWYUcgAnkUE/iIzJSEVs=', 9)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (20, CAST(N'2023-06-26T16:31:19.9724963' AS DateTime2), CAST(N'2023-06-26T14:31:50.1479613' AS DateTime2), N'bozic', N'Bozo', N'Bozic', N'bozic@gmail.com', N'gk+jJnuaEU/yC6K8teJy5dxFrDGY6LX+W/qrtxxkiCI=', N'taAAyN+MU4AOUuEAZUuB3Q==', N'097 11 99 221', 1, N'E3ssY8xpGXXxZArEQEGHVVA+ryP5BZN+Fvh+ODXt+jU=', 6)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (21, CAST(N'2023-06-26T16:42:09.3324904' AS DateTime2), NULL, N'dom', N'Dominik', N'Domic', N'dom@gmail.com', N'95Uj1iUFxYyTnjZvKhdZ7jsOLtwUtz4FGXA9YeIq0n4=', N'zEXwdFYdHsYQtR8hMSsAcw==', N'092 11 22 989', 1, N'Ldq7vyBu+WcbniQfa6CLF+X8fIgB0MCtlS8fWwjTzGY=', 8)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (22, CAST(N'2023-06-26T16:56:58.4272103' AS DateTime2), NULL, N'ivich', N'Ivica', N'Ivich', N'ivich@gmail.com', N'Kf40vblRynp41spLnH+/nd235IWHuRYvrD+s914Ik1E=', N'v0TQg/3Sck17K2yQKPMN3Q==', N'092 11 31 221', 1, N'1dnwSwBL5adfo/AFhwhayRuiSg3SXoOOwEVJ7isfsms=', 9)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (23, CAST(N'2023-06-26T16:57:34.9491155' AS DateTime2), NULL, N'bbrunich', N'Bruno', N'Brunich', N'bbrunich@gmail.com', N'UOPXERknzs0NpByCUmruUQe3O6A9k2WT98pf8XpAq9k=', N'j+qCLpAWX56Y7ftQnSb6qA==', N'091 22 99 987', 0, N'W48PO5yXoLMqXHMHrX95Gy6KMXTRcFIBUS+6l99TesA=', 8)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (24, CAST(N'2023-06-26T17:00:25.7110365' AS DateTime2), NULL, N'lea', N'Lea', N'Leich', N'lea@gmail.com', N'JYROtSOj+sRsBaMYamPXXFOjM/7/oOpfh2Ug9bz75+o=', N'bhcGhVlyTpoGx7xuGftqtA==', N'096 11 78 221', 0, N'z02l2ql2SE2pkcMm+QA2pIGBKQvwg8zXmePobjfgi1k=', 1)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (25, CAST(N'2023-06-26T17:09:11.0370623' AS DateTime2), NULL, N'tanja', N'Tanja', N'Tanjich', N'tanja@gmail.com', N'OB12USQAaeW7nFlXJEDKQoN2bjJLLIbghjBvsFdjrVY=', N'JNQh1+p4LeahNQGD/ki0ng==', N'096 11 67 871', 1, N'0GWjjub2ejXkMiwlwyQRPp6HVZ0WCvX1l4mA3pEmmlA=', 1)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (26, CAST(N'2023-06-26T17:13:09.5571456' AS DateTime2), NULL, N'nedo', N'Nedo', N'Nedich', N'nedo@gmail.com', N'pAqdZE5AbYhYDcUAAws5bFKreLcxp1giNvkWyYL68vw=', N'AODGRjtpTQThfkRKetrZwQ==', N'094 11 22 984', 1, N'eRanfzZHcrh0mwq8gNldl1IUqvY5qd556q4AM7usCCM=', 1)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (27, CAST(N'2023-06-26T17:17:31.3773256' AS DateTime2), NULL, N'red', N'Rodo', N'Rodich', N'red@gmail.com', N'C7ej6S0tCL7hv8Lh9Cde3FXsq8XkNtdbppVr9MlLTVI=', N'GOHVfC3/5FLPoH4AlWmrzQ==', N'098 22 77 113', 1, N'uXefQrJNat2nOXwy/aqD1C5jU1TSEUk/usqVWORc3/E=', 1)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (28, CAST(N'2023-06-26T17:21:04.5204117' AS DateTime2), NULL, N'mili', N'Milica', N'Milich', N'mili@gmail.com', N'mPMSRLsIv7oFTnUtuorKBklSOhU4MQWI8sVCwPF6pdw=', N'VdqTCYqRBvmmAb/HyHyeOQ==', N'098 11 33 221', 1, N'mVoH5bA89NHZbIZGyLND04osLk78x9vIG937mo1irT8=', 1)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (29, CAST(N'2023-06-26T17:26:41.6419012' AS DateTime2), NULL, N'test1', N'Test1', N'Testing', N'test1@gamil.com', N't4wHCxIIicEKR2l5/2miLrv3sXsC3SzAKcmlt49Ldus=', N'UsDNBfSZ38nm9oBAW+qNAQ==', N'091 22 11 786', 1, N'xzQyqS0Cfvb5CXI4mr8y2HCGisUepX3gzecOmapg5es=', 1)
INSERT [dbo].[User] ([Id], [CreatedAt], [DeletedAt], [Username], [FirstName], [LastName], [Email], [PwdHash], [PwdSalt], [Phone], [IsConfirmed], [SecurityToken], [CountryOfResidenceId]) VALUES (30, CAST(N'2023-06-26T17:30:55.5154691' AS DateTime2), NULL, N'dora', N'Dora', N'Dorich', N'dora@gmail.com', N'gVROEgTSj+UHnCr8jiCmqfOoTBPzUwpI2DBlgoRiZ14=', N'+UyTskvJ3w+BXB5j0WSqaA==', N'093 11 22 331', 1, N'tw7D2qCFTz9FWUj0XDhJu0SRlb3emQAKorxzIXxuS/M=', 1)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Video] ON 

INSERT [dbo].[Video] ([Id], [CreatedAt], [Name], [Description], [GenreId], [TotalSeconds], [StreamingUrl], [ImageId]) VALUES (15, CAST(N'2023-06-24T14:14:03.4366667' AS DateTime2), N'Mortal Kombat', N'Released in 2021, "Mortal Kombat" revives the iconic fighting game franchise in a blockbuster spectacle. This explosive martial arts film introduces Cole Young, a washed-up MMA fighter unaware of his hidden lineage, who finds himself thrown into an interdimensional tournament - Mortal Kombat. As Earthrealm''s champions, Cole and a team of hardened warriors must battle against the Outworld''s deadliest fighters. Expect a barrage of jaw-dropping fatalities and brutal fight scenes true to the game''s legacy, alongside fan-favorite characters like Sub-Zero, Scorpion, and Liu Kang', 1, 5555666, N'https://www.youtube.com/watch?v=NYH2sLid0Zc&ab_channel=IGN', 3)
INSERT [dbo].[Video] ([Id], [CreatedAt], [Name], [Description], [GenreId], [TotalSeconds], [StreamingUrl], [ImageId]) VALUES (16, CAST(N'2023-06-24T14:16:37.4200000' AS DateTime2), N'Minecraft Movie', N'"Minecraft: The Movie" takes you on a thrilling journey through the limitless and imaginative world of Minecraft, the beloved sandbox video game. When an unexpected threat looms over the pixelated world, a group of unlikely adventurers must team up to save their blocky universe. ', 1, 54324, N'https://www.youtube.com/watch?v=Rla3FUlxJdE&ab_channel=Minecraft', 4)
INSERT [dbo].[Video] ([Id], [CreatedAt], [Name], [Description], [GenreId], [TotalSeconds], [StreamingUrl], [ImageId]) VALUES (17, CAST(N'2023-06-25T10:24:53.6933333' AS DateTime2), N'Sonic 2 Movie ', N' "Sonic the Hedgehog" is an exciting live-action adventure comedy that follows the journey of Sonic, the speedy blue hedgehog known from the iconic video game series. Forced to leave his home world, Sonic ends up on Earth and finds himself pursued by the evil Dr. Robotnik, who is hellbent on capturing Sonic to use his immense speed for world domination. With the help of his newfound human friend, Tom Wachowski, Sonic must dodge Robotnik''s traps, collect his rings, and save the world in this action-packed movie that is sure to delight fans of all ages.', 2, 22312, N'https://www.youtube.com/watch?v=47r8FXYZWNU&ab_channel=ParamountPictures', 2)
INSERT [dbo].[Video] ([Id], [CreatedAt], [Name], [Description], [GenreId], [TotalSeconds], [StreamingUrl], [ImageId]) VALUES (22, CAST(N'2023-06-26T11:28:34.7366667' AS DateTime2), N'Summer Music vibes', N'Dive into the sizzling energy of the season with our "Summer Music Vibes" video. Let these upbeat tunes, paired with stunning visuals of sun-drenched beaches, clear blue skies, and swaying palm trees, transport you to a summer paradise. Whether you''re planning a poolside party or looking for a soundtrack to your lazy afternoons, this video delivers a seamless mix of pop, dance, and tropical beats that capture the carefree spirit of summer.', 1, 2342, N'https://www.youtube.com/watch?v=2iCQkkfkE7I&ab_channel=Ambition', 6)
INSERT [dbo].[Video] ([Id], [CreatedAt], [Name], [Description], [GenreId], [TotalSeconds], [StreamingUrl], [ImageId]) VALUES (25, CAST(N'2023-06-26T12:33:57.8233333' AS DateTime2), N'The Matrix', N'The first Matrix movie is a groundbreaking science fiction film that takes place in a dystopian future. Set in a world where machines have taken over, humanity is enslaved and trapped within a virtual reality known as the Matrix. The story follows Neo, a computer hacker who becomes aware of the Matrix''s existence and joins a group of rebels led by Morpheus to fight against the machines. ', 1, 12000, N'https://www.youtube.com/watch?v=vKQi3bBA1y8&ab_channel=RottenTomatoesClassicTrailers', 11)
INSERT [dbo].[Video] ([Id], [CreatedAt], [Name], [Description], [GenreId], [TotalSeconds], [StreamingUrl], [ImageId]) VALUES (26, CAST(N'2023-06-26T12:39:31.9966667' AS DateTime2), N'Matrix Reloaded', N'"The second Matrix movie, titled ''The Matrix Reloaded,'' continues the gripping saga set in a dystopian future where machines rule over humanity. Neo, the chosen one, now faces even greater challenges as he delves deeper into the truth of the Matrix. Joined by his allies, including Trinity and Morpheus, Neo must confront the Architect and unravel the mysteries surrounding the prophecy of his role. As the rebels mount an all-out war against the machines, thrilling action sequences and mind-bending visuals captivate audiences.', 1, 30000, N'https://www.youtube.com/watch?v=kYzz0FSgpSU&ab_channel=RottenTomatoesClassicTrailers', 12)
INSERT [dbo].[Video] ([Id], [CreatedAt], [Name], [Description], [GenreId], [TotalSeconds], [StreamingUrl], [ImageId]) VALUES (27, CAST(N'2023-06-26T15:44:27.5700000' AS DateTime2), N'Matrix Revolutions', N'"The Matrix Revolutions" is the third film in the Matrix series, released in 2003, and continues the story from where "The Matrix Reloaded" left off. In this movie, Neo, played by Keanu Reeves, along with human rebels, continue their battle against the Machines that control the Matrix. A significant portion of the narrative unfolds in the real world, as opposed to the Matrix, focusing on a climactic battle to protect the human city of Zion. Meanwhile, inside the Matrix, Neo confronts the rogue program Smith in a decisive showdown. ', 1, 36000, N'https://www.youtube.com/watch?v=hMbexEPAOQI&ab_channel=RottenTomatoesClassicTrailers', 13)
SET IDENTITY_INSERT [dbo].[Video] OFF
GO
SET IDENTITY_INSERT [dbo].[VideoTag] ON 

INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (21, 15, 47)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (22, 15, 48)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (23, 15, 49)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (24, 16, 52)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (25, 16, 53)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1034, 22, 1069)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1036, 25, 1075)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1037, 25, 1076)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1038, 25, 1077)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1039, 26, 1080)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1040, 26, 1081)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1041, 27, 1086)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1042, 27, 1087)
INSERT [dbo].[VideoTag] ([Id], [VideoId], [TagId]) VALUES (1043, 27, 1088)
SET IDENTITY_INSERT [dbo].[VideoTag] OFF
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsConfirmed]  DEFAULT ((0)) FOR [IsConfirmed]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_TotalSeconds]  DEFAULT ((0)) FOR [TotalSeconds]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Country] FOREIGN KEY([CountryOfResidenceId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Country]
GO
ALTER TABLE [dbo].[Video]  WITH CHECK ADD  CONSTRAINT [FK_Video_Genre] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([Id])
GO
ALTER TABLE [dbo].[Video] CHECK CONSTRAINT [FK_Video_Genre]
GO
ALTER TABLE [dbo].[Video]  WITH CHECK ADD  CONSTRAINT [FK_Video_Images] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Image] ([Id])
GO
ALTER TABLE [dbo].[Video] CHECK CONSTRAINT [FK_Video_Images]
GO
ALTER TABLE [dbo].[VideoTag]  WITH CHECK ADD  CONSTRAINT [FK_VideoTag_Tag] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[VideoTag] CHECK CONSTRAINT [FK_VideoTag_Tag]
GO
ALTER TABLE [dbo].[VideoTag]  WITH CHECK ADD  CONSTRAINT [FK_VideoTag_Video] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Video] ([Id])
GO
ALTER TABLE [dbo].[VideoTag] CHECK CONSTRAINT [FK_VideoTag_Video]
GO
USE [master]
GO
ALTER DATABASE [RwaMovies] SET  READ_WRITE 
GO
