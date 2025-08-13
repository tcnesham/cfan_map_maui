using Microsoft.Extensions.DependencyInjection;

namespace CFAN.SchoolMap.Helpers
{
    public static class ServiceHelper
    {
        public static IServiceProvider? Services { get; set; }

        public static T? GetService<T>() where T : class
        {
            return Services?.GetService<T>();
        }

        public static T GetRequiredService<T>() where T : class
        {
            if (Services == null)
                throw new InvalidOperationException("Services not initialized");
            
            return Services.GetRequiredService<T>();
        }
    }
}
