namespace DutyFree.WebCore.Extension.Middleware
{
    using Microsoft.AspNetCore.Builder;
    using WebCore.Middleware;

    public static class ContextExtension
    {
        public static IApplicationBuilder UseContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContextMiddleware>();
        }
    }
}
