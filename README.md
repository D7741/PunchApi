# 🕒 UserPunchApi (Workforce Management System)

A full-stack workforce management system for managing employees, schedules, attendance (punch records), and leave requests.

This project is designed to simulate real-world enterprise systems (e.g., UKG / Workday) with a clean backend architecture and scalable design.

---

## 🚀 Tech Stack

### Backend

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQLite (Development Database)
* Swagger (API Documentation)

### Frontend (Planned)

* React.js
* JavaScript / TypeScript
* REST API integration
* Responsive UI

---

## 📌 Core Features

### 👤 User Management

* Employee and Manager roles
* User-based relationships with schedules, punch records, and leave requests

---

### 🗓️ Schedule Management

* Create / update / delete schedules
* Assign schedules to employees
* Track which manager created a schedule
* View schedules by user

---

### ⏱️ Punch Records

* Punch in / punch out functionality
* Track working sessions
* Open/closed status handling

---

### 📝 Leave Requests

* Submit leave requests
* Leave types:

  * Annual Leave
  * Sick Leave
  * Carer's Leave
* Status tracking:

  * Pending
  * Approved
  * Rejected

---

## 🏗️ System Architecture

This project follows a layered architecture:

```
Controller → Service → Repository → DbContext → Database
```

### Layer Responsibilities

* **Controller**

  * Handles HTTP requests & responses

* **Service**

  * Business logic
  * Validation (e.g. time checks)

* **Repository**

  * Database operations via EF Core

* **DTOs**

  * Clean API input/output models

---

## 📂 Project Structure

```
UserPunchApi/
│
├── Controllers/
│   └── V1/
│       ├── UsersController.cs
│       ├── SchedulesController.cs
│       ├── PunchRecordsController.cs
│       └── LeaveRequestsController.cs
│
├── Services/
│   ├── Interfaces/
│   └── Implementations/
│
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
│
├── Models/
│   ├── User.cs
│   ├── Schedule.cs
│   ├── PunchRecord.cs
│   └── LeaveRequest.cs
│
├── DTOs/
│   └── V1/
│
├── Data/
│   └── AppDbContext.cs
│
├── Migrations/
└── Program.cs
```

---

## 🔌 API Overview

### Users

```
GET    /api/v1/users
GET    /api/v1/users/{id}
POST   /api/v1/users
```

---

### Schedules

```
GET    /api/v1/schedules
GET    /api/v1/schedules/{id}
GET    /api/v1/schedules/user/{userId}

POST   /api/v1/schedules
PUT    /api/v1/schedules/{id}
DELETE /api/v1/schedules/{id}
```

---

### Punch Records

```
GET    /api/v1/punchrecords
GET    /api/v1/punchrecords/{id}

POST   /api/v1/punchrecords
PUT    /api/v1/punchrecords/{id}
```

---

### Leave Requests

```
GET    /api/v1/leaverequests
GET    /api/v1/leaverequests/{id}

POST   /api/v1/leaverequests
PUT    /api/v1/leaverequests/{id}
```

---

## 🧪 Example: Create Schedule

```json
{
  "userId": 1,
  "shiftDate": "2026-04-09T00:00:00",
  "startTime": "2026-04-09T09:00:00",
  "endTime": "2026-04-09T17:00:00",
  "shiftName": "Morning Shift",
  "createdByManagerId": 2
}
```

---

## ⚠️ Important Constraints

* Foreign Keys:

  * `UserId` must exist in Users table
  * `CreatedByManagerId` must exist

* Validation:

  * `EndTime` must be greater than `StartTime`

---

## 🔐 Security Considerations

* DTO validation for input safety
* Business rules enforced in Service layer
* Planned improvements:

  * JWT authentication
  * Role-based authorization (Manager vs Employee)
  * Data access control

See `SECURITY.md` for vulnerability reporting.

---

## 📦 Setup Instructions

### 1. Clone repository

```bash
git clone <your-repo-url>
cd UserPunchApi
```

---

### 2. Install dependencies

```bash
dotnet restore
```

---

### 3. Apply migrations

```bash
dotnet ef database update
```

---

### 4. Run the application

```bash
dotnet run
```

---

### 5. Open Swagger

```
http://localhost:5007/swagger
```

---

## 🧠 Future Improvements

* Schedule conflict detection (no overlapping shifts)
* Full role-based access control
* JWT authentication
* React frontend implementation
* Cloud deployment (Azure / AWS)
* Notification system

---

## 📄 License

MIT License

---

## 👨‍💻 Author

Pengyu Liu

---

## ⭐ Project Goal

This project demonstrates:

* Clean backend architecture (Controller / Service / Repository)
* RESTful API design
* Entity relationships & database modeling
* Scalable system structure for real-world applications
