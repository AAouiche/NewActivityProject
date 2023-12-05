using Application.ImageHandlers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.InfrastructureUnitTests.Services
{
    public class ImageUploadTests
    {
        private readonly Mock<IAccessUser> _accessUserMock;
        private readonly Mock<IImageRepository> _imageRepositoryMock;
        private readonly Mock<ICloudinaryService> _cloudinaryServiceMock;
        private readonly ImageUpload.Handler _handler;

        public ImageUploadTests()
        {
            _accessUserMock = new Mock<IAccessUser>();
            _imageRepositoryMock = new Mock<IImageRepository>();
            _cloudinaryServiceMock = new Mock<ICloudinaryService>();

            _handler = new ImageUpload.Handler(_imageRepositoryMock.Object, _accessUserMock.Object, _cloudinaryServiceMock.Object);
        }
        [Fact]
        public async Task Handle_UploadsImageAndSavesDetails()
        {
            // Arrange
            var userId = "test-user-id";
            _accessUserMock.Setup(x => x.GetUser()).Returns(userId);

            var mockFile = MockIFormFile("image content", "test.jpg", "image/jpeg");

            var command = new ImageUpload.Command
            {
                ImageFile = mockFile
            };

            var expectedImageUrl = "http://example.com/image.jpg";
            var expectedPublicId = "public-id";

            
            _cloudinaryServiceMock.Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>()))
                                  .ReturnsAsync((expectedImageUrl, expectedPublicId));

           
            _imageRepositoryMock.Setup(s => s.CreateAsync(It.IsAny<Domain.Models.Image>()))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            
            _cloudinaryServiceMock.Verify(s => s.UploadImageAsync(It.IsAny<IFormFile>()), Times.Once);
            _imageRepositoryMock.Verify(s => s.CreateAsync(It.IsAny<Domain.Models.Image>()), Times.Once);
        }

        private IFormFile MockIFormFile(string content, string fileName, string contentType)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            return fileMock.Object;
        }

    }
}
