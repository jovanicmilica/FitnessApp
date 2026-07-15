# Fitness Mentoring Application

A complete fitness mentoring platform built with **Avalonia UI** and **.NET**. This application connects clients with personal trainers, allowing them to manage workouts, track progress, and provide feedback.

---

## Features

### Authentication & User Management
- **Client Registration** – Create a client account with health profile
- **Trainer Registration** – Create a trainer account with qualifications (requires admin approval)
- **Login/Logout** – Secure authentication for all roles (Client, Trainer, Admin)

### Admin Features
- **Trainer Registration Requests** – Approve or reject pending trainer registrations
- **Remove Trainers** – Remove existing trainers from the system

### Client Features
- **Search Trainers** – Find trainers by name, view ratings and monthly prices
- **Send Tutelage Requests** – Submit requests with goals, preferences, and health information
- **View & Pay for Tutelages** – Track active tutelages and extend them with payments
- **My Workouts** – View assigned workouts with exercise details
- **Rate Trainer** – Leave feedback for your trainer (rating + comment)
- **Rate Workout** – Rate completed workouts
- **Rate Individual Exercise** – Rate specific exercises within a workout
- **View My Feedbacks** – See all your submitted feedback with readable names

### Trainer Features
- **Tutelage Requests** – Approve or reject client requests, view questionnaire details
- **Create Workouts** – Build custom workouts for clients with selected exercises
- **View My Workouts** – See all workouts you've created
- **View Client Feedback** – See ratings and comments from clients
- **Subscription Management** – Pay monthly subscription to stay active
- **CRUD for Equipment** – Add, edit, delete exercise equipment
- **CRUD for Props** – Add, edit, delete props/accessories
- **CRUD for Exercises** – Add, edit, delete exercises with descriptions and video URLs

### Profile Management
- **Update Profile** – Edit personal information (name, email, password)
- **Health Information** – Add/remove health conditions and disabilities (clients)
- **Professional Information** – Manage price, qualifications, and status (trainers)

---

## Technology Stack

| Technology | Purpose |
|------------|---------|
| **.NET 8.0** | Application framework |
| **Avalonia UI** | Cross-platform desktop UI |
| **CommunityToolkit.MVVM** | MVVM pattern with ObservableProperty and RelayCommand |
| **System.Text.Json** | JSON serialization for data persistence |
| **Repository Pattern** | Data access abstraction |

---

## Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with Avalonia extension OR
- [JetBrains Rider](https://www.jetbrains.com/rider/) OR
- Any code editor with .NET support

### Clone & Build

```bash
# Clone the repository
git clone https://github.com/jovanicmilica/FitnessApp.git
cd FitnessApp

# Build the project
dotnet build

# Run the application
dotnet run
