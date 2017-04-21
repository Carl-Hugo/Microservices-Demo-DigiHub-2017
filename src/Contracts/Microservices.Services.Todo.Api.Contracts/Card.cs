using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Todo.Api.Contracts
{
    public class Card
    {
        public string Id { get; set; }
        public string BoardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorEmail { get; set; }
    }
}
