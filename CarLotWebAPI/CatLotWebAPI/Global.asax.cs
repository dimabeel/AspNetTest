using AutoLotDAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace CatLotWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Data in DB always new each application run.
            // Have to delete before production release.
            Database.SetInitializer(new MyDataInitializer());
        }
    }
}
