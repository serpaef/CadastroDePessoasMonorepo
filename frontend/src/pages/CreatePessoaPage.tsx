import { Link } from "react-router-dom";
import PessoaForm from "../components/PessoaForm";

export default function CreatePessoaPage() {
  return (
    <>
    <header className="bg-white shadow p-4 flex justify-between items-center">
        <h1 className="text-xl font-bold">Cadastro de Pessoas</h1>
        <div className="flex gap-2">
            <Link
            to="/home"
            className="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
            >
            Cadastrar Pessoa
            </Link>
        </div>
    </header>
    <div className="min-h-screen bg-gray-100 py-10">
      <PessoaForm />
    </div>
    </>
  );
}