# 🚀 Git Workflow & PR Pipeline

📘 Internal Git workflow documentation
⚙️ GitHub Desktop • Review • CI • QA • Production
> 🧠 *Clean commits, clean pipeline, clean production.*

---

## 📚 Table of Contents

- 🌿 [Branching Strategy](#-1-branching-strategy)
- 🧾 [Naming Convention](#-naming-convention)
- 🔄 [Clean Git Setup](#-2-clean-git-setup)
- 🚀 [Pull Request Pipeline](#-3-pull-request-pipeline)
- 📝 [PR Content](#pr-content)
- 📊 [Pipeline Diagram](#-4-pipeline-diagram-mermaid)
- 🧪 [GitHub Actions](#-5-github-actions-ci-suggestion)
- 🚀 [Expected Result](#-expected-result)
- 🧩 [Final Pipeline](#-6-final-pipeline)

---

## 🌿 1. Branching Strategy

### 📌 Main branches

| Branch       | Role                        |
| ------------ | --------------------------- |
| `main`       | Production                  |
| `Acceptance` | QA validation + integration |
| `feature/*`  | Development                 |
| `fix/*`      | Bug fix                     |
| `hotfix/*`   | Critical prod fix           |

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

### 🧹 Enable automatic prune

```
git config --global fetch.prune true
```

### Clean sync:

```
git fetch --prune
```

## 🚀 3. Pull Request Pipeline

### 🔀 Mandatory step before a PR:

```
git checkout feature/xxx
```

```
git merge origin/Acceptance
```

### 👉 Goal:

- Resolve conflicts locally
- Start from a stable base
- 📝 PR required

### PR Content:

- 📁 Modified files
- 🧠 Functional description
- 🧪 Tests to perform
- 👀 Review + QA flow

### ⚠️ Rules:

- Code review required
- QA validation (before / during / after review is possible)
- Merge only once validated

### ❌ If a PR is rejected:

- Comment required
- Convert to Draft PR
- Fixes required

## 📊 4. Pipeline Diagram (Mermaid)

![mermaid-diagram](https://github.com/user-attachments/assets/90a5d377-c495-47d7-9d0b-9f9d2a92b19a)

## 🧪 5. GitHub Actions (CI suggestion)

### Simple CI example:

```
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
```

### 🚀 Expected Result:

This workflow guarantees:

- Clean code
- Conflict-free integration
- Controlled QA
- Readable Git history
- Zero surprises in production

## 🧩 6. Final Pipeline

feature → PR → CI runs → review → QA → manual merge → Acceptance
