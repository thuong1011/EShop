# EShop
cd EshopApi
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Asp.Versioning.Mvc
dotnet add package Asp.Versioning.Mvc.ApiExplorer
dotnet add package DotNetEnv
dotnet add package BCrypt.Net-Next
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
dotnet ef migrations add InitialCreate -o Infrastructure/Migrations
dotnet ef database update
dotnet run
