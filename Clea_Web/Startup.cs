using Clea_Web.Models;
using Clea_Web.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Clea_Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddDbContext<dbContext>();
            services.AddTransient<BaseService>();
            services.AddTransient<RoleService>();
            services.AddTransient<AccountService>();
            services.AddTransient<LoginService>();
            services.AddTransient<HomeService>();


            ////加入Mapper
            //var mapperConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new AllMapperProfile());
            //    mc.AddProfile(new FileServiceProfile());
            //    mc.AddProfile(new WSB02Profile());
            //    mc.AddProfile(new WSB03Profile());
            //    mc.AddProfile(new WSB04Profile());
            //    mc.AddProfile(new WSB08Profile());
            //    mc.AddProfile(new WSB09Profile());
            //    mc.AddProfile(new WSB10Profile());
            //});
            //IMapper mapper = mapperConfig.CreateMapper();
            //services.AddSingleton(mapper);

            //services.AddControllers().AddJsonOptions(
            //    options =>
            //    {
            //        //options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            //        //options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
            //        options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
            //    }
            //    );

            //    .Add(options =>
            //{
            //    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            //    options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
            //});

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter());
            //}).AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter())).AddControllersAsServices();
            services.AddControllersWithViews();
            //Ū��appsetting
            //string TicketAuthTimeout = APUtils.ConfigData("AppConfiguration:TicketAuthTimeout");
            //double AuthTimeout = double.Parse(TicketAuthTimeout);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 403;
                    //context.Response.WriteAsJsonAsync(new ErrorViewModel { errorMessage = "您已被登出，若要繼續操作請關閉視窗，重新連結!" });
                    return Task.CompletedTask;
                };
                //options.LoginPath = new PathString("/Account/Login");
                //options.LoginPath = new PathString("/Account/UnauthorizedMsg");
                options.LogoutPath = new PathString("/Account/Logout?Msg=您已被登出，若要繼續操作請關閉視窗，重新連結");
                //options.AccessDeniedPath = new PathString("/Account/Logout?Msg=您已被登出，若要繼續操作請關閉視窗，重新連結");
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(AuthTimeout);
                options.SlidingExpiration = true;
            });

            //services.AddDbContext<DbContextPortal>();
            
            //services.AddDbContext<DCPortal>();
            services.AddTransient<AccountService, AccountService>();

        }
    }
}
