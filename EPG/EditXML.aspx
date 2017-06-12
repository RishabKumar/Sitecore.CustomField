<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditXML.aspx.cs" Inherits="EPG.EditXML" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     
    <div>
   <%--  <asp:PlaceHolder ID="XMLDataTable" runat="server" />--%>
      
        <select id="DayFilter" style="position:fixed">
            <option value="All">All Shows</option>
            <option value="Monday">Monday</option>
            <option value="Tuesday">Tuesday</option>
            <option value="Wednesday">Wednesday</option>
            <option value="Thursday">Thursday</option>
            <option value="Friday">Friday</option>
            <option value="Saturday">Saturday</option>
            <option value="Sunday">Sunday</option>
        </select>
        <select id="XMLFileFilter" style="margin-left:100px; position:fixed">
            <option value="Select">Select File</option>
            <%foreach (var file in xmllist) 
              {%>
                <option value="<%=file %>"><%=file %></option>
            <%} %>

        </select>
        
        <div class="Plandiv" id ="Plandiv" style="position:absolute; margin-top:20px">
           
        </div>
        <div style="text-align:center">
            <img src="Content/Loading_icon.gif" class="loading" style="position:fixed;display:none" />
        </div>
        
    </div>

    <div id="jsGrid"></div>
  
  
  
