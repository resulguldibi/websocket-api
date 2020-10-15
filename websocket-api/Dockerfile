#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["websocket-api/websocket-api.csproj", "websocket-api/"]
RUN dotnet restore "websocket-api/websocket-api.csproj"
COPY . .
WORKDIR "/src/websocket-api"
RUN dotnet build "websocket-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "websocket-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "websocket-api.dll"]