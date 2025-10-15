
# 📄 API_SPEC.md — Pessoa API

## 📌 Visão Geral

A API permite realizar operações de **Cadastro**, **Consulta**, **Atualização** e **Remoção** de pessoas.  
Versão inicial: **v1**  
Versão futura: **v2** (inclui campo obrigatório de endereço).

Base URL:
```
/api/v1/pessoas
```

Formato:
- Requisições e respostas no formato **JSON**.
- Status HTTP compatíveis com boas práticas REST.
- Validações aplicadas em nível de DTO e regras de negócio.

---

## 📁 Endpoints

### 1. Criar Pessoa

**POST** `/api/v1/pessoas`

#### 📨 Request Body
```json
{
  "nome": "João Silva",
  "sexo": "Masculino",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-05-20",
  "naturalidade": "Brasília",
  "nacionalidade": "Brasileira",
  "cpf": "12345678901"
}
```

#### 🧾 Validações
- `nome`: obrigatório, mínimo 3 caracteres.  
- `sexo`: opcional.  
- `email`: opcional, se informado deve ser um e-mail válido, no padrão `nome@dominio.extensao`, sem espaços, com um `@` obrigatório, e uma extensão obrigatória.
- `dataNascimento`: obrigatória, formato `yyyy-MM-dd`, não pode ser futura.  
- `cpf`: obrigatório, deve seguir padrão numérico de 11 dígitos`, utilizando o método oficial de módulo 11(Link do site do Macoratti para referência: https://www.macoratti.net/alg_cpf.htm), único no banco.

#### ✅ Response (201 Created)
```json
{
  "id": 1,
  "nome": "João Silva",
  "sexo": "Masculino",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-05-20",
  "naturalidade": "Brasília",
  "nacionalidade": "Brasileira",
  "cpf": "12345678901",
  "dataCadastro": "2025-10-14T16:00:00Z",
  "dataAtualizacao": "2025-10-14T16:00:00Z"
}
```

#### ❌ Possíveis Erros
| Status | Motivo                              | Exemplo de mensagem                    |
|--------|-------------------------------------|-----------------------------------------|
| 400    | Campo inválido                     | `"CPF inválido"`                        |
| 409    | CPF já cadastrado                  | `"CPF já existe no sistema"`            |
| 422    | Violação de regra de negócio       | `"Data de nascimento não pode ser futura"` |

---

### 2. Consultar Pessoas

**GET** `/api/v1/pessoas`

#### 📎 Parâmetros Opcionais
- `nome` (string) → Filtra por nome parcial.
- `cpf` (string) → Busca exata por CPF.
- `page` (int) → Paginação (padrão de 10 resultados).
- `pageSize` (int) → Tamanho da página.

#### ✅ Response (200 OK)
```json
{
  "data": [
    {
      "id": 1,
      "nome": "João Silva",
      "cpf": "12345678901",
      "dataNascimento": "1990-05-20"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalItems": 1
}
```

---

### 3. Consultar Pessoa por ID

**GET** `/api/v1/pessoas/{id}`

#### ✅ Response (200 OK)
```json
{
  "id": 1,
  "nome": "João Silva",
  "sexo": "Masculino",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-05-20",
  "naturalidade": "Brasília",
  "nacionalidade": "Brasileira",
  "cpf": "12345678901",
  "dataCadastro": "2025-10-14T16:00:00Z",
  "dataAtualizacao": "2025-10-14T16:00:00Z"
}
```

#### ❌ Possíveis Erros
| Status | Motivo              | Exemplo de mensagem      |
|--------|---------------------|---------------------------|
| 404    | Pessoa não encontrada | `"Pessoa não encontrada"` |

---

### 4. Atualizar Pessoa

**PUT** `/api/v1/pessoas/{id}`

#### 📨 Request Body
```json
{
  "nome": "João S. Silva",
  "sexo": "Masculino",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-05-20",
  "naturalidade": "Brasília",
  "nacionalidade": "Brasileira",
  "cpf": "12345678901"
}
```

> ⚠️ O CPF não pode ser alterado.

#### ✅ Response (200 OK)
```json
{
  "id": 1,
  "nome": "João S. Silva",
  "cpf": "12345678901",
  "dataAtualizacao": "2025-10-14T17:00:00Z"
}
```

#### ❌ Possíveis Erros
| Status | Motivo              | Exemplo de mensagem                  |
|--------|---------------------|---------------------------------------|
| 400    | Campo inválido     | `"E-mail inválido"`                   |
| 404    | Não encontrado     | `"Pessoa não encontrada"`             |
| 422    | Regra de negócio   | `"Data de nascimento não pode ser futura"` |

---

### 5. Remover Pessoa

**DELETE** `/api/v1/pessoas/{id}`

#### ✅ Response (204 No Content)

#### ❌ Possíveis Erros
| Status | Motivo              | Exemplo de mensagem        |
|--------|---------------------|----------------------------|
| 404    | Não encontrado     | `"Pessoa não encontrada"`  |

---

## 🧪 Regras de Validação (Resumo)

| Campo            | Obrigatório | Validação                                     |
|------------------|-------------|-----------------------------------------------|
| nome             | ✅          | ≥ 3 caracteres                                |
| sexo             | ❌          | Opcional                                     |
| email            | ❌          | Regex de e-mail válido                        |
| dataNascimento   | ✅          | Formato `yyyy-MM-dd`, não pode ser futura     |
| naturalidade     | ❌          | Opcional                                     |
| nacionalidade    | ❌          | Opcional                                     |
| cpf              | ✅          | 11 dígitos, único                             |

---

## 🔐 Autenticação (Extra)

Para acessar qualquer rota da API, será necessário autenticação via JWT.  
Header:
```
Authorization: Bearer <token>
```

---

## 🆚 Versão 2 da API (Requisito Extra)

Base URL:
```
/api/v2/pessoas
```

- Adicionar campo obrigatório:
  ```json
  "endereco": "Rua Exemplo, 123 - Centro - Brasília/DF"
  ```
- As demais rotas e regras permanecem iguais à versão 1.

---

## 📘 Status Codes

| Código | Descrição                   |
|--------|-----------------------------|
| 200    | OK                          |
| 201    | Criado                      |
| 204    | Sem conteúdo (delete ok)    |
| 400    | Requisição inválida         |
| 401    | Não autorizado (JWT)        |
| 404    | Não encontrado              |
| 409    | Conflito (ex. CPF duplicado)|
| 422    | Erro de regra de negócio    |
| 500    | Erro interno do servidor    |

---

## 🧭 Exemplos de Erro (Padrão de Resposta)

```json
{
  "status": 400,
  "errors": [
    "O campo nome é obrigatório",
    "CPF inválido"
  ]
}
```
