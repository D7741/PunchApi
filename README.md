# PunchApi
# 🕒 UserPunchApi (Workforce Management System)

A full-stack workforce management system designed to handle employee scheduling, attendance tracking (punch in/out), and leave management.

This project simulates real-world enterprise systems such as UKG or Workday, focusing on clean backend architecture and scalable design.

---

## 🚀 Tech Stack

### Backend

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQLite (development)
* Swagger (API documentation)

### Frontend (Planned)

* React.js
* JavaScript / TypeScript
* REST API integration
* Responsive UI design

---

## 📌 Features

### ✅ Implemented (Backend)

#### 👤 User Management

* Role-based users (Employee / Manager)
* Relationships with schedules, punch records, and leave requests

#### 🗓️ Schedule Management

* Create, update, delete schedules
* Assign schedules to users
* Track manager who created schedules
* Retrieve schedules by user

#### ⏱️ Punch Records (In Progress)

* Punch in / punch out system
* Track working sessions

#### 📝 Leave Requests (In Progress)

* Submit leave requests
* Leave types (Annual, Sick, etc.)
* Status tracking

---

### 🔜 Planned (Frontend)

* User login & dashboard
* View schedules
* Punch in/out UI
* Apply for leave
* Manager dashboard:

  * Create schedules
  * Approve leave
  * Manage employees

---

## 🏗️ Architecture

This project follows a layered architecture:

```
Controller → Service → Repository → DbContext → Database
```

### Layers

* **Controller** → Handles HTTP requests
* **Service** → Business logic & validation
* **Repository** → Database operations (EF Core)
* **DTOs** → Data transfer (clean API design)

---

## 📂 Project Structure

```
UserPunchApi/
│
├── Controllers/
│   └── V1/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
├── Models/
├── DTOs/
│   └── V1/
├── Data/
├── Migrations/
└── Program.cs
```

---

## 🔌 API Endpoints

### Schedule

```
GET    /api/v1/schedules
GET    /api/v1/schedules/{id}
GET    /api/v1/schedules/user/{userId}

POST   /api/v1/schedules
PUT    /api/v1/schedules/{id}
DELETE /api/v1/schedules/{id}
```

---

## 🧪 Example Request

### Create Schedule

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

## ⚠️ Important Notes

* Foreign key constraints enforced:

  * `UserId` must exist
  * `CreatedByManagerId` must exist
* Validation:

  * `EndTime > StartTime`

---

## 🔐 Security

* Input validation via DTOs
* Business logic validation in Service layer
* Planned:

  * JWT authentication
  * Role-based authorization

See `SECURITY.md` for vulnerability reporting.

---

## 📦 Setup

### 1. Clone repo

```bash
git clone <your-repo-url>
cd UserPunchApi
```

### 2. Install

```bash
dotnet restore
```

### 3. Database

```bash
dotnet ef database update
```

### 4. Run

```bash
dotnet run
```

### 5. Swagger

```
http://localhost:5007/swagger
```

---

## 🧠 Future Improvements

* Schedule conflict detection
* Role-based permissions
* JWT authentication
* React frontend
* Cloud deployment

---

## 📄 License

MIT License

---

## 👨‍💻 Author
D7741
