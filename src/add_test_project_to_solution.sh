#!/bin/zsh

# This script adds the test project to the solution file
# Run this script after installing the .NET SDK

# Navigate to the solution directory
cd "$(dirname "$0")"

# Add the test project to the solution
dotnet sln add SamtryggBrfPortal.Tests/SamtryggBrfPortal.Tests.csproj

echo "Test project added to solution successfully!"
