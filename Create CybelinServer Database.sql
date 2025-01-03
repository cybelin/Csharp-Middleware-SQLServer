USE [CybelinServer]
GO
/****** Object:  Table [dbo].[BlacklistedIps]    Script Date: 10/31/2024 4:54:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlacklistedIps](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IpAddress] [nvarchar](max) NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_BlacklistedIps] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configurations]    Script Date: 10/31/2024 4:54:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Configurations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestLogs]    Script Date: 10/31/2024 4:54:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLogs](
	[RequestLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestId] [uniqueidentifier] NOT NULL,
	[HttpMethod] [nvarchar](max) NULL,
	[RequestPath] [nvarchar](max) NULL,
	[QueryString] [nvarchar](max) NULL,
	[RequestHeaders] [nvarchar](max) NULL,
	[ClientIp] [nvarchar](max) NULL,
	[UserAgent] [nvarchar](max) NULL,
	[RequestTime] [datetime2](7) NOT NULL,
	[HttpVersion] [nvarchar](max) NULL,
	[RequestBody] [nvarchar](max) NULL,
 CONSTRAINT [PK_RequestLogs] PRIMARY KEY CLUSTERED 
(
	[RequestLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResponseLogs]    Script Date: 10/31/2024 4:54:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResponseLogs](
	[ResponseLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestId] [uniqueidentifier] NOT NULL,
	[StatusCode] [int] NOT NULL,
	[ResponseHeaders] [nvarchar](max) NULL,
	[ResponseTime] [datetime2](7) NOT NULL,
	[DurationMs] [bigint] NOT NULL,
	[ServerIp] [nvarchar](max) NULL,
	[ResponseSizeInBytes] [bigint] NOT NULL,
 CONSTRAINT [PK_ResponseLogs] PRIMARY KEY CLUSTERED 
(
	[ResponseLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ResponseLogs] ADD  DEFAULT (CONVERT([bigint],(0))) FOR [ResponseSizeInBytes]
GO
/****** Object:  StoredProcedure [dbo].[GetLogsAfterDate]    Script Date: 10/31/2024 4:54:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetLogsAfterDate]
    @RequestTimeFilter DATETIME2(7)
AS
BEGIN
    SELECT 
        rl.RequestLogId,
        rl.RequestId,
        rl.HttpMethod,
        rl.RequestPath,
        rl.QueryString,
        rl.RequestHeaders,
        rl.ClientIp,
        rl.UserAgent,
        rl.RequestTime,
        rl.HttpVersion,
        rl.RequestBody,
        resl.ResponseLogId,
        resl.StatusCode,
        resl.ResponseHeaders,
        resl.ResponseTime,
        resl.DurationMs,
        resl.ServerIp,
        resl.ResponseSizeInBytes
    FROM 
        dbo.RequestLogs rl
    LEFT JOIN 
        dbo.ResponseLogs resl
    ON 
        rl.RequestId = resl.RequestId
    WHERE 
        rl.RequestTime >= @RequestTimeFilter
    ORDER BY 
        rl.RequestLogId;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetLogsFilteredByTimeAndClientIp]    Script Date: 10/31/2024 4:54:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetLogsFilteredByTimeAndClientIp]
    @RequestTimeFilter DATETIME2(7),
    @ClientIp NVARCHAR(MAX)
AS
BEGIN
    -- Seleccionamos los campos de las dos tablas unidas por RequestId
    SELECT 
        r.RequestLogId,
        r.RequestId,
        r.HttpMethod,
        r.RequestPath,
        r.QueryString,
        r.RequestHeaders,
        r.ClientIp,
        r.UserAgent,
        r.RequestTime,
        r.HttpVersion,
        r.RequestBody,
        res.ResponseLogId,
        res.StatusCode,
        res.ResponseHeaders,
        res.ResponseTime,
        res.DurationMs,
        res.ServerIp,
        res.ResponseSizeInBytes
    FROM dbo.RequestLogs r
    LEFT JOIN dbo.ResponseLogs res
        ON r.RequestId = res.RequestId
    WHERE r.RequestTime >= @RequestTimeFilter
      AND r.ClientIp = @ClientIp
	ORDER BY r.RequestLogId;
END;
GO

USE [CybelinServer]
GO
SET IDENTITY_INSERT [dbo].[Configurations] ON 

INSERT [dbo].[Configurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (2, N'MaliciousIpCheckIntervalInSeconds', N'60', CAST(N'2024-10-12T04:15:57.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Configurations] OFF
GO
