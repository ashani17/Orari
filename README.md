
# 📅 Orari - University Academic and Exam Scheduling System

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Status](https://img.shields.io/badge/status-active-brightgreen.svg)]()
[![Build](https://img.shields.io/github/actions/workflow/status/ashani17/Orari/dotnet.yml?branch=main)](https://github.com/ashani17/Orari/actions)

> A modular and extensible academic scheduling platform designed for universities and faculties to manage teaching timetables, exam schedules, and professor-course allocations.

---

## 🚀 Features

- 📚 Manage academic programs and syllabuses
- 👩‍🏫 Assign professors to courses
- 🕒 Auto-generate course and exam schedules
- 🏛️ Multi-faculty and multi-department support
- 🗓️ Define constraints (room availability, course dependencies, etc.)
- 📄 Export schedules to PDF/DOCX
- 🔒 Identity-based secure access for admin and professors
- 🌐 RESTful API powered by ASP.NET Core 8
- 🐘 SQL Server backend with EF Core
- 🐳 Dockerized for easy deployment

---

## 🏗️ Architecture

This project follows a clean architecture pattern with:

- `Domain`: Entities and business rules
- `Application`: CQRS handlers (MediatR), validation, and DTOs
- `Infrastructure`: EF Core, repository implementations, and service integrations
- `API`: REST controllers, authentication, and Swagger UI

### Tech Stack

| Layer        | Tech                               |
|-------------|------------------------------------|
| Backend API | ASP.NET Core 8, MediatR, FluentValidation |
| DB Access   | Entity Framework Core + SQL Server |
| Auth        | ASP.NET Identity + JWT             |
| Frontend    | (To be integrated) React / Razor   |
| DevOps      | Docker, GitHub Actions             |

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Docker](https://www.docker.com/) *(optional for container deployment)*

### Run Locally

```bash
git clone https://github.com/ashani17/Orari.git
cd Orari
dotnet restore
dotnet ef database update --project Syllabus.Infrastructure
dotnet run --project Syllabus.API
```

API will be accessible at: `https://localhost:5001`

---

## 🔧 Configuration

Update `appsettings.json` to configure:

- Database connection string
- JWT token parameters
- Swagger/OpenAPI settings

---

## 📦 Docker Support

```bash
docker-compose build
docker-compose up
```

---

## 🧪 Testing

Unit and integration tests (coming soon). Planned to include:

- Course scheduling logic
- CQRS pipeline behaviors
- API endpoints with mocked DB context

---

## 📁 Project Structure

```
/Syllabus.API             → Web API
/Syllabus.Domain          → Domain models
/Syllabus.Application     → CQRS handlers, validators
/Syllabus.Infrastructure  → EF Core, DB context, repositories
```

---

## 🛡️ Security

- Role-based access control
- JWT authentication
- Input validation via FluentValidation

---

## 📘 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

Pull requests and forks are welcome! For major changes, please open an issue first to discuss what you’d like to change.

---

## 👨‍🎓 Author

Developed by [@ashani17](https://github.com/ashani17) — as part of a university thesis project for academic schedule generation and management.
