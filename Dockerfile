# Use Microsoft's official .NET images to build and run the app
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY src/OfferApi/OfferApi.csproj ./src/OfferApi/
RUN dotnet restore ./src/OfferApi/OfferApi.csproj

# copy everything else and build
COPY . .
RUN dotnet publish ./src/OfferApi/OfferApi.csproj -c Release -o /app/publish --no-restore

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "OfferApi.dll"]
