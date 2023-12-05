using Application.ImageHandlers;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ApplicationUnitTests.ImageTests
{
    
    public class ImageUploadTests
    {
        private readonly Mock<IAccessUser> _accessUserMock;
        private readonly Mock<IImageRepository> _imageRepositoryMock;
        private readonly Mock<ICloudinaryService> _cloudinaryServiceMock;
        private readonly ImageUpload.Handler _handler;

        public ImageUploadTests()
        {
            _accessUserMock= new Mock<IAccessUser>();
            _cloudinaryServiceMock= new Mock<ICloudinaryService>();
            _imageRepositoryMock = new Mock<IImageRepository>();
            _handler = new ImageUpload.Handler(_imageRepositoryMock.Object,_accessUserMock.Object,_cloudinaryServiceMock.Object);
        }
        private IFormFile MockIFormFile(string content, string fileName, string contentType)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            

            return fileMock.Object;
        }
        [Fact]
#pragma warning disable IDE1006 // Naming Styles
        public async Task uploadImage_shouldReturnSuccess()
#pragma warning restore IDE1006 // Naming Styles
        {
            //Arrange
            var mockFile = MockIFormFile("image content", "test.jpg", "image/jpeg");
            _accessUserMock.Setup(x => x.GetUser()).Returns("testId");
            _cloudinaryServiceMock.Setup(x => x.UploadImageAsync(MockIFormFile("xd","xd","xd"))).ReturnsAsync(("xd","xd"));
            _imageRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Image>())).Returns(Task.CompletedTask);

            


            var command = new ImageUpload.Command { ImageFile = mockFile };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);


        }


    }
}
