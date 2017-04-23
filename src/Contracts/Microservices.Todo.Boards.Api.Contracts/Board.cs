using System;
using System.ComponentModel.DataAnnotations;

namespace Microservices.Todo.Boards.Api.Contracts
{
    public class Board
    {
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string OwnerId { get; set; }
    }
}
