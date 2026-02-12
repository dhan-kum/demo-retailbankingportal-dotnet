#
# Build stage
#
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY RetailBankingPortal/*.csproj ./RetailBankingPortal/
RUN dotnet restore RetailBankingPortal/RetailBankingPortal.csproj
COPY RetailBankingPortal/ ./RetailBankingPortal/
RUN dotnet publish RetailBankingPortal/RetailBankingPortal.csproj -c Release -o /app/publish

#
# Package stage
#
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "RetailBankingPortal.dll"]
