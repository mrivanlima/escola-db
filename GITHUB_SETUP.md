# GitHub Setup Instructions

## Quick Setup

1. **Create a new repository on GitHub**:
   - Go to https://github.com/new
   - Repository name: `escola-platform` (or your preferred name)
   - Description: "K-12 Educational Platform with PostgreSQL database for Brazilian Portuguese market"
   - Choose Public or Private
   - **Do NOT initialize with README, .gitignore, or license** (we already have them)

2. **Connect your local repository to GitHub**:
   ```powershell
   # Replace YOUR_USERNAME with your GitHub username
   # Replace REPOSITORY_NAME with your repository name
   git remote add origin https://github.com/YOUR_USERNAME/REPOSITORY_NAME.git
   
   # Set the default branch name to main (GitHub standard)
   git branch -M main
   
   # Push your code to GitHub
   git push -u origin main
   ```

3. **Verify**:
   - Visit your repository on GitHub
   - You should see all files uploaded

## Alternative: Using SSH

If you prefer SSH authentication:

```powershell
# Replace with your GitHub username and repository name
git remote add origin git@github.com:YOUR_USERNAME/REPOSITORY_NAME.git
git branch -M main
git push -u origin main
```

## What's Included in This Repository

✅ Complete database schema (17 tables, 6 schemas)
✅ Build scripts (quick_build.ps1, build_db.ps1)
✅ Text normalization for Brazilian Portuguese
✅ Documentation (README.md, ARCHITECTURE.md, DB_GENERATION_RULES.md)
✅ Database diagram (PDF + Mermaid + DOT formats)
✅ Seed data for testing
✅ .gitignore configured (excludes credentials and temporary files)

## What's NOT Included (Protected by .gitignore)

❌ db_config.ps1 (database credentials)
❌ .env files
❌ Python virtual environment (.venv/)
❌ __pycache__ and temporary files

## After Pushing to GitHub

Consider adding:
- [ ] GitHub Actions for CI/CD
- [ ] Issue templates
- [ ] Pull request templates
- [ ] CONTRIBUTING.md
- [ ] LICENSE file
- [ ] Wiki with detailed documentation

## Collaborating

To allow others to contribute:
1. Go to repository Settings → Collaborators
2. Add team members
3. They can clone with:
   ```powershell
   git clone https://github.com/YOUR_USERNAME/REPOSITORY_NAME.git
   cd REPOSITORY_NAME
   ```

## Updating Your Repository

After making changes:
```powershell
git add .
git commit -m "Description of changes"
git push
```

## Need Help?

GitHub Docs: https://docs.github.com/en/get-started
Git Basics: https://git-scm.com/book/en/v2/Getting-Started-Git-Basics
