USE ValueTechDB;
GO
IF NOT EXISTS (SELECT 1 FROM Region)
BEGIN
    SET IDENTITY_INSERT Region ON;
    INSERT INTO Region (IdRegion, Region) VALUES (1, 'Metropolitana de Santiago');
    INSERT INTO Region (IdRegion, Region) VALUES (2, 'Valparaíso');
    SET IDENTITY_INSERT Region OFF;
END
GO
IF NOT EXISTS (SELECT 1 FROM Comuna)
BEGIN
    INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional)
    VALUES (
        1, 
        'Santiago', 
        '<Info><Superficie>22.4</Superficie><Poblacion Densidad="17898.4">404495</Poblacion></Info>'
    );
    INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional)
    VALUES (
        1, 
        'Providencia', 
        '<Info><Superficie>14.4</Superficie><Poblacion Densidad="9899.9">142079</Poblacion></Info>'
    );
    INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional)
    VALUES (
        2, 
        'Valparaíso', 
        '<Info><Superficie>401.6</Superficie><Poblacion Densidad="741.0">296655</Poblacion></Info>'
    );
END
GO
