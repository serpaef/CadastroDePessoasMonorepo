import type { LoginRequest, LoginResponse } from "../types/auth";

export async function login(
  email: string,
  password: string
): Promise<LoginResponse> {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password } as LoginRequest),
  });

  if (!response.ok) {
    throw new Error("Credenciais inválidas");
  }

  const data: LoginResponse = await response.json();
  localStorage.setItem("token", data.token);
  return data;
}