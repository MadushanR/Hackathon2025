# 📚 GPA & Study Plan Assistant (React Native + .NET + OpenAI)

An AI-powered **React Native mobile application** that helps students predict grades needed to reach a target GPA and generates **personalized study plans**.  
Backed by a **.NET Web API**, **SQLite database**, and **OpenAI integration**, the app empowers students to set goals, receive tailored advice, and manage their academic journey.

---

## 📑 Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Clone the Repository](#clone-the-repository)
  - [Backend Setup](#backend-setup)
  - [Frontend (React Native) Setup](#frontend-react-native-setup)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)

---

## 🚀 Features

- **🎯 GPA Prediction**: Input current grades + desired GPA → see grades needed.  
- **🤖 AI-Powered Study Plans**: GPT generates personalized study recommendations, schedules, and study methods.  
- **📊 Accuracy Metrics**: Predictions validated with ~90% accuracy.  
- **📝 Grade & Study History**: Track all prediction requests + study plans.  
- **👤 User Profiles**: Authentication + saved preferences.  
- **📘 Study Plan Dashboard**: New section to view & manage study plans.  

---

## 🏗️ Architecture

```
React Native App (Expo)
   |
   |--> API Requests (Axios / Fetch)
   |
.NET 6 Web API (ASP.NET Core)
   |
   |--> Entity Framework Core
   |--> OpenAI API (Study Plan, GPA Prediction)
   |
MySQL Database
```

---

## 💻 Tech Stack

- **Frontend**: React Native (Expo, TypeScript/JavaScript), Axios  
- **Backend**: .NET 8 (ASP.NET Core Web API)  
- **Database**: SQLite  
- **AI Service**: OpenAI API  
- **State Management**: React Context  
- **Auth**: JWT-based  

---

## ⚙️ Prerequisites

- **Node.js** 16+  
- **React Native CLI or Expo**  
- **.NET 8 SDK** or later  
- **SQLite** 6+  
- **OpenAI API Key**  

---

## 🔧 Getting Started

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/MadushanR/Hackathon2025.git
cd Hackathon2025
```

### 2️⃣ Backend Setup
```bash
cd Backend/
```

1. Create `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=GradePredictorDb;User=<username>;Password=<password>;"
     },
     "OpenAI": {
       "ApiKey": "YOUR_OPENAI_API_KEY"
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

2. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

3. Run the API:
   ```bash
   dotnet run
   ```
   API runs at: `https://localhost:5001`

---

### 3️⃣ Frontend (React Native) Setup
```bash
cd frontend/
npm install
```

1. Configure API URL in `config.js`:
   ```js
   export const API_BASE_URL = "http://localhost:5001/api";
   ```

   ⚠️ When testing on device/emulator: replace `localhost` with your machine IP (e.g. `http://192.168.1.10:5001/api`).

2. Start app with Expo:
   ```bash
   npx expo start
   ```
   or if not using Expo:
   ```bash
   npx react-native run-android
   npx react-native run-ios
   ```

---

## ⚙️ Configuration

| File                           | Purpose                               |
| ------------------------------ | ------------------------------------- |
| `Backend/appsettings.json`     | Database, OpenAI API key, logging     |
| `frontend/config.js`           | API endpoint for React Native app     |

---

## ▶️ Running the Application

1. Ensure MySQL is running + migrations applied.  
2. Start backend API (`dotnet run`).  
3. Start React Native app (`npx expo start`).  
4. Log in → Access GPA Predictor + Study Plan dashboard.  

---

## 🌐 API Endpoints

Base URL: `/api`

### Auth
- `POST /auth/register` → Register new student  
- `POST /auth/login` → Login, receive JWT  

### GPA Prediction
- `POST /predict` → Submit grades + target GPA  

### History
- `GET /history` → Retrieve past GPA predictions  

### Study Plan (NEW)
- `POST /studyplan/{studentId}` → Save GPT study plan  
- `GET /studyplan/{studentId}` → Fetch saved study plans  

---


💡 *Built with React Native, .NET, SQLite, and OpenAI for smarter student success!*
