FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src 

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy only the updated .csproj file to avoid copying all files
COPY ["ProductApp.API/ProductApp.API.csproj", "ProductApp.API/"]

# Restore dependencies for the project (use this step for any dependency updates)
RUN dotnet restore "ProductApp.API/ProductApp.API.csproj"

# Optionally, copy the rest of the code if other files have changed
COPY . .

# Build the app (this is where the actual build happens)
WORKDIR "/src/ProductApp.API"
RUN dotnet build "ProductApp.API.csproj" -c Release -o /app/build

# Publish the app to be used in the final image
FROM build AS publish
RUN dotnet publish "ProductApp.API.csproj" -c Release -o /app/publish

# Final image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductApp.API.dll"]