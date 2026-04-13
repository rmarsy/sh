FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
WORKDIR /src
COPY ["Sh.Web/Sh.Web.csproj", "Sh.Web/"]
RUN dotnet restore "Sh.Web/Sh.Web.csproj"
COPY . .
WORKDIR /src/Sh.Web
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sh.Web/Sh.Web.csproj", "Sh.Web/"]
RUN dotnet restore "Sh.Web/Sh.Web.csproj"
COPY . .
WORKDIR /src/Sh.Web
RUN dotnet publish "Sh.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Sh.Web.dll"]
