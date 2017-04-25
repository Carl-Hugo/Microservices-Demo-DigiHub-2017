using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Gateway
{
    public class ServicesHostSettings
    {
        public string BoardsHost { get; set; }
        public string CardsHost { get; set; }
        public string UsersReadHost { get; set; }
        public string UsersWriteHost { get; set; }
    }
}
