# iOS GPA Grade Predictor

An OpenAI-powered iOS application that predicts the grades you need to achieve a target GPA with up to 90% accuracy, backed by a .NET Web API and MySQL database.

## Table of Contents

* [Features](#features)
* [Tech Stack](#tech-stack)
* [Prerequisites](#prerequisites)
* [Getting Started](#getting-started)

  * [Clone the Repository](#clone-the-repository)
  * [Backend Setup](#backend-setup)
  * [Frontend Setup](#frontend-setup)
* [Configuration](#configuration)
* [Running the Application](#running-the-application)
* [API Endpoints](#api-endpoints)
* [Contributing](#contributing)

## Features

* **GPA Prediction**: Enter your current grades and desired GPA to see the grades you need in upcoming courses.
* **OpenAI® Insights**: Utilizes the OpenAI API to refine predictions based on historical grading patterns.
* **Accuracy Metrics**: Delivers predictions with approximately 90% accuracy, validated against sample datasets.
* **Grade History**: View past prediction requests and outcomes to track progress over time.
* **User Profiles**: Secure authentication allows students to save preferences and prediction history.

## Tech Stack

* **iOS Frontend**: Swift, SwiftUI, Combine
* **Backend API**: .NET 6 (ASP.NET Core Web API), Entity Framework Core
* **Database**: MySQL
* **AI Service**: OpenAI API
* **Dependency Management**: CocoaPods (iOS), NuGet (.NET)

## Prerequisites

* **Xcode** 13 or later
* **iOS** 14.0+ deployment target
* **.NET 6 SDK** or later
* **MySQL** 5.7+
* **CocoaPods** 1.11+

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/MadushanR/Hackathon2025.git
cd Hackathon2025
```

### Backend Setup

1. Navigate to the backend project:

   ```bash
   cd backend/
   ```
2. Configure database connection in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=GradePredictorDb;User=<username>;Password=<password>;"
     },
     "OpenAI": {
       "ApiKey": "YOUR_OPENAI_API_KEY"
     }
   }
   ```
3. Apply migrations and seed sample data:

   ```bash
   dotnet ef database update
   ```
4. Run the API:

   ```bash
   dotnet run
   ```

   The API will listen on `https://localhost:5001` by default.

### Frontend Setup

1. Install CocoaPods dependencies:

   ```bash
   cd ios/GpaPredictorApp
   pod install
   ```
2. Open the Xcode workspace:

   ```bash
   open GpaPredictorApp.xcworkspace
   ```
3. Configure the API base URL in `Networking/Config.swift`:

   ```swift
   struct API {
     static let baseURL = URL(string: "https://localhost:5001/api")!
   }
   ```
4. Build and run on a simulator or device (iOS 14+).

## Configuration

| File                               | Purpose                             |
| ---------------------------------- | ----------------------------------- |
| `backend/appsettings.json`         | Database, OpenAI API key, and ports |
| `ios/GpaPredictorApp/Config.swift` | Frontend API endpoint               |

## Running the Application

1. Ensure MySQL is running and the DB is migrated.
2. Start the .NET backend (`dotnet run`).
3. Launch the iOS app in Xcode and run on simulator or device.
4. Sign up or log in to begin forecasting grades!

## API Endpoints

> All routes are under `/api`

| Endpoint         | Method | Description                        |
| ---------------- | ------ | ---------------------------------- |
| `/auth/register` | POST   | Create a new student profile       |
| `/auth/login`    | POST   | Authenticate and receive JWT       |
| `/predict`       | POST   | Submit current grades & target GPA |
| `/history`       | GET    | Retrieve past prediction requests  |

> See `backend/README_API.md` for full documentation.

## Contributing

1. Fork the repo ⭐️
2. Create a feature branch (`git checkout -b feature/YourFeature`) 🔀
3. Commit your changes (`git commit -m "Add feature"`) 🛠️
4. Push to your branch (`git push origin feature/YourFeature`) 🚀
5. Open a Pull Request 🔎

---

*Built with 🚀 and 🤖 using Swift, .NET, and OpenAI.*
