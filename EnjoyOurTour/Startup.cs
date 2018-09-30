using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using Hangfire.Dashboard;
using Hangfire.States;
using EnjoyOurTour.Models;
using EnjoyOurTour.Helpers.Services;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(EnjoyOurTour.Startup))]

namespace EnjoyOurTour
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var sqlServerOptions = new Hangfire.SqlServer.SqlServerStorageOptions()
            {
                InvisibilityTimeout = new TimeSpan(0, 120, 0)
            };

            GlobalConfiguration.Configuration
                .UseSqlServerStorage("TourDB");

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = Enumerable.Empty<IDashboardAuthorizationFilter>()
            });
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate("SendBirthdayWishes", () => SendBirthdayWishes(), "0 12 * * *");
        }

        public async static Task SendBirthdayWishes()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var baseUrl = ConfigurationManager.AppSettings["AppBaseUrl"];

                    await httpClient.GetAsync(
                        "{0}/SchedulerJob/SendBirthdayWishes".F(baseUrl));
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
