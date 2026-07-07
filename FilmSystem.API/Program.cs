using FilmSystem.API.Middleware;
using FilmSystem.API.Services;
using FilmSystem.API.Services.Omdb;
using FilmSystem.Domain.Repositories;
using FilmSystem.Infrastructure.Data;
using FilmSystem.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core - registrujemo DbContext sa SQL Server konekcijom iz appsettings.json
builder.Services.AddDbContext<FilmSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work - jedan po HTTP zahtevu (Scoped), da svi repozitorijumi
// u okviru istog zahteva dele isti DbContext
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRezervacijaStateMachineService, RezervacijaStateMachineService>();

// OMDb integracija - named HttpClient sa BaseAddress iz konfiguracije i
// standardnim resilience handler-om (retry sa exponential backoff + timeout)
// iz Microsoft.Extensions.Http.Resilience paketa.
builder.Services.AddHttpClient(OmdbService.HttpClientName, client =>
{
    var baseUrl = builder.Configuration["Omdb:BaseUrl"] ?? "https://www.omdbapi.com/";
    client.BaseAddress = new Uri(baseUrl);
})
.AddStandardResilienceHandler();

builder.Services.AddScoped<IOmdbService, OmdbService>();

// FluentValidation - automatski trazi sve klase koje nasledjuju AbstractValidator<T>
// u ovoj (API) assembly-ju i registruje ih; AddFluentValidationAutoValidation
// kaci validaciju direktno u MVC pipeline (ModelState), tako da je [ApiController]
// automatski vraca 400 BadRequest kad validacija ne prodje - kontroler ne mora
// rucno da poziva validator.
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Scoped);
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Mora biti PRVI u pipeline-u da bi mogao da uhvati greske iz svega sto dolazi posle njega
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
