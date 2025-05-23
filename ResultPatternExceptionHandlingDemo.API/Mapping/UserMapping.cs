using ResultPatternExceptionHandlingDemo.API.Data.Models;
using ResultPatternExceptionHandlingDemo.API.DTOs;

namespace ResultPatternExceptionHandlingDemo.API.Mapping
{
	public static class UserMapping
	{
		public static UserDTO ToDTO(this User user)
		{
			return new UserDTO
			(
				user.Id,
				user.FirstName,
				user.LastName,
				user.Email,
				user.CreatedAt
			);
		}

		public static User ToEntity(this CreateUserRequest request)
		{
			return new User
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				CreatedAt = DateTime.UtcNow
			};
		}
	}
}
