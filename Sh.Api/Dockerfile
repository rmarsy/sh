FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
WORKDIR /src
COPY ["Sh.Api/Sh.Api.csproj", "Sh.Api/"]
RUN dotnet restore "Sh.Api/Sh.Api.csproj"
COPY . .
WORKDIR /src/Sh.Api
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sh.Api/Sh.Api.csproj", "Sh.Api/"]
RUN dotnet restore "Sh.Api/Sh.Api.csproj"
COPY . .
WORKDIR /src/Sh.Api
RUN dotnet publish "Sh.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Sh.Api.dll"]
