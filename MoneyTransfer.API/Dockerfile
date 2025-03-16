FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["MoneyTransfer.API/MoneyTransfer.API.csproj", "MoneyTransfer.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "MoneyTransfer.API/MoneyTransfer.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/MoneyTransfer.API"
RUN dotnet build "MoneyTransfer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyTransfer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add non-root user
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1

ENTRYPOINT ["dotnet", "MoneyTransfer.API.dll"] 