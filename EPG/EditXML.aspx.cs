using EPG.Models;
using Newtonsoft.Json;
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
        public string xmlpath = "";
        public List<PlanEvent> planlist;
        public PropertyInfo[] properties;
        public List<string> xmllist;

        protected void Page_Load(object sender, EventArgs e)
        {
            xmlpath = Request.QueryString["xmlpath"];
            xmllist = new List<string>();
            if (!string.IsNullOrWhiteSpace(xmlpath))
            {
                var xmlfolderitem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(xmlpath);
                foreach (Item child in xmlfolderitem.Children)
                {
                    xmllist.Add(child.Name);
                }
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

        

        
    }
}