using AdminPortalElixirHand.Services;

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
            services.AddAutoMapper(typeof(Startup));
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            });
           

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}