using EmpManager.Core.Domain;
using EmpManager.Infrastructure.RelationalDb;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration;
using EmpManager.Core.Services.CQRS.Handlers.Employees;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.DependencyInjection;

namespace EmpManager.Core.Services.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds Db related services. 
        /// </summary>
        /// <param name="services">Services collection</param>
        public static IServiceCollection AddDbServices(this IServiceCollection services)
        {
            services.AddScoped(x =>
            {
                const string MemoryDb = "MemoryDb"; // For simpilicity let us not use config for memory db.
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                .UseLazyLoadingProxies()
                                                .UseInMemoryDatabase(MemoryDb);

                var dbContext = new ApplicationDbContext(options.Options);
                dbContext.Database.EnsureCreated();
                return dbContext;
            });
            return services;
        }

        /// <summary>
        /// Adds business services.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <returns>Services collection.</returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddAutoMapper(cfg => cfg.AddExpressionMapping(), typeof(GetAllEmployeesHandler).Assembly)
                .AddMediatRServices();
        }

        /// <summary>
        /// Add MediatRServices.
        /// </summary>
        /// <param name="services">Services collection.</param>
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
            => services.AddMediatR(x=> x.RegisterServicesFromAssemblyContaining<GetEmployeeByIdHandler>());
    }
}
