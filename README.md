# UserPunch — Workforce Management System

A full-stack workforce management MVP for handling employee scheduling, attendance, and leave requests — built with ASP.NET Core 8 and React 19.

---

## Features

### All Authenticated Users
- **Punch in / Punch out** — clock in and out from the dashboard with live status
- **Leave requests** — submit requests and track their approval status
- **Schedule** — view assigned shifts on an interactive calendar

### Managers Only
- **Schedule management** — assign shifts to employees via a calendar (click a date to create, click a shift to delete)
- **Leave approvals** — approve or reject pending employee leave requests
- **User management** — view all registered users, roles, and departments

---

## Tech Stack

### Backend
| | |
|---|---|
| Framework | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Database | SQLite |
| Auth | JWT Bearer (HMAC-SHA256) |
| Passwords | BCrypt |
| Docs | Swagger / OpenAPI |

### Frontend
| | |
|---|---|
| Framework | React 19 + Vite 8 |
| Routing | React Router v7 |
| State | Zustand |
| HTTP | Axios (with auth interceptor) |
| Calendar | React Big Calendar + date-fns |
| Styles | Plain CSS with CSS variables |

---

## Project Structure

```
userPunch/
├── UserPunchApi/                  # ASP.NET Core backend
│   ├── Controllers/V1/            # Auth, Users, PunchRecords, Schedules, LeaveRequests, Departments
│   ├── Services/
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Models/                    # User, PunchRecord, Schedule, LeaveRequest, Department
│   ├── Dtos/V1/                   # Input/output models per domain
│   ├── Common/                    # Roles, status constants, ServiceResult
│   ├── Data/                      # AppDbContext
│   ├── Migrations/
│   └── Program.cs
│
└── frontend/                      # React frontend
    └── src/
        ├── api/                   # axiosClient + one file per domain
        ├── components/
        │   ├── layout/            # AppLayout, Sidebar
        │   └── common/            # ProtectedRoute, RoleRoute
        ├── pages/                 # One file per page/route
        ├── store/                 # authStore (Zustand)
        ├── utils/                 # token.js, formatDate.js
        └── styles/                # global.css
```

---

## API Reference

### Auth — `/api/v1/auth`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| POST | `/login` | Public | Email + password → JWT |
| POST | `/register` | Public | Create account |
| GET | `/me` | Authenticated | Current user info |

### Punch Records — `/api/v1/punchrecords`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/` | Manager | All records |
| GET | `/user/{userId}` | Authenticated | Records for a user |
| POST | `/punchin` | Authenticated | Clock in |
| POST | `/punchout` | Authenticated | Clock out |

### Leave Requests — `/api/v1/leaverequests`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/` | Manager | All requests |
| GET | `/my` | Authenticated | Own requests |
| POST | `/` | Authenticated | Submit request |
| PUT | `/{id}/approve` | Manager | Approve |
| PUT | `/{id}/reject` | Manager | Reject |

### Schedules — `/api/v1/schedules`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/` | Manager | All schedules |
| GET | `/user/{userId}` | Authenticated | Schedules for a user |
| POST | `/` | Manager | Create shift |
| PUT | `/{id}` | Manager | Update shift |
| DELETE | `/{id}` | Manager | Delete shift |

### Users — `/api/v1/users`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/` | Authenticated | All users |
| GET | `/{id}` | Authenticated | User by ID |
| POST | `/` | Manager | Create user |
| PUT | `/{id}` | Manager | Update user |
| DELETE | `/{id}` | Manager | Delete user |

---

## Frontend Routes

| Route | Access | Page |
|-------|--------|------|
| `/login` | Public | Login |
| `/dashboard` | Authenticated | Punch in/out + recent records |
| `/leave-requests` | Authenticated | Leave request list + approval (managers) |
| `/leave-requests/new` | Employee | Submit new leave request |
| `/schedule` | Employee | Personal shift calendar |
| `/admin/users` | Manager | User list |
| `/admin/schedules` | Manager | Schedule management calendar |

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)

### 1. Clone the repo
```bash
git clone <your-repo-url>
cd userPunch
```

### 2. Configure the backend

Edit `UserPunchApi/appsettings.json`:
```json
{
  "Jwt": {
    "Key": "replace-with-a-strong-secret-at-least-32-chars",
    "Issuer": "UserPunchApi",
    "Audience": "UserPunchApiUsers",
    "ExpiryMinutes": 60
  }
}
```

### 3. Run the backend
```bash
cd UserPunchApi
dotnet restore
dotnet ef database update
dotnet run
```
API runs on `http://localhost:5007`  
Swagger UI: `http://localhost:5007/swagger`

### 4. Run the frontend
```bash
cd frontend
npm install
npm run dev
```
App runs on `http://localhost:5173`

> If Vite picks a different port, update the CORS origin in `UserPunchApi/Program.cs` to match.

---

## Authentication

- Login returns a JWT stored in `localStorage`
- Axios automatically attaches `Authorization: Bearer <token>` on every request
- 401 responses clear the session and redirect to `/login`
- Two roles: **Employee** (default) and **Manager**
- Role-based access enforced on both backend (`[Authorize(Roles)]`) and frontend (`RoleRoute`)

---

## Architecture

```
HTTP Request
    │
    ▼
Controller        ← validates input, extracts JWT claims
    │
    ▼
Service           ← business logic, validation rules
    │
    ▼
Repository        ← EF Core queries
    │
    ▼
AppDbContext → SQLite
```

---

## Potential Next Steps

- Shift conflict detection (prevent overlapping schedules)
- Pagination on long lists
- Email/push notifications for leave approvals
- Reporting & analytics dashboard
- Cloud deployment (Azure App Service + Azure SQL)
- Refresh token rotation
- Docker support

---

## Author

**Pengyu Liu**

---

## License

MIT
