using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using Clea_Web.ViewModels;
using Clea_Web.Models;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using Microsoft.Office.Interop.PowerPoint;

namespace Clea_Web.Service
{
	public class FileService : BaseService
	{
		private IConfiguration configuration;

		public FileService(dbContext dbContext, IConfiguration configuration)
		{
			db = dbContext;
			this.configuration = configuration;
		}


		#region ViewModel
		public class SysFileInfo
		{
			/// <summary>
			/// SYS_File表的Key
			/// </summary>
			public string SfUid { get; set; } = string.Empty;
			/// <summary>
			/// 檔案原始檔名
			/// </summary>
			public string FileOriginalName { get; set; } = string.Empty;
			/// <summary>
			/// 檔案實際儲存檔名
			/// </summary>
			public string FileSaveName { get; set; } = string.Empty;
			/// <summary>
			/// 檔案副檔名
			/// </summary>
			public string Extension { get; set; } = string.Empty;
			/// <summary>
			/// 檔案原始檔名含副檔名
			/// </summary>
			public string OriginalFullName
			{
				get
				{
					return $"{FileOriginalName}.{Extension}";
				}
			}
			/// <summary>
			/// 檔案實際儲存檔名含副檔名
			/// </summary>
			public string SaveFullName
			{
				get
				{
					return $"{FileSaveName}.{Extension}";
				}
			}
			/// <summary>
			/// 檔案MimeType
			/// </summary>
			public string MimeType { get; set; } = string.Empty;
			/// <summary>
			/// 檔案實體儲存路徑
			/// </summary>
			public string PhysicalFilePath { get; set; } = string.Empty;
		}
		#endregion

		/// <summary>
		/// 檔案上傳
		/// </summary>
		/// <param name="mType">功能模組ID 0:課程 1:教材</param>
		/// <param name="matchKey">MatchKey</param>
		/// <param name="file">檔案</param>
		/// <param name="overwrite">是否複寫檔案</param>
		/// <returns></returns>
		public bool UploadFile(Int32 mType, Guid matchKey, IFormFile file, bool overwrite = false)
		{


			//檢查傳入的參數
			if (string.IsNullOrEmpty(mType.ToString()) || string.IsNullOrEmpty(matchKey.ToString()) || file == null)
			{
				return false;
			}

			//搜尋檔案資料表
			SysFile? sysFile = db.SysFiles.Where(x => x.FMatchKey == matchKey).FirstOrDefault();

			//刪除

			//如果已經有檔案又沒開複寫就失敗
			if (sysFile != null && !overwrite)
			{
				return false;
			}

			//取得模組對應的目錄 //課程:Handouts=>H_ppt&H_png、教材:Books=>B_ppt&B_png
			String directory = sysFile != null ? sysFile.FPath : Guid.NewGuid().ToString();
			//String directory = mType == 0 ? "Handouts" : "Books";

			//取得失敗
			if (string.IsNullOrEmpty(directory.ToString()))
			{
				return false;
			}

			string physicalPath = Path.Combine(configuration.GetValue<String>("FileRootPath"), directory);

			//如果不存在建立目錄
			if (!Directory.Exists(physicalPath))
			{
				Directory.CreateDirectory(physicalPath);
			}
			else
			{

			}

			if (sysFile == null)
			{
				//新檔案
				sysFile = new SysFile()
				{
					FileId = Guid.NewGuid(),
					FModule = mType == 0 ? "HandOut" : "Book",
					FMatchKey = matchKey,
					FMimeType = GetMimeType(file.FileName),
					FFullName = file.FileName,
					FNameReal = Path.GetFileNameWithoutExtension(file.FileName),
					FNameDl = Guid.NewGuid().ToString(),
					FExt = Path.GetExtension(file.FileName).Replace(".", ""),
					FPath = directory.ToString(),
					FDescription = null,
					FOrder = null,
					FRemark = null,
					Creuser = Guid.Parse(GetUserID(user)),
					Credate = DateTime.Now
				};

				db.SysFiles.Add(sysFile);
			}
			else
			{
				//已經有檔案
				sysFile.FFullName = file.FileName;
				sysFile.FMimeType = GetMimeType(file.FileName);
				sysFile.FNameReal = Path.GetFileNameWithoutExtension(file.FileName);
				sysFile.FNameDl = Guid.NewGuid().ToString();
				sysFile.FExt = Path.GetExtension(file.FileName).Replace(".", "");
				sysFile.FPath = directory.ToString();
				sysFile.Upduser = Guid.Parse(GetUserID(user));
				sysFile.Upddate = DateTime.Now;

				db.SysFiles.Update(sysFile);
			}

			//上傳
			string savePath = physicalPath + "\\" + sysFile.FNameDl + "." + sysFile.FExt;
			//string savePath = "D:\\CLEA_FILES\\df22ae60-1b84-481f-afd7-39c108bdcd4adf22ae60-1b84-481f-afd7-39c108bdcd4a\\";
			//String SaveName = sysFile.FNameDl + "." + sysFile.FExt;
			using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
			{
				file.CopyTo(fileStream);
			}
			db.SaveChanges();

			pptToPng(Guid.NewGuid(), savePath, physicalPath);

			return true;
		}

