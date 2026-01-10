using Moq;
using ValueTech.Api.Services;
using ValueTech.Data.Interfaces;
using ValueTech.Data.Models;

namespace ValueTech.Tests.Services
{
    public class ComunaServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ShouldParseXmlAttributes_WhenXmlIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IComunaRepository>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ComunaService>>();
            var comunaId = 1;
            var rawXml = @"<Info>
                             <Superficie>150.5</Superficie>
                             <Poblacion Densidad=""10.5"">5000</Poblacion>
                           </Info>";
            
            var comunaEntity = new Comuna
            {
                IdComuna = comunaId,
                Nombre = "Comuna Test",
                InformacionAdicional = rawXml
            };

            mockRepo.Setup(repo => repo.GetByIdAsync(comunaId))
                .ReturnsAsync(comunaEntity);

            var service = new ComunaService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.GetByIdAsync(comunaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(150.5m, result.Superficie);
            Assert.Equal(5000, result.Poblacion);
            Assert.Equal(10.5m, result.Densidad);
            Assert.Equal("Comuna Test", result.Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDefaultValues_WhenXmlIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IComunaRepository>();
            var comunaId = 2;
            var invalidXml = "<Info>Bad Xml"; // XML Malformado

            var comunaEntity = new Comuna
            {
                IdComuna = comunaId,
                Nombre = "Comuna Fallida",
                InformacionAdicional = invalidXml
            };

            mockRepo.Setup(repo => repo.GetByIdAsync(comunaId))
                .ReturnsAsync(comunaEntity);

            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ComunaService>>();
            var service = new ComunaService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.GetByIdAsync(comunaId);

            // Assert
            Assert.NotNull(result);
            // Si falla el parseo, el try-catch devuelve el objeto con valores default (0)
            Assert.Equal(0, result.Poblacion);
            Assert.Equal("Comuna Fallida", result.Nombre);
        }

        [Fact]
        public async Task UpdateAsync_ShouldGenerateCorrectXmlStructure()
        {
            // Arrange
            var mockRepo = new Mock<IComunaRepository>();
            var comunaId = 5;
            var request = new Api.Contracts.Requests.CreateComunaRequest
            {
                IdComuna = comunaId,
                IdRegion = 1,
                Nombre = "Comuna Update",
                Superficie = 200.5m,
                Poblacion = 10000,
                Densidad = 50.0m
            };

            // Capturamos el objeto que llega al repositorio para inspeccionar el XML
            Comuna capturedEntity = null;
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Comuna>()))
                .Callback<Comuna>(c => capturedEntity = c)
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ComunaService>>();
            var service = new ComunaService(mockRepo.Object, mockLogger.Object);

            // Act
            await service.UpdateAsync(comunaId, request);

            // Assert
            Assert.NotNull(capturedEntity);
            Assert.Equal("Comuna Update", capturedEntity.Nombre);
            
            // Validamos estructura XML
            var xml = System.Xml.Linq.XElement.Parse(capturedEntity.InformacionAdicional);
            Assert.Equal("Info", xml.Name.LocalName);
            Assert.Equal("200.5", xml.Element("Superficie")?.Value);
            Assert.Equal("10000", xml.Element("Poblacion")?.Value);
            Assert.Equal("50.0", xml.Element("Poblacion")?.Attribute("Densidad")?.Value);
        }
    }
}
