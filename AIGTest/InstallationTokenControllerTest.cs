using AIG.Contracts;
using AIG.Controllers;
using AIG.Models;
using AIG.Services.InstallationToken;
using Moq;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace AIGTest
{
    public class InstallationTokenControllerTest
    {

        [Fact]
        public void TestFetchInstallationToken()
        {
            //Arrange
            var mockInstallationTokenService = new Mock<IInstallationTokenService>();
            mockInstallationTokenService.Setup(instaSer => 
            instaSer.FetchInstallationToken(It.IsAny<InstallationTokenServiceContext>()))
                .Returns(new List<InstallationTokenResponse>() { new InstallationTokenResponse()});
            var instTokenContext = new InstallationTokenContext();
            var controller = new InstallationTokenController(mockInstallationTokenService.Object);

            //Act
            var result = controller.FetchToken(instTokenContext, "uuygfcdsmbfsuh5gkjhkjhv");

            //Assert
            Assert.NotEmpty(result);
        }
    }
}
