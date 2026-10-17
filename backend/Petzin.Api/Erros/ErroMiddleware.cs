using Microsoft.AspNetCore.Mvc;

namespace Petzin.Api.Erros;

public class ErroMiddleware
{
    private readonly RequestDelegate _next;

    public ErroMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            await Escrever(context, ex.StatusCode, ex.Message);
        }
        catch
        {
            await Escrever(context, StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
        }
    }

    private static async Task Escrever(HttpContext context, int status, string mensagem)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var corpo = new ProblemDetails
        {
            Status = status,
            Title = mensagem,
            Detail = mensagem
        };
        await context.Response.WriteAsJsonAsync(corpo);
    }
}
