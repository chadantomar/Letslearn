using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "GoogleOpenID";
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/denied";
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnSignedIn = async context => // click in once signed in
                        {
                            await Task.CompletedTask;
                        },
                        OnSigningIn = async context => // when click in signin
                        {
                            var claimprinicpal = context.Principal;
                            if (claimprinicpal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                            {
                                if (claimprinicpal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value == "chandan")
                                {
                                    var claimidentity = claimprinicpal.Identity as ClaimsIdentity;
                                    claimidentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                                }
                            }
                            await Task.CompletedTask;
                        },
                        OnValidatePrincipal = async context => // for each request 
                        {
                            await Task.CompletedTask;
                        }
                    };
                })
                .AddOpenIdConnect("GoogleOpenID", options =>
                {
                    options.Authority = "https://accounts.google.com";
                    options.ClientId = "34020387615-akamspkbipvi4pkfk770i75q7pd3sf73.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                    options.CallbackPath = "/auth";
                    options.SaveTokens=true;
                    options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents()
                    {
                        OnTokenValidated = async context =>
                        {

                            var claimidentity = context.Principal.Identity as ClaimsIdentity;
                            claimidentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                        }
                    };
                });
                //.AddGoogle(
                //    options =>
                //    {
                //        options.ClientId = "34020387615-akamspkbipvi4pkfk770i75q7pd3sf73.apps.googleusercontent.com";
                //        options.ClientSecret = "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";
                //        options.CallbackPath = "/auth";
                //        //options.AuthorizationEndpoint += "prompt=consent"; // this wil ask to prompt if
                //                                                           // user try to login for differnt google login
                //    }
                //);
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
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
