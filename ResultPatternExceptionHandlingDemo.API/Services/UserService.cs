using Microsoft.EntityFrameworkCore;
using ResultPatternExceptionHandlingDemo.API.AppResults;
using ResultPatternExceptionHandlingDemo.API.AppResults.Errors;
using ResultPatternExceptionHandlingDemo.API.Data;
using ResultPatternExceptionHandlingDemo.API.DTOs;
using ResultPatternExceptionHandlingDemo.API.Exceptions;
using ResultPatternExceptionHandlingDemo.API.Mapping;

namespace ResultPatternExceptionHandlingDemo.API.Services
{
	public class UserService
	{
		private readonly AppDBContext _dbContext;

		public UserService(AppDBContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<Result<UserDTO>> GetUserByIdOrFailAsync(int id)
		{ 
			var user = await _dbContext.Users.FindAsync(id);
			if (user is null)
			{
				return Result.Failure<UserDTO>(UserErrors.NotFound);
			}

			return user.ToDTO();
		}

		public async Task<Result<int>> CreateUserOrFailAsync(CreateUserRequest request)
		{
			var errors = ValidateUser(request);
			if (errors.Any())
			{
				var validationError = new ValidationError("User validation failed", errors);
				return Result.Failure<int>(validationError);
			}

			var existingUser = await _dbContext.Users
				.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (existingUser is not null)
			{
				return Result.Failure<int>(UserErrors.ExistingEmail);
			}

			var user = request.ToEntity();
			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();

			return user.Id;
		}

		public async Task<LanguageExt.Common.Result<UserDTO>> GetUserByIdOrFailLangExtAsync(int id)
		{
			var user = await _dbContext.Users.FindAsync(id);
			if (user is null)
			{
				return new LanguageExt.Common.Result<UserDTO>(new UserNotFoundException());
			}

			return user.ToDTO();
		}

		public async Task<LanguageExt.Common.Result<int>> CreateUserOrFailLangExtAsync(CreateUserRequest request)
		{
			var errors = ValidateUser(request);
			if (errors.Any())
			{
				return new LanguageExt.Common.Result<int>(new UserValidationException(errors));
			}

			var existingUser = await _dbContext.Users
				.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (existingUser is not null)
			{
				return new LanguageExt.Common.Result<int>(new ConflictException("User with same email already exists"));
			}

				var user = request.ToEntity();
			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();

			return user.Id;
		}

		public async Task<FluentResults.Result<UserDTO>> GetUserByIdOrFailFluentResAsync(int id)
		{
			var user = await _dbContext.Users.FindAsync(id);
			if (user is null)
			{
				return FluentResults.Result.Fail<UserDTO>(AppResults.FluentErrors.UserErrors.NotFound);
			}

			return user.ToDTO();
		}

		public async Task<FluentResults.Result<int>> CreateUserOrFailFluentResAsync(CreateUserRequest request)
		{
			var errors = ValidateUser(request)
				.Select(e => new AppResults.FluentErrors.DomainError(e.Code, e.Description));

			if (errors.Any())
			{
				var validationError = new AppResults.FluentErrors.ValidationError("User validation failed", errors);
				return FluentResults.Result.Fail<int>(validationError);
			}

			var existingUser = await _dbContext.Users
				.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (existingUser is not null)
			{
				return FluentResults.Result.Fail<int>(AppResults.FluentErrors.UserErrors.ExistingEmail);
			}

			var user = request.ToEntity();
			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();

			return user.Id;
		}

		public async Task<UserDTO?> GetUserByIdOrThrowAsync(int id)
		{
			var user = await _dbContext.Users.FindAsync(id);
			if (user is null)
			{
				throw new UserNotFoundException();
			}

			return user.ToDTO();
		}

		public async Task<int> CreateUserOrThrowAsync(CreateUserRequest request)
		{
			var errors = ValidateUser(request);

			if(errors.Any())
			{
				throw new UserValidationException(errors);
			}

			var existingUser = await _dbContext.Users
				.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (existingUser is not null)
			{
				throw new ConflictException("User with same email already exists");
			}

			var user = request.ToEntity();
			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();

			return user.Id;
		}

		private IEnumerable<Error> ValidateUser(CreateUserRequest request)
		{ 
			var errors = new List<Error>();

			if (string.IsNullOrWhiteSpace(request.FirstName))
			{
				errors.Add(UserErrors.FirstNameInvalid);
			}

			if (string.IsNullOrWhiteSpace(request.LastName))
			{
				errors.Add(UserErrors.LastNameInvalid);
			}

			if (string.IsNullOrWhiteSpace(request.Email))
			{
				errors.Add(UserErrors.EmailInvalid);
			}

			return errors;
		}
	}
}
