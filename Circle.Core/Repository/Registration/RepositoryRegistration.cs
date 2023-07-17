using Circle.Core.Repository.Abstraction;
using Circle.Core.Repository.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Circle.Core.Registration
{
    public static class RepositoryRegistration
    {
        public static WebApplicationBuilder RepositoryRegDI(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            return builder;
        }
    }
}
