using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using ForEvolve.Azure.Storage.Table;
using Microservices.Users.Api.Contracts;
using Newtonsoft.Json;

namespace Microservices.Users.Read.Api
{
    public class UserEntity : TableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}