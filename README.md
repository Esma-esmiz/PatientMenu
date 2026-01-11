# PatientMenu Configuration

## Project Architecture
This project uses a hybrid approach for data access to optimize for both performance and maintainability:

- **Dapper for Reads**: Utilized for read-heavy operations. Dapper is a lightweight micro-ORM that allows for raw SQL execution, providing maximum performance and minimized overhead when fetching data.
- **Entity Framework Core for Writes**: I Utilized for Create operations. EF Core provides robust change tracking and simplifies complex transaction management, ensuring data consistency with less boilerplate code.

## Build Configuration
The project is configured to exclude the test folder from the main build to prevent circular dependencies and referenced assembly issues.


