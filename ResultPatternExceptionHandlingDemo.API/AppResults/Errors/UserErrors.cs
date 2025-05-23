namespace ResultPatternExceptionHandlingDemo.API.AppResults.Errors
{
	public class UserErrors
	{
		public static readonly Error FirstNameInvalid = new($"User.{nameof(FirstNameInvalid)}", "User's first name is invalid");
		public static readonly Error LastNameInvalid = new($"User.{nameof(LastNameInvalid)}", "User's last name is invalid");
		public static readonly Error EmailInvalid = new($"User.{nameof(EmailInvalid)}", "User's email is invalid");
		public static readonly NotFoundError NotFound = new("User could not be found");
		public static readonly ConflictError ExistingEmail = new("User with same email alerady exists");
	}
}
