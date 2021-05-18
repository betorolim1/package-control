using Microsoft.Extensions.DependencyInjection;
using PackageControl.Core.Country.Service;
using PackageControl.Core.Handlers;
using PackageControl.Core.Handlers.Interfaces;
using PackageControl.Core.LastCheckpoint;
using PackageControl.Core.Package.Repository;
using PackageControl.Data.Dao;
using PackageControl.Data.Repository;
using PackageControl.Data.Service;
using PackageControl.Query.Handlers;
using PackageControl.Query.Handlers.Interface;
using PackageControl.Query.Package.Dao.Interfaces;
using PackageControl.Shared;

namespace PackageControl
{
    public static class ConfigureDI
    {
        public static void ConfigureDIs(this IServiceCollection service)
        {
            AddHandlers(service);
            AddDaos(service);
            AddRepositories(service);
            AddDB(service);
            AddService(service);
        }

        private static void AddService(IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
        }

        private static void AddDB(IServiceCollection services)
        {
            services.AddScoped<DbSession>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
        
        private static void AddHandlers(IServiceCollection services)
        {
            services.AddScoped<IPackageQueryHandler, PackageQueryHandler>();
            services.AddScoped<IPackageHandler, PackageHandler>();
        }

        private static void AddDaos(IServiceCollection services)
        {
            services.AddScoped<IPackageDao, PackageDao>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IPackagesRepository, PackageRepository>();
            services.AddScoped<ILastCheckpointRepository, LastCheckPointRepository>();
        }
    }
}
