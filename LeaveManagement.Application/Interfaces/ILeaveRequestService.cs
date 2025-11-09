using LeaveManagement.Application.DTOs;

namespace LeaveManagement.Application.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequestDto> CreateLeaveRequestAsync(CreateLeaveRequestDto createDto);
        Task<LeaveRequestDto> GetLeaveRequestByIdAsync(Guid id);
        Task<IEnumerable<LeaveRequestDto>> GetAllLeaveRequestsAsync();
        Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId);
        Task<LeaveRequestDto> ApproveLeaveRequestAsync(Guid id, UpdateLeaveRequestStatusDto updateDto);
        Task<LeaveRequestDto> RejectLeaveRequestAsync(Guid id, UpdateLeaveRequestStatusDto updateDto);
    }
}