using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ResultPatternExceptionHandlingDemo.API.Exceptions
{
	internal sealed class ExceptionHandler : IExceptionHandler
	{

		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			var problemDetails = exception switch
			{
				NotFoundException nf => new ProblemDetails
				{
					Status = StatusCodes.Status404NotFound,
					Title = "Not Found",
					Detail = nf.Message,
					Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
				},
				ConflictException nf => new ProblemDetails
				{
					Status = StatusCodes.Status409Conflict,
					Title = "Conflict",
					Detail = nf.Message,
					Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
				},
				BadRequestException br => new ProblemDetails
				{
					Status = StatusCodes.Status400BadRequest,
					Title = "Bad Request",
					Detail = br.Message,
					Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
					Extensions = new Dictionary<string, object?>
					{
						{ "errors", br.Errors }
					}
				},
				_ => new ProblemDetails
				{
					Status = StatusCodes.Status500InternalServerError,
					Title = "Server Error",
					Detail = "An unexpected error occurred.",
					Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
				}
			};

			httpContext.Response.StatusCode = problemDetails.Status!.Value;
			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

			return true;
		}
	}
}
