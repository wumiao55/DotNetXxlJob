using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using DotNetXxlJob.Config;
using DotNetXxlJob.DefaultHandlers;
using DotNetXxlJob.Queue;

namespace DotNetXxlJob.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddXxlJobExecutor(this IServiceCollection services, IConfiguration configuration, params Type[] implementationJobHandlerTypes)
        {
            services.AddLogging();
            services.AddOptions();
            services.Configure<XxlJobExecutorOptions>(configuration.GetSection("xxlJob"))
                .AddXxlJobExecutorServiceDependency();
            services.AddAutoRegistry();
            services.AddDefaultXxlJobHandlers();
            if (implementationJobHandlerTypes != null && implementationJobHandlerTypes.Count() > 0)
            {
                foreach (var jobHandlerType in implementationJobHandlerTypes)
                {
                    services.AddSingleton(typeof(IJobHandler), jobHandlerType);
                }
            }
            return services;
        }

        public static IServiceCollection AddXxlJobExecutor(this IServiceCollection services, Action<XxlJobExecutorOptions> configAction)
        {
            services.AddLogging();
            services.AddOptions();
            services.Configure(configAction).AddXxlJobExecutorServiceDependency();
            return services;
        }

        public static IServiceCollection AddDefaultXxlJobHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IJobHandler, SimpleHttpJobHandler>();
            return services;
        }

        public static IServiceCollection AddAutoRegistry(this IServiceCollection services)
        {
            services.AddSingleton<IExecutorRegistry, ExecutorRegistry>()
                .AddSingleton<IHostedService, JobsExecuteHostedService>();
            return services;
        }

        private static IServiceCollection AddXxlJobExecutorServiceDependency(this IServiceCollection services)
        {

            //可在外部提前注册对应实现，并替换默认实现
            services.TryAddSingleton<IJobLogger, JobLogger>();
            services.TryAddSingleton<IJobHandlerFactory, DefaultJobHandlerFactory>();
            services.TryAddSingleton<IExecutorRegistry, ExecutorRegistry>();

            services.AddHttpClient("DotXxlJobClient");
            services.AddSingleton<JobDispatcher>();
            services.AddSingleton<TaskExecutorFactory>();
            services.AddSingleton<XxlRestfulServiceHandler>();
            services.AddSingleton<CallbackTaskQueue>();
            services.AddSingleton<AdminClient>();
            services.AddSingleton<ITaskExecutor, TaskExecutors.BeanTaskExecutor>();

            return services;
        }

    }
}