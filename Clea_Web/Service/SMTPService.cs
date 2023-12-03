using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Net;
using System.Text;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
	//郵件發送模組
	public class SMTPService : BaseService
	{
		private IConfiguration configuration;
		public SMTPService(dbContext dbContext, IConfiguration configuration)
		{
			db = dbContext;
			this.configuration = configuration;
		}

		#region Mail Object
		public class SmtpConfig
		{
			/// <summary>
			/// SMTP位址
			/// </summary>
			public string SMTP_PATH { get; set; }
			/// <summary>
			/// SMTP埠號
			/// </summary>
			public string SMTP_PORT { get; set; }
			/// <summary>
			/// 寄件者郵件
			/// </summary>
			public string SMTP_SENDERMAIL { get; set; }
			/// <summary>
			/// 寄件者名稱
			/// </summary>
			public string SMTP_SENDERMAILNAME { get; set; }
			/// <summary>
			/// SMTP認證帳號
			/// </summary>
			public string SMTP_USERNAME { get; set; }
			/// <summary>
			/// SMTP認證密碼
			/// </summary>
			public string SMTP_PASSWORD { get; set; }
			/// <summary>
			/// 是否使用SSL
			/// </summary>
			public string SMTP_USESSL { get; set; }
			/// <summary>
			/// DOMAIN名稱
			/// </summary>
			public string SMTP_DOMAINNAME { get; set; }
			/// <summary>
			/// IIS路徑
			/// </summary>
			public string SMTP_IISPATH { get; set; }
			public string SMTP_BATCHNUM { get; set; }
		}

		public class FileData
		{
			/// <summary>
			/// 檔案位置
			/// </summary>
			public string FilePath { get; set; }

			/// <summary>
			/// 檔案名稱
			/// </summary>
			public string FileNewName { get; set; }
		}
		#endregion

		#region
		public Boolean SendMail(List<string> toEmail, string subject, string content, List<string> ccEmail = null, List<string> BccEmail = null, List<FileData> FileList = null)
		{
			string smtp_sendermailname = configuration.GetValue<String>("MailInfo:SenderName");
			string smtp_sendermail = configuration.GetValue<String>("MailInfo:SenderMailName");
			string smtp_host = configuration.GetValue<String>("MailInfo:ServerDomain");
			string smtp_port = configuration.GetValue<String>("MailInfo:MailPort");
			string maileDomainName = configuration.GetValue<String>("MailInfo:ServerDomain");
			string smtp_username = configuration.GetValue<String>("MailInfo:MailFullAccount");
			string smtp_password = configuration.GetValue<String>("MailInfo:MailPassword");
			string smtp_usessl = string.Empty;
			string fromEmail = smtp_sendermail;
			string fromName = smtp_sendermailname;

			MailAddress from = new MailAddress(fromEmail, fromName, Encoding.UTF8);

			MailMessage mail = new MailMessage();
			mail.From = from;

			if (toEmail != null && toEmail.Count() > 0)
			{
				foreach (string addr in toEmail)
				{
					mail.To.Add(new MailAddress(addr));
				}
			}

			if (ccEmail != null && ccEmail.Count() > 0)
			{
				foreach (string addr in ccEmail)
				{
					mail.CC.Add(new MailAddress(addr));
				}
			}

			if (BccEmail != null && BccEmail.Count() > 0)
			{
				foreach (string addr in BccEmail)
				{
					mail.Bcc.Add(new MailAddress(addr));
				}
			}

			if (mail.To == null && mail.Bcc == null && mail.CC == null)
			{
				return false;
			}

			//信件夾帶附件
			if (FileList != null)
			{
				try
				{
					foreach (var item in FileList)
					{
						Attachment attachment = new Attachment(item.FilePath);
						if (!string.IsNullOrEmpty(item.FileNewName))
						{
							attachment.ContentDisposition.FileName = item.FileNewName;
						}
						mail.Attachments.Add(attachment);
					}
				}
				catch
				{
				}

			}

			mail.SubjectEncoding = Encoding.UTF8;
			mail.Subject = subject;
			mail.BodyEncoding = Encoding.UTF8;
			mail.Body = content;
			mail.IsBodyHtml = true;
			mail.Priority = MailPriority.Normal;


			SmtpClient client = new SmtpClient();
			client.Host = smtp_host;
			client.Timeout = 200000;
			client.UseDefaultCredentials = false;
			client.Port = int.Parse(smtp_port);
			client.EnableSsl = false;
			
			if (smtp_username != "")
			{
				client.Credentials = new NetworkCredential(smtp_username, smtp_password);
			}

			try
			{						
				client.Send(mail);
				return true;
			}
			catch (Exception ex)
			{
				//msg = ex.ToString();
				return false;
			}
		}
		#endregion
	}
}

