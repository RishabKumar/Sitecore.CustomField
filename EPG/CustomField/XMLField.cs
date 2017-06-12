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
    [SupportsEventValidation]
    
    public class XMLField : Sitecore.Web.UI.HtmlControls.Control, Sitecore.Shell.Applications.ContentEditor.IContentField
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
                    obj = new HtmlInputText();
                    obj.ID = "XMLFilePathID";
                    obj.Visible = true;
                    obj.Value = Value;
                  
                    
                    Controls.Add(obj);

                   

                    base.OnLoad(e);
                }
            }
        }

        public string GetValue()
        {
            Value = ((System.Web.UI.HtmlControls.HtmlInputText)(FindControl("XMLFilePathID"))).Value;
            return Value;

        }

        public void SetValue(string value)
        {
            Value = value;
        }

        protected override void Render(HtmlTextWriter output)
        {
            var page = new System.Web.UI.Page();
            var xmltable = (XMLViewer)page.LoadControl("/XMLViewer.ascx");
            xmltable.Visible = true;
            xmltable.ID = "XMLViewerID";
            xmltable.xmlpath = obj.Value;
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
 

            if (messageText.Trim() == "item:save")
            {
                var c = FindControl("XMLViewerID");
                var f = c.FindControl("Plandiv");
            }

            if (messageText.Trim() == "editxml:open")
            {
                var nvc = new NameValueCollection();
                nvc.Add("xmlpath", Value);
                Sitecore.Context.ClientPage.Start(this, "Run", nvc);
            }

            if (messageText.Trim() == "contentimage:open")
            {
                //Sitecore.Context.ClientPage.Dispatch("contentimage:open");
                  var url = new UrlString("/sitecore/shell/Applications/Content Manager/default.aspx");
                  var header = string.Empty;
                  var applicationItem = Client.CoreDatabase.GetItem("{7B2EA99D-BA9D-45B8-83B3-B38ADAD50BB8}");
                  if (applicationItem != null)
                  {
                    header = applicationItem["Display name"];
                  }
                  if (header.Length == 0)
                  {
                    header = "Media Library";
                  }
                  url.Add("he", header);
                  url.Add("pa", "1");
                  url.Add("ic", "Applications/16x16/photo_scenery.png");
                  url.Add("mo", "media");
                  url.Add("ro", ItemIDs.MediaLibraryRoot.ToString());
                  SheerResponse.Eval("window.open('" + url + "', 'MediaLibrary', 'location=0,resizable=1')");
            }
            base.HandleMessage(message);
        }

        protected static void Run(ClientPipelineArgs args)
        {
            if (!args.IsPostBack)
            {
                UrlString url = new UrlString("/EditXML.aspx");
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