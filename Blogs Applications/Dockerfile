


# This stage is used when running from VS in fast mode
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#  ’ÕÌÕ «·Œÿ√ «·≈„·«∆Ì Â‰« ?
COPY ["Blogs Applications/Blogs Applications.csproj", "Blogs Applications/"]
COPY ["Application/Application.csproj", "Application/"]  # ? ’ÕÕ 
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "./Blogs Applications/Blogs Applications.csproj"
COPY . .
WORKDIR "/src/Blogs Applications"
RUN dotnet build "./Blogs Applications.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blogs Applications.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blogs Applications.dll"]