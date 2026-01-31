FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Prog2WebApi/Prog2WebApi.csproj", "Prog2WebApi/"]
RUN dotnet restore "Prog2WebApi/Prog2WebApi.csproj"
COPY . .
WORKDIR "/src/Prog2WebApi"
RUN dotnet publish "Prog2WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Create data directory for SQLite
RUN mkdir -p /app/data

EXPOSE 10000
ENTRYPOINT ["dotnet", "Prog2WebApi.dll"]