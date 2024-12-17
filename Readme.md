
# Project Management System

## 📖 Overview

The **Project Management System** is a console application developed in **C#** designed to help manage classrooms. It allows you to organize information about students, teachers, assessments and attendences, leveraging **SQLite** for persistent data storage. The project emphasizes simplicity and learning.

## 🛠️ External Dependency
The project uses the following external dependency:

- [System.Data.SQLite (v1.0.119)](https://www.nuget.org/packages/System.Data.SQLite/)

## 🚀 Local Setup
Follow the instructions below to set up the project locally.

### 1. Prerequisites
- **.NET SDK** (recommended version: 6.0).
- **Visual Studio** (optional, if you prefer running the project in an IDE).
- **SQLite Database** (automatically configured by the project).

### 2. Clone the repository
Clone the repository to your local machine:
```bash
git clone https://github.com/IvanFrezzaJr/atu-project-management-system.git
cd atu-project-management-system/ProjectManagementSystem
```

### 3. Restore dependencies

Run the following command in the project directory to restore dependencies:
```bash
dotnet restore
```

### 4. Initial Configuration
The SQLite configuration will be set in the first time any user runs it.

#### 🏃 How to Run
##### Run via Visual Studio
1. Open the project in Visual Studio.
2. Build the project (Ctrl + Shift + B).
3. Run (F5).

##### Run via Terminal

1. Navigate to the project directory and build it:
```bash
dotnet build -c Release
```

2. The generated binary will be available in the bin\Release\net6.0 folder:
```bash
cd bin\Release\net6.0
```

3. Execute the binary directly:
- Windows:
```bash
ProjectManagementSystem.exe
```
   
- Linux/MacOS:
```bash
./ProjectManagementSystem
```

##### Example Usage
When the program starts, you will be guided through interactive menus in the terminal to manage your classrooms. 

#### 🏃 How to Run the Tests
##### Run via Visual Studio
1. Open the project in Visual Studio.
2. Select the menu `Test -> Run all tests` or  (Ctrl + R, A).

##### Run via Terminal
```bash
dotnet test
````


#### 📂 Project Structure

```
ProjectManagementSystem/
├── Program.cs		# Entry point of the application
├── Schema.cs		# Schemas to convert data
├── Helpers.cs		# helpers (ReadPassword)
├── Database/		# Database queries
├── Models/			# Data models (Students, Teachers, etc.)
├── Controller/		# Business logic and database access
├── Views/			# View logic
├── Core/		  	# System common classes (Interfaces, Menus, Subscribers)

```

#### 📑 Additional Notes
- The application automatically creates the database without requiring any configuration. If the database needs to be reset, it must be deleted.
- For any issues or errors, refer to the System.Data.SQLite documentation for troubleshooting tips.
