import api from './axios';

export const getSematskiPrikaz = (salaId, projekcijaId) =>
  api.get(`/sale/${salaId}/projekcije/${projekcijaId}/sedista`);