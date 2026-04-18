# UserPunch — Backend API

ASP.NET Core 8 Web API for the UserPunch workforce management system. Handles authentication, employee management, attendance (punch in/out), shift scheduling, and leave requests.

---

## Tech Stack

| Concern | Library / Tool |
|---------|---------------|
| Framework | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Database | SQLite (via `userpunch.db`) |
| Auth | JWT Bearer — HMAC-SHA256 |
| Passwords | BCrypt.Net-Next |
| API Docs | Swagger / Swashbuckle |

---

## Architecture

The project follows a strict three-layer architecture so that each layer has one job and can be tested or swapped independently.

```
HTTP Request
     │
     ▼
 Controller        — parse request, call service, return HTTP response
     │
     ▼
  Service          — business rules, validation logic
     │
     ▼
 Repository        — EF Core database queries only
     │
     ▼
 AppDbContext  ──►  SQLite (userpunch.db)
```

Every dependency is injected via the built-in ASP.NET Core DI container, and every layer talks only to the layer directly below it.

---

## Project Structure

```
UserPunchApi/
├── Controllers/
│   └── V1/
│       ├── AuthController.cs
│       ├── UsersController.cs
│       ├── DepartmentsController.cs
│       ├── PunchRecordsController.cs
│       ├── SchedulesController.cs
│       └── LeaveRequestsController.cs
│
├── Services/
│   ├── Interfaces/                    # IAuthService, IUserService, ...
│   └── Implementations/               # AuthService, UserService, ...
│
├── Repositories/
│   ├── Interfaces/                    # IAuthRepository, IUserRepository, ...
│   └── Implementations/               # AuthRepository, UserRepository, ...
│
├── Models/                            # EF Core entities
│   ├── User.cs
│   ├── Department.cs
│   ├── PunchRecord.cs
│   ├── Schedule.cs
│   ├── LeaveRequest.cs
│   └── PunchEditLog.cs
│
├── Dtos/
│   └── V1/                            # Input/output models per domain
│       ├── AuthDtos/
│       ├── UserDtos/
│       ├── DepartmentDtos/
│       ├── PunchRecordsDtos/
│       ├── ScheduleDtos/
│       └── LeaveRequestsDtos/
│
├── Common/
│   ├── Roles.cs                       # "Manager" / "Employee" constants
│   ├── PunchRecordStatus.cs           # "Open" / "Closed"
│   ├── LeaveRequestStatus.cs          # "Pending" / "Approved" / "Rejected"
│   └── ServiceResult<T>.cs            # Generic result wrapper
│
├── Data/
│   └── AppDbContext.cs
│
├── Migrations/
└── Program.cs
```

---

## Data Models

### User
| Field | Type | Notes |
|-------|------|-------|
| Id | long | PK |
| FirstName / LastName | string | |
| Email | string | Unique index |
| PasswordHash | string | BCrypt hash |
| Role | string | `Employee` (default) or `Manager` |
| IsActive | bool | |
| DepartmentId | long? | FK → Department (Restrict on delete) |
| CreatedAt | DateTime | UTC |

### Department
| Field | Type | Notes |
|-------|------|-------|
| Id | long | PK |
| Name | string | |
| Description | string | |

### PunchRecord
| Field | Type | Notes |
|-------|------|-------|
| PunchRecordId | long | PK |
| UserId | long | FK → User (Cascade) |
| PunchInTime | DateTime | Unique per user |
| PunchOutTime | DateTime? | Null = still clocked in |
| ModifiedByManagerId | long? | Audit trail |
| ModificationReason | string? | |

> `(UserId, PunchInTime)` has a unique composite index to prevent duplicate punch-ins.

### Schedule
| Field | Type | Notes |
|-------|------|-------|
| ScheduleId | long | PK |
| UserId | long | FK → User (Cascade) |
| ShiftDate | DateTime | |
| StartTime | DateTime | Full datetime |
| EndTime | DateTime | Full datetime |
| ShiftName | string | e.g. "Morning Shift" |
| CreatedByManagerId | long? | Audit trail |

### LeaveRequest
| Field | Type | Notes |
|-------|------|-------|
| LeaveRequestId | long | PK |
| UserId | long | FK → User (Cascade) |
| StartDate | DateTime | |
| EndDate | DateTime | |
| LeaveReason | Reason (enum) | See values below |
| Status | string | `Pending` / `Approved` / `Rejected` |

`Reason` enum values:
```
0 = Annual_Leave
1 = Alt_Holiday_Leave
2 = Personal_Sick
3 = Person_Carers
4 = Long_Service_Leave
```

---

## Authentication & Authorisation

### Login flow
1. Client `POST /api/v1/auth/login` with `{ email, password }`
2. Server looks up user, verifies BCrypt hash
3. On success, returns a signed JWT containing `sub` (userId), `email`, `jti`, and `role` claims
4. Client attaches the token as `Authorization: Bearer <token>` on every subsequent request

### JWT configuration (`appsettings.json`)
```json
"Jwt": {
  "Key": "your-secret-key-at-least-32-characters",
  "Issuer": "UserPunchApi",
  "Audience": "UserPunchApiUsers",
  "ExpiryMinutes": 60
}
```

