using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Application.DTOs
{
    public class UpdateLeaveRequestStatusDto
    {
        [StringLength(500, ErrorMessage = "Manager comments cannot exceed 500 characters")]
        public string? ManagerComments { get; set; }
    }
}