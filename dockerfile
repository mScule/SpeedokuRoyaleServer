FROM ubuntu:22.04

# Install the .NET 6 SDK from the Ubuntu archive
# (no need to clean the apt cache as this is an unpublished stage)
RUN apt-get update && apt-get install -y dotnet6 ca-certificates

WORKDIR /usr/SpeedokuRoyaleServer

# Install .NET entity framework
RUN dotnet tool install --global dotnet-ef

# Copying project
COPY SpeedokuRoyaleServer.csproj .
COPY ./appsettings.Development.json .
COPY ./Migrations ./Migrations
COPY ./Properties ./Properties
COPY ./Source     ./Source

# Install packages
RUN dotnet add package Swashbuckle.AspNetCore               --version 6.2.3
RUN dotnet add package Pomelo.EntityFrameworkCore.MySql     --version 6.0.2
RUN dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.8
RUN dotnet add package Microsoft.EntityFrameworkCore.Tools

# Copying shell scripts
COPY ./init-db.sh .
COPY ./reset-db.sh .
COPY ./run-server.sh .
COPY ./start.sh .

EXPOSE 8000
EXPOSE 47819
EXPOSE 44366

CMD ["bash", "start.sh"]
