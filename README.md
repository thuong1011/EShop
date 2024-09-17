# Describe
This project is a Web API application that allows Web API to login and sell products, with 2 functions: seller and buyer. Buyers can buy, sellers can add, edit and delete products. Both can log in to the same endpoint. Different permissions. APIs can have different versions and use cookies. 
# EShop
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
