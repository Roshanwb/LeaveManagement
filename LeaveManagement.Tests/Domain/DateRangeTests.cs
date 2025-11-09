using LeaveManagement.Domain.ValueObjects;
using Xunit;

namespace LeaveManagement.Tests.Domain
{
    public class DateRangeTests
    {
        [Fact]
        public void Create_WithStartDateAfterEndDate_ShouldThrowException()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(5);
            var endDate = DateTime.Now;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new DateRange(startDate, endDate));
        }

        [Fact]
        public void TotalDays_ShouldCalculateCorrectly()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 5);
            var dateRange = new DateRange(startDate, endDate);

            // Act
            var totalDays = dateRange.TotalDays;

            // Assert
            Assert.Equal(5, totalDays);
        }
    }
}