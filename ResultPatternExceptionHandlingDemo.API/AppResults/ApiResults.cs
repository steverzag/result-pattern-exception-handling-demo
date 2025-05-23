using ResultPatternExceptionHandlingDemo.API.AppResults.Errors;
using ResultPatternExceptionHandlingDemo.API.Exceptions;

namespace ResultPatternExceptionHandlingDemo.API.AppResults
{
	public static class ApiResults
	{
		
		public static IResult ToProblem(this Exception ex)
		{
			return Results.Problem
				(
					title: GetTitle(ex),
					detail: GetDetail(ex),
					statusCode: GetStatusCode(ex),
					type: GetType(ex),
					extensions: GetErrors(ex)
				);
		}

		public static IResult ToProblem(this Error er)
		{
			return Results.Problem
				(
					title: er.Code ?? "Server Error",
					detail: er.Description ?? "An unexpected error occurred.",
					statusCode: GetStatusCode(er),
					type: GetType(er),
					extensions: GetErrors(er)
				);
		}

		public static IResult ToProblem(
				this FluentResults.IError error)
		{
			return Results.Problem
				(
					title: GetTitle(error),
					detail: GetDetail(error),
					statusCode: GetStatusCode(error),
					type: GetType(error),
					extensions: GetErrors(error)
				);
		}

		//Exception to Problem's Helpers

		private static string GetTitle(Exception ex)
		{
			var title = ex switch
			{
				NotFoundException notFoundException => "Not Found",
				ConflictException notFoundException => "Conflict",
				BadRequestException badRequestException => "Bad Request",
				_ => "Server Error"
			};

			return title;
		}

		private static string GetDetail(Exception ex)
		{
			var detail = ex switch
			{
				NotFoundException notFoundException => notFoundException.Message,
				ConflictException conflictException => conflictException.Message,
				BadRequestException badRequestException => badRequestException.Message,
				_ => "An unexpected error occurred."
			};
			return detail;
		}

		private static Dictionary<string, object?>? GetErrors(Exception ex)
		{
			if (ex is not BadRequestException)
				return null;

			return new Dictionary<string, object?>
			{
				{ "errors", ((BadRequestException)ex).Errors }
			};
		}

		private static string GetType(Exception ex)
		{
			var type = ex switch
			{
				NotFoundException notFoundException => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
				ConflictException conflictError => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
				BadRequestException badRequestException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				_ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
			};

			return type;
		}

		private static int GetStatusCode(Exception ex)
		{
			var statusCode = ex switch
			{
				NotFoundException notFoundException => StatusCodes.Status404NotFound,
				ConflictException conflictError => StatusCodes.Status409Conflict,
				BadRequestException badRequestException => StatusCodes.Status400BadRequest,
				_ => StatusCodes.Status500InternalServerError
			};

			return statusCode;
		}

		//Error to Problem's Helpers

		private static Dictionary<string, object?>? GetErrors(Error error)
		{
			if (error is not ValidationError)
			{
				return null;
			}

			return new Dictionary<string, object?>
			{
				{ "errors", ((ValidationError)error).Errors }
			};
		}

		private static string GetType(Error error)
		{
			var statusCode = error switch
			{
				NotFoundError notFoundError => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
				ConflictError conflictError => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
				ProblemError problemError => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				ValidationError validationError => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				_ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
			};

			return statusCode;
		}

		private static int GetStatusCode(Error error)
		{
			var statusCode = error switch
			{
				NotFoundError notFoundError => StatusCodes.Status404NotFound,
				ConflictError conflictError => StatusCodes.Status409Conflict,
				ProblemError problemError => StatusCodes.Status400BadRequest,
				ValidationError validationError => StatusCodes.Status400BadRequest,
				_ => StatusCodes.Status500InternalServerError
			};

			return statusCode;
		}

		//FluentResults.IError to Problem's Helpers

		private static string GetTitle(FluentResults.IError er)
		{
			var isDomainError = typeof(FluentErrors.DomainError).IsAssignableFrom(er.GetType());

			if (isDomainError)
				return (er as FluentErrors.DomainError)!.Code;

			return "Server Error";
		}

		private static string GetDetail(FluentResults.IError er)
		{
			var isDomainError = typeof(FluentErrors.DomainError).IsAssignableFrom(er.GetType());

			if (isDomainError)
				return (er as FluentErrors.DomainError)!.Message;

			return "An unexpected error occurred.";
		}

		private static Dictionary<string, object?>? GetErrors(FluentResults.IError er)
		{
			if (er is not FluentErrors.ValidationError)
				return null;

			return new Dictionary<string, object?>
			{
				{ "errors", ((FluentErrors.ValidationError)er).Errors }
			};
		}

		private static string GetType(FluentResults.IError er)
		{
			var type = er switch
			{
				FluentErrors.NotFoundError notFoundError => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
				FluentErrors.ConflictError conflictError => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
				FluentErrors.BadRequestError badRequestError => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				FluentErrors.ValidationError validationError => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				_ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
			};

			return type;
		}

		private static int GetStatusCode(FluentResults.IError er)
		{
			var statusCode = er switch
			{
				FluentErrors.NotFoundError notFoundError => StatusCodes.Status404NotFound,
				FluentErrors.ConflictError conflictError => StatusCodes.Status409Conflict,
				FluentErrors.BadRequestError badRequestError => StatusCodes.Status400BadRequest,
				FluentErrors.ValidationError validationError => StatusCodes.Status400BadRequest,
				_ => StatusCodes.Status500InternalServerError
			};

			return statusCode;
		}
	}

}
