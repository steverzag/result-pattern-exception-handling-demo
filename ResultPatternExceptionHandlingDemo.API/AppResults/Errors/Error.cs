namespace ResultPatternExceptionHandlingDemo.API.AppResults.Errors
{
	public record Error(string Code, string Description);

	public sealed record ValidationError(string ErrorMessage, IEnumerable<Error>? Errors = null) : Error("Bad Request", ErrorMessage)
	{
		public IEnumerable<Error> Errors { get; } = Errors ?? [];
	}

	public sealed record ProblemError(string ErrorMessage) : Error("Bad Request", ErrorMessage);

	public sealed record NotFoundError(string ErrorMessage) : Error("Not Found", ErrorMessage);

	public sealed record ConflictError(string ErrorMessage) : Error("Conflict", ErrorMessage);
}
