using System;
using Microsoft.Extensions.Configuration;

namespace DTO
{
    public static class AppConfig
    {
        public static IConfiguration Configuration { get; }

        static AppConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static string? GetOptional(string key)
        {
            string environmentKey = $"ECOS_{key.Replace(":", "__")}";
            string? environmentValue = Environment.GetEnvironmentVariable(environmentKey);

            return string.IsNullOrWhiteSpace(environmentValue)
                ? Configuration[key]
                : environmentValue;
        }

        public static string GetRequired(string key)
        {
            string? value = GetOptional(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Thiếu cấu hình '{key}'. Hãy dùng biến môi trường ECOS_{key.Replace(":", "__")} " +
                    "hoặc file appsettings.local.json (không commit file này)."
                );
            }

            return value;
        }
    }

}
