# Samtrygg BRF Portal Test Environment Setup Guide

This guide provides step-by-step instructions for setting up the test environment for the Samtrygg BRF Portal project.

## Prerequisites

Before setting up the test environment, you need to install the following prerequisites:

1. **.NET 8 SDK** - Required for building and running the application and tests
2. **Visual Studio Code** (optional) - Recommended IDE for working with the project

## Step 1: Install .NET 8 SDK

Refer to the `dotnet_installation_guide.md` file for detailed instructions on installing the .NET 8 SDK on your system.

## Step 2: Verify .NET Installation

After installing the .NET SDK, verify that it's installed correctly by running:

```shell
dotnet --version
```

You should see a version number like `8.0.100` or similar.

## Step 3: Restore Dependencies

Navigate to the solution directory and restore the dependencies:

```shell
cd samtrygg_brf_portal/src
dotnet restore
```

This will download all the required NuGet packages for the project.

## Step 4: Add Test Project to Solution

Run the provided script to add the test project to the solution:

```shell
cd samtrygg_brf_portal/src
./add_test_project_to_solution.sh
```

Note: The script uses zsh (the default shell on macOS). If you're using a different shell, you may need to modify the shebang line in the script.

Alternatively, you can run the command manually:

```shell
cd samtrygg_brf_portal/src
dotnet sln add SamtryggBrfPortal.Tests/SamtryggBrfPortal.Tests.csproj
```

## Step 5: Build the Solution

Build the entire solution to ensure everything compiles correctly:

```shell
cd samtrygg_brf_portal/src
dotnet build
```

## Step 6: Run the Tests

Run the tests to verify that the test environment is set up correctly:

```shell
cd samtrygg_brf_portal/src
dotnet test SamtryggBrfPortal.Tests
```

## Test Project Structure

The test project is organized into the following directories:

- **Core**: Tests for the domain entities and business logic
- **Infrastructure**: Tests for data access and services
- **Web**: Tests for controllers, views, and API endpoints

## Running Specific Tests

To run tests in a specific category:

```shell
dotnet test SamtryggBrfPortal.Tests --filter "Category=UnitTest"
```

To run tests in a specific class:

```shell
dotnet test SamtryggBrfPortal.Tests --filter "FullyQualifiedName~BrfAssociationTests"
```

## Test Coverage

To generate a test coverage report:

```shell
dotnet add SamtryggBrfPortal.Tests/SamtryggBrfPortal.Tests.csproj package coverlet.msbuild
dotnet test SamtryggBrfPortal.Tests /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

Then use a tool like ReportGenerator to create a visual report:

```shell
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:./SamtryggBrfPortal.Tests/coverage.opencover.xml -targetdir:./coverage
```

## Troubleshooting

### Common Issues

1. **dotnet command not found**
   - Ensure the .NET SDK is installed correctly
   - Check if the .NET SDK is in your PATH

2. **Build errors**
   - Make sure all dependencies are restored
   - Check for syntax errors in the code

3. **Test failures**
   - Check the test output for details on why tests are failing
   - Ensure the test environment is set up correctly

### Getting Help

If you encounter any issues with the test environment setup, please refer to:

- The official .NET documentation: https://docs.microsoft.com/en-us/dotnet/
- The xUnit documentation: https://xunit.net/docs/getting-started/netcore/cmdline
- The Entity Framework Core documentation: https://docs.microsoft.com/en-us/ef/core/

## Next Steps

After setting up the test environment, you can:

1. Run the application to see it in action
2. Add more tests to increase test coverage
3. Implement new features and ensure they're covered by tests
