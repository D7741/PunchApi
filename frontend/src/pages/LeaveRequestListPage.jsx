import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getLeaveRequests, getMyLeaveRequests, approveLeaveRequest, rejectLeaveRequest } from '../api/leaveApi';
import { formatDate } from '../utils/formatDate';
import useAuthStore from '../store/authStore';

export default function LeaveRequestListPage() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const user = useAuthStore((s) => s.user);
  const isManager = user?.role === 'Manager';

  const load = () => {
    const fetch = isManager ? getLeaveRequests : getMyLeaveRequests;
    fetch()
      .then((res) => setRequests(res.data))
      .catch(() => setError('Failed to load leave requests.'))
      .finally(() => setLoading(false));
  };

  useEffect(() => { load(); }, [isManager]);

  const handleApprove = (id) => {
    approveLeaveRequest(id)
      .then(() => setRequests((prev) => prev.map((r) => r.leaveRequestId === id ? { ...r, status: 'Approved' } : r)))
      .catch(() => setError('Failed to approve request.'));
  };

  const handleReject = (id) => {
    rejectLeaveRequest(id)
      .then(() => setRequests((prev) => prev.map((r) => r.leaveRequestId === id ? { ...r, status: 'Rejected' } : r)))
      .catch(() => setError('Failed to reject request.'));
  };

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 20 }}>
        <h1 className="page-title" style={{ marginBottom: 0 }}>Leave Requests</h1>
        {!isManager && (
          <Link to="/leave-requests/new">
            <button className="btn btn-primary">+ New Request</button>
          </Link>
        )}
      </div>

      <div className="card">
        {loading ? (
          <p style={{ color: 'var(--color-text-muted)' }}>Loading...</p>
        ) : error ? (
          <p className="error-text">{error}</p>
        ) : requests.length === 0 ? (
          <p style={{ color: 'var(--color-text-muted)' }}>No leave requests found.</p>
        ) : (
          <table style={tableStyle.table}>
            <thead>
              <tr>
                {isManager && <th style={tableStyle.th}>Employee ID</th>}
                <th style={tableStyle.th}>Start Date</th>
                <th style={tableStyle.th}>End Date</th>
                <th style={tableStyle.th}>Reason</th>
                <th style={tableStyle.th}>Status</th>
                {isManager && <th style={tableStyle.th}>Actions</th>}
              </tr>
            </thead>
            <tbody>
              {requests.map((r) => (
                <tr key={r.leaveRequestId}>
                  {isManager && <td style={tableStyle.td}>{r.userId}</td>}
                  <td style={tableStyle.td}>{formatDate(r.startDate)}</td>
                  <td style={tableStyle.td}>{formatDate(r.endDate)}</td>
                  <td style={tableStyle.td}>{r.leaveReason}</td>
                  <td style={tableStyle.td}>
                    <span className={`badge badge-${r.status?.toLowerCase()}`}>{r.status}</span>
                  </td>
                  {isManager && (
                    <td style={tableStyle.td}>
                      {r.status === 'Pending' ? (
                        <div style={{ display: 'flex', gap: 6 }}>
                          <button className="btn btn-primary" style={{ padding: '3px 10px', fontSize: 12 }} onClick={() => handleApprove(r.leaveRequestId)}>Approve</button>
                          <button className="btn btn-secondary" style={{ padding: '3px 10px', fontSize: 12 }} onClick={() => handleReject(r.leaveRequestId)}>Reject</button>
                        </div>
                      ) : (
                        <span style={{ color: 'var(--color-text-muted)', fontSize: 12 }}>—</span>
                      )}
                    </td>
                  )}
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
