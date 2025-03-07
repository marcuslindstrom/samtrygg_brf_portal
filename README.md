# Samtrygg BRF Portal

A web application for managing housing associations (BRF - Bostadsrättsförening) in Sweden. This portal provides tools for both BRF board members and residents to manage properties, rental applications, documents, and more.

## Features

- **BRF Board Member Portal**
  - Dashboard with key metrics
  - Property management
  - Rental application review and approval
  - Document management
  - Member management

- **Resident Portal**
  - Dashboard with important information
  - Application status tracking
  - Document submission and access
  - Profile management

## Technology Stack

- ASP.NET Core 8.0
- Entity Framework Core
- ASP.NET Core Identity for authentication
- Bootstrap for frontend styling
- C# for backend logic

## Project Structure

- **SamtryggBrfPortal.Core**: Contains domain entities, enums, and business logic
- **SamtryggBrfPortal.Infrastructure**: Contains data access, repositories, services, and view models
- **SamtryggBrfPortal.Web**: Contains controllers, views, and web-specific code
- **SamtryggBrfPortal.Tests**: Contains unit and integration tests

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository
   ```
   git clone https://github.com/marcuslindstrom/samtrygg_brf_portal.git
   ```

2. Navigate to the project directory
   ```
   cd samtrygg_brf_portal
   ```

3. Restore dependencies
   ```
   dotnet restore
   ```

4. Update the database connection string in `appsettings.json` if needed

5. Apply database migrations
   ```
   dotnet ef database update
   ```

6. Run the application
   ```
   dotnet run --project src/SamtryggBrfPortal.Web
   ```

## Development

- Use `dotnet watch run --project src/SamtryggBrfPortal.Web` for hot reload during development
- Run tests with `dotnet test`

## License

This project is proprietary and confidential.

## Contact

For more information, contact [Samtrygg](https://www.samtrygg.se/).
