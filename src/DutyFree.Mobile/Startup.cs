namespace DutyFree.Mobile
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Platform.Context;
    using Platform.Setting;
    using Service.Media;
    using Service.Order;
    using Service.Product;
    using Service.Promote;
    using Service.Standard;
    using Service.User;
    using System.Collections.Generic;
    using System.Globalization;
    using WebCore.Context;
    using WebCore.Extension.Middleware;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Setting.Instance.Init(Configuration);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                }); ;

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddCors();

            services.AddSwaggerGen();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //ToDo: move to Autofac Ioc and scoped lifetime
            services.AddSingleton<IContextRepository, WebContextRepository>();
            services.AddSingleton<UserService>();
            services.AddSingleton<AirportService>();
            services.AddSingleton<FlightService>();
            services.AddSingleton<ImgKlService>();
            services.AddSingleton<ImgService>();
            services.AddSingleton<OrderService>();
            services.AddSingleton<PartnerOrderService>();
            services.AddSingleton<PartnerProductService>();
            services.AddSingleton<ReviewService>();
            services.AddSingleton<PromoteRuleService>();
            services.AddSingleton<BrandService>();
            services.AddSingleton<CategoryService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<FlightService>();
            services.AddSingleton<PropertyService>();
            services.AddSingleton<RegionService>();
            services.AddSingleton<OldRegionService>();
            services.AddSingleton<UnitService>();
            services.AddSingleton<DownloadRecordService>();
            services.AddSingleton<PreferenceService>();

            //// Add Autofac
            //var containerBuilder = new ContainerBuilder();

            //var container = containerBuilder.Build();
            //return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(builder => builder.AllowAnyOrigin().WithMethods("GET", "POST", "HEAD").AllowAnyHeader().AllowCredentials());

            app.UseStaticFiles();

            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                }
            };

            app.UseRequestLocalization(requestLocalizationOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                AccessDeniedPath = new PathString("/Home"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseSession();
            //app.UseUserType();

            app.UseContext();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();
        }
    }
}
