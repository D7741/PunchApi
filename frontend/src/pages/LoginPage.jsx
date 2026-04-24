import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { GoogleLogin } from '@react-oauth/google';
import { login, googleAuth } from '../api/authApi';
import useAuthStore from '../store/authStore';

export default function LoginPage() {
  const navigate = useNavigate();
  const loginStore = useAuthStore((s) => s.login);

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleGoogleSuccess = async (credentialResponse) => {
    setError('');
    setLoading(true);
    try {
      const res = await googleAuth(credentialResponse.credential);
      const { accessToken, userId, fullName, email: userEmail, role } = res.data;
      loginStore(accessToken, { id: userId, fullName, email: userEmail, role });
      navigate('/dashboard');
    } catch (err) {
      setError(err.response?.data?.message || 'Google sign-in failed.');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const res = await login({ email, password });
      const { accessToken, userId, fullName, email: userEmail, role } = res.data;
      loginStore(accessToken, { id: userId, fullName, email: userEmail, role });
      navigate('/dashboard');
    } catch (err) {
      const msg = err.response?.data?.message || 'Login failed. Please check your credentials.';
      setError(msg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.page}>
      <div className="card" style={styles.card}>
        <h1 style={styles.title}>UserPunch</h1>
        <p style={styles.subtitle}>Employee Attendance System</p>

        <div style={styles.googleWrapper}>
          <GoogleLogin
            onSuccess={handleGoogleSuccess}
            onError={() => setError('Google sign-in failed.')}
            width="340"
            text="continue_with"
          />
        </div>

        <div style={styles.divider}><span>or</span></div>

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              className="form-control"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="you@example.com"
              required
              autoFocus
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              className="form-control"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••••"
              required
            />
          </div>

          {error && <p className="error-text" style={{ marginBottom: 12 }}>{error}</p>}

          <button
            type="submit"
            className="btn btn-primary"
            style={{ width: '100%', padding: '10px' }}
            disabled={loading}
          >
            {loading ? 'Signing in...' : 'Sign In'}
          </button>
        </form>

        <p style={styles.footer}>
          Don&apos;t have an account?{' '}
          <Link to="/signup" style={styles.link}>Sign up</Link>
        </p>
      </div>
    </div>
  );
}

const styles = {
  page: {
    minHeight: '100vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    background: 'var(--color-bg)',
  },
  card: {
    width: '100%',
    maxWidth: 380,
  },
  title: {
    fontSize: 24,
    fontWeight: 700,
    marginBottom: 4,
    textAlign: 'center',
  },
  subtitle: {
    color: 'var(--color-text-muted)',
    textAlign: 'center',
    marginBottom: 20,
    fontSize: 13,
  },
  googleWrapper: {
    display: 'flex',
    justifyContent: 'center',
    marginBottom: 16,
  },
  divider: {
    display: 'flex',
    alignItems: 'center',
    gap: 12,
    marginBottom: 16,
    color: 'var(--color-text-muted)',
    fontSize: 12,
    textAlign: 'center',
  },
  footer: {
    textAlign: 'center',
    marginTop: 16,
    fontSize: 13,
    color: 'var(--color-text-muted)',
  },
  link: {
    color: 'var(--color-primary, #4f46e5)',
    textDecoration: 'none',
    fontWeight: 500,
  },
};
