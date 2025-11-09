namespace LeaveManagement.Domain.ValueObjects
{
    public record DateRange
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be after end date");

            StartDate = startDate.Date;
            EndDate = endDate.Date;
        }

        public int TotalDays => (EndDate - StartDate).Days + 1;
    }
}