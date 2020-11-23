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
				string goTo      = "pgLogon.aspx";
				string parms     = "";
//				string appCode   = "*";
				string appStatus = "*";
				ApplicationCode  = "";

				k = dName.ToUpper().IndexOf("/HOME.ASPX");
				if ( k > 0 )
					dName = dName.Substring(0,k);
				k = url.IndexOf("?");
				if ( k > 0 )
					parms = url.Substring(k);
				string sql = "exec sp_Get_BackOfficeApplication " + Tools.DBString(dName);

				using (MiscList mList = new MiscList())
					if ( mList.ExecQuery(sql,0) == 0 )
					{
						string appSecurity = mList.GetColumn("EnforceMenuItemSecurity");
						appStatus          = mList.GetColumn("ApplicationStatus").ToUpper();
						ApplicationCode    = mList.GetColumn("ApplicationCode");

						if ( appStatus == "A" )
							if ( ApplicationCode == "000" )
								goTo = "Register.aspx";
					}

				Tools.LogInfo("Home.PageLoad/1","url="       + url
				                            + ", domain="    + dName
				                            + ", goTo="      + goTo
				                            + ", parms="     + parms
				                            + ", appCode="   + ApplicationCode
				                            + ", appStatus=" + appStatus,233);

				Response.Redirect(goTo+parms);
			}	
			catch (System.Threading.ThreadAbortException)
			{
			//	Ignore
			}
			catch (Exception ex)
			{
				Tools.LogException("Home.PageLoad/2","url="+url,ex);
			}
		}
	}
}