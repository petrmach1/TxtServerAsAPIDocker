# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
#EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TxtServerAsAPIDocker/TxtServerAsAPIDocker.csproj", "TxtServerAsAPIDocker/"]
RUN dotnet restore "./TxtServerAsAPIDocker/TxtServerAsAPIDocker.csproj"
COPY . .
WORKDIR "/src/TxtServerAsAPIDocker"
RUN dotnet build "./TxtServerAsAPIDocker.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TxtServerAsAPIDocker.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Download and install Visual C++ Redistributable Packages
#USER ContainerAdministrator
RUN Invoke-WebRequest -OutFile vc_redist.x64.exe "https://aka.ms/vs/17/release/vc_redist.x64.exe"; \
    Start-Process "vc_redist.x64.exe" -ArgumentList "/passive" -wait -Passthru; \
    Remove-Item -Force vc_redist.x64.exe;


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

## Install the TX Text Control Service
RUN New-Service -Name "TXBackend" -BinaryPathName '"C:\app\SyncService\TXTextControl.Web.Server.exe"';


ENTRYPOINT ["dotnet", "TxtServerAsAPIDocker.dll"]