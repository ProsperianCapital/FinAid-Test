using System;

namespace PCIWebFinAid
{
	public partial class XHeader : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void ShowUser(SessionGeneral sessionGeneral)
		{
			if ( sessionGeneral == null )
				lblUName.Text = "";
			else
			{
				lblUName.Text    = sessionGeneral.UserName;
				lblUName.ToolTip = "UserCode " + sessionGeneral.UserCode;
			//	lblURole.Text    = sessionGeneral.AccessName;
			}
		}
	}
}