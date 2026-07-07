import api from './axios';

export const getAllRezervacije = () => api.get('/rezervacije');

export const createRezervacija = (projekcijaId, sedisteId) => {
  return api.post('/rezervacije', { 
    ProjekcijаId: projekcijaId, 
    SedisteId: sedisteId 
  });
};

export const potvrdiRezervaciju = (id) => api.put(`/rezervacije/${id}/potvrdi`);
export const platiRezervaciju = (id) => api.put(`/rezervacije/${id}/plati`);
export const otkaziRezervaciju = (id) => api.put(`/rezervacije/${id}/otkazi`);