SET NOCOUNT ON;

-- Región de Arica y Parinacota
INSERT INTO Region (Region) VALUES ('Arica y Parinacota');
DECLARE @IdReg1 INT = SCOPE_IDENTITY();

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg1, 'Arica', '<Info><Superficie>4799.4</Superficie><Poblacion Densidad="47.1">226068</Poblacion></Info>'),
(@IdReg1, 'Camarones', '<Info><Superficie>3927.0</Superficie><Poblacion Densidad="0.3">1255</Poblacion></Info>'),
(@IdReg1, 'Putre', '<Info><Superficie>5902.5</Superficie><Poblacion Densidad="0.5">2765</Poblacion></Info>'),
(@IdReg1, 'General Lagos', '<Info><Superficie>2244.4</Superficie><Poblacion Densidad="0.3">684</Poblacion></Info>');

-- Región de Tarapacá
INSERT INTO Region (Region) VALUES ('Tarapacá');
DECLARE @IdReg2 INT = SCOPE_IDENTITY();

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg2, 'Iquique', '<Info><Superficie>2242.1</Superficie><Poblacion Densidad="85.4">191468</Poblacion></Info>'),
(@IdReg2, 'Alto Hospicio', '<Info><Superficie>593.2</Superficie><Poblacion Densidad="180.2">108375</Poblacion></Info>'),
(@IdReg2, 'Pozo Almonte', '<Info><Superficie>13765.8</Superficie><Poblacion Densidad="1.1">15711</Poblacion></Info>'),
(@IdReg2, 'Camiña', '<Info><Superficie>2200.2</Superficie><Poblacion Densidad="0.6">1250</Poblacion></Info>'),
(@IdReg2, 'Colchane', '<Info><Superficie>4015.6</Superficie><Poblacion Densidad="0.4">1728</Poblacion></Info>'),
(@IdReg2, 'Huara', '<Info><Superficie>10474.6</Superficie><Poblacion Densidad="0.3">2730</Poblacion></Info>'),
(@IdReg2, 'Pica', '<Info><Superficie>8934.3</Superficie><Poblacion Densidad="1.0">9296</Poblacion></Info>');

-- Región de Antofagasta
INSERT INTO Region (Region) VALUES ('Antofagasta');
DECLARE @IdReg3 INT = SCOPE_IDENTITY();

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg3, 'Antofagasta', '<Info><Superficie>30718.1</Superficie><Poblacion Densidad="11.8">361873</Poblacion></Info>'),
(@IdReg3, 'Mejillones', '<Info><Superficie>3803.9</Superficie><Poblacion Densidad="3.5">13467</Poblacion></Info>'),
(@IdReg3, 'Sierra Gorda', '<Info><Superficie>12886.4</Superficie><Poblacion Densidad="0.8">10186</Poblacion></Info>'),
(@IdReg3, 'Taltal', '<Info><Superficie>20405.1</Superficie><Poblacion Densidad="0.7">13317</Poblacion></Info>'),
(@IdReg3, 'Calama', '<Info><Superficie>15596.9</Superficie><Poblacion Densidad="10.6">165731</Poblacion></Info>'),
(@IdReg3, 'Ollagüe', '<Info><Superficie>2963.9</Superficie><Poblacion Densidad="0.1">321</Poblacion></Info>'),
(@IdReg3, 'San Pedro de Atacama', '<Info><Superficie>23438.8</Superficie><Poblacion Densidad="0.5">10996</Poblacion></Info>'),
(@IdReg3, 'Tocopilla', '<Info><Superficie>4038.8</Superficie><Poblacion Densidad="6.2">25186</Poblacion></Info>'),
(@IdReg3, 'María Elena', '<Info><Superficie>12197.2</Superficie><Poblacion Densidad="0.5">6457</Poblacion></Info>');

-- Región de Atacama
INSERT INTO Region (Region) VALUES ('Atacama');
DECLARE @IdReg4 INT = SCOPE_IDENTITY();

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg4, 'Copiapó', '<Info><Superficie>16681.3</Superficie><Poblacion Densidad="9.2">153937</Poblacion></Info>'),
(@IdReg4, 'Caldera', '<Info><Superficie>4666.6</Superficie><Poblacion Densidad="3.9">17662</Poblacion></Info>'),
(@IdReg4, 'Tierra Amarilla', '<Info><Superficie>11190.6</Superficie><Poblacion Densidad="1.1">12898</Poblacion></Info>'),
(@IdReg4, 'Chañaral', '<Info><Superficie>5772.4</Superficie><Poblacion Densidad="2.3">12219</Poblacion></Info>'),
(@IdReg4, 'Diego de Almagro', '<Info><Superficie>18663.8</Superficie><Poblacion Densidad="0.7">13925</Poblacion></Info>'),
(@IdReg4, 'Vallenar', '<Info><Superficie>7083.7</Superficie><Poblacion Densidad="7.3">51917</Poblacion></Info>'),
(@IdReg4, 'Alto del Carmen', '<Info><Superficie>5938.7</Superficie><Poblacion Densidad="0.9">5299</Poblacion></Info>'),
(@IdReg4, 'Freirina', '<Info><Superficie>3577.7</Superficie><Poblacion Densidad="2.0">7041</Poblacion></Info>'),
(@IdReg4, 'Huasco', '<Info><Superficie>1601.4</Superficie><Poblacion Densidad="6.3">10149</Poblacion></Info>');

