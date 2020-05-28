using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XLogin : BasePageLogin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			lblErr2.Text        = "";
			pnlSecurity.Visible = false;
			SetErrorDetail("",-888);

			if ( Page.IsPostBack )
				SessionCheck(5);
			else
			{
				SessionCheck(2);
				ascxXHeader.ShowUser(null);
				txtID.Focus();
			}
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			pnlSecurity.Visible = true;
			SetErrorDetail("btnOK_Click",11010,"Invalid security code","The security code cannot be blank/empty",23,2,null,true);
			if ( txtSecurity.Text.Trim().Length < 1 )
				return;

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_Verify_BackOfficeSecurityCode"
				    + " @UserCode = "     + Tools.DBString(sessionGeneral.UserCode)
				    + ",@SecurityCode = " + Tools.DBString(txtSecurity.Text);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnOK_Click",11020,"Internal database error (sp_Verify_BackOfficeSecurityCode)",sql,23,1,null,true);
				else if ( mList.EOF )
					SetErrorDetail("btnOK_Click",11030,"Invalid security code",sql + " (no data returned)",23,1,null,true);
				else
				{
					string status  = mList.GetColumn("Status").ToUpper();
					string message = mList.GetColumn("Message");
					if ( status != "S" )
						SetErrorDetail("btnOK_Click",11040,message,sql + " (Status = '" + status + "')",23,1,null,true);
					else
					{
						SessionSave(null,null,"P");
						WebTools.Redirect(Response,sessionGeneral.StartPage);
					}
				}
			}
		}

		protected void btnLogin_Click(Object sender, EventArgs e)
		{
			SetErrorDetail("btnLogin_Click",10010,"Invalid user name and/or password","One or both of user name/password is blank",2,2,null,true);
			txtID.Text = txtID.Text.Trim();
			txtPW.Text = txtPW.Text.Trim();

			if ( txtID.Text.Length < 1 || txtPW.Text.Length < 1 )
				return;

//	Testing
			if ( ! Tools.SystemIsLive() && txtID.Text.ToUpper() == "PK" && txtPW.Text.ToUpper() == "PK" )
			{
				SessionSave("248","Paul Kilfoil","P");
				WebTools.Redirect(Response,sessionGeneral.StartPage);
				return;
			}
//	Testing

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_Check_BackOfficeUser"
				    + " @UserName = " + Tools.DBString(txtID.Text)
				    + ",@Password = " + Tools.DBString(txtPW.Text);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnLogin_Click",10020,"Internal database error (sp_Check_BackOfficeUser)",sql,1,1,null,true);
				else if ( mList.EOF )
					SetErrorDetail("btnLogin_Click",10030,"Invalid user name and/or password",sql + " (no data returned)",1,1,null,true);
				else
				{
					string userCode = mList.GetColumn("UserCode");
					string userName = mList.GetColumn("UserDisplayName");
					string status   = mList.GetColumn("Status").ToUpper();
					string message  = mList.GetColumn("Message");
					if ( status != "S" )
						SetErrorDetail("btnLogin_Click",10040,message,sql + " (Status = '" + status + "')",1,1,null,true);
					else if ( userCode.Length < 1 || userName.Length < 2 )
						SetErrorDetail("btnLogin_Click",10050,"User details corrupted",sql + " (UserCode/UserDisplayName empty/invalid)",1,1,null,true);
					else
					{
						SetErrorDetail("",-777);
						SessionSave(userCode,userName,"X");
						pnlSecurity.Visible = true;
						txtSecurity.Text    = "";
						txtSecurity.Focus();
					}
				}
			}
		}
	}
}