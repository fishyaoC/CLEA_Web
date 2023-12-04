using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using NPOI.POIFS.Crypt.Dsig;
using X.PagedList;

namespace Clea_Web.Service
{
	//後台-評鑑模組 0=課程、1=教材
	public class LectorClassService : BaseService
	{
		private FileService _fileservice;
		private SMTPService _smtpService;
		public LectorClassService(dbContext dbContext, FileService fileservice, SMTPService smtpService)
		{
			db = dbContext;
			_fileservice = fileservice;
			_smtpService = smtpService;
		}

		#region Index
		public IPagedList<LectorClassViewModel.ClassMenu> GetClassMenuPageList(Int32 page)
		{
			Guid L_UID = Guid.Parse(GetUserID(user));
			List<LectorClassViewModel.ClassMenu> result = new List<LectorClassViewModel.ClassMenu>();

			result = (from plc in db.ViewPLectorClasses
					  where plc.Reception == L_UID && plc.IsSche && plc.Status != 8
					  select new LectorClassViewModel.ClassMenu()
					  {
						  ES_ID = plc.EsId,
						  ClassName = plc.CName,
						  SubName = plc.DName,
						  Status = plc.Status,
						  IsUpload = string.IsNullOrEmpty(plc.FileName) ? false : true,
						  CREDATE = plc.Credate
					  }).OrderByDescending(x => x.CREDATE).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region GetLogList
		public List<LectorClassViewModel.UploadLog> GetUploadLogs(Guid ES_ID)
		{
			List<LectorClassViewModel.UploadLog> result = new List<LectorClassViewModel.UploadLog>();

			result = (from log in db.EClassUploadLogs.OrderByDescending(x => x.Credate)
					  where log.EsId == ES_ID
					  select new LectorClassViewModel.UploadLog()
					  {
						  F_Name = log.FileFullName,
						  CreDate = log.Credate,
						  IsUpdate = log.IsUpdate,
						  Other = log.Other,
						  Status = log.Status
					  }).ToList();

			return result;
		}
		#endregion

		#region Modify
		public LectorClassViewModel.Modify GetModifyModel(Guid ES_ID)
		{
			ViewPLectorClassAbstract viewPLectorClassAbstract = db.ViewPLectorClassAbstracts.Where(x => x.EsId == ES_ID).FirstOrDefault();

			SysFile? sysFile = db.SysFiles.Where(x => x.FMatchKey == ES_ID).FirstOrDefault();
			LectorClassViewModel.Modify result = new LectorClassViewModel.Modify()
			{
				ES_ID = ES_ID,
				C_Name = viewPLectorClassAbstract.CName,
				S_Name = viewPLectorClassAbstract.DName,
				FileName = sysFile == null ? null : sysFile.FFullName,
				F_ID = sysFile == null ? null : sysFile.FileId,
				Syllabus = viewPLectorClassAbstract.ETeachSyllabus,
				Object = viewPLectorClassAbstract.ETeachObject,
				Abstract = viewPLectorClassAbstract.ETeachAbstract,
				Status = viewPLectorClassAbstract.Status,
				IsClose = viewPLectorClassAbstract.IsClose
			};

			return result;
		}
		#endregion

		#region SaveModifyData
		public BaseViewModel.errorMsg SaveModifyData(LectorClassViewModel.Modify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			try
			{
				EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Where(x => x.EsId == data.ES_ID).FirstOrDefault() ?? null;
				if (eEvaluationSche != null)
				{
					if (!string.IsNullOrEmpty(data.FileName) && data.file == null)
					{
						result.CheckMsg = true;
					}
					else if (string.IsNullOrEmpty(data.FileName) && data.file != null)
					{
						_fileservice.user = user;
						result.CheckMsg = _fileservice.UploadFile(true, 0, data.ES_ID, data.file, true);
						if (result.CheckMsg)
						{
							AddUploadLog(data.ES_ID, data.IsUpdate, data.file.FileName, data.UpdContent, data.Other);
						}
						else
						{
							result.CheckMsg = false;
							result.ErrorMsg = "檔案上傳失敗";
						}
					}
					if (result.CheckMsg)
					{
						eEvaluationSche.ETeachSyllabus = data.Syllabus;
						eEvaluationSche.ETeachObject = data.Object;
						eEvaluationSche.ETeachAbstract = data.Abstract;
						eEvaluationSche.Status = data.IsUpdate ? 8 : 1;
						eEvaluationSche.IsSche = data.IsUpdate ? false : eEvaluationSche.IsSche;
						eEvaluationSche.Upduser = Guid.Parse(GetUserID(user));
						eEvaluationSche.Upddate = DateTime.Now;
						result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
					}
				}
				else
				{
					result.CheckMsg = false;
					result.ErrorMsg = "查無此筆資料!";
				}

				List<string> strings = new List<string>() { "asiaice2010@hotmail.com" };
				_smtpService.SendMail(strings, "[審核通知]CLEA_System", "系統測試郵件，請勿直接回覆並直接忽視本郵件");
			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}
		#endregion

		#region SetNewES
		public BaseViewModel.errorMsg SetNewSubEv(LectorClassViewModel.Modify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			try
			{
				EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Where(x => x.EsId == data.ES_ID).FirstOrDefault() ?? null;
				if (eEvaluationSche != null)
				{
					EEvaluationSche eEvaluationScheNew = new EEvaluationSche();
					eEvaluationScheNew = eEvaluationSche;
					eEvaluationScheNew.EsId = Guid.NewGuid();
					eEvaluationScheNew.Status = 1;
					eEvaluationScheNew.ScheNum = eEvaluationSche.ScheNum + 1;
					eEvaluationScheNew.ChkNum = 0;
					eEvaluationScheNew.IsSche = true;
					eEvaluationScheNew.ETeachSyllabus = data.Syllabus;
					eEvaluationScheNew.ETeachObject = data.Object;
					eEvaluationScheNew.ETeachAbstract = data.Abstract;
					eEvaluationScheNew.Upduser = Guid.Parse(GetUserID(user));
					eEvaluationScheNew.Upddate = DateTime.Now;
					db.EEvaluationSches.Add(eEvaluationScheNew);
					result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

					if (!string.IsNullOrEmpty(data.FileName) && data.file == null)
					{
						result.CheckMsg = true;
					}
					else if (string.IsNullOrEmpty(data.FileName) && data.file != null)
					{
						_fileservice.user = user;
						result.CheckMsg = _fileservice.UploadFile(true, 0, eEvaluationScheNew.EsId, data.file, true);
						if (result.CheckMsg)
						{
							CopyUploadLog(data.ES_ID, eEvaluationScheNew.EsId);
						}
						else
						{
							result.CheckMsg = false;
							result.ErrorMsg = "檔案上傳失敗";
						}
					}
				}
				else
				{
					result.CheckMsg = false;
					result.ErrorMsg = "查無此筆資料!";
				}

			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}
		#endregion

		#region CopyUploadLog
		public void CopyUploadLog(Guid ES_ID_O, Guid ES_ID_N)
		{
			List<EClassUploadLog> eClassUploadLogs = db.EClassUploadLogs.Where(x => x.EsId == ES_ID_O).OrderBy(x => x.Credate).ToList();
			if (eClassUploadLogs != null && eClassUploadLogs.Count > 0)
			{
				foreach (EClassUploadLog old in eClassUploadLogs)
				{
					EClassUploadLog eClassUploadLog = new EClassUploadLog();
					eClassUploadLog = old;
					eClassUploadLog.Sn = 0;
					eClassUploadLog.EsId = ES_ID_N;
					db.EClassUploadLogs.Add(eClassUploadLog);
				}
				db.SaveChanges();
			}
		}
		#endregion

		#region AddUploadLog
		public void AddUploadLog(Guid ES_ID, Boolean IsUpdate, String FileName, Int32? UpdContent, String? Other)
		{
			EClassUploadLog eClassUploadLog = new EClassUploadLog()
			{
				EsId = ES_ID,
				FileFullName = FileName,
				IsUpdate = IsUpdate,
				Status = UpdContent,
				Other = Other,
				Creuser = Guid.Parse(GetUserID(user)),
				Credate = DateTime.Now
			};
			db.EClassUploadLogs.Add(eClassUploadLog);
			db.SaveChanges();
		}

		#endregion

		#region SaveLogData

		#endregion
	}
}

