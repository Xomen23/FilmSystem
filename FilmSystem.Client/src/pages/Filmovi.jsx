import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getAllFilmovi } from "../api/filmovi";

export default function Filmovi() {
  const [filmovi, setFilmovi] = useState([]);
  const [filter, setFilter] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    getAllFilmovi().then(res => setFilmovi(res.data));
  }, []);

  const filtrirani = filmovi.filter(f =>
    f.naziv.toLowerCase().includes(filter.toLowerCase())
  );

  return (
    <div style={{ padding: "20px" }}>
      <h1>Filmovi</h1>
      <input
        placeholder="Pretraži po nazivu..."
        value={filter}
        onChange={e => setFilter(e.target.value)}
        style={{ marginBottom: "20px", padding: "8px", width: "300px" }}
      />
      <div style={{ display: "flex", flexWrap: "wrap", gap: "20px" }}>
        {filtrirani.map(film => (
          <div
            key={film.id}
            onClick={() => navigate(`/filmovi/${film.id}/projekcije`)}
            style={{
              width: "180px", cursor: "pointer", border: "1px solid #ccc",
              borderRadius: "8px", overflow: "hidden",
              boxShadow: "2px 2px 6px rgba(0,0,0,0.15)"
            }}
          >
            {film.poster
              ? <img src={film.poster} alt={film.naziv} style={{ width: "100%" }} />
              : <div style={{ height: "260px", background: "#eee", display: "flex", alignItems: "center", justifyContent: "center" }}>
                  Nema postera
                </div>
            }
            <div style={{ padding: "10px" }}>
              <strong>{film.naziv}</strong>
              <p style={{ margin: "4px 0", color: "#666" }}>{film.godina}</p>
              <p style={{ margin: "4px 0", color: "#666", fontSize: "12px" }}>{film.zanrNaziv}</p>
              <p style={{ margin: "4px 0", color: "#666", fontSize: "12px" }}>{film.trajanjeMin} min</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}