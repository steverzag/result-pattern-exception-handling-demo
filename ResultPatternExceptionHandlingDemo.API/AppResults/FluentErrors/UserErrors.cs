using FluentResults;

namespace ResultPatternExceptionHandlingDemo.API.AppResults.FluentErrors
{
	public class UserErrors
	{
		public static readonly DomainError FirstNameInvalid = new($"User.{nameof(FirstNameInvalid)}", "User's first name is invalid");
		public static readonly DomainError LastNameInvalid = new($"User.{nameof(LastNameInvalid)}", "User's last name is invalid");
		public static readonly DomainError EmailInvalid = new($"User.{nameof(EmailInvalid)}", "User's email is invalid");
		public static readonly NotFoundError NotFound = new("User could not be found");
		public static readonly ConflictError ExistingEmail = new("User with same email alerady exists");
	}
}

