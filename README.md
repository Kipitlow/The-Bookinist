# 🚀 Git Workflow & PR Pipeline

![Git](https://img.shields.io/badge/Git-Workflow-orange)
![PR](https://img.shields.io/badge/Pull_Request-Driven-blue)
![CI](https://img.shields.io/badge/CI-Pipeline-green)
![QA](https://img.shields.io/badge/QA-Required-red)

📘 Documentation interne du workflow Git  
⚙️ GitHub Desktop • Review • CI • QA • Production  

> 🧠 *Clean commits, clean pipeline, clean production.*

---

## 📚 Sommaire

- 🌿 [Branching Strategy](#-1-branching-strategy)
- 🧾 [Naming Convention](#-naming-convention)
- 🔄 [Clean Git Setup](#-2-clean-git-setup)
- 🚀 [Pull Request Pipeline](#-3-pull-request-pipeline)
- 📝 [Contenu d’une PR](#-contenu-obligatoire-dune-pr)
- 📊 [Pipeline visuelle](#-4-pipeline-visuelle)
- 🧪 [GitHub Actions](#-5-github-actions-ci-suggestion)
- 🚀 [Résultat attendu](#-résultat-attendu)
- 🧩 [Pipeline final](#-6-pipeline-final)

---

## 🌿 1. Branching Strategy

### 📌 Branches principales

| Branche      | Rôle                         |
|--------------|------------------------------|
| `main`       | Production                   |
| `Acceptance` | Validation QA + intégration  |
| `feature/*`  | Développement                |
| `fix/*`      | Correction de bug            |
| `hotfix/*`   | Correction critique prod     |

---

## 🧾 Naming Convention

### 🚀 Features
- `feature/login-system`
- `feature/payment-integration`

### 🐛 Fixes
- `fix/login-error`
- `fix/null-pointer-dashboard`

### 🚨 Hotfix
- `hotfix/security-patch`

---

## 🔄 2. Clean Git Setup

### 🧹 Activer le prune automatique
```bash
git config --global fetch.prune true
```

### Sync propre :
bash
git fetch --prune
## 🚀 3. Pull Request Pipeline : ### 🔀 Étape obligatoire avant PR :
bash
git checkout feature/xxx
bash
git merge origin/Acceptance
### 👉 Objectif : - Résoudre conflits localement - Partir d’une base stable - 📝 PR obligatoire ### Contenu de la PR : - 📁 fichiers modifiés - 🧠 description fonctionnelle - 🧪 tests à effectuer - 👀 Review + QA flow ### ⚠️ Règles - Code review obligatoire - QA validation (avant / pendant / après review possible) - Merge uniquement si validé ### ❌ Si PR refusée : - Commentaire obligatoire - Conversion en Draft PR - Corrections requises ## 📊 4. Pipeline visuelle (Mermaid) : <img width="1167" height="2553" alt="mermaid-diagram" src="https://github.com/user-attachments/assets/90a5d377-c495-47d7-9d0b-9f9d2a92b19a" /> ## 🧪 5. GitHub Actions (CI suggestion) : ### Exemple simple de CI :
bash
name: CI Pipeline

on:
  pull_request:
    branches:
      - Acceptance

jobs:
  test:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Install dependencies
        run: npm install

      - name: Run tests
        run: npm test
### 🚀 Résultat attendu : Ce workflow garantit : - Code propre - Intégration sans conflit - QA contrôlée - Historique Git lisible - Zéro surprise en production ## 🧩 6. Pipeline final réaliste : feature → PR → CI runs → review → QA → manual merge → Acceptance
