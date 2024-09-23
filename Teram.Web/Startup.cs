using DNTCaptcha.Core;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModuleSharedContracts.Hubs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Teram.DataAccessLayer;
using Teram.Framework.Core.Domain;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Providers.EmailHistories;
using Teram.Framework.Core.Providers.EmailHistories.Models;
using Teram.Framework.Core.Providers.Logging.Mongo;
using Teram.Framework.Core.Providers.Logging.Mongo.Models;
using Teram.Framework.Core.Service;
using Teram.Framework.Core.Tools;
using Teram.Module.Authentication.Cache;
using Teram.Module.Authentication.Constant;
using Teram.Module.Authentication.Entities;
using Teram.Module.Authentication.Factory;
using Teram.Module.Authentication.Logic;
using Teram.Module.Authentication.Models;
using Teram.Module.Authentication.Service;
using Teram.Module.Authentication.Tools;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Cache;
using Teram.Web.Core.Configurations;
using Teram.Web.Core.Helper;
using Teram.Web.Core.MiddleWares;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;
using Teram.Web.Models;
using WebEssentials.AspNetCore.Pwa;

namespace Teram.Web
{
    public class Startup
    {
        private List<ModuleInformation> _modules;
        private List<MethodInfo> jobs;
        public IConfiguration Configuration { get; }
        private IServiceCollection _services;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services.AddSignalR();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Resource & Localization 
            services.AddLocalization(x => x.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                   new CultureInfo("fa-IR"),
                   new CultureInfo("en-US"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "fa-IR");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

            });
            #endregion

            services.AddDbContext<TeramDbContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("TeramConnectionString"));
#if DEBUG
                x.EnableSensitiveDataLogging(true);
#endif
            }
            );
            services.AddDbContext<CacheContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("CacheConnectionString"), c => c.MigrationsAssembly("Teram.Web"));
#if DEBUG
                x.EnableSensitiveDataLogging(true);
