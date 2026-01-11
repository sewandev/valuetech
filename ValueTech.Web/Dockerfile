# Use ASP.NET Core Runtime for the final image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# Use SDK for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ValueTech.Web/ValueTech.Web.csproj", "ValueTech.Web/"]
COPY ["ValueTech.Api/ValueTech.Api.csproj", "ValueTech.Api/"]
COPY ["ValueTech.Data/ValueTech.Data.csproj", "ValueTech.Data/"]
# Note: Web refers to Api project? Checking csproj... Yes, it references Api csproj.
RUN dotnet restore "./ValueTech.Web/ValueTech.Web.csproj"
COPY . .
WORKDIR "/src/ValueTech.Web"
RUN dotnet build "./ValueTech.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ValueTech.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM build AS dev
WORKDIR "/src/ValueTech.Web"
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
ENTRYPOINT ["dotnet", "ValueTech.Web.dll"]

