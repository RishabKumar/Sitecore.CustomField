using EPG.Models;
using Glass.Mapper.Sc;
using Newtonsoft.Json;
using Sitecore.Buckets.Managers;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPG.Service
{
    public class ShowsDataController : ApiController
    {
        // GET: api/ShowsData
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ShowsData/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public string GetCurrentWeekShows(string today)
        {
            var list = GetItemsFromBucket(today);
            return JsonConvert.SerializeObject(list);
        }

        private List<PlanEvent> GetItemsFromBucket(string today)
        {
            var list = new List<PlanEvent>();

          //  string today = DateTime.Now.ToString("dd/MM/yyyy");//9/1/2015"
          //  today = "29/05/2017";
            var todaydate = DateTime.ParseExact(today, "dd/MM/yyyy", null);
            var endofweekdate = todaydate.AddDays(7.0d);

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
                            var tmpdate = DateTime.ParseExact(planevent.BroadcastDate, "dd/MM/yyyy", null);
                            if (tmpdate.CompareTo(todaydate) >= 0 && tmpdate.CompareTo(endofweekdate) <= 0)
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


    }
}
