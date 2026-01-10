USE ValueTechDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Auditoria')
BEGIN
    CREATE TABLE [dbo].[Auditoria](
        [IdAuditoria] [int] IDENTITY(1,1) NOT NULL,
        [Fecha] [datetime] NOT NULL DEFAULT GETDATE(),
        [Usuario] [nvarchar](50) NOT NULL,
        [IpAddress] [nvarchar](50) NOT NULL,
        [Accion] [nvarchar](50) NOT NULL,
        [Detalle] [nvarchar](MAX) NULL,
     CONSTRAINT [PK_Auditoria] PRIMARY KEY CLUSTERED 
    (
        [IdAuditoria] ASC
    )
    )
END
GO
