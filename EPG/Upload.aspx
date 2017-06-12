<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="EPG.Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="XML_FileUpload" runat="server" />
        <asp:Button ID="XML_UploadButton" runat="server" Text="Upload file" OnClick="XML_UploadButton_Click" />
        <asp:Label ID="XML_UploadStatusLabel" runat="server" />
    </div>
    </form>
    

</body>
</html>
