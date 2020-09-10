using System;
using System.Collections.Generic;
using System.Text;
using Analogy.AspNetCore.LogProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Analogy.LogServer.Tests
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddAnalogyLogger(new AnalogyLoggerConfiguration
            {
                LogLevel = LogLevel.Trace,
                EventId = 0,
                AnalogyServerUrl = "http://localhost:6000"
            });
        }
    }
}
