using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Domain.ValueObjects;
using Xunit;

namespace LeaveManagement.Tests.Domain
{
    public class LeaveRequestValidationTests
    {
        [Fact]
        public void Create_WithStartDateInPast_ShouldThrowDomainException()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var startDate = DateTime.Now.AddDays(-5); // Past date
            var endDate = DateTime.Now.AddDays(5);
            var leavePeriod = new DateRange(startDate, endDate);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new LeaveRequest(employeeId, leavePeriod, LeaveType.Vacation, "Test"));
        }

        [Fact]
        public void Create_WithLeaveExceeding30Days_ShouldThrowDomainException()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(35); // 35 days total
            var leavePeriod = new DateRange(startDate, endDate);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new LeaveRequest(employeeId, leavePeriod, LeaveType.Vacation, "Test"));
        }
    }
}