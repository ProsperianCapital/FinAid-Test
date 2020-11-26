﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class pgChangePIN : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck() != 0 )
				return;
			if ( PageCheck()    != 0 )
				return;

			ClearData();

			if ( Page.IsPostBack )
				return;

			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode,ApplicationCode) == 0 )
				LoadDataInitial();
			else
				StartOver(13666);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		private void LoadDataInitial()
		{
//		Called once in the beginning

			LoadLabelText(ascxXMenu);
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			string pinOld  = txtPINOld.Text.Trim();
			string pinNew1 = txtPINNew1.Text.Trim();
			string pinNew2 = txtPINNew2.Text.Trim();

			if ( pinOld.Length < 5 || pinNew1.Length < 5 || pinNew2.Length < 5 || pinNew1 != pinNew2 )
				return;

			SetErrorDetail("",13667,"[SQL] Update yet to be implemented","",102,0);
		}
	}
}