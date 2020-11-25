// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public abstract class BasePageRegister : BasePageLogin
	{
		protected override void StartOver(int errNo,string pageName="")
		{
			base.StartOver ( errNo, ( pageName.Length > 0 ? pageName : "RegisterEx3.aspx" ) );
		}
	}
}
