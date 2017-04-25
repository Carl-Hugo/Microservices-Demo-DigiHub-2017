using System;
using System.Linq;
using System.Collections.Generic;
using Microservices.Gateway.Contracts.Boards;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservices.Gateway.Services
{
    public interface ICommunicationService
    {
        HttpClient HttpClient { get; }
        ServicesHostSettings Services { get; }
        Task<TContract> GetAsync<TContract>(string url);
    }
}