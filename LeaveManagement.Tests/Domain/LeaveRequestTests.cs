using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.ValueObjects;
using Xunit;

namespace LeaveManagement.Tests.Domain
{
    public class LeaveRequestTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateLeaveRequest()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(5);
            var leavePeriod = new DateRange(startDate, endDate);

            // Act
            var leaveRequest = new LeaveRequest(employeeId, leavePeriod, LeaveType.Vacation, "Test comment");

            // Assert
            Assert.Equal(employeeId, leaveRequest.EmployeeId);
            Assert.Equal(LeaveType.Vacation, leaveRequest.LeaveType);
            Assert.Equal("Test comment", leaveRequest.Comments);
            Assert.Equal(LeaveStatus.Pending, leaveRequest.Status);
            Assert.NotNull(leaveRequest.CreatedAt);
        }

        [Fact]
        public void Approve_ShouldUpdateStatusAndTimestamp()
        {
            // Arrange
            var leaveRequest = CreateTestLeaveRequest();

            // Act
            leaveRequest.Approve("Approved by manager");

            // Assert
            Assert.Equal(LeaveStatus.Approved, leaveRequest.Status);
            Assert.Equal("Approved by manager", leaveRequest.ManagerComments);
            Assert.NotNull(leaveRequest.UpdatedAt);
        }

        [Fact]
        public void Reject_ShouldUpdateStatusAndTimestamp()
        {
            // Arrange
            var leaveRequest = CreateTestLeaveRequest();

            // Act
            leaveRequest.Reject("Rejected by manager");

            // Assert
            Assert.Equal(LeaveStatus.Rejected, leaveRequest.Status);
            Assert.Equal("Rejected by manager", leaveRequest.ManagerComments);
            Assert.NotNull(leaveRequest.UpdatedAt);
        }

        [Fact]
        public void IsOverlapping_WithOverlappingPeriod_ShouldReturnTrue()
        {
            // Arrange
            var leaveRequest = CreateTestLeaveRequest();
            var overlappingPeriod = new DateRange(
                DateTime.Now.AddDays(3),
                DateTime.Now.AddDays(7));

            // Act
            var isOverlapping = leaveRequest.IsOverlapping(overlappingPeriod);

            // Assert
            Assert.True(isOverlapping);
        }

        [Fact]
        public void IsOverlapping_WithNonOverlappingPeriod_ShouldReturnFalse()
        {
            // Arrange
            var leaveRequest = CreateTestLeaveRequest();
            var nonOverlappingPeriod = new DateRange(
                DateTime.Now.AddDays(10),
                DateTime.Now.AddDays(15));

            // Act
            var isOverlapping = leaveRequest.IsOverlapping(nonOverlappingPeriod);

            // Assert
            Assert.False(isOverlapping);
        }

        private static LeaveRequest CreateTestLeaveRequest()
        {
            var employeeId = Guid.NewGuid();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(5);
            var leavePeriod = new DateRange(startDate, endDate);

            return new LeaveRequest(employeeId, leavePeriod, LeaveType.Vacation, "Test comment");
        }
    }
}