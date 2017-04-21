using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Services.Todo.Api.Contracts
{
    public class CardData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorEmail { get; set; }
    }
}