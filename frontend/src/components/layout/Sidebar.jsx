import { NavLink } from 'react-router-dom';
import useAuthStore from '../../store/authStore';

const NAV_ITEMS = [
  { label: 'Dashboard', path: '/dashboard' },
  { label: 'Leave Requests', path: '/leave-requests' },
  { label: 'Schedule', path: '/schedule' },
];

const MANAGER_ITEMS = [
  { label: 'User Management', path: '/admin/users' },
  { label: 'Manage Schedules', path: '/admin/schedules' },
];

export default function Sidebar() {
  const user = useAuthStore((s) => s.user);
  const logout = useAuthStore((s) => s.logout);
  const isManager = user?.role === 'Manager';

  const linkStyle = ({ isActive }) => ({
    display: 'block',
    padding: '10px 16px',
    borderRadius: '4px',
    color: isActive ? '#fff' : 'var(--color-sidebar-text)',
    background: isActive ? 'var(--color-sidebar-active)' : 'transparent',
    fontWeight: isActive ? 600 : 400,
    fontSize: 14,
    marginBottom: 2,
    transition: 'background 0.15s',
  });

  return (
    <aside style={styles.sidebar}>
      <div style={styles.brand}>UserPunch</div>

      <div style={styles.userInfo}>
        <p style={styles.userName}>{user?.fullName}</p>
        <p style={styles.userRole}>{user?.role}</p>
      </div>

      <nav style={{ flex: 1 }}>
        {NAV_ITEMS.map((item) => (
          <NavLink key={item.path} to={item.path} style={linkStyle}>
            {item.label}
          </NavLink>
        ))}

        {isManager && (
          <>
            <div style={styles.divider} />
            <p style={styles.sectionLabel}>Manager</p>
            {MANAGER_ITEMS.map((item) => (
              <NavLink key={item.path} to={item.path} style={linkStyle}>
                {item.label}
              </NavLink>
            ))}
          </>
        )}
      </nav>

      <button
        className="btn"
        style={styles.logoutBtn}
        onClick={logout}
      >
        Logout
      </button>
    </aside>
  );
}

const styles = {
  sidebar: {
    width: 220,
    minHeight: '100vh',
    background: 'var(--color-sidebar)',
    display: 'flex',
    flexDirection: 'column',
    padding: '20px 12px',
    flexShrink: 0,
  },
  brand: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 700,
    marginBottom: 24,
    paddingLeft: 4,
  },
  userInfo: {
    marginBottom: 20,
    paddingLeft: 4,
    borderBottom: '1px solid #334155',
    paddingBottom: 16,
  },
  userName: {
    color: '#fff',
    fontWeight: 500,
    fontSize: 14,
  },
  userRole: {
    color: 'var(--color-sidebar-text)',
    fontSize: 12,
    marginTop: 2,
  },
  divider: {
    borderTop: '1px solid #334155',
    margin: '12px 0',
  },
  sectionLabel: {
    color: '#64748b',
    fontSize: 11,
    textTransform: 'uppercase',
    letterSpacing: '0.05em',
    padding: '4px 16px',
    marginBottom: 4,
  },
  logoutBtn: {
    marginTop: 'auto',
    background: 'transparent',
    color: 'var(--color-sidebar-text)',
    border: '1px solid #334155',
    width: '100%',
    padding: '8px',
  },
};
