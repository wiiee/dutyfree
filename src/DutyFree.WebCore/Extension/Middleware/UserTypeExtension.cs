namespace DutyFree.WebCore.Extension.Middleware
{
    using Microsoft.AspNetCore.Builder;
    using WebCore.Middleware;

    public static class UserTypeExtension
    {
        public static IApplicationBuilder UseUserType(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserTypeMiddleware>();
        }
    }
}
