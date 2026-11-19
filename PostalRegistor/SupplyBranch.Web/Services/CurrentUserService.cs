using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SupplyBranch.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int UserId
        {
            get
            {
                if (!IsAuthenticated) return 0;

                string? id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (int.TryParse(id, out int result)) return result;

                return 0;
            }
        }

        public string UserName => User?.Identity?.Name ?? string.Empty;

        public string FullName => User?.FindFirst("FullName")?.Value ?? string.Empty;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
