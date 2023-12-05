using Application.Activities;
using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class ListHandlerTests
    {
        private readonly Mock<IActivityRepository> _mockActivityRepo;
        private readonly Mock<IAccessUser> _mockAccessUser;
        private readonly Mock<IMapper> _mockMapper;
        private readonly List.Handler _handler;

        public ListHandlerTests()
        {
            _mockActivityRepo = new Mock<IActivityRepository>();
            _mockAccessUser = new Mock<IAccessUser>();
            _mockMapper = new Mock<IMapper>();
            _handler = new List.Handler(_mockActivityRepo.Object, _mockAccessUser.Object, _mockMapper.Object);
        }

        private List<Activity> GetTestActivities(int count)
        {
            var activities = new List<Activity>();
            for (int i = 1; i <= count; i++)
            {
                activities.Add(new Activity
                {
                    Id = Guid.NewGuid(),
                    Title = $"Test Activity {i}",
                    Description = $"Description for Test Activity {i}",
                    Category = $"Category{i}",
                    City = $"City{i}",
                    Date = DateTime.UtcNow.AddDays(i * 10),
                    Venue = $"Venue{i}"
                });
            }
            return activities;
        }

        [Fact]
        public async Task Handle_ReturnsPaginatedListOfActivityDTO_WhenActivitiesExist()
        {
            // Arrange
            var fakeActivities = GetTestActivities(2);
            var fakePaginatedResult = new PaginatedResult<Activity>
            {
                Items = fakeActivities,
                Metadata = new PaginationMetadata
                {
                    TotalCount = fakeActivities.Count,
                    PageSize = 10,
                    CurrentPage = 1,
                    TotalPages = 1
                }
            };

            _mockActivityRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(),It.IsAny<string>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(fakePaginatedResult);

            var fakeActivitiesDto = fakeActivities.Select(a => new ActivityDTO
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Category = a.Category,  
                City = a.City,
                Date = a.Date,
                Venue = a.Venue,

            }).ToList();

            _mockMapper.Setup(mapper => mapper.Map<List<ActivityDTO>>(It.IsAny<List<Activity>>()))
                .Returns(fakeActivitiesDto);

            var query = new List.Query { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value.Items);
            Assert.Equal(fakeActivitiesDto.Count, result.Value.Items.Count);
            Assert.All(result.Value.Items, item => Assert.IsType<ActivityDTO>(item));
        }

        
    }
}
