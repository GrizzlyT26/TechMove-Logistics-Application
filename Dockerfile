FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TechMove Logistics Application.csproj", "./"]
RUN dotnet restore "TechMove Logistics Application.csproj"
COPY . .
RUN dotnet publish "TechMove Logistics Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechMove Logistics Application.dll"]