<script type="text/javascript">
   
    var $ = jQuery.noConflict();
    $.getScript("https://cdnjs.cloudflare.com/ajax/libs/jsgrid/1.5.3/jsgrid.min.js", function (data, textStatus, jqxhr) {
       
        if (jqxhr.status == 200)
        {
            console.log("jsGrid has been loaded.");
            $(function () {

                $("#jsGrid").jsGrid({
                    height: "90%",
                    width: "100%",

                    filtering: true,
                    editing: true,
                    sorting: true,
                    paging: true,
                    autoload: true,

                    pageSize: 15,
                    pageButtonCount: 5,

                    deleteConfirm: "Do you really want to delete the client?",

                    controller: db,

                    fields: [
                        { name: "Name", type: "text", width: 150 },
                        { name: "Age", type: "number", width: 50 },
                        { name: "Address", type: "text", width: 200 },
                        { name: "Country", type: "select", items: db.countries, valueField: "Id", textField: "Name" },
                        { name: "Married", type: "checkbox", title: "Is Married", sorting: false },
                        { type: "control" }
                    ]
                });

            });
        }
       
    });
      
    //test code above

    var xmlfolderpath = "<%=xmlpath%>";
    var loadingimg = document.querySelector("img.loading");
    function uploadFile(id, type) {        
        //var xhr = new XMLHttpRequest();                 
        //var file = document.getElementById("form"+id);
        //xhr.open("POST", "api/UploadThumbnail/UploadMedia");
        //xhr.setRequestHeader("filename", id);
        //xhr.setRequestHeader("content-type","multipart/form-data");
        //xhr.send(file);
        var data = new FormData();

        var files = $("#"+type+id).get(0).files;

        // Add the uploaded image content to the form data collection
        if (files.length > 0) {
            data.append("UploadedImage", files[0]);
        }

        // Make Ajax request with the contentType = false, and procesDate = false
        var ajaxRequest = $.ajax({
            type: "POST",
            url: "api/UploadThumbnail/UploadMedia?filename="+id+"&type="+type,
            contentType: false,
            processData: false,
            filename: id,
            data: data
        });

        ajaxRequest.done(function (data, textStatus, jqXHR ) {
            // Do other operation
            $('#'+type+id+"_img").attr('src', "data:image/png;base64," + data);
        });
     


    }

 
   
    $("#DayFilter").change(function()
    {
        var listoftables = document.querySelectorAll("div.Plandiv table tr");
        var listofheadings = document.querySelectorAll("div.PlanTables table tr.heading");
        var rows = document.querySelectorAll("tr." + this.value);
        console.log(rows);
        if(rows !== undefined && rows.length > 0)
        {
            console.log("in if");
            listoftables.forEach(function(ele){
                ele.style = "display:none";
            });
            rows.forEach(function(ele){
                ele.style = "display:table-row";
                });
        }
        else {
            console.log("in else");
            listoftables.forEach(function(ele){
                ele.style = "display:table-row";
            });
        }
        listofheadings.forEach(function(ele){
            ele.style = "display:table-row";
        });
    });

     
    $("#XMLFileFilter").change(function()
    {
        loadingimg.style = "display:inline-block";
        var ajaxRequest = $.ajax({
            type: "GET",
            url: "api/UploadThumbnail/GetXmlDataInJSON?filepath=" + xmlfolderpath + "/" + this.value,
            contentType: false,
            processData: false,
            success: function (json) {
                var parsedJson = $.parseJSON(json);
                $('#Plandiv').empty();
                $('#Plandiv').append('<table border=1 id="Plantable"></table>');
                var th = "<tr>" +
                                "<th>BroadcastDate </th>" +
                                "<th>PlanDate </th>" +
                                "<th>PlanTime </th>" +
                                "<th>PlanDuration </th>" +
                                "<th>MaterialID</th>" +
                                "<th>TitleName </th>" +
                                "<th>EpisodeName </th>" +
                                "<th>EPG </th>" +
                                "<th>TitleID </th>" +
                                "<th>EpisodeID </th>" +
                                "<th>PlanTitleID </th>" +
                                "<th>DayName </th>" +
                                "<th>Definition </th>" +
                                "<th>Portrait Image </th>" +
                                "<th>Landscape Image </th>" +
                                "</tr>";
                $('#Plantable').append(th);
                for (var i = 0; i < parsedJson.length; ++i)
                {
                    var rows = "<tr class='" + parsedJson[i].DayName + "'>" +
                        "<td>" + parsedJson[i].BroadcastDate + "</td>" +
                        "<td>" + parsedJson[i].PlanDate + "</td>" +
                        "<td>" + parsedJson[i].PlanTime + "</td>" +
                        "<td>" + parsedJson[i].PlanDuration + "</td>" +
                        "<td>" + parsedJson[i].MaterialID + "</td>" +
                        "<td>" + parsedJson[i].TitleName + "</td>" +
                        "<td>" + parsedJson[i].EpisodeName + "</td>" +
                        "<td>" + parsedJson[i].EPG + "</td>" +
                        "<td>" + parsedJson[i].TitleID + "</td>" +
                        "<td>" + parsedJson[i].EpisodeID + "</td>" +
                        "<td>" + parsedJson[i].PlanTitleID + "</td>" +
                        "<td>" + parsedJson[i].DayName + "</td>" +
                        "<td>" + parsedJson[i].Definition + "</td>" +
                        "<td>" + "<img id='portrait" + parsedJson[i].PlanTitleID + "_img'  width='100px' src='" + (parsedJson[i].PortraitImage) + "?r=" + new Date().getMilliseconds() + "' />" +
                        "<form id='form" + parsedJson[i].PlanTitleID + " enctype='multipart/form-data'>" +
                    "<input type='file' id='portrait"+parsedJson[i].PlanTitleID+"' />"+  
                    "<input type='button' onclick=uploadFile('"+parsedJson[i].PlanTitleID+"','portrait') value='Upload' />"+
                     "   </form></td>" +
                        "<td>" + "<img id='landscape" + parsedJson[i].PlanTitleID + "_img'  width='100px' src='" + (parsedJson[i].LandscapeImage) + "?r=" + new Date().getMilliseconds() + "' />" +
                        "<form id='form" + parsedJson[i].PlanTitleID + " enctype='multipart/form-data'>" +
                    "<input type='file' id='landscape" + parsedJson[i].PlanTitleID + "' />" +
                    "<input type='button' onclick=uploadFile('" + parsedJson[i].PlanTitleID + "','landscape') value='Upload' />" +
                     "   </form></td>" +
                        "</tr>";
                    $('#Plantable').append(rows);
                     
                    loadingimg.style = "display:none";
                }
            },
            error: function (xmlhttprequest, textstatus, errorthrown) {
                console.log(errorthrown);
            }
        });


        ajaxRequest.done(function (data, textStatus, jqXHR ) {
            // Do other operation
           

        });

       

    });

    function getMasterMediaPath(path)
    {
        var relpath = "";
        var mediapath = "";
        if (path !== null && path !== '')
        {
            relpath = path.toLowerCase().split('media library');
            if (relpath.length > 0) {
                relpath = relpath[1];
            }
            else {
                return mediapath;
            }
            mediapath = window.location.origin + "/-/media" + relpath;
        }
        return mediapath;
    }

</script>
    <style>
        td,th{
            min-width:100px;
        }
    </style>
</body>
</html>
