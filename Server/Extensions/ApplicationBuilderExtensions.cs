using Application.Interfaces.Services;
using Server.Hubs;
using Shared.Constants.Application;

namespace Server.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }

            return app;
        }

        internal static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
            => app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHub<SignalRHub>(ApplicationConstants.SignalR.HubUrl);
            });
    }
}
