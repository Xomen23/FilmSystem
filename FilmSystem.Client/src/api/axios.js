import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7194/api',
});

export default api;