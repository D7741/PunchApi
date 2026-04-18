import { useState, useEffect, useMemo } from 'react';
import { Calendar, dateFnsLocalizer } from 'react-big-calendar';
import { format, parse, startOfWeek, getDay } from 'date-fns';
import { enUS } from 'date-fns/locale/en-US';
import 'react-big-calendar/lib/css/react-big-calendar.css';
import useAuthStore from '../store/authStore';
import { getSchedulesByUser } from '../api/scheduleApi';

const localizer = dateFnsLocalizer({ format, parse, startOfWeek, getDay, locales: { 'en-US': enUS } });

export default function SchedulePage() {
  const user = useAuthStore((s) => s.user);
  const [schedules, setSchedules] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [selected, setSelected] = useState(null);

  useEffect(() => {
    if (!user) return;
    getSchedulesByUser(user.id)
      .then((res) => setSchedules(res.data))
      .catch(() => setError('Failed to load schedules.'))
      .finally(() => setLoading(false));
  }, [user]);

  const events = useMemo(() =>
    schedules.map((s) => ({
      id: s.scheduleId,
      title: s.shiftName || 'Shift',
      start: new Date(s.startTime),
      end: new Date(s.endTime),
      resource: s,
    })),
    [schedules]
  );

  const eventStyleGetter = () => ({
    style: {
      backgroundColor: '#2563eb',
      borderRadius: '4px',
      color: '#fff',
      border: 'none',
      fontSize: 12,
      padding: '2px 6px',
    },
  });

  return (
    <div>
      <h1 className="page-title" style={{ marginBottom: 20 }}>My Schedule</h1>

      {error && <p className="error-text" style={{ marginBottom: 12 }}>{error}</p>}
      {loading ? (
        <p style={{ color: 'var(--color-text-muted)' }}>Loading...</p>
      ) : (
        <div style={{ display: 'flex', gap: 20 }}>
          <div className="card" style={{ flex: 1, padding: 0, overflow: 'hidden' }}>
            <div style={{ padding: 16, height: 580 }}>
              <Calendar
                localizer={localizer}
                events={events}
                startAccessor="start"
                endAccessor="end"
                eventPropGetter={eventStyleGetter}
                onSelectEvent={(e) => setSelected(e.resource)}
                views={['month', 'week']}
                style={{ height: '100%' }}
              />
            </div>
          </div>

          {selected && (
            <div className="card" style={{ width: 240, alignSelf: 'flex-start' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 }}>
                <h3 style={{ fontSize: 14, fontWeight: 600 }}>Shift Detail</h3>
                <button
                  onClick={() => setSelected(null)}
                  style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: 16, color: 'var(--color-text-muted)' }}
                >×</button>
              </div>
              <p style={detailStyle.label}>Shift Name</p>
              <p style={detailStyle.value}>{selected.shiftName || '-'}</p>
              <p style={detailStyle.label}>Date</p>
              <p style={detailStyle.value}>{new Date(selected.shiftDate).toLocaleDateString()}</p>
              <p style={detailStyle.label}>Start</p>
              <p style={detailStyle.value}>{new Date(selected.startTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</p>
              <p style={detailStyle.label}>End</p>
              <p style={detailStyle.value}>{new Date(selected.endTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</p>
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
