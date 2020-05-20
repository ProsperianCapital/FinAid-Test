namespace PCIWebFinAid
{
	public partial class SessionGeneral
	{
//		User details
		private string  clientCode;
		private string  contractCode;
//		private string  clientName;
		private string  accessType;

//		Product details
		private string  productCode;
		private string  languageCode;
		private string  languageDialectCode;

//		System details
		private string  startPage;

		public  string  ClientCode
		{
			get { return clientCode; }
			set { clientCode = value.Trim(); }
		}
		public  string  ContractCode
		{
			get { return contractCode; }
			set { contractCode = value.Trim(); }
		}

//		public  string  ClientName
//		{
//			get { return clientName; }
//			set { clientName = value.Trim(); }
//		}
		public  string  AccessType
		{
			get
			{
				if ( ClientCode.Length < 1 )
					accessType = "";
				else
				{
					accessType = PCIBusiness.Tools.NullToString(accessType).ToUpper();
					if ( accessType.Length < 1 )
						accessType = "N"; // Client/Normal = "N", Admin = "A"
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
					if ( AccessType == "A" ) // Admin
						startPage = "LAdmin.aspx";
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

		public void Clear()
		{
			clientCode          = "";
			contractCode        = "";
//			clientName          = "";
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
