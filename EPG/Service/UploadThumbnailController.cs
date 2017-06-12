using EPG.Models;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace EPG.Service
{
    public class UploadThumbnailController : ApiController
    {
        // GET: api/UploadThumbnail
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public Database db = Sitecore.Configuration.Factory.GetDatabase("master");

        [HttpGet]
        public PlanEvent GetThumbnailBytes(PlanEvent obj)
        {
            byte[] bytes = null;
            if (obj != null)
            {
                var itemBuckets = db.GetItem("/sitecore/content/EPG/Sections/PlanBuckets/AsiaPlan/");
                if (itemBuckets != null && BucketManager.IsBucket(itemBuckets))
                {
                    using (var searchContext = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().Where(x => x.Name.Equals(obj.PlanTitleID) && x.TemplateId== new ID("{50B06575-9906-4B50-8E63-3E8BBEF1F858}")).FirstOrDefault();
                        if (result != null)
                        {
                            if(!string.IsNullOrWhiteSpace(result.GetItem().Fields["Portrait Image"].Value))
                            {
                                MediaItem mediaitem = ((Sitecore.Data.Fields.ImageField)result.GetItem().Fields["Portrait Image"]).MediaItem;
                                Stream stream = new MediaItem(mediaitem).GetMediaStream();
                                bytes = new byte[stream.Length];
                                int l = stream.Read(bytes, 0, (int)stream.Length);
                                obj.PortraitImageByte = bytes;
                            }
                            if (!string.IsNullOrWhiteSpace(result.GetItem().Fields["Landscape Image"].Value))
                            {
                                MediaItem mediaitem = ((Sitecore.Data.Fields.ImageField)result.GetItem().Fields["Landscape Image"]).MediaItem;
                                Stream stream = new MediaItem(mediaitem).GetMediaStream();
                                bytes = new byte[stream.Length];
                                int l = stream.Read(bytes, 0, (int)stream.Length);
                                obj.LandscapeImageByte = bytes;
                            }
                        }
                    }
                } 
            }
            return obj;
        }

        public void SetThumbnailPath(string PlanTitleId, Item mi, string type)
        {
            if (!string.IsNullOrWhiteSpace(PlanTitleId))
            {
                var itemBuckets = db.GetItem("/sitecore/content/EPG/Sections/PlanBuckets/AsiaPlan/");
                if (itemBuckets != null && BucketManager.IsBucket(itemBuckets))
                {
                    using (var searchContext = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().Where(x => x.Name.Equals(PlanTitleId) && x.TemplateId == new ID("{50B06575-9906-4B50-8E63-3E8BBEF1F858}")).FirstOrDefault();
                        if (result != null && mi != null)
                        {
                            using (new SecurityDisabler())
                            {
                                Item item = result.GetItem();
                                item.Editing.BeginEdit();
                                if (type.Equals("portrait"))
                                {
                                    ((Sitecore.Data.Fields.ImageField)item.Fields["Portrait Image"]).MediaID = mi.ID;
                               //     ((Sitecore.Data.Fields.ImageField)item.Fields["Portrait Image"]).UpdateLink(mi.Links.GetAllLinks()[0]);
                                }
                                else
                                {
                                    ((Sitecore.Data.Fields.ImageField)item.Fields["Landscape Image"]).MediaID = mi.ID;
                               //     ((Sitecore.Data.Fields.ImageField)item.Fields["Landscape Image"]).Value = mi.Parent.Paths.FullPath;
                                }
                                item.Editing.EndEdit();
                            }
                        }
                    }
                }
            }
        }

        public void GetThumbnailPath(PlanEvent obj)
        {
            if (!string.IsNullOrWhiteSpace(obj.PlanTitleID))
            {
                var itemBuckets = db.GetItem("/sitecore/content/EPG/Sections/PlanBuckets/AsiaPlan/");
                if (itemBuckets != null && BucketManager.IsBucket(itemBuckets))
                {
                    using (var searchContext = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().Where(x => x.Name.Equals(obj.PlanTitleID) && x.TemplateId == new ID("{50B06575-9906-4B50-8E63-3E8BBEF1F858}")).FirstOrDefault();
                        if (result != null)
                        {
                            Item item = result.GetItem();
                            if(item == null)
                            {
                                return;
                            }
                            var portraitimage = (Sitecore.Data.Fields.ImageField)item.Fields["Portrait Image"];
                            var landscape = (Sitecore.Data.Fields.ImageField)item.Fields["Landscape Image"];
                            if (portraitimage != null && (portraitimage).MediaItem != null)
                            {
                                string imagepath = ConvertSitecorePathtoMediaMasterPath((portraitimage).MediaItem.Paths.ContentPath);
                                obj.PortraitImage = imagepath;
                            }
                            if (landscape != null && (landscape).MediaItem != null)
                            {
                                string imagepath = ConvertSitecorePathtoMediaMasterPath((landscape).MediaItem.Paths.ContentPath);
                                obj.LandscapeImage = imagepath;
                            }
                        }
                    }
                }
            }
        }

        private string ConvertSitecorePathtoMediaMasterPath(string url)
        {
            var path = "";
            if(!string.IsNullOrWhiteSpace(url))
            {
                var tmp= url.ToLower().Split(new string[]{"media library"}, StringSplitOptions.RemoveEmptyEntries)[1];
                path = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Url.Host + "/-/media" + tmp;
            }
            return path;
        }

        [HttpPost]
        public async Task<byte[]> UploadMedia(string filename, string type)
        {
            Item item = null;
            byte[] bytes = null;
            ImageInfo imgobj = null;
            try
            {
                imgobj = await Process();
                bytes = imgobj.Bytes;
                var fullMediaPath = "/sitecore/media library/Thumbnails";
                var language = Sitecore.Globalization.Language.Parse("en");
                using (new SecurityDisabler())
                {
                    var db = Sitecore.Configuration.Factory.GetDatabase("master");
                    var options = new MediaCreatorOptions();
                    options.FileBased = false;
                    options.IncludeExtensionInItemName = false;
                    options.KeepExisting = false;
                    options.Versioned = true;
                    options.Destination = fullMediaPath;
                    options.Database = db;
                    options.Language = language;
                    
                    var creator = new MediaCreator();
                    var fileStream = new MemoryStream(bytes);
                    var parentitem = db.GetItem(fullMediaPath, language);
                    if (parentitem != null)
                    {
                        item = GetMediaItem(fullMediaPath + "/" + filename + "-" + type, db);
                        if (item == null)
                        {
                            item = parentitem.Add(filename + "-" + type, new Sitecore.Data.TemplateID(new ID("{F1828A2C-7E5D-4BBD-98CA-320474871548}")));
                            var mediaItem = new MediaItem(item);
                            var media = MediaManager.GetMedia(mediaItem);
                            media.SetStream(fileStream, imgobj.Extension);

                            item.Editing.BeginEdit();
                            item.Fields["Title"].Value = filename + "-" + type;
                            item.Editing.EndEdit();
                            item.Database.Engines.HistoryEngine.RegisterItemCreated(item);
                            //pass filename as it is same as plantitleid
                            SetThumbnailPath(filename, mediaItem, type);
                        }
                        else
                        {
                            var mediaItem = new MediaItem(item);
                            var media = MediaManager.GetMedia(mediaItem);
                             //pass filename as it is same as plantitleid
                            media.SetStream(fileStream, imgobj.Extension);
                            item.Database.Engines.HistoryEngine.RegisterItemSaved(item, new ItemChanges(item));
                        }
                    }
                }
            }
            catch(Exception e)
            {
                
            }
            return bytes;
        }

        private async Task<ImageInfo> Process()
        {
            byte[] bytes = null;
            ImageInfo imgobj = null;
            try
            {
                bytes = null;
                var files = HttpContext.Current.Request.Files;
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                bytes = null;
                foreach (var file in provider.Contents)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    bytes = await file.ReadAsByteArrayAsync();
                    string[] tmp = filename.Split('.');
                    imgobj = new ImageInfo()
                    {
                        Bytes = bytes,
                        Name = tmp[0],
                        Extension = tmp[1]
                    };
                }
                
            }
            catch(Exception e)
            {
                string s = e.Message;
            }
            return imgobj;
        }

        public class ImageInfo
        {
            public byte[] Bytes { get; set; }
            public string Name { get; set; }
            public string Extension { get; set; }
        }
       
        private bool DeleteItemByName(string itempath, Database db, int count = 0)
        {
            if (itempath.Contains("Thumbnail"))
            {
                try
                {
                    Item item = db.GetItem(itempath);
                    item.Delete();
                    return DeleteItemByName(itempath, db, ++count);
                }
                catch(Exception e)
                {

                }
            }
            else
            {
                return false;
            }
            if (count > 0)
            {
                return true;
            }
           
            return false;
        }

        private Item GetMediaItem(string path, Database db)
        {
            try
            {
                Item item = db.GetItem(path);
                if (item != null)
                {
                    return item;
                }
            }
            catch { }
            return null;
        }

        [HttpGet]
        public string GetXmlDataInJSON()
        {
            var querystringpair = Request.GetQueryNameValuePairs().First(x => x.Key.Equals("filepath"));
            var filepath = querystringpair.Value;
            var xmlitem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(filepath);
            Phoenix7 obj;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Phoenix7));
            Stream xmltream = new Sitecore.Data.Items.MediaItem(xmlitem).GetMediaStream();
            PropertyInfo[] properties = typeof(PlanEvent).GetProperties();
            obj = (Phoenix7)xmlSerializer.Deserialize(xmltream);

            obj.listOfPlans.ForEach(x => GetThumbnailPath(x));
            string json = JsonConvert.SerializeObject(obj.listOfPlans);
            return json;
        }
    }

}
