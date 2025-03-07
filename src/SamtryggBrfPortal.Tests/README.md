# SamtryggBrfPortal Test Project

This project contains tests for the Samtrygg BRF Portal application. It includes unit tests, integration tests, and end-to-end tests for the different layers of the application.

## Test Structure

The test project is organized into the following directories:

- **Core**: Tests for the domain entities and business logic
- **Infrastructure**: Tests for data access and services
- **Web**: Tests for controllers, views, and API endpoints

## Test Types

### Unit Tests

Unit tests focus on testing individual components in isolation. They use mocking to replace dependencies and focus on the behavior of a single unit of code.

Examples:
- `BrfAssociationTests.cs`: Tests for the BrfAssociation entity
- `HomeControllerTests.cs`: Tests for the HomeController

### Integration Tests

Integration tests verify that different components work together correctly. They often use an in-memory database to test data access code.

Examples:
- `ApplicationDbContextTests.cs`: Tests for the database context and entity relationships

### End-to-End Tests

End-to-end tests verify that the entire application works correctly from the user's perspective. They use a test server to simulate HTTP requests and responses.

Examples:
- `IntegrationTests.cs`: Tests for the web application's HTTP endpoints

## Test Fixtures

The test project includes several test fixtures to set up the test environment:

- `DatabaseFixture.cs`: Sets up an in-memory database for testing
- `WebApplicationFactory.cs`: Sets up a test server for end-to-end testing

## Running the Tests

### Prerequisites

- .NET 8 SDK

### Commands

To run all tests:

```
cd samtrygg_brf_portal/src
dotnet test SamtryggBrfPortal.Tests
```

To run tests in a specific category:

```
dotnet test SamtryggBrfPortal.Tests --filter "Category=UnitTest"
```

To run tests in a specific class:

```
dotnet test SamtryggBrfPortal.Tests --filter "FullyQualifiedName~BrfAssociationTests"
```

## Adding New Tests

When adding new tests, follow these guidelines:

1. Place tests in the appropriate directory based on what they're testing (Core, Infrastructure, or Web)
2. Use descriptive test names that explain what the test is verifying
3. Follow the Arrange-Act-Assert pattern
4. Use test fixtures for common setup code
5. Keep tests independent and idempotent

## Test Coverage

To generate a test coverage report:

```
dotnet test SamtryggBrfPortal.Tests /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

Then use a tool like ReportGenerator to create a visual report:

```
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:./SamtryggBrfPortal.Tests/coverage.opencover.xml -targetdir:./coverage
