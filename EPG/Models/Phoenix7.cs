using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace EPG.Models
{
    [XmlRoot("phoenix7")]
    public class Phoenix7
    {
        [XmlElement(ElementName = "PlanEvent", Type = typeof(PlanEvent))]
        public List<PlanEvent> listOfPlans { get; set; }
        public string Version { get; set; }
    }
}