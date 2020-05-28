using System;
using System.Web;

// Developed by Paul Kilfoil
// http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public abstract class BasePage : StdDisposable
	{
		override protected void OnInit(EventArgs e) // You must set AutoEventWireup="false" in the ASPX page
		{
			base.OnInit(e);
			this.Load += new System.EventHandler(this.PageLoad);
		}

		protected void StartOver(int errNo)
		{
			StartOver(errNo,"");
		}

		protected void StartOver(int errNo,string pageName)
		{
			if ( pageName.Length < 6 )
				pageName = "XLogin.aspx";
			Response.Redirect ( pageName + ( errNo > 0 ? "?ErrNo=" + errNo.ToString() : "" ) , true );
		}

		public override void Close()
		{
		// This will automatically be called by the base class destructor (StdDisposable).

		//	Clean up the derived class
			CleanUp();

		//	Clean up the base class
		//	sessionGeneral = null;
		}

		public virtual void CleanUp()
		{
		//	This method can be overridden in the derived class to CLEAN UP stuff - not to initialize in the beginning
		//	Nothing here, so can completely override it in the derived class
		}

		protected abstract void PageLoad(object sender, EventArgs e);

		//	This method MUST be overridden in the derived class - it is the standard "Page Load" event
		// You must also set AutoEventWireup="false" in the ASPX page

		//	Put this in the derived class:
		//	protected override void PageLoad(object sender, EventArgs e)

	}
}
