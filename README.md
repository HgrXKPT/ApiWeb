# ApiGerenciamentoDatabase

Este repositório contém o código-fonte do **ApiGerenciamentoDatabase**, um sistema para gerenciar usuários, equipes e projetos. O projeto está atualmente **em desenvolvimento** e continua sendo aprimorado.

---

## 🚀 Sobre o Projeto

O **ApiGerenciamentoDatabase** é uma aplicação que permite:
- Gerenciamento de usuários com validações personalizadas.
- Cadastro e organização de equipes.
- Criação e acompanhamento de projetos.

O sistema utiliza validações robustas, mapeamentos de banco de dados e padrões de DTO para comunicação eficiente.

---

## 📌 Status do Projeto

**Em desenvolvimento.**

Funcionalidades podem estar incompletas ou sujeitas a mudanças.

---
## ⚡ Como Configurar o Projeto

Siga as etapas abaixo para configurar o projeto localmente:

### 1. Clone o repositório
Execute o seguinte comando para clonar o repositório e acessar o diretório do projeto:
```bash
git clone https://github.com/seu-usuario/WebApplication1.git
cd WebApplication1

###
```
### 2. Configure o banco de dados

Instale uma instância de SQL Server (ou outro banco compatível).

Configure a string de conexão no arquivo appsettings.json, seguindo o formato:

```bash
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=SEU_BANCO;User Id=SEU_USUARIO;Password=SUA_SENHA;"
  }
}
```

### 3. Restaure os pacotes NuGet
Restaure os pacotes necessários usando o comando:

```bash
dotnet restore
```

### 4. Adicionar migração no database
Abra o Console de gerenciador nuget e digite:

```bash
Add-Migration
```

### 5. Rode o projeto normalmente

---

## 🛠️ Estrutura do Código

### **Modelos**

#### Users
- Representa os usuários do sistema.
- **Principais Atributos**:
  - `Nome`, `Email`, `RA`, `SenhaHash`.
- **Validações**:
  - Campos obrigatórios.
  - Validação de formato para e-mail.
  - Campos "trimados" automaticamente.
- **Relacionamento**:
  - Possui um relacionamento opcional com **Equipes**.

#### Projetos
- Representa os projetos gerenciados no sistema.
- **Principais Atributos**:
  - `NomeProjeto`, `Descricao`, `DataCriacao`, `DataFinal`.
- **Validações**:
  - Campos obrigatórios.
  - Verificação de datas (não futuras e coerentes com início/término).
- **Relacionamento**:
  - Associado a uma **Equipe**.

#### Equipes
- Representa as equipes cadastradas.
- **Principais Atributos**:
  - `NomeEquipe`, `QuantidadeMembros`, `QuantidadeProjetos`.
- **Validações**:
  - Campos obrigatórios.
  - Valores negativos não permitidos.
- **Relacionamento**:
  - Coleção de **Users** e **Projetos** associados.

---

### **DTOs (Data Transfer Objects)**

#### LoginDto
- Usado para autenticação de usuários.
- **Atributos**:
  - `Email` e `Senha` (ambos obrigatórios).

#### CreateUserDto
- Usado para criação de novos usuários.
- **Atributos**:
  - `Nome`, `Email`, `Senha`.
- **Validações**:
  - Tamanho mínimo e máximo da senha.
  - Validação de formato para e-mail.

---

### **Banco de Dados**

- Configurado com **Entity Framework Core**.
- **Relacionamentos**:
  - **Users** possuem relação com **Equipes** (um para muitos).
  - **Projetos** possuem relação com **Equipes** (um para muitos).

---

### **Controladores**

#### UserController
- Operações com usuários:
  - Registro de novos usuários com senha criptografada.
  - Login com validação de credenciais.
  - Consulta de usuários por e-mail.
  - Exclusão de usuários.

#### EquipeController
- Operações com equipes:
  - Cadastro de novas equipes.
  - Exclusão de equipes por nome.
  - Adição de usuários a equipes existentes.

---

## 🧑‍💻 Tecnologias Utilizadas

- **ASP.NET Core** para a criação da API.
- **Entity Framework Core** para persistência e mapeamento de dados.
- **BCrypt.Net** para criptografia de senhas.
- **C#** como linguagem de programação principal.

---

## ⚠️ Observação

Este projeto está em **desenvolvimento**, e funcionalidades podem estar incompletas ou passar por mudanças. Feedbacks e contribuições são bem-vindos para melhorar o sistema!

---

Se precisar de ajustes, adições ou algo mais específico, me avise! 😊
