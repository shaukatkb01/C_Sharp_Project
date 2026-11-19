namespace SupplyBranch.Web.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        string UserName { get; }

        string FullName { get; }

        bool IsAuthenticated { get; }
    }
}
