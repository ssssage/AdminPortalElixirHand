using AdminPortalElixirHand.Services;
using API.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AdminPortalElixirHand
{
    public class Startup
    {

          public Startup(IConfiguration configuration)
  {
      Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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


//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.Name = "Secure_Cookie";
//        options.LoginPath = "/login";
//        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
//        options.AccessDeniedPath = "/access-denied";
//    });

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            services.AddScoped<TokenProvider>();
            //services.AddScoped<TokenManager>();

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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