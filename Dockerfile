# Use Microsoft's official .NET images to build and run the app
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY OfferApiService/OfferApiService.csproj ./OfferApiService/
RUN dotnet restore ./OfferApiService/OfferApiService.csproj

# copy everything else and build
COPY . .
RUN dotnet publish ./OfferApiService/OfferApiService.csproj -c Release -o /app/publish --no-restore

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "OfferApiService.dll"]