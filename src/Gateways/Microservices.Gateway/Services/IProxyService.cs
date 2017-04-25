using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Gateway.Services
{
    public interface IProxyService
    {
        IBoardsProxy Boards { get; }
        ICardsProxy Cards { get; }
        IUsersProxy Users { get; }
    }

}
