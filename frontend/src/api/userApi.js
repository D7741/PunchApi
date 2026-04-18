import axiosClient from './axiosClient';

export const getAllUsers = () => axiosClient.get('/users');
export const getUserById = (id) => axiosClient.get(`/users/${id}`);
