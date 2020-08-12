using System;
using System.Xml;


namespace PCIBusiness
{
	public abstract class Transaction : StdDisposable
	{
		protected string      payRef;
		protected string      payToken;
//		protected string      authCode;
		protected string      resultCode;
		protected string      resultStatus;
		protected string      resultMsg;
		protected string      xmlSent;
		protected string      bureauCode;
		protected string      bureauCodeTokenizer;
		protected string      bureauURL;
		protected string      strResult;
		protected XmlDocument xmlResult;

//	3d Stuff
		protected string      eci;
		protected string      paReq;
		protected string      termUrl;
		protected string      md;
		protected string      acsUrl;
		protected string      keyValuePairs;

		public  string      PaymentReference
		{
			get { return     Tools.NullToString(payRef); }
		}
//		public  string      AuthorizationCode
//		{
//			get { return     Tools.NullToString(authCode); }
//		}
		public  string      BureauCode
		{
			get { return     Tools.NullToString(bureauCode); }
		}
		public  virtual     string BureauURL
		{
			get { return     Tools.NullToString(bureauURL); }
		}
		public  string      PaymentToken
		{
			get { return     Tools.NullToString(payToken); }
		}
		public  string      ResultCode
		{
			get { return     Tools.NullToString(resultCode); }
			set { resultCode = value.Trim(); }
		}
		public  string      ResultStatus
		{
			get { return     Tools.NullToString(resultStatus); }
		}
		public  string      ResultMessage
		{
			get { return     Tools.NullToString(resultMsg); }
		}
		public  string      XMLSent
		{
			get { return     Tools.NullToString(xmlSent); }
		}
		public  string      XMLResult
		{
			get
			{
				try
				{
					return     xmlResult.InnerXml;
				}
				catch
				{ }
				try
				{
					return     Tools.NullToString(strResult);
				}
				catch
				{ }
				return "";
			}
		}
//		public  XmlDocument XMLResult
//		{
//			get { return     xmlResult; }
//		}

//		public  Constants.BureauStatus ProviderStatus
//		{
//			get
//			{
//				try
//				{
//				if ( Tools.NullToString(bureauCode).Length < 1 )
//					return Constants.BureauStatus.Unknown;
//				string status = Tools.ConfigValue("BureauStatus/"+bureauCode);
//				if ( status )
//			}
//		}

		public  bool      ThreeDRequired
		{
			get { return ( Tools.NullToString(acsUrl).Length > 0 ); }
		}
		public  string    ThreeDeci
		{
			get { return   Tools.NullToString(eci); }
		}
		public  string    ThreeDtermUrl
		{
			get { return   Tools.NullToString(termUrl); }
		}
		public  string    ThreeDpaReq
		{
			get { return   Tools.NullToString(paReq); }
		}
		public  string    ThreeDacsUrl
		{
			get { return   Tools.NullToString(acsUrl); }
		}
		public  string    ThreeDmd
		{
			get { return   Tools.NullToString(md); }
		}
		public  string    ThreeDKeyValuePairs
		{
			get { return   Tools.NullToString(keyValuePairs); }
		}


		public virtual int GetToken(Payment payment)
		{
			return 14010;
		}

		public virtual int DeleteToken(Payment payment)
		{
			return 14020;
		}

		public virtual int TokenPayment(Payment payment)
		{
			return 14030;
		}

		public virtual int CardPayment(Payment payment)
		{
			return 14040;
		}

		public virtual int CardPaymentThirdParty(Payment payment)
		{
			return 14050;
		}

      public virtual bool EnabledFor3d(byte transactionType)
		{
			if ( transactionType != (byte)Constants.TransactionType.ManualPayment )
				return true;

			resultCode = "99999";
			resultMsg  = "3D Secure payments are not supported for this provider";
			return false;
		}

		protected void LoadBureauDetails(Constants.PaymentProvider bureau)
		{
			bureauCode = Tools.BureauCode(bureau);
			bureauURL  = Tools.ConfigValue(bureauCode+"/URL");

			if ( bureauURL.Length > 0 )
				return;

			if ( Tools.SystemIsLive() )
			{
				if ( bureau == Constants.PaymentProvider.Peach )
					bureauURL = "https://www.oppwa.com/v1";
				else if ( bureau == Constants.PaymentProvider.PayGate )
					bureauURL = "https://secure.paygate.co.za/payhost/process.trans";
				else if ( bureau == Constants.PaymentProvider.TokenEx )
					bureauURL = "https://api.tokenex.com";
			}
			else
			{
				if ( bureau == Constants.PaymentProvider.Peach )
					bureauURL = "https://test.oppwa.com/v1";
				else if ( bureau == Constants.PaymentProvider.Ecentric )
					bureauURL = "https://sandbox.ecentricswitch.co.za:8443/paymentgateway/v1";
				else if ( bureau == Constants.PaymentProvider.eNETS )
					bureauURL = "https://uat-api.nets.com.sg:9065/GW2/TxnReqListener";
				else if ( bureau == Constants.PaymentProvider.PayGate )
					bureauURL = "https://secure.paygate.co.za/payhost/process.trans";
				else if ( bureau == Constants.PaymentProvider.PayGenius )
					bureauURL = "https://developer.paygenius.co.za";
				else if ( bureau == Constants.PaymentProvider.PayU )
					bureauURL = "https://staging.payu.co.za";
				else if ( bureau == Constants.PaymentProvider.T24 )
					bureauURL = "https://payment.ccp.transact24.com";
				else if ( bureau == Constants.PaymentProvider.TokenEx )
					bureauURL = "https://test-api.tokenex.com";
			}
		}

      public override void Close()
		{
			xmlResult = null;
		}

		public Transaction()
		{
			bureauCodeTokenizer = Tools.BureauCode(Constants.PaymentProvider.TokenEx);
			bureauCode          = "";
			bureauURL           = "";
			payRef              = "";
			payToken            = "";
//			authCode            = "";
			resultCode          = "";
			resultMsg           = "";
			xmlSent             = "";
			strResult           = "";
			eci                 = "";
			paReq               = "";
			termUrl             = "";
			md                  = "";
			acsUrl              = "";
			keyValuePairs       = "";
			xmlResult           = null;
		}
	}
}
