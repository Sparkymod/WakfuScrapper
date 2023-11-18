using WakfuScrapper.Api.Commons;
using WakfuScrapper.Api.Features.ArmorFeature;

namespace WakfuScrapper.Api.Endpoints;

public static class ArmorEndpoints
{
    public static void MapArmorEndpoints(this WebApplication app)
    {
        var armorApi = app.MapGroup(ApiRoutes.Armor);
        armorApi.MapGet(ApiRoutes.ArmorBy, GetByUrl);
    }

    public static async Task<IResult> GetByUrl(string? url, ArmorServices armorService)
    {
        url = string.IsNullOrEmpty(url) ? "https://www.wakfu.com/en/mmorpg/encyclopedia/armors/12488" : url;
        var payload = await armorService.GetItemByUrl(url);

        return payload.ToResult();
    }
}