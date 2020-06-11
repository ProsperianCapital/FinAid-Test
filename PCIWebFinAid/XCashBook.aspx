<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="XCashBook.aspx.cs" Inherits="PCIWebFinAid.XCashBook" EnableEventValidation="false" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainAdmin.htm" -->
	<title>Prosperian BackOffice</title>
	<link rel="stylesheet" href="CSS/BackOffice.css" type="text/css" />
	<link rel="stylesheet" href="CSS/Calendar.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<script type="text/javascript" src="JS/AJAXUtils.js"></script>
<script type="text/javascript">
function AJAXFinalize(typeCode)
{
	if ( typeCode == 1 ) // Cashbook lookup
	{
		var codes      = xmlDOM.getElementsByTagName("CBCode");
		var names      = xmlDOM.getElementsByTagName("CBName");
		var sel        = xmlDOM.getElementsByTagName("CBSel");
		var searchEdit = XMLValue('SearchEdit');
		var lstCB      = GetElt("lst"+searchEdit+"CashBook");
		var k;
		var item;

		while (lstCB.length > 0)
			lstCB.remove(0);

		SetEltValue('hdn'+searchEdit+'CashBook','');

		for (k = 0; k < codes.length; k++)
		{
			item          = document.createElement('option');
			item.value    = codes[k].firstChild.nodeValue;
			item.text     = names[k].firstChild.nodeValue;
			item.selected = ( sel[k].firstChild.nodeValue == "Y" );
			if ( k == 0 )
				SetEltValue('hdn'+searchEdit+'CashBook',item.value);
			try
			{
				lstCB.add(item,null);
			}
			catch (x) // MS
			{
				lstCB.add(item);
			}
		}
	}
	HideBusy();
}
function LoadCashBooks(selCode,searchEdit)
{
	SetEltValue('hdn'+searchEdit+'CashBook','');
	var coy = GetListValue('lst'+searchEdit+'Company');
	if ( coy.length > 0 && coy != "0" )
	{
		ShowBusy("Loading ...");
		AJAXInitialize(1,"CompanyCode="+coy+"&SearchEdit="+searchEdit+(selCode==null?'':'&Selected='+selCode));
		return;
	}
	var lstCB = GetElt('lst'+searchEdit+'CashBook');
	while (lstCB.length > 0)
		lstCB.remove(0);
}
function HidePopups()
{
	hideCalendar();
	ShowMessages(0);
	ShowElt('pnlDelete',false);
//	ShowElt('pnlEdit',false);
}
function EditMode(editInsert)
{
//	editInsert values:
//	0 : Hide
//	1 : Show, edit
//	2 : Show, insert
	HidePopups();
	if ( editInsert == 1 )
		SetEltValue('lblEditHead','Edit/Delete Transaction');
	else
		SetEltValue('lblEditHead','New Transaction');
	ShowElt('pnlEdit',(editInsert> 0));
	ShowElt('btnDel1',(editInsert==1));
	SetEltValue('hdnEditInsert',editInsert);
}
function DeleteMode(show)
{
	HidePopups();
	SetEltValue('lblDTranID',GetEltValue('txtETranID'));
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
	<div class="PopupHead" style="background-color:orange">Search/Filter
		<span style="float:right">
		<a href="#" onclick="JavaScript:ShowElt('tblFilter',true)">Show</a>&nbsp;&nbsp;
		<a href="#" onclick="JavaScript:ShowElt('tblFilter',false)">Hide</a></span>
	</div>
	<table id="tblFilter">
	<tr>
		<td>Company</td><td><asp:DropDownList runat="server" ID="lstSCompany" onchange="JavaScript:LoadCashBooks(null,'S')"></asp:DropDownList></td>
		<td>Cashbook</td>
		<td>
			<select id="lstSCashBook" onchange="JavaScript:SetEltValue('hdnSCashBook',GetListValue(this))"></select>
			<asp:HiddenField runat="server" ID="hdnSCashBook" value="" /></td></tr>
	<tr>
		<td>Receipt / Payment</td><td><asp:DropDownList runat="server" ID="lstSReceipt"></asp:DropDownList></td>
		<td>Transaction Type</td><td><asp:DropDownList runat="server" ID="lstSTType" Enabled="false"></asp:DropDownList></td></tr>
	<tr>
		<td>OBO Company</td><td><asp:DropDownList runat="server" ID="lstSOBOCompany"></asp:DropDownList></td>
		<td>Transaction Description</td><td><asp:TextBox runat="server" ID="txtSDesc"></asp:TextBox></td></tr>
	<tr>
		<td>GL Account Code</td><td><asp:DropDownList runat="server" ID="lstSGLCode"></asp:DropDownList></td>
		<td>GL Account Dimension</td><td><asp:DropDownList runat="server" ID="lstSGLDimension"></asp:DropDownList></td></tr>
	<tr>
		<td>Transaction Start Date</td><td><asp:TextBox runat="server" ID="txtSDate1" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtSDate1)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td>
		<td>Transaction End Date</td><td><asp:TextBox runat="server" ID="txtSDate2" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtSDate2)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>Tax Rate</td><td><asp:DropDownList runat="server" ID="lstSTaxRate"></asp:DropDownList></td>
		<td style="white-space:nowrap">Amount
		> <asp:TextBox runat="server" ID="txtSAmt1" Width="100px"></asp:TextBox></td>
		<td>and
		< <asp:TextBox runat="server" ID="txtSAmt2" Width="100px"></asp:TextBox></td>
		<td rowspan="99" style="text-align:right">&nbsp;
			<asp:Button runat="server" ID="btnSearch" Text="Filter" OnClientClick="JavaScript:ShowBusy('Searching ... Please be patient',null,0)" OnClick="btnSearch_Click" />
		</td></tr>
	</table>
	</div>

	<p>
	<asp:Button runat="server" ID="btnNew" Text="New" ToolTip="Capture a new transaction" OnClick="btnNew_Click" />&nbsp;
	<asp:PlaceHolder runat="server" ID="pnlGridBtn" Visible="false">
	<asp:Button runat="server" ID="btnPDF" Text="PDF" ToolTip="Download in PDF format" />&nbsp;
	<asp:Button runat="server" ID="btnCSV" Text="CSV" ToolTip="Download in CSV (Excel) format" />
	</asp:PlaceHolder>
	</p>

	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<asp:DataGrid id="grdData" runat="server" AutoGenerateColumns="False" CellSpacing="5" OnItemCommand="grdData_ItemCommand">
		<HeaderStyle CssClass="tRowHead"></HeaderStyle>
		<ItemStyle CssClass="tRow"></ItemStyle>
		<AlternatingItemStyle CssClass="tRowAlt"></AlternatingItemStyle>
		<Columns>
			<asp:BoundColumn DataField="RowNumber" ItemStyle-BackColor="GreenYellow" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1" ItemStyle-BorderColor="Red" DataFormatString="&nbsp{0}&nbsp;"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Transaction<br />ID">
				<ItemTemplate>
					<asp:LinkButton runat="server" CommandName='Edit' CommandArgument='<%# ((PCIBusiness.MiscData)Container.DataItem).GetColumn(0) %>'><%# ((PCIBusiness.MiscData)Container.DataItem).NextColumn %></asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Date" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Receipt or<br />Payment" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Type" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="GL Main<br />Account" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="GL Account<br />Dimension" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Description" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Tax<br />Rate" DataField="NextColumn"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Amount<br />(Inclusive)" DataField="NextColumn" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>

	<div class="Popup2" id="pnlEdit" style="visibility:hidden;display:none">
	<asp:HiddenField runat="server" ID="hdnEditInsert" Value="0" />
	<div class="PopupHead" id="lblEditHead"></div>
	<table>
	<tr>
		<td>Company<br /><asp:DropDownList runat="server" ID="lstECompany" onchange="JavaScript:LoadCashBooks(null,'E')"></asp:DropDownList></td>
		<td>
			Cashbook<br /><asp:DropDownList runat="server" ID="lstECashBook" onchange="JavaScript:SetEltValue('hdnECashBook',GetListValue(this))"></asp:DropDownList>
			<asp:HiddenField runat="server" ID="hdnECashBook" value="" /></td></tr>
	<tr>
		<td>Receipt / Payment<br /><asp:DropDownList runat="server" ID="lstEReceipt"></asp:DropDownList></td>
		<td>Transaction Type<br /><asp:DropDownList runat="server" ID="lstETType" Enabled="false"></asp:DropDownList></td></tr>
	<tr>
		<td>OBO Company<br /><asp:DropDownList runat="server" ID="lstEOBOCompany"></asp:DropDownList></td>
		<td>Tax Rate<br /><asp:TextBox runat="server" ID="txtETaxRate" Width="40px"></asp:TextBox></td></tr>
	<tr>
		<td>GL Account Code<br /><asp:DropDownList runat="server" ID="lstEGLCode"></asp:DropDownList></td>
		<td>GL Account Dimension<br /><asp:DropDownList runat="server" ID="lstEGLDimension"></asp:DropDownList></td></tr>
	<tr>
		<td>Amount<br /><asp:TextBox runat="server" ID="txtEAmt" MaxLength="10" Width="80px"></asp:TextBox></td>
		<td>Transaction Date<br /><asp:TextBox runat="server" ID="txtEDate" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtEDate)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>Currency<br />
			<asp:DropDownList runat="server" ID="lstECurr">
				<asp:ListItem Value="USD" Text="US Dollars"></asp:ListItem>
				<asp:ListItem Value="EUR" Text="Euros"></asp:ListItem>
				<asp:ListItem Value="ZAR" Text="SA Rands"></asp:ListItem>
				<asp:ListItem Value="THB" Text="Thai Bhats"></asp:ListItem>
			</asp:DropDownList></td>
		<td>Recon Date<br /><asp:TextBox runat="server" ID="txtERecon" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtERecon)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>Transaction Description<br /><asp:TextBox runat="server" ID="txtEDesc" Width="250px" TextMode="MultiLine" Rows="3"></asp:TextBox></td>
		<td>Transaction ID<br /><asp:TextBox runat="server" ID="txtETranID" readOnly="true"></asp:TextBox></td></tr>
	</table>
	<asp:Button runat="server" ID="btnSave" Text="Save" title="Save this transaction" OnClick="btnUpdate_Click" />&nbsp;
	<input type="button" value="Delete" id="btnDel1" title="Delete this transaction" onclick="JavaScript:DeleteMode(1)" />
	<input type="button" value="Cancel" id="btnCancel" onclick="JavaScript:EditMode(0)" />
	<asp:Label runat="server" id="lblErr2" cssclass="Error"></asp:Label>
	</div>

	<div id="pnlDelete" class="PopupConfirm" style="visibility:hidden;display:none;width:320px">
	<div class="HelpHead">Please confirm ...</div>
	<p>
	You are about to DELETE transaction id <span id="lblDTranID" style="font-weight:bold"></span>.<br />
	This action cannot be reversed.
	</p><p class="Error"><b>
	Are you sure you want to do this?
   </b></p>
	<asp:Button runat="server" ID="btnDel2" Text="Delete" title="Delete this transaction" OnClick="btnDelete_Click" />&nbsp;
	<input type="button" value="Cancel" onclick="JavaScript:DeleteMode(0)" />
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>