The middleware validates signature, issuer, audience, and expiry on every request. `MapInboundClaims = false` is set so claim names stay as-is (e.g. `"email"` not remapped to a long URN).

### Role-based access
```csharp
[Authorize]                          // any valid JWT
[Authorize(Roles = Roles.Manager)]   // Manager only
```

Role constants live in `Common/Roles.cs` so they can't drift between attributes and token generation.

---

## API Reference

### Auth — `/api/v1/auth`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/login` | Public | Email + password → JWT |
| POST | `/register` | Public | Register new user |
| POST | `/refresh` | Public | Exchange refresh token |
| POST | `/logout` | Bearer | Invalidate session |
| GET | `/me` | Bearer | Current user from token |

**Login request / response example:**
```json
// POST /api/v1/auth/login
{ "email": "alice@example.com", "password": "secret" }

// 200 OK
{
  "accessToken": "<jwt>",
  "userId": 1,
  "fullName": "Alice Smith",
  "email": "alice@example.com",
  "role": "Employee"
}
```

---

### Users — `/api/v1/users`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | Bearer | All users |
| GET | `/{id}` | Bearer | User by ID |
| POST | `/` | Manager | Create user |
| PUT | `/{id}` | Manager | Update user |
| DELETE | `/{id}` | Manager | Delete user |

---

### Departments — `/api/v1/departments`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | Bearer | All departments |
| GET | `/{id}` | Bearer | Department by ID |
| POST | `/` | Bearer | Create department |
| DELETE | `/{id}` | Bearer | Delete department |

---

### Punch Records — `/api/v1/punchrecords`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | Manager | All records |
| GET | `/{id}` | Bearer | Record by ID |
| GET | `/user/{userId}` | Bearer | Records for a user |
| POST | `/punchin` | Bearer | Clock in |
| POST | `/punchout` | Bearer | Clock out |

**Punch in / out examples:**
```json
// POST /api/v1/punchrecords/punchin
{ "userId": 1 }

// POST /api/v1/punchrecords/punchout
{ "userId": 1 }
```

Status is derived at query time: `PunchOutTime == null` → `"Open"`, otherwise `"Closed"`.

---

### Schedules — `/api/v1/schedules`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | Manager | All schedules |
| GET | `/{id}` | Bearer | Schedule by ID |
| GET | `/user/{userId}` | Bearer | Schedules for a user |
| POST | `/` | Manager | Create shift |
| PUT | `/{id}` | Manager | Update shift |
| DELETE | `/{id}` | Manager | Delete shift |

**Create schedule example:**
```json
// POST /api/v1/schedules
{
  "userId": 3,
  "shiftDate": "2026-05-01T00:00:00Z",
  "startTime": "2026-05-01T09:00:00Z",
  "endTime": "2026-05-01T17:00:00Z",
  "shiftName": "Morning Shift",
  "createdByManagerId": 1
}
```

---

### Leave Requests — `/api/v1/leaverequests`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/` | Manager | All requests |
| GET | `/my` | Bearer | Current user's requests |
| GET | `/{id}` | Bearer | Request by ID |
| POST | `/` | Bearer | Submit request |
| PUT | `/{id}/approve` | Manager | Approve |
| PUT | `/{id}/reject` | Manager | Reject |

**Submit leave request example:**
```json
// POST /api/v1/leaverequests
{
  "startDate": "2026-05-10T00:00:00Z",
  "endDate": "2026-05-12T00:00:00Z",
  "leaveReason": 0
}
```
> `userId` is extracted from the JWT `sub` claim — clients do not send it.

---

## ServiceResult Pattern

Business operations in the service layer return `ServiceResult<T>` instead of throwing exceptions for expected failures:

```csharp
public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    public static ServiceResult<T> Ok(T data, string message = "Success") { ... }
    public static ServiceResult<T> Fail(string message) { ... }
}
```

Controllers check `result.Success` and map accordingly — no try/catch needed for predictable states like "already punched in".

---

## Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`

### Run locally

```bash
cd UserPunchApi

# 1. Restore packages
dotnet restore

# 2. Apply database migrations
dotnet ef database update

# 3. Start the server
dotnet run
```

- API base URL: `http://localhost:5007`
- Swagger UI: `http://localhost:5007/swagger`

### CORS

The allowed frontend origin is configured in `Program.cs`:
```csharp
policy.WithOrigins("http://localhost:5174")
```
Update this if your frontend runs on a different port.

---

## Database Migrations

```bash
# Create a new migration after model changes
dotnet ef migrations add <MigrationName>

# Apply pending migrations
dotnet ef database update

# Roll back one migration
dotnet ef database update <PreviousMigrationName>
```

The SQLite database file (`userpunch.db`) is created automatically in the project root on first run.

---

## Key Design Decisions

- **DTO layer** — controllers never expose EF entities directly; DTOs define the API contract
- **Unique composite index** on `(UserId, PunchInTime)` prevents duplicate clock-ins at the database level
- **Cascade vs Restrict** — PunchRecords, Schedules, and LeaveRequests cascade-delete when a user is deleted; Department deletion is restricted to prevent orphaned users
- **Role constants** in `Common/Roles.cs` keep `[Authorize(Roles)]` attributes and token generation in sync
- **`MapInboundClaims = false`** preserves JWT claim names so `"sub"` stays `"sub"`, not a long .NET URN
