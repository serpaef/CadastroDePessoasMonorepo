import { useNavigate, useLocation } from "react-router-dom";
import { removeToken, getToken } from "../services/authService";

export default function LogoffButton() {
  const navigate = useNavigate();
  const location = useLocation();

  if (location.pathname === "/" || location.pathname === "/login" || !getToken()) return null;

  function handleLogoff() {
    removeToken();
    navigate("/login");
  }

  return (
    <button
      onClick={handleLogoff}
      className="bg-red-600 text-white rounded px-4 py-2 hover:bg-red-700 transition"
    >
      Logoff
    </button>
  );
}
