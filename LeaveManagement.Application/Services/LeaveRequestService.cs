using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Interfaces;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Interfaces.Repositories;
using LeaveManagement.Domain.ValueObjects;

namespace LeaveManagement.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<LeaveRequestDto> CreateLeaveRequestAsync(CreateLeaveRequestDto createDto)
        {
            var leavePeriod = new DateRange(createDto.StartDate, createDto.EndDate);

            var hasOverlapping = await _leaveRequestRepository.HasOverlappingLeaveAsync(
                createDto.EmployeeId, leavePeriod);

            if (hasOverlapping)
            {
                throw new InvalidOperationException("There is an overlapping leave request for this period");
            }

            var leaveRequest = new LeaveRequest(
                createDto.EmployeeId,
                leavePeriod,
                createDto.LeaveType,
                createDto.Comments);

            await _leaveRequestRepository.AddAsync(leaveRequest);

            return MapToDto(leaveRequest);
        }

        public async Task<LeaveRequestDto> GetLeaveRequestByIdAsync(Guid id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new KeyNotFoundException($"Leave request with ID {id} not found");

            return MapToDto(leaveRequest);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllLeaveRequestsAsync()
        {
            var leaveRequests = await _leaveRequestRepository.GetAllAsync();
            return leaveRequests.Select(MapToDto);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId)
        {
            var leaveRequests = await _leaveRequestRepository.GetByEmployeeIdAsync(employeeId);
            return leaveRequests.Select(MapToDto);
        }

        public async Task<LeaveRequestDto> ApproveLeaveRequestAsync(Guid id, UpdateLeaveRequestStatusDto updateDto)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new KeyNotFoundException($"Leave request with ID {id} not found");

            leaveRequest.Approve(updateDto.ManagerComments);
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            return MapToDto(leaveRequest);
        }

        public async Task<LeaveRequestDto> RejectLeaveRequestAsync(Guid id, UpdateLeaveRequestStatusDto updateDto)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new KeyNotFoundException($"Leave request with ID {id} not found");

            leaveRequest.Reject(updateDto.ManagerComments);
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            return MapToDto(leaveRequest);
        }

        private static LeaveRequestDto MapToDto(LeaveRequest leaveRequest)
        {
            return new LeaveRequestDto
            {
                Id = leaveRequest.Id,
                EmployeeId = leaveRequest.EmployeeId,
                StartDate = leaveRequest.LeavePeriod.StartDate,
                EndDate = leaveRequest.LeavePeriod.EndDate,
                LeaveType = leaveRequest.LeaveType,
                Comments = leaveRequest.Comments,
                Status = leaveRequest.Status,
                ManagerComments = leaveRequest.ManagerComments,
                CreatedAt = leaveRequest.CreatedAt,
                UpdatedAt = leaveRequest.UpdatedAt,
                TotalDays = leaveRequest.LeavePeriod.TotalDays
            };
        }
    }
}