import axiosClient from './axiosClient';

export const getSchedules = () => axiosClient.get('/schedules');
export const getSchedulesByUser = (userId) => axiosClient.get(`/schedules/user/${userId}`);
export const createSchedule = (payload) => axiosClient.post('/schedules', payload);
export const updateSchedule = (id, payload) => axiosClient.put(`/schedules/${id}`, payload);
export const deleteSchedule = (id) => axiosClient.delete(`/schedules/${id}`);
