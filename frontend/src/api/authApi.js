import axiosClient from './axiosClient';

export const login = (payload) => axiosClient.post('/auth/login', payload);
export const register = (payload) => axiosClient.post('/auth/register', payload);
export const googleAuth = (credential) => axiosClient.post('/auth/google', { credential });
export const getMe = () => axiosClient.get('/auth/me');
export const logout = () => axiosClient.post('/auth/logout');
