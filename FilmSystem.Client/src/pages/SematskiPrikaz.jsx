import { useEffect, useState } from "react";
import { useParams, useSearchParams, useNavigate } from "react-router-dom";
import { getSematskiPrikaz } from "../api/sedista";
import { createRezervacija } from "../api/rezervacije";

export default function SematskiPrikaz() {
  const { id: projekcijaId } = useParams();
  const [searchParams] = useSearchParams();
  const salaId = searchParams.get("salaId");
  const navigate = useNavigate();

  const [sedista, setSedista] = useState([]);
  const [poruka, setPoruka] = useState("");

  const ucitaj = () => {
    getSematskiPrikaz(salaId, projekcijaId).then(res => setSedista(res.data));
  };

  useEffect(() => { ucitaj(); }, [projekcijaId, salaId]);

  const redovi = sedista.reduce((acc, s) => {
    if (!acc[s.brojReda]) acc[s.brojReda] = [];
    acc[s.brojReda].push(s);
    return acc;
  }, {});

  const klikniSediste = async (sediste) => {
    if (!sediste.slobodno) return;
    try {
      await createRezervacija(parseInt(projekcijaId), sediste.id);
      setPoruka(`✅ Rezervacija kreirana za Red ${sediste.brojReda}, Mesto ${sediste.brojMesta}`);
      ucitaj();
    } catch (e) {
      setPoruka("❌ Greška pri kreiranju rezervacije.");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <button onClick={() => navigate(-1)}>← Nazad</button>
      <h2>Prikaz sale</h2>
      {poruka && (
        <div style={{ padding: "10px", margin: "10px 0", background: "#f0f8f0", border: "1px solid #4CAF50", borderRadius: "6px" }}>
          {poruka}
        </div>
      )}
      {/* Legenda */}
      <div style={{ display: "flex", gap: "20px", margin: "16px 0" }}>
        <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
          <div style={{ ...kvadrat, background: "#4CAF50" }} /> Slobodno
        </div>
        <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
          <div style={{ ...kvadrat, background: "#e53935" }} /> Zauzeto
        </div>
      </div>
      {/* Ekran */}
      <div style={{ background: "#ccc", textAlign: "center", padding: "8px", borderRadius: "4px", marginBottom: "24px", width: "100%", maxWidth: "500px" }}>
        EKRAN
      </div>
      {/* Sedista po redovima */}
      {Object.keys(redovi).sort((a, b) => a - b).map(red => (
        <div key={red} style={{ display: "flex", gap: "8px", marginBottom: "8px", alignItems: "center" }}>
          <span style={{ width: "40px", color: "#666", fontSize: "13px" }}>Red {red}</span>
          {redovi[red].sort((a, b) => a.brojMesta - b.brojMesta).map(s => (
            <div
              key={s.id}
              onClick={() => klikniSediste(s)}
              title={`Red ${s.brojReda}, Mesto ${s.brojMesta}`}
              style={{
                ...kvadrat,
                background: s.slobodno ? "#4CAF50" : "#e53935",
                cursor: s.slobodno ? "pointer" : "not-allowed",
                color: "white",
                fontSize: "11px",
                display: "flex",
                alignItems: "center",
                justifyContent: "center"
              }}
            >
              {s.brojMesta}
            </div>
          ))}
        </div>
      ))}
    </div>
  );
}

const kvadrat = { width: "36px", height: "36px", borderRadius: "4px" };