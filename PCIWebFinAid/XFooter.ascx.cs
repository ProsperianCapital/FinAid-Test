using System;

namespace PCIWebFinAid
{
	public partial class XFooter : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			lblJS.Text = "";

			if ( ! Page.IsPostBack )
			{
				hdnVer.Value   = "Version " + PCIBusiness.SystemDetails.AppVersion + " (" + PCIBusiness.SystemDetails.AppDate + ")";
				lblVer.Text    = hdnVer.Value;
//	Temporarily taken out ...
//				lblVer.Visible = ! PCIBusiness.Tools.SystemIsLive();
			}
		}

		public string JSText
		{
			get { return lblJS.Text.Trim(); }
			set { lblJS.Text = PCIBusiness.Tools.NullToString(value); }
		}
	}
}