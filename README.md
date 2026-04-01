# HotelBooking - Full Stack Hotel Booking System

Angular 21 + .NET 8 + SQL Server

## Run with Docker (Recommended)

**Prerequisites:** [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.

```bash
# Clone the repo
git clone <repo-url>
cd HotelBooking

# Start everything
docker compose up --build

# Wait ~60 seconds for first build, then open:
# http://localhost:4000
```

That's it! The database is created and seeded automatically.

To stop:
```bash
docker compose down
```

To reset the database:
```bash
docker compose down -v
docker compose up --build
```

## Run Locally (Development)

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 22+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for SQL Server)

### 1. Start SQL Server
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=HotelBooking@2024" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/azure-sql-edge:latest
```

### 2. Start Backend API
```bash
cd HotelBooking.API
dotnet run --urls "http://localhost:5135"
```

### 3. Start Frontend
```bash
cd hotel-booking-client
npm install
ng serve
```

Open http://localhost:4200

## Features
- Hotel search with filters (city, price, stars, guests)
- Hotel details with room categories and availability
- User registration and JWT authentication
- Room booking with promo code support
- Booking confirmation emails
- My Bookings page

## Tech Stack
- **Frontend:** Angular 21, Bootstrap 5, SCSS
- **Backend:** ASP.NET Core 8, Entity Framework Core
- **Database:** SQL Server (Azure SQL Edge for ARM/Mac)
- **Email:** Gmail SMTP with MailKit
- **Auth:** JWT + ASP.NET Identity

## Available Hotels
US: New York, Miami, Denver, San Francisco, Charleston, Chicago, Phoenix, Seattle
India: Ahmedabad, Mumbai, Delhi, Jaipur, Goa, Bangalore
