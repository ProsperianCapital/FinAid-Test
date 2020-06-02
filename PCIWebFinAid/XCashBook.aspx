﻿<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="XCashBook.aspx.cs" Inherits="PCIWebFinAid.XCashBook" ValidateRequest="false" %>
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
<script type="text/javascript">
function HidePopups()
{
	hideCalendar();
	ShowMessages(0);
	ShowElt('pnlDelete',false);
}
function EditMode(show,descr)
{
//	0 : Hide
//	1 : Show, edit
//	2 : Show, insert
	HidePopups();
	SetEltValue('lblEditHead',descr+' Transaction');
	ShowElt('pnlEdit',(show>0));
	if ( show == 2 )
		ShowElt('btnDel1',false);
}
function DeleteMode(show)
{
	HidePopups();
	ShowElt('pnlDelete',(show>0));
}
</script>
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
		<td>Transaction Start Date<br /><asp:TextBox runat="server" ID="txtSDate1" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtSDate1)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td style="white-space:nowrap" rowspan="2">Amount<br />
		> <asp:TextBox runat="server" ID="DropDownList7" Width="100px"></asp:TextBox> and<br />
		< <asp:TextBox runat="server" ID="TextBox1" Width="100px"></asp:TextBox></td>
		<td>Transaction End Date<br /><asp:TextBox runat="server" ID="txtSDate2" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtSDate2)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>Transaction Recon Date<br /><asp:TextBox runat="server" ID="txtSRecon" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtSRecon)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td>
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

	<input type="button" value="New" title="Capture a new transaction" onclick="JavaScript:EditMode(2,'New');return false" />&nbsp;
	<asp:PlaceHolder runat="server" ID="pnlGridBtn" Visible="false">
	<asp:Button runat="server" ID="btnPDF" Text="PDF" ToolTip="Download in PDF format" />&nbsp;
	<asp:Button runat="server" ID="btnCSV" Text="CSV" ToolTip="Download in CSV (Excel) format" />
	</asp:PlaceHolder>

	<div class="Popup2" id="pnlEdit" style="visibility:hidden;display:none">
	<div class="PopupHead" id="lblEditHead"></div>
	<table>
	<tr>
		<td>Company<br /><asp:DropDownList runat="server" ID="lstECompany"></asp:DropDownList></td>
		<td>Cashbook<br /><asp:DropDownList runat="server" ID="DropDownList9"></asp:DropDownList></td></tr>
	<tr>
		<td>Receipt / Payment<br /><asp:DropDownList runat="server" ID="DropDownList10"></asp:DropDownList></td>
		<td>Transaction Type<br /><asp:DropDownList runat="server" ID="DropDownList11"></asp:DropDownList></td></tr>
	<tr>
		<td>OBO Company<br /><asp:DropDownList runat="server" ID="DropDownList12"></asp:DropDownList></td>
		<td>Tax Rate<br /><asp:DropDownList runat="server" ID="DropDownList13"></asp:DropDownList></td></tr>
	<tr>
		<td>GL Account Code<br /><asp:TextBox runat="server" ID="TextBox2"></asp:TextBox></td>
		<td>GL Account Dimension<br /><asp:DropDownList runat="server" ID="DropDownList14"></asp:DropDownList></td></tr>
	<tr>
		<td>Amount<br /><asp:TextBox runat="server" ID="TextBox3" MaxLength="10" Width="80px"></asp:TextBox></td>
		<td>Transaction Date<br /><asp:TextBox runat="server" ID="txtEDate" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtEDate)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>Transaction ID<br /><asp:TextBox runat="server" ID="txtETranID" readOnly="true"></asp:TextBox></td>
		<td>Transaction Recon Date<br /><asp:TextBox runat="server" ID="txtERecon" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtERecon)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td colspan="2">Transaction Description<br /><asp:TextBox runat="server" ID="TextBox5" Width="500px"></asp:TextBox></td></tr>
	</table>
	<asp:Button runat="server" ID="btnSave" Text="Save" title="Save this transaction" />&nbsp;
	<input type="button" value="Delete" id="btnDel1" title="Delete this transaction" onclick="JavaScript:DeleteMode(1)" />
	<input type="button" value="Cancel" id="btnCancel" onclick="JavaScript:EditMode(0)" />
	</div>

	<div id="pnlDelete" class="PopupConfirm" style="visibility:hidden;display:none;width:320px">
	<div class="HelpHead">Please confirm ...</div>
	<p>
	You are about to DELETE transaction id 879879.<br />
	This action cannot be reversed.
	</p><p class="Error"><b>
	Are you sure you want to do this?
   </b></p>
	<asp:Button runat="server" ID="btnDel2" Text="Delete" title="Delete this transaction" />&nbsp;
	<input type="button" value="Cancel" onclick="JavaScript:DeleteMode(0)" />
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>