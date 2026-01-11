# PatientMenu Microservice

## Overview
The **PatientMenu** microservice is designed to manage patient-specific menu items in a hospital setting. It is built to be **high-performance, multi-tenant aware**, and easily maintainable. This microservice demonstrates a **hybrid data access approach**, combining Entity Framework Core for writes and Dapper for high-performance reads.

This project was built as part of the **Computrition Weekend Challenge**, simulating a real-world migration of a monolithic hospital application to cloud-native microservices.

---

## Architecture

The project follows a **clean architecture** approach:

- **Controllers → Services → Repositories → DbContext**
- **Multi-tenancy**: Each request carries a `TenantId` in the header, ensuring data isolation.
- **Caching**: Frequently accessed read operations are cached in-memory to reduce database load.
- **Hybrid ORM**:
  - **Dapper** for read-heavy endpoints (like `AllowedMenuItem`) to ensure raw SQL performance.
  - **Entity Framework Core** for writes and standard CRUD operations.

---

## Endpoints

### Menu Item CRUD
- `POST /api/menu-items`  
  - Adds a new menu item.  
  - Uses **EF Core** for transactional writes and change tracking.  

- `GET /api/menu-items`  
  - Returns all menu items for the current tenant.  
  - Uses in-memory caching for faster repeated reads.  

### Allowed Menu
- `GET /api/patients/{patientId}/allowed-menu`  
  - Returns menu items a patient is allowed to eat based on their `DietaryRestrictionCode`.  
  - Logic:
    - `GF` (Gluten Free) → return items where `IsGlutenFree == true`
    - `SF` (Sugar Free) → return items where `IsSugarFree == true`
    - `NONE` → return all items
  - Uses **Dapper** for **raw SQL execution**, ensuring high throughput for bedside tablets.

---

## Tools and Why They Are Used

### Data Access
- **Dapper**: Used for the `AllowedMenuItem` endpoint to **maximize read performance** and allow direct SQL filtering by dietary restrictions and tenant.
- **Entity Framework Core**: Used for create/update operations to simplify **transaction management and change tracking**.

### Caching
- **IMemoryCache**: Caches tenant-specific menu and patient queries to improve performance and reduce DB load.

### Multi-Tenancy
- Each request requires an `X-Tenant-Id` header.
- Tenant isolation is enforced at the service/repository layer.
- Middleware validates the tenant header and ensures it exists in the database.

### Testing
- **xUnit**: Unit testing framework.
- **Moq**: Mock dependencies (repositories, services) for isolated unit tests.  
- **FluentAssertions**: Makes test assertions readable and expressive.  
- **coverlet.collector**: Collects code coverage metrics.  
- **Microsoft.NET.Test.Sdk**: Enables test building and execution in .NET projects.

### Other Tools
- **Swagger**: Interactive API documentation and testing.
- **SQL Server**: Database backend for development and testing.
- **AutoMapper**: Maps between DTOs and domain entities to reduce boilerplate code.

---

## Performance and Optimization

- **Dapper for high-volume reads**: Raw SQL queries minimize overhead.  
- **Indexes**: Tenant-based indexes on `MenuItems` and `Patients` optimize query performance.  
- **In-memory caching**: Reduces database round-trips for repeated reads.  
- **Tenant isolation**: Ensures queries are always filtered by `TenantId`.

---

## Project Structure

PatientMenu/
├── Controllers/ # API endpoints
├── Services/ # Business logic
├── Repositories/ # Data access (EF/Dapper)
├── Data/ # DbContext, models, migrations
├── Middleware/ # Tenant middleware, request filters
├── Tests/ # Unit tests using xUnit and Moq
├── Program.cs # Application startup
├── README.md

