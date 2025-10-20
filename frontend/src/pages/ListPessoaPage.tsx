import { Link } from "react-router-dom";
import PessoasList from "../components/PessoasList";

export function Home() {
  return (
    <>
    <header className="bg-white shadow p-4 flex justify-between items-center">
        <div className="flex gap-2">
            <Link
            to="/cadastrar"
            className="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
            >
            Cadastrar Pessoa
            </Link>
        </div>
    </header>
    <div className="min-h-screen bg-gray-100">
      <header className="bg-white shadow p-4 flex justify-between items-center">
        <h1 className="text-xl font-bold">Listagem de Pessoas</h1>
      </header>

      <main className="p-6">
        <PessoasList />
      </main>
    </div>
    </>
  );
}