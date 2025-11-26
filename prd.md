# Projeto: Sistema de Gestão Proativa de Insumos de TI (SGPI)

## **1. Visão Geral e Objetivos de Negócio**

O objetivo é desenvolver uma Single Page Application (SPA) robusta, denominada PGP-TI, para migrar a gestão de insumos de TI de um processo reativo (baseado em planilhas) para um modelo **proativo, auditável e orientado por dados**.

O sistema deve servir a três objetivos principais:

1. **Proatividade Operacional:** Garantir a disponibilidade contínua de insumos críticos (cabos, mouses, toners) através da gestão de pontos de ressuprimento (Estoque Mínimo/Máximo).
2. **Conformidade e Auditoria:** Mitigar riscos de fracionamento de despesas, fornecendo um fluxo de compra rastreável—desde a **solicitação** (baseada na necessidade do estoque), passando pela **aprovação** (Ordem de Compra), até o **recebimento** (entrada fiscal).
3. **Eficiência Operacional:** Automatizar o "trabalho manual" de verificar estoque, consolidar listas de compras e registrar entradas/saídas, liberando a equipe de NTI para atividades de maior valor.

## **2. Arquitetura e Stack Tecnológica**

A arquitetura será desacoplada, com uma API de back-end e uma SPA de front-end.

