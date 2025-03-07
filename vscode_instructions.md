# Running the Samtrygg BRF Portal Prototype in Visual Studio Code

Visual Studio Code is an excellent choice for working with this ASP.NET Core project. Here's how to set it up and run the prototype:

## Prerequisites

1. **Install .NET 8 SDK**
   - Download and install from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Verify installation by running `dotnet --version` in your terminal

2. **Install Visual Studio Code**
   - Download from [https://code.visualstudio.com/](https://code.visualstudio.com/)

3. **Install Recommended Extensions**
   - C# Dev Kit (by Microsoft)
   - C# (by Microsoft)
   - .NET Core Tools (by Jun Han)
   - IntelliCode (by Microsoft)
   - HTML CSS Support (by ecmel)

## Opening the Project

1. **Launch Visual Studio Code**

2. **Open the project folder**
   - File > Open Folder...
   - Navigate to `/Users/marcuslindstrom/samtrygg_cleaning/samtrygg_brf_portal`
   - Click 'Open'

3. **Trust the workspace if prompted**
   - Click "Yes, I trust the authors"

## Restoring Dependencies

1. **Open the terminal in VS Code**
   - Terminal > New Terminal (or press Ctrl+` / Cmd+`)

2. **Navigate to the solution directory**
   ```
   cd src
   ```

3. **Restore dependencies**
   ```
   dotnet restore
   ```

## Running the Project

1. **Navigate to the Web project**
   ```
   cd SamtryggBrfPortal.Web
   ```

2. **Run the application**
   ```
   dotnet run
   ```
   
3. **Open your browser**
   - Navigate to https://localhost:5001 or http://localhost:5000
   - The application will be running with an in-memory database
   - A demo admin user will be created (username: admin@example.com, password: Password123!)

## Debugging in VS Code

1. **Set up launch configuration**
   - In VS Code, go to the Run and Debug view (Ctrl+Shift+D / Cmd+Shift+D)
   - Click "create a launch.json file" if it doesn't exist
   - Select ".NET Core" from the dropdown
   - Choose "Web Launch" template

2. **Start debugging**
   - Press F5 or click the green play button
   - VS Code will build and launch the application
   - The browser will open automatically

## Working with the Code

1. **Solution Explorer**
   - Open the C# Projects view in VS Code to see the project structure
   - This gives you a similar experience to Visual Studio's Solution Explorer

2. **Editing Files**
   - Use the Explorer view to browse and open files
   - VS Code will provide IntelliSense for C# files
   - HTML files will have formatting and CSS support

3. **Running Tasks**
   - Use Terminal > Run Task... to access common .NET tasks
   - Or use the terminal to run dotnet commands manually

## Making Changes

1. **Edit the views**
   - The main views are in `/src/SamtryggBrfPortal.Web/Views`
   - Home, About, and Contact pages are in the Home folder
   - The site layout is in `/src/SamtryggBrfPortal.Web/Views/Shared/_Layout.cshtml`

2. **Edit the styles**
   - The main CSS file is in `/src/SamtryggBrfPortal.Web/wwwroot/css/site.css`

3. **After making changes**
   - Save your files (Ctrl+S / Cmd+S)
   - The application will automatically reload changes to views
   - For C# code changes, you'll need to stop and restart the application

## Troubleshooting

1. **If you see HTTPS certificate errors:**
   - Run `dotnet dev-certs https --trust` in the terminal
   - Restart your browser

2. **If port 5001 is already in use:**
   - Edit `Properties/launchSettings.json` to change the port
   - Or kill the process using that port

3. **If you see dependency errors:**
   - Run `dotnet restore` again
   - Make sure you have .NET 8 SDK installed, not just the runtime

## Next Steps

After exploring the prototype, you might want to:

1. Create new views for the dashboard interfaces
2. Add more controllers for different functionality
3. Extend the entity models in the Core project
4. Connect to a real database instead of the in-memory one