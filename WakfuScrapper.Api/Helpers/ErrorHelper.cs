using Serilog;
using WakfuScrapper.Api.Commons;
using WakfuScrapper.Domain.Commons;

namespace WakfuScrapper.Api.Helpers;

public class ErrorHelper
{
    public static Payload<TData, List<JsonResponse>> LogExceptionAndReturnError<TData, T>(Exception ex)
    {
        Log.Logger.ForContext("Payload", typeof(T).Name).Error(ex, $"{typeof(T).Name} was caught!");

        var message = ex.InnerException is not null
            ? ex.InnerException.Message
            : ex.Message;

        var errors = new List<JsonResponse>
        {
            new ()
            {
                ErrorCode = StatusCodes.Status500InternalServerError.ToString(),
                ErrorMessage = message
            }
        };

        return Payload<TData, List<JsonResponse>>.Failure(errors, true);
    }
}