		#region PPT TO PNG
		public void pptToPng(Guid matchKey, String sourcePath, String savePath)
		{
			Microsoft.Office.Interop.PowerPoint.Application appPpt = new Microsoft.Office.Interop.PowerPoint.Application();
			Microsoft.Office.Interop.PowerPoint.Presentation objActivePresentation = appPpt.Presentations.Open(sourcePath, Microsoft.Office.Core.MsoTriState.msoCTrue,
										Microsoft.Office.Core.MsoTriState.msoTriStateMixed,
										Microsoft.Office.Core.MsoTriState.msoFalse);
			int i = 0;
			foreach (Microsoft.Office.Interop.PowerPoint.Slide objSlide in objActivePresentation.Slides)
			{
				//Names are generated based on timestamp. 
				//objSlide.Export("Slide" + i, "PNG", 960, 720);
				objSlide.Export(savePath + @"\" + i + matchKey + ".png", "png", 1280, 720);
				i++;
			}
			objActivePresentation.Close();
			appPpt.Quit();
		}
		#endregion

		/// <summary>
		/// 取得圖片Base64(matchKey)
		/// </summary>
		/// <param name="matchKey"></param>
		/// <param name="IsTop">是否置頂/封面</param>
		/// <returns></returns>
		public List<String> GetImageBase64List_PNG(Guid matchKey)
		{
			List<String> result = new List<string>();
			SysFile? file = db.SysFiles.Where(x => x.FMatchKey == matchKey).FirstOrDefault();

			if (file != null)
			{				
				String physicalPath = Path.Combine(configuration.GetValue<String>("FileRootPath"), file.FPath);
				String[] dirFile = Directory.GetFiles(physicalPath, "*.png");
				if (dirFile.Length > 0)
				{
					foreach (String p in dirFile)
					{
						string base64 = ImageToBase64(p);
						string base64WithPrefix = $"data:image/png;base64,{base64}";
						result.Add(base64WithPrefix);
					}
				}				
			}




			////找出檔案資訊
			//List<SysFile> lst_files = db.SysFiles.Where(x => x.FMatchKey == matchKey).OrderBy(x => x.FOrder).ToList();

			//if (lst_files != null && lst_files.Count > 0)
			//{
			//	foreach (SysFile file in lst_files)
			//	{
			//		if (!IsImage(file.FFullName))
			//		{
			//			continue;
			//		}
			//		else
			//		{
			//			string base64 = ImageToBase64(configuration.GetValue<String>("FileRootPath") + "\\" + file.FPath + "\\" + file.FNameDl + "." + file.FExt);
			//			string base64WithPrefix = $"data:{file.FMimeType};base64,{base64}";
			//			result.Add(base64WithPrefix);
			//		}
			//	}
			//}

			//string base64 = ImageToBase64(sysFileInfo.PhysicalFilePath);

			//string base64WithPrefix = $"data:{sysFileInfo.MimeType};base64,{base64}";

			return result;
		}

		/// <summary>
		/// 以檔名判斷是否為圖片
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public bool IsImage(string fileName)
		{
			string mimeType = GetMimeType(fileName);

			if (mimeType.Contains("image"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		/// <summary>
		/// 使用檔名取得MimeType
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private string GetMimeType(string fileName)
		{
			new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string? contentType);

			return contentType ?? "";
		}

		/// <summary>
		/// 圖片路徑轉換Base64
		/// </summary>
		/// <param name="imagePath"></param>
		/// <returns></returns>
		private string ImageToBase64(string imagePath)
		{
			using (FileStream fileStream = File.OpenRead(imagePath))
			{
				byte[] imageBytes = new byte[fileStream.Length];

				fileStream.Read(imageBytes, 0, (int)fileStream.Length);

				string base64String = Convert.ToBase64String(imageBytes);

				return base64String;
			}
		}


		#region 刪除檔案
		public Boolean DeleteFile(SysFile file)
		{
			try
			{
				String directory = file.FPath;
				String physicalPath = Path.Combine(configuration.GetValue<String>("FileRootPath"), directory);
				Directory.Delete(physicalPath, true);

				db.SysFiles.Remove(file);
				db.SaveChanges();

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		#endregion
	}
}