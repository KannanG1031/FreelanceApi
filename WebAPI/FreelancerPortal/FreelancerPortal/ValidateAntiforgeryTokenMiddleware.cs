using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FreelancerPortal
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ValidateAntiforgeryTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public ValidateAntiforgeryTokenMiddleware(RequestDelegate next,IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (HttpMethods.IsPost(context.Request.Method) ||
                HttpMethods.IsGet(context.Request.Method)||
                HttpMethods.IsPut(context.Request.Method) ||
                HttpMethods.IsDelete(context.Request.Method))
            {
                await _antiforgery.ValidateRequestAsync(context); // Validate token
            }

            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ValidateAntiforgeryTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidateAntiforgeryTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateAntiforgeryTokenMiddleware>();
        }
    }
}
