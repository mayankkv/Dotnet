namespace MiddlewareLogin.CustomMiddleware;

public static class MiddlewareExtentions
{
    public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoginMiddleware>();
    }
}