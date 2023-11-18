using System;
using System.Threading;
using WakfuScrapper.Api.Attributes;
using WakfuScrapper.Api.Commons;
using WakfuScrapper.Api.Helpers;
using WakfuScrapper.Domain.Commons;
using WakfuScrapper.Domain.Models;

namespace WakfuScrapper.Api.Features.ArmorFeature;

[ServiceAvailable(Type = ServiceType.Scoped)]
public class ArmorServices
{
    private readonly ArmorScrapperService _service;

    public ArmorServices(ArmorScrapperService service)
    {
        _service = service;
    }

    public async Task<Payload<Armor, List<JsonResponse>>> GetItemByUrl(string url)
    {
        try
        {
            var equipment = await _service.GetEquipmentDetailsAsync(url);
            return equipment;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<Armor, ArmorServices>(ex);
        }
    }
}