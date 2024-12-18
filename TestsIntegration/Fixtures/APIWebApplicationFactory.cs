﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace TestsIntegration.Fixtures
{
    public class APIWebApplicationFactory : WebApplicationFactory<Program>
    {

        public IConfiguration Configuration { get; set; }

        public APIWebApplicationFactory() : base()
        {
            //Creer base de données
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            //appsettings.integration.json
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Integrations.json")
                .Build();
                config.AddConfiguration(Configuration);
            });
        }


        protected override void Dispose(bool disposing)
        {
            //Clean la base de donnée
            base.Dispose(disposing);
        }
    }
}
