using EquipmentRentalSystem.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using EquipmentRentalSystem.Data;
using EquipmentRentalSystem.Services;
using EquipmentRentalSystem.ViewModels;
using Microsoft.AspNetCore.Components;


namespace EquipmentRentalSystem
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<MessageHelper>();
            
            string connectionString = "Server=localhost;Database=test;User=root;Password=123456;";

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 0, 0)),
                mysqlOptions => mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null))
        );

            builder.Services.AddTransient<GenericityService>();
            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<CustomerViewModel>();
            builder.Services.AddTransient<EquipmentViewModel>();
            builder.Services.AddTransient<RentalViewModel>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
