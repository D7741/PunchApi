# Frontend Agent Development Doc

## Project Goal

Build the frontend for UserPunchApi step by step in a controlled and maintainable way.
This frontend is for an employee attendance and leave management system.

Main features include:
- User login
- Punch in / punch out
- View own punch records
- Create leave requests
- View leave requests
- View schedules
- Manager-only user/admin pages

The backend already exists as an ASP.NET Core Web API.

The frontend should focus on:
- clean structure
- correct API integration
- simple and usable UI
- role-aware routing
- incremental delivery

Do not over-engineer. Do not build everything at once. Do not create many abstract components too early.

---

## Core Principle

Build from outside to inside, and from simple to complex.

Order of thinking:
1. Define all pages first
2. Build page skeletons
3. Connect routing
4. Add auth flow
5. Connect APIs
6. Extract reusable components only when repetition appears

---

## Tech Stack

Use the following stack unless explicitly changed:
- React
- Vite
- React Router
- Zustand
- Axios
- Plain CSS / simple module CSS / minimal styling approach

Avoid introducing unnecessary libraries unless needed.
Do not add Redux, React Query, Tailwind, MUI, or complicated form libraries unless explicitly requested.

---

## App Pages / Routes

| Route | Description |
|---|---|
| `/login` | Login page |
| `/dashboard` | Main page (punch in/out, quick summary) |
| `/leave-requests` | Leave request list |
| `/leave-requests/new` | Create a new leave request |
| `/schedule` | Schedule page |
| `/admin/users` | User management page (Manager only) |

Optional later routes if needed:
- `/punch-records` — Full punch record history
- `/profile` — Current user profile
- `/not-authorized` — Permission denied page

---

## Recommended Build Order

Follow this exact order:

1. App.jsx + React Router setup
2. authStore.js using Zustand
3. Axios client config with automatic Bearer token
4. Login page
5. ProtectedRoute
6. Dashboard page
7. Leave request pages
8. Schedule page
9. Admin users page
10. Shared components extraction
11. Styling polish

Do not skip ahead.

---

## Folder Structure

```
src/
 api/
   axiosClient.js
   authApi.js
   punchApi.js
   leaveApi.js
   scheduleApi.js
   userApi.js

 components/
   layout/
     AppLayout.jsx
     Navbar.jsx
     Sidebar.jsx
   common/
     ProtectedRoute.jsx
     RoleRoute.jsx
     LoadingSpinner.jsx
     ErrorMessage.jsx
     EmptyState.jsx

 pages/
   LoginPage.jsx
   DashboardPage.jsx
   LeaveRequestListPage.jsx
   NewLeaveRequestPage.jsx
   SchedulePage.jsx
   AdminUsersPage.jsx

 store/
   authStore.js

 utils/
   token.js
   formatDate.js

 styles/
   global.css

 App.jsx
 main.jsx
```

Keep it flat and understandable. Do not create too many nested folders unless necessary.

---

## Implementation Rules

### 1. Build page skeletons first

Before adding detailed UI, each page should render:
- page title
- basic placeholder section
- future content area

### 2. Authentication must be implemented early

The frontend must support: login, token storage, current user storage, protected routes, logout.

Use Zustand to store:
```js
{
  token: null,
  user: null,
  login: () => {},
  logout: () => {},
  setUser: () => {}
}
```

Persist token/user to localStorage.

### 3. Axios must automatically attach token

Create a shared axios instance. Requirements:
- base URL configurable
- automatically attach `Authorization: Bearer <token>`
- centralized error handling for 401
- easy to reuse across all API modules

### 4. Route protection rules

Pages that require login: `/dashboard`, `/leave-requests`, `/leave-requests/new`, `/schedule`, `/admin/users`

Manager-only page: `/admin/users`

- If user is not logged in → redirect to `/login`
- If user is logged in but not authorized → redirect to `/not-authorized` or `/dashboard`

### 5. UI should be simple first

Initial version should prioritize: working login, successful routing, successful API calls, clear page structure.

Do not spend time on advanced animation or fancy design in early stages.

---

## Data / Role Assumptions

Backend returns a user object similar to:
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Smith",
  "email": "john@example.com",
  "role": "Manager"
}
```

Role values: `Employee`, `Manager`

Frontend must use role to determine access. If backend shape differs, adapt the API mapping layer rather than rewriting the app structure.

---

## API Integration Strategy

Create separate API files by domain.

**authApi.js** — `login`

**punchApi.js** — get current/open punch record if needed, punch in, punch out, get own punch history

**leaveApi.js** — get leave requests, create leave request

**scheduleApi.js** — get schedules

**userApi.js** — get all users, maybe create/update/deactivate user later

Keep API functions small and explicit. Example style:
```js
export const login = (payload) => axiosClient.post('/auth/login', payload);
```

---

## Backend API Base URL

`http://localhost:5007`

## Backend Endpoints

### Auth
| Method | Endpoint |
|---|---|
| POST | `/api/v1/auth/login` |
| POST | `/api/v1/auth/register` |
| POST | `/api/v1/auth/refresh` |
| POST | `/api/v1/auth/logout` |
| GET | `/api/v1/auth/me` |

### Departments
| Method | Endpoint |
|---|---|
| GET | `/api/v1/departments` |
| POST | `/api/v1/departments` |
| GET | `/api/v1/departments/{id}` |
| PUT | `/api/v1/departments/{id}` |
| DELETE | `/api/v1/departments/{id}` |

