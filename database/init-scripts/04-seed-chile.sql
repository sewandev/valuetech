SET NOCOUNT ON;
DECLARE @IdReg INT;
SELECT @IdReg = IdRegion FROM Region WHERE Region = 'Arica y Parinacota';
IF @IdReg IS NULL
BEGIN
    INSERT INTO Region (Region) VALUES ('Arica y Parinacota');
    SET @IdReg = SCOPE_IDENTITY();
END

IF NOT EXISTS (SELECT 1 FROM Comuna WHERE IdRegion = @IdReg AND Comuna = 'Arica')
INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg, 'Arica', '<Info><Superficie>4799.4</Superficie><Poblacion Densidad="47.1">226068</Poblacion></Info>');

IF NOT EXISTS (SELECT 1 FROM Comuna WHERE IdRegion = @IdReg AND Comuna = 'Camarones')
INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg, 'Camarones', '<Info><Superficie>3927.0</Superficie><Poblacion Densidad="0.3">1255</Poblacion></Info>');

IF NOT EXISTS (SELECT 1 FROM Comuna WHERE IdRegion = @IdReg AND Comuna = 'Putre')
INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg, 'Putre', '<Info><Superficie>5902.5</Superficie><Poblacion Densidad="0.5">2765</Poblacion></Info>');

IF NOT EXISTS (SELECT 1 FROM Comuna WHERE IdRegion = @IdReg AND Comuna = 'General Lagos')
INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg, 'General Lagos', '<Info><Superficie>2244.4</Superficie><Poblacion Densidad="0.3">684</Poblacion></Info>');
SET @IdReg = NULL;
SELECT @IdReg = IdRegion FROM Region WHERE Region = 'Tarapacá';
IF @IdReg IS NULL
BEGIN
    INSERT INTO Region (Region) VALUES ('Tarapacá');
    SET @IdReg = SCOPE_IDENTITY();
END

IF NOT EXISTS (SELECT 1 FROM Comuna WHERE IdRegion = @IdReg AND Comuna = 'Iquique')
INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg, 'Iquique', '<Info><Superficie>2242.1</Superficie><Poblacion Densidad="85.4">191468</Poblacion></Info>');
