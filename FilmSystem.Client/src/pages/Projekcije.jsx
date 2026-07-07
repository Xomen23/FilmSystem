import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getFilmById } from "../api/filmovi";
import { getProjekcijeByFilm } from "../api/projekcije";

export default function Projekcije() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [film, setFilm] = useState(null);
  const [projekcije, setProjekcije] = useState([]);

  useEffect(() => {
    getFilmById(id).then(res => setFilm(res.data));
    getProjekcijeByFilm(id).then(res => setProjekcije(res.data));
  }, [id]);

  return (
    <div style={{ padding: "20px" }}>
      <button onClick={() => navigate("/filmovi")}>← Nazad</button>
      {film && (
        <div style={{ display: "flex", gap: "20px", margin: "20px 0" }}>
          {film.poster && (
            <img src={film.poster} alt={film.naziv} style={{ width: "120px", borderRadius: "8px" }} />
          )}
          <div>
            <h1>{film.naziv} ({film.godina})</h1>
            <p>{film.opis}</p>
            <p><strong>Žanr:</strong> {film.zanrNaziv}</p>
            <p><strong>Trajanje:</strong> {film.trajanjeMin} min</p>
          </div>
        </div>
      )}
      <h2>Projekcije</h2>
      {projekcije.length === 0
        ? <p>Nema zakazanih projekcija.</p>
        : <table style={{ borderCollapse: "collapse", width: "100%" }}>
            <thead>
              <tr style={{ background: "#4472C4", color: "white" }}>
                <th style={th}>Datum i vreme</th>
                <th style={th}>Cena karte</th>
                <th style={th}></th>
              </tr>
            </thead>
            <tbody>
              {projekcije.map(p => (
                <tr key={p.id}>
                  <td style={td}>{new Date(p.datumVreme).toLocaleString("sr-RS")}</td>
                  <td style={td}>{p.cenaKarte} RSD</td>
                  <td style={td}>
                    <button
                      onClick={() => navigate(`/projekcije/${p.id}/sala?salaId=${p.salaId}`)}
                      style={{ padding: "6px 14px", background: "#4472C4", color: "white", border: "none", borderRadius: "4px", cursor: "pointer" }}
                    >
                      Odaberi sedište
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
      }
    </div>
  );
}

const th = { padding: "10px", textAlign: "left", border: "1px solid #ddd" };
const td = { padding: "10px", border: "1px solid #ddd" };