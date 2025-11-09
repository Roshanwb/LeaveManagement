using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.DTOs
{
    public class LeaveRequestDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string? Comments { get; set; }
        public LeaveStatus Status { get; set; }
        public string? ManagerComments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TotalDays { get; set; }
    }
}