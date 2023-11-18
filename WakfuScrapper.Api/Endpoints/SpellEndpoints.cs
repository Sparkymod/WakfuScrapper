using WakfuScrapper.Api.Commons;
using WakfuScrapper.Api.Features.SpellFeature;

namespace WakfuScrapper.Api.Endpoints;

public static class SpellEndpoints
{
    public static void MapSpellEndpoints(this WebApplication app)
    {
        var spellApi = app.MapGroup(ApiRoutes.Spell);
        spellApi.MapGet(ApiRoutes.SpellBy, GetByUrl);
    }

    public static async Task<IResult> GetByUrl(string? url, SpellService spellService)
    {
        url = string.IsNullOrEmpty(url) ? "https://www.wakfu.com/es/mmorpg/enciclopedia/clases/1-feca/6972" : url;
        var payload = await spellService.GetSpellDetailsByUrl(url);

        return payload.ToResult();
    }
}