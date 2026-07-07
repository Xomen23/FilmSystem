import axios from 'axios';

// baza URL-a ka tvom .NET API-ju
const api = axios.create({
  baseURL: 'https://localhost:7194/api',
});

export default api;