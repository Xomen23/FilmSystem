import { useEffect, useState } from "react";
import { getAllRezervacije, potvrdiRezervaciju, platiRezervaciju, otkaziRezervaciju } from "../api/rezervacije";

const boje = {
  Kreirana: "#FFA726",
  Potvrdjana: "#42A5F5",
  Placena: "#66BB6A",
  Otkazana: "#EF5350",
  Istekla: "#9E9E9E"
};

export default function Rezervacije() {
  const [rezervacije, setRezervacije] = useState([]);

  const ucitaj = () => getAllRezervacije().then(res => setRezervacije(res.data));
  useEffect(() => { ucitaj(); }, []);

  const akcija = async (fn, id) => {
    try { await fn(id); ucitaj(); }
    catch (e) { alert(e.response?.data || "Greška"); }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h1>Rezervacije</h1>
      <table style={{ borderCollapse: "collapse", width: "100%" }}>
        <thead>
          <tr style={{ background: "#4472C4", color: "white" }}>
            <th style={th}>ID</th>
            <th style={th}>Projekcija</th>
            <th style={th}>Sedište</th>
            <th style={th}>Vreme</th>
            <th style={th}>Status</th>
            <th style={th}>Akcije</th>
          </tr>
        </thead>
        <tbody>
          {rezervacije.map(r => (
            <tr key={r.id}>
              <td style={td}>{r.id}</td>
              <td style={td}>{r.projekcijaId}</td>
              <td style={td}>{r.sedisteId}</td>
              <td style={td}>{new Date(r.vremeKreiranja).toLocaleString("sr-RS")}</td>
              <td style={td}>
                <span style={{
                  background: boje[r.status] || "#ccc",
                  color: "white", padding: "3px 10px",
                  borderRadius: "12px", fontSize: "13px"
                }}>
                  {r.status}
                </span>
              </td>
              <td style={td}>
                {r.status === "Kreirana" && (
                  <>
                    <Btn color="#42A5F5" onClick={() => akcija(potvrdiRezervaciju, r.id)}>Potvrdi</Btn>
                    <Btn color="#EF5350" onClick={() => akcija(otkaziRezervaciju, r.id)}>Otkaži</Btn>
                  </>
                )}
                {r.status === "Potvrdjana" && (
                  <>
                    <Btn color="#66BB6A" onClick={() => akcija(platiRezervaciju, r.id)}>Plati</Btn>
                    <Btn color="#EF5350" onClick={() => akcija(otkaziRezervaciju, r.id)}>Otkaži</Btn>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

const Btn = ({ children, onClick, color }) => (
  <button onClick={onClick} style={{
    marginRight: "6px", padding: "4px 10px", background: color,
    color: "white", border: "none", borderRadius: "4px", cursor: "pointer"
  }}>
    {children}
  </button>
);

const th = { padding: "10px", textAlign: "left", border: "1px solid #ddd" };
const td = { padding: "10px", border: "1px solid #ddd" };