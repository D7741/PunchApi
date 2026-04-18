import axiosClient from './axiosClient';

export const login = (payload) => axiosClient.post('/auth/login', payload);
export const getMe = () => axiosClient.get('/auth/me');
export const logout = () => axiosClient.post('/auth/logout');
