# Use the SDK image to build and publish the website
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UmbDockPi.csproj", "."]
RUN dotnet restore "UmbDockPi.csproj"
COPY . .
RUN dotnet publish "UmbDockPi.csproj" -c Release -o /app/publish

# Copy the published output to the final running image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final 
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UmbDockPi.dll"]
