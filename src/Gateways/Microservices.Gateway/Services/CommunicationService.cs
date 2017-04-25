using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Microservices.Todo.Boards.Api.Contracts;
using Newtonsoft.Json;

namespace Microservices.Gateway.Services
{
    public class CommunicationService : ICommunicationService
    {
        public HttpClient HttpClient { get; }
        public ServicesHostSettings Services { get; }

        public CommunicationService(HttpClient httpClient, IOptions<ServicesHostSettings> services)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
            if (services.Value == null) { throw new ArgumentNullException($"{nameof(services)}.Value"); }
            Services = services.Value;
        }

        public async Task<TContract> GetAsync<TContract>(string url)
        {
            var response = await HttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<TContract>(responseContent);
            return responseObject;
        }
    }

}