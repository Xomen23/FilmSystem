import api from './axios';

export const getAllRezervacije = () => api.get('/rezervacije');
export const createRezervacija = (projekcijaId, sedisteId) =>
  api.post('/rezervacije', { projekcijaId, sedisteId });
export const potvrdiRezervaciju = (id) => api.put(`/rezervacije/${id}/potvrdi`);
export const platiRezervaciju = (id) => api.put(`/rezervacije/${id}/plati`);
export const otkaziRezervaciju = (id) => api.put(`/rezervacije/${id}/otkazi`);