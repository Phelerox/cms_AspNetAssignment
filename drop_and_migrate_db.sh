dotnet ef database drop -f --verbose
dotnet ef migrations remove --verbose
dotnet ef migrations add $1 --verbose
dotnet ef database update --verbose
