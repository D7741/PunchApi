import { create } from 'zustand';

const useAuthStore = create((set) => ({
  token: localStorage.getItem('accessToken') || null,
  user: JSON.parse(localStorage.getItem('user') || 'null'),

  login: (token, user) => {
    localStorage.setItem('accessToken', token);
    localStorage.setItem('user', JSON.stringify(user));
    set({ token, user });
  },

  setUser: (user) => {
    localStorage.setItem('user', JSON.stringify(user));
    set({ user });
  },

  logout: () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('user');
    set({ token: null, user: null });
  },
}));

export default useAuthStore;
