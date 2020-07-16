using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Home : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			string url = Request.Url.AbsoluteUri.Trim();

			try
			{
				int    k         = url.IndexOf("://");
				string dName     = url.Substring(k+3);
				string goTo      = "XLogin.aspx";
				string appCode   = "*";
				string appStatus = "*";

				k = dName.ToUpper().IndexOf("/HOME.ASPX");
				if ( k > 0 )
					dName   = dName.Substring(0,k);
				string sql = "exec sp_Get_BackOfficeApplication " + Tools.DBString(dName);

				using (MiscList mList = new MiscList())
					if ( mList.ExecQuery(sql,0) == 0 )
					{
						string appSecurity = mList.GetColumn("EnforceMenuItemSecurity");
						appStatus          = mList.GetColumn("ApplicationStatus").ToUpper();
						appCode            = mList.GetColumn("ApplicationCode");

						if ( appStatus == "A" )
							if ( appCode == "000" )
								goTo = "Register.aspx";
//							else if ( appCode == "001" || Tools.StringToInt(appCode) == 1 )
//								Response.Redirect("Blah.aspx");
//							else if ( appCode == "002" || Tools.StringToInt(appCode) == 2 )
//								Response.Redirect("Blah.aspx");
					}

				Tools.LogInfo("Home.PageLoad/1","url=" + url + ", domain=" + dName + ", appCode=" + appCode + ", appStatus=" + appStatus,233);

				Response.Redirect(goTo);
			}	
			catch (Exception ex)
			{
				Tools.LogException("Home.PageLoad/2","url="+url,ex);
			}
		}
	}
}