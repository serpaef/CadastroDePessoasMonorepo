import { useState } from "react";
import LoginForm from "../components/LoginForm";
import { setToken, login } from "../services/authService";
import type { LoginRequest } from "../types/auth";

export default function LoginPage() {
  const [error, setError] = useState<string>("");

  async function handleLogin({ usuario, senha }: LoginRequest) {
    try {
      setError("");
      const result = await login(usuario, senha);
      setToken(result.token); // vulnerável a XSS. O ideal era usar HttpOnly cookies. Mas por causa do tempo, setei no localstorage.
      window.location.href = "/home";
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError("Erro desconhecido");
      }
    }
  }

  return (
    <div className="page-container">
      <LoginForm onSubmit={handleLogin} />
      {error && <p className="text-red-600 text-center mt-2">{error}</p>}
    </div>
  );
}
