using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
	//前台評鑑模組
	public class LectorEvaluationService : BaseService
	{
		private SMTPService _smtpService;
		public LectorEvaluationService(dbContext dbContext, SMTPService smtpService)
		{
			db = dbContext;
			_smtpService = smtpService;
		}

		#region Index
		public IPagedList<LectorEvaluationViewModel.schEvInfo> GetSchEvInfosPageList(Int32 page)
		{
			List<LectorEvaluationViewModel.schEvInfo> result = new List<LectorEvaluationViewModel.schEvInfo>();
			Guid UID = Guid.Parse(GetUserID(user));

			result = (from ple in db.ViewPLectorEvaluates.OrderBy(x => x.Evaluate)
					  where ple.Evaluate == UID
					  select new LectorEvaluationViewModel.schEvInfo()
					  {
						  ED_ID = ple.EdId,
						  mType = ple.EType == 0 ? "課程" : "教材",
						  ClassName_BookName = ple.EType == 0 ? ple.CName : ple.MName,
						  SubName_PName = ple.EType == 0 ? ple.DName : ple.BpName,
						  Status = ple.Status,
						  IsUpload = (from fi in db.SysFiles where fi.FMatchKey == ple.EsId select fi).Count() > 0 ? true : false,
						  CreDate = ple.Credate,
						  IsClose = ple.IsClose,
						  ScoreA = ple.EScoreA
					  }).OrderByDescending(x => x.CreDate).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region SaveScoreData
		public BaseViewModel.errorMsg SaveScoreData(LectorEvaluationViewModel.scoreModify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			try
			{
				String Content = string.Empty;
				EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(data.ED_ID) ?? null;
				if (eEvaluateDetail != null)
				{
					eEvaluateDetail.EScoreA = data.ScoreA;
					eEvaluateDetail.EScoreB = data.mType == 0 ? data.ScoreB : data.ScoreBB;
					eEvaluateDetail.EScoreC = data.mType == 0 ? data.ScoreC : data.ScoreCB;
					if (data.mType == 0)
					{
						eEvaluateDetail.EScoreD = data.ScoreD;
						eEvaluateDetail.EScoreE = data.ScoreE;
						Content = "有新的[課程]評分審核項目";
					}
					else
					{
						Content = "有新的[教材]評分審核項目";
					}
					eEvaluateDetail.ERemark = data.Remark;
					eEvaluateDetail.Status = 3;
					eEvaluateDetail.EFirstScoreDate = eEvaluateDetail.EFirstScoreDate == null ? DateTime.Now.Date : eEvaluateDetail.EFirstScoreDate;
					eEvaluateDetail.Upduser = Guid.Parse(GetUserID(user));
					eEvaluateDetail.Upddate = DateTime.Now;
					result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

					if (result.CheckMsg)
					{
						EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(eEvaluateDetail.EsId) ?? null;
						if (eEvaluationSche != null)
						{
							eEvaluationSche.Status = 3;
							eEvaluationSche.Upduser = Guid.Parse(GetUserID(user));
							eEvaluationSche.Upddate = DateTime.Now;
							result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
						}
					}

					//List<string> strings = new List<string>() { "pp740729@hotmail.com" };									
					//_smtpService.SendMail(strings, "[審核通知]CLEA_System", Content + "請承辦人至後台進行審核，請勿直接回覆並直接忽視本郵件");
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
	}
}

