using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Services;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Domain.Interfaces.Repositories;
using LeaveManagement.Domain.ValueObjects;
using Moq;
using Xunit;

namespace LeaveManagement.Tests.Application
{
    public class LeaveRequestServiceTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockRepository;
        private readonly LeaveRequestService _service;

        public LeaveRequestServiceTests()
        {
            _mockRepository = new Mock<ILeaveRequestRepository>();
            _service = new LeaveRequestService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithValidData_ShouldCreateRequest()
        {
            // Arrange
            var createDto = new CreateLeaveRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                LeaveType = LeaveType.Vacation,
                Comments = "Test comment"
            };

            _mockRepository.Setup(x => x.HasOverlappingLeaveAsync(It.IsAny<Guid>(), It.IsAny<DateRange>(), null))
                .ReturnsAsync(false);

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<LeaveRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateLeaveRequestAsync(createDto);

            // Assert
            Assert.Equal(createDto.EmployeeId, result.EmployeeId);
            Assert.Equal(createDto.LeaveType, result.LeaveType);
            Assert.Equal(createDto.Comments, result.Comments);
            Assert.Equal(LeaveStatus.Pending, result.Status);
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithOverlappingLeave_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreateLeaveRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                LeaveType = LeaveType.Vacation
            };

            _mockRepository.Setup(x => x.HasOverlappingLeaveAsync(It.IsAny<Guid>(), It.IsAny<DateRange>(), null))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateLeaveRequestAsync(createDto));
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithPastStartDate_ShouldThrowDomainException()
        {
            // Arrange
            var createDto = new CreateLeaveRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(-5), // Past date
                EndDate = DateTime.Now.AddDays(5),
                LeaveType = LeaveType.Vacation
            };

            _mockRepository.Setup(x => x.HasOverlappingLeaveAsync(It.IsAny<Guid>(), It.IsAny<DateRange>(), null))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(
                () => _service.CreateLeaveRequestAsync(createDto));
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithExcessiveDuration_ShouldThrowDomainException()
        {
            // Arrange
            var createDto = new CreateLeaveRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(35), // 35 days - exceeds limit
                LeaveType = LeaveType.Vacation
            };

            _mockRepository.Setup(x => x.HasOverlappingLeaveAsync(It.IsAny<Guid>(), It.IsAny<DateRange>(), null))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(
                () => _service.CreateLeaveRequestAsync(createDto));
        }

        [Fact]
        public async Task GetLeaveRequestByIdAsync_WithNonExistentId_ShouldThrowException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetByIdAsync(nonExistentId))
                .ReturnsAsync((LeaveRequest?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetLeaveRequestByIdAsync(nonExistentId));
        }
    }
}