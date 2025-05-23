using FluentResults;

namespace ResultPatternExceptionHandlingDemo.API.AppResults.FluentErrors
{
	public class DomainError : Error
	{
		public string Code { get; }
		public DomainError(string code, string description) : base(description) 
		{
			Code = code;
		}
	}

	public class NotFoundError : DomainError
	{
		public NotFoundError(string description) : base("Not Found", description) { }
	}

	public class BadRequestError : DomainError
	{
		public BadRequestError(string description) : base("Bad Request", description) { }
	}

	public class ConflictError : DomainError
	{
		public ConflictError(string description) : base("Conflict", description) { }
	}

	public class ValidationError : DomainError
	{
		public IEnumerable<DomainError> Errors { get; }
		public ValidationError(string description, IEnumerable<DomainError>? errors = null) : base("Bad Request", description) 
		{
			Errors = errors ?? [];
		}
	}
}
