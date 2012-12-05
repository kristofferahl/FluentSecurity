<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	MissingConfiguration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Missing configuration</h1>
    
    <p>This page should only be displayed if security has been configured or if IgnoreMissingConfiguraion has been called.</p>
</asp:Content>