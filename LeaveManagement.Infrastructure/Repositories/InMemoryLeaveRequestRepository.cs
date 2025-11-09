using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Interfaces.Repositories;
using LeaveManagement.Domain.ValueObjects;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class InMemoryLeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly List<LeaveRequest> _leaveRequests = new();

        public Task<LeaveRequest?> GetByIdAsync(Guid id)
        {
            var leaveRequest = _leaveRequests.FirstOrDefault(lr => lr.Id == id);
            return Task.FromResult(leaveRequest);
        }

        public Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(Guid employeeId)
        {
            var leaveRequests = _leaveRequests.Where(lr => lr.EmployeeId == employeeId);
            return Task.FromResult(leaveRequests);
        }

        public Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return Task.FromResult(_leaveRequests.AsEnumerable());
        }

        public Task AddAsync(LeaveRequest leaveRequest)
        {
            _leaveRequests.Add(leaveRequest);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(LeaveRequest leaveRequest)
        {
            var existing = _leaveRequests.FirstOrDefault(lr => lr.Id == leaveRequest.Id);
            if (existing != null)
            {
                _leaveRequests.Remove(existing);
                _leaveRequests.Add(leaveRequest);
            }
            return Task.CompletedTask;
        }

        public Task<bool> HasOverlappingLeaveAsync(Guid employeeId, DateRange leavePeriod, Guid? excludeId = null)
        {
            var hasOverlapping = _leaveRequests.Any(lr =>
                lr.EmployeeId == employeeId &&
                lr.Id != excludeId &&
                lr.IsOverlapping(leavePeriod));

            return Task.FromResult(hasOverlapping);
        }
    }
}