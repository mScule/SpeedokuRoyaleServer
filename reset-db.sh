dotnet ef database drop -f -v;
dotnet ef migrations add Initial;
dotnet ef database update;
