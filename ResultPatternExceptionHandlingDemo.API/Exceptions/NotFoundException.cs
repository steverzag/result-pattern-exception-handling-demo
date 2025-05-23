namespace ResultPatternExceptionHandlingDemo.API.Exceptions
{
	public class NotFoundException(string message) : Exception(message)
	{
	}
}
