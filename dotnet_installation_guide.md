# Installing .NET 8 SDK on macOS

This guide provides instructions for installing the .NET 8 SDK on macOS, which is required to run and test the Samtrygg BRF Portal project.

## Method 1: Using the Official Installer (Recommended)

1. **Download the .NET 8 SDK installer**:
   - Visit [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Click on "Download .NET 8.0 SDK" (macOS x64 or ARM64 depending on your Mac)
   - This will download a .pkg installer file

2. **Run the installer**:
   - Open the downloaded .pkg file
   - Follow the installation instructions
   - The installer will guide you through the process

3. **Verify the installation**:
   - Open a new Terminal window (to ensure the PATH is updated)
   - Run the following command:
     ```
     dotnet --version
     ```
   - You should see the version number (e.g., 8.0.100)

## Method 2: Using Homebrew

If you have Homebrew installed, you can use it to install the .NET SDK:

1. **Open Terminal** and run:
   ```
   brew install dotnet
   ```

2. **Verify the installation**:
   ```
   dotnet --version
   ```

## Troubleshooting

### If dotnet command is not found after installation:

1. **Check if the .NET SDK was installed**:
   - Look for the .NET SDK in the following locations:
     - `/usr/local/share/dotnet/`
     - `/usr/local/bin/dotnet`

2. **Add to PATH manually**:
   - If the SDK is installed but not in your PATH, add it:
   - Edit your shell profile file:
     - For zsh (default on macOS):
       ```
       echo 'export PATH=$PATH:/usr/local/share/dotnet' >> ~/.zshrc
       source ~/.zshrc
       ```
     - For bash:
       ```
       echo 'export PATH=$PATH:/usr/local/share/dotnet' >> ~/.bash_profile
       source ~/.bash_profile
       ```

3. **Restart Terminal**:
   - Sometimes simply restarting the Terminal application helps

### If you encounter permission issues:

1. **Use sudo for the installation**:
   - For Homebrew: `sudo brew install dotnet`
   - For manual installation: ensure you have admin rights

## Next Steps After Installation

Once the .NET 8 SDK is installed, you can proceed with:

1. **Restoring project dependencies**:
   ```
   cd samtrygg_brf_portal/src
   dotnet restore
   ```

2. **Running the application**:
   ```
   cd SamtryggBrfPortal.Web
   dotnet run
   ```

3. **Setting up the test environment** as outlined in our plan
