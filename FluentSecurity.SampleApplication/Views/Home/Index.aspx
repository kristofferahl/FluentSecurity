<%@ Page MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HomeView>" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Getting started</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Fluent Security</h1>
	
	<h2>Log in / Log out</h2>
	<ul>
		<%= Html.NavigationLink(Url.Action<AccountController>(x => x.LogInAsAdministrator()), "Log in as administrator", "li") %>
		<%= Html.NavigationLink(Url.Action<AccountController>(x => x.LogInAsPublisher()), "Log in as publisher", "li") %>
		<%= Html.NavigationLink(Url.Action<AccountController>(x => x.LogOut()), "Log out", "li") %>
	</ul>
	
	<h2>Examples</h2>
	<ul>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.DenyAnonymousAccess()), "Deny anonymous access", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.DenyAuthenticatedAccess()), "Deny authenticated access", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.RequireAdministratorRole()), "Require administrator role", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.RequirePublisherRole()), "Require publisher role", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.MissingConfiguration()), "Missing configuration", "li") %>
	</ul>
	
	<h2>What do i have</h2>
	<pre><%= Model.WhatDoIHave %></pre>
</asp:Content>