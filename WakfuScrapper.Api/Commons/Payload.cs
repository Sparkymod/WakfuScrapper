using System.Collections;
using System.Text.Json;

namespace WakfuScrapper.Api.Commons;

/// <summary>
///     Represents the result of an operation that can either succeed with
///     a value of type TData, or fail with an error of type TError.
/// </summary>
public class Payload<TData, TError>
{
	/// <summary>
	///     Gets the data of the successful operation, or default(TData) if the operation failed.
	/// </summary>
	public TData Data { get; } = default!;

	/// <summary>
	///     Gets the error of the failed operation, or default(TError) if the operation succeeded.
	/// </summary>
	public TError Error { get; } = default!;

	/// <summary>
	///     Gets a value indicating whether the operation has a critical error.
	/// </summary>
	public bool IsCritical { get; }

	/// <summary>
	///     Gets a value indicating whether the resource was created.
	/// </summary>
	public bool WasResourceCreated { get; }

	/// <summary>
	///     Gets a value indicating whether the operation succeeded.
	///     The operation is considered to have succeeded if Error is null.
	/// </summary>
	public bool IsSuccess => Error == null;

	public Payload(TData data) => Data = data;

	public Payload(TData data, bool wasResourceCreated)
	{
		Data = data;
		WasResourceCreated = wasResourceCreated;
	}

	public Payload(TError error) => Error = error;

	public Payload(TError error, bool isCritical)
	{
		Error = error;
		IsCritical = isCritical;
	}

	/// <summary>
	///     Creates a new Payload representing a successful operation with the given data.
	/// </summary>
	public static Payload<TData, TError> Success(TData value, bool wasResourceCreated = false) => new(value);

	/// <summary>
	///     Creates a new Payload representing a failed operation with the given error.
	/// </summary>
	public static Payload<TData, TError> Failure(TError error, bool isCritical = false) => new(error, isCritical);

	/// <summary>
	///     Calls the success function if the operation was successful,
	///     or the failure function if the operation failed, and returns the result.
	/// </summary>
	public TResult Match<TResult>(Func<TData, TResult> success, Func<TError, TResult> failure) => IsSuccess ? success(Data) : failure(Error);

	/// <summary>
	///     Converts the encapsulated outcome of a business operation int an appropriate HTTP response.
	///     On success operations, the method evaluate the content.
	///		If a resource is created returns Created with the Location URI.
	///		If there's content returns Ok, otherwise NoContent.
	///     On failures, it generates a '400 Bad Request' including the error.
	/// </summary>
	/// <returns></returns>
	public IResult ToResult(string locationUri = "")
	{
		return Match(
			success => success != null && (success is not ICollection collection || collection.Count > 0)
				? WasResourceCreated && !string.IsNullOrEmpty(locationUri) 
					? Results.Created(locationUri, success) 
					: Results.Ok(success) 
				: Results.NoContent(), // If is empty then no content.
			failure => IsCritical
				? Results.Json(failure, JsonSerializerOptions.Default, statusCode: StatusCodes.Status500InternalServerError)
				: Results.BadRequest(failure));

	}

	public static implicit operator Payload<TData, TError>(TData data) => new(data);
	public static implicit operator Payload<TData, TError>(TError error) => new(error);

	public IResult ToResult(Func<TData, IResult> onSuccess, Func<TError, IResult> onFailure) => Match(onSuccess, onFailure);
}