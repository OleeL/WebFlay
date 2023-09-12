# WebFlay

Scraper

## Setting Up the Database in Docker
### Prerequisites
Ensure you have Docker and docker compose installed on your machine for db set up or set up your sql express in your own way.
### Steps

Run the docker compose file: 

```bash
docker-compose up
```

## Applying Database Migrations

### Prerequisites

Make sure you have .NET 7 SDK installed.

### Steps
Navigate to the Project Directory:

Run the following command to apply migrations (or apply migrations in your gui):

```bash
dotnet restore
dotnet build
dotnet ef database update -p WebFlay.Lib -c WebFlayDbContext -s WebFlay.API.Web
```

You can run the project with:

```bash
dotnet run --project WebFlay.API.Web --release
```

Or just run the http launch configuration in the gui
