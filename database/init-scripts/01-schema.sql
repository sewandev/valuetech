USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ValueTechDB')
BEGIN
    CREATE DATABASE ValueTechDB;
END
GO

USE ValueTechDB;
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Region]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Region](
        [IdRegion] [int] IDENTITY(1,1) NOT NULL,
        [Region] [nvarchar](128) NOT NULL,
     CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
    (
        [IdRegion] ASC
    )
    )
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comuna]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Comuna](
        [IdComuna] [int] IDENTITY(1,1) NOT NULL,
        [IdRegion] [int] NOT NULL,
        [Comuna] [nvarchar](128) NOT NULL,
        [InformacionAdicional] [xml] NULL,
     CONSTRAINT [PK_Comuna] PRIMARY KEY CLUSTERED 
    (
        [IdComuna] ASC
    )
    )

    ALTER TABLE [dbo].[Comuna]  WITH CHECK ADD  CONSTRAINT [FK_Comuna_Region] FOREIGN KEY([IdRegion])
    REFERENCES [dbo].[Region] ([IdRegion])
    
    ALTER TABLE [dbo].[Comuna] CHECK CONSTRAINT [FK_Comuna_Region]
END
GO
