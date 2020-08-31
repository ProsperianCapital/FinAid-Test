using System.Collections.Generic;

namespace PCIBusiness
{
	public class MenuItems : BaseList
	{
		int err;

		public override BaseData NewItem()
		{
			return new   MenuItem();
		}
		public List<MenuItem> LoadMenu(string userCode, string applicationCode)
		{
			sql = "exec sp_Get_BackOfficeMenuB @UserCode="        + Tools.DBString(userCode)
	                                     + ",@ApplicationCode=" + Tools.DBString(applicationCode);
			err = ExecuteSQL(null,false,true);
			if ( err > 0 )
			{
				Tools.LogException("MenuItems.LoadMenu/5","err="+err.ToString()+" ("+sql+")");
				return null;
			}

			string         level1   = "X";
			string         level2   = "X";
			string         level3   = "X" ;
			string         level4   = "X";
			MenuItem       menu1    = null;
			MenuItem       menu2    = null;
			MenuItem       menu3    = null;
			MenuItem       menu4    = null;
			List<MenuItem> menuList = new List<MenuItem>();

			while ( ! dbConn.EOF )
			{
				if ( dbConn.ColString("Level1ItemDescription") != level1 )
				{
					level1      = dbConn.ColString("Level1ItemDescription");
					level2      = "X";
					level3      = "X";
					level4      = "X";
					menu1       = new MenuItem();
					menu1.Level = 1;
					menu1.LoadData(dbConn);
					menuList.Add(menu1);
				}
				if ( dbConn.ColString("Level2ItemDescription") != level2 )
				{
					level2      = dbConn.ColString("Level2ItemDescription");
					level3      = "X";
					level4      = "X";
					menu2       = new MenuItem();
					menu2.Level = 2;
					menu2.LoadData(dbConn);
					menu1.SubItems.Add(menu2);
				}
				if ( dbConn.ColString("Level3ItemDescription") != level3 )
				{
					level3      = dbConn.ColString("Level3ItemDescription");
					level4      = "X";
					menu3       = new MenuItem();
					menu3.Level = 3;
					menu3.LoadData(dbConn);
					menu2.SubItems.Add(menu3);
				}
				if ( dbConn.ColString("Level4ItemDescription") != level4 )
				{
					level4      = dbConn.ColString("Level4ItemDescription");
//					level5      = "X"; // We don't have 5 levels
					menu4       = new MenuItem();
					menu4.Level = 4;
					menu4.LoadData(dbConn);
					menu3.SubItems.Add(menu4);
				}
				dbConn.NextRow();
			}
			return menuList;
		}
	}
}