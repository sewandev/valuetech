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
