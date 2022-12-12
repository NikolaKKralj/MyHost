using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Authorization.Handlers;
using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Configurations;
using System.Reflection;

namespace MyHostAPI.Configuration
{
    public class AppModule : Autofac.Module
    {
        private readonly ConfigurationManager configuration;

        public AppModule(ConfigurationManager configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {

            var databaseSection = configuration.GetSection(DatabaseSection.Name).Get<DatabaseSection>();
            builder.RegisterType<MongoClient>().As<IMongoClient>().WithParameter("connectionString", databaseSection.ConnectionString).SingleInstance();

            // Register all services and repositories
            builder.RegisterAssemblyTypes(Assembly.Load("MyHostAPI.Business"))
              .Where(t => t.Name.EndsWith("Service"))
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("MyHostAPI.Reporting"))
              .Where(t => t.Name.EndsWith("Service"))
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load("MyHostAPI.Data"))
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load("MyHostAPI.Authorization"))
               .Where(t => t.Name.EndsWith("Validations"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(AuthorizationHandler<>))
                .As(typeof(IAuthorizationHandler<>));
        }
    }
}
