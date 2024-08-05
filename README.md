## Setup Instructions

### 1. Clone the Repository
### 2. Run the Application
[http://localhost:7220/swagger](https://localhost:7220/swagger/index.html)
## Project Structure

- **Application**: Contains the business logic and interfaces.
- **Domain**: Contains the domain entities.
- **Presistence**: Contains the implementation of the interfaces and database context.
- **TaskManagementSolution**: The main entry point of the application, including middleware and configuration.

## Logging

The application uses Serilog for logging. Logs are configured in `Program.cs`:

