using System.ComponentModel.DataAnnotations;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.DTOs
{
    public class CreateLeaveRequestDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "LeaveType is required")]
        [Range(1, 5, ErrorMessage = "LeaveType must be between 1 and 5")]
        public LeaveType LeaveType { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }
    }
}