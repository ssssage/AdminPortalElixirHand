using AdminPortalElixirHand.Services;
using API.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;

namespace AdminPortalElixirHand
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Register the configuration section
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["AppSettings:BaseUrl"]);
            });

            services.AddHttpClient<IUserAccountService, UserAccountService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["AppSettings:BaseUrl"]);
            });

            services.AddScoped<IImageService, ImageService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .AddCookie(options =>
            //             {
            //                 options.Cookie.Name = "Secure_Cookie";
            //                 options.LoginPath = "/login";
            //                 options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
            //                 options.AccessDeniedPath = "/access-denied";
            //             });

            services.AddScoped<TokenProvider>();

            // Configure JSON options globally
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase; // Apply to dictionary keys
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
