using System;

namespace PCIWebFinAid
{
	public partial class XHeader : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void ShowUser(SessionGeneral sessionGeneral,string applicationCode)
		{
			if ( sessionGeneral == null )
			{
				lnkMessages.Visible = false;
				lblUName.Text       = "";
			}
			else
			{
				lnkMessages.Visible = true;
				lblUName.Text       = sessionGeneral.UserName;
				lblUName.ToolTip    = "UserCode " + sessionGeneral.UserCode;
			//	lblURole.Text       = sessionGeneral.AccessName;
			}
			if ( ! ("/001/002/003/004/005/006/007/008/009/").Contains("/"+applicationCode+"/") )
				applicationCode = "001";

			pnl001.Visible = ( applicationCode == "001" ); // BackOffice
			pnl002.Visible = ( applicationCode == "002" ); // CareAssist CRM
			pnl003.Visible = ( applicationCode == "003" );
			pnl004.Visible = ( applicationCode == "004" );
			pnl005.Visible = ( applicationCode == "005" );
			pnl006.Visible = ( applicationCode == "006" ); // Mobile app
			pnl007.Visible = ( applicationCode == "007" );
			pnl008.Visible = ( applicationCode == "008" );
			pnl009.Visible = ( applicationCode == "009" );
		}
	}
}