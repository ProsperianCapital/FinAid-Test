using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XMenu : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public int LoadMenu(string userCode)
		{
			userCode = Tools.NullToString(userCode);
			if ( userCode.Length < 1 )
				return 10;

			List<MenuItem> menuList;

			using (MiscList mList = new MiscList())
			{
				int ret = mList.ExecQuery("exec sp_Get_BackOfficeMenu " + Tools.DBString(userCode),0);
				if ( ret != 0 )
					return 20;
				if ( mList.EOF )
					return 30;

				int      level1 = 0;
				int      level2 = 0;
				int      level3 = 0;
				int      level4 = 0;
				MenuItem menu1  = null;
				MenuItem menu2  = null;
				MenuItem menu3  = null;
				MenuItem menu4  = null;
				menuList        = new List<MenuItem>();

				while ( ! mList.EOF )
				{
					if ( mList.GetColumnInt("Level1ItemCode") != level1 )
					{
						level1 = mList.GetColumnInt("Level1ItemCode");
						level2 = 0;
						level3 = 0;
						level4 = 0;
						menu1  = new MenuItem();
						menu1.Setup(1,mList);
						menuList.Add(menu1);
					}
					if ( mList.GetColumnInt("Level2ItemCode") != level2 )
					{
						level2 = mList.GetColumnInt("Level2ItemCode");
						level3 = 0;
						level4 = 0;
						menu2  = new MenuItem();
						menu2.Setup(2,mList);
						menu1.SubItems.Add(menu2);
					}
					if ( mList.GetColumnInt("Level3ItemCode") != level3 )
					{
						level3 = mList.GetColumnInt("Level3ItemCode");
						level4 = 0;
						menu3  = new MenuItem();
						menu3.Setup(3,mList);
						menu2.SubItems.Add(menu3);
					}
					mList.NextRow();
				}
			}
	
			StringBuilder str     = new StringBuilder();
			byte          menuNum = 0;
			string        menuID  = "";
			string        menuImg = "";
			string        tRowEnd = "</td></tr>" + Environment.NewLine;

			foreach (MenuItem m1 in menuList)
			{
				menuNum++;
				menuID = "mx" + menuNum.ToString();
				if      ( menuNum == 1 ) menuImg = "PCapital.png' height='75";
				else if ( menuNum == 2 ) menuImg = "PFintech.png' height='75";
				else if ( menuNum == 3 ) menuImg = "PWealth.png' height='75";
				else if ( menuNum == 4 ) menuImg = "PKnab.png' width='130";

				str.Append("<table id='" + menuID + "' style='position:absolute;left:152px;visibility:hidden;display:none;border:1px #000000 solid' onmouseleave=\"JavaScript:XMenu('',0)\">" + Environment.NewLine);
				str.Append("<tr><td class='VHead'>" + m1.Name + tRowEnd);

				foreach ( MenuItem m2 in m1.SubItems )
				{
					str.Append("<tr><td class='VMenu'>");
					if ( m2.SubItems.Count < 2 )
						str.Append(URLTag(m2) + tRowEnd);
					else
					{
						str.Append(" " + m2.Name + " " + tRowEnd);
						foreach ( MenuItem m3 in m2.SubItems )
						{
							str.Append("<tr><td class='VMenu'>&nbsp;&nbsp;->");
							if ( m3.SubItems.Count < 2 )
								str.Append(URLTag(m3) + tRowEnd);
							else
								str.Append(" " + m3.Name + " " + tRowEnd);
						}
					}
				}
				str.Append("</table>"+Environment.NewLine);
				str.Append("<a href=\"JavaScript:XMenu('" + menuID + "',1)\"><img src='Images/" + menuImg + "' title='" + m1.Description + "' onmouseover=\"JavaScript:XMenu('" + menuID + "',1)\" /></a>"+Environment.NewLine);
				str.Append("<br /><hr />"+Environment.NewLine);
			}

			lblMenu.Text = "<div style='float:left;vertical-align:top;padding:5px;margin-right:8px;background-color:black'>" + Environment.NewLine
			             + str.ToString()
			             + "<br /><div style='text-align:center'>"
					       + "<a href='XLogin.aspx' style='font-size:20px;font-weight:bold;text-decoration:none;color:white' onmouseover=\"JavaScript:XMenu('',0)\" title='Close all resources and log out'>Log Off</a>"
					       + "</div><br /></div>";
			menuList     = null;
			str          = null;
			return 0;
		}

		private string URLTag(MenuItem mx)
		{
			string T = "<a title='" + mx.Description + "' ";
			if ( string.IsNullOrWhiteSpace(mx.URL) )
				return T + "href=\"JavaScript:alert('You do not have access to this functionality')\"> " + mx.Name + " </a>";
			if ( mx.URL.ToUpper().StartsWith("HTTP") ) 
				return T + "href='" + mx.URL + "' target='_blank'> " + mx.Name + " </a>";
			return T + "href='" + mx.URL + "' onclick=\"JavaScript:ShowBusy('Loading ...',null,0)\"> " + mx.Name + " </a>";
		}
	}
}