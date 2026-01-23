# Como Enviar para o GitHub

## PASSO 1: Criar o Repositório no GitHub

1. Acesse: **https://github.com/new**
2. Faça login com `mrivanlima` / `mrivanlima@gmail.com`
3. Preencha:
   - **Repository name**: `escola-platform` (ou outro nome de sua escolha)
   - **Description**: `K-12 Educational Platform - PostgreSQL Database`
   - Escolha: **Public** ou **Private**
   - ⚠️ **NÃO marque**: "Add a README file"
   - ⚠️ **NÃO marque**: "Add .gitignore"
   - ⚠️ **NÃO marque**: "Choose a license"
4. Clique em **"Create repository"**

## PASSO 2: Conectar e Enviar

Depois de criar o repositório no GitHub, execute estes comandos no PowerShell:

```powershell
# Adicionar o remote (substitua NOME_DO_REPOSITORIO pelo nome que você escolheu)
git remote add origin https://github.com/mrivanlima/NOME_DO_REPOSITORIO.git

# Enviar para o GitHub
git push -u origin main
```

**Exemplo se você chamar o repositório de "escola-platform":**
```powershell
git remote add origin https://github.com/mrivanlima/escola-platform.git
git push -u origin main
```

## PASSO 3: Autenticação

Quando executar `git push`, o GitHub vai pedir autenticação:

### Opção 1: Personal Access Token (Recomendado)
1. Vá em: https://github.com/settings/tokens
2. Clique em **"Generate new token"** → **"Generate new token (classic)"**
3. Dê um nome: "Escola Platform"
4. Marque o escopo: **repo** (todas as opções)
5. Clique em **"Generate token"**
6. **COPIE O TOKEN** (você só verá uma vez!)
7. Quando o git pedir senha, cole o token

### Opção 2: GitHub CLI
```powershell
# Instalar GitHub CLI
winget install --id GitHub.cli

# Fazer login
gh auth login

# Depois executar:
git push -u origin main
```

## Verificação

Após o push bem-sucedido, acesse:
```
https://github.com/mrivanlima/NOME_DO_REPOSITORIO
```

Você verá todos os 35 arquivos online! 🎉

## Comandos Já Configurados

✅ Nome: `mrivanlima`
✅ Email: `mrivanlima@gmail.com`
✅ Branch: `main`
✅ Commit inicial: Feito

## Próximas Atualizações

Depois do primeiro push, para enviar mudanças:

```powershell
git add .
git commit -m "Descrição da mudança"
git push
```

## Problema com Autenticação?

Se der erro de senha/token, você pode usar SSH:

1. Gerar chave SSH:
```powershell
ssh-keygen -t ed25519 -C "mrivanlima@gmail.com"
```

2. Adicionar no GitHub:
   - Copie o conteúdo de: `~/.ssh/id_ed25519.pub`
   - Vá em: https://github.com/settings/keys
   - Clique em "New SSH key"
   - Cole a chave pública

3. Mudar o remote para SSH:
```powershell
git remote set-url origin git@github.com:mrivanlima/NOME_DO_REPOSITORIO.git
git push -u origin main
```
