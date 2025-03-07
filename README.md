# Samtrygg BRF Portal

A comprehensive platform for managing subletting in housing associations (BRFs), built with ASP.NET Core 8 and Bootstrap 5.3.

## Project Overview

The Samtrygg BRF Portal provides a digital solution for housing associations to handle the process of apartment subletting. It connects three main user groups:

1. **BRF Board Members** - Housing association board members who need to approve subletting applications
2. **Property Owners** - Apartment owners who want to sublet their apartments
3. **Tenants** - People who are looking to rent apartments

The platform streamlines the entire process from application to contract signing, with features for background checks, document management, and communication.

## Technology Stack

### Backend
- **.NET 8** - Latest version of Microsoft's development platform
- **ASP.NET Core MVC** - Web application framework
- **Entity Framework Core** - ORM for database access
- **Identity Framework** - Authentication and authorization
- **SQL Server** - Database

### Frontend
- **Bootstrap 5.3** - Latest version of the popular CSS framework
- **JavaScript/TypeScript** - For interactive features
- **Chart.js** - For dashboard visualizations

### Integration
- **BankID** - Swedish digital identification system for secure signing
- **Azure Blob Storage** - For document storage

## Project Structure

The solution follows a clean architecture approach with separation of concerns:

- **SamtryggBrfPortal.Web** - User interface and controllers
- **SamtryggBrfPortal.Core** - Business logic and domain models
- **SamtryggBrfPortal.Infrastructure** - Data access and external services

## Key Features

- **Dashboard** for each user type with relevant information
- **Application Management** for subletting requests
- **Document Handling** with version control and digital signing
- **Background Checks** of potential tenants
- **Messaging System** between all parties
- **Notification System** for important updates
- **Mobile-responsive Design** for all screen sizes

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or Visual Studio Code

### Setup

1. Clone the repository
   ```
   git clone https://github.com/yourusername/samtrygg-brf-portal.git
   ```

2. Navigate to the project directory
   ```
   cd samtrygg-brf-portal
   ```

3. Restore dependencies
   ```
   dotnet restore
   ```

4. Update the database
   ```
   dotnet ef database update
   ```

5. Run the application
   ```
   dotnet run --project src/SamtryggBrfPortal.Web
   ```

6. Open your browser and navigate to:
   ```
   https://localhost:5001
   ```

## Development

### Database Migrations

To create a new migration after changing the models:

```
dotnet ef migrations add MigrationName --project src/SamtryggBrfPortal.Infrastructure --startup-project src/SamtryggBrfPortal.Web
```

To apply migrations to the database:

```
dotnet ef database update --project src/SamtryggBrfPortal.Infrastructure --startup-project src/SamtryggBrfPortal.Web
```

### Running Tests

```
dotnet test
```

## Design Guidelines

This project follows the Samtrygg design guidelines with specific color schemes:

- **Primary Color:** #1E5B94 (Dark Blue)
- **Secondary Color:** #4F9CE8 (Light Blue)
- **Accent Color:** #F7941D (Orange)
- **Neutral Colors:** #FFFFFF (White), #F5F7FA (Light Gray), #545B62 (Dark Gray)
- **Status Colors:** #28a745 (Success/Green), #ffc107 (Warning/Yellow), #dc3545 (Danger/Red)

Typography uses Montserrat for headings and Open Sans for body text.

## License

Proprietary - All rights reserved. This source code is owned by Samtrygg and may not be reproduced, distributed, or used without explicit permission.

## Contact

For more information, please contact:
- **Email:** info@samtrygg.se
- **Website:** https://samtrygg.se