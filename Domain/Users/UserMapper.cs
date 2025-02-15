namespace HealthCare.Domain.Users
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto(
                user.Id.AsGuid(),
                user.Email.Address,
                user.Role.ToString(),
                user.Active
            );
        }
    }
}