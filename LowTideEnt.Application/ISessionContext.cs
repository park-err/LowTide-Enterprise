namespace LowTideEnt.Application
{
    /// <summary>
    /// Lets Application-layer services access user identity without
    /// depending directly on ASP.NET Core or HttpContext.
    /// </summary>
    public interface ISessionContext
    {
        bool IsAuthenticated { get; }
        int UserId { get; }
        string UserName { get; }
        string? Email { get; }
        string? GetClaimValue(string claimType);
    }
}
