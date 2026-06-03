FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ['TestWorkindForEducation/TestWorkindForEducation.csproj', 'TestWorkindForEducation/']
COPY ['Application/Application.csproj', 'Application/']
COPY ['Domain/Domain.csproj', 'Domain/']
COPY ['Infrastructure/Infrastructure.csproj', 'Infrastructure/']
RUN dotnet restore 'TestWorkindForEducation/TestWorkindForEducation.csproj'
COPY . .
WORKDIR '/src/TestWorkindForEducation'
RUN dotnet build 'TestWorkindForEducation.csproj' -c Release -o /app/build

FROM build AS publish
RUN dotnet publish 'TestWorkindForEducation.csproj' -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ['dotnet', 'TestWorkindForEducation.dll']
