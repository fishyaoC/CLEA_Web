﻿using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using NPOI.OpenXmlFormats;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using X.PagedList;
using static Clea_Web.ViewModels.AssignClassViewModel;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class B_LectorBtnService : BaseService
    {
        private B_LectorBtnViewModel.Modify vm = new B_LectorBtnViewModel.Modify();
        private FileService _fileservice;

        public B_LectorBtnService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<B_LectorBtnViewModel.schPageList> schPages(B_LectorBtnViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<B_LectorBtnViewModel.schPageList> GetPageLists(B_LectorBtnViewModel.SchItem data)
        {
            B_LectorBtnViewModel.schPageList model;
            List<B_LectorBtnViewModel.schPageList> result = new List<B_LectorBtnViewModel.schPageList>();
            if (data != null)
            {
                db.PNews.Where
                    (x =>
                x.NStartDate == data.NStartDate
                && x.NEndDate == data.NEndDate
                && x.NTitle == data.NTitle
                && x.NType == data.NType
                ).ToList().ForEach(x =>
                {
                    model = new B_LectorBtnViewModel.schPageList();
                    model.NTitle = x.NTitle;
                    model.Upddate = x.Upddate;
                    model.Upduser = x.Upduser;
                    model.Creuser = x.Creuser;
                    model.Curdate = x.Curdate;
                    model.NStatus = x.NStatus;
                    model.RId = x.RId;
                    model.NContent = x.NContent;
                    model.NClass = x.NClass;
                    model.NEndDate = x.NEndDate;
                    model.NStartDate = x.NStartDate;
                    model.NewsId = x.NewsId;
                    model.NIsShow = x.NIsShow;
                    model.NIsTop = x.NIsTop;
                    model.NType = x.NType;
                    model.NTypeName = db.SysCodes.Where(y => y.CParentCode == "btnType" && y.CItemCode == x.NType.ToString()).Select(z => z.CItemName).FirstOrDefault();
                    result.Add(model);
                });
            }
            else
            {
                db.PNews.ToList().ForEach(x =>
                                {
                                    model = new B_LectorBtnViewModel.schPageList();
                                    model.NTitle = x.NTitle;
                                    model.Upddate = x.Upddate;
                                    model.Upduser = x.Upduser;
                                    model.Creuser = x.Creuser;
                                    model.Curdate = x.Curdate;
                                    model.NStatus = x.NStatus;
                                    model.RId = x.RId;
                                    model.NContent = x.NContent;
                                    model.NClass = x.NClass;
                                    model.NEndDate = x.NEndDate;
                                    model.NStartDate = x.NStartDate;
                                    model.NewsId = x.NewsId;
                                    model.NIsShow = x.NIsShow;
                                    model.NIsTop = x.NIsTop;
                                    model.NType = x.NType;
                                    model.NTypeName = db.SysCodes.Where(y => y.CParentCode == "btnType" && y.CItemCode == x.NType.ToString()).Select(z => z.CItemName).FirstOrDefault();
                                    result.Add(model);
                                });
            }
            //result = (from pn in db.PNews
            //              //join user in db.SysUsers on r.Creuser equals user.UName
            //          where
            //          (
            //          //公告類型、公告標題、開始日期、結束日期
            //          (string.IsNullOrEmpty(data.NTitle) || pn.NTitle.Contains(data.NTitle)) &&
            //          (string.IsNullOrEmpty(data.NStartDate.ToString()) || pn.NStartDate == data.NStartDate) &&
            //          (string.IsNullOrEmpty(data.NEndDate.ToString()) || pn.NEndDate == data.NEndDate) &&
            //          (string.IsNullOrEmpty(data.NType.ToString()) || pn.NTitle == data.NType.ToString())
            //          )
            //          select new B_LectorBtnViewModel.schPageList
            //          {
            //              NewsId = pn.NewsId,
            //              NType = pn.NType,
            //              NTitle = pn.NTitle,
            //              NClass = pn.NClass,
            //              NStartDate = pn.NStartDate,
            //              NEndDate = pn.NEndDate,
            //              NIsTop = pn.NIsTop,
            //              NIsShow = pn.NIsShow,
            //              NStatus = pn.NStatus,
            //              NContent = pn.NContent,
            //              NRole = pn.NRole,
            //              RId = pn.RId,
            //              NTypeName = (from code in db.SysCodes where code.CParentCode.Equals("btnType") && code.CItemCode == pn.NType.ToString() select code).FirstOrDefault().CItemName
            //              //creDate = r.Credate.ToShortDateString(),
            //              //creUser = r.Creuser,
            //              //Upddate = pn.Upddate == null ? pn.Curdate.ToShortDateString() : pn.Upddate.Value.ToShortDateString(),
            //              //Upduser = string.IsNullOrEmpty(pn.Upduser.ToString()) ? pn.Creuser : pn.Upduser
            //          }).OrderByDescending(x => x.Curdate).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(B_LectorBtnViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PNews? PNews = db.PNews.Find(vm.R_ID);

                if (PNews is null)
                {
                    PNews = new PNews();
                }

                PNews.NType = vm.N_Type;
                PNews.NTitle = vm.N_Title;
                PNews.NClass = vm.N_Class;
                PNews.NStartDate = vm.N_StartDate.Date;
                PNews.NEndDate = vm.N_EndDate.Date;
                PNews.NIsTop = vm.N_IsTop;
                PNews.NIsShow = vm.N_IsShow;
                PNews.NStatus = vm.N_Status;
                PNews.NContent = vm.NContent;
                PNews.NRole = vm.NRole;
                PNews.RId = vm.R_ID.ToString();

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    PNews.Upduser = Guid.Parse(GetUserID(user));
                    PNews.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    PNews.NewsId = Guid.NewGuid();
                    PNews.Creuser = Guid.Parse(GetUserID(user));
                    PNews.Curdate = DateTime.Now;
                    db.PNews.Add(PNews);
                }

                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadNewFile(PNews.NewsId, vm.file);
                    if (result.CheckMsg)
                    {

                    }
                    else
                    {
                        result.CheckMsg = false;
                        result.ErrorMsg = "檔案上傳失敗";
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
                //return false;
            }
            return result;

        }
        #endregion

        #region 編輯
        public B_LectorBtnViewModel.Modify GetEditData(string NewsID)
        {
            //撈資料
            B_LectorBtnViewModel.Modify model = new B_LectorBtnViewModel.Modify();
            //List<B_LectorBtnViewModel.Modify> result = new List<B_LectorBtnViewModel.Modify>();
            var _PNews = db.PNews.Where(x => x.NewsId.ToString() == NewsID).FirstOrDefault();

            model = new B_LectorBtnViewModel.Modify();
            model.N_Title = _PNews.NTitle;
            model.Upddate = _PNews.Upddate;
            model.Upduser = _PNews.Upduser;
            model.Creuser = _PNews.Creuser;
            model.Curdate = _PNews.Curdate;
            model.NStatus = _PNews.NStatus;
            model.RId = _PNews.RId;
            model.N_Content = _PNews.NContent;
            model.NClass = _PNews.NClass;
            model.NEndDate = _PNews.NEndDate;
            model.NStartDate = _PNews.NStartDate;
            model.N_EndDate = _PNews.NEndDate;
            model.N_StartDate = _PNews.NStartDate;
            model.NewsId = _PNews.NewsId;
            model.NIsShow = _PNews.NIsShow;
            model.NIsTop = _PNews.NIsTop;
            model.N_IsShow = _PNews.NIsShow;
            model.N_IsTop = _PNews.NIsTop;
            model.NType = _PNews.NType;
            model.NRole = _PNews.NRole;
            if (_PNews.NRole == true)
            {
                model.GroupID = db.SysCodes.Where(x => x.CParentCode.Equals("L_Type") && x.Uid == Guid.Parse(_PNews.RId)).Select(x => x.Uid).FirstOrDefault();
            }
            else
            {
                model.PersonID = db.SysUsers.Where(x => x.UId == Guid.Parse(_PNews.RId)).Select(x => x.UId).FirstOrDefault();
            }

            return model;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid NewsId)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PNews _PNews = db.PNews.Where(x => x.NewsId == NewsId).FirstOrDefault();
            vm = new B_LectorBtnViewModel.Modify();

            try
            {
                db.PNews.Remove(_PNews);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        #region 匯出Excel
        public Byte[] ExportExcel(Guid NewsID, String Title, Boolean? Role, String RId)
        {

            //PNews pn = db.PNews.Where(x => x.NewsId == NewsID).FirstOrDefault();

            #region ExportExcel
            using (var exportData = new MemoryStream())
            {
                IWorkbook wb = new XSSFWorkbook();  //字型定義
                ISheet sheet = wb.CreateSheet(Title);
                XSSFCellStyle TitleStyle = (XSSFCellStyle)wb.CreateCellStyle(); //標題字型
                TitleStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                TitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                XSSFFont font = (XSSFFont)wb.CreateFont();
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                TitleStyle.SetFont(font);

                XSSFCellStyle ContentStyle = (XSSFCellStyle)wb.CreateCellStyle();//內容造型
                ContentStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                ContentStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                ContentStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                ContentStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                ContentStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                ContentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;


                var rowTitle = sheet.CreateRow(0);
                rowTitle.CreateCell(0).SetCellValue("序號");
                rowTitle.CreateCell(1).SetCellValue("講師名單");
                rowTitle.CreateCell(2).SetCellValue("是否已讀");
                int count = 1;

                List<CLector> clList = db.CLectors.ToList();
                //true=群發，false=個人
                if (Role == true)
                {

                    foreach (var item in clList)
                    {

                        var row = sheet.CreateRow(count);
                        //序號
                        row.CreateCell(0).SetCellValue(count);
                        //講師名單
                        row.CreateCell(1).SetCellValue(item.LName);
                        //是否已讀
                        SysUser user = db.SysUsers.Where(x => x.UAccount.Equals(item.LId)).FirstOrDefault();
                        PNewsReadLog pnLog = db.PNewsReadLogs.Where(x => x.NewsId == NewsID && x.Creuser == user.UId).FirstOrDefault();


                        if (pnLog == null)
                        {
                            row.CreateCell(2).SetCellValue("未讀");
                        }
                        else
                        {
                            row.CreateCell(2).SetCellValue("已讀");
                        }

                        count++;

                    }
                }
                else
                {
                    var row = sheet.CreateRow(1);
                    SysUser user = db.SysUsers.Where(x => x.UId == Guid.Parse(RId) ).FirstOrDefault();
                    //CLector cl = db.CLectors.Where(x=>x.LId.Equals(user.UAccount)).FirstOrDefault();
                    //序號
                    row.CreateCell(0).SetCellValue(1);
                    //講師名單
                    row.CreateCell(1).SetCellValue(user.UName);
                    //是否已讀
                    PNewsReadLog pnLog = db.PNewsReadLogs.Where(x => x.NewsId == NewsID && x.Creuser == user.UId).FirstOrDefault();

                    if (pnLog == null)
                    {
                        row.CreateCell(2).SetCellValue("未讀");
                    }
                    else
                    {
                        row.CreateCell(2).SetCellValue("已讀");
                    }
                }


                wb.Write(exportData, true);

                Byte[] result = exportData.ToArray();
                return result;
            }
            #endregion

        }
        #endregion
    }

}

