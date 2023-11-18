using WakfuScrapper.Api.Endpoints;

namespace WakfuScrapper.Api.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    ///     Extension method to map application-specific endpoints for the WebApplication.
    /// </summary>
    /// <param name="app">The WebApplication instance for which to map the application endpoints.</param>
    public static void MapApplicationEndpoints(this WebApplication app)
    {
        app.MapArmorEndpoints();
        app.MapSpellEndpoints();
    }
}