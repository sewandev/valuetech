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
            var mockRepo = new Mock<IComunaRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
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

            var service = new ComunaService(mockRepo.Object, mockAudit.Object, mockLogger.Object);
            var result = await service.GetByIdAsync(comunaId);
            Assert.NotNull(result);
            Assert.Equal(150.5m, result.Superficie);
            Assert.Equal(5000, result.Poblacion);
            Assert.Equal(10.5m, result.Densidad);
            Assert.Equal("Comuna Test", result.Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDefaultValues_WhenXmlIsInvalid()
        {
            var mockRepo = new Mock<IComunaRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
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
            var service = new ComunaService(mockRepo.Object, mockAudit.Object, mockLogger.Object);
            var result = await service.GetByIdAsync(comunaId);
            Assert.NotNull(result);
            Assert.Equal(0, result.Poblacion);
            Assert.Equal("Comuna Fallida", result.Nombre);
        }

        [Fact]
        public async Task UpdateAsync_ShouldGenerateCorrectXmlStructure()
        {
            var mockRepo = new Mock<IComunaRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
            var comunaId = 5;
            var request = new Api.Contracts.Requests.UpdateComunaRequest
            {
                IdComuna = comunaId,
                IdRegion = 1,
                Nombre = "Comuna Update",
                Superficie = 200.5m,
                Poblacion = 10000,
                Densidad = 50.0m
            };
            Comuna? capturedEntity = null; // Fix CS8600
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Comuna>()))
                .Callback<Comuna>(c => capturedEntity = c)
                .Returns(Task.CompletedTask);
            mockAudit.Setup(a => a.AddAsync(It.IsAny<Auditoria>())).Returns(Task.CompletedTask);

            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ComunaService>>();
            var service = new ComunaService(mockRepo.Object, mockAudit.Object, mockLogger.Object);
            await service.UpdateAsync(comunaId, request, "TestUser", "127.0.0.1");
            Assert.NotNull(capturedEntity);
            Assert.Equal("Comuna Update", capturedEntity!.Nombre); // Fix CS8602 context
            Assert.NotNull(capturedEntity.InformacionAdicional); // Ensure not null before Parse
            var xml = System.Xml.Linq.XElement.Parse(capturedEntity.InformacionAdicional!); // Fix CS8604
            
            Assert.Equal("Info", xml.Name.LocalName);
            Assert.Equal("200.5", xml.Element("Superficie")?.Value);
            Assert.Equal("10000", xml.Element("Poblacion")?.Value);
            Assert.Equal("50.0", xml.Element("Poblacion")?.Attribute("Densidad")?.Value);
            mockAudit.Verify(a => a.AddAsync(It.Is<Auditoria>(x => 
                x.Usuario == "TestUser" && 
                x.IpAddress == "127.0.0.1" && 
                x.Accion == "UPDATE_COMUNA")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenSuperficieIsNegative()
        {
            var mockRepo = new Mock<IComunaRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ComunaService>>();
            var service = new ComunaService(mockRepo.Object, mockAudit.Object, mockLogger.Object);
            
            var request = new Api.Contracts.Requests.UpdateComunaRequest
            {
                IdComuna = 1,
                Superficie = -50 // Invalid
            };
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(1, request, "TestUser", "127.0.0.1"));
            Assert.Contains("superficie no puede ser negativa", ex.Message);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Comuna>()), Times.Never);
            mockAudit.Verify(a => a.AddAsync(It.IsAny<Auditoria>()), Times.Never);
        }
        [Fact]
        public async Task CreateAsync_ShouldCallRepoAndAudit_WhenRequestIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IComunaRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ComunaService>>();
            var service = new ComunaService(mockRepo.Object, mockAudit.Object, mockLogger.Object);

            var request = new Api.Contracts.Requests.CreateComunaRequest 
            { 
                IdRegion = 1,
                Nombre = "Nueva Comuna",
                Superficie = 100.5m,
                Poblacion = 5000,
                Densidad = 49.7m
            };
            var expectedNewId = 200;
            string testUser = "AdminUser";
            string testIp = "192.168.1.10";

            mockRepo.Setup(r => r.CreateAsync(It.IsAny<Comuna>()))
                .ReturnsAsync(expectedNewId);

            // Act
            var result = await service.CreateAsync(request, testUser, testIp);

            // Assert
            Assert.Equal(expectedNewId, result);

            mockRepo.Verify(r => r.CreateAsync(It.Is<Comuna>(c => 
                c.Nombre == request.Nombre &&
                c.IdRegion == request.IdRegion &&
                (c.InformacionAdicional ?? "").Contains(request.Superficie.Value.ToString(System.Globalization.CultureInfo.InvariantCulture))
            )), Times.Once);

            mockAudit.Verify(a => a.AddAsync(It.Is<Auditoria>(x => 
                x.Usuario == testUser && 
                x.IpAddress == testIp && 
                x.Accion == "CREATE_COMUNA"
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepoAndAudit_WhenExistingId()
        {
            // Arrange
            var mockRepo = new Mock<IComunaRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ComunaService>>();
            var service = new ComunaService(mockRepo.Object, mockAudit.Object, mockLogger.Object);

            int comunaId = 50;
            string testUser = "AdminUser";
            string testIp = "10.0.0.1";

            // Act
            await service.DeleteAsync(comunaId, testUser, testIp);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(comunaId), Times.Once);

            mockAudit.Verify(a => a.AddAsync(It.Is<Auditoria>(x => 
                x.Usuario == testUser && 
                x.IpAddress == testIp && 
                x.Accion == "DELETE_COMUNA" &&
                x.Detalle.Contains(comunaId.ToString())
            )), Times.Once);
        }
    }
}
