using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Details;

namespace Application.Tests
{
    public class DetailsHandlerTests
    {
        private readonly Mock<IActivityRepository> activityRepositoryMock;
        private readonly Mock<IAccessUser> accessUserMock;

        public DetailsHandlerTests()
        {
            activityRepositoryMock = new Mock<IActivityRepository>();
            accessUserMock = new Mock<IAccessUser>();
        }

        private Activity CreateSampleActivity()
        {
            return new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Sample Activity",
                Description = "Description of the sample activity",
                Date = DateTime.UtcNow.AddDays(7),
                Category = "Category",
                City = "City",
                Venue = "Venue"
            };
        }

        [Fact]
        public async Task Handle_ActivityFound_ShouldReturnSuccessWithActivity()
        {
            // Arrange
            var sampleActivity = CreateSampleActivity();
            activityRepositoryMock.Setup(repo => repo.GetByIdWithAttendeesAsync(sampleActivity.Id))
                .ReturnsAsync(sampleActivity);
            var handler = new Details.Handler(activityRepositoryMock.Object, accessUserMock.Object);
            var query = new Details.Query(sampleActivity.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.Equal(sampleActivity.Id, result.Value.Id);
        }

        [Fact]
        public async Task Handle_ActivityNotFound_ShouldReturnFailure()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            activityRepositoryMock.Setup(repo => repo.GetByIdWithAttendeesAsync(nonExistentId))
                .ReturnsAsync((Activity)null);
            var handler = new Details.Handler(activityRepositoryMock.Object, accessUserMock.Object);
            var query = new Details.Query(nonExistentId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Handle_InvalidId_ShouldFailValidation()
        {
            // Arrange
            var handler = new Details.Handler(activityRepositoryMock.Object, accessUserMock.Object);
            var query = new Details.Query(Guid.Empty); 

            // Act & Assert
            var validationResults = new QueryValidator().Validate(query);
            Assert.False(validationResults.IsValid);
        }
    }
}
