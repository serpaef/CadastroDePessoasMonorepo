import { useState, type FormEvent } from "react";
import type { LoginRequest } from "../types/auth";

interface LoginFormProps {
  onSubmit: (data: LoginRequest) => void;
}

export default function LoginForm({ onSubmit }: LoginFormProps) {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  function handleSubmit(e: FormEvent) {
    e.preventDefault();
    onSubmit({ email, password });
  }

  return (
    <form
      onSubmit={handleSubmit}
      className="flex flex-col gap-3 max-w-sm mx-auto bg-white p-6 rounded-lg shadow"
    >
      <h1 className="text-2xl font-bold text-center mb-2">Login</h1>

      <input
        type="email"
        placeholder="E-mail"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        required
        className="border rounded p-2"
      />

      <input
        type="password"
        placeholder="Senha"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
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
