using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace FilmSystem.API.Middleware
{
    // Hvata neuhvacene greske iz cele aplikacije na jednom mestu, umesto da svaki
    // kontroler ima svoj try/catch. Registruje se u Program.cs kao PRVI middleware
    // u pipeline-u (app.UseMiddleware<ExceptionHandlingMiddleware>()) da moze
    // da uhvati greske iz svega sto dolazi posle njega.
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
                await _next(context); // pusti zahtev dalje kroz ostatak pipeline-a (kontroleri itd.)
            }
            catch (DbUpdateException ex)
            {
                // Ovo se desava npr. kad SQL Server odbije DELETE/UPDATE zbog
                // OnDelete(Restrict) FK constraint-a (npr. brisanje Zanra koji ima Filmove,
                // ili Sedista koje ima Rezervacije).
                _logger.LogWarning(ex, "DbUpdateException - verovatno FK constraint (Restrict) je odbio operaciju.");

                await NapisiOdgovor(context, HttpStatusCode.Conflict,
                    "Operacija nije moguca jer je ovaj podatak povezan sa drugim podacima " +
                    "(npr. postoje zavisni zapisi koji to sprecavaju). Prvo obrisi/izmeni te zavisnosti.");
            }
            catch (InvalidOperationException ex)
            {
                // Npr. Stateless sm.Fire() na nedozvoljen prelaz, ako se negde pozove
                // direktno bez prethodne provere MozeDaPredje().
                _logger.LogWarning(ex, "InvalidOperationException - verovatno nedozvoljen state machine prelaz.");

                await NapisiOdgovor(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                // Sve ostalo - ne zelimo da klijent vidi stack trace ili detalje implementacije.
                _logger.LogError(ex, "Neocekivana greska.");

                await NapisiOdgovor(context, HttpStatusCode.InternalServerError,
                    "Doslo je do neocekivane greske na serveru.");
            }
        }

        private static async Task NapisiOdgovor(HttpContext context, HttpStatusCode statusCode, string poruka)
        {
            if (context.Response.HasStarted)
                return; // ne moze da se menja odgovor koji je vec poceo da se salje

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var telo = JsonSerializer.Serialize(new { greska = poruka });
            await context.Response.WriteAsync(telo);
        }
    }
}
