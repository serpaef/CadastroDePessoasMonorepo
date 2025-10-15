# Guia de Início Rápido: Aplicação .NET 8.0

Este README fornece os comandos básicos para gerenciar sua aplicação desenvolvida em **.NET 8.0**.

---

## 1. Pré-requisitos

Certifique-se de que você tem instalado:

- **SDK do .NET 8.0** → Para compilar, testar e executar nativamente.  
- **Docker** → Para construir e executar a aplicação como um container.

---

## 2. Comandos Essenciais do `dotnet CLI` (Execução Nativa)

Assuma que você está no diretório raiz do projeto (onde se encontra o arquivo `.csproj`).

### 2.1. Restaurar Dependências
Baixa todos os pacotes NuGet necessários para o projeto:
```bash
dotnet restore
```

### 2.2. Compilar (Build)
Compila o código-fonte em binários executáveis:
```bash
dotnet build
```

### 2.3. Executar Testes Unitários
Executa os testes contidos no projeto de testes (`backend.Tests.csproj`):
```bash
dotnet test
```

### 2.4. Iniciar a Aplicação (Run)
Compila e executa o projeto diretamente:
```bash
dotnet run
```

## 3. Comandos Docker (Containerização)

### 3.1. Construir a Imagem Docker
Este comando constrói a imagem usando o `Dockerfile`, aplicando a tag `-t` para facilitar a identificação.  
Substitua `<minha-app>` pelo nome desejado para a imagem:
```bash
docker build -t minha-app:latest .
```

### 3.2. Executar a Imagem Docker
Inicia um container a partir da imagem construída.  
- A flag `-d` executa em segundo plano (detached).  
- A flag `-p` mapeia a porta do host (primeiro número) para a porta interna do container (segundo número, geralmente `8080` ou `80` para aplicações .NET):

```bash
docker run -d -p 8080:8080 minha-app:latest
```

### 3.3. Parar o Container
Se você executou o container em segundo plano (com `-d`), localize o ID ou nome do container e use o comando `stop`:

```bash
# Listar containers em execução
docker ps

# (Localize o CONTAINER ID)
docker stop <CONTAINER_ID_ou_NOME>
```

---
