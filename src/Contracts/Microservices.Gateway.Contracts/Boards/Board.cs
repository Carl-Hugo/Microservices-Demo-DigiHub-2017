using Microservices.Gateway.Contracts.Cards;
using Microservices.Gateway.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Gateway.Contracts.Boards
{
    public class Board
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public PublicUser Owner { get; set; }

        public IEnumerable<CardExcerpt> Cards { get; set; }
    }
}
