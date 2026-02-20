# CareBridge

A clinical decision support (CDS) mini-service that identifies patients overdue for screenings (e.g., colonoscopies) and provides a clinician dashboard.

> [!CAUTION]
> **Active Development:** This project is evolving rapidly. Expect breaking changes and frequent updates.

### Demo

https://github.com/user-attachments/assets/9b2fcd24-3b21-49c9-9122-adaa392e5ca1

### Tech Stack

- **Backend:** ASP.NET Core 10 Web API (RESTful).
- **Frontend:** Angular 21.

### Dependencies

- dotnet v10 (_required_)
- node v25 (_required_)
- angular v21 (_required_)
- [mise](https://mise.jdx.dev/getting-started.html) (_recommended_)
- make (_recommended_)

> [!NOTE]
> mise will handle the other required dependencies (excluding make) if
> installed. Simply run `mise install` in the root of the local clone.

### Quickstart

- Clone repository.
- Start the backend and the frontend either by:
  - Running `ng serve` in ui directory and `dotnet run` in api directory.
  - Running `make` in the the root directory.
    - To clean run `make clean`.
- Go to http://localhost:4200/ to view dashboard.
