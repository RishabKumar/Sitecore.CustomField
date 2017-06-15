using Sitecore;
using Sitecore.Data;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
 
 

namespace EPG.CustomField
{
    public class XMLField : Sitecore.Shell.Applications.ContentEditor.File, Sitecore.Shell.Applications.ContentEditor.IContentField
    {
        
        HtmlInputText obj { get; set; }
        string CustomControlAsString { get; set; }
        private string itemid = "";

        public string ItemID
        {
            get
            {
                return itemid;
            }
            set
            {
                itemid = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                if (Page.IsPostBack)
                {
                  
                    //obj = new HtmlInputText();
                    //obj.ID = "XMLFilePathID";
                    //obj.Visible = true;
                    //obj.Value = Value;
                    //Controls.Add(obj);
                    base.OnLoad(e);
                }
            }
        }

        public new string GetValue()
        {
            Value = ((Sitecore.Shell.Applications.ContentEditor.File)(FindControl(this.ID))).Value;
            return Value;

        }

        public new void SetValue(string value)
        {
            Value = value;
        }

        protected override void Render(HtmlTextWriter output)
        {
            var page = new System.Web.UI.Page();
            var xmltable = (XMLViewer)page.LoadControl("/XMLViewer.ascx");
            xmltable.Visible = true;
            xmltable.ID = "XMLViewerID";
            xmltable.xmlpath = Value;
            page.Controls.Add(xmltable);
            string htmlstring = string.Empty;

            using(var sw = new StringWriter())
            {
                System.Web.HttpContext.Current.Server.Execute(page, sw, false);
                htmlstring = sw.ToString();
            }

            output.Write(htmlstring);
            output.Flush();
            base.Render(output);
        }

        public override void HandleMessage(Message message)
        {
            string messageText;
            if ((messageText = message.Name) == null)
            {
                return;
            }

            //if (messageText.Trim() == "item:save")
            //{
                
            //}

            if (messageText.Trim() == "XMLField:maximize")
            {
                var nvc = new NameValueCollection();
                nvc.Add("xmlpath", Value);
                Sitecore.Context.ClientPage.Start(this, "Run", nvc);
            }

            base.HandleMessage(message);
        }

        protected static void Run(ClientPipelineArgs args)
        {
            if (!args.IsPostBack)
            {
                UrlString url = new UrlString("/XMLFullPageViewer.aspx");
                SheerResponse.ShowModalDialog(url.ToString() + "?xmlpath=" + args.Parameters["xmlpath"], "1200", "600", "XML Viewer", true);
            }
            else
            {
                bool b = args.HasResult;
                SheerResponse.ClosePopups(true);          
            }
        }
    }
}