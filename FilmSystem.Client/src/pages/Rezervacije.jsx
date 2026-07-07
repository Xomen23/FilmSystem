import { useEffect, useState } from "react";
import { getAllRezervacije, potvrdiRezervaciju, platiRezervaciju, otkaziRezervaciju } from "../api/rezervacije";

const boje = {
  Kreirana: "#FFA726",
  Potvrdjana: "#42A5F5",
  Placena: "#66BB6A",
  Otkazana: "#EF5350",
  Istekla: "#9E9E9E"
};

const Btn = ({ children, onClick, color }) => (
  <button onClick={onClick} style={{
    marginRight: "6px", 
    padding: "6px 12px", 
    background: color,
    color: "white", 
    border: "none", 
    borderRadius: "4px", 
    cursor: "pointer",
    fontSize: "13px",
    fontWeight: "bold",
    transition: "opacity 0.2s"
  }}
  onMouseEnter={(e) => e.target.style.opacity = "0.8"}
  onMouseLeave={(e) => e.target.style.opacity = "1"}
  >
    {children}
  </button>
);

export default function Rezervacije() {
  const [rezervacije, setRezervacije] = useState([]);
  const [loading, setLoading] = useState(true);

  const ucitaj = () => {
    setLoading(true);
    getAllRezervacije()
      .then(res => {
        setRezervacije(res.data);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  };
  
  useEffect(() => { ucitaj(); }, []);

  const akcija = async (fn, id, nazivAkcije) => {
    try { 
      await fn(id); 
      ucitaj(); 
    } catch (e) { 
      alert(e.response?.data || `Greška pri izvršavanju akcije: ${nazivAkcije}`); 
    }
  };

  return (
    <div style={{ 
      width: "100%",
      maxWidth: "100%",
      minHeight: "100vh", 
      boxSizing: "border-box", 
      padding: "20px 30px", 
      fontFamily: "sans-serif", 
      background: "#f9f9f9",
      overflowX: "hidden"
    }}>
      
      <div style={{ background: "white", padding: "20px 25px", borderRadius: "12px", boxShadow: "0 2px 10px rgba(0,0,0,0.05)", marginBottom: "25px" }}>
        <h1 style={{ marginTop: 0, marginBottom: "5px", fontSize: "24px", color: "#222" }}>
          Pregled Rezervacija
        </h1>
        <p style={{ color: "#666", margin: 0, fontSize: "14px" }}>Upravljanje statusima rezervacija karata</p>
      </div>

      {loading ? (
        <div style={{ textTransform: "none", textAlign: "center", padding: "50px", fontSize: "16px", color: "#666" }}>Učitavanje rezervacija...</div>
      ) : rezervacije.length === 0 ? (
        <div style={{ textAlign: "center", padding: "50px", background: "white", borderRadius: "8px", border: "1px solid #eee" }}>
          <h3>Nema kreiranih rezervacija.</h3>
        </div>
      ) : (
        <div style={{ background: "white", borderRadius: "12px", boxShadow: "0 4px 15px rgba(0,0,0,0.05)", overflow: "hidden", width: "100%" }}>
          <table style={{ borderCollapse: "collapse", width: "100%", fontSize: "14px" }}>
            <thead>
              <tr style={{ background: "#4472C4", color: "white" }}>
                <th style={th}>Film</th>
                <th style={th}>Sedište</th>
                <th style={th}>Cena</th>
                <th style={th}>Vreme kreiranja</th>
                <th style={th}>Status</th>
                <th style={th}>Dostupne Akcije</th>
              </tr>
            </thead>
            <tbody>
              {rezervacije.map(r => (
                <tr key={r.id} style={{ borderBottom: "1px solid #eee" }} onMouseEnter={(e) => e.currentTarget.style.background = "#fafafa"} onMouseLeave={(e) => e.currentTarget.style.background = "white"}>
                  
                  {/* KOLONA FILM */}
                  <td style={{ ...td, fontWeight: "bold", color: "#333" }}>
                    {r.projekcijaFilmNaziv || r.filmNaziv || (r.projekcija && r.projekcija.film && r.projekcija.film.naziv) || `Projekcija ID: ${r.projekcijaId}`}
                  </td>
                  
                  {/* SEDIŠTE  */}
                  <td style={td}>
                    {r.sedisteBrojReda && r.sedisteBrojMesta 
                      ? `Red ${r.sedisteBrojReda}, Mesto ${r.sedisteBrojMesta}` 
                      : (r.sedisteId || r.sediste_id || "/")}
                  </td>
                  
                  {/* CENA */}
                  <td style={{ ...td, color: "#2e7d32", fontWeight: "bold" }}>
                    {r.cenaKarte || r.cena || (r.projekcija && r.projekcija.cenaKarte) ? `${r.cenaKarte || r.cena || r.projekcija.cenaKarte} RSD` : "500 RSD (Default)"}
                  </td>
                  
                  {/* VREME KREIRANJA */}
                  <td style={td}>
                    {r.vremeKreiranja || r.vreme 
                      ? new Date(r.vremeKreiranja || r.vreme).toLocaleString("sr-RS") 
                      : "/"}
                  </td>
                  
                  {/* STATUS */}
                  <td style={td}>
                    <span style={{
                      background: boje[r.status] || "#ccc",
                      color: "white", 
                      padding: "4px 10px", 
                      borderRadius: "12px", 
                      fontSize: "12px",
                      fontWeight: "bold",
                      display: "inline-block"
                    }}>
                      {r.status}
                    </span>
                  </td>
                  
                  {/* AKCIJE */}
                  <td style={td}>
                    <div style={{ display: "flex", gap: "2px" }}>
                      {r.status === "Kreirana" && (
                        <>
                          <Btn color="#42A5F5" onClick={() => akcija(potvrdiRezervaciju, r.id, "potvrđena")}>Potvrdi</Btn>
                          <Btn color="#EF5350" onClick={() => akcija(otkaziRezervaciju, r.id, "otkazana")}>Otkaži</Btn>
                        </>
                      )}
                      
                      {r.status === "Potvrdjana" && (
                        <>
                          <Btn color="#66BB6A" onClick={() => akcija(platiRezervaciju, r.id, "plaćena")}>Plati</Btn>
                          <Btn color="#EF5350" onClick={() => akcija(otkaziRezervaciju, r.id, "otkazana")}>Otkaži</Btn>
                        </>
                      )}
                      
                      {(r.status === "Placena" || r.status === "Otkazana" || r.status === "Istekla") && (
                        <span style={{ color: "#999", fontSize: "13px", fontStyle: "italic" }}>Nema akcija</span>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

const th = { 
  padding: "12px 18px", 
  textAlign: "left", 
  fontWeight: "bold",
  textTransform: "uppercase",
  fontSize: "12px",
  letterSpacing: "0.5px"
};

const td = { 
  padding: "12px 18px", 
  color: "#555",
  whiteSpace: "nowrap"
};