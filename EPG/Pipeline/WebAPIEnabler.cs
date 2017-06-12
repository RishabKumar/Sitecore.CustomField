using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace EPG.Pipeline
{
    public class WebAPIEnabler
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configuration.Routes.Add("api", new HttpRoute("api/{controller}/{action}"));
        }
    }
}