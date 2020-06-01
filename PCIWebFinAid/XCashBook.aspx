<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="XCashBook.aspx.cs" Inherits="PCIWebFinAid.XCashBook" ValidateRequest="false" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainAdmin.htm" -->
	<title>Prosperian BackOffice</title>
	<link rel="stylesheet" href="CSS/BackOffice.css?v=1" type="text/css" />
	<link rel="stylesheet" href="CSS/Calendar.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<form id="frmCashBook" runat="server">
	<script type="text/javascript" src="JS/Calendar.js"></script>

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	Cash Book
	</div>

	<div id="pnlFilter" style="border:1px solid #000000">
	<div class="PopupHead">Search/Filter
		<span style="float:right">
		<a href="#" onclick="JavaScript:ShowElt('tblFilter',true)">Show</a>&nbsp;&nbsp;
		<a href="#" onclick="JavaScript:ShowElt('tblFilter',false)">Hide</a></span>
	</div>
	<table id="tblFilter">
	<tr>
		<td>Company<br /><asp:DropDownList runat="server" ID="lstCompany"></asp:DropDownList></td>
		<td>Cashbook<br /><asp:DropDownList runat="server" ID="lstCashBook"></asp:DropDownList></td></tr>
	<tr>
		<td>Receipt / Payment<br /><asp:DropDownList runat="server" ID="DropDownList1"></asp:DropDownList></td>
		<td>Transaction Type<br /><asp:DropDownList runat="server" ID="DropDownList2"></asp:DropDownList></td></tr>
	<tr>
		<td>OBO Company<br /><asp:DropDownList runat="server" ID="DropDownList5"></asp:DropDownList></td>
		<td>Tax Rate<br /><asp:DropDownList runat="server" ID="DropDownList6"></asp:DropDownList></td></tr>
	<tr>
		<td>GL Account Code<br /><asp:TextBox runat="server" ID="DropDownList3"></asp:TextBox></td>
		<td>GL Account Dimension<br /><asp:DropDownList runat="server" ID="DropDownList4"></asp:DropDownList></td></tr>
	<tr>
		<td>Transaction Description<br /><asp:TextBox runat="server" ID="DropDownListX"></asp:TextBox></td>
		<td>Transaction Start Date<br /><asp:TextBox runat="server" ID="txtDateStart" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtDateStart)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td style="white-space:nowrap" rowspan="2">Amount<br />
		> <asp:TextBox runat="server" ID="DropDownList7" Width="100px"></asp:TextBox> and<br />
		< <asp:TextBox runat="server" ID="TextBox1" Width="100px"></asp:TextBox></td>
		<td>Transaction End Date<br /><asp:TextBox runat="server" ID="txtDateEnd" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtDateEnd)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>Transaction Recon Date<br /><asp:TextBox runat="server" ID="txtDateRecon" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtDateRecon)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td>
		<td rowspan="99">
			<asp:Button runat="server" ID="btnSearch" Text="Filter" OnClientClick="JavaScript:ShowBusy('Searching ... Please be patient',null,0)" OnClick="btnSearch_Click" />
		</td></tr>
	</table>
	</div>

	<br />

	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<asp:DataGrid id="grdData" runat="server" AutoGenerateColumns="False" CellSpacing="5" OnItemCommand="grdData_ItemCommand">
		<HeaderStyle CssClass="tRowHead"></HeaderStyle>
		<ItemStyle CssClass="tRow"></ItemStyle>
		<AlternatingItemStyle CssClass="tRowAlt"></AlternatingItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="Transaction<br />ID">
				<ItemTemplate>
					<asp:LinkButton runat="server" CommandName='Edit' CommandArgument='<%# ((PCIBusiness.MiscData)Container.DataItem).GetColumn(0) %>'><%# ((PCIBusiness.MiscData)Container.DataItem).GetColumn(0) %></asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Date" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Receipt or<br />Payment" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Type" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="GL Main<br />Account" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="GL Account<br />Dimension" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Description" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Tax<br />Rate" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Amount<br /<(Inclusive)" DataField="NextColumn" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>

	<asp:Button runat="server" ID="btnNew" Text="New" title="Capture a new transaction" />&nbsp;
	<asp:Button runat="server" ID="btnPDF" Text="PDF" title="Download in PDF format" />&nbsp;
	<asp:Button runat="server" ID="btnCSV" Text="CSV" title="Download in CSV (Excel) format" />

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>