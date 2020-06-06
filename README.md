# TweetbookAPI
This is a solution that demonstrates a possible way to build powerful, scalable and secure WebAPIs using .Net Core 3.1 with C# language. The source code was written based on a series of YouTube videos authored by Nick Chapsas.

Application features

Scalable code with async/await keywords
Separated domain-model from request and response objects by using AutoMapper (Encapsulated via CastTo<T> extension method)
Service installers pattern for a better separation of app configuration logic (IInstaller interface)
Centralized external input data validation
Authentication
 - Token-based JWT bearer
 - Refresh tokens
 - Client Api Key
Authorization
 - Claims-based
 - Role-based
 - Policy-based
Versioning
 Routed versioning
Logging/auditing (To do!)
Database access by using Entity Framework Core DataContext class along with command-line Migrations
Developer documentation
 - Swagger web page
 
API Client SDK for consumers with samples (To do!) -> https://www.youtube.com/watch?v=grBTYaTGLv8&list=PLUOequmGnXxOgmSDWU7Tl6iQTsOtyjtwU&index=24
 - Refit (RestService class) by defining API interfaces
Initial dataload with server initialization code
Health checking
Basic Integration tests

Packages
 - Microsoft.EntityFrameworkCore
 - Microsoft.EntityFrameworkCore.SqlServer
 - Microsoft.AspNetCore.Authentication.JwtBearer
 - Microsoft.AspNetCore.Identity.EntityFrameworkCore
 - FluentValidation.AspNetCore
 - Swashbuckle.AspNetCore
 - Swashbuckle.AspNetCore.Filters
 - Microsoft.AspNetCore.Mvc.Versioning
 - AutoMapper.Extensions.Microsoft.DependencyInjection
 - Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
 - Refit
 - xUnit
 - FluentAssertions
 - Microsoft.AspNet.WebApi.Client
 - Microsoft.AspNet.Mvc.Testing
 - Microsoft.EntityFrameworkCore.InMemory
