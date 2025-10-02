# Hackathon2025

This is the backend for the Hackathon 2025 project built with **ASP.NET Core** and **Entity Framework Core**.

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQLite (installed with .NET EF by default) or any database you configure
- [OpenAI API Key](https://platform.openai.com/)

### 📂 Project Structure
```
Backend/
  HackathonProject/
    Controllers/
    Models/
    DataContext.cs
    Program.cs
    appsettings.json (generated at runtime or local only)
```

### ⚙️ Configuration

Secrets such as API keys **must not** be stored in source control.  
We use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) in development and environment variables in production.

#### Add OpenAI API Key
```bash
cd Backend/HackathonProject
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "your-api-key-here"
```

This will store the key **outside** your repo (safe from GitHub scanning).

#### Example `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> ⚠️ Notice: No API keys are stored here. They come from secrets or environment variables.

### ▶️ Run the Project
```bash
cd Backend/HackathonProject
dotnet build
dotnet ef database update   # create database schema
dotnet run
```

API will be available at:
```
https://localhost:5001/api/plan
http://localhost:5000/api/plan
```

### 🔑 GitHub & Secrets
- API keys will **never** be pushed to GitHub (blocked by push protection).
- Use `git rm --cached <file>` to remove sensitive files from history.
- If you accidentally pushed a secret, rotate it immediately from the OpenAI dashboard.

---

## 🤝 Contributing
1. Clone the repository
2. Create a feature branch
3. Commit changes (no secrets in code)
4. Push to your fork
5. Open a Pull Request 🎉

---

## 📜 License
MIT License © 2025