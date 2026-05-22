# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Copy project file and restore dependencies
COPY ECommerceApp.csproj .
RUN dotnet restore
# Copy all files and publish the application
COPY . .
RUN dotnet publish -c Release -o /app/publish
# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8089
ENTRYPOINT ["dotnet", "ECommerceApp.dll"]