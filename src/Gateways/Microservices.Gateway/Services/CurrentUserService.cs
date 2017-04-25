using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microservices.Gateway.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MemoryCache _cache;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, MemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public string Id
        {
            get
            {
                var id = _cache.GetOrCreate(User, (cache) =>
                {
                    var regex = new Regex("[^a-zA-Z0-9]");
                    string formattedUserName;
                    if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                    {
                        var identifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                        // Replace | by - in "facebook|000000000000000" then clean the rest (if needed)
                        formattedUserName = regex.Replace(identifier.Replace("|", "-"), "");
                    }
                    else
                    {
                        formattedUserName = regex.Replace(User.Identity.Name, "");
                    }
                    return formattedUserName;
                });
                return id;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return Identity.IsAuthenticated;
            }
        }

        public ClaimsPrincipal User
        {
            get
            {
                return _httpContextAccessor.HttpContext.User;
            }
        }

        public IIdentity Identity
        {
            get
            {
                return User.Identity;
            }
        }
    }
}