#endif
            });            
            services.AddScoped(typeof(IBasePersistenceService<Token, IIdentityUnitOfWork>), typeof(TokenPersistanceService));
            services.AddScoped(typeof(IBasePersistenceService<TokenParameter, IIdentityUnitOfWork>), typeof(TokenParameterPersistanceService));
            services.AddDbContext<TeramIdentityContext>(
                x =>
                {
                    x.UseSqlServer(Configuration.GetConnectionString("IdentityConnectionString"), c => c.MigrationsAssembly("Teram.Module.Authentication"));
#if DEBUG
                    x.EnableSensitiveDataLogging(true);
#endif
                });

            services.AddScoped<IIdentityUnitOfWork>(x => x.GetRequiredService<TeramIdentityContext>());

            services.AddDistributedSqlServerCache(options =>
            {

                options.ConnectionString = Configuration.GetConnectionString("CacheConnectionString");
                options.SchemaName = "dbo";
                options.TableName = "ApplicationCache";
            });


            services.AddIdentity<TeramUser, TeramRole>(x =>
            {
                x.SignIn.RequireConfirmedAccount = false;
                x.SignIn.RequireConfirmedEmail = false;




                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 8;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false;

                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                x.Lockout.MaxFailedAccessAttempts = 5;
                x.Lockout.AllowedForNewUsers = true;

                x.User.RequireUniqueEmail = false;


            })
                .AddEntityFrameworkStores<TeramIdentityContext>()
                .AddClaimsPrincipalFactory<TeramClaimsPrincipalFactory>()
                .AddErrorDescriber<MultilanguageIdentityErrorDescriber>()
                .AddDefaultTokenProviders();


            services.AddScoped<IUserClaimsPrincipalFactory<TeramUser>, TeramClaimsPrincipalFactory>();
            services.AddScoped<UserClaimsPrincipalFactory<TeramUser, TeramRole>, TeramClaimsPrincipalFactory>();

            services.AddAuthentication()
                .AddJwtBearer("TokenBase", options =>
                {
                    var secretKey = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenSettings").GetValue<string>("Secret"));
                    options.Audience = Configuration.GetSection("TokenSettings").GetValue<string>("Audience");
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    var encryptionkey = Encoding.UTF8.GetBytes("Entekhab@EncKey_"); //must be 16 character
                    options.TokenValidationParameters = new TokenValidationParameters

                    {
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),
                    };


                });

           

            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));


            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.ClientId = Configuration.GetSection("Authentication").GetSection("Google").GetValue<string>("ClientId");
            //        options.ClientSecret = Configuration.GetSection("Authentication").GetSection("Google").GetValue<string>("ClientSecret");
            //    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ConstantPolicies.RolePermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new ManagementRoleRequirement());
                });
            });



            services.AddScoped<IAuthorizationHandler, ManagementRoleHandler>();
            services.AddSingleton<IDistributedCache, SqlServerCache>();
            services.AddSingleton<ITicketStore, DistributedCacheTicketStore>();
            services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>();




            services.AddScoped<ISecurity, Security>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            services.AddControllersWithViews();

            services.AddProgressiveWebApp();

            services.AddMvc(options => options.EnableEndpointRouting = false)

                .ConfigureApplicationPartManager(ConfigureApplicationParts)
                .AddRazorOptions(x =>
                {
                    x.ViewLocationFormats.Add("/Views/Controlpanel/{0}.cshtml");
                    x.ViewLocationFormats.Add("/Containers/{0}.cshtml");

                    x.ViewLocationFormats.Add("/wwwroot/Themes/IT/Layouts/fa-IR/Partials/{0}" + RazorViewEngine.ViewExtension);
                    x.ViewLocationFormats.Add("/wwwroot/Themes/IT/Layouts/fa-IR/Identity/{0}" + RazorViewEngine.ViewExtension);

                    x.ViewLocationFormats.Insert(0, "/wwwroot/Themes/Default/Layouts/fa-IR/{0}" + RazorViewEngine.ViewExtension);
                })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource));

                });
            
            services.AddSession(option =>
            {
                option.Cookie.IsEssential = true;
                option.Cookie.HttpOnly = true;
                option.IdleTimeout = TimeSpan.FromMinutes(15);
            });

            services.AddScoped(typeof(ILogic<>), typeof(Logic<>));
            services.AddScoped(typeof(IBusinessOperations<,,>), typeof(BusinessOperations<,,>));
            services.AddScoped(typeof(IPersistenceService<>), typeof(PersistenceService<>));

            services.AddScoped(typeof(IBaseLogic<,>), typeof(BaseLogic<,>));
            services.AddScoped(typeof(IBaseBusinessOperations<,,,>), typeof(BaseBusinessOperations<,,,>));
            services.AddScoped(typeof(IBasePersistenceService<,>), typeof(BasePersistenceService<,>));

            services.AddScoped<IUnitOfWork>(x => x.GetService<TeramDbContext>());


            services.AddMemoryCache();




            services.AddSingleton(typeof(HtmlTemplateParser));

            services.AddScoped<IUserPrincipal, UserPrincipal>();
            services.AddScoped<IUserSharedService, UserService>();
            services.AddScoped<ISignInSharedService, SignInService>();
            services.AddScoped<IRoleSharedService, RoleService>();

            services.AddActionDiscoveryService();


            #region Hangfire
            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnectionString"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }
                    )
         );

            services.AddHangfireServer();
            #endregion


            #region Swagger
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Teram API",
                    Description = "Common Teram Api",
                    TermsOfService = new Uri("https://www.partikan.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "WebTeam",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                    }
                });
                var securitySchema = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
                c.AddSecurityRequirement(securityRequirement);

                string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var documentPath = Path.Combine(rootPath, "API Documents");
                if (!Directory.Exists(documentPath))
                {
                    Directory.CreateDirectory(documentPath);
                }
                foreach (var item in Directory.GetFiles(documentPath, "*.xml"))
                {
                    c.IncludeXmlComments(item);
                }

                c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);

            });
            #endregion


            #region Logging

            services.AddSingleton<LogService>();

            services.Configure<LogstoreDatabaseSettings>(
                Configuration.GetSection(nameof(LogstoreDatabaseSettings)));

            services.AddSingleton<ILogstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<LogstoreDatabaseSettings>>().Value);

            var logConfig = Configuration.GetSection(nameof(LogstoreDatabaseSettings)).Get<LogstoreDatabaseSettings>();
            var applicationName = Configuration.GetValue<string>("ApplicationName");

            var serviceProvider = services.BuildServiceProvider();

            //            services.AddLogging(x =>
            //            {
            //                var logging = x.AddMongoLogger(new TeramLogConfiguration
            //                {
            //                    LogstoreDatabaseSettings = logConfig,
            //                    ApplicationName = applicationName,
            //                    LogAllLevels = true,
            //                }, serviceProvider);
            //#if DEBUG
            //                logging.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
            //#endif
            //            });


            #endregion



            #region Mail Config

            services.AddTransient<IMailStoreDatabaseSettings>(x => new MailStoreDatabaseSettings("MailStoreDatabaseSettings", Configuration));
            services.AddTransient<IHistoryService, HistoryService>();

            services.AddTransient<ITeramEmailSender, EmailSender>(x => new EmailSender(Configuration, "IdentityConfig", x.GetService<ILogger<MailService>>(), x.GetService<IHistoryService>()));
            #endregion

            #region DNTCaptcha
            //services.AddDNTCaptcha(options =>
            // options.UseCookieStorageProvider()
            //.ShowThousandsSeparators(false)
            //);
            services.AddDNTCaptcha(options =>
            {
                // options.UseSessionStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
                // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
                options.UseCookieStorageProvider(SameSiteMode.Strict) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                                      // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                                                                      // .UseDistributedSerializationProvider()

                // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
                // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
                // This is optional.
                .AbsoluteExpiration(minutes: 7)
                .ShowThousandsSeparators(false)
                .WithNoise(0.10f, 0.10f, 0, 0)
                .WithEncryptionKey("This is my secure key!")
                .InputNames(// This is optional. Change it if you don't like the default names.
                    new DNTCaptchaComponent
                    {
                        CaptchaHiddenInputName = "DNTCaptchaText",
                        CaptchaHiddenTokenName = "DNTCaptchaToken",
                        CaptchaInputName = "DNTCaptchaInputText"
                    })
                .Identifier("dntCaptcha")// This is optional. Change it if you don't like its default name.
                ;
            });
            #endregion


            services.AddHealthChecks();            
        }
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains(".resources"))
                return null;

            var x = AppDomain.CurrentDomain.GetAssemblies().ToList();

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            string filename = args.Name.Split(',')[0] + ".dll".ToLower();
            if (filename.StartsWith("Teram"))
            {
                var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var assemblyFiles = Directory.GetFiles(Path.Combine(currentPath, "Plugins"), filename, SearchOption.AllDirectories);
                if (assemblyFiles.Count() > 1)
                {
                    throw new Exception($"Duplicate filename => {filename}");
                }
                var plugin = assemblyFiles.Single();

                try
                {
                    return System.Reflection.Assembly.LoadFrom(plugin);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;

        }


        private void ConfigureApplicationParts(ApplicationPartManager applicationPartManager)
        {
            GlobalConfigurations.Assemblies = new List<Assembly>();
            jobs = new List<MethodInfo>();
            _modules = new List<ModuleInformation>();
            string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var pluginsPath = Path.Combine(rootPath, "Plugins");
            LoadAssemblies(applicationPartManager, pluginsPath);
            var themesPath = Path.Combine(rootPath, "Themes");
            LoadAssemblies(applicationPartManager, themesPath);

            applicationPartManager.FeatureProviders.Add(new ViewComponentFeatureProvider());
        }

        private void LoadAssemblies(ApplicationPartManager applicationPartManager, string pluginsPath)
        {
            var service = _services.BuildServiceProvider();
            var logger = service.GetRequiredService<ILogger<Program>>();
            if (Directory.Exists(pluginsPath))
            {
                var assemblyFiles = Directory.GetFiles(pluginsPath, "Teram.*.dll", SearchOption.AllDirectories);
                foreach (var assemblyFile in assemblyFiles)
                {

                    try
                    {
                        var assembly = Assembly.LoadFrom(assemblyFile);

                        var assemblyPart = new AssemblyPart(assembly);
                        var compiledPart = new CompiledRazorAssemblyPart(assembly);

                        if (!applicationPartManager.ApplicationParts.Any(x => x.Name == assemblyPart.Name))
                        {
                            applicationPartManager.ApplicationParts.Add(compiledPart);
                            applicationPartManager.ApplicationParts.Add(assemblyPart);
                        }


                        GlobalConfigurations.Assemblies.Add(assembly);
                        var manifestResourceNames = assembly.GetManifestResourceNames();

                        var entities = assembly.DefinedTypes.Where(x => typeof(EntityBase).IsAssignableFrom(x.AsType()) && !x.IsAbstract).OfType<Type>().ToList();

                        GlobalConfigurations.Entities.AddRange(entities);

                        var serviceRegistration = assembly.DefinedTypes.Where(x => x.Name == "ServiceRegistration").FirstOrDefault();
                        bool autoMigrate = true;
                        if (serviceRegistration != null)
                        {
                            var migrationProperty = serviceRegistration.GetField("AutoMigration");
                            if (migrationProperty != null)
                            {
                                autoMigrate = (bool)migrationProperty.GetValue(null);
                            }
                            var register = serviceRegistration.GetMethod("Register");
                            if (register.GetParameters().Count() == 2)
                            {
                                register.Invoke(null, new object[] { _services, Configuration });
                            }
                            else
                            {
                                register.Invoke(null, new[] { _services });
                            }
                            var jobRegister = serviceRegistration.GetMethod("JobRegister");

                            if (jobRegister != null)
                            {
                                jobs.Add(jobRegister);
                            }
                        }

                        TypeInfo moduleRegistration = assembly.DefinedTypes.Where(x => x.Name == "ModuleRegistration" || x.Name == "ModuleRegisteration").FirstOrDefault();
                        if (moduleRegistration != null)
                        {
#nullable enable
                            PropertyInfo? listProperty = moduleRegistration.GetProperty("Modules", BindingFlags.Public | BindingFlags.Static);


                            if (listProperty is not null && listProperty.GetValue(null) is List<ModuleInformation> list)
                            {

                                _modules.AddRange(list);
                            }
#nullable disable
                        }

                        var dbContext = assembly.DefinedTypes.Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsAbstract && !x.IsGenericTypeParameter).ToList();
                        if (autoMigrate)
                        {
                            dbContext.ForEach(x => ((DbContext)Activator.CreateInstance(x, Configuration)).Database.Migrate());
                        }

                        logger.LogInformation(assemblyFile + " has been added", null);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(5001, e, assemblyFile + "=>" + e.Message, null);

                    }



                }
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IModuleSharedService moduleRegisterer,
                              TeramIdentityContext TeramIdentity,
                              TeramDbContext TeramDbContext,
                              CacheContext cacheContext)
        {
            app.UseIPBlocker();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            TeramIdentity.Database.Migrate();
            TeramDbContext.Database.Migrate();
            cacheContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Request.Path = "/Home/NotFound";
                    await next();
                }
            });


            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            var cookieProvider = options.Value.RequestCultureProviders
                .OfType<CookieRequestCultureProvider>()
                .First();
            var urlProvider = options.Value.RequestCultureProviders
                .OfType<QueryStringRequestCultureProvider>().First();

            cookieProvider.Options.DefaultRequestCulture = new RequestCulture("en-US", "fa-IR");
            urlProvider.Options.DefaultRequestCulture = new RequestCulture("en-US", "fa-IR");
            cookieProvider.CookieName = "UserCulture";

            options.Value.RequestCultureProviders.Clear();
            options.Value.RequestCultureProviders.Add(cookieProvider);
            options.Value.RequestCultureProviders.Add(urlProvider);


            app.UseRequestLocalization(options.Value);



            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Add(".apk", "application/vnd.android.package-archive");

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
            app.UseModuleResource();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teram API");
            });
            app.UseHealthChecks("/health");
            app.UseRouting();

            // app.UseMiddleware<SiteRouteMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            var service = _services.BuildServiceProvider();
            #region Hangfire
            var hangfireOptions = new DashboardOptions
            {
                Authorization = new[] {
                    new HangFireDashboardAuthorization()
                },
                IgnoreAntiforgeryToken = true

            };
            app.UseHangfireDashboard("/hangfire", hangfireOptions);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // HangFire Dashboard endpoint
                endpoints.MapHangfireDashboard();
            });

            jobs.ForEach(x => x.Invoke(null, new[] { service
}));
            #endregion


            moduleRegisterer.RegisterModules(_modules);
            GlobalConfigurations.Title = Configuration.GetValue<string>("Title");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=My}/{action=Index}/{id?}");
                endpoints.MapHub<ProgressHub>("/progress");

                endpoints.MapControllerRoute(
                   name: "specificPage",
                   pattern: "Page/{url}",
                   defaults: new { controller = "Page", action = "SpecificPage" });


                endpoints.MapControllerRoute(
                  name: "dummyText",
                  pattern: "{url=*}/{id:int}/{dummyText}",
                  defaults: new { controller = "Page", action = "SpecificPage" });
                endpoints.MapControllerRoute(
                  name: "searchResult",
                  pattern: "{url=Search}/Result",
                  defaults: new { controller = "Page", action = "SpecificPage" });

                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                   name: "Simple_Route",
                   pattern: "{controller}/{id:int}",
                   defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=My}/{action=Index}/{id?}");
                endpoints.MapRazorPages();              
            });           
        }
    }
}
