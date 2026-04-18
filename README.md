# UserPunch — Workforce Management System

A full-stack workforce management MVP for employee scheduling, attendance tracking, and leave management.

Built with **ASP.NET Core 8** (backend) and **React 19 + Vite** (frontend).

---

## Features

### All Authenticated Users
- **Punch in / Punch out** — clock in and out from the dashboard with live status
- **Leave requests** — submit requests and track approval status
- **Schedule** — view assigned shifts on an interactive calendar

### Managers Only
- **Schedule management** — assign shifts to employees via calendar
- **Leave approvals** — approve or reject pending requests
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
| Framework | React 19 + Vite |
| Routing | React Router v7 |
| State | Zustand |
| HTTP | Axios (with auth interceptor) |
| Calendar | React Big Calendar + date-fns |
| Styles | Plain CSS with CSS variables |

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+

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
- API: `http://localhost:5007`
- Swagger UI: `http://localhost:5007/swagger`

### 4. Run the frontend
```bash
cd frontend
npm install
npm run dev
```
- App: `http://localhost:5173`

> If Vite picks a different port, update the CORS origin in `UserPunchApi/Program.cs` to match.

### 5. Create a test account

Register via Swagger or POST directly:
```json
POST /api/v1/auth/register
{
  "email": "manager@test.com",
  "password": "Test123!",
  "firstName": "Test",
  "lastName": "Manager",
  "role": "Manager"
}
```

---

## API Reference

### Auth — `/api/v1/auth`
| Method | Endpoint | Access | Description |
|---|---|---|---|
| POST | `/login` | Public | Email + password → JWT |
| POST | `/register` | Public | Create account |
| GET | `/me` | Authenticated | Current user info |

### Punch Records — `/api/v1/punchrecords`
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/` | Manager | All records |
| GET | `/user/{userId}` | Authenticated | Records for a user |
| POST | `/punchin` | Authenticated | Clock in |
| POST | `/punchout` | Authenticated | Clock out |

### Leave Requests — `/api/v1/leaverequests`
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/` | Manager | All requests |
| GET | `/my` | Authenticated | Own requests |
| POST | `/` | Authenticated | Submit request |
| PUT | `/{id}/approve` | Manager | Approve |
| PUT | `/{id}/reject` | Manager | Reject |

### Schedules — `/api/v1/schedules`
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/` | Manager | All schedules |
| GET | `/user/{userId}` | Authenticated | Schedules for a user |
| POST | `/` | Manager | Create shift |
| PUT | `/{id}` | Manager | Update shift |
| DELETE | `/{id}` | Manager | Delete shift |

### Users — `/api/v1/users`
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/` | Authenticated | All users |
| GET | `/{id}` | Authenticated | User by ID |
| POST | `/` | Manager | Create user |
| PUT | `/{id}` | Manager | Update user |
| DELETE | `/{id}` | Manager | Soft delete |

---

## Authentication

- Login returns a JWT stored in `localStorage`
- Axios automatically attaches `Authorization: Bearer <token>` on every request
- `401` responses clear the session and redirect to `/login`
- Two roles: `Employee` (default) and `Manager`
- Role-based access enforced on both backend (`[Authorize(Roles)]`) and frontend (`RoleRoute`)

---

## Architecture

HTTP Request
│
▼
Controller     ← validates input, extracts JWT claims
│
▼
Service        ← business logic, validation rules
│
▼
Repository     ← EF Core queries
│
▼
AppDbContext → SQLite

---

## Potential Next Steps

- Refresh token rotation (DB-backed)
- Shift conflict detection
- Pagination on long lists
- Email notifications for leave approvals
- Cloud deployment (Railway + Vercel)
- Docker support

---

## Author

Pengyu Liu

## License

MIT
