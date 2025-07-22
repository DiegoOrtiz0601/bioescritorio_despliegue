using BiomentricoHolding.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BiomentricoHolding
{
    public static class AppSettings
    {
        private static IConfigurationRoot configuration;

        static AppSettings()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string GetConnectionString(string name)
        {
            return configuration.GetConnectionString(name);
        }

        // Nuevo contexto principal apuntando a BiometricoDesarrollo
        public static BiometricoDbContext GetContextUno()
        {
            var connection = GetConnectionString("MainDbConnection");
            var options = new DbContextOptionsBuilder<BiometricoDbContext>()
                .UseSqlServer(connection)
                .Options;
            return new BiometricoDbContext(options);
        }

       
    }
}
