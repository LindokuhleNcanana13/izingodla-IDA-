using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IDA.Startup))]

namespace IDA
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();    
        }
    }
}
