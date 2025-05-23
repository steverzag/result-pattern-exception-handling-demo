using ResultPatternExceptionHandlingDemo.API.AppResults;
using ResultPatternExceptionHandlingDemo.API.DTOs;
using ResultPatternExceptionHandlingDemo.API.Endpoints.Configuration;
using ResultPatternExceptionHandlingDemo.API.Services;

namespace ResultPatternExceptionHandlingDemo.API.Endpoints
{
	public class UserEndpoints : IEndpoints
	{
		public void RegisterEndpoints(IEndpointRouteBuilder builder)
		{
			var group1 = builder
				.MapGroup("/v-throw/users")
				.WithTags("UsersV1");

			var group2 = builder
				.MapGroup("/v-fail/users")
				.WithTags("UsersV2");

			var group3 = builder
				.MapGroup("/v-lang-ext/users")
				.WithTags("UsersV3");

			var group4 = builder
				.MapGroup("/v-fluent-res/users")
				.WithTags("UsersV4");

			group1.MapGet("/{id}", GetUserByIdOrThrow).WithName("UserByIdV1");
			group1.MapPost("/", CreateUserOrThrow);


			group2.MapGet("/{id}", GetUserByIdOrFail).WithName("UserByIdV2");
			group2.MapPost("/", CreateUserOrFail);

			group3.MapGet("/{id}", GetUserByIdOrFailLangExt).WithName("UserByIdV3");
			group3.MapPost("/", CreateUserOrFailLangExt);

			group4.MapGet("/{id}", GetUserByIdOrFailFluentRes).WithName("UserByIdV4");
			group4.MapPost("/", CreateUserOrFailFluentRes);
		}

		private async static Task<IResult> GetUserByIdOrThrow(int id, UserService userService)
		{
			var user = await userService.GetUserByIdOrThrowAsync(id);
			if (user is null)
			{
				return Results.NotFound("user not found");
			}

			return Results.Ok(user);
		}

		private async static Task<IResult> CreateUserOrThrow(CreateUserRequest request, UserService userService)
		{
			var userId = await userService.CreateUserOrThrowAsync(request);
			return Results.CreatedAtRoute("UserByIdV1", routeValues: new { id = userId });
		}

		private async static Task<IResult> GetUserByIdOrFail(int id, UserService userService)
		{
			var result = await userService.GetUserByIdOrFailAsync(id);

			return result.ToOkOrProblem();
		}

		private async static Task<IResult> CreateUserOrFail(CreateUserRequest request, UserService userService)
		{
			var result = await userService.CreateUserOrFailAsync(request);

			return result.Match
				(userId => Results.CreatedAtRoute("UserByIdV2",
					routeValues: new { id = userId }),
				er => er.ToProblem());
		}

		private async static Task<IResult> GetUserByIdOrFailLangExt(int id, UserService userService)
		{
			var result = await userService.GetUserByIdOrFailLangExtAsync(id);

			return result.ToOkOrProblem();
		}

		private async static Task<IResult> CreateUserOrFailLangExt(CreateUserRequest request, UserService userService)
		{
			var result = await userService.CreateUserOrFailLangExtAsync(request);

			return result.Match(userId => Results.CreatedAtRoute("UserByIdV3",
					routeValues: new { id = userId }),
				ex => ex.ToProblem());
		}

		private async static Task<IResult> GetUserByIdOrFailFluentRes(int id, UserService userService)
		{
			var result = await userService.GetUserByIdOrFailFluentResAsync(id);

			return result.ToOkOrProblem();
		}

		private async static Task<IResult> CreateUserOrFailFluentRes(CreateUserRequest request, UserService userService)
		{
			var result = await userService.CreateUserOrFailFluentResAsync(request);

			return result.Match(userId => Results.CreatedAtRoute("UserByIdV4",
					routeValues: new { id = userId }),
				er => er.ToProblem());
		}
	}
}
