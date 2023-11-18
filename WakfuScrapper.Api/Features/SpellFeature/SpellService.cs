using WakfuScrapper.Api.Attributes;
using WakfuScrapper.Api.Commons;
using WakfuScrapper.Api.Features.ArmorFeature;
using WakfuScrapper.Api.Helpers;
using WakfuScrapper.Domain.Commons;
using WakfuScrapper.Domain.Models;

namespace WakfuScrapper.Api.Features.SpellFeature;

[ServiceAvailable(Type = ServiceType.Scoped)]
public class SpellService
{
    private readonly SpellScrapperService _service;

    public SpellService(SpellScrapperService service)
    {
        _service = service;
    }
    public async Task<Payload<List<Spell>, List<JsonResponse>>> GetSpellDetailsByUrl(string url)
    {
        try
        {
            var spellDetails = await _service.GetSpellDetailsAsync(url);
            return spellDetails;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<List<Spell>, ArmorServices>(ex);
        }
    }
}