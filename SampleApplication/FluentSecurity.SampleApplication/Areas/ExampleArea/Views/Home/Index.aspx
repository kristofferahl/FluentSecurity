<%@ Page MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Getting started</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Fluent Security - Example area</h1>
	
	<ul>
		<li><%= Html.ActionLink("Publishers only", "PublishersOnly") %></li>
		<li><%= Html.ActionLink("Administrators only", "AdministratorsOnly") %></li>
	</ul>
</asp:Content>