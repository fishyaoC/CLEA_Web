using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
	//後台-評鑑模組 0=課程、1=教材
	public class LectorEvaluationService : BaseService
	{
		public LectorEvaluationService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region Index
		public IPagedList<LectorEvaluationViewModel.schEvInfo> GetSchEvInfosPageList(Int32 page)
		{
			List<LectorEvaluationViewModel.schEvInfo> result = new List<LectorEvaluationViewModel.schEvInfo>();
			Guid UID = Guid.Parse(GetUserID(user));
			result = (from ed in db.EEvaluateDetails
					  where ed.Evaluate == UID && string.IsNullOrEmpty(ed.ERemark)
					  select new LectorEvaluationViewModel.schEvInfo()
					  {
						  ED_ID = ed.EdId,
						  Year = (from e in db.EEvaluates where e.EId == ed.EId select e).FirstOrDefault().EYear,
						  mType = (from e in db.EEvaluates where e.EId == ed.EId select e).FirstOrDefault().EType == 0 ? "課程" : "教材"
					  }).OrderByDescending(x => x.Year).ToList();

			if (result.Count > 0)
			{
				foreach (var traf in result)
				{
					EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(traf.ED_ID) ?? null;
					EEvaluate? eEvaluate = db.EEvaluates.Find(eEvaluateDetail.EId) ?? null;
					if (traf.mType.Contains("課程"))
					{
						CClass? cClass = db.CClasses.Find(eEvaluate.MatchKey) ?? null;
						CClassLector? cClassLector = db.CClassLectors.Find(eEvaluateDetail.MatchKey2) ?? null;
						CClassSubject? cClassSubject = db.CClassSubjects.Where(x => x.DUid == cClassLector.DUid).FirstOrDefault();
						traf.ClassName_BookName = cClass.CName;
						traf.SubName_PName = cClassSubject.DName;
					}
					else
					{
						CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;
						CBookDetail? cBookDetail = db.CBookDetails.Where(x => x.MdId == eEvaluateDetail.MatchKey2).FirstOrDefault();
						CBookPublish? cBookPublish = db.CBookPublishes.Find(cBookDetail.MdPublish) ?? null;
						traf.ClassName_BookName = cBook.MName;
						traf.SubName_PName = cBookPublish.BpName;
					}
				}
			}

			return result.OrderByDescending(x => x.Year).ThenByDescending(x => x.ClassName_BookName).ToPagedList(page, pagesize);
		}
		#endregion

		#region GetModel
		public LectorEvaluationViewModel.EvInfo GetEvModel()
		{
			LectorEvaluationViewModel.EvInfo result = new LectorEvaluationViewModel.EvInfo();
			return result;
		}
		#endregion

		#region SaveScoreData
		public BaseViewModel.errorMsg SaveScoreData(LectorEvaluationViewModel.scoreModify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			try
			{
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
					}
					eEvaluateDetail.Upduser = Guid.Parse(GetUserID(user));
					eEvaluateDetail.Upddate = DateTime.Now;
					result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
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

