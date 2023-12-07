
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

# Copy the solution file and project files and restore as distinct layers
COPY ["NewActivityProject - Copy.sln", "./"]
COPY NewActivityProject/NewActivityProject.csproj NewActivityProject/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
# Add other project references here if you have more

# Restore the Nuget packages
RUN dotnet restore "NewActivityProject - Copy.sln"

# Copy the rest of your app's source code from your host to your image filesystem.
COPY . .

# Publish the application
RUN dotnet publish NewActivityProject/NewActivityProject.csproj -c Release -o out

# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Set the working directory.
WORKDIR /app

# Copy the build output from the build-env image to the new image
COPY --from=build-env /app/out .


ENTRYPOINT ["dotnet", "NewActivityProject.dll"]