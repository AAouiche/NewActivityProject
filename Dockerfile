
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
EXPOSE 44314 
EXPOSE 5262 
EXPOSE 7056

WORKDIR /app


COPY ["NewActivityProject - Copy.sln", "./"]
COPY NewActivityProject/NewActivityProject.csproj NewActivityProject/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
# Add other project references here if you have more


RUN dotnet restore "NewActivityProject - Copy.sln"

# Copy the rest of your app's source code from your host to your image filesystem.
COPY . .

# Publish the application
RUN dotnet publish NewActivityProject/NewActivityProject.csproj -c Release -o out

# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app


COPY --from=build-env /app/out .


ENTRYPOINT ["dotnet", "NewActivityProject.dll"]