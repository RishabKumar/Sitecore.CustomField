using EPG.Models;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.Buckets.Managers;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace EPG.Controllers
{
    public class PlanEventController : GlassController
    {
        // GET: PlanEvent
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult CurrentWeekShows()
        {
     //       var list = GetItemsFromBucket();
            return View();
        }


        private List<PlanEvent> GetItemsFromBucket()
        {
            var list = new List<PlanEvent>();

            string today = DateTime.Now.ToString("dd/MM/yyyy");//9/1/2015"
            today = "29/05/2017";

            var sc = new SitecoreContext();
            var itemBuckets = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/content/EPG/Sections/PlanBuckets/AsiaPlan/");
            if (itemBuckets != null && BucketManager.IsBucket(itemBuckets))
            {
                using (var searchContext = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
                {
                    var result = searchContext.GetQueryable<SearchResultItem>().Where(x => x.Path.Contains("Sections/PlanBuckets/AsiaPlan/") && x.TemplateId == new ID("{50B06575-9906-4B50-8E63-3E8BBEF1F858}"));
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            var planevent = sc.Cast<PlanEvent>(item.GetItem());
                            if (DateTime.ParseExact(planevent.BroadcastDate, "dd/MM/yyyy", null).CompareTo(DateTime.ParseExact(today, "dd/MM/yyyy", null)) > 0)
                            {
                                list.Add(sc.Cast<PlanEvent>(item.GetItem()));
                            }
                        }
                    }
                }
            }

            //sort them in ascending
            list.Sort((date1, date2) => DateTime.ParseExact(date1.BroadcastDate + " " + date1.PlanTime, "dd/MM/yyyy HH:mm", null).CompareTo(DateTime.ParseExact(date2.BroadcastDate + " " + date2.PlanTime, "dd/MM/yyyy HH:mm", null)));
                
            return list;
        }

        public ActionResult GetShowsForToday()
        {
            var list = new List<PlanEvent>();
            var sc = new SitecoreContext();
            var itemBuckets = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/content/EPG/Sections/PlanBuckets/AsiaPlan/");
            if (itemBuckets != null && BucketManager.IsBucket(itemBuckets))
            {
                using (var searchContext = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
                {
                    var result = searchContext.GetQueryable<SearchResultItem>().Where(x => x.Path.Contains("Sections/PlanBuckets/AsiaPlan/") && x.TemplateId == new ID("{50B06575-9906-4B50-8E63-3E8BBEF1F858}"));
                    if (result != null)
                    {
                        foreach(var item in result)
                            list.Add(sc.Cast<PlanEvent>(item.GetItem()));
                    }
                }
            }

            string today = DateTime.Now.ToString("dd/MM/yyyy");//9/1/2015"
            //  today = "01/12/2017";
         

            if (list.Count > 0)
            {
                //get shows after today 12:00 AM
                IEnumerable<PlanEvent> elist = list.Where(x => DateTime.ParseExact(x.BroadcastDate, "dd/MM/yyyy", null).CompareTo(DateTime.ParseExact(today, "dd/MM/yyyy", null)) > 0);
                list = new List<PlanEvent>(elist);
                //sort them in ascending
                list.Sort((date1, date2) => DateTime.ParseExact(date1.BroadcastDate + " " + date1.PlanTime, "dd/MM/yyyy HH:mm", null).CompareTo(DateTime.ParseExact(date2.BroadcastDate + " " + date2.PlanTime, "dd/MM/yyyy HH:mm", null)));
                int a = list.Count;
            }
            return View(list);
        }

        //public ActionResult GetShowsForToday()
        //{
        //    string datasourceid = RenderingContextWrapper.GetDataSource();
        //    PlanXML shows = SitecoreContext.GetItem<PlanXML>(new Guid(datasourceid));

        //    string xmlpath = shows.XML;

        //    Phoenix7 obj;
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Phoenix7));
        //    Stream xmltream = new Sitecore.Data.Items.MediaItem(Sitecore.Configuration.Factory.GetDatabase("master").GetItem(xmlpath)).GetMediaStream();
        //    PropertyInfo[] properties = typeof(PlanEvent).GetProperties();
        //    obj = (Phoenix7)xmlSerializer.Deserialize(xmltream);
        //    List<PlanEvent> list = obj.listOfPlans;
        //    string today = DateTime.Now.ToString("dd/MM/yyyy");//9/1/2015"
        //    today = "01/12/2016";
        //    //get shows after today 12:00 AM
        //    IEnumerable<PlanEvent> elist = list.Where(x => DateTime.ParseExact(x.BroadcastDate, "dd/MM/yyyy", null).CompareTo(DateTime.ParseExact(today, "dd/MM/yyyy", null)) > 0);
        //    list = new List<PlanEvent>(elist);
        //    //sort them in ascending
        //    list.Sort((date1, date2) => DateTime.ParseExact(date1.BroadcastDate+" "+date1.PlanTime, "dd/MM/yyyy HH:mm", null).CompareTo(DateTime.ParseExact(date2.BroadcastDate+" "+date2.PlanTime, "dd/MM/yyyy HH:mm", null)));
        //    int a = list.Count;
        //    return View(list);
        //}
    }
}