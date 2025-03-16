FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MoneyTransfer.API/MoneyTransfer.API.csproj", "MoneyTransfer.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "MoneyTransfer.API/MoneyTransfer.API.csproj"
COPY . .
WORKDIR "/src/MoneyTransfer.API"
RUN dotnet build "MoneyTransfer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyTransfer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyTransfer.API.dll"] 