- **Backend: .NET 8**
    - **Arquitetura:** API RESTful seguindo princípios de *design* da API (Endpoints claros, verbos HTTP corretos).
    - **Padrões:** Utilização da estrutura de design de software chamada de *Clean Architecture* com implementação clara da Camada de Serviços (*Service Layer*) e do Padrão Repositório (*Repository*) sobre o EF Core. A lógica de negócio **deve** residir nos serviços.
    - **ORM:** Entity Framework Core 8 (Code-First) com Migrations.
    - **Banco de Dados:** PostgreSQL 17
    - **Segurança:** Autenticação baseada em Token JWT ([ASP.NET](http://asp.net/) Core Identity). Autorização baseada em *Claims/Roles*.
    - **Documentação:** Geração automática de documentação via Swagger (OpenAPI).
- **Frontend: Angular 20**
    - **Arquitetura:** Single Page Application (SPA) modularizada.
    - **Core:** Uso de `standalone components`, `signals` para gerenciamento de estado reativo, e `services` (com `HttpClient`) para consumo da API.
    - **Formulários:** Uso exclusivo de **Reactive Forms** para todas as entradas de dados (CRUDs, movimentações).
    - **UI/UX:** Design responsivo (Mobile-first para operações de estoque), limpo e focado na rapidez das operações de entrada e saída.

## **3. Infraestrutura de Dados e Conectividade**

O ambiente de banco de dados já está provisionado e em execução via Docker. A aplicação não deve tentar criar o banco de dados (catalog), mas deve gerenciar o esquema (tabelas e relações) dentro dele.

- **Ambiente:** PostgreSQL 17 (executando em container Docker orquestrado via docker-compose.yml).
- **Estratégia de Conexão:**
    - A API .NET rodando em ambiente de desenvolvimento (fora do container) se conectará via localhost.
    - O banco de dados sgpi já existe previamente; o Entity Framework Core deve ser configurado para aplicar as **Migrations** (Code-First) e criar as tabelas na inicialização ou via comando CLI, sem tentar recriar o banco de dados em si.
- **Credenciais e Acesso:**
    - **Host:** localhost
    - **Porta:** 5432 (Mapeada do container para o host).
    - **Banco de Dados:** sgpi
    - **Usuário:** sgpi
    - **Senha:** sgpi
- **String de Conexão (Exemplo para appsettings.Development.json):**
    
    "Host=localhost;Port=5432;Database=sgpi;Username=sgpi;Password=sgpi"
    

## **4. Modelagem de Dados**

- `ItemCatalogo`: (O item mestre)
    - `Id`, `Nome`, `DescricaoDetalhada`, `UnidadeMedida` (Enum: `UN`, `KIT`, `METRO`), `CategoriaId` (FK).
- `Categoria`:
    - `Id`, `Nome` (Ex: "Periféricos", "Cabos").
- `Estoque`: (O "Cérebro" do controle)
    - `Id`, `ItemCatalogoId` (FK, Relação 1:1, UNIQUE), `EstoqueMinimo` (EMin), `EstoqueMaximo` (EMax), **`QuantidadeEmEstoque`**.
    - **Nota de Implementação:** `QuantidadeEmEstoque` **deve** ser um campo persistido e denormalizado. Ele **não** deve ser calculado por `SUM()` no banco. Ele será **atomicamente atualizado** (incrementado/decrementado) pela camada de serviço (`MovimentacaoService`) dentro de uma transação.
- `Fornecedor`:
    - `Id`, `Nome`, `EmailContato`, `TelefoneContato`, `CNPJ` (Opcional).
- `CotacaoItem`: (Preço de referência)
    - `Id`, `ItemCatalogoId` (FK), `FornecedorId` (FK), `PrecoUnitario`, `DataCotacao`, `LinkProduto` (Opcional).
- **`OrdemDeCompra` (OC):** (A "Autorização de Compra")
    - `Id`, `DataCriacao`, `UsuarioSolicitanteId` (FK para `Usuario`), `UsuarioAprovadorId` (FK para `Usuario`, Nullable), `DataAprovacao` (Nullable), `StatusOC` (Enum: `PENDENTE`, `APROVADA`, `REJEITADA`, `CONCLUIDA_PARCIAL`, `CONCLUIDA_TOTAL`), `Justificativa`.
- **`ItemOrdemDeCompra` (NOVA ENTIDADE CRÍTICA):** (Itens da OC)
    - `Id`, `OrdemDeCompraId` (FK), `ItemCatalogoId` (FK), `QuantidadeSolicitada`, **`QuantidadeRecebida`**, `ValorUnitarioCotado`.
    - **Nota de Implementação:** `QuantidadeRecebida` é denormalizada e atualizada pelo serviço de entrada.
- `MovimentacaoEstoque`: (O "Ledger" auditável)
    - `Id`, `ItemCatalogoId` (FK), `TipoMovimentacao` (Enum: `ENTRADA`, `SAIDA`, `AJUSTE_INVENTARIO`), `Quantidade`, `Data`, `UsuarioId` (FK).
    - **Campos de Entrada (Nullable):** `OrdemDeCompraId` (FK), `NotaFiscal`, `ValorUnitarioEfetivo`.
    - **Campos de Saída (Nullable):** `Solicitante`.
    - **Campos de Ajuste (Nullable):** `ObservacaoAjuste`.
- `Usuario` (Extensão do [ASP.NET](http://asp.net/) IdentityUser):
    - `NomeCompleto`, `Perfil` (Enum: `Admin`, `OperadorNTI`, `GestorNTI`).

**4. Módulos e Requisitos Funcionais (RFs)**

A aplicação será organizada pelos seguintes fluxos de trabalho:

**Módulo 1: Dashboard e Inventário (Core)**

- **RF001 (Dashboard de Status):** Tela principal exibindo a grade de `Estoque`.
    - Colunas: `Item`, `Categoria`, `QuantidadeEmEstoque`, `EstoqueMinimo`, `Status`.
    - **Lógica Chave:** A coluna "Status" (calculada no *backend view model*):
        - `IF QuantidadeEmEstoque <= EstoqueMinimo THEN "COMPRAR"`
        - `IF QuantidadeEmEstoque > EstoqueMinimo AND QuantidadeEmEstoque < (EstoqueMinimo * 1.2) THEN "ATENÇÃO"`
        - `ELSE "OK"`
    - Deve permitir filtragem por `Categoria` e `Status`.
- **RF002 (Detalhe do Item - *Drill-down*):** Ao clicar em um item do dashboard (RF001), o usuário deve ver um *modal* ou tela de detalhe com (1) os dados do `ItemCatalogo`, (2) as definições de `Estoque` (Min/Max) e (3) o histórico de `MovimentacaoEstoque` (o "Ledger") para aquele item.

**Módulo 2: Operações de Estoque (Perfil: OperadorNTI)**

- **RF003 (Registro de Saída de Consumo):** Formulário simples (Reactive Form) para consumo interno.
    - Campos: `Item` (Autocomplete/Dropdown **otimizado para leitura de código de barras**), `Quantidade`, `Solicitante` (Texto).
    - **Backend (Lógica Atômica):** O `MovimentacaoService` deve:
        1. Iniciar Transação.
        2. Criar `MovimentacaoEstoque` (Tipo=`SAIDA`).
        3. Decrementar `Estoque.QuantidadeEmEstoque` (com verificação de saldo).
        4. Comitar Transação.
- **RF004 (Registro de Entrada de Compras - *Via OC*):** Formulário para recebimento de material. **Este é o ponto-chave de conformidade.**
    - O usuário primeiro seleciona uma `OrdemDeCompra` com Status=`APROVADA` ou `CONCLUIDA_PARCIAL`.
    - O formulário carrega os `ItemOrdemDeCompra` pendentes dessa OC.
    - O usuário informa os dados da NF (cabeçalho) e preenche a `QuantidadeRecebida` e o `ValorUnitarioEfetivo` para cada item que chegou.
    - **Backend (Lógica Atômica Complexa):** O `EntradaService` deve (para cada item recebido):
        1. Iniciar Transação.
        2. Criar `MovimentacaoEstoque` (Tipo=`ENTRADA`, com FK da OC, NF, Valor).
        3. Incrementar `Estoque.QuantidadeEmEstoque`.
        4. Incrementar `ItemOrdemDeCompra.QuantidadeRecebida`.
        5. Verificar se a `OrdemDeCompra` foi total ou parcialmente concluída e atualizar seu `StatusOC`.
        6. Comitar Transação.
- **RF005 (Ajuste de Inventário) (Perfil: Admin):** Formulário para ajuste manual após contagem física.
    - Campos: `Item`, `NovaQuantidadeContada`, `ObservacaoAjuste`.
    - **Backend (Lógica Atômica):** O serviço calculará a diferença (`NovaQuantidadeContada - QuantidadeEmEstoqueAtual`).
        1. Iniciar Transação.
        2. Criar `MovimentacaoEstoque` (Tipo=`AJUSTE_INVENTARIO`, `Quantidade` = diferença, positiva ou negativa).
        3. Atualizar `Estoque.QuantidadeEmEstoque` para a `NovaQuantidadeContada`.
        4. Comitar Transação.

**Módulo 3: Planejamento e Compras (Perfil: GestorNTI)**

- **RF006 (Geração da Lista de Compras):** Uma tela dedicada que filtra o RF001 por `Status == "COMPRAR"`.
    - Exibe: `Item`, `Qtd. a Comprar` (Calculado: `EstoqueMaximo - QuantidadeEmEstoque`), `FornecedorPreferencial` (baseado na `CotacaoItem`), `CustoEstimado`.
- **RF007 (Criação da Ordem de Compra):** O Gestor seleciona os itens da Lista (RF006) e clica em "Gerar Ordem de Compra".
    - Isso cria uma nova `OrdemDeCompra` (Status=`PENDENTE`) e os respectivos `ItemOrdemDeCompra`, vinculados ao `UsuarioSolicitanteId` (o gestor logado).
- **RF008 (Aprovação da Ordem de Compra):** O Gestor (ou um superior, se a regra de negócio exigir) revisa a `OrdemDeCompra` `PENDENTE` e pode "Aprovar" ou "Rejeitar".
    - Ao Aprovar, o sistema atualiza o `StatusOC` para `APROVADA`, registra o `UsuarioAprovadorId` e a `DataAprovacao`.
    - A OC agora está apta a ser recebida pelo RF004.

**Módulo 4: Administração e Catálogo (Perfil: Admin)**

- **RF009 (Gestão de Itens do Catálogo):** CRUD completo para `ItemCatalogo`.
- **RF010 (Gestão de Categorias):** CRUD completo para `Categoria`.
- **RF011 (Gestão de Fornecedores):** CRUD completo para `Fornecedor`.
- **RF012 (Gestão de Cotações):** CRUD para `CotacaoItem` (preços de referência).
- **RF013 (Definição de Níveis de Estoque):** Interface dedicada para CRUD dos registros de `Estoque` (definindo `EstoqueMinimo` e `EstoqueMaximo` para cada item).

**Módulo 5: Conformidade e Auditoria (Perfil: Admin, GestorNTI)**

- **RF014 (Gestão de Usuários):** CRUD para `Usuario` e atribuição de Perfis (`Admin`, `GestorNTI`, `OperadorNTI`).
- **RF015 (Relatório de Movimentações - Auditoria):** Relatório de `MovimentacaoEstoque` (o "Ledger").
    - Filtros: Data, Tipo, Usuário, Item, NF.
    - Deve exibir valores de entrada (`ValorUnitarioEfetivo * Quantidade`) para prestação de contas do cartão corporativo.
    - Deve permitir exportação (CSV/Excel).
- **RF016 (Relatório de Ordens de Compra - Conformidade):** Relatório de `OrdemDeCompra`.
    - Filtros: Status, Data, Solicitante, Aprovador.
    - Permite à gestão verificar todo o ciclo de compra (Quem pediu, quem aprovou, quanto custou, o que já foi recebido).

**5. Requisitos Não-Funcionais (RNFs) Clarificados**

- **RNF-001 (Segurança e Controle de Acesso):** A API deve implementar autorização baseada em perfis. O Frontend deve usar *Route Guards* do Angular para proteger módulos.

| Perfil | Dashboard (RF001) | Operações (RF003, RF004) | Compras (RF006-008) | Administração (RF009-014) | Auditoria (RF015-016) |
| --- | --- | --- | --- | --- | --- |
| **Admin** | R | R/W (RF005) | R | R/W | R/W |
| **GestorNTI** | R | R | R/W | R | R/W |
| **OperadorNTI** | R | R/W (RF003, RF004) | Não | Não | Não |
- **RNF-002 (Atomicidade e Transações):** Todas as operações que alteram `Estoque.QuantidadeEmEstoque` (RF003, RF004, RF005) **devem** ser executadas dentro de uma transação de banco de dados (`DbContext.Database.BeginTransactionAsync()`) na camada de serviço do .NET. A falha em *qualquer* etapa deve reverter a operação inteira (ex: falhar se não conseguir criar a `MovimentacaoEstoque`).
- **RNF-003 (Performance):** O Dashboard (RF001) deve carregar em menos de 2 segundos. As operações de entrada/saída (RF003, RF004) devem ter resposta inferior a 500ms.
- **RNF-004 (Documentação da API):** O *build* da API .NET deve gerar um `swagger.json` válido.
- **RNF-005 (UX Operacional):** As telas do Módulo 2 (Operações) devem ser responsivas e perfeitamente funcionais em dispositivos móveis (tablets/celulares) para uso no local do estoque.

**6. Entregáveis**

1. Repositório Git (Backend): Código-fonte da API .NET 8, incluindo Migrations do EF Core.
2. Repositório Git (Frontend): Código-fonte da aplicação SPA Angular 20.
3. Documentação da API (Endpoint Swagger publicado) e um breve `README.md` com instruções de *build* e *deploy*.