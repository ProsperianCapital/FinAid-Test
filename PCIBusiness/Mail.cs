using System;
using System.Net;
using System.Net.Mail;

namespace PCIBusiness
{
	public class Mail : Message
	{
		private SmtpClient  smtp;
		private MailMessage msg;
//		private string      mailSender;

		private void AddMail(MailAddressCollection mailList,string mail)
		{
			try
			{
				mail = Tools.NullToString(mail);
				if ( mail.Length > 5 && ( mail.Contains(",") || mail.Contains(";") ) )
				{
					mail          = mail.Replace(";",",");
					string[] mArr = mail.Split(',');
					for ( int k = 0; k < mArr.Length ; k++ )
						if ( mArr[k].Length > 5 )
							mailList.Add(mArr[k].Trim());
				}
				else if ( Tools.CheckEMail(mail) )
					mailList.Add(mail);
			}
			catch
			{ }
		}

		public  string BCC
		{
			set { AddMail(msg.Bcc,value); }
		}

		public  string CC
		{
			set { AddMail(msg.CC,value); }
		}
		public  string To
		{
			set { AddMail(msg.To,value); }
		}
		public  string ReplyTo
		{
			set { AddMail(msg.ReplyToList,value); }
		}
		public  string From
		{
			set {	msg.From = new MailAddress(value); }
		}
//		public  string Sender
//		{
//			set {	msg.Sender = new MailAddress(value); }
//		}
		public  string Heading
		{
			set {	msg.Subject = value.Trim(); }
		}
		public  string Body
		{
			set {	msg.Body = value.Trim(); }
		}

		public override int Send()
		{
			if ( smtp == null )
				return 10;
			if ( msg == null )
				return 20;
			if ( msg.Body.Length < 10 )
				return 30;
			if ( msg.To.Count < 1 )
				return 40;

			if ( msg.Sender == null )
				try
				{
					msg.Sender = new MailAddress(Tools.ConfigValue("SMTP-User"));
				}
				catch
				{ }

			if ( msg.Sender == null )
				try
				{
					msg.Sender = new MailAddress(Tools.ConfigValue("SMTP-From"));
				}
				catch
				{ }

			if ( msg.From == null )
				msg.From = msg.Sender;

			msg.IsBodyHtml = msg.Body.ToUpper().Contains("<HTML");

			for ( int q = 1 ; q < 6 ; q++ ) // Try 5 times before quitting with an error
				try
				{
					smtp.Send(msg);
					return 0;
				}
				catch (Exception ex)
				{
					if ( q >= 3 ) // Log an error on the 3'rd failed attempt
						Tools.LogException("Mail.Send","EMail failure (try " + q.ToString() + "), to " + msg.To.ToString(), ex);
				}

			return 90;
		}

		public override void Clear()
		{
			base.Clear();
			msg.CC.Clear();
			msg.Bcc.Clear();
			msg.To.Clear();
			msg.ReplyToList.Clear();
//			msg.From    = null;
			msg.Subject = "";
			msg.Body    = "";
		}

		public override void Close()
		{
			Clear();
			msg  = null;
			smtp = null;
		}

		public Mail()
		{
			msg                 = new MailMessage();
			string smtpServer   = Tools.ConfigValue("SMTP-Server");
			string smtpUser     = Tools.ConfigValue("SMTP-User");
			string smtpPassword = Tools.ConfigValue("SMTP-Password");
			string smtpBCC      = Tools.ConfigValue("SMTP-BCC");
			int    smtpPort     = Tools.StringToInt(Tools.ConfigValue("SMTP-Port"));

			if ( smtpServer.Length   == 0 ) smtpServer   = Tools.ConfigValue("SMTP/Server");
			if ( smtpUser.Length     == 0 ) smtpUser     = Tools.ConfigValue("SMTP/User");
			if ( smtpPassword.Length == 0 ) smtpPassword = Tools.ConfigValue("SMTP/Password");
			if ( smtpBCC.Length      == 0 ) smtpBCC      = Tools.ConfigValue("SMTP/BCC");
			if ( smtpPort            == 0 ) smtpPort     = Tools.StringToInt(Tools.ConfigValue("SMTP/Port"));

			if ( smtpServer.Length > 0 )
			{
				smtp = new SmtpClient(smtpServer);
				if ( smtpPort > 0 )
					smtp.Port = smtpPort;
				smtp.UseDefaultCredentials = false;
				smtp.Credentials           = new NetworkCredential(smtpUser,smtpPassword);
			}
		}
	}
}