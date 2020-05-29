using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class ShowBusy : System.Web.UI.UserControl
	{
//		protected void Page_Load(object sender, EventArgs e)
//		{
//		//	Default message ...
//		//	Doesn't work here! This is now in the actual ASCX page
//			if ( ctlBusyMsg.Text.Length == 0 )
//				ctlBusyMsg.Text = "Your payment is being processed.<br /><br />Do <b>not</b> refresh the screen, navigate away or close your browser.";
//		}
		public string MessageCtlName
		{
			get { return ctlBusyMsg.ClientID; }
		}
		public string Message
		{
			get { return ctlBusyMsg.Text; }
			set { ctlBusyMsg.Text = value; }
		}
//		public bool ShowButtons
//		{
//			get { return ctlBusyButtons.Visible; }
//			set { ctlBusyButtons.Visible = value; }
//		}
	}
}