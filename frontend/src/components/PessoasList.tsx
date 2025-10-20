import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { get } from "../api/apiClient";
import type { Pessoa } from "../types/Pessoa";
import type { PagedResult } from "../types/PagedResult";

export default function PessoasList() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  async function fetchPessoas(page: number) {
    setLoading(true);
    try {
      const data: PagedResult<Pessoa> = await get(
        `${import.meta.env.VITE_API_URL}/v1/Pessoa?page=${page}`,
        navigate
      );

      setPessoas(data.items);
      setCurrentPage(data.currentPage);
      setTotalPages(data.totalPages);
    } catch (error) {
      console.error("Erro ao buscar pessoas:", error);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchPessoas(currentPage);
  }, [currentPage]);

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white rounded shadow">
      <h1 className="text-2xl font-bold mb-4">Lista de Pessoas</h1>

      {loading && <p>Carregando...</p>}

      {!loading && pessoas.length === 0 && <p>Nenhuma pessoa encontrada.</p>}

      {!loading && pessoas.length > 0 && (
        <table className="w-full border-collapse border">
          <thead>
            <tr className="bg-gray-100">
              <th className="border p-2">ID</th>
              <th className="border p-2">Nome</th>
              <th className="border p-2">Sexo</th>
              <th className="border p-2">Email</th>
              <th className="border p-2">Data Nascimento</th>
              <th className="border p-2">Naturalidade</th>
              <th className="border p-2">Nacionalidade</th>
              <th className="border p-2">CPF</th>
            </tr>
          </thead>
          <tbody>
            {pessoas.map((p) => (
              <tr key={p.id}>
                <td className="border p-2">{p.id}</td>
                <td className="border p-2">{p.nome}</td>
                <td className="border p-2">{p.sexo || "-"}</td>
                <td className="border p-2">{p.email || "-"}</td>
                <td className="border p-2">{p.dataNascimento}</td>
                <td className="border p-2">{p.naturalidade || "-"}</td>
                <td className="border p-2">{p.nacionalidade || "-"}</td>
                <td className="border p-2">{p.cpf}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {/* Paginação */}
      <div className="mt-4 flex justify-between">
        <button
          onClick={() => setCurrentPage((p) => Math.max(p - 1, 1))}
          disabled={currentPage === 1}
          className="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
        >
          Anterior
        </button>
        <span>
          Página {currentPage} de {totalPages}
        </span>
        <button
          onClick={() => setCurrentPage((p) => Math.min(p + 1, totalPages))}
          disabled={currentPage === totalPages}
          className="px-4 py-2 bg-gray-300 rounded disabled:opacity-50"
        >
          Próxima
        </button>
      </div>
    </div>
  );
}
