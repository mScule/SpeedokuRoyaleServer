# Install .NET 6
apt-get update
apt-get install -y dotnet6 ca-certificates

dotnet tool install --global dotnet-ef

# Install packages
dotnet add package Swashbuckle.AspNetCore               --version 6.2.3
dotnet add package Pomelo.EntityFrameworkCore.MySql     --version 6.0.2
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.8
dotnet add package Microsoft.EntityFrameworkCore.Tools
