# UserPunch — Frontend

The frontend for **UserPunch**, an internal employee attendance and leave management system. Built as an MVP-focused single-page application with clean structure, role-aware routing, and direct integration with the UserPunchApi backend.

---

## Tech Stack

| Tool | Purpose |
|---|---|
| [React 19](https://react.dev) | UI framework |
| [Vite 8](https://vite.dev) | Build tool and dev server |
| [React Router v7](https://reactrouter.com) | Client-side routing |
| [Zustand](https://zustand-demo.pmnd.rs) | Global auth state management |
| [Axios](https://axios-http.com) | HTTP client with interceptors |
| Plain CSS | Styling via CSS variables and utility classes |

No Redux, no Tailwind, no heavy UI library.

---

## Features

### Authentication
- Email/password login via `/api/v1/auth/login`
- JWT access token stored in `localStorage`
- Zustand store holds `{ token, user }` — persisted across page refreshes
- Automatic `Authorization: Bearer <token>` header on every request
- 401 responses automatically clear session and redirect to `/login`

### Role-aware Access
- Routes protected by `ProtectedRoute` (login required) and `RoleRoute` (role required)
- `Manager` role unlocks the User Management page in the sidebar
- Non-authorized access redirects to `/not-authorized`

### Pages

| Route | Access | Description |
|---|---|---|
| `/login` | Public | Email + password login form |
| `/dashboard` | All users | Punch in/out with live status, recent punch records |
| `/leave-requests` | All users | List of own leave requests with status badges |
| `/leave-requests/new` | All users | Submit a new leave request with date validation |
| `/schedule` | All users | View own assigned shifts in a table |
| `/admin/users` | Manager only | Read-only list of all users with role and department |
| `/not-authorized` | Public | Shown when a user accesses a restricted page |

### Layout
- Persistent sidebar with role-aware navigation
- User name and role displayed in sidebar
- Logout button always visible

---

## Project Structure

```
src/
├── api/
│   ├── axiosClient.js       # Shared Axios instance with token interceptor
│   ├── authApi.js           # login, getMe, logout
│   ├── punchApi.js          # punchIn, punchOut, getPunchRecordsByUser
│   ├── leaveApi.js          # getLeaveRequests, createLeaveRequest, approve, reject
│   ├── scheduleApi.js       # getSchedules, getSchedulesByUser
│   └── userApi.js           # getAllUsers, getUserById
│
├── components/
│   ├── layout/
│   │   ├── AppLayout.jsx    # Shell: sidebar + <Outlet />
│   │   └── Sidebar.jsx      # Role-aware nav + logout
│   └── common/
│       ├── ProtectedRoute.jsx   # Redirects to /login if no token
│       └── RoleRoute.jsx        # Redirects to /not-authorized if wrong role
│
├── pages/
│   ├── LoginPage.jsx
│   ├── DashboardPage.jsx
│   ├── LeaveRequestListPage.jsx
│   ├── NewLeaveRequestPage.jsx
│   ├── SchedulePage.jsx
│   ├── AdminUsersPage.jsx
│   └── NotAuthorizedPage.jsx
│
├── store/
│   └── authStore.js         # Zustand: token, user, login(), logout(), setUser()
│
├── utils/
│   ├── token.js             # localStorage token helpers
│   └── formatDate.js        # formatDate(), formatDateTime()
│
├── styles/
│   └── global.css           # CSS variables, utility classes (card, btn, badge, form)
│
├── App.jsx                  # BrowserRouter + all route definitions
└── main.jsx                 # React entry point
```

---

## Getting Started

The backend must be running first:

```bash
# In /UserPunchApi
dotnet run
# Listening on http://localhost:5007
```

Then start the frontend:

```bash
cd frontend
npm install
npm run dev
# Running on http://localhost:5173
```

---

## Backend Connection

Base URL: `http://localhost:5007/api/v1`

Configured in [src/api/axiosClient.js](src/api/axiosClient.js). Change the `baseURL` there if the backend port changes.

---

## State Management

Zustand is used **only** for global auth state:

```js
{
  token,       // JWT access token (string | null)
  user,        // { id, fullName, email, role }
  login(),     // saves token + user to store and localStorage
  logout(),    // clears store and localStorage
  setUser()    // updates user info only
}
```

All page-level state (forms, loading, errors) lives in local component state.

---

## Development Notes

- Each API domain has its own file under `src/api/` — do not mix API calls across files
- Do not add business logic outside of page components or API files
- Extract shared components only when the same JSX appears in 3+ places
- The `global.css` provides utility classes (`card`, `btn`, `btn-primary`, `form-control`, `badge-*`) — use these before writing new styles
