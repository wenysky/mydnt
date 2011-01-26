<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
   ToolKit首页
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <p>
        要想了解更多产品信息，请访问 <a href="http://nt.discuz.net" title="Discuz!NT Website">http://nt.discuz.net</a>.
    </p>
</asp:Content>
