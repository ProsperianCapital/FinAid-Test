using System;
using System.Text;
using System.Collections.Generic;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XMenu : System.Web.UI.UserControl
	{
		private string userCode;
		private string applicationCode;

		protected void Page_Load(object sender, EventArgs e)
		{
			userCode = "";
		}

		public int LoadMenu(string sessionUserCode,string sessionAppCode)
		{
			userCode        = Tools.NullToString(sessionUserCode);
			applicationCode = Tools.NullToString(sessionAppCode);
			if ( userCode.Length < 1 || applicationCode.Length < 1 )
				return 10;

			List<MenuItem> menuList;

			using (MiscList mList = new MiscList())
			{
				int ret = mList.ExecQuery("exec sp_Get_BackOfficeMenuA @UserCode="        + Tools.DBString(userCode)
				                                                   + ",@ApplicationCode=" + Tools.DBString(applicationCode),0);
				if ( ret != 0 )
					return 20;
				if ( mList.EOF )
					return 30;

				string   level1 = "X";
				string   level2 = "X";
				string   level3 = "X";
				string   level4 = "X";
				MenuItem menu1  = null;
				MenuItem menu2  = null;
				MenuItem menu3  = null;
				MenuItem menu4  = null;
				menuList        = new List<MenuItem>();

				while ( ! mList.EOF )
				{
					if ( mList.GetColumn("Level1ItemCode") != level1 )
					{
						level1 = mList.GetColumn("Level1ItemCode");
						level2 = "X";
						level3 = "X";
						level4 = "X";
						menu1  = new MenuItem();
						menu1.Setup(1,mList);
						menuList.Add(menu1);
					}
					if ( mList.GetColumn("Level2ItemCode") != level2 )
					{
						level2 = mList.GetColumn("Level2ItemCode");
						level3 = "X";
						level4 = "X";
						menu2  = new MenuItem();
						menu2.Setup(2,mList);
						menu1.SubItems.Add(menu2);
					}
					if ( mList.GetColumn("Level3ItemCode") != level3 )
					{
						level3 = mList.GetColumn("Level3ItemCode");
						level4 = "X";
						menu3  = new MenuItem();
						menu3.Setup(3,mList);
						menu2.SubItems.Add(menu3);
					}
					if ( mList.GetColumn("Level4ItemCode") != level4 )
					{
						level4 = mList.GetColumn("Level4ItemCode");
//						level5 = "X"; // We don't have 5 levels
						menu4  = new MenuItem();
						menu4.Setup(4,mList);
						menu3.SubItems.Add(menu4);
					}
					mList.NextRow();
				}
			}
	
			StringBuilder str     = new StringBuilder();
			StringBuilder sub     = new StringBuilder();
			byte          menuNum = 0;
			byte          subNum  = 0;
			string        menuID  = "";
			string        menuRef = "";
			string        tRowEnd = "</td></tr>" + Environment.NewLine;

			foreach (MenuItem m1 in menuList)
			{
				menuNum++;
				menuID = "mx" + menuNum.ToString();
//	Ver 1
//				if      ( menuNum == 1 ) menuRef = "PCapital.png' height='75";
//				else if ( menuNum == 2 ) menuRef = "PFintech.png' height='75";
//				else if ( menuNum == 3 ) menuRef = "PWealth.png' height='75";
//				else if ( menuNum == 4 ) menuRef = "PKnab.png' width='130";

				if ( m1.DisplayImageOrText == "0" )
// Ver 2
//					menuRef = "<img src='Images/" + m1.ImageName + "' title='" + m1.Description + "' onmouseover=\"JavaScript:XMenu('" + menuID + "',1)\" />";
//	Ver 3
//					menuRef = "><img src='Images/" + m1.ImageName + "' title='" + m1.Description + "' style='width:130px' />";
//	Ver 4
					menuRef = "><img src='Images/" + m1.ImageName + "' title='" + m1.Description + "' style='height:75px' />";
				else
					menuRef = " class='VText'>" + m1.Name;

				str.Append("<table id='" + menuID + "' style='position:absolute;left:152px;visibility:hidden;display:none;border:1px #000000 solid' onmouseleave=\"JavaScript:XMenu('',0)\">" + Environment.NewLine);
				str.Append("<tr><td class='VHead'>" + m1.Name + tRowEnd);

				foreach ( MenuItem m2 in m1.SubItems )
				{
					str.Append("<tr><td class='VMenu' onmouseover='XSubMenu(null)'>");
					if ( m2.SubItems.Count < 2 )
						str.Append(URLTag(m2) + tRowEnd);
					else
					{
						str.Append(" " + m2.Name + " " + tRowEnd);
						foreach ( MenuItem m3 in m2.SubItems )
						{
							str.Append("<tr><td class='VMenu'");
							if ( m3.SubItems.Count < 2 )
								str.Append(" onmouseover='XSubMenu(null)'>&nbsp;&nbsp;->" + URLTag(m3) + tRowEnd);
							else
							{
								string mID = "vx" + (++subNum).ToString();
								str.Append(" onmouseover=\"XSubMenu('"+mID+"',this)\">&nbsp;&nbsp;->");
								str.Append(" " + m3.Name + " " + tRowEnd);
								sub.Append(Environment.NewLine+"<table id='"+mID+"' style='border:1px solid #000000;position:absolute;visibility:hidden;display:none' onmouseleave='JavaScript:XSubMenu(null)'>");
								foreach ( MenuItem m4 in m3.SubItems )
									sub.Append("<tr><td class='VMenu'>" + URLTag(m4) + tRowEnd);
								sub.Append("</table>");
							}
						}
					}
				}
				str.Append("</table>"+Environment.NewLine);
				str.Append("<a href=\"JavaScript:XMenu('" + menuID + "',1)\" onmouseover=\"JavaScript:XMenu('" + menuID + "',1)\"" + menuRef + "</a>"+Environment.NewLine);
				str.Append("<hr />"+Environment.NewLine);
			}

//	Exclude "Log Off"
			if ( str.ToString().EndsWith("<br /><hr />"+Environment.NewLine) )
			{
				str.Append("[X*Z]");
				str.Replace("<br /><hr />"+Environment.NewLine+"[X*Z]","");
			}
			lblMenu.Text = "<div style='float:left;vertical-align:top;padding:5px;margin-right:8px;background-color:black'>" + Environment.NewLine
			             + str.ToString()
			             + "</div>" + Environment.NewLine
			             + sub.ToString();
//	Include "Log Off"
//			lblMenu.Text = "<div style='float:left;vertical-align:top;padding:5px;margin-right:8px;background-color:black'>" + Environment.NewLine
//			             + str.ToString()
//			             + "<br /><div style='text-align:center'>"
//					       + "<a href='pgLogon.aspx' class='VText' onmouseover=\"JavaScript:XMenu('',0)\" title='Close all resources and log out'>Log Off</a>"
//					       + "</div><br /></div>" + Environment.NewLine
//			             + sub.ToString();

			menuList     = null;
			str          = null;
			return 0;
		}

		private string URLTag(MenuItem mx)
		{
			string T = "<a " + ( mx.Description.Length > 0 ? "title='" + mx.Description.Replace("'","&#39;") + "' " : "" );
			if ( string.IsNullOrWhiteSpace(mx.URL) )
				return T + "href=\"JavaScript:alert('You do not have access to this functionality')\"> " + mx.Name + " </a>";

			mx.URL = mx.URL.Replace("{usercode}",userCode);
			mx.URL = mx.URL.Replace("{UserCode}",userCode);
			mx.URL = mx.URL.Replace("{USERCODE}",userCode);
			T      = T + "href='" + mx.URL + "'";

			if ( mx.URL.ToUpper().StartsWith("HTTP") || mx.URL.ToUpper().StartsWith("WWW") )
				if ( mx.Code.Length > 0 )
					return T + " target='" + mx.Code.Replace(" ","") + "'> " + mx.Name + " </a>";
				else
					return T + " target='_blank'> " + mx.Name + " </a>";

			return T + " onclick=\"JavaScript:ShowBusy('Loading ...',null,0)\"> " + mx.Name + " </a>";
		}
	}
}