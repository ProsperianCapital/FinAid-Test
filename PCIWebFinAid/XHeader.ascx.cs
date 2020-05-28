using System;

namespace PCIWebFinAid
{
	public partial class XHeader : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void ShowUser(string userName,string userAccess)
		{
			lblUName.Text = userName.Trim();
			lblURole.Text = userAccess.Trim();
		}
	}
}