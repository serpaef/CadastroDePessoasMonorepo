import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import LogoffButton from "./components/LogoffButton";
import { Home } from "./pages/ListPessoaPage";
import CreatePessoaPage from "./pages/CreatePessoaPage";

export default function App() {
  return (
    <Router>
      <div className="p-4 flex justify-end">
        <LogoffButton />
      </div>
      <Routes>
        <Route path="/" element={<LoginPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/home" element={<Home />} />
        <Route path="/cadastrar" element={<CreatePessoaPage />} />
      </Routes>
    </Router>
  );
}