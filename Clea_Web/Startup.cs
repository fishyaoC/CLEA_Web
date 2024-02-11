using Clea_Web.Models;
using Clea_Web.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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

            //services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            //註冊services
            services.AddDbContext<dbContext>();
            services.AddTransient<BaseService>();
            services.AddTransient<RoleService>();
            services.AddTransient<AccountService>();
            services.AddTransient<AccountSettingService>();
            services.AddTransient<LoginService>();
            services.AddTransient<HomeService>();
            services.AddTransient<BookService>();
            services.AddTransient<CodeService>();
            services.AddTransient<AssignClassService>();
            services.AddTransient<AssignBookService>();
            services.AddTransient<LectorClassService>();
            services.AddTransient<LectorEvaluationService>();
            services.AddTransient<SMTPService>();
            services.AddTransient<FileService>();
            services.AddTransient<B_LectorBtnService>();
            services.AddTransient<P_LectorBtnService>();
            services.AddTransient<B_LectorAdvService>();
            services.AddTransient<P_LectorAdvService>();
            services.AddTransient<LinkService>();
            services.AddTransient<BannerService>();



            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });
            services.AddControllersWithViews();
            //Ū��appsetting
            //string TicketAuthTimeout = APUtils.ConfigData("AppConfiguration:TicketAuthTimeout");
            //double AuthTimeout = double.Parse(TicketAuthTimeout);

            Double AuthTimeout = Configuration.GetValue<double>("TimeConfig:TimeOut");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.Redirect("/Sys_Login/Index");
                    //context.Response.WriteAsJsonAsync(new ErrorViewModel { errorMessage = "您已被登出，若要繼續操作請關閉視窗，重新連結!" });
                    return Task.CompletedTask;
                };
                //options.LoginPath = new PathString("/Account/Login");
                //options.LoginPath = new PathString("/Account/UnauthorizedMsg");
                options.LogoutPath = new PathString("/Sys_Login/Index");
                //options.AccessDeniedPath = new PathString("/Account/Logout?Msg=您已被登出，若要繼續操作請關閉視窗，重新連結");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(AuthTimeout);
                options.SlidingExpiration = true;
            });

            //services.AddDbContext<DbContextPortal>();
            
            //services.AddDbContext<DCPortal>();
            services.AddTransient<AccountService, AccountService>();
            services.Configure<FormOptions>(option => 
            {
                option.ValueLengthLimit = int.MaxValue;
                option.MultipartBodyLengthLimit = 4L * 1024L * 1024L * 1024L;
                option.MultipartBoundaryLengthLimit = int.MaxValue;
                option.MultipartHeadersCountLimit = int.MaxValue;
                option.MultipartHeadersLengthLimit = int.MaxValue;
                option.BufferBodyLengthLimit = 4L * 1024L * 1024L * 1024L;
                option.BufferBody = true;
                option.ValueCountLimit = int.MaxValue;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = Int64.MaxValue;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = Int64.MaxValue;
            });
        }
    }
}
