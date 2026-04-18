import { Link } from 'react-router-dom';

export default function NotAuthorizedPage() {
  return (
    <div style={{ textAlign: 'center', padding: '80px 20px' }}>
      <h1 style={{ fontSize: 24, fontWeight: 700, marginBottom: 12 }}>Access Denied</h1>
      <p style={{ color: 'var(--color-text-muted)', marginBottom: 24 }}>
        You do not have permission to view this page.
      </p>
      <Link to="/dashboard">
        <button className="btn btn-primary">Back to Dashboard</button>
      </Link>
    </div>
  );
}
