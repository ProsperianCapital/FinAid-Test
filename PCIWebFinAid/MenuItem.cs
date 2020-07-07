﻿using System;
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
		private string imageName;
		private string displayImageOrText;
		private List<MenuItem> subItems;

		public  byte    Level
		{
			get { return level; }
			set { level = value; }
		}
		public  string  Code
		{
			get { return Tools.NullToString(menuCode); }
			set { menuCode = value.Trim(); }
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
		public string   DisplayImageOrText
		{
			get { return Tools.NullToString(displayImageOrText); }
		}
		public string   ImageName
		{
			get { return Tools.NullToString(imageName); }
		}
		public string   URL
		{
			get
			{
				url = Tools.NullToString(url);
				if ( url.ToUpper() == "TBA" )
					url = "XHome.aspx";
				return url;
			}
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
			string x           = "Level" + levelCode.ToString();
			level              = levelCode;
			menuCode           = mList.GetColumn(x+"ItemCode");
			menuName           = mList.GetColumn(x+"ItemDescription");
			menuDescription    = mList.GetColumn(x+"ItemDescription");
			url                = mList.GetColumn("URL");
			imageName          = mList.GetColumn("ImageFileName");
			displayImageOrText = mList.GetColumn("DisplayMenuText");
//	Testing
//			url                = mList.GetColumn("URL") + "?UserCode={UserCode}";
		}
	}
}