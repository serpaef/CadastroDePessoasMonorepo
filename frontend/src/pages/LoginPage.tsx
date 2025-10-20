import { useState } from "react";
import LoginForm from "../components/LoginForm";
import { login } from "../services/authService";
import type { LoginRequest } from "../types/auth";

export default function LoginPage() {
  const [error, setError] = useState<string>("");

  async function handleLogin({ email, password }: LoginRequest) {
    try {
      setError("");
      const result = await login(email, password);
      alert(`Login bem-sucedido! Bem-vindo, ${result.user.name}`);
      window.location.href = "/dashboard";
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError("Erro desconhecido");
      }
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div>
        <LoginForm onSubmit={handleLogin} />
        {error && <p className="text-red-600 text-center mt-2">{error}</p>}
      </div>
    </div>
  );
}
