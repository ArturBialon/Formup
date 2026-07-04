namespace Infrastructure.Access
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Role { get; }
        string? Token { get; }
        bool IsInRole(string roleName);
    }
}
