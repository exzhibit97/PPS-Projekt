using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Movies.Infrastructure.Data;
using Movies.Shared.Interfaces;
using System.Net.Http;

namespace Movies.Tests
{
    public abstract class TestBase
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;
        protected readonly IRepository _repository;
        protected readonly IWebHostEnvironment webHostEnvironment;
        protected readonly MoviesDbContext context;

        protected TestBase()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
    }
}
