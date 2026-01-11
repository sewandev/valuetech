# Use ASP.NET Core Runtime for the final image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# Use SDK for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ValueTech.Api/ValueTech.Api.csproj", "ValueTech.Api/"]
COPY ["ValueTech.Data/ValueTech.Data.csproj", "ValueTech.Data/"]
RUN dotnet restore "./ValueTech.Api/ValueTech.Api.csproj"
COPY . .
WORKDIR "/src/ValueTech.Api"
RUN dotnet build "./ValueTech.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ValueTech.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM build AS dev
WORKDIR "/src/ValueTech.Api"
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValueTech.Api.dll"]
