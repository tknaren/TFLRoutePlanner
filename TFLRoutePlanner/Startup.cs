using TFLRoutePlanner.Utilities;

namespace TFLRoutePlanner
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
            services.Configure<ConfigSettings>(Configuration.GetSection("ConfigSettings"));

        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=RoutePlanner}/{action=Index}/{id?}");
            app.Run();

        }
    }
}
