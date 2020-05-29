using System;
using System.Collections;
using System.Collections.Generic;
using PCIBusiness;

namespace PCIWebFinAid
{
	public class MenuItem
	{
		private byte   level;
		private string menuCode;
		private string menuName;
		private string menuDescription;
		private string url;
		private List<MenuItem> subItems;

		public  byte    Level
		{
			get { return level; }
			set { level = value; }
		}
		public  string  Code
		{
			get { return menuCode; }
			set { menuCode = value; }
		}
		public string   Name
		{
			get { return Tools.NullToString(menuName); }
			set { menuName = value.Trim(); }
		}
		public string   Description
		{
			get { return Tools.NullToString(menuDescription); }
			set { menuDescription = value.Trim(); }
		}
		public string   URL
		{
			get { return Tools.NullToString(url); }
			set { url = value.Trim(); }
		}
		public List<MenuItem> SubItems
		{
			get
			{
				if ( subItems == null )
					subItems = new List<MenuItem>();
				return subItems;
			}
		}

		public void Setup(byte levelCode,MiscList mList)
		{
			string x        = "Level" + levelCode.ToString();
			level           = levelCode;
			menuCode        = mList.GetColumn(x+"ItemCode");
			menuName        = mList.GetColumn(x+"ItemDescription");
			menuDescription = mList.GetColumn(x+"ItemDescription");
			url             = mList.GetColumn("URL");
		}

	}
}