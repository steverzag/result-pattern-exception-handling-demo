namespace ResultPatternExceptionHandlingDemo.API.Exceptions
{
	public class UserNotFoundException() : NotFoundException("User not found")
	{
	}
}
