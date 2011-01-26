<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="websitemanage.aspx.cs" Inherits="Discuz.Async.Web.websitemanage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       apikey: <input id="apikey" name="apikey" type="text" /><br />
       security: <input id="security" name="security" type="text" /><br />
       websiteurl: <input id="websiteurl" name="websiteurl" type="text" /><br />
       callbackurl: <input id="callbackurl" name="callbackurl" type="text" /><br />
       asyncurl: <input id="asyncurl" name="asyncurl" type="text" /><br />
       asyncdatalist: <input id="asyncdatalist" name="asyncdatalist" type="text" /><br />
       sitetype: <input id="sitetype" name="sitetype" type="text" />
       <input type="submit" id="addsite" />
    </div>
    </form>
</body>
</html>
