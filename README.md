# Azure Functions - Exemplo Didatico (HTTP Trigger)

Este projeto e uma Azure Function em .NET 8 (modelo **isolated worker**) com um endpoint HTTP simples para estudo.

A funcao principal fica em `HttpTriggerExample.cs` e:
- aceita os metodos `GET` e `POST`;
- recebe o parametro `name` pela query string (`?name=...`) ou no corpo JSON (`{ "name": "..." }`);
- responde com uma mensagem de boas-vindas.

## 1. Tecnologias usadas

- .NET 8
- Azure Functions v4
- Modelo isolated worker
- Application Insights (configurado no host)

## 2. Estrutura dos arquivos principais

- `Program.cs`: inicializa a aplicacao Functions e configura servicos.
- `HttpTriggerExample.cs`: endpoint HTTP chamado `HttpTriggerExample`.
- `host.json`: configuracoes do runtime e logging.
- `local.settings.json`: configuracoes locais (nao usar em producao).
- `Azure Functions.csproj`: dependencias e configuracao do projeto.

## 3. Pre-requisitos

Antes de rodar localmente, instale:

1. .NET SDK 8
2. Azure Functions Core Tools v4
3. (Opcional) VS Code com extensao Azure Functions

### Verificando instalacao

```powershell
dotnet --version
func --version
```

## 4. Como executar localmente

Na pasta raiz do projeto:

```powershell
dotnet build
func start
```

Quando o host subir, voce vera uma URL parecida com:

```text
http://localhost:7071/api/HttpTriggerExample
```

## 5. Testando a funcao

## 5.1 GET com query string

No navegador ou terminal:

```powershell
curl "http://localhost:7071/api/HttpTriggerExample?name=Jukia"
```

Resposta esperada:

```text
Hello, Jukia! Your Azure Function is working.
```

## 5.2 GET sem name

```powershell
curl "http://localhost:7071/api/HttpTriggerExample"
```

Resposta esperada (mensagem de ajuda):

```text
Function running! Add ?name=Jukia to the URL or send JSON { "name": "Jukia" }
```

## 5.3 POST com JSON

```powershell
curl -X POST "http://localhost:7071/api/HttpTriggerExample" \
  -H "Content-Type: application/json" \
  -d '{"name":"Jukia"}'
```

Resposta esperada:

```text
Hello, Jukia! Your Azure Function is working.
```

## 5.4 POST com JSON invalido

```powershell
curl -X POST "http://localhost:7071/api/HttpTriggerExample" \
  -H "Content-Type: application/json" \
  -d '{name:"sem-aspas"}'
```

Resposta esperada:

```text
Invalid JSON. Send something like: { "name": "Jukia" }
```

## 6. Entendendo o fluxo da funcao

1. O runtime recebe uma requisicao HTTP em `/api/HttpTriggerExample`.
2. A funcao tenta ler `name` da query string.
3. Se `name` nao vier e houver body, tenta fazer parse do JSON.
4. Se o JSON for invalido, retorna erro de requisicao invalida.
5. Se `name` continuar vazio, retorna uma mensagem de orientacao.
6. Se `name` existir, retorna mensagem de sucesso com o nome.

## 7. Configuracoes importantes

### `local.settings.json`

- `FUNCTIONS_WORKER_RUNTIME`: deve ser `dotnet-isolated`.
- `AzureWebJobsStorage`: pode ficar vazio para este exemplo HTTP simples local.

> Observacao: `local.settings.json` e apenas para ambiente local e nao deve ser publicado com segredos.

## 8. Build e publicacao

Comandos uteis:

```powershell
dotnet clean
dotnet build
dotnet publish --configuration Release
```

## 9. Proximos passos sugeridos

- Adicionar validacoes mais detalhadas para o campo `name`.
- Criar testes automatizados para o endpoint.
- Trocar o `AuthorizationLevel.Anonymous` por um nivel mais seguro quando necessario.
- Conectar com Azure Storage ou outro servico para persistencia de dados.
