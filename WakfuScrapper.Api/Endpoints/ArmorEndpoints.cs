using WakfuScrapper.Api.Commons;
using WakfuScrapper.Api.Features.ArmorFeature;

namespace WakfuScrapper.Api.Endpoints;

public static class ArmorEndpoints
{
    public static void MapArmorEndpoints(this WebApplication app)
    {
        var armorApi = app.MapGroup(ApiRoutes.Armor);
        armorApi.MapGet("", GetAll);
    }

    public static async Task<IResult> GetAll(ArmorServices armorService)
    {
        var payload = await armorService.Test();
        return payload.ToResult();
    }
}