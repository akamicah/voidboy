FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 9400
EXPOSE 9401

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

RUN mkdir ./DirectoryService.Api && \
mkdir ./DirectoryService.Core && \
mkdir ./DirectoryService.Core.Services && \
mkdir ./DirectoryService.DAL && \
mkdir ./DirectoryService.Shared && \
mkdir ./DirectoryService.Tests

COPY OverteDirectoryService.sln ./
COPY ./DirectoryService.Api/DirectoryService.Api.csproj ./DirectoryService.Api
COPY ./DirectoryService.Core/DirectoryService.Core.csproj ./DirectoryService.Core
COPY ./DirectoryService.Core.Services/DirectoryService.Core.Services.csproj ./DirectoryService.Core.Services
COPY ./DirectoryService.DAL/DirectoryService.DAL.csproj ./DirectoryService.DAL
COPY ./DirectoryService.Shared/DirectoryService.Shared.csproj ./DirectoryService.Shared
COPY ./DirectoryService.Tests/DirectoryService.Tests.csproj ./DirectoryService.Tests

RUN ls -la
RUN dotnet restore
COPY . .
WORKDIR "/src/DirectoryService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DirectoryService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DirectoryService.Api.dll"]
