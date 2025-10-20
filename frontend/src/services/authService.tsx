import type { LoginRequest, LoginResponse } from "../types/auth";

export const TOKEN_KEY = "authToken";

export async function login(
  usuario: string,
  senha: string
): Promise<LoginResponse> {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/Login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ usuario, senha } as LoginRequest),
  });

  if (!response.ok) {
    throw new Error("Credenciais inválidas");
  }

  const data: LoginResponse = await response.json();
  setToken(data.token);
  return data;
}

export function setToken(token: string) {
  localStorage.setItem(TOKEN_KEY, token);
}

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function removeToken() {
  localStorage.removeItem(TOKEN_KEY);
}

export function isTokenValid(): boolean {
  const token = getToken();
  if (!token) return false;

  try {
    const payloadBase64 = token.split(".")[1];
    const payloadJson = atob(payloadBase64);
    const payload = JSON.parse(payloadJson);

    const now = Math.floor(Date.now() / 1000);
    return payload.exp && payload.exp > now;
  } catch (error) {
    console.error("Erro ao validar token:", error);
    return false;
  }
}