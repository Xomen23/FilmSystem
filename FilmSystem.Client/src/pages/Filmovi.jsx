import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getAllFilmovi, searchExternal, importFilm } from "../api/filmovi";
import axios from "axios";

const API_URL = "https://localhost:7194/api";

export default function Filmovi() {
  const [filmovi, setFilmovi] = useState([]);
  const [eksterniUpit, setEksterniUpit] = useState("");
  const [eksterniRezultati, setEksterniRezultati] = useState([]);
  const [prikažiMeni, setPrikažiMeni] = useState(false);
  const navigate = useNavigate();

  const [bazaZanrovi, setBazaZanrovi] = useState([]);
  const [bazaSale, setBazaSale] = useState([]);

  const [prikažiModal, setPrikažiModal] = useState(false);
  const [izabraniFilm, setIzabraniFilm] = useState(null);
  
  const [formaPodaci, setFormaPodaci] = useState({
  zanrId: "",
  salaId: "",
  cena: "500",
  vreme: new Date(Date.now() + 86400000).toISOString().slice(0, 16)
});

  const osvežiLokalneFilmove = () => {
    getAllFilmovi().then(res => setFilmovi(res.data));
  };

  useEffect(() => {
    osvežiLokalneFilmove();

    axios.get(`${API_URL}/zanrovi`)
      .then(res => {
        setBazaZanrovi(res.data || []);
        if (res.data && res.data.length > 0) {
          setFormaPodaci(prev => ({ ...prev, zanrId: res.data[0].id.toString() }));
        }
      })
      .catch(err => console.error("Greška pri učitavanju žanrova:", err));

    axios.get(`${API_URL}/sale`)
      .then(res => {
        setBazaSale(res.data || []);
        if (res.data && res.data.length > 0) {
          setFormaPodaci(prev => ({ ...prev, salaId: res.data[0].id.toString() }));
        }
      })
      .catch(err => console.error("Greška pri učitavanju sala:", err));
  }, []);

  useEffect(() => {
    if (eksterniUpit.trim().length > 2) {
      searchExternal(eksterniUpit)
        .then(res => {
          setEksterniRezultati(res.data || []);
          setPrikažiMeni(true);
        })
        .catch(() => setEksterniRezultati([]));
    } else {
      setEksterniRezultati([]);
      setPrikažiMeni(false);
    }
  }, [eksterniUpit]);

  const otvoriModalZaUvoz = (filmIzMenija) => {
    setIzabraniFilm(filmIzMenija);
    setPrikažiModal(true);
    setPrikažiMeni(false);
  };

  const handleSacuvaj = async (e) => {
  e.preventDefault();
  
  const imdbId = izabraniFilm.imdbId || izabraniFilm.id || izabraniFilm.imdbID; 
  const nazivFilma = izabraniFilm.naziv || izabraniFilm.title || izabraniFilm.Title;

  let konacanFilmId = null;

  const postojeciFilm = filmovi.find(f => 
    (f.imdbId && f.imdbId === imdbId) || 
    (f.naziv && f.naziv.toLowerCase() === nazivFilma.toLowerCase())
  );

  if (postojeciFilm) {
    konacanFilmId = postojeciFilm.id;
  } else {
    try {
      const uvozRes = await importFilm(imdbId, parseInt(formaPodaci.zanrId));
      konacanFilmId = uvozRes?.data?.id || uvozRes?.data?.Id;
    } catch (importError) {
      console.warn("Uvoz bacio grešku, ali idemo dalje:", importError);
    }
  }

  if (!konacanFilmId) {
    const ponovniPokusaj = filmovi.find(f => f.naziv.toLowerCase() === nazivFilma.toLowerCase());
    if (ponovniPokusaj) {
      konacanFilmId = ponovniPokusaj.id;
    } else {
      alert("Molimo osvežite stranicu. Film je uvezen ali React još uvek ne vidi njegov ID.");
      return;
    }
  }

  const izabranoVreme = formaPodaci.vreme ? new Date(formaPodaci.vreme).toISOString().replace("Z", "") : new Date().toISOString().replace("Z", "");

  try {
    await axios.post(`${API_URL}/projekcije`, {
       filmId: parseInt(konacanFilmId), 
       salaId: parseInt(formaPodaci.salaId),
       cenaKarte: parseFloat(formaPodaci.cena),
       datumVreme: izabranoVreme 
    });

    alert(`🎉 Uspešno!\nFilm: "${nazivFilma}" je uvršten na repertoar.\nProjekcija je kreirana!`);
    
    setPrikažiModal(false);
    setIzabraniFilm(null);
    setEksterniUpit("");
    osvežiLokalneFilmove();
  } catch (error) {
    console.error("Detalji greške:", error.response?.data);
    alert(`❌ Greška pri kreiranju projekcije:\n${JSON.stringify(error.response?.data || "Proveri unos.")}`);
  }
};

  return (
    <div style={{ width: "100%", maxWidth: "100%", minHeight: "100vh", boxSizing: "border-box", padding: "20px 30px", fontFamily: "sans-serif", background: "#f9f9f9" }}>
      
      {/* PRETRAGA */}
      <div style={{ background: "white", padding: "25px", borderRadius: "12px", boxShadow: "0 2px 10px rgba(0,0,0,0.05)", marginBottom: "30px" }}>
        <h1 style={{ marginTop: 0, fontSize: "24px" }}>🎬 Upravljanje Filmovima</h1>
        
        <div style={{ position: "relative", width: "100%", maxWidth: "500px" }}>
          <label style={{ display: "block", marginBottom: "8px", fontWeight: "bold", color: "#555" }}>
            Pretraži i dodaj film na repertoar:
          </label>
          <input
            placeholder="Ukucaj naziv filma..."
            value={eksterniUpit}
            onChange={e => setEksterniUpit(e.target.value)}
            onFocus={() => setPrikažiMeni(true)}
            style={{ padding: "12px", width: "100%", boxSizing: "border-box", borderRadius: "6px", border: "2px solid #4472C4", fontSize: "15px" }}
          />

          {/* PADUJUĆI MENI ZA PRETRAGU */}
          {prikažiMeni && eksterniRezultati.length > 0 && (
            <div style={{
              position: "absolute", top: "70px", left: 0, width: "100%",
              background: "white", border: "1px solid #ccc", borderRadius: "6px",
              boxShadow: "0px 8px 24px rgba(0,0,0,0.15)", zIndex: 150,
              maxHeight: "320px", overflowY: "auto"
            }}>
              {eksterniRezultati.map((f, index) => {
                const naslov = f.naziv || f.title || f.Title;
                const god = f.godina || f.year || f.Year;
                const slika = f.poster || f.Poster;

                return (
                  <div
                    key={index}
                    onClick={() => otvoriModalZaUvoz(f)}
                    style={{ padding: "12px", cursor: "pointer", display: "flex", alignItems: "center", gap: "15px", borderBottom: "1px solid #eee" }}
                    onMouseEnter={(e) => e.currentTarget.style.background = "#f5f5f5"}
                    onMouseLeave={(e) => e.currentTarget.style.background = "white"}
                  >
                    {slika && slika !== "N/A" ? (
                      <img src={slika} style={{ width: "40px", height: "55px", objectFit: "cover", borderRadius: "4px" }} alt="" />
                    ) : (
                      <div style={{ width: "40px", height: "55px", background: "#eee", borderRadius: "4px" }} />
                    )}
                    <div>
                      <div style={{ fontWeight: "bold", color: "#333" }}>{naslov}</div>
                      <div style={{ fontSize: "13px", color: "#666" }}>{god}</div>
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </div>

      {/* KARTICE SA FILMOVIMA */}
      <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(200px, 1fr))", gap: "25px" }}>
        {filmovi.map(film => (
          <div
            key={film.id}
            onClick={() => navigate(`/filmovi/${film.id}/projekcije`)}
            style={{ cursor: "pointer", border: "1px solid #e0e0e0", background: "white", borderRadius: "10px", overflow: "hidden", boxShadow: "0 4px 10px rgba(0,0,0,0.05)" }}
          >
            {film.poster ? (
              <img src={film.poster} alt={film.naziv} style={{ width: "100%", height: "280px", objectFit: "cover" }} />
            ) : (
              <div style={{ height: "280px", background: "#eee", display: "flex", alignItems: "center", justifyContent: "center" }}>Nema postera</div>
            )}
            <div style={{ padding: "15px" }}>
              <strong style={{ fontSize: "16px", color: "#222" }}>{film.naziv}</strong>
              <p style={{ margin: "6px 0 2px 0", color: "#666", fontSize: "14px" }}>{film.godina}</p>
            </div>
          </div>
        ))}
      </div>

      {/* --- POP-UP MODAL SA DINAMIČKIM PODACIMA --- */}
      {prikažiModal && izabraniFilm && (
        <div style={{
          position: "fixed", top: 0, left: 0, width: "100vw", height: "100vh",
          background: "rgba(0, 0, 0, 0.5)", display: "flex", alignItems: "center", justifyContent: "center",
          zIndex: 1000
        }}>
          <div style={{ background: "white", padding: "30px", borderRadius: "12px", width: "400px", boxShadow: "0 4px 20px rgba(0,0,0,0.3)" }}>
            <h3 style={{ marginTop: 0, marginBottom: "20px" }}>
              🎬 Podešavanje projekcije za: <br/>
              <span style={{ color: "#4472C4" }}>{izabraniFilm.naziv || izabraniFilm.title || izabraniFilm.Title}</span>
            </h3>

            <form onSubmit={handleSacuvaj}>
              
              {/* DINAMIČKI ŽANROVI SA BACKENDA */}
              <div style={{ marginBottom: "15px" }}>
                <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold", fontSize: "14px" }}>Izaberi Žanr:</label>
                <select 
                  value={formaPodaci.zanrId} 
                  onChange={e => setFormaPodaci({...formaPodaci, zanrId: e.target.value})}
                  style={{ width: "100%", padding: "8px", borderRadius: "4px", border: "1px solid #ccc" }}
                >
                  {bazaZanrovi.map(z => (
                    <option key={z.id} value={z.id}>{z.naziv || z.name} (ID: {z.id})</option>
                  ))}
                  {bazaZanrovi.length === 0 && <option value="">Nema učitanih žanrova</option>}
                </select>
              </div>

              {/* DINAMIČKE SALE SA BACKENDA */}
              <div style={{ marginBottom: "15px" }}>
                <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold", fontSize: "14px" }}>Izaberi Salu:</label>
                <select 
                  value={formaPodaci.salaId} 
                  onChange={e => setFormaPodaci({...formaPodaci, salaId: e.target.value})}
                  style={{ width: "100%", padding: "8px", borderRadius: "4px", border: "1px solid #ccc" }}
                >
                  {bazaSale.map(s => (
                    <option key={s.id} value={s.id}>{s.naziv || `Sala ${s.id}`} (ID: {s.id})</option>
                  ))}
                  {bazaSale.length === 0 && <option value="">Nema učitanih sala</option>}
                </select>
              </div>

              {/* INPUT ZA DATUM I VREME PROJEKCIJE */}
              <div style={{ marginBottom: "15px" }}>
                <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold", fontSize: "14px" }}>Vreme projekcije:</label>
                <input 
                  type="datetime-local" 
                  value={formaPodaci.vreme} 
                  onChange={e => setFormaPodaci({...formaPodaci, vreme: e.target.value})}
                  style={{ width: "100%", padding: "8px", boxSizing: "border-box", borderRadius: "4px", border: "1px solid #ccc" }}
                  required
                />
              </div>

              {/* INPUT ZA CENU KARTE */}
              <div style={{ marginBottom: "25px" }}>
                <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold", fontSize: "14px" }}>Cena karte (RSD):</label>
                <input 
                  type="number" 
                  value={formaPodaci.cena} 
                  onChange={e => setFormaPodaci({...formaPodaci, cena: e.target.value})}
                  style={{ width: "100%", padding: "8px", boxSizing: "border-box", borderRadius: "4px", border: "1px solid #ccc" }}
                  required
                />
              </div>

              {/* AKCIJE */}
              <div style={{ display: "flex", justifyContent: "flex-end", gap: "10px" }}>
                <button 
                  type="button" 
                  onClick={() => setPrikažiModal(false)}
                  style={{ padding: "8px 16px", background: "#e0e0e0", border: "none", borderRadius: "4px", cursor: "pointer" }}
                >
                  Otkaži
                </button>
                <button 
                  type="submit" 
                  style={{ padding: "8px 16px", background: "#4472C4", color: "white", border: "none", borderRadius: "4px", cursor: "pointer", fontWeight: "bold" }}
                >
                  Sačuvaj
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}