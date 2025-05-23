using ResultPatternExceptionHandlingDemo.API.AppResults.Errors;

namespace ResultPatternExceptionHandlingDemo.API.AppResults
{
	public static class ResultExtensions
	{
		public static void Match(
				this Result result,
				Action onSuccess,
				Action<Error> onFailure)
		{
			if (result.IsSuccess)
			{
				onSuccess();
				return;
			}

			onFailure(result.Error!);
		}

		public static void Match<TIn>(
				this Result<TIn> result,
				Action<TIn> onSuccess,
				Action<Error> onFailure)
		{
			if (result.IsSuccess)
			{
				onSuccess(result.Value);
				return;
			}

			onFailure(result.Error!);
		}

		public static TOut Match<TOut>(
				this Result result,
				Func<TOut> onSuccess,
				Func<Error, TOut> onFailure)
		{
			return result.IsSuccess
				? onSuccess()
				: onFailure(result.Error!);
		}

		public static TOut Match<TIn, TOut>(
				this Result<TIn> result,
				Func<TIn, TOut> onSuccess,
				Func<Error, TOut> onFailure)
		{
			return result.IsSuccess
				? onSuccess(result.Value)
				: onFailure(result.Error!);
		}

		public static IResult ToOkOrProblem<T>(
				this Result<T> result)
		{
			if (result.IsSuccess)
			{
				return Results.Ok(result.Value);
			}

			return result.Error!.ToProblem();
		}

		public static IResult ToCreatedOrProblem<T>(
				this Result<T> result)
		{
			if (result.IsSuccess)
			{
				return Results.Created();
			}

			return result.Error!.ToProblem();
		}
	}
}
