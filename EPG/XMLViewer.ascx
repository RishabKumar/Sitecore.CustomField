<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XMLViewer.ascx.cs" Inherits="EPG.XMLViewer" %>
     
    <div>    
        <div class="Plandiv" id ="Plandiv" style="margin-top:20px">
           
        </div>
    </div>

<link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jsgrid/1.5.3/jsgrid-theme.min.css" />
<link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jsgrid/1.5.3/jsgrid.min.css" />
 
 <script type="text/javascript">
     
        (function ($) {
            var xmlfolderpath = "<%=xmlpath%>";
            var loadingimg = document.querySelector("img.loading");
           
            $.getScript("https://cdnjs.cloudflare.com/ajax/libs/jsgrid/1.5.3/jsgrid.min.js", function (data, textStatus, jqxhr) {

                if (jqxhr.status == 200) {
                    console.log("jsGrid has been loaded.");

                    // Upload file function
                    $.uploadFile = function (id, type, fileinput, img) {
                        var data = new FormData();
                        
                        var files = fileinput.get(0).files;

                        // Add the uploaded image content to the form data collection
                        if (files.length > 0) 
                        {
                            data.append("UploadedImage", files[0]);
                       

                            // Make Ajax request with the contentType = false, and procesDate = false
                            var ajaxRequest = $.ajax({
                                type: "POST",
                                url: "/api/UploadThumbnail/UploadMedia?filename=" + id + "&type=" + type,
                                contentType: false,
                                processData: false,
                                filename: id,
                                data: data
                            });
                            ajaxRequest.done(function (data, textStatus, jqXHR) {
                                // Do other operation
                                img.attr('src', "data:image/png;base64," + data);
                            });
                        }
                    };
                   
                        $("#Plandiv").jsGrid({
                           
                            filtering: true,
                            editing: false,
                            sorting: true,
                            paging: true,
                            autoload: true,
                           
                            pageSize: 15,
                            pageButtonCount: 5,

                            deleteConfirm: "Do you really want to delete the client?",
                            controller: {
                                loadData: function (filter) {
                                    var d = $.Deferred();
                                    $.ajax({
                                        type: "GET",
                                        contentType: "application/json; charset=utf-8",
                                        url: "/api/UploadThumbnail/GetXmlDataInJSON?filepath=" + xmlfolderpath + "/" + "MUTVPlan_Sample",

                                        dataType: "json",
                                        success: function (data) {
                                            d.resolve($.parseJSON(data));
                                            console.log("Grid Data loaded");
                                        },
                                        error: function (e) {
                                            console.log(e);
                                        }
                                    });
                                    //value returned is passed on to fields, accessible via item object
                                    return d.promise();
                                }
                            },
                            height: "500px",
                            width: "100%",

                            fields: [
                                {
                                    name: "Portrait Image",
                                    editButton: false,
                                    width: 200,
                                    itemTemplate: function (value, item) {
                                        var img = $("<img>").attr('src', item.PortraitImage).attr('width', '100px');
                                        var fileinput = $("<input>").attr('type', 'file').text("Browse");
                                        var test = $("<a>").text("Test").click($.abc);
                                        var $link = $("<a>").text("Upload").attr("style", "display:block").click(function () { $.uploadFile(item.PlanTitleID, "portrait", fileinput, img) });
                                        return $("<div>").append(img).append($link).append(fileinput);
                                    }
                                },
                               
                                {
                                    name: "Landscape Image",
                                    editButton: false,
                                    width: 200,
                                    itemTemplate: function (value, item) {
                                        var img = $("<img>").attr('src', item.LandscapeImage).attr('width', '100px');
                                        var fileinput = $("<input>").attr('type', 'file').text("Browse");
                                        var $link = $("<a>").text("Upload").attr("style", "display:block").click(function () { $.uploadFile(item.PlanTitleID, "landscape", fileinput, img) });
                                        return $("<div>").append(img).append($link).append(fileinput);
                                    }
                                },

                                { name: "BroadcastDate", type: "text", width: 200 },
                                { name: "PlanDate", type: "text", width: 200 },
                                { name: "PlanTime", type: "text", width: 200 },
                                { name: "PlanDuration", type: "text", width: 200 },
                                { name: "MaterialID", type: "text", width: 200 },
                                { name: "TitleName", type: "text", width: 200 },
                                { name: "EpisodeName", type: "text", width: 200 },
                                { name: "EPG", type: "text", width: 200 },
                                { name: "TitleID", type: "text", width: 200 },
                                { name: "EpisodeID", type: "text", width: 200 },
                                { name: "PlanTitleID", type: "text", width: 200 },
                                { name: "DayName", type: "text", width: 200 },
                                { name: "Definition", type: "text", width: 200 },
                                { name: "PortraitImage", type: "text", width: 200 },
                                { name: "LandscapeImage", type: "text", width: 200 },
                                { type: "control" },
                                
                            ]
                        });   
                }
            });
        })(jQuery);


 </script>
 