using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Domain.ValueObjects;

namespace LeaveManagement.Domain.Entities
{
    public class LeaveRequest
    {
        public Guid Id { get; private set; }
        public Guid EmployeeId { get; private set; }
        public DateRange LeavePeriod { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public string? Comments { get; private set; }
        public LeaveStatus Status { get; private set; }
        public string? ManagerComments { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private LeaveRequest() { }

        public LeaveRequest(Guid employeeId, DateRange leavePeriod, LeaveType leaveType, string? comments)
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            LeavePeriod = leavePeriod;
            LeaveType = leaveType;
            Comments = comments;
            Status = LeaveStatus.Pending;
            CreatedAt = DateTime.UtcNow;

            // Validate business rules
            ValidateLeavePeriod();
        }

        public void Approve(string? managerComments = null)
        {
            Status = LeaveStatus.Approved;
            ManagerComments = managerComments;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject(string? managerComments = null)
        {
            Status = LeaveStatus.Rejected;
            ManagerComments = managerComments;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsOverlapping(DateRange otherPeriod)
        {
            return LeavePeriod.StartDate <= otherPeriod.EndDate &&
                   otherPeriod.StartDate <= LeavePeriod.EndDate;
        }

        public void ValidateLeavePeriod()
        {
            if (LeavePeriod.StartDate < DateTime.Today)
            {
                throw new DomainException("Leave start date cannot be in the past");
            }

            if (LeavePeriod.TotalDays > 30)
            {
                throw new DomainException("Leave request cannot exceed 30 days");
            }

            if (LeavePeriod.StartDate.DayOfWeek == DayOfWeek.Saturday ||
                LeavePeriod.StartDate.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new DomainException("Leave cannot start on a weekend");
            }
        }
    }
}