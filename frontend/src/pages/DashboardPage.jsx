import { useState, useEffect } from 'react';
import useAuthStore from '../store/authStore';
import { punchIn, punchOut, getPunchRecordsByUser } from '../api/punchApi';
import { formatDateTime } from '../utils/formatDate';

export default function DashboardPage() {
  const user = useAuthStore((s) => s.user);

  const [records, setRecords] = useState([]);
  const [loading, setLoading] = useState(true);
  const [actionLoading, setActionLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  // The open record is the one with no punchOutTime
  const openRecord = records.find((r) => r.status === 'Open');

  const fetchRecords = async () => {
    if (!user) return;
    try {
      const res = await getPunchRecordsByUser(user.id);
      // Sort descending by punch in time
      const sorted = [...res.data].sort(
        (a, b) => new Date(b.punchInTime) - new Date(a.punchInTime)
      );
      setRecords(sorted);
    } catch {
      setError('Failed to load punch records.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRecords();
  }, [user]);

  const handlePunchIn = async () => {
    setMessage('');
    setError('');
    setActionLoading(true);
    try {
      const res = await punchIn(user.id);
      setMessage(res.data.message || 'Punched in successfully.');
      await fetchRecords();
    } catch (err) {
      setError(err.response?.data?.message || 'Punch in failed.');
    } finally {
      setActionLoading(false);
    }
  };

  const handlePunchOut = async () => {
    setMessage('');
    setError('');
    setActionLoading(true);
    try {
      const res = await punchOut(user.id);
      setMessage(res.data.message || 'Punched out successfully.');
      await fetchRecords();
    } catch (err) {
      setError(err.response?.data?.message || 'Punch out failed.');
    } finally {
      setActionLoading(false);
    }
  };

  return (
    <div>
      <h1 className="page-title">Dashboard</h1>

      {/* Welcome */}
      <div className="card" style={{ marginBottom: 16 }}>
        <p style={{ fontSize: 16 }}>
          Welcome back, <strong>{user?.fullName}</strong>
        </p>
        <p style={{ color: 'var(--color-text-muted)', fontSize: 13 }}>
          Role: {user?.role}
        </p>
      </div>

      {/* Punch actions */}
      <div className="card" style={{ marginBottom: 16 }}>
        <h2 style={{ fontSize: 16, fontWeight: 600, marginBottom: 12 }}>Attendance</h2>

        <p style={{ marginBottom: 16, color: 'var(--color-text-muted)' }}>
          Status:{' '}
          <span className={`badge ${openRecord ? 'badge-open' : 'badge-closed'}`}>
            {openRecord ? 'Punched In' : 'Not Punched In'}
          </span>
        </p>

        {openRecord && (
          <p style={{ marginBottom: 16, fontSize: 13, color: 'var(--color-text-muted)' }}>
            Punched in at: {formatDateTime(openRecord.punchInTime)}
          </p>
        )}

        <div style={{ display: 'flex', gap: 10 }}>
          <button
            className="btn btn-primary"
            onClick={handlePunchIn}
            disabled={actionLoading || !!openRecord}
          >
            Punch In
          </button>
          <button
            className="btn btn-secondary"
            onClick={handlePunchOut}
            disabled={actionLoading || !openRecord}
          >
            Punch Out
          </button>
        </div>

        {message && <p className="success-text" style={{ marginTop: 10 }}>{message}</p>}
        {error && <p className="error-text" style={{ marginTop: 10 }}>{error}</p>}
      </div>

      {/* Recent records */}
      <div className="card">
        <h2 style={{ fontSize: 16, fontWeight: 600, marginBottom: 12 }}>Recent Records</h2>

        {loading ? (
          <p style={{ color: 'var(--color-text-muted)' }}>Loading...</p>
        ) : records.length === 0 ? (
          <p style={{ color: 'var(--color-text-muted)' }}>No punch records yet.</p>
        ) : (
          <table style={tableStyle.table}>
            <thead>
              <tr>
                <th style={tableStyle.th}>Punch In</th>
                <th style={tableStyle.th}>Punch Out</th>
                <th style={tableStyle.th}>Status</th>
              </tr>
            </thead>
            <tbody>
              {records.slice(0, 5).map((r) => (
                <tr key={r.punchRecordId}>
                  <td style={tableStyle.td}>{formatDateTime(r.punchInTime)}</td>
                  <td style={tableStyle.td}>{r.punchOutTime ? formatDateTime(r.punchOutTime) : '-'}</td>
                  <td style={tableStyle.td}>
                    <span className={`badge badge-${r.status?.toLowerCase()}`}>{r.status}</span>
                  </td>
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
