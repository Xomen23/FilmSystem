import api from './axios';

export const getAllFilmovi = () => api.get('/filmovi');
export const getFilmById = (id) => api.get(`/filmovi/${id}`);
export const getFilmoviByZanr = (zanrId) => api.get(`/filmovi/zanr/${zanrId}`);
export const searchExternal = (naziv) => api.get(`/filmovi/search-external?naziv=${naziv}`);
export const importFilm = (imdbId, zanrId) => api.post(`/filmovi/import/${imdbId}`, { zanrId });
export const deleteFilm = (id) => api.delete(`/filmovi/${id}`);