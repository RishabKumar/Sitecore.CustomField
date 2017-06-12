using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPG.Models
{
    [SitecoreType(AutoMap=true)]
    public class PlanXML
    {
        public virtual string XML {get ; set;} 
    }
}