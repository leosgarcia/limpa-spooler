# Limpa Spooler de Impressão 🖨️✨

Um utilitário de sistema projetado para ambientes corporativos (Windows 10 e 11) que facilita a limpeza e resolução de travamentos na fila de impressão do Windows de forma rápida, segura e transparente.

![Badge](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Badge](https://img.shields.io/badge/WPF-UI-blueviolet.svg)
![Badge](https://img.shields.io/badge/Plataforma-Windows-lightgrey.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

## 📋 Sobre o Projeto

O **Limpa Spooler** é uma aplicação WPF desenvolvida em C# (.NET 8). Seu objetivo principal é fornecer aos usuários (especialmente equipes de suporte e TI) uma ferramenta de 1-clique para reiniciar o serviço de impressão do Windows e limpar arquivos corrompidos da fila, resolvendo travamentos sem a necessidade de comandos manuais no terminal.

### ✨ Funcionalidades

- **Design Moderno:** Interface de usuário limpa e em Dark Mode.
- **Log em Tempo Real:** Acompanhamento visual de todas as etapas (parada do serviço, exclusão de arquivos, reinício do serviço).
- **Elevação Automática:** Solicita privilégios de Administrador (UAC) automaticamente ao ser executado, uma exigência para manipular serviços do Windows.
- **Deploy Simplificado:** Compilado como um único executável estático (`.exe`) auto-contido. Não requer a instalação do .NET no computador do cliente!

## ⚙️ Como Funciona?

Ao clicar em **Limpar Fila de Impressão**, o aplicativo executa os seguintes passos assincronamente:
1. Interage com a API do Windows para parar o serviço `Spooler`.
2. Acessa o diretório de fila nativo do Windows (`C:\Windows\System32\spool\PRINTERS`).
3. Itera e exclui de forma segura todos os arquivos travados (`.SHD` e `.SPL`).
4. Reinicia o serviço `Spooler`.
5. Valida o sucesso da operação e emite um log visual completo de toda a operação.

## 🚀 Como Baixar e Usar

Graças à integração com o **GitHub Actions**, você não precisa compilar o projeto para testar. Sempre que houver uma atualização, um executável limpo é gerado automaticamente!

1. Vá na aba [**Actions**](../../actions) deste repositório (ou verifique os **Releases**).
2. Baixe o artefato `LimpaSpooler-Exe`.
3. Extraia e execute o `LimpaSpooler.exe`.
4. (O Windows solicitará privilégios de Administrador, basta aceitar).

## 🛠️ Como Compilar Localmente (Apenas para Windows)

Se você for um desenvolvedor e desejar fazer modificações, clone este repositório em uma **máquina Windows** e certifique-se de ter o [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0) instalado.

```bash
# Clone o repositório
git clone https://github.com/leosgarcia/limpa-spooler.git

# Entre na pasta do projeto
cd limpa-spooler

# Compile o projeto para gerar um executável único de release
dotnet publish -c Release
```

O arquivo `LimpaSpooler.exe` estará disponível em `bin\Release\net8.0-windows\win-x64\publish\`.

## 📄 Licença

Este projeto é distribuído sob a licença MIT. Sinta-se à vontade para utilizá-lo, modificá-lo e distribuí-lo.
