using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Gateway.Services
{
    public class ProxyService : IProxyService
    {
        public IBoardsProxy Boards { get; }
        public ICardsProxy Cards { get; }
        public IUsersProxy Users { get; }

        public ProxyService(IBoardsProxy boardsProxy, ICardsProxy cardsProxy, IUsersProxy usersProxy)
        {
            Boards = boardsProxy ?? throw new ArgumentNullException(nameof(boardsProxy));
            Cards = cardsProxy ?? throw new ArgumentNullException(nameof(cardsProxy));
            Users = usersProxy ?? throw new ArgumentNullException(nameof(usersProxy));
        }
    }
}