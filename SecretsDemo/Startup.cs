using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SecretsDemo
{
    public class Startup
    {
        private IReadOnlyDictionary<string, string> FullConfig;
        public Startup(IHostingEnvironment env)
        {
             var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            AddKeyVault(builder);

            //builder.AddEnvironmentVariables();

            FullConfig = builder.Build()
                                .AsEnumerable()
                                .ToDictionary(x => x.Key, x => x.Value);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(FullConfig);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }

        private void AddKeyVault(IConfigurationBuilder builder)
        {
            var baseConfig = builder
                             //.AddEnvironmentVariables()
                             .Build();

            var thumbprint = baseConfig["AAD:Thumbprint"];
            if (!string.IsNullOrEmpty(thumbprint))
            {
                var cert = CertificateLoader.GetCertificate(thumbprint);
                if (cert != null)
                {
                    builder.AddAzureKeyVault(
                        baseConfig["Vault:Name"],
                        baseConfig["AAD:AppId"],
                        cert,
                        new SecretManager());
                }
                else
                {
                    //log it!
                }
            }
        }
    }
}
