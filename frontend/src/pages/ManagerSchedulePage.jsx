import { useState, useEffect, useMemo } from 'react';
import { Calendar, dateFnsLocalizer } from 'react-big-calendar';
import { format, parse, startOfWeek, getDay } from 'date-fns';
import { enUS } from 'date-fns/locale/en-US';
import 'react-big-calendar/lib/css/react-big-calendar.css';
import { getSchedules, createSchedule, deleteSchedule } from '../api/scheduleApi';
import { getAllUsers } from '../api/userApi';
import useAuthStore from '../store/authStore';

const localizer = dateFnsLocalizer({ format, parse, startOfWeek, getDay, locales: { 'en-US': enUS } });

const EMPTY_FORM = { userId: '', startTime: '09:00', endTime: '17:00', shiftName: '' };

// Cycle through colors for different employees
const PALETTE = ['#2563eb', '#16a34a', '#d97706', '#9333ea', '#dc2626', '#0891b2'];
const colorFor = (userId, users) => {
  const idx = users.findIndex((u) => u.id === userId);
  return PALETTE[idx % PALETTE.length] ?? PALETTE[0];
};

export default function ManagerSchedulePage() {
  const user = useAuthStore((s) => s.user);
  const [schedules, setSchedules] = useState([]);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // Panel state
  const [panel, setPanel] = useState(null); // { mode: 'create'|'detail', date?, schedule? }
  const [form, setForm] = useState(EMPTY_FORM);
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState('');

  useEffect(() => {
    Promise.all([getSchedules(), getAllUsers()])
      .then(([schedRes, userRes]) => {
        setSchedules(schedRes.data);
        setUsers(userRes.data.filter((u) => u.role !== 'Manager'));
      })
      .catch(() => setError('Failed to load data.'))
      .finally(() => setLoading(false));
  }, []);

  const events = useMemo(() =>
    schedules.map((s) => {
      const emp = users.find((u) => u.id === s.userId);
      const name = emp ? `${emp.firstName} ${emp.lastName}` : `User #${s.userId}`;
      return {
        id: s.scheduleId,
        title: `${name} — ${s.shiftName || 'Shift'}`,
        start: new Date(s.startTime),
        end: new Date(s.endTime),
        resource: s,
        userId: s.userId,
      };
    }),
    [schedules, users]
  );

  const eventStyleGetter = (event) => ({
    style: {
      backgroundColor: colorFor(event.userId, users),
      borderRadius: '4px',
      color: '#fff',
      border: 'none',
      fontSize: 12,
      padding: '2px 6px',
    },
  });

  const handleSelectSlot = ({ start }) => {
    const dateStr = format(start, 'yyyy-MM-dd');
    setForm({ ...EMPTY_FORM, userId: users[0]?.id ?? '' });
    setPanel({ mode: 'create', date: dateStr });
    setFormError('');
  };

  const handleSelectEvent = (event) => {
    setPanel({ mode: 'detail', schedule: event.resource });
  };

  const handleChange = (e) => setForm((f) => ({ ...f, [e.target.name]: e.target.value }));

  const handleCreate = async (e) => {
    e.preventDefault();
    if (!form.userId || !form.shiftName) {
      setFormError('Employee and shift name are required.');
      return;
    }
    setFormError('');
    setSubmitting(true);
    try {
      const payload = {
        userId: Number(form.userId),
        shiftDate: new Date(panel.date).toISOString(),
        startTime: new Date(`${panel.date}T${form.startTime}`).toISOString(),
        endTime: new Date(`${panel.date}T${form.endTime}`).toISOString(),
        shiftName: form.shiftName,
        createdByManagerId: user?.id ?? null,
      };
      const res = await createSchedule(payload);
      setSchedules((prev) => [...prev, res.data]);
      setPanel(null);
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
      setPanel(null);
    } catch {
      setError('Failed to delete schedule.');
    }
  };

  return (
    <div>
      <h1 className="page-title" style={{ marginBottom: 20 }}>Manage Schedules</h1>
      <p style={{ color: 'var(--color-text-muted)', fontSize: 13, marginBottom: 16 }}>
        Click any date on the calendar to assign a shift. Click an existing shift to view or delete it.
      </p>

      {error && <p className="error-text" style={{ marginBottom: 12 }}>{error}</p>}

      {/* Legend */}
      {users.length > 0 && (
        <div style={{ display: 'flex', gap: 12, flexWrap: 'wrap', marginBottom: 16 }}>
          {users.map((u, i) => (
            <div key={u.id} style={{ display: 'flex', alignItems: 'center', gap: 6, fontSize: 12 }}>
              <span style={{ width: 12, height: 12, borderRadius: 2, background: PALETTE[i % PALETTE.length], display: 'inline-block' }} />
              {u.firstName} {u.lastName}
            </div>
          ))}
        </div>
      )}

      {loading ? (
        <p style={{ color: 'var(--color-text-muted)' }}>Loading...</p>
      ) : (
        <div style={{ display: 'flex', gap: 20, alignItems: 'flex-start' }}>
          <div className="card" style={{ flex: 1, padding: 0, overflow: 'hidden' }}>
            <div style={{ padding: 16, height: 620 }}>
              <Calendar
                localizer={localizer}
                events={events}
                startAccessor="start"
                endAccessor="end"
                selectable
                onSelectSlot={handleSelectSlot}
                onSelectEvent={handleSelectEvent}
                eventPropGetter={eventStyleGetter}
                views={['month', 'week']}
                style={{ height: '100%' }}
              />
            </div>
          </div>

          {/* Side panel */}
          {panel && (
            <div className="card" style={{ width: 260, flexShrink: 0 }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 14 }}>
                <h3 style={{ fontSize: 14, fontWeight: 600 }}>
                  {panel.mode === 'create' ? `New Shift — ${panel.date}` : 'Shift Detail'}
                </h3>
                <button
                  onClick={() => setPanel(null)}
                  style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: 18, color: 'var(--color-text-muted)', lineHeight: 1 }}
                >×</button>
              </div>

              {panel.mode === 'create' ? (
                <form onSubmit={handleCreate}>
                  <div className="form-group">
                    <label style={{ fontSize: 12 }}>Employee</label>
                    <select name="userId" className="form-control" value={form.userId} onChange={handleChange}>
                      {users.map((u) => (
                        <option key={u.id} value={u.id}>{u.firstName} {u.lastName}</option>
                      ))}
                    </select>
                  </div>
                  <div className="form-group">
                    <label style={{ fontSize: 12 }}>Shift Name</label>
                    <input type="text" name="shiftName" className="form-control" placeholder="e.g. Morning Shift" value={form.shiftName} onChange={handleChange} />
                  </div>
                  <div className="form-group">
                    <label style={{ fontSize: 12 }}>Start Time</label>
                    <input type="time" name="startTime" className="form-control" value={form.startTime} onChange={handleChange} />
                  </div>
                  <div className="form-group">
                    <label style={{ fontSize: 12 }}>End Time</label>
                    <input type="time" name="endTime" className="form-control" value={form.endTime} onChange={handleChange} />
                  </div>
                  {formError && <p className="error-text" style={{ marginBottom: 10, fontSize: 12 }}>{formError}</p>}
                  <button type="submit" className="btn btn-primary" style={{ width: '100%' }} disabled={submitting}>
                    {submitting ? 'Saving...' : 'Assign Shift'}
                  </button>
                </form>
              ) : (
                <>
                  {(() => {
                    const s = panel.schedule;
                    const emp = users.find((u) => u.id === s.userId);
                    return (
                      <>
                        <p style={detailStyle.label}>Employee</p>
                        <p style={detailStyle.value}>{emp ? `${emp.firstName} ${emp.lastName}` : `User #${s.userId}`}</p>
                        <p style={detailStyle.label}>Shift Name</p>
                        <p style={detailStyle.value}>{s.shiftName || '-'}</p>
                        <p style={detailStyle.label}>Date</p>
                        <p style={detailStyle.value}>{new Date(s.shiftDate).toLocaleDateString()}</p>
                        <p style={detailStyle.label}>Start</p>
                        <p style={detailStyle.value}>{new Date(s.startTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</p>
                        <p style={detailStyle.label}>End</p>
                        <p style={detailStyle.value}>{new Date(s.endTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</p>
                        <button
                          className="btn btn-secondary"
                          style={{ width: '100%', marginTop: 16, color: 'var(--color-danger)', borderColor: 'var(--color-danger)' }}
                          onClick={() => handleDelete(s.scheduleId)}
                        >
                          Delete Shift
                        </button>
                      </>
                    );
                  })()}
                </>
              )}
            </div>
          )}
        </div>
      )}
    </div>
  );
}

const detailStyle = {
  label: { fontSize: 11, color: 'var(--color-text-muted)', textTransform: 'uppercase', letterSpacing: '0.04em', marginTop: 10 },
  value: { fontSize: 13, color: 'var(--color-text)', marginTop: 2 },
};
