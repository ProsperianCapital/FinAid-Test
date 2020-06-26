// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Web.UI.WebControls;
using PCIBusiness;

// Error codes 77000-77099

namespace PCIWebFinAid
{
	public partial class XTradingGateway : BasePageBackOffice
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode) != 0 )
				StartOver(10888);
			else
			{
				SetErrorDetail("",-888);
				LoadTickers();
			}
		}

		private void LoadTickers(int chgCode=0,int chgStatus=0)
		{
			Image   tImg;
			Literal tName;
			Label   tStatus;
			Button  tButton;
			string  tickerName;

			for ( int k = 1; k < 100; k++ )
			{
				tickerName = Tools.TickerName(k);
				if ( tickerName.Length > 0 )
				{
					tImg          = (Image)  FindControl("img"+k.ToString());
					if ( tImg    != null )   tImg.ImageUrl = "Images/LightR.png";
					tName         = (Literal)FindControl("lblName"+k.ToString());
					if ( tName   != null )   tName.Text = tickerName;
					tStatus       = (Label)  FindControl("lblStatus"+k.ToString());
					if ( tStatus != null )   tStatus.Text = "&nbsp;";
					tButton       = (Button) FindControl("btnStart"+k.ToString());
					if ( tButton != null )   tButton.Enabled = true;
					tButton       = (Button) FindControl("btnStop"+k.ToString());
					if ( tButton != null )   tButton.Enabled = false;
				}
			}

			try
			{
				using (MiscList tickStatus = new MiscList())
				{
					sql = "exec sp_TickerState";
					if ( chgStatus > 0 )
						sql = sql + " @TickerStatus='" + chgStatus.ToString() + "',";
					if ( chgCode > 0 )
						sql = sql + " @TickerCode='" + chgCode.ToString().PadLeft(3,'0') + "',";
					if ( sql.EndsWith(",") )
						sql = sql + " @UserCode=" + Tools.DBString(sessionGeneral.UserCode)
						          + ",@ActionOrigin='BackOffice.XTradingGateway.aspx'";

//	Use a different DB connection, called "DBConnTrade"

					if ( tickStatus.ExecQuery(sql,0,"",true,false,"DBConnTrade") != 0 )
						SetErrorDetail("LoadTickers",77010,"Unable to load ticker status (SQL error)",sql);
					else if ( tickStatus.EOF )
						SetErrorDetail("LoadTickers",77020,"Unable to load ticker status (no data returned)",sql);
					else
					{
						int tickerCode   = Tools.StringToInt(tickStatus.GetColumn("TickerCode"));
						int tickerStatus = Tools.StringToInt(tickStatus.GetColumn("TickerStatus"));
						if ( tickerCode > 0 && tickerStatus == (int)Constants.TickerAction.Run )
						{
							tImg          = (Image)  FindControl("img"+tickerCode.ToString());
							if ( tImg    != null )   tImg.ImageUrl = "Images/LightG.png";
							tStatus       = (Label)  FindControl("lblStatus"+tickerCode.ToString());
							if ( tStatus != null )   tStatus.Text = "Running&nbsp;";
							tButton       = (Button) FindControl("btnStart"+tickerCode.ToString());
							if ( tButton != null )   tButton.Enabled = false;
							tButton       = (Button) FindControl("btnStop"+tickerCode.ToString());
							if ( tButton != null )   tButton.Enabled = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				SetErrorDetail("LoadTickers",77030,"Unable to load ticker status (internal error)",sql,2,2,ex);
			}
		}

		protected void btnRefresh_Click(Object sender, EventArgs e)
		{
			LoadTickers();
		}

		protected void btnStop_Click(Object sender, EventArgs e)
		{
			int ticker = Tools.StringToInt(((Button)sender).ToolTip);
			LoadTickers(ticker,(int)Constants.TickerAction.Stop);
		}

		protected void btnStart_Click(Object sender, EventArgs e)
		{
			int ticker = Tools.StringToInt(((Button)sender).ToolTip);
			if ( ticker > 0 )
				LoadTickers(ticker,(int)Constants.TickerAction.Run);
		}

		protected void btnShutDown_Click(Object sender, EventArgs e)
		{
			LoadTickers(0,(int)Constants.TickerAction.ShutDown);
		}
	}
}