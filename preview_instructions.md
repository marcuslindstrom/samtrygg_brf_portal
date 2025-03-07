# How to Preview the Samtrygg BRF Portal Prototype

We've created a functional prototype that can be run and previewed without setting up a database. The prototype includes the following pages:

1. Home page - Landing page with key service information
2. About page - Detailed information about the service
3. Contact page - Contact form and FAQ

## Running the Prototype

### Option 1: Using Visual Studio 2022

1. **Open the solution**
   - Open Visual Studio 2022
   - Open the solution file at `samtrygg_brf_portal/src/SamtryggBrfPortal.sln`

2. **Run the application**
   - Press F5 or click the "Run" button
   - The application will automatically use an in-memory database for preview
   - A demo admin user will be created (username: admin@example.com, password: Password123!)

### Option 2: Using .NET CLI

1. **Navigate to the Web project directory**
   ```
   cd /Users/marcuslindstrom/samtrygg_cleaning/samtrygg_brf_portal/src/SamtryggBrfPortal.Web
   ```

2. **Restore dependencies**
   ```
   dotnet restore
   ```

3. **Run the application**
   ```
   dotnet run
   ```

4. **Open in a browser**
   - Open your browser and go to:
   - https://localhost:5001 or http://localhost:5000
   - The application uses an in-memory database by default
   - A demo admin user will be created (username: admin@example.com, password: Password123!)

## What's Included in the Prototype

The current prototype includes:

1. **Complete project structure:**
   - SamtryggBrfPortal.Web - The web interface
   - SamtryggBrfPortal.Core - Core entities and business logic
   - SamtryggBrfPortal.Infrastructure - Data access and services

2. **Core entities for:**
   - BRF associations
   - Properties
   - Rental applications
   - Document management
   - Background checks
   - Messaging and notifications

3. **Visual pages:**
   - Home page with service overview
   - About page with process description
   - Contact page with form and FAQs

4. **Authentication framework:**
   - Ready for integration with Swedish BankID
   - User roles for BRF boards, property owners, tenants, and admins

## What's Missing

The following features are not yet implemented in this early prototype:

1. **User dashboard pages** - These will be developed next
2. **Authentication screens** - Login and registration pages
3. **Application forms** for subletting
4. **Document upload functionality**
5. **Actual background check integration**
6. **Real database integration** - Currently using in-memory database
7. **Real images** - Placeholders are used instead

## Design Notes

The prototype follows the specified design guidelines:

- **Primary Color:** #1E5B94 (Dark Blue)
- **Secondary Color:** #4F9CE8 (Light Blue)
- **Accent Color:** #F7941D (Orange)
- **Typography:** Montserrat for headings, Open Sans for body text
- **Design Elements:** Rounded corners, subtle shadows, generous white space

## Next Steps

After reviewing this prototype, the next development steps would be:

1. Implement authentication screens and workflows
2. Create dashboard interfaces for each user type
3. Build application forms for subletting requests
4. Implement document upload and management
5. Create the messaging system between parties
6. Build the background check interface
7. Set up real database connectivity
8. Integrate with BankID for secure signing