# Utilisez une image de base officielle comme point de départ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Utilisez l'image SDK pour construire l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN echo ls
# Copiez le fichier .csproj et restaurez les dépendances
COPY ["APIBoiteALivre/APIBoiteALivre.csproj", "APIBoiteALivre/"]
RUN dotnet restore "APIBoiteALivre/APIBoiteALivre.csproj"

# Copiez le reste du code et construisez l'application
COPY . .
WORKDIR "/src/APIBoiteALivre"
RUN dotnet build "APIBoiteALivre.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publiez l'application
FROM build AS publish
RUN dotnet publish "APIBoiteALivre.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Construisez l'image runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIBoiteALivre.dll"]
