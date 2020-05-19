using Microsoft.AspNetCore.Http;
using Umbraco.Net;

namespace Umbraco.Web.Common.AspNetCore
{
    public class AspNetCoreIpResolver : IIpResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreIpResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentRequestIpAddress() => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
    }
}
