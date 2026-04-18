import axiosClient from './axiosClient';

export const getLeaveRequests = () => axiosClient.get('/leaverequests');
export const getMyLeaveRequests = () => axiosClient.get('/leaverequests/my');
export const createLeaveRequest = (payload) => axiosClient.post('/leaverequests', payload);
export const approveLeaveRequest = (id) => axiosClient.put(`/leaverequests/${id}/approve`);
export const rejectLeaveRequest = (id) => axiosClient.put(`/leaverequests/${id}/reject`);
