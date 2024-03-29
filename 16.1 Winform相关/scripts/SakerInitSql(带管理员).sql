USE [Saker]
GO
/****** Object:  Table [dbo].[Data_Calibration_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Data_Calibration_Info](
	[GUID] [varchar](36) NOT NULL,
	[Name] [varchar](32) NULL,
	[IP] [varchar](32) NULL,
	[SerialNumber] [varchar](32) NULL,
	[DeviceDelayTime] [int] NULL,
	[ChanDelayCalTime] [datetime] NULL,
	[DevDelayCalTime] [datetime] NULL,
	[LineLength] [varchar](32) NULL,
 CONSTRAINT [PK_Data_Calibration_Info] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[View_Data_Calibration]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_Data_Calibration]
AS
SELECT   A.Name, B.SerialNumber, B.DevDelayCalTime, B.ChanDelayCalTime, A.DeviceDelayTime,A.LineLength
FROM      (SELECT   SerialNumber, MAX(DevDelayCalTime) AS DevDelayCalTime, MAX(ChanDelayCalTime) 
                                 AS ChanDelayCalTime
                 FROM      dbo.Data_Calibration_Info
                 GROUP BY SerialNumber) AS B LEFT OUTER JOIN
                dbo.Data_Calibration_Info AS A ON A.SerialNumber = B.SerialNumber AND A.DevDelayCalTime = B.DevDelayCalTime



GO
/****** Object:  Table [dbo].[Config_Channel]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_Channel](
	[GUID] [varchar](36) NOT NULL,
	[DeviceGUID] [varchar](36) NULL,
	[ChannelID] [int] NULL,
	[Collect] [varchar](32) NULL,
	[Record] [varchar](32) NULL,
	[Valid] [varchar](32) NULL,
	[Open] [varchar](32) NULL,
	[TriggerPosition] [varchar](32) NULL,
	[Tag] [varchar](32) NULL,
	[TagDesc] [varchar](32) NULL,
	[MeasureType] [varchar](32) NULL,
	[Scale] [varchar](32) NULL,
	[Offset] [varchar](32) NULL,
	[Impedance] [varchar](32) NULL,
	[Coupling] [varchar](32) NULL,
	[ProbeRatio] [varchar](32) NULL,
	[BchanInv] [varchar](32) NULL,
	[BchanImpedance] [varchar](32) NULL,
	[BchanBWLimit] [varchar](32) NULL,
	[CreateTime] [datetime] NULL,
	[Xincrement] [varchar](32) NULL,
	[Xreference] [varchar](32) NULL,
	[Xorigin] [varchar](32) NULL,
	[Yincrement] [varchar](32) NULL,
	[Yreference] [varchar](32) NULL,
	[Yorigin] [varchar](32) NULL,
	[ChannelDelayTime] [varchar](32) NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_Device_All]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_Device_All](
	[GUID] [varchar](36) NOT NULL,
	[ProjectGUID] [varchar](36) NULL,
	[TriggerSource] [varchar](32) NULL,
	[TriggerMode] [varchar](32) NULL,
	[HorizontalOffset] [varchar](32) NULL,
	[HorizontalTimebase] [varchar](32) NULL,
	[TriggerLevel] [varchar](32) NULL,
	[Memdepth] [varchar](32) NULL,
	[HoldOff] [varchar](32) NULL,
	[IP] [varchar](32) NULL,
	[CreateTime] [datetime] NULL,
	[WaveTableName] [varchar](100) NULL,
 CONSTRAINT [PK_Config_Device_All] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_Record]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_Record](
	[GUID] [varchar](36) NOT NULL,
	[ProjectGUID] [varchar](36) NULL,
	[CollectGUID] [varchar](36) NULL,
	[CollectCondition] [varchar](50) NULL,
	[AbsTime] [datetime] NULL,
	[RecordLocation] [varchar](50) NULL,
	[FileType] [varchar](50) NULL,
	[FileLocation] [varchar](100) NULL,
	[FileNameRule] [int] NULL,
	[FileNameRuleDesc] [varchar](50) NULL,
	[IsAddDateTime] [varchar](5) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Config_Record] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_View]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_View](
	[GUID] [varchar](36) NOT NULL,
	[GroupName] [varchar](50) NULL,
	[CollectGUID] [varchar](36) NULL,
	[ProjectGUID] [varchar](36) NULL,
	[Tag] [varchar](50) NULL,
	[ScaleMin] [varchar](50) NULL,
	[ScaleMax] [varchar](50) NULL,
	[WaveColor] [varchar](50) NULL,
	[WaveType] [varchar](50) NULL,
	[IsShow] [varchar](10) NULL,
	[CreateTime] [datetime] NULL,
	[ID] [varchar](50) NULL,
	[No] [varchar](50) NULL,
	[Y] [varchar](50) NULL,
	[IsChoose] [varchar](10) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Congfig_Storage]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Congfig_Storage](
	[GUID] [varchar](36) NOT NULL,
	[CollectCondition] [nvarchar](32) NULL,
	[RecordPosition] [nvarchar](32) NULL,
	[FileType] [nvarchar](32) NULL,
	[Location] [nvarchar](32) NULL,
	[FileName] [nvarchar](32) NULL,
	[AddDate] [char](1) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Congfig_Storage] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Data_Device_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Data_Device_Info](
	[GUID] [varchar](36) NOT NULL,
	[CollectGUID] [varchar](36) NULL,
	[Name] [varchar](32) NULL,
	[ServerIP] [varchar](32) NULL,
	[IP] [varchar](32) NULL,
	[SerialNumber] [varchar](32) NULL,
	[MAC] [varchar](32) NULL,
	[SoftVersion] [varchar](32) NULL,
	[Model] [varchar](32) NULL,
	[VirtualNumber] [varchar](32) NULL,
	[Channel] [int] NULL,
	[Status] [varchar](32) NULL,
	[DelayTime] [varchar](32) NULL,
	[SampleRate] [varchar](32) NULL,
	[CreateTime] [datetime] NULL,
	[ChannelModel] [int] NULL,
 CONSTRAINT [PK_Data_Device_Info] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Data_HardWare_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Data_HardWare_Info](
	[GUID] [varchar](36) NOT NULL,
	[Column_1] [varchar](256) NULL,
	[Column_2] [varchar](256) NULL,
	[Column_3] [varchar](256) NULL,
	[Column_4] [varchar](256) NULL,
	[Column_5] [varchar](256) NULL,
	[Column_6] [varchar](256) NULL,
	[Column_7] [varchar](256) NULL,
	[Column_8] [varchar](256) NULL,
	[Column_9] [varchar](256) NULL,
 CONSTRAINT [PK_Data_HardWare_Info] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Data_Project_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Data_Project_Info](
	[GUID] [varchar](36) NOT NULL,
	[Name] [varchar](100) NULL,
	[Location] [varchar](100) NULL,
	[Remark] [varchar](500) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Data_Project_Info] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Data_SoftWare_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Data_SoftWare_Info](
	[GUID] [varchar](36) NOT NULL,
	[Column_1] [varchar](256) NULL,
	[Column_2] [varchar](256) NULL,
	[Column_3] [varchar](256) NULL,
	[Column_4] [varchar](256) NULL,
	[Column_5] [varchar](256) NULL,
	[Column_6] [varchar](256) NULL,
	[Column_7] [varchar](256) NULL,
	[Column_8] [varchar](256) NULL,
	[Column_9] [varchar](256) NULL,
 CONSTRAINT [PK_Data_SoftWare_Info] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Data_Waveform_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Data_Waveform_Info](
	[GUID] [varchar](36) NOT NULL,
	[DeviceGUID] [varchar](36) NULL,
	[ChannelGUID] [varchar](36) NULL,
	[Data] [image] NULL,
	[Location] [varchar](100) NULL,
	[StartTime] [datetime] NULL,
	[IsWholeComplete] [varchar](10) NULL,
	[CreateTime] [datetime] NOT NULL,
	[TrigTimeStamp] [varchar](32) NULL,
	[ChanDelayTime] [varchar](32) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sys_Collect_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Collect_Info](
	[GUID] [varchar](36) NOT NULL,
	[CollectGUID] [varchar](36) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Sys_Collect_Info] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sys_Permission_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Permission_Info](
	[GUID] [varchar](36) NULL,
	[ID] [int] NULL,
	[RoleID] [int] NULL,
	[RoleName] [varchar](32) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sys_User_Info]    Script Date: 2020/9/4 9:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_User_Info](
	[GUID] [varchar](36) NOT NULL,
	[UserName] [varchar](32) NULL,
	[Password] [varchar](32) NULL,
	[Role] [int] NULL,
	[IsUse] [char](1) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Sys_User_Info] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Sys_Permission_Info] ([GUID], [ID], [RoleID], [RoleName]) VALUES (N'483DA241-EE73-4BA3-81EC-313F7A739732', 0, 0, N'管理员')
INSERT [dbo].[Sys_Permission_Info] ([GUID], [ID], [RoleID], [RoleName]) VALUES (N'E1207B51-12DA-48A9-BEAD-BB4D4BF3749D', 1, 1, N'访客')
INSERT [dbo].[Sys_User_Info] ([GUID], [UserName], [Password], [Role], [IsUse], [CreateTime]) VALUES (N'5b303228-e17a-4590-b3cd-734242388295', N'vist', N'001', 1, N'1', CAST(N'2020-08-28T18:33:00.237' AS DateTime))
INSERT [dbo].[Sys_User_Info] ([GUID], [UserName], [Password], [Role], [IsUse], [CreateTime]) VALUES (N'740178f2-170e-4484-b839-8e42adbdce5b', N'admin', N'admin', 0, N'1', CAST(N'2020-08-17T20:18:14.150' AS DateTime))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "B"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 127
               Right = 234
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 272
               Bottom = 146
               Right = 468
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Data_Calibration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Data_Calibration'
GO
