using ResultPatternExceptionHandlingDemo.API.AppResults.Errors;

namespace ResultPatternExceptionHandlingDemo.API.Exceptions
{
	public class BadRequestException(string message, IEnumerable<Error>? errors = null) : Exception(message)
	{
		public IEnumerable<Error> Errors { get; } = errors ?? [];
	}
}