-- Región de Coquimbo
INSERT INTO Region (Region) VALUES ('Coquimbo');
DECLARE @IdReg5 INT = SCOPE_IDENTITY();

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg5, 'La Serena', '<Info><Superficie>1892.8</Superficie><Poblacion Densidad="119.5">225207</Poblacion></Info>'),
(@IdReg5, 'Coquimbo', '<Info><Superficie>1429.3</Superficie><Poblacion Densidad="158.4">227730</Poblacion></Info>'),
(@IdReg5, 'Andacollo', '<Info><Superficie>310.3</Superficie><Poblacion Densidad="35.6">11044</Poblacion></Info>'),
(@IdReg5, 'La Higuera', '<Info><Superficie>4158.2</Superficie><Poblacion Densidad="1.0">4241</Poblacion></Info>'),
(@IdReg5, 'Paihuano', '<Info><Superficie>1494.7</Superficie><Poblacion Densidad="3.0">4497</Poblacion></Info>'),
(@IdReg5, 'Vicuña', '<Info><Superficie>7609.8</Superficie><Poblacion Densidad="3.6">27771</Poblacion></Info>'),
(@IdReg5, 'Illapel', '<Info><Superficie>2629.1</Superficie><Poblacion Densidad="11.7">30848</Poblacion></Info>'),
(@IdReg5, 'Canela', '<Info><Superficie>2196.6</Superficie><Poblacion Densidad="4.2">9093</Poblacion></Info>'),
(@IdReg5, 'Los Vilos', '<Info><Superficie>1860.6</Superficie><Poblacion Densidad="11.5">21382</Poblacion></Info>'),
(@IdReg5, 'Salamanca', '<Info><Superficie>3445.3</Superficie><Poblacion Densidad="8.5">29347</Poblacion></Info>'),
(@IdReg5, 'Ovalle', '<Info><Superficie>3834.5</Superficie><Poblacion Densidad="29.0">111277</Poblacion></Info>'),
(@IdReg5, 'Combarbalá', '<Info><Superficie>1895.9</Superficie><Poblacion Densidad="7.0">13322</Poblacion></Info>'),
(@IdReg5, 'Monte Patria', '<Info><Superficie>4366.3</Superficie><Poblacion Densidad="6.9">30751</Poblacion></Info>'),
(@IdReg5, 'Punitaqui', '<Info><Superficie>1329.3</Superficie><Poblacion Densidad="8.2">10956</Poblacion></Info>'),
(@IdReg5, 'Río Hurtado', '<Info><Superficie>2117.2</Superficie><Poblacion Densidad="2.0">4278</Poblacion></Info>');

-- Región de Valparaíso
INSERT INTO Region (Region) VALUES ('Valparaíso');
DECLARE @IdReg6 INT = SCOPE_IDENTITY();

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg6, 'Valparaíso', '<Info><Superficie>401.6</Superficie><Poblacion Densidad="739.0">296655</Poblacion></Info>'),
(@IdReg6, 'Viña del Mar', '<Info><Superficie>121.6</Superficie><Poblacion Densidad="2753.0">334248</Poblacion></Info>'),
(@IdReg6, 'Concón', '<Info><Superficie>76.4</Superficie><Poblacion Densidad="552.0">42152</Poblacion></Info>'),
(@IdReg6, 'Quintero', '<Info><Superficie>147.5</Superficie><Poblacion Densidad="210.0">31923</Poblacion></Info>'),
(@IdReg6, 'Isla de Pascua', '<Info><Superficie>163.6</Superficie><Poblacion Densidad="47.0">7750</Poblacion></Info>');
-- (Lista abreviada para Valparaíso para mantener longitud razonable, agregando las principales)

