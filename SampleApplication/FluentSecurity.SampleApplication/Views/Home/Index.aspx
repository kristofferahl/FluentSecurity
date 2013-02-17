<%@ Page MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HomeView>" %>
<%@ Import Namespace="FluentSecurity.SampleApplication" %>
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
	
	<h3>Area examples</h3>
	<p>Log in to see all the actions available</p>
	<ul>
		<%= Html.NavigationLink(Url.AreaAction<FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers.HomeController>(x => x.Index(), "ExampleArea"), "Index", "li") %>
		<%= Html.NavigationLink(Url.AreaAction<FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly(), "ExampleArea"), "Publishers only", "li") %>
		<%= Html.NavigationLink(Url.AreaAction<FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly(), "ExampleArea"), "Administrators only", "li") %>
	</ul>
	
	<h2>What do I have</h2>
	<pre><%= Model.WhatDoIHave %></pre>
	
	<h2>Here's what is happening behind the scenes</h2>
	<table style="font-size: .7em; table-layout: fixed;">
		<thead>
			<tr>
				<th style="width: 190px;">Correlation Id</th>
				<th>Message</th>
				<th style="width: 80px;">Completed in</th>
			</tr>
		</thead>
		<tbody>
		<% foreach (var @event in Bootstrapper.Events) { %>
			<tr>
				<td><%= @event.CorrelationId %></td>
				<td style="word-wrap: break-word"><%= Html.Encode(@event.Message).Replace(Environment.NewLine, "<br/>") %></td>
				<td><%= @event.CompletedInMilliseconds != null ? @event.CompletedInMilliseconds + " ms" : "-" %></td>
			</tr>
		<% } %>
		</tbody>
	</table>
</asp:Content>