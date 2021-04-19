# TweetbookAPI
This is a solution that demonstrates a possible way to build powerful, scalable and secure WebAPIs using .Net Core 3.1 with C# language.<br/>
The source code was written based on a series of YouTube videos authored by Nick Chapsas.<br/>

Application features

Scalable code with async/await keywords<br/>
Separated domain-model from request and response objects by using AutoMapper (Encapsulated via CastTo<T> extension method)<br/>
Service installers pattern for a better separation of app configuration logic (IInstaller interface)<br/>
Centralized external input data validation<br/>
Authentication<br/>
 - Token-based JWT bearer<br/>
 - Refresh tokens<br/>
 - Client Api Key<br/>
Authorization<br/>
 - Claims-based<br/>
 - Role-based<br/>
 - Policy-based<br/>
Versioning<br/>
 Routed versioning<br/>
Logging/auditing (To do!)
Database access by using Entity Framework Core DataContext class along with command-line Migrations<br/>
Developer documentation<br/>
 - Swagger web page<br/>
 
API Client SDK for consumers with samples (To do!) -> https://www.youtube.com/watch?v=grBTYaTGLv8&list=PLUOequmGnXxOgmSDWU7Tl6iQTsOtyjtwU&index=24<br/>
 - Refit (RestService class) by defining API interfaces<br/>
Initial dataload with server initialization code<br/>
Health checking<br/>
Basic Integration tests

Packages<br/>
 - Microsoft.EntityFrameworkCore<br/>
 - Microsoft.EntityFrameworkCore.SqlServer<br/>
 - Microsoft.AspNetCore.Authentication.JwtBearer<br/>
 - Microsoft.AspNetCore.Identity.EntityFrameworkCore<br/>
 - FluentValidation.AspNetCore<br/>
 - Swashbuckle.AspNetCore<br/>
 - Swashbuckle.AspNetCore.Filters<br/>
 - Microsoft.AspNetCore.Mvc.Versioning<br/>
 - AutoMapper.Extensions.Microsoft.DependencyInjection<br/>
 - Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore<br/>
 - Refit<br/>
 - xUnit<br/>
 - FluentAssertions<br/>
 - Microsoft.AspNet.WebApi.Client<br/>
 - Microsoft.AspNet.Mvc.Testing<br/>
 - Microsoft.EntityFrameworkCore.InMemory
