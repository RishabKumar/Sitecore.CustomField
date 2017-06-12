using EPG.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace EPG
{
    public partial class EditXML : System.Web.UI.Page
    {
        string xmlpath = "";
        public List<PlanEvent> planlist;
        public PropertyInfo[] properties;
        public Dictionary<string, List<PlanEvent>> plandic;
        public List<string> xmllist;
        protected void Page_Load(object sender, EventArgs e)
        {
            // old code
            //xmlpath = Request.QueryString["xmlpath"];
            //Phoenix7 obj;
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(Phoenix7));
            //Stream xmltream = new Sitecore.Data.Items.MediaItem(Sitecore.Configuration.Factory.GetDatabase("master").GetItem(xmlpath)).GetMediaStream();
            //properties = typeof(PlanEvent).GetProperties();
            //obj = (Phoenix7)xmlSerializer.Deserialize(xmltream);
            
            //planlist = obj.listOfPlans;

            plandic = new Dictionary<string, List<PlanEvent>>();
            xmllist = new List<string>();
            xmlpath = Request.QueryString["xmlpath"];
            Phoenix7 obj;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Phoenix7));
            var xmlfolderitem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(xmlpath);
            foreach (Item child in xmlfolderitem.Children)
            {
                Stream xmltream = new Sitecore.Data.Items.MediaItem(child).GetMediaStream();
                properties = typeof(PlanEvent).GetProperties();
                obj = (Phoenix7)xmlSerializer.Deserialize(xmltream);

                planlist = obj.listOfPlans;
                plandic.Add(child.Name, planlist);
                xmllist.Add(child.Name);
            }
            
        }

        public string GetThumbnailBytes(string name)
        {
            string base64 = "";
            byte[] bytes = null;
            if (!string.IsNullOrWhiteSpace(name))
            {
                var db = Sitecore.Configuration.Factory.GetDatabase("master");
                try
                {
                    var item = db.GetItem("/sitecore/media library/Thumbnails/" + name);
                    Stream stream = new MediaItem(item).GetMediaStream();
                    bytes = new byte[stream.Length];
                    int l = stream.Read(bytes, 0, (int)stream.Length);
                    base64 = Convert.ToBase64String(bytes);
                }
                catch(Exception e)
                {

                }
            }
            return base64;
        }

        private void save_Click(object sender, EventArgs e)
        {
            Button save = (Button)sender;
            
        }

        
    }
}