# 🚴 Article Management Dashboard

An internal tool for managing bicycle component articles — built with Angular 18, .NET 8 Web API, and Clean Architecture.

---

## 🧰 Tech Stack

### Frontend
- Angular 18 (standalone)
- Angular Material & Tailwind CSS
- NgRx for state management

### Backend
- .NET 8 Web API
- EF Core with SQL Server (code-first)
- Serilog for logging
- CQRS (without MediatR)
- Swagger for API documentation

---

## ⚙️ Setup Instructions

### 🔧 Prerequisites

- Node.js (v18+)
- .NET 8 SDK
- SQL Server (local or Docker)

---

### 🔁 Setup Backend

```bash
cd backend/ArticleDashboard.API
dotnet restore
dotnet ef database update
dotnet run
```

- Swagger UI: [https://localhost:7023/swagger](https://localhost:7023/swagger)

✅ API will run at: `https://localhost:7023`

---

### 🖼️ Setup Frontend

```bash
cd frontend/article-dashboard-app
npm install
npm start
```

✅ Angular app will run at: `http://localhost:4200`

---

### 🐧 Run Both Projects on Linux

```bash
chmod +x run.sh
./run.sh
```

---

## ✅ Assumptions

- Enums like `ArticleCategory`, `Material`, and `BicycleCategory` are mapped consistently across frontend and backend
- The backend handles all filtering/sorting/pagination
- SQL Server is assumed to be available locally on default port

---

## 📁 Folder Structure

```
ArticleDashboard/
├── backend/
│   └── ArticleDashboard.API/
├── frontend/
│   └── article-dashboard-app/
├── run.sh
└── README.md
```

---

## 🧪 Testing

- Backend: xUnit unit and integration tests
- Frontend: Angular unit tests (Jest or Karma)

```bash
cd backend/tests/...
dotnet test

cd frontend/article-dashboard-app
ng test
```

---

## 📫 Contact

Made with ❤️ by Mohsen MOjabi · GitHub: [@popcom]
