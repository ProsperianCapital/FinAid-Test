using System;
using System.Text;
using System.Collections.Generic;

namespace PCIBusiness
{
	public class MiscList : BaseList
	{
		private int colNo;

		public override BaseData NewItem()
		{
			return new MiscData();
		}

		public int ExecQuery(string sqlQuery,byte loadRows,string dataClass="",bool noRowsIsError=true,bool alwaysClose=false)
		{
			sql = sqlQuery;

			if ( loadRows == 0 )
				return base.ExecuteSQL(null,alwaysClose,noRowsIsError);

			if ( string.IsNullOrWhiteSpace(dataClass) )
				return base.LoadDataFromSQL();

//	The above creates "MiscData" objects.
//	Below creates objects of type {dataClass} using Reflection.

			string err = "Invalid class name";
			try
			{
				Type classType  = (System.Reflection.Assembly.Load("PCIBusiness")).GetType("PCIBusiness."+dataClass);
				if ( classType != null )
					return base.LoadDataFromSQL(null,0,classType);
			}
			catch (Exception ex)
			{
				err = ex.Message;
			}
			Tools.LogException("MiscList.ExecQuery",err + " (DataClass=" + dataClass + ", SQL=" + sqlQuery + ")");
			return 0;
		}

		public void Add(MiscData dataX)
		{
			if ( objList == null )
				objList = new List<BaseData>();
			objList.Add(dataX);
		}

		public string GetColumn(int colNumber)
		{
			try
			{
				if ( dbConn != null )
					return dbConn.ColValue(colNumber);
			}
			catch
			{ }
			return "";
		}
		public string GetColumn(string colName,byte errorMode=1)
		{
			try
			{
				if ( dbConn != null )
				{
					int x = dbConn.ColNumber(colName,errorMode);
					if ( x >= 0 )
						return GetColumn(x);	
				}
			}
			catch
			{ }
			return "";
		}
		public int GetColumnInt(string colName,byte errorMode=1)
		{
			try
			{
				return dbConn.ColLong(colName,0,errorMode);

//				if ( dbConn != null )
//				{
//					int x = dbConn.ColNumber(colName,errorMode);
//					if ( x >= 0 )
//						return dbConn.ColLong(x);
//				}
			}
			catch
			{ }
			return 0;
		}
		public DateTime GetColumnDate(string colName,byte errorMode=1)
		{
			try
			{
				return dbConn.ColDate(colName,0,errorMode);

//				if ( dbConn != null )
//				{
//					int x = dbConn.ColNumber(colName,errorMode);
//					if ( x >= 0 )
//						return dbConn.ColDate(x);
//				}
			}
			catch
			{ }
			return Constants.C_NULLDATE();
		}
		public string GetColumnCurrency(string colName,byte errorMode=1)
		{
			try
			{
				if ( dbConn != null )
				{
					int x = dbConn.ColNumber(colName,errorMode);
					if ( x >= 0 )
					{
						string y = GetColumn(x);
						if ( string.IsNullOrWhiteSpace(y) )
							return "0.00";
						x = y.IndexOf(".");
						if ( x < 0 )
							return y + ".00";
						y = y + "000";
						if ( x == 0 )
							return "0." + y.Substring(1,2);
						return y.Substring(0,x+3);
					}
				}
			}
			catch
			{ }
			return "0.00";
		}
		public string NextColumn
		{
			get
			{
				if ( colNo < 0 )
					colNo = 0;
				else
					colNo++;
				return GetColumn(colNo);
			}
		}

		public bool EOF
		{
			get
			{
				if ( dbConn == null )
					return true;
				return dbConn.EOF;
			}
		}

		public bool NextRow()
		{
			if ( dbConn == null )
				return false;
			return dbConn.NextRow();
		}
	}
}