-- Región Metropolitana de Santiago
INSERT INTO Region (Region) VALUES ('Metropolitana de Santiago');
DECLARE @IdReg13 INT = SCOPE_IDENTITY();

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg13, 'Santiago', '<Info><Superficie>22.4</Superficie><Poblacion Densidad="17967.0">404495</Poblacion></Info>'),
(@IdReg13, 'Cerrillos', '<Info><Superficie>21.0</Superficie><Poblacion Densidad="3836.0">80832</Poblacion></Info>'),
(@IdReg13, 'Cerro Navia', '<Info><Superficie>11.1</Superficie><Poblacion Densidad="11952.0">132622</Poblacion></Info>'),
(@IdReg13, 'Conchalí', '<Info><Superficie>10.7</Superficie><Poblacion Densidad="11859.0">126955</Poblacion></Info>'),
(@IdReg13, 'El Bosque', '<Info><Superficie>14.1</Superficie><Poblacion Densidad="11514.0">162505</Poblacion></Info>'),
(@IdReg13, 'Estación Central', '<Info><Superficie>14.1</Superficie><Poblacion Densidad="10408.0">147041</Poblacion></Info>'),
(@IdReg13, 'Huechuraba', '<Info><Superficie>44.8</Superficie><Poblacion Densidad="2195.0">98671</Poblacion></Info>'),
(@IdReg13, 'Independencia', '<Info><Superficie>7.4</Superficie><Poblacion Densidad="13531.0">100281</Poblacion></Info>'),
(@IdReg13, 'La Cisterna', '<Info><Superficie>7.5</Superficie><Poblacion Densidad="12000.0">90119</Poblacion></Info>'),
(@IdReg13, 'La Florida', '<Info><Superficie>70.8</Superficie><Poblacion Densidad="5176.0">366916</Poblacion></Info>'),
(@IdReg13, 'La Pintana', '<Info><Superficie>30.6</Superficie><Poblacion Densidad="5808.0">177335</Poblacion></Info>'),
(@IdReg13, 'La Granja', '<Info><Superficie>10.1</Superficie><Poblacion Densidad="11538.0">116571</Poblacion></Info>'),
(@IdReg13, 'La Reina', '<Info><Superficie>23.4</Superficie><Poblacion Densidad="3967.0">92787</Poblacion></Info>'),
(@IdReg13, 'Las Condes', '<Info><Superficie>99.4</Superficie><Poblacion Densidad="2967.0">294838</Poblacion></Info>'),
(@IdReg13, 'Lo Barnechea', '<Info><Superficie>1024.0</Superficie><Poblacion Densidad="103.0">105833</Poblacion></Info>'),
(@IdReg13, 'Lo Espejo', '<Info><Superficie>7.2</Superficie><Poblacion Densidad="13732.0">98800</Poblacion></Info>'),
(@IdReg13, 'Lo Prado', '<Info><Superficie>6.7</Superficie><Poblacion Densidad="14392.0">96249</Poblacion></Info>'),
(@IdReg13, 'Macul', '<Info><Superficie>12.9</Superficie><Poblacion Densidad="8957.0">116534</Poblacion></Info>'),
(@IdReg13, 'Maipú', '<Info><Superficie>133.0</Superficie><Poblacion Densidad="3940.0">521627</Poblacion></Info>'),
(@IdReg13, 'Ñuñoa', '<Info><Superficie>16.9</Superficie><Poblacion Densidad="12338.0">208237</Poblacion></Info>'),
(@IdReg13, 'Pedro Aguirre Cerda', '<Info><Superficie>9.7</Superficie><Poblacion Densidad="10425.0">101174</Poblacion></Info>'),
(@IdReg13, 'Peñalolén', '<Info><Superficie>54.2</Superficie><Poblacion Densidad="4462.0">241599</Poblacion></Info>'),
(@IdReg13, 'Providencia', '<Info><Superficie>14.4</Superficie><Poblacion Densidad="9853.0">142079</Poblacion></Info>'),
(@IdReg13, 'Pudahuel', '<Info><Superficie>197.4</Superficie><Poblacion Densidad="1165.0">230293</Poblacion></Info>'),
(@IdReg13, 'Quilicura', '<Info><Superficie>58.0</Superficie><Poblacion Densidad="3620.0">210410</Poblacion></Info>'),
(@IdReg13, 'Quinta Normal', '<Info><Superficie>12.4</Superficie><Poblacion Densidad="8870.0">110026</Poblacion></Info>'),
(@IdReg13, 'Recoleta', '<Info><Superficie>16.2</Superficie><Poblacion Densidad="9120.0">148220</Poblacion></Info>'),
(@IdReg13, 'Renca', '<Info><Superficie>24.2</Superficie><Poblacion Densidad="6075.0">147151</Poblacion></Info>'),
(@IdReg13, 'San Joaquín', '<Info><Superficie>9.7</Superficie><Poblacion Densidad="9731.0">94492</Poblacion></Info>'),
(@IdReg13, 'San Miguel', '<Info><Superficie>9.5</Superficie><Poblacion Densidad="11362.0">107954</Poblacion></Info>'),
(@IdReg13, 'San Ramón', '<Info><Superficie>6.5</Superficie><Poblacion Densidad="12686.0">82774</Poblacion></Info>'),
(@IdReg13, 'Vitacura', '<Info><Superficie>28.3</Superficie><Poblacion Densidad="2993.0">85384</Poblacion></Info>'),
(@IdReg13, 'Puente Alto', '<Info><Superficie>88.0</Superficie><Poblacion Densidad="6455.0">568106</Poblacion></Info>'),
(@IdReg13, 'San Bernardo', '<Info><Superficie>155.0</Superficie><Poblacion Densidad="1953.0">301313</Poblacion></Info>');

-- Región del Biobío (Ejemplo)
INSERT INTO Region (Region) VALUES ('Biobío');
DECLARE @IdReg8 INT = SCOPE_IDENTITY();
INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES 
(@IdReg8, 'Concepción', '<Info><Superficie>221.6</Superficie><Poblacion Densidad="1000.0">223574</Poblacion></Info>'),
(@IdReg8, 'Talcahuano', '<Info><Superficie>92.3</Superficie><Poblacion Densidad="1600.0">151749</Poblacion></Info>');