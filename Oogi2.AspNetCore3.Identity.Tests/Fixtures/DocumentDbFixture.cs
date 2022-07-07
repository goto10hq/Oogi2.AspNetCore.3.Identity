using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Oogi2.AspNetCore3.Identity.Tests.Fixtures
{
    public class DocumentDbFixture : IDisposable
    {
        public Connection Connection { get; }

        public DocumentDbFixture()
        {
            var appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("oogi2")
                .AddEnvironmentVariables()
                .Build();

            Connection = new Connection(appSettings["endpoint"], appSettings["authorizationKey"], appSettings["database"], appSettings["collection"], "/partitionKey");            
        }
        
        public void Dispose()
        {            
        }
    }
}