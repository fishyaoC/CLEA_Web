using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
	//後台-評鑑模組 0=課程、1=教材
	public class LectorClassService : BaseService
	{
		private FileService _fileservice;
		public LectorClassService(dbContext dbContext, FileService fileservice)
		{
			db = dbContext;
			_fileservice = fileservice;
		}

		#region Index
		public IPagedList<LectorClassViewModel.ClassMenu> GetClassMenuPageList(Int32 page)
		{
			Guid L_UID = Guid.Parse(GetUserID(user));
			List<LectorClassViewModel.ClassMenu> result = new List<LectorClassViewModel.ClassMenu>();

			result = (from ed in db.EEvaluateDetails
					  where ed.Reception == L_UID && string.IsNullOrEmpty(ed.ERemark)
					  select new LectorClassViewModel.ClassMenu()
					  {
						  ED_ID = ed.EdId
					  }).ToList();


			if (result.Count > 0)
			{
				foreach (var traf in result)
				{
					EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(traf.ED_ID) ?? null;
					EEvaluate? eEvaluate = db.EEvaluates.Find(eEvaluateDetail.EId) ?? null;
					CClassLector? cClassLector = db.CClassLectors.Find(eEvaluateDetail.MatchKey2) ?? null;
					CClass? cClass = db.CClasses.Find(cClassLector.CUid) ?? null;
					CClassSubject? cClassSubject = db.CClassSubjects.Find(cClassLector.DUid) ?? null;
					SysFile? sysFile = db.SysFiles.Where(x => x.FMatchKey == eEvaluateDetail.EdId).FirstOrDefault();
					traf.mType = eEvaluate.EType;
					traf.Year = eEvaluate.EYear;
					traf.ClassName = cClass.CName;
					traf.SubName = cClassSubject.DName;
					traf.IsUpload = sysFile == null ? false : true;
				}
			}

			return result.OrderByDescending(x => x.Year).ThenByDescending(x => x.ClassName).ToPagedList(page, pagesize);
		}
		#endregion

		#region GetLogList
		public List<LectorClassViewModel.UploadLog> GetUploadLogs(Guid ED_ID)
		{
			List<LectorClassViewModel.UploadLog> result = new List<LectorClassViewModel.UploadLog>();

			result = (from log in db.EClassUploadLogs.OrderByDescending(x => x.Credate)
					  where log.EdId == ED_ID
					  select new LectorClassViewModel.UploadLog()
					  {
						  F_Name = log.FileFullName,
						  CreDate = log.Credate,
						  IsUpdate = log.IsUpdate
					  }).ToList();

			return result;
		}
		#endregion

		#region Modify
		public LectorClassViewModel.Modify GetModifyModel(Guid ED_ID)
		{
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;
			EEvaluate? eEvaluate = db.EEvaluates.Find(eEvaluateDetail.EId) ?? null;
			CClassLector? cClassLector = db.CClassLectors.Find(eEvaluateDetail.MatchKey2) ?? null;
			CClass? cClass = db.CClasses.Find(cClassLector.CUid) ?? null;
			CClassSubject? cClassSubject = db.CClassSubjects.Find(cClassLector.DUid) ?? null;
			SysFile? sysFile = db.SysFiles.Where(x => x.FMatchKey == ED_ID).FirstOrDefault();

			LectorClassViewModel.Modify result = new LectorClassViewModel.Modify()
			{
				ED_ID = ED_ID,
				Year = eEvaluate.EYear,
				C_Name = cClass.CName,
				S_Name = cClassSubject.DName,
				FileName = sysFile == null ? null : sysFile.FFullName,
				F_ID = sysFile == null ? null : sysFile.FileId,
				Syllabus = eEvaluateDetail.ETeachSyllabus,
				Object = eEvaluateDetail.ETeachObject,
				Abstract = eEvaluateDetail.ETeachAbstract
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
				EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(data.ED_ID) ?? null;

				if (eEvaluateDetail != null)
				{
					if (!string.IsNullOrEmpty(data.FileName) && data.file == null)
					{
						result.CheckMsg = true;
					}
					else if (string.IsNullOrEmpty(data.FileName) && data.file != null)
					{
						_fileservice.user = user;
						result.CheckMsg = _fileservice.UploadFile(true, 0, data.ED_ID, data.file);
						if (result.CheckMsg)
						{
							EClassUploadLog eClassUploadLog = new EClassUploadLog()
							{
								EdId = data.ED_ID,
								IsUpdate = data.IsUpdate,
								FileFullName = data.file.FileName,
								Creuser = Guid.Parse(GetUserID(user)),
								Credate = DateTime.Now
							};
							db.EClassUploadLogs.Add(eClassUploadLog);
						}
						else
						{
							result.CheckMsg = false;
							result.ErrorMsg = "檔案上傳失敗";
						}
					}



					if (result.CheckMsg)
					{
						eEvaluateDetail.ETeachSyllabus = data.Syllabus;
						eEvaluateDetail.ETeachObject = data.Object;
						eEvaluateDetail.ETeachAbstract = data.Abstract;
						if (data.IsUpdate)
						{
							eEvaluateDetail.EScoreA = null;
							eEvaluateDetail.EScoreB = null;
							eEvaluateDetail.EScoreC = null;
							eEvaluateDetail.EScoreD = null;
							eEvaluateDetail.EScoreE = null;
							eEvaluateDetail.ERemark = null;
						}
						eEvaluateDetail.Upduser = Guid.Parse(GetUserID(user));
						eEvaluateDetail.Upddate = DateTime.Now;
						result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
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

		#region SaveLogData

		#endregion
	}
}

