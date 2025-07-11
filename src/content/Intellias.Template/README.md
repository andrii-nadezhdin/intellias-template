#if (SQLServer)
dotnet ef migrations add InitialCreate -s ./src/Intellias.Template.Api -p ./src/Intellias.Template.Infrastructure.Database
dotnet ef database update -s ./src/Intellias.Template.Api -p ./src/Intellias.Template.Infrastructure.Database
#endif