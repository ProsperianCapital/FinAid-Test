using System;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class LFinnHub : BasePageLogin
	{
		private struct RefreshData
		{
			public int    iter;
			public byte   status;
			public byte   ticker;
			public string interval;
			public string apiKey;
			public string stockList;
			public string forexList;
			public string baseCurr;
			public string exchange;
			public string exchangeList;
			public string result;
		}
		private RefreshData  pageParms;
		private int          ret;
		private string       dataOK;
		private string       dataErr;
		private const string fhURL = "https://finnhub.io/api/v1/";

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck(19)  != 0 )
				return;
//			if ( SecurityCheck(19) != 0 )
//				return;

			dataOK     = "<span style='color:green'>&nbsp;&nbsp;&nbsp;&nbsp;";
			dataErr    = "<span style='color:red'>&nbsp;&nbsp;&nbsp;&nbsp;";
			lblJS.Text = "";

			try
			{
				lblErr.Text = "";
				pageParms   = (RefreshData)Session["LFinnHubData"];
			}
			catch
			{
				pageParms.iter   = 0;
				pageParms.status = 1;
			}

			if ( Page.IsPostBack )
				return;

			if ( pageParms.iter > 0 )
			{
				txtKey.Text      = pageParms.apiKey;
				txtRefresh.Text  = pageParms.interval;
				txtBase.Text     = pageParms.baseCurr;
				txtStock.Text    = pageParms.stockList;
				txtForex.Text    = pageParms.forexList;
				lblData.Text     = pageParms.result;
				rdoTick1.Checked = ( pageParms.ticker == (int)Constants.TickerType.StockPrices );
				rdoTick5.Checked = ( pageParms.ticker == (int)Constants.TickerType.ExchangeCandles );
				rdoTick2.Checked = ( pageParms.ticker == (int)Constants.TickerType.ExchangeRates );
				lstExchange.Items.Add(new ListItem(pageParms.exchange,pageParms.exchange));
				if ( pageParms.status != 22 )
					lblJS.Text    = WebTools.JavaScriptSource("ChooseTicker("+pageParms.ticker.ToString()+")");
			}
			else
				lblJS.Text = WebTools.JavaScriptSource("ChooseTicker(0)");

			SetupFinnHub();
			StartStop(pageParms.status);
			lblDate.Text = Tools.DateToString(System.DateTime.Now,7,1); // yyyy-mm-dd hh:mm:ss
			ascxMenu.SetAdmin();
		}

		private void SetupFinnHub()
		{
			try
			{
				lstExchange.Items.Clear();
				string exchList = Tools.NullToString(pageParms.exchangeList);
				if ( exchList.Length == 0 )
					exchList = GetWebData (fhURL+"forex/exchange?token=" + txtKey.Text.Trim());
				if ( exchList.StartsWith("[") && exchList.EndsWith("]") && exchList.Length > 2 )
					exchList = exchList.Substring(1,exchList.Length-2);
				if ( exchList.Length > 0 )
				{
					pageParms.exchangeList = exchList;
					string[] exch          = exchList.Split(',');
					string   chgX;
					foreach ( string chg in exch )
					{
						chgX = chg.Replace("\"","");
						lstExchange.Items.Add(new ListItem(chgX,chgX));
						if ( Tools.NullToString(pageParms.exchange) == chgX )
							lstExchange.SelectedIndex = lstExchange.Items.Count-1;
					}
				}
			}
			catch
			{ }
		}

		private void StartStop(byte which)
		{
		//	btnStart.Text       = ( which != 22 ? "Start" : "Busy ..." );
			btnStart.Enabled    = ( which != 22 );
			txtKey.Enabled      = ( which != 22 );
			txtRefresh.Enabled  = ( which != 22 );
			txtStock.Enabled    = ( which != 22 );
			txtForex.Enabled    = ( which != 22 );
			lstExchange.Enabled = ( which != 22 );
			lstCurr.Enabled     = ( which != 22 );
			rdoTick1.Enabled    = ( which != 22 );
			rdoTick5.Enabled    = ( which != 22 );
			rdoTick2.Enabled    = ( which != 22 );

			btnStop.Enabled     = ( which == 22 );

			if ( which == 22 )
			{
				lblStatus.Text      = "<span style='color:red'>Busy</span>";
				lblRefresh.Text     = "<meta http-equiv='Refresh' content='" + txtRefresh.Text + "' />";
				pageParms.status    = 22;
				pageParms.interval  = txtRefresh.Text;
				pageParms.apiKey    = txtKey.Text;
				pageParms.baseCurr  = txtBase.Text;
				pageParms.stockList = txtStock.Text;
				pageParms.forexList = txtForex.Text;
				pageParms.exchange  = WebTools.ListValue(lstExchange,"ZAR");
				pageParms.ticker    = (byte)( rdoTick1.Checked ? (int)Constants.TickerType.StockPrices
				                          : ( rdoTick5.Checked ? (int)Constants.TickerType.ExchangeCandles
				                          : ( rdoTick2.Checked ? (int)Constants.TickerType.ExchangeRates : 0 ) ) );
				FetchData();
			}
			else
			{
				lblJS.Text       = WebTools.JavaScriptSource("ChooseTicker("+pageParms.ticker.ToString()+")");
				lblStatus.Text   = "<span style='color:green'>Inactive</span>";
				lblRefresh.Text  = "";
				pageParms.status = 11;
			}
		}

		private void FetchData()
		{
			if ( lblData.Text.Length > 2000 )
				lblData.Text = lblData.Text.Substring(0,1000);

			string   url;
			string   urlX;
			string   show;
			string   result  = "";
			string   curr    = "";
			string   symb    = "";
			string   token   = "token=" + txtKey.Text.Trim();
			string[] symbols = null;

			if ( rdoTick1.Checked )
			{
				ret     = 10;
				show    = "Stock";
				symbols = txtStock.Text.Split(',');
				url     = fhURL + "quote?" + token + "&symbol=";
			}
			else if ( rdoTick2.Checked )
			{
				ret     = 14;
				show    = "Currency";
				symbols = txtBase.Text.Split(',');
				url     = fhURL + "forex/rates?" + token
				                + "&base=";
			}
			else if ( rdoTick5.Checked )
			{
				ret     = 17;
				show    = "Currency";
				curr    = WebTools.ListValue(lstCurr,"ZAR").ToUpper();
				symbols = txtForex.Text.Split(',');
				url     = fhURL + "forex/candle?resolution=D&" + token
					             + "&from="   + DateTimeOffset.Now.Subtract(new TimeSpan(1,0,0,0)).ToUnixTimeSeconds().ToString()
						          + "&to="     + DateTimeOffset.Now.ToUnixTimeSeconds().ToString()
				                + "&symbol=" + WebTools.ListValue(lstExchange,"OANDA") + ":";
			}
			else
				return;

			for ( int k = 0 ; k < symbols.Length ; k++ )
				try
				{
					ret  = 20;
					symb = symbols[k].ToUpper().Trim();
					if ( symb.Length < 1 )
						continue;

					if ( rdoTick1.Checked )
					{
						urlX   = url + symb;
						result = result + "&nbsp;- " + show + " " + symb + "<br />";
					}
					else if ( rdoTick2.Checked )
					{
						urlX   = url + symb;
						result = result + "&nbsp;- " + show + " " + symb + "<br />";
					}
					else
					{
						urlX   = "";
						result = result + "&nbsp;- " + show + " " + curr + "->" + symb + "<br />";
						if ( symb == curr )
							result = result + dataErr + "Cannot convert";
						else
						{
							result = result + GetWebData (url+curr+"_"+symb,7) + "</span><br />";
							urlX   = url + symb + "_" + curr;
							result = result + "&nbsp;- " + show + " " + symb + "->" + curr + "<br />";
						}
					}
	
					result = result + GetWebData(urlX,7) + "</span><br />";
				}
				catch (Exception ex)
				{
					result = result + dataErr + "Error, ret="+ret.ToString()+"</span><br />";
					Tools.LogException("LFinnHub.FetchData","ret="+ret.ToString()+", "+url+symb,ex);
				}
				
			pageParms.iter++;
			ret                     = 80;
			pageParms.result        = "(" + pageParms.iter.ToString() + ") " + Tools.DateToString(System.DateTime.Now,7,1) + " : Fetch data<br />" + result + "<br />" + lblData.Text;
			lblData.Text            = pageParms.result;
			Session["LFinnHubData"] = pageParms;
		}


		private string GetWebData(string url,byte formatOutput=0)
		{
			if ( string.IsNullOrWhiteSpace(url) )
				return "";

			try
			{
				string         json       = "";
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
//				webRequest.Accept         = "text/xml";
				webRequest.ContentType    = "application/x-www-form-urlencoded";
				webRequest.Method         = "GET";
//				webRequest.KeepAlive      = false;
				ret                       = 80010;

				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 80020;;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret  = 80030;
						json = rd.ReadToEnd().Trim();
					}
				}

				webRequest = null;

				if ( formatOutput == 0 )
					return json;
				if ( json.Length < 1 )
					return "No data returned";
				if ( json.Length > 30 )
					json = json.Replace(",\"",",<br />\"");
				if ( json.StartsWith("{") )
					return dataOK + json;
				else
					return dataErr + json;
			}
			catch (Exception ex)
			{
				Tools.LogException("LFinnHub.GetWebData","ret="+ret.ToString()+", "+url,ex);
			}
			return ( formatOutput == 0 ? "" : dataErr ) + "Error, ret="+ret.ToString();
		}

		private int ValidatePage()
		{
			string apiKey  = txtKey.Text.Trim();
			string stocks  = txtStock.Text.Trim();
			string forex   = txtForex.Text.Trim();
			int    refresh = Tools.StringToInt(txtRefresh.Text);
			lblErr.Text    = "";

			if ( apiKey.Length < 4 )
				lblErr.Text = lblErr.Text + "Invalid API key<br />";
			if ( refresh < 5 || refresh > 1200 )
				lblErr.Text = lblErr.Text + "Invalid refresh interval (must be between 5 and 1200)<br />";
			else
				txtRefresh.Text = refresh.ToString();
			if ( rdoTick1.Checked && stocks.Length < 2 )
				lblErr.Text = lblErr.Text + "Invalid list of stock symbols<br />";
			if ( rdoTick5.Checked && forex.Length  < 2 )
				lblErr.Text = lblErr.Text + "Invalid list of currency symbols<br />";
			if ( rdoTick2.Checked && txtBase.Text.Trim().Length != 3 )
				lblErr.Text = lblErr.Text + "Invalid base currency<br />";

			return lblErr.Text.Length;
		}

		protected void btnStart_Click(Object sender, EventArgs e)
		{
			if ( ValidatePage() == 0 )
				StartStop(22);
			else
				lblJS.Text = WebTools.JavaScriptSource("ChooseTicker("+(rdoTick1.Checked?"1)":(rdoTick5.Checked?"5)":(rdoTick2.Checked?"2)":"0)"))));
		}

		protected void btnStop_Click(Object sender, EventArgs e)
		{
			StartStop(11);
		}

		protected void btnClear_Click(Object sender, EventArgs e)
		{
			lblData.Text     = "";
			txtStock.Text    = "";
			txtForex.Text    = "";
			txtRefresh.Text  = "10";
			rdoTick1.Checked = false;
			rdoTick5.Checked = false;
			rdoTick2.Checked = false;
			SessionClearData();
			StartStop(1);
		}
	}
}