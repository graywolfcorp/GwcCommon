using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Gwc.Common.Testing
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

        }

        //gets called in the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {



        }

       

       
    }
}
