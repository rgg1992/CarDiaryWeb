using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarDiaryWeb.Startup))]
namespace CarDiaryWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
