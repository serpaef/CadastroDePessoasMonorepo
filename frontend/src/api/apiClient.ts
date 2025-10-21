import { getToken, isTokenValid, removeToken } from "../services/authService";
import type { NavigateFunction } from "react-router-dom";

interface ApiClientOptions extends RequestInit {
  authenticated?: boolean; 
}

export async function apiClient(
  url: string,
  options: ApiClientOptions = {},
  navigate?: NavigateFunction
) {
  if (options.authenticated) {
    if (!isTokenValid()) {
      alert("Token expirado. Faça login novamente.");
      removeToken();
      navigate?.("/login");
      return Promise.reject("Token expirado");
    }
  }

  const headers = new Headers({
    "Content-Type": "application/json",
    });

  if (options.authenticated) {
    const token = getToken();
    if (token) {
      headers.append("Authorization", `Bearer ${token}`);
    }
  }

  const response = await fetch(url, {
    ...options,
    headers,
  });

  if (!response.ok) {
    const errorText = await response.text();
    return Promise.reject(new Error(errorText || response.statusText));
  }

  try {
    return await response.json();
  } catch {
    return null;
  }
}

/**
 * Funções auxiliares para cada método HTTP
 */
export const get = (url: string, navigate?: NavigateFunction) =>
  apiClient(url, { method: "GET", authenticated: true }, navigate);

export const post = (url: string, body: any, navigate?: NavigateFunction) =>
  apiClient(
    url,
    { method: "POST", body: JSON.stringify(body), authenticated: true },
    navigate
  );

export const put = (url: string, body: any, navigate?: NavigateFunction) =>
  apiClient(
    url,
    { method: "PUT", body: JSON.stringify(body), authenticated: true },
    navigate
  );

export const del = (url: string, navigate?: NavigateFunction) =>
  apiClient(url, { method: "DELETE", authenticated: true }, navigate);
