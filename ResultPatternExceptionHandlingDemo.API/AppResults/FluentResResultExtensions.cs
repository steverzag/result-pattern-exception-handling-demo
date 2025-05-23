using FluentResults;
using Microsoft.AspNetCore.Mvc.Formatters;
using ResultPatternExceptionHandlingDemo.API.AppResults.FluentErrors;

namespace ResultPatternExceptionHandlingDemo.API.AppResults
{
	public static class FluentResResultExtensions
	{
		public static void Match(
				this FluentResults.Result result,
				Action onSuccess,
				Action<IError> onFailure)
		{
			if (result.IsSuccess)
			{
				onSuccess();
				return;
			}

			onFailure(result.Errors[0]);
		}

		public static void Match<TIn>(
				this FluentResults.Result<TIn> result,
				Action<TIn> onSuccess,
				Action<IError> onFailure)
		{
			if (result.IsSuccess)
			{
				onSuccess(result.Value);
				return;
			}

			onFailure(result.Errors[0]);
		}

		public static TOut Match<TOut>(
				this FluentResults.Result result,
				Func<TOut> onSuccess,
				Func<IError, TOut> onFailure)
		{
			return result.IsSuccess
				? onSuccess()
				: onFailure(result.Errors[0]);
		}

		public static TOut Match<TIn, TOut>(
				this FluentResults.Result<TIn> result,
				Func<TIn, TOut> onSuccess,
				Func<IError, TOut> onFailure)
		{
			return result.IsSuccess
				? onSuccess(result.Value)
				: onFailure(result.Errors[0]);
		}

		public static IResult ToOkOrProblem(
				this FluentResults.Result result)
		{
			if (result.IsSuccess)
			{
				return Results.Ok();
			}

			var error = result.Errors[0];

			return error.ToProblem();
		}

		public static IResult ToOkOrProblem<T>(
				this FluentResults.Result<T> result)
		{
			if (result.IsSuccess)
			{
				return Results.Ok(result.Value);
			}

			var error = result.Errors[0];

			return error.ToProblem();
		}

		public static IResult ToCreatedOrProblem<T>(
				this FluentResults.Result<T> result)
		{
			if (result.IsSuccess)
			{
				return Results.Created();
			}

			var error = result.Errors[0];
			return error.ToProblem();
		}
	}
}
