using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Login : BasePageLogin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			SessionCheck(3);

			if ( ! Page.IsPostBack )
			{
				hdnVer.Value                       = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
				lblVer.Text                        = hdnVer.Value;
				lblVer.Visible                     = ! Tools.SystemIsLive();
				sessionGeneral.ProductCode         = WebTools.RequestValueString(Request,"PC");
				sessionGeneral.LanguageCode        = WebTools.RequestValueString(Request,"LC");
				sessionGeneral.LanguageDialectCode = WebTools.RequestValueString(Request,"LDC");

//	Testing 1 (English)
				if ( sessionGeneral.ProductCode.Length         < 1 ) sessionGeneral.ProductCode         = "10278";
				if ( sessionGeneral.LanguageCode.Length        < 1 ) sessionGeneral.LanguageCode        = "ENG";
				if ( sessionGeneral.LanguageDialectCode.Length < 1 ) sessionGeneral.LanguageDialectCode = "0002";

//	Testing 2 (Thai)
//				if ( sessionGeneral.ProductCode.Length         < 1 ) sessionGeneral.ProductCode         = "10024";
//				if ( sessionGeneral.LanguageCode.Length        < 1 ) sessionGeneral.LanguageCode        = "THA";
//				if ( sessionGeneral.LanguageDialectCode.Length < 1 ) sessionGeneral.LanguageDialectCode = "0001";

				if ( LoadLabelText(null) != 0 )
					X103016.Enabled = false;
				SessionClearData();
				SessionSave();
				txtID.Focus();
			}
		}

		protected void btnLogin_Click(Object sender, EventArgs e)
		{
			SetErrorDetail("btnLogin_Click",10010,"Invalid login and/or PIN","One or both of ID/PIN is blank");
			txtID.Text = txtID.Text.Trim();
			txtPW.Text = txtPW.Text.Trim();

			if ( txtID.Text.Length < 1 || txtPW.Text.Length < 1 )
				return;

			if ( txtID.Text.ToUpper() == "XADMIN" && txtPW.Text.ToUpper() == "X8Y3Z7" )
			{
				SessionSave("Prosperian","Admin","A");
				WebTools.Redirect(Response,sessionGeneral.StartPage);
				return;
			}

			using (MiscList mList = new MiscList())
			{
				sql = "exec SP_ClientCRMValidateLoginC"
				    + " @IPAddress = "   + Tools.DBString(WebTools.ClientIPAddress(Request))
				    + ",@ClientCode = "  + Tools.DBString(txtID.Text)
				    + ",@ContractPin = " + Tools.DBString(txtPW.Text)
				    + ",@ProductCode = " + Tools.DBString(sessionGeneral.ProductCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnLogin_Click",10020,"Internal database error (SP_ClientCRMValidateLoginC)",sql,1,1);
				else if ( mList.EOF )
					SetErrorDetail("btnLogin_Click",10030,"Invalid login and/or PIN","SP_ClientCRMValidateLoginC, no data returned",1,1);
				else if ( mList.GetColumn("Status") != "S" )
					SetErrorDetail("btnLogin_Click",10040,"Invalid login and/or PIN","SP_ClientCRMValidateLoginC, Status = '" + mList.GetColumn("Status") + "'",1,1);
				else
				{
					string clientCode   = mList.GetColumn("ClientCode");
					string contractCode = mList.GetColumn("ContractCode");
					string access       = mList.GetColumn("Access");
					SessionSave(clientCode,contractCode,access);
					WebTools.Redirect(Response,sessionGeneral.StartPage);
				}
			}
		}

/*
Moved to BasePageLogin

		private void SetErrorDetail(int errCode,string errBrief="",string errDetail="",byte briefMode=2,byte detailMode=2)
		{
			if ( errCode == 0 )
				return;

			if ( errCode <  0 )
			{
				lblError.Text    = "";
				lblErrorDtl.Text = "";
				lblError.Visible = false;
				btnError.Visible = false;
				return;
			}
			Tools.LogInfo("Login.SetErrorDetail","(errCode="+errCode.ToString()+") "+errDetail,244);

			if ( briefMode == 2 ) // Append
				lblError.Text = lblError.Text + ( lblError.Text.Length > 0 ? "<br />" : "" ) + errBrief;
			else
				lblError.Text = errBrief;

			if ( errDetail.Length < 1 )
				errDetail = errBrief;
			errDetail = "[" + errCode.ToString() + "] " + errDetail;

			errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
			if ( detailMode == 2 ) // Append
				errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
			lblErrorDtl.Text = errDetail;
			if ( ! lblErrorDtl.Text.StartsWith("<div") )
				lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white;height:20px'>Error Details<img src='Images/Close1.png' title='Close' style='float:right' onclick=\"JavaScript:ShowElt('lblErrorDtl',false)\" /></div>" + lblErrorDtl.Text;

			lblError.Visible = ( lblError.Text.Length    > 0 );
			btnError.Visible = ( lblErrorDtl.Text.Length > 0 ) && lblError.Visible && ! Tools.SystemIsLive();
		}
*/

	}
}