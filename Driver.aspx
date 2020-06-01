<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Driver.aspx.cs" Inherits="Kata.Driver" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <div class="mainBox">
        <form id="form1" runat="server" enctype="multipart/form-data">
            <input id="file-upload" type="file" name="FileUpload" accept="text/plain" />
            <asp:Button ID="Button1" CssClass="buttonUpload" Text="Upload" runat="server" OnClick="Upload" />
            <br />
        <asp:Label ID = "lblMessage" Text="File uploaded successfully." runat="server" ForeColor="#33cc33" Visible="false" />
            <div style="padding-top:2rem">
                <asp:GridView 
                    ID="driversMPH" 
                    runat="server" 
                    AutoGenerateColumns="false"
                    Gridlines="None"
                    cssclass="cssGrid"
                    width="600px">
                    <Columns>
                        <asp:BoundField DataField="driver" HeaderText="Driver" />
                        <asp:BoundField DataField="distance" HeaderText="Distance (Miles)" />
                        <asp:BoundField DataField="mph" HeaderText="Miles per Hour" />
                    </Columns>
                </asp:GridView>
            </div>
         </form>
     </div>
</body>
</html>
