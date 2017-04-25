using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Microservices.Gateway.Services
{
    public interface ICurrentUserService
    {
        string Id { get; }
        bool IsAuthenticated { get; }
        ClaimsPrincipal User { get; }
        IIdentity Identity { get; }
    }
}
