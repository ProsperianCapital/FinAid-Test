using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XLogin : BasePageLogin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			SessionCheck(2);

			if ( ! Page.IsPostBack )
				txtID.Focus();
		}

		protected void btnLogin_Click(Object sender, EventArgs e)
		{
			SetErrorDetail("btnLogin_Click",10010,"Invalid user name and/or password","One or both of user id/password is blank",2,2,null,true);
			txtID.Text = txtID.Text.Trim();
			txtPW.Text = txtPW.Text.Trim();

			if ( txtID.Text.Length < 1 || txtPW.Text.Length < 1 )
				return;

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
					string status   = mList.GetColumn("Status").ToUpper();
					string message  = mList.GetColumn("Message");
					if ( status != "S" )
						SetErrorDetail("btnLogin_Click",10040,message,sql + " (Status = '" + status + "')",1,1,null,true);
					else
					{
						SessionSave(userCode,"Pete Smith","P");
						WebTools.Redirect(Response,sessionGeneral.StartPage);
					}
				}
			}
		}
	}
}