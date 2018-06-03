#pragma warning disable CS1572, CS1573, CS1591
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using hwpuzzles.Core.Models;
using hwpuzzles.Core.Repositories;
using hwpuzzles.Core.Services;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Text;

namespace puzzles
{
    public class Startup
    {
        ILogger Logger { get; }
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<Startup>();

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Adds services required for using options.
            services.AddOptions();
            // Register the IConfiguration instance which the Options classes bind against.
            services.Configure<PuzzleBoardGeneratorOptions>(options => Configuration.GetSection("Board").Bind(options));
            services.Configure<WordsRepositoryOptions>(options => Configuration.GetSection("Word").Bind(options));

            // Add CORS support
            services.AddCors();

            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "puzzles API",
                    Description = "A word search generator service application written in ASP.NET Core",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Kevin McWhirter", Email = "", Url = "https://github.com/klmcwhirter/puzzle-service" },
                    License = new License { Name = "Use under MIT", Url = "https://github.com/klmcwhirter/puzzle-service/blob/master/LICENSE" }
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "hw-puzzles.xml");
                c.IncludeXmlComments(xmlPath);
            });

            Container = AddToAutofac(services);

#if MOVE_TO_HW_PB_CACHE
            // Start worker threads filling the cache
            Task.Factory.StartNew(
                () => Container.Resolve<PuzzleBoardCacheManager>()?.FillQueues(false),
                TaskCreationOptions.LongRunning);
#endif

            var rc = new AutofacServiceProvider(Container);
            return rc;
        }

        private IContainer AddToAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // Add Redis services
            var redisUrl = GetRedisUrl();
            builder.Register<IRedisClientsManager>(c => new PooledRedisClientManager(redisUrl)).As<IRedisClientsManager>();
            builder.Register<IRedisTypedClient<Puzzle>>(c =>
            {
                var mgr = c.Resolve<IRedisClientsManager>();
                var client = mgr.GetClient();
                var typedClient = client.As<Puzzle>();
                return typedClient;
            }).As<IRedisTypedClient<Puzzle>>();

            // Add application repositories.
            builder.RegisterType<RedisPuzzlesRepository>().As<IPuzzlesRepository>();
            builder.RegisterType<TagsRepository>().As<ITagsRepository>();
            builder.RegisterType<TopicsRepository>().As<ITopicsRepository>();

            builder.RegisterType<WordsRepository>().As<IWordsRepository>().SingleInstance();

            // Add application services.
            builder.RegisterType<CharacterGenerator>().As<ICharacterGenerator>();

            builder.RegisterType<PuzzleBoardGenerator>().As<IGenerator<PuzzleBoard>>();
            builder.RegisterType<PuzzleWordGenerator>().As<IGenerator<IList<string>>>();

            builder.Populate(services);
            var rc = builder.Build();
            return rc;
        }

        private string GetRedisUrl()
        {
            var password64 = Configuration.GetValue<string>("REDIS_PASSWORD");
            var password = password64; // Encoding.UTF8.GetString(Convert.FromBase64String(password64));
            var redisHost = Configuration.GetValue<string>("REDIS_SERVICE_HOST");
            var redisPort = Configuration.GetValue<string>("REDIS_SERVICE_PORT");
            var rc = $"{password}@{redisHost}:{redisPort}";
            return rc;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Startup.cs(88,13): warning CS0618: 'ConsoleLoggerExtensions.AddConsole(ILoggerFactory, IConfiguration)' is obsolete: 'This method is obsolete and will be removed in a future version. The recommended alternative is to call the Microsoft.Extensions.Logging.AddConsole() extension method on the Microsoft.Extensions.Logging.LoggerFactory instance.' [/Users/klmcw/src/github.com/klmcwhirter/puzzle-service/puzzles/puzzles.csproj]
            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            // Startup.cs(89,13): warning CS0618: 'DebugLoggerFactoryExtensions.AddDebug(ILoggerFactory)' is obsolete: 'This method is obsolete and will be removed in a future version. The recommended alternative is to call the Microsoft.Extensions.Logging.AddDebug() extension method on the Microsoft.Extensions.Logging.LoggerFactory instance.' [/Users/klmcw/src/github.com/klmcwhirter/puzzle-service/puzzles/puzzles.csproj]
            // loggerFactory.AddDebug();

            app.UseCors(builder =>
            {
                var originsString = Configuration.GetValue<string>("PUZZLE_SERVICE_ORIGINS") ?? "http://huntwords";
                Logger.LogInformation($"Configured to use these CORS origins={originsString}");

                var origins = originsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                builder.WithOrigins(origins);

                builder.WithMethods("GET", "POST", "PUT", "DELETE");

                builder.AllowAnyHeader();

                builder.AllowCredentials();
            });

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "puzzles API V1");
            });
        }
    }
}

