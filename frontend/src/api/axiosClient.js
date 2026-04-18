import axios from 'axios';

const axiosClient = axios.create({
  baseURL: 'http://localhost:5007/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Attach Bearer token from localStorage on every request
axiosClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// On 401, clear storage and redirect to login
axiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default axiosClient;
