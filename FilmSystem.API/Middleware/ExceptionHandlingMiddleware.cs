using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace FilmSystem.API.Middleware
{

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (DbUpdateException ex)
            {
       
                _logger.LogWarning(ex, "DbUpdateException - verovatno FK constraint (Restrict) je odbio operaciju.");

                await NapisiOdgovor(context, HttpStatusCode.Conflict,
                    "Operacija nije moguca jer je ovaj podatak povezan sa drugim podacima " +
                    "(npr. postoje zavisni zapisi koji to sprecavaju). Prvo obrisi/izmeni te zavisnosti.");
            }
            catch (InvalidOperationException ex)
            {
             
                _logger.LogWarning(ex, "InvalidOperationException - verovatno nedozvoljen state machine prelaz.");

                await NapisiOdgovor(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Neocekivana greska.");

                await NapisiOdgovor(context, HttpStatusCode.InternalServerError,
                    "Doslo je do neocekivane greske na serveru.");
            }
        }

        private static async Task NapisiOdgovor(HttpContext context, HttpStatusCode statusCode, string poruka)
        {
            if (context.Response.HasStarted)
                return; 

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var telo = JsonSerializer.Serialize(new { greska = poruka });
            await context.Response.WriteAsync(telo);
        }
    }
}
