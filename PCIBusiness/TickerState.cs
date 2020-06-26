using System;

namespace PCIBusiness
{
	public class TickerState : BaseData
	{
		private string    dbConnection;
		private string    tickerCode;
		private string    tickerStatus;
		private string    userCode;
		private string    origin;
		private DateTime  dateUpdated;

		public  string    DBConnection
		{
			get { return   Tools.NullToString(dbConnection); }
			set { dbConnection = value.Trim(); }
		}
		public  string    TickerCode
		{
			get { return   Tools.NullToString(tickerCode); }
			set { tickerCode = value.Trim(); }
		}
		public  string    TickerStatus
		{
			get { return   Tools.NullToString(tickerStatus); }
			set { tickerStatus = value.Trim(); }
		}
		public  string    UserCode
		{
			get { return   Tools.NullToString(userCode); }
			set { userCode = value.Trim(); }
		}
		public  string    Origin
		{
			get { return   Tools.NullToString(origin); }
			set { origin = value.Trim(); }
		}

		private int DoSQL()
		{
			try
			{
				Tools.LogInfo("TickerState.DoSQL/1",sql,222);
				int ret = ExecuteSQL(null,1,false,DBConnection);
				if ( ret == 0 )
					LoadData(dbConn);
				else
					Tools.LogInfo("TickerState.DoSQL/2","Error, ret="+ret.ToString(),222);
				return ret;
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TickerState.DoSQL/3",ex.Message,222);
				Tools.LogException("TickerState.DoSQL/4",sql,ex);
			}
			finally
			{
				Tools.CloseDB(ref dbConn);
			}
			return 205;
		}

		public int Update()
		{
			if ( TickerStatus.Length < 1 )
				return 103;

			sql = "exec sp_TickerState  @TickerStatus=" + Tools.DBString(TickerStatus)
			                        + ",@TickerCode="   + Tools.DBString(TickerCode)
			                        + ",@UserCode="     + Tools.DBString(UserCode)
			                        + ",@ActionOrigin=" + Tools.DBString(Origin);
			return DoSQL();
		}

		public int Enquire()
		{
			sql = "exec sp_TickerState";
			return DoSQL();
		}

		public override void LoadData(DBConn dbConn)
		{
			dbConn.SourceInfo = "TickerState.LoadData";
			tickerCode        = dbConn.ColString("TickerCode");
			tickerStatus      = dbConn.ColString("TickerStatus");
			userCode          = dbConn.ColString("UserCode");
			origin            = dbConn.ColString("ActionOrigin");
		}
	}
}
