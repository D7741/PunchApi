import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createLeaveRequest } from '../api/leaveApi';

// Values match the backend Reason enum index order:
// 0=Annual_Leave, 1=Alt_Holiday_Leave, 2=Personal_Sick, 3=Person_Carers, 4=Long_Service_Leave
const LEAVE_REASONS = [
  { value: 0, label: 'Annual Leave' },
  { value: 1, label: 'Alternative Holiday Leave' },
  { value: 2, label: 'Personal / Sick Leave' },
  { value: 3, label: "Personal Carer's Leave" },
  { value: 4, label: 'Long Service Leave' },
];

export default function NewLeaveRequestPage() {
  const navigate = useNavigate();

  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [leaveReason, setLeaveReason] = useState(LEAVE_REASONS[0].value);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const validate = () => {
    if (!startDate || !endDate) return 'Start date and end date are required.';
    if (new Date(endDate) < new Date(startDate)) return 'End date cannot be before start date.';
    return null;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationError = validate();
    if (validationError) {
      setError(validationError);
      return;
    }

    setError('');
    setLoading(true);

    try {
      await createLeaveRequest({ startDate, endDate, leaveReason });
      navigate('/leave-requests');
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to submit leave request.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1 className="page-title">New Leave Request</h1>

      <div className="card" style={{ maxWidth: 480 }}>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="startDate">Start Date</label>
            <input
              id="startDate"
              type="date"
              className="form-control"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="endDate">End Date</label>
            <input
              id="endDate"
              type="date"
              className="form-control"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="leaveReason">Leave Reason</label>
            <select
              id="leaveReason"
              className="form-control"
              value={leaveReason}
              onChange={(e) => setLeaveReason(Number(e.target.value))}
            >
              {LEAVE_REASONS.map((r) => (
                <option key={r.value} value={r.value}>{r.label}</option>
              ))}
            </select>
          </div>

          {error && <p className="error-text" style={{ marginBottom: 12 }}>{error}</p>}

          <div style={{ display: 'flex', gap: 10 }}>
            <button type="submit" className="btn btn-primary" disabled={loading}>
              {loading ? 'Submitting...' : 'Submit'}
            </button>
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => navigate('/leave-requests')}
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
