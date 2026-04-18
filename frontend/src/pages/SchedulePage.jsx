import { useState, useEffect } from 'react';
import useAuthStore from '../store/authStore';
import { getSchedulesByUser } from '../api/scheduleApi';
import { formatDate, formatDateTime } from '../utils/formatDate';

export default function SchedulePage() {
  const user = useAuthStore((s) => s.user);
  const [schedules, setSchedules] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!user) return;
    getSchedulesByUser(user.id)
      .then((res) => {
        const sorted = [...res.data].sort(
          (a, b) => new Date(a.shiftDate) - new Date(b.shiftDate)
        );
        setSchedules(sorted);
      })
      .catch(() => setError('Failed to load schedules.'))
      .finally(() => setLoading(false));
  }, [user]);

  return (
    <div>
      <h1 className="page-title">My Schedule</h1>

      <div className="card">
        {loading ? (
          <p style={{ color: 'var(--color-text-muted)' }}>Loading...</p>
        ) : error ? (
          <p className="error-text">{error}</p>
        ) : schedules.length === 0 ? (
          <p style={{ color: 'var(--color-text-muted)' }}>No schedules assigned.</p>
        ) : (
          <table style={tableStyle.table}>
            <thead>
              <tr>
                <th style={tableStyle.th}>Shift Date</th>
                <th style={tableStyle.th}>Shift Name</th>
                <th style={tableStyle.th}>Start Time</th>
                <th style={tableStyle.th}>End Time</th>
              </tr>
            </thead>
            <tbody>
              {schedules.map((s) => (
                <tr key={s.scheduleId}>
                  <td style={tableStyle.td}>{formatDate(s.shiftDate)}</td>
                  <td style={tableStyle.td}>{s.shiftName || '-'}</td>
                  <td style={tableStyle.td}>{formatDateTime(s.startTime)}</td>
                  <td style={tableStyle.td}>{formatDateTime(s.endTime)}</td>
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
