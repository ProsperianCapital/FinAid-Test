using System;
using System.Net;

namespace PCIBusiness
{
	public abstract class Message : StdDisposable
	{
	//	Provider details
		protected string providerAddress;
		protected string providerID;
		protected string providerPassword;

	//	Result of Send()
		protected string resultMsg;
		protected string resultDetail;
		protected string resultCode;
		protected string resultID;

	//	Message details
		protected string messageCode;
		protected string messageBody;

		private DBConn   dbConn;

		public string ProviderAddress
		{
			get { return Tools.NullToString(providerAddress); }
			set { providerAddress = value.Trim(); }
		}
		public string ProviderID
		{
			get { return Tools.NullToString(providerID); }
			set { providerID = value.Trim(); }
		}
		public string ProviderPassword
		{
			get { return Tools.NullToString(providerPassword); }
			set { providerPassword = value.Trim(); }
		}

		public string   ResultMessage
		{
			get { return Tools.NullToString(resultMsg); }
		}
		public string   ResultDetail
		{
			get { return Tools.NullToString(resultDetail); }
		}
		public string   ResultCode
		{
			get { return Tools.NullToString(resultCode); }
		}
		public string   ResultID
		{
			get { return Tools.NullToString(resultID); }
		}

		public string   MessageCode
		{
			get { return Tools.NullToString(messageCode); }
			set { messageCode = value.Trim(); }
		}
		public string   MessageBody
		{
			get { return Tools.NullToString(messageBody); }
			set { messageBody = value.Trim(); }
		}

		public abstract int Send();

		public abstract byte LoadProvider();

		public abstract string Recipient { get; }

		public byte UpdateStatus()
		{
			if ( messageCode.Length < 1 )
				return 4;

			try
			{
				string sql = "exec sp_eMailStatus_Upd @Key = "           + Tools.DBString(messageCode)
				           +                        ",@StatusCode = "    + Tools.DBString(resultCode)
				           +                        ",@StatusMessage = " + Tools.DBString(resultMsg);

				if ( ! Tools.OpenDB(ref dbConn) )
					return 1;
				else if ( ! dbConn.Execute(sql,true) )
					return 2;
				return 0;
			}
			catch (Exception ex)
			{ }
			finally
			{
				Tools.CloseDB(ref dbConn);
			}
			return 3;
		}

		public virtual void Clear()
		{
			Tools.CloseDB(ref dbConn);
			dbConn       = null;
			messageBody  = "";
			messageCode  = "";
			resultMsg    = "";
			resultDetail = "";
			resultCode   = "";
			resultID     = "";
		}

		public override void Close()
		{
			Clear();
		}

		public Message()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			messageCode                          = "";
		}
	}
}