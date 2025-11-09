# Leave Management System - Backend (.NET 8)

A lightweight implementation of a Leave Management API built with **Hexagonal Architecture** and **Domain-Driven Design (DDD)**.  
This project corresponds to **User Story 1 - Submit a Leave Request** from the TF1 technical test.

---

## 1. Architecture (Hexagonal + DDD)

The solution is structured into four independent layers:

### **Domain**
- Core business logic  
- Aggregates (`LeaveRequest`)  
- Value Objects (`DateRange`)  
- Enums (`LeaveStatus`, `LeaveType`)  
- Domain exceptions  
- Repository interfaces  

### **Application**
- Services / use cases (`LeaveRequestService`)  
- Input/Output DTOs  
- Validation logic  
- Error propagation  

### **Infrastructure**
- In-memory implementation of the repository (simple, no DB required)  
- Dependency Injection setup  

### **API**
- REST controllers with versioning (`/api/v1/leaverequests`)  
- Request validation  
- Global exception middleware  
- Swagger/OpenAPI  

The layers communicate through well-defined interfaces, following Hexagonal principles.

---

## 2. Implemented Features

- Submit a leave request  
- Retrieve all leave requests  
- Retrieve by ID  
- Retrieve by employee  
- Approve or reject  
- Prevent overlapping date ranges  
- Return structured and consistent error responses  
- Swagger documentation  
- Unit tests for domain & application layers  

---

## 3. Folder Structure

```
LeaveManagement/
  -LeaveManagement.Domain
  -LeaveManagement.Application
  -LeaveManagement.Infrastructure
  -LeaveManagement.API
  -LeaveManagement.Tests
```

---

## 4. Prerequisites

- .NET 8 SDK  
- Any compatible IDE (Visual Studio, Rider, VS Code)

---

## 5. Build & Run

### Clone repo
```bash
git clone https://github.com/Roshanwb/LeaveManagement.git
cd LeaveManagement
```

### Build
```bash
dotnet build
```

### Run tests
```bash
dotnet test
```

### Launch API
```bash
cd LeaveManagement.API
dotnet run
```

### Access
- API: http://localhost:7000  
- Swagger: http://localhost:7000/swagger  

---

## 6. REST Endpoints (v1)

### Create a leave request  
`POST /api/v1/leaverequests`

### Get all  
`GET /api/v1/leaverequests`

### Get by ID  
`GET /api/v1/leaverequests/{id}`

### Get by employee  
`GET /api/v1/leaverequests/employee/{employeeId}`

### Approve  
`POST /api/v1/leaverequests/{id}/approve`

### Reject  
`POST /api/v1/leaverequests/{id}/reject`

---

## 7. Example Requests

### Create  
```bash
curl -X POST "http://localhost:7000/api/v1/leaverequests" \
-H "Content-Type: application/json" \
-d '{
  "employeeId": "12345678-1234-1234-1234-123456789abc",
  "startDate": "2024-01-15",
  "endDate": "2024-01-20",
  "leaveType": 1,
  "comments": "Vacances"
}'
```

### Approve  
```bash
curl -X POST "http://localhost:7000/api/v1/leaverequests/{id}/approve" \
-H "Content-Type: application/json" \
-d '{ "managerComments": "Approved" }'
```

---

## 8. Tests

Tests cover:

- `DateRange` validations  
- Leave request rules (overlaps, status changes)  
- Service logic  
- Validation failures  

Run:
```bash
dotnet test
```

---

## 9. Notes

- No database is required.  
- Architecture is intentionally simple and focused on core principles.  
- The project aims for clarity, correctness, and readability.

---

## 10. Git Workflow Used

The project follows a simple organization inspired by GitFlow:

- **main**: stable version of the project
- **dev**: development branch
- **feature/***: branches dedicated to enhancements (via Pull Requests)
- **release/***: preparing a release version

This allows for a clear history and a distinct separation between stable code and ongoing development.
