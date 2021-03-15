// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using PCIBusiness;

// Error codes 11000-11099

namespace PCIWebFinAid
{
	public partial class pgLogonCRM : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ApplicationCode = "002"; // CareAssist CRM
			SetErrorDetail("",-888);

			if ( Page.IsPostBack )
				SessionCheck(5);

			else
			{
				SessionCheck(3);

				string productCode         = WebTools.RequestValueString(Request,"PC");  // ProductCode");
				string languageCode        = WebTools.RequestValueString(Request,"LC");  // LanguageCode");
				string languageDialectCode = WebTools.RequestValueString(Request,"LDC"); // LanguageDialectCode");

				if ( ! Tools.SystemIsLive() )
				{
//	Testing 1 (English)
					if ( productCode.Length           < 1 ) productCode         = "10387";
					if ( languageCode.Length          < 1 ) languageCode        = "ENG";
					if ( languageDialectCode.Length   < 1 ) languageDialectCode = "0001";
//	Testing 2 (Thai)
//					if ( productCode.Length           < 1 ) productCode         = "10024";
//					if ( languageCode.Length          < 1 ) languageCode        = "THA";
//					if ( languageDialectCode.Length   < 1 ) languageDialectCode = "0001";
				}
				if ( productCode.Length         < 1 ||
				     languageCode.Length        < 1 ||
				     languageDialectCode.Length < 1 )
				{
					SetErrorDetail("PageLoad",10200,"Invalid startup values ... system cannot continue","");
					X103016.Text    = "Disabled"; // Button
					X103016.Enabled = false;
					X103014.Enabled = false; // User name
					X103015.Enabled = false; // Password
				}
				else
				{
					LoadLabelText(null);
					LoadPageData();
					X103014.Focus(); // User name
					SessionSave(null,null,null,null,productCode,languageCode,languageDialectCode);
				}
				ascxXHeader.ShowUser(null,ApplicationCode);
			}
		}

		protected override void LoadPageData()
		{
		}

		protected void btnLogin_Click(Object sender, EventArgs e)
		{
			SetErrorDetail("btnLogin_Click",10100,"Invalid user name and/or password","One or both of user name/password is blank",2,2,null,true);
			X103014.Text = X103014.Text.Trim(); // User name
			X103015.Text = X103015.Text.Trim(); // Password

			if ( X103014.Text.Length < 1 || X103015.Text.Length < 1 )
				return;

//	Testing
			if ( ! Tools.SystemIsLive() && X103014.Text.ToUpper() == "PK" && X103015.Text.ToUpper() == "PK" )
			{
				SessionSave("100248","Paul Kilfoil","N","20200304-1911");
				WebTools.Redirect(Response,sessionGeneral.StartPage);
				return;
			}
//	Testing

//			if ( txtID.Text.ToUpper() == "XADMIN" && txtPW.Text.ToUpper() == "X8Y3Z7" )
//			{
//				SessionSave("109337","Ivan Pyotrsky","N","20200304-2054");
//				WebTools.Redirect(Response,sessionGeneral.StartPage);
//				return;
//			}

			using (MiscList mList = new MiscList())
			{
				sqlProc = "SP_ClientCRMValidateLoginD";
				sql     = "exec " + sqlProc + " @IPAddress = "   + Tools.DBString(WebTools.ClientIPAddress(Request))
				                            + ",@ClientCode = "  + Tools.DBString(X103014.Text,47)
				                            + ",@ContractPin = " + Tools.DBString(X103015.Text)
				                            + ",@ProductCode = " + Tools.DBString(sessionGeneral.ProductCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnLogin_Click",10110,"Internal database error (" + sqlProc + ")",sql,1,1);
				else if ( mList.EOF )
					SetErrorDetail("btnLogin_Click",10120,"Invalid login and/or PIN",sqlProc + ", no data returned",1,1);
				else if ( mList.GetColumn("Status") != "S" )
				{
					string err = mList.GetColumn("ActionResultMessage");
					if ( err.Length < 1 )
						err = "Invalid login and/or PIN";
					SetErrorDetail("btnLogin_Click",10130,err,sqlProc + ", Status = '" + mList.GetColumn("Status") + "'",1,1);
				}
				else
				{
					string userCode     = mList.GetColumn("ClientCode");
					string contractCode = mList.GetColumn("ContractCode");
					string access       = mList.GetColumn("Access");
					SessionSave(userCode,"",access,contractCode);
					WebTools.Redirect(Response,sessionGeneral.StartPage);
				}
			}
		}
	}
}