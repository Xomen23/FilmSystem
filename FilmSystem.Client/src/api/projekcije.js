import api from './axios';

export const getProjekcijeByFilm = (filmId) => api.get(`/projekcije/film/${filmId}`);
export const getProjekcijaById = (id) => api.get(`/projekcije/${id}`);