import axiosClient from './axiosClient';

export const punchIn = (userId) => axiosClient.post('/punchrecords/punchin', { userId });
export const punchOut = (userId) => axiosClient.post('/punchrecords/punchout', { userId });
export const getPunchRecordsByUser = (userId) => axiosClient.get(`/punchrecords/user/${userId}`);
export const getAllPunchRecords = () => axiosClient.get('/punchrecords');
