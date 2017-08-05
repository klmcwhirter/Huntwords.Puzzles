using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            // Add framework services.
            services.AddMvc();

            Container = AddToAutofac(services);

            // Start worker threads filling the cache
            Task.Run(() => Container.Resolve<PuzzleBoardGeneratorManager>()?.FillQueues(false));

            var rc = new AutofacServiceProvider(Container);
            return rc;
        }

        private IContainer AddToAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // Add application repositories.
            builder.RegisterType<PuzzlesDbContext>().AsSelf();

            builder.RegisterType<DbPuzzlesRepository>().AsSelf();
            builder.RegisterType<PuzzlesRepository>().As<IPuzzlesRepository>();
            builder.RegisterType<TagsRepository>().As<ITagsRepository>();
            builder.RegisterType<TopicsRepository>().As<ITopicsRepository>();

            builder.RegisterType<WordsRepository>().As<IWordsRepository>().SingleInstance();

            // Add application services.
            builder.RegisterType<CharacterGenerator>().As<ICharacterGenerator>();

            builder.RegisterType<PuzzleBoardCache>().AsSelf().SingleInstance();
            builder.RegisterType<PuzzleBoardGeneratorManager>().AsSelf().SingleInstance();
            builder.RegisterType<PuzzleBoardGenerator>().As<IGenerator<PuzzleBoard>>();
            builder.RegisterType<PuzzleWordGenerator>().As<IGenerator<IList<PuzzleWord>>>();

            // Word Generators
            builder.RegisterType<WordWordGenerator>().Keyed<IPuzzleKind>(WordWordGenerator.StaticKey);
            builder.RegisterType<DbPuzzleWordGenerator>().Keyed<IPuzzleKind>(DbPuzzleWordGenerator.StaticKey);
            builder.RegisterType<RandomWordGenerator>().Keyed<IPuzzleKind>(RandomWordGenerator.StaticKey);

            builder.Populate(services);
            var rc = builder.Build();
            return rc;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Startup.cs(88,13): warning CS0618: 'ConsoleLoggerExtensions.AddConsole(ILoggerFactory, IConfiguration)' is obsolete: 'This method is obsolete and will be removed in a future version. The recommended alternative is to call the Microsoft.Extensions.Logging.AddConsole() extension method on the Microsoft.Extensions.Logging.LoggerFactory instance.' [/Users/klmcw/src/github.com/klmcwhirter/puzzle-service/puzzles/puzzles.csproj]
            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            // Startup.cs(89,13): warning CS0618: 'DebugLoggerFactoryExtensions.AddDebug(ILoggerFactory)' is obsolete: 'This method is obsolete and will be removed in a future version. The recommended alternative is to call the Microsoft.Extensions.Logging.AddDebug() extension method on the Microsoft.Extensions.Logging.LoggerFactory instance.' [/Users/klmcw/src/github.com/klmcwhirter/puzzle-service/puzzles/puzzles.csproj]
            // loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}

