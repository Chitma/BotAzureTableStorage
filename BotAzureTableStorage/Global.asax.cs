using Autofac;
using BotAzureTableStorage.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace BotAzureTableStorage
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
         
            var builder = new ContainerBuilder();

            builder.RegisterType<CustomTableLogger>().AsImplementedInterfaces().InstancePerDependency();

            builder.Update(Conversation.Container);

            GlobalConfiguration.Configure(WebApiConfig.Register);

        }
    }
}
