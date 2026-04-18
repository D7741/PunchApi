import { useState, useEffect } from 'react';
import { getSchedules, createSchedule, deleteSchedule } from '../api/scheduleApi';
import { getAllUsers } from '../api/userApi';
import { formatDate } from '../utils/formatDate';
import useAuthStore from '../store/authStore';

const EMPTY_FORM = { userId: '', shiftDate: '', startTime: '', endTime: '', shiftName: '' };

export default function ManagerSchedulePage() {
  const user = useAuthStore((s) => s.user);
  const [schedules, setSchedules] = useState([]);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [form, setForm] = useState(EMPTY_FORM);
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState('');
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    Promise.all([getSchedules(), getAllUsers()])
      .then(([schedRes, userRes]) => {
        setSchedules(schedRes.data);
        setUsers(userRes.data.filter((u) => u.role !== 'Manager'));
      })
      .catch(() => setError('Failed to load data.'))
      .finally(() => setLoading(false));
  }, []);

  const handleChange = (e) => setForm((f) => ({ ...f, [e.target.name]: e.target.value }));

  const handleCreate = async (e) => {
    e.preventDefault();
    if (!form.userId || !form.shiftDate || !form.startTime || !form.endTime || !form.shiftName) {
      setFormError('All fields are required.');
      return;
    }
    setFormError('');
    setSubmitting(true);
    try {
      const payload = {
        userId: Number(form.userId),
        shiftDate: new Date(form.shiftDate).toISOString(),
        startTime: new Date(`${form.shiftDate}T${form.startTime}`).toISOString(),
        endTime: new Date(`${form.shiftDate}T${form.endTime}`).toISOString(),
        shiftName: form.shiftName,
        createdByManagerId: user?.id ?? null,
      };
      const res = await createSchedule(payload);
      setSchedules((prev) => [...prev, res.data]);
      setForm(EMPTY_FORM);
      setShowForm(false);
    } catch {
      setFormError('Failed to create schedule.');
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this schedule entry?')) return;
    try {
      await deleteSchedule(id);
      setSchedules((prev) => prev.filter((s) => s.scheduleId !== id));
    } catch {
      setError('Failed to delete schedule.');
    }
  };

  const userName = (userId) => {
    const u = users.find((u) => u.id === userId);
    return u ? `${u.firstName} ${u.lastName}` : `User #${userId}`;
  };

  const fmtTime = (iso) => {
    if (!iso) return '-';
    return new Date(iso).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 20 }}>
        <h1 className="page-title" style={{ marginBottom: 0 }}>Manage Schedules</h1>
        <button className="btn btn-primary" onClick={() => setShowForm((v) => !v)}>
          {showForm ? 'Cancel' : '+ New Schedule'}
        </button>
      </div>

      {showForm && (
        <div className="card" style={{ maxWidth: 520, marginBottom: 24 }}>
          <h2 style={{ fontSize: 15, fontWeight: 600, marginBottom: 16 }}>New Schedule Entry</h2>
          <form onSubmit={handleCreate}>
            <div className="form-group">
              <label>Employee</label>
              <select name="userId" className="form-control" value={form.userId} onChange={handleChange}>
                <option value="">Select employee...</option>
                {users.map((u) => (
                  <option key={u.id} value={u.id}>{u.firstName} {u.lastName}</option>
                ))}
              </select>
            </div>
            <div className="form-group">
              <label>Shift Date</label>
              <input type="date" name="shiftDate" className="form-control" value={form.shiftDate} onChange={handleChange} />
            </div>
            <div className="form-group">
              <label>Start Time</label>
              <input type="time" name="startTime" className="form-control" value={form.startTime} onChange={handleChange} />
            </div>
            <div className="form-group">
              <label>End Time</label>
              <input type="time" name="endTime" className="form-control" value={form.endTime} onChange={handleChange} />
            </div>
            <div className="form-group">
              <label>Shift Name</label>
              <input type="text" name="shiftName" className="form-control" placeholder="e.g. Morning Shift" value={form.shiftName} onChange={handleChange} />
            </div>
            {formError && <p className="error-text" style={{ marginBottom: 12 }}>{formError}</p>}
            <button type="submit" className="btn btn-primary" disabled={submitting}>
              {submitting ? 'Saving...' : 'Create'}
            </button>
          </form>
        </div>
      )}

      <div className="card">
        {loading ? (
          <p style={{ color: 'var(--color-text-muted)' }}>Loading...</p>
        ) : error ? (
          <p className="error-text">{error}</p>
        ) : schedules.length === 0 ? (
          <p style={{ color: 'var(--color-text-muted)' }}>No schedules yet.</p>
        ) : (
          <table style={tableStyle.table}>
            <thead>
              <tr>
                <th style={tableStyle.th}>Employee</th>
                <th style={tableStyle.th}>Shift Date</th>
                <th style={tableStyle.th}>Shift Name</th>
                <th style={tableStyle.th}>Start</th>
                <th style={tableStyle.th}>End</th>
                <th style={tableStyle.th}></th>
              </tr>
            </thead>
            <tbody>
              {schedules.map((s) => (
                <tr key={s.scheduleId}>
                  <td style={tableStyle.td}>{userName(s.userId)}</td>
                  <td style={tableStyle.td}>{formatDate(s.shiftDate)}</td>
                  <td style={tableStyle.td}>{s.shiftName || '-'}</td>
                  <td style={tableStyle.td}>{fmtTime(s.startTime)}</td>
                  <td style={tableStyle.td}>{fmtTime(s.endTime)}</td>
                  <td style={tableStyle.td}>
                    <button
                      className="btn btn-secondary"
                      style={{ padding: '3px 10px', fontSize: 12 }}
                      onClick={() => handleDelete(s.scheduleId)}
                    >
                      Delete
                    </button>
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
