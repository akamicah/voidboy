This project is not yet suitable for use in production

# Overte Directory Service (Work In Progress)
### Project Voidboy

Copyright 2022, Overte e.V.

## Dependencies
This project is being developed using **asp dotnet 6.0**. Instructions on how to install this can be found here: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

The goal will be to provide this as a docker image so that no dependencies (except docker) will be required on the target server.

## Architecture
The architecture of this software follows loosely the onion architecture, and adopts the Dapper Repository pattern.

## Projects
- **DirectoryService.Api** - The entrypoint of the application that hosts the API service
- **DirectoryService.Core** - Home to core application components such as Entities, DTOs (Data-transfer objects), and repository interfaces
- **DirectoryService.Core.Services** - The core application service layer where services are contained
- **DirectoryService.DAL** - The Data-Access layer. This project should have no dependency on the service or API layer, and should be interchangeable in future with other database connectors.
- **DirectoryService.Shared** - Shared components required by the other layers of the application
- **DirectoryService.Tests** - Testing environment which houses integration and endpoint tests.

# API V1 Schema
This project aims to implement support for the already existing API schema defined and implemented in: https://github.com/overte-org/overte-metaverse

In future V2 will be designed to further standardise the API.

# Running Locally

Inside the `development` directory is a docker-compose.yml file which will setup both the database and mailhog. Simply run `docker-compose up -d` inside the `development` directory to create the services, or `docker-compose down` to terminate them.

## Database
In order to run locally, you need to have a local instance of Postgres running. The easiest way to do this is by using Docker.

**Treat all data in development as temporary, especially when running tests**

Run the following docker command to get the database setup;

`docker run -d --name postgres --rm -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=directory_service -p 5432:5432 -it postgres:15.1-alpine`

If you change the default postgres password then the password must be updated in the `DirectoryService.Api/config/serviceConfig.json`

## Email Testing
Recommended testing environment: https://github.com/mailhog/MailHog

# Testing
**WARNING:** All directory service database tables will be truncated (cleared) upon running tests, and test data seeded, so make sure you're not running with production data.

The tests included cover API endpoint tests and integration tests to ensure services are running the way they should.

With any contributions, please ensure tests are written to provide coverage where applicable.