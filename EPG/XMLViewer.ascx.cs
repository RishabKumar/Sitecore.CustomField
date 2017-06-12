using EPG.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EPG
{
    public partial class XMLViewer : System.Web.UI.UserControl
    {
        public string xmlpath = "";
        //public List<PlanEvent> planlist;
        //public PropertyInfo[] properties;
        //public List<string> xmllist;

        protected void Page_Load(object sender, EventArgs e)
        {
            //xmllist = new List<string>();
            //if (!string.IsNullOrWhiteSpace(xmlpath))
            //{
            //    var xmlfolderitem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(xmlpath);
            //    foreach (Item child in xmlfolderitem.Children)
            //    {
            //        xmllist.Add(child.Name);
            //    }
            //}
        }

        //public string GetThumbnailBytes(string name)
        //{
        //    string base64 = "";
        //    byte[] bytes = null;
        //    if (!string.IsNullOrWhiteSpace(name))
        //    {
        //        var db = Sitecore.Configuration.Factory.GetDatabase("master");
        //        try
        //        {
        //            var item = db.GetItem("/sitecore/media library/Thumbnails/" + name);
        //            Stream stream = new MediaItem(item).GetMediaStream();
        //            bytes = new byte[stream.Length];
        //            int l = stream.Read(bytes, 0, (int)stream.Length);
        //            base64 = Convert.ToBase64String(bytes);
        //        }
        //        catch(Exception e)
        //        {

        //        }
        //    }
        //    return base64;
        //}
    }
}