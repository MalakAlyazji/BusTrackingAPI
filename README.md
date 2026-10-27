# 🚌 Bus Wait Coordinator API

A REST API that replaces chaotic "wait for us!" group-chat messages with a clear system:
students confirm they are coming to a specific bus trip, and the driver sees exactly who to wait for.

Built with **ASP.NET Core 8 Web API**, **Entity Framework Core** and **SQL Server**, using a layered architecture.
Enums are returned as readable strings (for example `"Waiting"`, `"University"`).

---

## The problem

Before each bus trip, students post messages in WhatsApp/Telegram groups asking the driver to wait.
The driver can't follow scattered messages while driving, so he either waits too long or leaves students behind.

## The solution

| Role | What they can do |
|---|---|
| **Student** | See today's upcoming trips, confirm "I'm coming", cancel the confirmation |
| **Driver** | Start waiting at the stop, see who confirmed, mark departure, complete the trip |
| **Admin** | Manage routes, trips, drivers and admins |

---

## Architecture

```
BusTrackingAPI   → Controllers, Swagger, dependency injection        (Presentation)
BusTrackingBL    → Services, DTOs, Mappers, Result<T> pattern         (Business logic)
BusTrackingDL    → Entities, EF Core configurations, DbContext,       (Data access)
                   migrations
```

- **Result pattern:** every service returns `Result<T>` (success + data, or error message + HTTP code), so controllers stay thin and errors are consistent (`ProblemDetails`).
- **DTOs and mappers:** entities are never exposed directly. Request and response models are separate.
- **Fluent API configuration:** one `IEntityTypeConfiguration` class per entity (keys, lengths, unique indexes, delete behaviour).
- **Password hashing:** admin and driver passwords are stored with ASP.NET Core Identity's `PasswordHasher`, with login and change-password flows.
- **Background service:** a hosted `TripBackgroundService` runs every minute and automatically departs or cancels trips whose waiting window has ended.
- **Soft delete:** admins and drivers are deactivated rather than deleted, and can be re-activated.

## Business rules

- Trips follow a status flow: `Scheduled → Waiting → Departed → Completed` (or `Cancelled`).
- A driver can start waiting from **30 minutes before** the trip time, and only on the trip's day.
- A trip cannot depart before its scheduled time, or while no students have confirmed.
- Students can confirm only within the confirmation window, and only once per trip.
- A trip with confirmed students cannot be cancelled while the driver is waiting.
- A driver cannot have two trips at the same day and time (enforced by a unique index).
- When a trip completes and the next trip is 60+ minutes away, a return trip is created automatically.
- After the waiting window ends, the background service departs the trip if students confirmed, or cancels it if nobody did.

## Data model

`Admin` · `Driver` · `Route` · `Trip` · `Student` · `StudentConfirmation`

A `Trip` belongs to a `Route` and a `Driver`. A `StudentConfirmation` links a `Student` to a `Trip`.

---

## Getting started

**Requirements:** .NET 8 SDK, SQL Server (Express is fine).

1. Clone the repository.
2. Check the connection string in `BusTrackingAPI/appsettings.json`:
   ```json
   "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BusTrackingDB;Trusted_Connection=True;TrustServerCertificate=True;"
   ```
3. Create the database:
   ```bash
   dotnet ef database update --project BusTrackingDL --startup-project BusTrackingAPI
   ```
4. Run the API:
   ```bash
   dotnet run --project BusTrackingAPI
   ```
5. Open Swagger at `http://localhost:5105/swagger`.

## Main endpoints

| Area | Endpoints |
|---|---|
| Trips | `AddNewTrip`, `GetAllComingTrips`, `StartWaiting/{id}`, `StartDeparting/{id}`, `CompleteTrip`, `CancelTrip`, `GetDriverTrips/{id}` |
| Confirmations | `CreateConfirmStudent`, `CancelConfirmation`, `CancelConfirmationById/{id}`, `GetConfirmations/{tripId}` |
| Students | `CreateNewStudent`, `GetAllStudents`, `GetStudentById/{id}`, `UpdateStudentById/{id}` |
| Drivers | `AddDriver`, `LoginDriver`, `ChangeDriverPassword`, `GetActiveDrivers`, `GetDriverById/{id}`, `UpdateDriver/{id}`, `ActivateDriver/{id}`, `DeactivateDriver/{id}` |
| Routes | `CreateRoute`, `GetAllRoutes`, `GetRouteById/{id}`, `UpdateRoute/{id}`, `DeleteRoute/{id}` |
| Admins | `CreateNewAdmin`, `LoginAdmin`, `ChangeAdminPassword`, `GetAllAdmins`, `GetActiveAdmins`, `GetAdminById/{id}`, `updateAdmin/{id}`, `ActivateAdmin`, `DeleteAdmin/{id}` |

All routes are under `/api/{Controller}/…`. Full details are in Swagger.

---

## Roadmap

- [ ] JWT authentication and role-based authorization (Student / Driver / Admin)
- [ ] Real-time updates for the driver with **SignalR**
- [ ] Web front end for students and drivers
- [ ] Unit tests for the service layer
