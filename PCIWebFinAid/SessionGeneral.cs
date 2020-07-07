﻿namespace PCIWebFinAid
{
	public partial class SessionGeneral
	{
//		User details
		private string  userCode;
		private string  userName;
		private string  accessType;

//		Admin stuff

//		Client stuff
		private string  contractCode;
		private string  productCode;
		private string  languageCode;
		private string  languageDialectCode;

//		System details
		private string  startPage;
		private string  applicationCode;

		public  string  UserCode
		{
			get { return PCIBusiness.Tools.NullToString(userCode); }
			set { userCode = value.Trim(); }
		}
		public  string  UserName
		{
			get { return PCIBusiness.Tools.NullToString(userName); }
			set { userName = value.Trim(); }
		}
		public  string  ContractCode
		{
			get { return PCIBusiness.Tools.NullToString(contractCode); }
			set { contractCode = value.Trim(); }
		}
		public  string  ApplicationCode
		{
			get { return PCIBusiness.Tools.NullToString(applicationCode); }
			set { applicationCode = value.Trim(); }
		}
		public  bool    AdminUser
		{
			get { return AccessType == "A" || AccessType == "P"; }
		}
		public  string  AccessName
		{
			get
			{
				if ( AccessType == "N" ) return "Client";
				if ( AccessType == "A" ) return "Admin";
				if ( AccessType == "P" ) return "Admin";
				if ( AccessType == "X" ) return "Not secure";
				return "AccessType " + AccessType;
			}
		}

		public  string  AccessType
		{
			get
			{
				if ( UserCode.Length < 1 )
					accessType = "Q";
				else
				{
					accessType = PCIBusiness.Tools.NullToString(accessType).ToUpper();
					if ( accessType.Length < 1 )
						accessType = "N"; // Client/Normal = "N", Admin = "A"
					else if ( accessType.Length > 1 )
						accessType = accessType.Substring(0,1);
				}
				return accessType;
			}
			set { accessType = value.Trim().ToUpper(); }
		}

		public string   StartPage
		{
			get
			{
				if ( PCIBusiness.Tools.NullToString(startPage).Length < 6 )
					if ( AccessType == "P" ) // Admin
						startPage = "XHome.aspx";
					else if ( AccessType == "A" ) // Admin
						startPage = "LAdmin.aspx";
					else if ( AccessType == "X" ) // Login not confirmed
						startPage = "XLogin.aspx";
					else
						startPage = "LWelcome.aspx";
				return startPage;
			}
			set { startPage = value.Trim(); }
		}
		public  string  ProductCode
		{
			get { return productCode; }
			set { productCode = value.Trim(); }
		}
		public  string  LanguageCode
		{
			get { return languageCode; }
			set { languageCode = value.Trim(); }
		}
		public  string  LanguageDialectCode
		{
			get { return languageDialectCode; }
			set { languageDialectCode = value.Trim(); }
		}

		public int      CheckExpiry
		{
			get {	return ( userCode.Length > 0 ? 1 : 0 ); }
		}

		public void Clear()
		{
			userCode            = "";
			contractCode        = "";
			userName            = "";
			accessType          = "";
			productCode         = "";
			languageCode        = "";
			languageDialectCode = "";
			startPage           = "";
		}

		public SessionGeneral()
		{
			Clear();
		}
		~SessionGeneral()
		{
			Clear();
		}
	}
}
