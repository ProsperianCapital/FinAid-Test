using System;

namespace PCIWebFinAid
{
	public static class AppDetails
	{
		public static string AppName = "PlaNet Technologies";

		public static string Summary()
		{
			return "<!--" + Environment.NewLine
			     + AppName + Environment.NewLine
			     + "Version " + PCIBusiness.SystemDetails.AppVersion + Environment.NewLine
			     + PCIBusiness.SystemDetails.AppDate + Environment.NewLine
			     + "(c) " + PCIBusiness.SystemDetails.Owner + Environment.NewLine
			     + "Developed by " + PCIBusiness.SystemDetails.Developer + Environment.NewLine
			     + "-->";
		}
	}
}
