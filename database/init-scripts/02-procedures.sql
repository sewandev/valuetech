USE ValueTechDB;
GO

-- SP: Obtener todas las Regiones
CREATE OR ALTER PROCEDURE [dbo].[sp_Region_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRegion, Region FROM Region;
END
GO

-- SP: Obtener Comunas por Region
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

-- SP: Obtener Comuna por ID
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

-- SP: Actualizar Comuna usando MERGE (REGLA MANDATORIA)
-- Nota: La regla pide MERGE para "La actualización". 
-- Se diseñó para recibir datos y actualizar la fila correspondiente.
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
            
    -- No se implementa WHEN NOT MATCHED INSERT porque Id es Identity y el verbo es Update
    -- pero el MERGE es obligatorio por regla.
END
GO
