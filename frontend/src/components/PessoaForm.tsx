import { useState } from "react";
import { post } from "../api/apiClient";
import { useNavigate } from "react-router-dom";

export default function PessoaForm() {
  const [formData, setFormData] = useState({
    nome: "",
    sexo: "",
    email: "",
    dataNascimento: "",
    naturalidade: "",
    nacionalidade: "",
    cpf: "",
  });

  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);

    // Validação básica
    if (!formData.nome || !formData.dataNascimento || !formData.cpf) {
      setError("Preencha todos os campos obrigatórios.");
      return;
    }

    try {
      await post(`${import.meta.env.VITE_API_URL}/v1/Pessoa`, formData, navigate);
      alert("Pessoa cadastrada com sucesso!");
      navigate("/home"); // redireciona para home após cadastro
    } catch (err) {
      console.error(err);
      setError("Erro ao cadastrar pessoa.");
    }
  }

  return (
    <div className="max-w-lg mx-auto bg-white p-6 rounded shadow">
      <h1 className="text-2xl font-bold mb-4">Cadastrar Pessoa</h1>

      {error && <p className="text-red-600 mb-4">{error}</p>}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-medium">Nome *</label>
          <input
            type="text"
            name="nome"
            value={formData.nome}
            onChange={handleChange}
            className="w-full border rounded p-2"
            required
          />
        </div>

        <div>
          <label className="block font-medium">Sexo</label>
          <select
            name="sexo"
            value={formData.sexo}
            onChange={handleChange}
            className="w-full border rounded p-2"
          >
            <option value="">Selecione</option>
            <option value="M">Masculino</option>
            <option value="F">Feminino</option>
            <option value="O">Outro</option>
          </select>
        </div>

        <div>
          <label className="block font-medium">Email</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            className="w-full border rounded p-2"
          />
        </div>

        <div>
          <label className="block font-medium">Data de Nascimento *</label>
          <input
            type="date"
            name="dataNascimento"
            value={formData.dataNascimento}
            onChange={handleChange}
            className="w-full border rounded p-2"
            required
          />
        </div>

        <div>
          <label className="block font-medium">Naturalidade</label>
          <input
            type="text"
            name="naturalidade"
            value={formData.naturalidade}
            onChange={handleChange}
            className="w-full border rounded p-2"
          />
        </div>

        <div>
          <label className="block font-medium">Nacionalidade</label>
          <input
            type="text"
            name="nacionalidade"
            value={formData.nacionalidade}
            onChange={handleChange}
            className="w-full border rounded p-2"
          />
        </div>

        <div>
          <label className="block font-medium">CPF *</label>
          <input
            type="text"
            name="cpf"
            value={formData.cpf}
            onChange={handleChange}
            className="w-full border rounded p-2"
            required
          />
        </div>

        <div className="flex justify-end">
          <button
            type="submit"
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Salvar
          </button>
        </div>
      </form>
    </div>
  );
}
