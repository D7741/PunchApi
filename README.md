# 🚀 UserPunch — Workforce Management System

A full-stack **workforce management MVP** designed for employee scheduling, attendance tracking, and leave management.

Built with **ASP.NET Core 8** (backend) and **React 19 + Vite** (frontend), this project demonstrates clean architecture, role-based access control, and scalable design.

---

## ✨ Features

### 👤 Employee (All Authenticated Users)

- **Punch In / Punch Out**  
  Clock in and out from the dashboard with real-time status updates.

- **Leave Requests**  
  Submit leave requests and track approval status.

- **Schedule Viewing**  
  View assigned shifts in a calendar-based interface.

---

### 👨‍💼 Manager

- **Schedule Management**  
  Create, update, and assign shifts via calendar.

- **Leave Approvals**  
  Approving or rejecting pending leave requests.

- **User Management**  
  View and manage users, roles, and departments.

---

## 🛠 Tech Stack

### Backend

| Category | Technology |
|--------|----------|
| Framework | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Database | SQLite |
| Authentication | JWT Bearer (HMAC-SHA256) |
| Password Hashing | BCrypt |
| API Docs | Swagger / OpenAPI |

---

### Frontend

| Category | Technology |
|--------|----------|
| Framework | React 19 + Vite |
| Routing | React Router v7 |
| State Management | Zustand |
| HTTP Client | Axios (with auth interceptor) |
| Calendar | React Big Calendar + date-fns |
| Styling | Plain CSS (CSS variables) |

---

## ⚙️ Getting Started

### Prerequisites

- .NET 8 SDK  
- Node.js 18+

---

### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd userPunch
2. Configure Backend

Edit UserPunchApi/appsettings.json:

{
  "Jwt": {
    "Key": "replace-with-a-strong-secret-at-least-32-chars",
    "Issuer": "UserPunchApi",
    "Audience": "UserPunchApiUsers",
    "ExpiryMinutes": 60
  }
}
3. Run Backend
cd UserPunchApi
dotnet restore
dotnet ef database update
dotnet run
API: http://localhost:5007
Swagger: http://localhost:5007/swagger
4. Run Frontend
cd frontend
npm install
npm run dev
App: http://localhost:5173

⚠️ If Vite uses a different port, update CORS settings in Program.cs.

5. Create Test Account

Use Swagger or send request:

POST /api/v1/auth/register
{
  "email": "manager@test.com",
  "password": "Test123!",
  "firstName": "Test",
  "lastName": "Manager",
  "role": "Manager"
}
📡 API Overview
🔐 Auth — /api/v1/auth
Method	Endpoint	Access	Description
POST	/login	Public	Authenticate and return JWT
POST	/register	Public	Create new user
GET	/me	Authenticated	Get current user
⏱ Punch Records — /api/v1/punchrecords
Method	Endpoint	Access	Description
GET	/	Manager	Get all records
GET	/user/{userId}	Authenticated	Get user records
POST	/punchin	Authenticated	Clock in
POST	/punchout	Authenticated	Clock out
📝 Leave Requests — /api/v1/leaverequests
Method	Endpoint	Access	Description
GET	/	Manager	All requests
GET	/my	Authenticated	Own requests
POST	/	Authenticated	Submit request
PUT	/{id}/approve	Manager	Approve
PUT	/{id}/reject	Manager	Reject
📅 Schedules — /api/v1/schedules
Method	Endpoint	Access	Description
GET	/	Manager	All schedules
GET	/user/{userId}	Authenticated	User schedules
POST	/	Manager	Create shift
PUT	/{id}	Manager	Update shift
DELETE	/{id}	Manager	Delete shift
👥 Users — /api/v1/users
Method	Endpoint	Access	Description
GET	/	Authenticated	All users
GET	/{id}	Authenticated	Get user
POST	/	Manager	Create user
PUT	/{id}	Manager	Update user
DELETE	/{id}	Manager	Soft delete
🔐 Authentication & Authorization
JWT stored in localStorage
Axios automatically attaches Authorization: Bearer <token>
401 responses trigger logout and redirect to /login
Role-based access:
Employee
Manager
Enforced on:
Backend ([Authorize(Roles)])
Frontend (ProtectedRoute, RoleRoute)
🏗 Architecture Overview
HTTP Request
   ↓
Controller     → Input validation + JWT parsing
   ↓
Service        → Business logic
   ↓
Repository     → Data access (EF Core)
   ↓
AppDbContext → SQLite
🔮 Future Improvements
Refresh token support
Schedule conflict detection
Pagination for large datasets
Email notifications for leave approvals
Cloud deployment (Vercel / Railway / AWS)
Docker support
👤 Author

Pengyu Liu

📄 License

MIT
