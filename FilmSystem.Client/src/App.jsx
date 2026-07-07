import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import Filmovi from "./pages/Filmovi";
import Projekcije from "./pages/Projekcije";
import SematskiPrikaz from "./pages/SematskiPrikaz";
import Rezervacije from "./pages/Rezervacije";

export default function App() {
  return (
    <BrowserRouter>
      <nav style={{ background: "#4472C4", padding: "12px 20px", display: "flex", gap: "20px" }}>
        <Link to="/filmovi" style={{ color: "white", textDecoration: "none", fontWeight: "bold" }}>🎬 Filmovi</Link>
        <Link to="/rezervacije" style={{ color: "white", textDecoration: "none", fontWeight: "bold" }}>🎟 Rezervacije</Link>
      </nav>
      <Routes>
        <Route path="/" element={<Filmovi />} />
        <Route path="/filmovi" element={<Filmovi />} />
        <Route path="/filmovi/:id/projekcije" element={<Projekcije />} />
        <Route path="/projekcije/:id/sala" element={<SematskiPrikaz />} />
        <Route path="/rezervacije" element={<Rezervacije />} />
      </Routes>
    </BrowserRouter>
  );
}