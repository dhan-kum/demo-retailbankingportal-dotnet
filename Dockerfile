FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY RetailBankingPortal/RetailBankingPortal.csproj RetailBankingPortal/
RUN dotnet restore RetailBankingPortal/RetailBankingPortal.csproj

# Copy everything else and build
COPY RetailBankingPortal/ RetailBankingPortal/
WORKDIR /src/RetailBankingPortal
RUN dotnet build -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "RetailBankingPortal.dll"]
