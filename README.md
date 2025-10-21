# Cadastro de Pessoas - Local Development

Este guia rápido mostra como rodar o frontend e backend em ambiente de desenvolvimento.

---

## Pré-requisitos

* Node.js (v18+)
* npm
* .NET 8 SDK

---

## Rodar o Backend

1. Navegue até a pasta do backend:

```bash
cd backend
```

2. Rode a aplicação com hot reload:

```bash
dotnet watch run
```

O backend estará disponível em `https://localhost:5xxx` com a porta entre `5000` e `5999` a depender das portas disponíveis;

---

## Rodar o Frontend

1. Navegue até a pasta do frontend:

```bash
cd frontend
```

2. Instale dependências:

```bash
npm install
```

3. Rode o frontend:

```bash
npm run dev
```

O frontend estará disponível em `http://localhost:5173` (ou conforme indicado pelo Vite).

---

## Observações

* Certifique-se de definir a variável de ambiente do frontend apontando para o backend, no arquivo .env:

```bash
VITE_API_URL=https://localhost:5xxx
```

* O frontend irá consumir o backend usando esta variável. Há um arquivo .env.example para fins de consulta na pasta do frontent
