using EPG.Models;
using Sitecore.Collections;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace EPG.Commands
{
    public class CreateItemsInBucket : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length >= 1)
            {
                Item item = context.Items[0];
                if (item.TemplateID.ToString().Equals("{C59DE718-504E-4480-9D4C-9804465D4CE1}"))
                {
                    string xmlfolderpath = item.Fields["XML"].Value;
                    string parentpath = "/sitecore/content/EPG/Sections/PlanBuckets/UKPlan";
                    
                    Database db = Sitecore.Configuration.Factory.GetDatabase("master");
                    Item xmlfolderitem = db.GetItem(xmlfolderpath);
                    ChildList list = xmlfolderitem.GetChildren();
                   // IndexCustodian.PauseIndexing();
                    foreach(Item xmlitem in list)
                    {
                        if (xmlitem.Name.ToLower().Contains("asia"))
                        {
                            parentpath = "/sitecore/content/EPG/Sections/PlanBuckets/AsiaPlan";
                        }
                        else if (xmlitem.Name.ToLower().Contains("uk"))
                        {
                            parentpath = "/sitecore/content/EPG/Sections/PlanBuckets/UKPlan";
                        }
                        Phoenix7 obj;
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Phoenix7));
                        Stream xmltream = new Sitecore.Data.Items.MediaItem(xmlitem).GetMediaStream();
                        obj = (Phoenix7)xmlSerializer.Deserialize(xmltream);
                        var tmplist = obj.listOfPlans;
                        var tmppath = parentpath;
                        new Thread(() => CreatePlanItems(db, tmplist, tmppath)).Start();
                  //      Task t = Task.Run(() => CreatePlanItems(db, new List<PlanEvent>(obj.listOfPlans), new String(parentpath.ToCharArray())));
                  //      ThreadPool.QueueUserWorkItem(CreatePlanItems, params[]{db, new List<PlanEvent>(obj.listOfPlans), new String(parentpath.ToCharArray()));
                    }
                //    IndexCustodian.ResumeIndexing();
                }
                //ProgressBox.Execute("ItemCreation", "Creating items", Refresh, "Test");
                SheerResponse.Alert(" Please wait while items are being created in buckets.", false);
                
            }
        }


        public void CreatePlanItems(Database db, List<PlanEvent> planlist, string parentpath)
        {
            Item parentItem = db.GetItem(parentpath);
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

        public void Refresh(params object[] op)
        {
           
        }
      
    }
}