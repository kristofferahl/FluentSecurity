<%@ Page MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HomeView>" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">AccessDenied</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Access denied</h1>
	<p>You are not authorized to view this page.</p>
</asp:Content>
