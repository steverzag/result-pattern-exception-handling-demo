using ResultPatternExceptionHandlingDemo.API.Exceptions;

namespace ResultPatternExceptionHandlingDemo.API.AppResults
{
	public static class LangExtResultExtensions
	{

		public static IResult ToOkOrProblem<T>(
				this LanguageExt.Common.Result<T> result)
		{
			return result.Match(v => Results.Ok(v),
				ex => ex.ToProblem());
		}

		public static IResult ToCreatedOrProblem<T>(
				this LanguageExt.Common.Result<T> result)
		{
			return result.Match(v => Results.Created(),
				ex => ex.ToProblem());
		}
	}
}
