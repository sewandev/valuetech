using Moq;
using ValueTech.Api.Services;
using ValueTech.Data.Interfaces;
using ValueTech.Data.Models;
using Xunit;

namespace ValueTech.Tests.Services
{
    public class RegionServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldCallRepoAndAudit_WhenRequestIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IRegionRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<RegionService>>();
            var service = new RegionService(mockRepo.Object, mockAudit.Object, mockLogger.Object);

            var request = new Api.Contracts.Requests.CreateRegionRequest { Nombre = "Nueva Región" };
            var expectedNewId = 100;
            string testUser = "AdminUser";
            string testIp = "192.168.1.50";

            mockRepo.Setup(r => r.CreateAsync(It.IsAny<Region>()))
                .ReturnsAsync(expectedNewId);

            // Act
            var result = await service.CreateAsync(request, testUser, testIp);

            // Assert
            Assert.Equal(expectedNewId, result);
            
            mockRepo.Verify(r => r.CreateAsync(It.Is<Region>(x => x.Nombre == request.Nombre)), Times.Once);

            mockAudit.Verify(a => a.AddAsync(It.Is<Auditoria>(x => 
                x.Usuario == testUser && 
                x.IpAddress == testIp && 
                x.Accion == "CREATE_REGION" &&
                x.Detalle.Contains(expectedNewId.ToString())
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepoAndAudit_WhenGenericDeleteIsCalled()
        {
            var mockRepo = new Mock<IRegionRepository>();
            var mockAudit = new Mock<IAuditoriaRepository>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<RegionService>>();
            var service = new RegionService(mockRepo.Object, mockAudit.Object, mockLogger.Object);
            
            int regionId = 99;
            string testUser = "TestUser";
            string testIp = "127.0.0.1";
            await service.DeleteAsync(regionId, testUser, testIp);
            mockRepo.Verify(r => r.DeleteAsync(regionId), Times.Once);
            mockAudit.Verify(a => a.AddAsync(It.Is<Auditoria>(x => 
                x.Usuario == testUser && 
                x.IpAddress == testIp && 
                x.Accion == "DELETE_REGION")), Times.Once);
        }
    }
}
