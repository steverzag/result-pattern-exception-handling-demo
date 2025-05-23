using ResultPatternExceptionHandlingDemo.API.AppResults.Errors;
using System.Diagnostics.CodeAnalysis;

namespace ResultPatternExceptionHandlingDemo.API.AppResults
{
	public class Result
	{
		protected Result(bool isSuccess, Error? error)
		{
			if (isSuccess && error is not null || !isSuccess && error is null)
			{
				throw new InvalidOperationException("Success result cannot have an error.");
			}

			IsSuccess = isSuccess;
			Error = error;
		}
		public bool IsSuccess { get; }
		public bool IsFailure => !IsSuccess;
		public Error? Error { get; }

		public static Result Success() => new(true, null);
		public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);
		public static Result Failure(Error error) => new(false, error);
		public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);
	}

	public class Result<TValue> : Result
	{
		public Result(TValue value, bool isSuccess, Error? error) : base(isSuccess, error)
		{
			_value = value;
		}

		private readonly TValue _value;

		[NotNull]
		public TValue Value => IsSuccess
			? _value!
			: throw new InvalidOperationException("Result is not successful. Cannot access value.");

		public static implicit operator Result<TValue>(TValue value) =>
			value is not null
			? Success(value)
			: Failure<TValue>(new Error("Null Value", "Result value can not be null"));
	}
}
