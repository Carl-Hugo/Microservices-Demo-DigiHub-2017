using System;
using System.ComponentModel.DataAnnotations;

namespace Microservices.Users.Api.Contracts
{
    public class User
    {
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
    }
}
