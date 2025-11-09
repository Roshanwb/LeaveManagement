using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Services;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Domain.Interfaces.Repositories;
using Enums =  LeaveManagement.Domain.Enums ;
using Moq;
using Xunit;

namespace LeaveManagement.Tests.Application
{
    public class LeaveRequestServiceValidationTests
    {
        [Fact]
        public async Task CreateLeaveRequestAsync_WithPastStartDate_ShouldThrowDomainException()
        {
            // Arrange
            var mockRepository = new Mock<ILeaveRequestRepository>();
            var service = new LeaveRequestService(mockRepository.Object);

            var invalidDto = new CreateLeaveRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(-5), // Past date
                EndDate = DateTime.Now.AddDays(5),
                LeaveType = Enums.LeaveType.Vacation
            };

            // Act & Assert - Should throw DomainException from business rules
            await Assert.ThrowsAsync<DomainException>(() =>
                service.CreateLeaveRequestAsync(invalidDto));
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithExcessiveDuration_ShouldThrowDomainException()
        {
            // Arrange
            var mockRepository = new Mock<ILeaveRequestRepository>();
            var service = new LeaveRequestService(mockRepository.Object);

            var invalidDto = new CreateLeaveRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(40), // 40 days - too long
                LeaveType = Enums.LeaveType.Vacation
            };

            // Act & Assert - Should throw DomainException from business rules
            await Assert.ThrowsAsync<DomainException>(() =>
                service.CreateLeaveRequestAsync(invalidDto));
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithOverlappingLeave_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var mockRepository = new Mock<ILeaveRequestRepository>();
            mockRepository.Setup(x => x.HasOverlappingLeaveAsync(It.IsAny<Guid>(), It.IsAny<LeaveManagement.Domain.ValueObjects.DateRange>(), null))
                .ReturnsAsync(true); // Simulate overlapping leave

            var service = new LeaveRequestService(mockRepository.Object);

            var validDto = new CreateLeaveRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                LeaveType = Enums.LeaveType.Vacation
            };

            // Act & Assert - Should throw InvalidOperationException for overlapping leaves
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CreateLeaveRequestAsync(validDto));
        }
    }
}