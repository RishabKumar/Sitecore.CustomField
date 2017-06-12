using EPG.Models;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace EPG
{
    public partial class Upload : System.Web.UI.Page
    {
        protected string parentid = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            parentid = Request.QueryString["parentid"];
        }
        protected void XML_UploadButton_Click(object sender, EventArgs e)
        {
            if (!XML_FileUpload.HasFile) return;
            Phoenix7 obj;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Phoenix7));
            using (var fs = XML_FileUpload.FileContent)
            {
                obj = (Phoenix7)xmlSerializer.Deserialize(fs);
            }
            List<PlanEvent> planlist = obj.listOfPlans;
            CreatePlanItems(planlist, new ID(parentid));
        }

        public void CreatePlanItems(List<PlanEvent> planlist, ID parentid)
        {
            Database db = Sitecore.Configuration.Factory.GetDatabase("master");
            Item parentItem = db.GetItem(parentid);
            using (new SecurityDisabler())
            {
                parentItem.Editing.BeginEdit();
                foreach (var plan in planlist)
                {
                    Item child = parentItem.Add(plan.PlanTitleID, new TemplateID(new ID("{50B06575-9906-4B50-8E63-3E8BBEF1F858}")));
                    child.Editing.BeginEdit();
                    LookupField lf = child.Fields["Broadcast Date"];
                    lf.Value = HttpUtility.HtmlDecode(plan.BroadcastDate);

                    lf = child.Fields["Plan Date"];
                    lf.Value = HttpUtility.HtmlDecode(plan.PlanDate);

                    lf = child.Fields["Plan Time"];
                    lf.Value = plan.PlanTime;

                    lf = child.Fields["Plan Duration"];
                    lf.Value = plan.PlanDuration;

                    lf = child.Fields["Material ID"];
                    lf.Value = plan.MaterialID;

                    lf = child.Fields["Title Name"];
                    lf.Value = plan.TitleName;

                    lf = child.Fields["Episode Name"];
                    lf.Value = plan.EpisodeName;

                    lf = child.Fields["EPG"];
                    lf.Value = plan.EPG;

                    lf = child.Fields["Title ID"];
                    lf.Value = plan.TitleID;

                    lf = child.Fields["Episode ID"];
                    lf.Value = plan.EpisodeID;

                    lf = child.Fields["Plan Title ID"];
                    lf.Value = plan.PlanTitleID;

                    lf = child.Fields["Day Name"];
                    lf.Value = plan.DayName;

                    lf = child.Fields["Definition"];
                    lf.Value = plan.Definition;

                    child.Editing.EndEdit();
                }
                parentItem.Editing.EndEdit();
            }
        }


        [XmlRoot("phoenix7")]
        public class Phoenix7
        {
            [XmlElement(ElementName = "PlanEvent", Type = typeof(PlanEvent))]
            public List<PlanEvent> listOfPlans { get; set; }
            public string Version { get; set; }
        }

    }
}