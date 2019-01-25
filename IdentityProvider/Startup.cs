using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityProvider.Bus;
using IdentityProvider.Domain.Handler;
using IdentityProvider.Domain.Repositories;
using IdentityProvider.Infra.Datas;
using IdentityProvider.Infra.EventBus.Consumers;
using IdentityProvider.Infra.Repositories;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<IdentityContext>(options =>
             options.UseSqlite("Data Source=identity.db"));

            services.AddMvc();


            services.AddIdentityServer()
                
                .AddTestUsers(TestUsers.Users)
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryClients(Clients.Get())
                .AddDeveloperSigningCredential();

            services.AddAuthentication()
               .AddFacebook("Facebook", options =>
               {
                   options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                   options.ClientId = "317069148888035";
                   options.ClientSecret = "7e166a31b28042e93901d248b932162c";
               });


            services.AddScoped<CreateUserConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateUserConsumer>();
            });


            services.AddSingleton(provider => MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");

                });

                cfg.ReceiveEndpoint(host, "queue_created_employee", e =>
                {
                    e.LoadFrom(provider);
                });

            }));


            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            
            services.AddSingleton<IHostedService, BusHostedService>();
           

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IUserHandler), typeof(UserHandler));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, Microsoft.AspNetCore.Hosting.IApplicationLifetime applicationLifetime)
        {
            


            loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }

    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "fastcut_api_swagger",
                    ClientName = "Swagger UI for fastcut_api",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new[] { "http://localhost:50916/api-docs/oauth2-redirect.html" },
                    AllowedScopes = { "openid", "profile", "access_admin" }
                },
                new Client
                {
                    ClientId = "fastcut_queue_api_swagger",
                    ClientName = "Swagger UI for fastcut_queue_api",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new[] { "http://localhost:3200/api-docs/oauth2-redirect.html" },
                    AllowedScopes = { "access_employee" }
                }
            };
        }
    }

    internal class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("access_admin", "Acesso FastCut Api Administrador"),
                new ApiResource("access_employee", "Acesso FastCut Queue Api Funcionário")
            };
        }
    }

    internal class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            },
            new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                }
            }
        };
    }
}
