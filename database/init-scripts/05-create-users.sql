USE ValueTechDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuario')
BEGIN
    CREATE TABLE [dbo].[Usuario](
        [IdUsuario] [int] IDENTITY(1,1) NOT NULL,
        [Username] [nvarchar](50) NOT NULL UNIQUE,
        [Password] [nvarchar](100) NOT NULL, -- En prod esto debe ser Hash, para la prueba es texto plano como solicitado
     CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
    (
        [IdUsuario] ASC
    )
    )

    -- Seed Admin
    INSERT INTO [dbo].[Usuario] (Username, Password) VALUES ('admin', 'admin123');
END
GO
