﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CAHeader.ascx.cs" Inherits="PCIWebFinAid.CAHeader" %>

<div class="Header1">
	<table class="Header1a" style="width:99%">
	<tr>
		<td style="width:20%">
			<asp:Image runat="server" ID="P12001" /></td>
		<td style="width:50%;white-space:nowrap;text-align:center">
			Instant Help When You Need It Most</td>
		<td style="width:30%;white-space:nowrap">
			<asp:HyperLink runat="server" ID="X100008" CssClass="TopButton TopButtonO"></asp:HyperLink>&nbsp;
			<asp:HyperLink runat="server" ID="X100009" CssClass="TopButton TopButtonY"></asp:HyperLink>&nbsp;
			<asp:DropDownList runat="server" ID="lstLang" CssClass="TopButton" AutoPostBack="true" style="padding:0px"></asp:DropDownList>
	</table>
</div>
