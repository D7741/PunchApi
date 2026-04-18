import { useState, useEffect } from 'react';
import { getAllUsers } from '../api/userApi';
import { formatDate } from '../utils/formatDate';

export default function AdminUsersPage() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    getAllUsers()
      .then((res) => setUsers(res.data))
      .catch(() => setError('Failed to load users.'))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div>
      <h1 className="page-title">User Management</h1>

      <div className="card">
        {loading ? (
          <p style={{ color: 'var(--color-text-muted)' }}>Loading...</p>
        ) : error ? (
          <p className="error-text">{error}</p>
        ) : users.length === 0 ? (
          <p style={{ color: 'var(--color-text-muted)' }}>No users found.</p>
        ) : (
          <table style={tableStyle.table}>
            <thead>
              <tr>
                <th style={tableStyle.th}>Name</th>
                <th style={tableStyle.th}>Email</th>
                <th style={tableStyle.th}>Role</th>
                <th style={tableStyle.th}>Department</th>
                <th style={tableStyle.th}>Active</th>
                <th style={tableStyle.th}>Joined</th>
              </tr>
            </thead>
            <tbody>
              {users.map((u) => (
                <tr key={u.id}>
                  <td style={tableStyle.td}>{u.firstName} {u.lastName}</td>
                  <td style={tableStyle.td}>{u.email}</td>
                  <td style={tableStyle.td}>{u.role}</td>
                  <td style={tableStyle.td}>{u.departmentName || '-'}</td>
                  <td style={tableStyle.td}>
                    <span className={`badge ${u.isActive ? 'badge-approved' : 'badge-rejected'}`}>
                      {u.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td style={tableStyle.td}>{formatDate(u.createdAt)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}

const tableStyle = {
  table: { width: '100%', borderCollapse: 'collapse' },
  th: {
    textAlign: 'left',
    padding: '8px 10px',
    borderBottom: '1px solid var(--color-border)',
    color: 'var(--color-text-muted)',
    fontWeight: 500,
    fontSize: 13,
  },
  td: {
    padding: '8px 10px',
    borderBottom: '1px solid var(--color-border)',
    fontSize: 13,
  },
};
