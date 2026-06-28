namespace Infrastructure.Access
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Role { get; }
        bool IsInRole(string roleName);
    }
}
