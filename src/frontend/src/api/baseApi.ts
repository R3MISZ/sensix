import axios from 'axios';

// http://localhost:5000
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const api = axios.create({
  baseURL: `${API_BASE_URL}/api`
});

console.log("CURRENT API-URL:", API_BASE_URL);