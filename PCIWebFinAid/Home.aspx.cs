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
				int    k    = url.IndexOf("://");
				string sql  = url.Substring(k+3);
				string goTo = "XLogin.aspx";

				k = sql.ToUpper().IndexOf("/HOME.ASPX");
				if ( k > 0 )
					sql = sql.Substring(0,k);
				sql    = "exec sp_Get_BackOfficeApplication " + Tools.DBString(sql);

				using (MiscList mList = new MiscList())
					if ( mList.ExecQuery(sql,0) == 0 )
					{
						string appCode     = mList.GetColumn("ApplicationCode");
						string appSecurity = mList.GetColumn("EnforceMenuItemSecurity");
						string appStatus   = mList.GetColumn("ApplicationStatus").ToUpper();

						if ( appStatus == "A" )
							if ( appCode == "000" )
								goTo = "Register.aspx";
//							else if ( appCode == "001" )
//								Response.Redirect("Blah.aspx");
//							else if ( appCode == "002" )
//								Response.Redirect("Blah.aspx");
					}

				Response.Redirect(goTo);
			}	
			catch (Exception ex)
			{
				Tools.LogException("Home.PageLoad","url="+url,ex);
			}
		}
	}
}