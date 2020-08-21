using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RssFeeder.Core.Models;
using RssFeeder.Core.Services.Abstractions;
using RssFeeder.Core.Services.Implementations;

namespace RssFeeder.Client
{
    public class Startup
    {

        private RssFeederConfiguration _rssFeederConfiguration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IRssFeedReader, RssFeedReader>();
            services.AddSingleton<SettingsProvider>();
            services.AddSingleton<RssFeederConfiguration>(p => _rssFeederConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitAppConfiguration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        public void InitAppConfiguration()
        {
            try
            {
                String xmlConfig = File.ReadAllText("config.xml");

                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlConfig);
                XmlNode settingsNode = document.SelectSingleNode("Settings");

                _rssFeederConfiguration = new RssFeederConfiguration(settingsNode);
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }
    }
}
