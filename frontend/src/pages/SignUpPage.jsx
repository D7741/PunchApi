import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { GoogleLogin } from '@react-oauth/google';
import { register, googleAuth } from '../api/authApi';
import useAuthStore from '../store/authStore';

export default function SignUpPage() {
  const navigate = useNavigate();
  const loginStore = useAuthStore((s) => s.login);

  const [form, setForm] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: 'Employee',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e) => {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (form.password !== form.confirmPassword) {
      setError('Passwords do not match.');
      return;
    }

    setLoading(true);
    try {
      const res = await register({
        firstName: form.firstName,
        lastName: form.lastName,
        email: form.email,
        password: form.password,
        role: form.role,
      });
      const { accessToken, userId, fullName, email: userEmail, role } = res.data;
      loginStore(accessToken, { id: userId, fullName, email: userEmail, role });
      navigate('/dashboard');
    } catch (err) {
      setError(err.response?.data?.message || 'Sign up failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleGoogleSuccess = async (credentialResponse) => {
    setError('');
    setLoading(true);
    try {
      const res = await googleAuth(credentialResponse.credential);
      const { accessToken, userId, fullName, email: userEmail, role } = res.data;
      loginStore(accessToken, { id: userId, fullName, email: userEmail, role });
      navigate('/dashboard');
    } catch (err) {
      setError(err.response?.data?.message || 'Google sign-up failed.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.page}>
      <div className="card" style={styles.card}>
        <h1 style={styles.title}>UserPunch</h1>
        <p style={styles.subtitle}>Create your account</p>

        <div style={styles.googleWrapper}>
          <GoogleLogin
            onSuccess={handleGoogleSuccess}
            onError={() => setError('Google sign-up failed.')}
            width="380"
            text="signup_with"
          />
        </div>

        <div style={styles.divider}><span>or</span></div>

        <form onSubmit={handleSubmit}>
          <div style={styles.row}>
            <div className="form-group" style={{ flex: 1 }}>
              <label htmlFor="firstName">First Name</label>
              <input
                id="firstName"
                name="firstName"
                type="text"
                className="form-control"
                value={form.firstName}
                onChange={handleChange}
                placeholder="Jane"
                required
              />
            </div>
            <div className="form-group" style={{ flex: 1 }}>
              <label htmlFor="lastName">Last Name</label>
              <input
                id="lastName"
                name="lastName"
                type="text"
                className="form-control"
                value={form.lastName}
                onChange={handleChange}
                placeholder="Doe"
                required
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              name="email"
              type="email"
              className="form-control"
              value={form.email}
              onChange={handleChange}
              placeholder="you@example.com"
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              name="password"
              type="password"
              className="form-control"
              value={form.password}
              onChange={handleChange}
              placeholder="Min 6 characters"
              required
              minLength={6}
            />
          </div>

          <div className="form-group">
            <label htmlFor="confirmPassword">Confirm Password</label>
            <input
              id="confirmPassword"
              name="confirmPassword"
              type="password"
              className="form-control"
              value={form.confirmPassword}
              onChange={handleChange}
              placeholder="••••••••"
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="role">Role</label>
            <select
              id="role"
              name="role"
              className="form-control"
              value={form.role}
              onChange={handleChange}
            >
              <option value="Employee">Employee</option>
              <option value="Manager">Manager</option>
            </select>
          </div>

          {error && <p className="error-text" style={{ marginBottom: 12 }}>{error}</p>}

          <button
            type="submit"
            className="btn btn-primary"
            style={{ width: '100%', padding: '10px' }}
            disabled={loading}
          >
            {loading ? 'Creating account...' : 'Create Account'}
          </button>
        </form>

        <p style={styles.footer}>
          Already have an account?{' '}
          <Link to="/login" style={styles.link}>Sign in</Link>
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
    padding: '24px 0',
  },
  card: {
    width: '100%',
    maxWidth: 420,
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
  row: {
    display: 'flex',
    gap: 12,
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
