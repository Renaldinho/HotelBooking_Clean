FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HotelBooking.WebApi/HotelBooking.WebApi.csproj", "HotelBooking.WebApi/"]
COPY ["HotelBooking.Core/HotelBooking.Core.csproj", "HotelBooking.Core/"]
COPY ["HotelBooking.Infrastructure/HotelBooking.Infrastructure.csproj", "HotelBooking.Infrastructure/"]
RUN dotnet restore "HotelBooking.WebApi/HotelBooking.WebApi.csproj"
COPY . .
WORKDIR "/src/HotelBooking.WebApi"
RUN dotnet build "HotelBooking.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HotelBooking.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelBooking.WebApi.dll"]
