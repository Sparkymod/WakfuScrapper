using System.Threading;
using WakfuScrapper.Api.Commons;
using WakfuScrapper.Api.Helpers;
using WakfuScrapper.Domain.Commons;
using WakfuScrapper.Domain.Models;

namespace WakfuScrapper.Api.Features.ArmorFeature;

public class ArmorServices
{
    private readonly ArmorScrapperService _service;

    public ArmorServices(ArmorScrapperService service)
    {
        _service = service;
    }

    public async Task<Payload<List<Equipment>, List<JsonResponse>>> Test()
    {
        try
        {
            var equipmentUrls = new List<string>
            {
                "https://www.wakfu.com/es/mmorpg/enciclopedia/armaduras/12488-black-crow-helmet",
                // Añadir más URLs aquí
            };

            var equipments = new List<Equipment>();
            var errors = new List<JsonResponse>();

            foreach (var url in equipmentUrls)
            {
                try
                {
                    var equipment = await _service.GetEquipmentDetailsAsync(url);
                    equipments.Add(equipment);
                }
                catch (Exception ex)
                {
                    // Agregar errores a la lista con detalles
                    errors.Add(new JsonResponse { ErrorMessage = ex.Message });
                }
            }

            return equipments;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<List<Equipment>, ArmorServices>(ex);
        }
    }
}