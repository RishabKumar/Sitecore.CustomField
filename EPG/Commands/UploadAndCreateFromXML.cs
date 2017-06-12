using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPG.Commands
{
    public class UploadAndCreateFromXML : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length >= 1)
            {
                //var parameters = new Sitecore.Text.UrlString();
                //parameters.Add("parentid", context.Items[0].ID.ToString());
                
                //SheerResponse.Eval("window.open('http://www.google.com ', '_blank')");
              //  Sitecore.Web.UI.HtmlControls.
                 
           //     SheerResponse.Insert("XML Upload", "xmluploadtag", new UploadXML());
                context.Parameters.Add("parentid", context.Items[0].ID.ToString());
                Sitecore.Context.ClientPage.Start(this, "Run", context.Parameters);
        //        SheerResponse.Insert("XML Upload", "xmluploadtag", "<h1>FORM UPLOAD TEST</h1>");
            }
        }
        protected static void Run(ClientPipelineArgs args)
        {
            if (!args.IsPostBack)
            {
                UrlString url = new UrlString("/Upload.aspx");
                SheerResponse.ShowModalDialog(url.ToString()+"?parentid="+args.Parameters["parentid"], "400", "400","TEST", true);
            
                    
            }
            else
            {
                bool b = args.HasResult;
                if(b)
                {
                    SheerResponse.ClosePopups(true);
                }
                
            }
        }
    }
}