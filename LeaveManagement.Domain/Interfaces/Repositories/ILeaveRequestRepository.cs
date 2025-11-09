using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.ValueObjects;

namespace LeaveManagement.Domain.Interfaces.Repositories
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest?> GetByIdAsync(Guid id);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<LeaveRequest>> GetAllAsync();
        Task AddAsync(LeaveRequest leaveRequest);
        Task UpdateAsync(LeaveRequest leaveRequest);
        Task<bool> HasOverlappingLeaveAsync(Guid employeeId, DateRange leavePeriod, Guid? excludeId = null);
    }
}