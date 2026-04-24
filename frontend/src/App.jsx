import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';

import AppLayout from './components/layout/AppLayout';
import ProtectedRoute from './components/common/ProtectedRoute';
import RoleRoute from './components/common/RoleRoute';

import LoginPage from './pages/LoginPage';
import SignUpPage from './pages/SignUpPage';
import DashboardPage from './pages/DashboardPage';
import LeaveRequestListPage from './pages/LeaveRequestListPage';
import NewLeaveRequestPage from './pages/NewLeaveRequestPage';
import SchedulePage from './pages/SchedulePage';
import AdminUsersPage from './pages/AdminUsersPage';
import ManagerSchedulePage from './pages/ManagerSchedulePage';
import NotAuthorizedPage from './pages/NotAuthorizedPage';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/signup" element={<SignUpPage />} />
        <Route path="/not-authorized" element={<NotAuthorizedPage />} />

        {/* Protected — requires login, renders inside AppLayout */}
        <Route
          element={
            <ProtectedRoute>
              <AppLayout />
            </ProtectedRoute>
          }
        >
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/leave-requests" element={<LeaveRequestListPage />} />
          <Route path="/leave-requests/new" element={<NewLeaveRequestPage />} />
          <Route path="/schedule" element={<SchedulePage />} />

          {/* Manager only */}
          <Route
            path="/admin/users"
            element={
              <RoleRoute allowedRole="Manager">
                <AdminUsersPage />
              </RoleRoute>
            }
          />
          <Route
            path="/admin/schedules"
            element={
              <RoleRoute allowedRole="Manager">
                <ManagerSchedulePage />
              </RoleRoute>
            }
          />
        </Route>

        {/* Fallback */}
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
}
