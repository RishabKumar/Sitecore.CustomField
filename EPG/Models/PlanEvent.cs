using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPG.Models
{
    [SitecoreType(AutoMap=true)]
    public class PlanEvent
    {
        [SitecoreField(FieldName = "Broadcast Date")]
        public string BroadcastDate { get; set; }

        [SitecoreField(FieldName = "Plan Date")]
        public string PlanDate { get; set; }

        [SitecoreField(FieldName = "Plan Time")]
        public string PlanTime { get; set; }

        [SitecoreField(FieldName = "Plan Duration")]
        public string PlanDuration { get; set; }

        [SitecoreField(FieldName = "Material ID")]
        public string MaterialID { get; set; }

        [SitecoreField(FieldName = "Title Name")]
        public string TitleName { get; set; }

        [SitecoreField(FieldName = "Episode Name")]
        public string EpisodeName { get; set; }
        public string EPG { get; set; }

        [SitecoreField(FieldName = "Title ID")]
        public string TitleID { get; set; }

        [SitecoreField(FieldName = "Episode ID")]
        public string EpisodeID { get; set; }

        [SitecoreField(FieldName = "Plan Title ID")]
        public string PlanTitleID { get; set; }

        [SitecoreField(FieldName = "Day Name")]
        public string DayName { get; set; }

        public string Definition { get; set; }
        [SitecoreField(FieldName = "Portrait Image")]
        public string PortraitImage { get; set; }

        [SitecoreField(FieldName = "Landscape Image")]
        public string LandscapeImage { get; set; }

        
        public byte[] PortraitImageByte { get; set; }

        
        public byte[] LandscapeImageByte { get; set; }
    }

    public class PlanEventQueryable : SearchResultItem
    {
        public string BroadcastDate { get; set; }
        public string PlanDate { get; set; }
        public string PlanTime { get; set; }
        public string PlanDuration { get; set; }
        public string MaterialID { get; set; }
        public string TitleName { get; set; }
        public string EpisodeName { get; set; }
        public string EPG { get; set; }
        public string TitleID { get; set; }
        public string EpisodeID { get; set; }
        public string PlanTitleID { get; set; }
        public string DayName { get; set; }
        public string Definition { get; set; }
        public string PortraitImage { get; set; }
        public string LandscapeImage { get; set; }
        public byte[] PortraitImageByte { get; set; }
        public byte[] LandscapeImageByte { get; set; }
    }
}