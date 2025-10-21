import { useState, type FormEvent } from "react";
import type { LoginRequest } from "../types/auth";

interface LoginFormProps {
  onSubmit: (data: LoginRequest) => void;
}

export default function LoginForm({ onSubmit }: LoginFormProps) {
  const [usuario, setUsuario] = useState<string>("");
  const [senha, setSenha] = useState<string>("");

  function handleSubmit(e: FormEvent) {
    e.preventDefault();
    onSubmit({ usuario, senha });
  }

  return (
    <form
      onSubmit={handleSubmit}
      className="w-full max-w-sm flex flex-col items-stretch gap-4 bg-white p-6 rounded-lg shadow-lg"
    >
      <h1 className="text-2xl font-bold text-center mb-2">Login</h1>

      <input
        type="email"
        placeholder="E-mail"
        value={usuario}
        onChange={(e) => setUsuario(e.target.value)}
        required
        className="border rounded p-2"
      />

      <input
        type="password"
        placeholder="Senha"
        value={senha}
        onChange={(e) => setSenha(e.target.value)}
        required
        className="border rounded p-2"
      />

      <button
        type="submit"
        className="bg-blue-600 text-white rounded p-2 hover:bg-blue-700 transition"
      >
        Entrar
      </button>
    </form>
  );
}
