# Speedoku Royale | Server

## Prerequisites

Make sure you have these installed

- **MariaDB 10.7.3**
  https://mariadb.org/download/?t=mariadb&o=true&p=mariadb&r=10.7.3&os=windows&cpu=x86_64&pkg=msi

- **.NET 6** (Both **SDK**, and **Runtime**)
  https://dotnet.microsoft.com/en-us/download

- **VS Code**
  https://code.visualstudio.com/

- Latest version of **C# Extension** By Microsoft for VS Code
  *Download it from the Extensions marketplace*

Create following files in the project root, and replace dummy text places with
actual information.

### appsettings.Development.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "MariaDB": "server=localhost;user id=YOUR_USER_NAME_GOES_HERE;password=YOUR_PW_GOES_HERE;database=speedoku_royale_db"
  }
}

```

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

To initialize the MariaDB SQL Database, run the
`speedoku_royale_db_test_init.sql` query in the database server. You can use
HeidiSQL for this.

After this you should be able to run the server with `dotnet run` command.
After it starts, go to https://localhost:8000/swagger/index.html and try out
the different api features that they work.

## Useful commands

Trust the HTTPS development certificate by running the following command 
**Windows only**

`dotnet dev-certs https --trust`

Start server

`dotnet run`

You can shut down the server with **CTRL + C**

## VS Code tips

If you think that there's errors that doesn't make sense, it might be because of
OmniSharp is acting up. Restart OmniSharp by opening QuickOpen with **CTRL + P**
and writing following line and press **Enter**:

`> Omnisharp: Restart OmniSharp`
