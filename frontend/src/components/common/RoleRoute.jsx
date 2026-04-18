import { Navigate } from 'react-router-dom';
import useAuthStore from '../../store/authStore';

// Redirects to /not-authorized if user does not have the required role
export default function RoleRoute({ children, allowedRole }) {
  const user = useAuthStore((s) => s.user);

  if (!user || user.role !== allowedRole) {
    return <Navigate to="/not-authorized" replace />;
  }

  return children;
}
