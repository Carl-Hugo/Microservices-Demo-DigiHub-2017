using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Todo.Boards.Api.Models
{
    public class BoardEntity : TableEntity
    {
        public string Name { get; set; }
        public string OwnerId { get; set; }
    }
}
