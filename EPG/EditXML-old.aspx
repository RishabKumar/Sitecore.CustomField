<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditXML.aspx.cs" Inherits="EPG.EditXML" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     
    
    <div>
   <%--  <asp:PlaceHolder ID="XMLDataTable" runat="server" />--%>
      
        <select id="DayFilter">
            <option value="All">All Shows</option>
            <option value="Monday">Monday</option>
            <option value="Tuesday">Tuesday</option>
            <option value="Wednesday">Wednesday</option>
            <option value="Thursday">Thursday</option>
            <option value="Friday">Friday</option>
            <option value="Saturday">Saturday</option>
            <option value="Sunday">Sunday</option>
        </select>
        
        <div class="PlanTables">
            <table border="1">
            <tr class="heading">
                 
            <%
                foreach (var property in properties)
                {
                    %>
                        <th><%=property.Name%></th>
                    <%
                }
                        
            %>
                 
            </tr>
        <%
            foreach (var planobj in planlist)
            {
            %>
                <tr class="Row_<%= planobj.DayName%>">
                    <%
                        foreach (var property in properties)
                        {
                            %>
                             <td><%=property.GetValue(planobj).ToString()%></td>
                            <%
                        }
                        
                         %>
                    <td>
                        <img id="<%=planobj.PlanTitleID +"_img"%>" width="100px" src="data:image/png;base64,<%=GetThumbnailBytes(planobj.PlanTitleID)%>" />
                    </td>
                    <td> 
                        <form id="form<%=planobj.PlanTitleID%>" enctype="multipart/form-data">
                            <input type="file" id="<%=planobj.PlanTitleID %>" />  
                            <input type="button" onclick="uploadFile('<%=planobj.PlanTitleID %>')" value="Upload" />
                        </form>
                    </td>
                </tr>
             <%
            }
            %>
            </table>
        </div>
    </div>
     


  <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.2.1.min.js"></script>
<script type="text/javascript">

    function uploadFile(id) {        
        //var xhr = new XMLHttpRequest();                 
        //var file = document.getElementById("form"+id);
        //xhr.open("POST", "api/UploadThumbnail/UploadMedia");
        //xhr.setRequestHeader("filename", id);
        //xhr.setRequestHeader("content-type","multipart/form-data");
        //xhr.send(file);
        var data = new FormData();

        var files = $("#"+id).get(0).files;

        // Add the uploaded image content to the form data collection
        if (files.length > 0) {
            data.append("UploadedImage", files[0]);
        }

        // Make Ajax request with the contentType = false, and procesDate = false
        var ajaxRequest = $.ajax({
            type: "POST",
            url: "api/UploadThumbnail/UploadMedia?filename="+id,
            contentType: false,
            processData: false,
            filename: id,
            data: data
        });

        ajaxRequest.done(function (data, textStatus, jqXHR ) {
            // Do other operation
            $('#'+id+"_img").attr('src', "data:image/png;base64," + data);
        });
     


    }

    var listoftables = document.querySelectorAll("div.PlanTables table tr");
    var listofheadings = document.querySelectorAll("div.PlanTables table tr.heading");
    //listoftables.forEach(function(ele){
    //    ele.style = "display:block";
    //});

    $("#DayFilter").change(function()
    {
        var rows = document.querySelectorAll(".Row_"+this.value);
        if(rows !== undefined && rows.length > 0)
        {
            listoftables.forEach(function(ele){
                ele.style = "display:none";
            });
            rows.forEach(function(ele){
                ele.style = "display:table-row";
                });
        }
        else{
            listoftables.forEach(function(ele){
                ele.style = "display:table-row";
            });
        }
        listofheadings.forEach(function(ele){
            ele.style = "display:table-row";
        });
    });

</script>
    <style>
        td,th{
            min-width:100px;
        }
    </style>
</body>
</html>