### Leave Requests
| Method | Endpoint |
|---|---|
| GET | `/api/v1/leaverequests` |
| POST | `/api/v1/leaverequests` |
| GET | `/api/v1/leaverequests/{id}` |
| PUT | `/api/v1/leaverequests/{id}/approve` |
| PUT | `/api/v1/leaverequests/{id}/reject` |

### Punch Records
| Method | Endpoint |
|---|---|
| GET | `/api/v1/punchrecords` |
| GET | `/api/v1/punchrecords/{id}` |
| GET | `/api/v1/punchrecords/user/{userId}` |
| POST | `/api/v1/punchrecords/punchin` |
| POST | `/api/v1/punchrecords/punchout` |

### Schedules
| Method | Endpoint |
|---|---|
| GET | `/api/v1/schedules` |
| POST | `/api/v1/schedules` |
| GET | `/api/v1/schedules/{id}` |
| PUT | `/api/v1/schedules/{id}` |
| DELETE | `/api/v1/schedules/{id}` |
| GET | `/api/v1/schedules/user/{userId}` |

### Users
| Method | Endpoint |
|---|---|
| GET | `/api/v1/users` |
| POST | `/api/v1/users` |
| GET | `/api/v1/users/{id}` |
| PUT | `/api/v1/users/{id}` |
| DELETE | `/api/v1/users/{id}` |

---

## Page Requirements

### 1. Login Page

**Goal:** Get frontend and backend connected first.

Must have:
- email input
- password input
- submit button
- loading state
- error message display

Behavior: on success, save token + user into store and redirect to `/dashboard`.

Do not add yet: forgot password, register page, social login, form library complexity.

### 2. Dashboard Page

**Goal:** Main daily-use page for employee.

Must have:
- welcome section
- user basic info
- punch in button
- punch out button
- current punch status
- optional recent records summary

Behavior: show correct action depending on punch state, call backend punch endpoints, refresh state after action.

### 3. Leave Request List Page

Must have:
- title
- button to create new request
- leave request list/table with: start date, end date, leave reason, status

Nice to have later: filter by status, sorting.

### 4. New Leave Request Page

Must have:
- start date
- end date
- leave reason select
- submit button
- validation
- success/error message

Validation: end date cannot be before start date; required fields cannot be empty.

### 5. Schedule Page

Must have:
- schedule list or simple table
- shift date, start time, end time, shift name if available

Initial display can be a table. Do not build a full calendar UI unless explicitly required.

### 6. Admin Users Page

**Access:** Manager only.

Must have:
- user list with: name, email, role, department if available, active status if available

Initial version can be read-only. Do not force full CRUD unless backend endpoints are ready.

---

## Shared Components to Extract Later

Only extract components after duplication appears. Likely shared components:
- AppLayout
- Navbar
- Sidebar
- ProtectedRoute
- RoleRoute
- LoadingSpinner
- ErrorMessage
- EmptyState
- PageHeader
- DataTable

Do not create all of these on day one.

---

## Layout Guidance

Use a very simple app layout after login:
- top navbar or side navigation
- main content area
- logout button visible
- role/user info visible somewhere

Suggested navigation items:
- Dashboard
- Leave Requests
- Schedule
- Admin Users (only if Manager)

Keep navigation role-aware.

---

## Styling Guidance

Style priorities: clean, readable, consistent spacing, obvious buttons/forms/states.

Avoid: overly flashy colors, complex animations, oversized component systems.

Use a professional internal-tool style.

---

## State Management Rules

Use Zustand **only** for global auth state and minimal app-wide user info.

Do not put every page's local form state into Zustand.

Use **local component state** for: forms, loading flags, table UI state, filters.

Use **global state** only when shared across pages.

---

## Error Handling Rules

Every API-connected page should handle: loading state, empty state, API failure state.

401 should trigger logout or redirect to login. Do not leave unhandled promise errors.

---

## Code Quality Rules

When generating code:
- Keep components small and readable
- Prefer explicit code over clever abstractions
- Use clear names
- Add comments only when useful
- Do not generate dead code or fake placeholder business logic
- Do not invent backend endpoints that do not exist
- If backend response shape is uncertain, isolate mapping in API layer
- Finish one page before starting the next
- Make code runnable at each step
- Do not refactor too early

---

## Delivery Strategy (Phases)

### Phase 1
- project setup
- router setup
- Zustand auth store
- axios client
- login page
- protected route

### Phase 2
- dashboard page
- basic layout
- logout flow

### Phase 3
- leave request list
- new leave request page

### Phase 4
- schedule page

### Phase 5
- admin users page
- role-based access polish

### Phase 6
- shared components extraction
- styling improvements
- cleanup

Each phase should leave the app in a runnable state.

---

## What the AI Agent Should NOT Do

- Redesign the app scope
- Add unnecessary pages
- Introduce a large UI library without permission
- Create fake backend endpoints
- Build advanced calendar widgets too early
- Build full admin CRUD unless backend supports it
- Over-abstract components in the beginning
- Mix business logic everywhere

---

## Important Constraint

This project is an MVP-style internal management app. The frontend should prioritize **correctness**, **maintainability**, **API integration**, and **role control** over visual complexity, fancy interactions, or premature optimization.
