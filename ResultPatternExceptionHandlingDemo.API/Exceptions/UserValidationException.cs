using ResultPatternExceptionHandlingDemo.API.AppResults.Errors;

namespace ResultPatternExceptionHandlingDemo.API.Exceptions
{
	public class UserValidationException(IEnumerable<Error>? errors = null) : BadRequestException("User validation failed", errors)
	{
	}
}
