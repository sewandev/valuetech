USE ValueTechDB;
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Region_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRegion, Region FROM Region;
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Region_GetById]
    @IdRegion int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRegion, Region FROM Region
    WHERE IdRegion = @IdRegion;
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Comuna_GetByRegion]
    @IdRegion int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdComuna, IdRegion, Comuna, InformacionAdicional
    FROM Comuna
    WHERE IdRegion = @IdRegion;
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Comuna_GetById]
    @IdComuna int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdComuna, IdRegion, Comuna, InformacionAdicional
    FROM Comuna
    WHERE IdComuna = @IdComuna;
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Comuna_Update]
    @IdComuna int,
    @IdRegion int,
    @Comuna nvarchar(128),
    @InformacionAdicional xml
AS
BEGIN
    SET NOCOUNT ON;

    MERGE [dbo].[Comuna] AS target
    USING (SELECT @IdComuna AS IdVal) AS source
    ON (target.IdComuna = source.IdVal)
    WHEN MATCHED THEN
        UPDATE SET 
            IdRegion = @IdRegion,
            Comuna = @Comuna,
            InformacionAdicional = @InformacionAdicional;
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Region_Delete]
    @IdRegion int
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Comuna WHERE IdRegion = @IdRegion)
    BEGIN
        RAISERROR('No se puede eliminar la región porque tiene comunas asociadas.', 16, 1);
        RETURN;
    END

    DELETE FROM Region WHERE IdRegion = @IdRegion;
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Comuna_Delete]
    @IdComuna int
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Comuna WHERE IdComuna = @IdComuna;
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Region_Create]
    @Region nvarchar(64),
    @NewId int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Region (Region) VALUES (@Region);
    SET @NewId = SCOPE_IDENTITY();
END
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Comuna_Create]
    @IdRegion int,
    @Comuna nvarchar(128),
    @InformacionAdicional xml,
    @NewId int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) 
    VALUES (@IdRegion, @Comuna, @InformacionAdicional);
    SET @NewId = SCOPE_IDENTITY();
END
GO
