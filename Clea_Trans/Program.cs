using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clea_Trans
{
    internal class Program
    {

        #region Log
        /// <summary>
        /// Log
        /// </summary>
        public static void Log(string log)
        {
            string vFileName = DateTime.Now.ToString("yyyyMMdd");

            Assembly assembly = Assembly.GetExecutingAssembly();
            string location = assembly.CodeBase;
            string fullPath = new Uri(location).LocalPath; // 包含dll的路徑 
            string directoryPath = Path.GetDirectoryName(fullPath); // 目錄路徑 

            /*檢查Log資料夾是否存在*/
            string logFolderPath = Path.Combine(directoryPath, "Trans_Log");
            if (!Directory.Exists(logFolderPath))
            {
                /*如果沒有Log資料夾，則新建一個*/
                Directory.CreateDirectory(logFolderPath);
            }

            /*寫入文字檔*/
            /*訊息前加入時、分、秒與辨識字首*/
            string logFormat = DateTime.Now.ToString("G") + " ==> ";
            string logFilePath = Path.Combine(logFolderPath, "Trans_Log_" + vFileName + ".log");
            using (StreamWriter sw = new StreamWriter(logFilePath, true, Encoding.UTF8))
            {
                sw.WriteLine(logFormat + log);
            }
        }
        #endregion

        #region 建立帳號
        public static void CreateUser(View_Bridge_C_Lector cl)
        {
            CELA_WEBDBEntities db = new CELA_WEBDBEntities();

            //判斷有無此帳號
            SYS_User su = db.SYS_User.Where(x => x.U_Account.Equals(cl.L_ID)).FirstOrDefault();
            try
            {
                if (su != null)
                {
                    //update
                    su.U_Account = cl.L_ID;
                    su.U_Name = cl.L_NAME;
                    su.U_Email = cl.L_Email;
                    su.U_Birthday = cl.L_BRITHDAY;
                    su.U_Status = string.IsNullOrEmpty(cl.L_Active) ? false : cl.L_Active.Equals("N") ? true : false;
                    su.UPDUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                    su.UPDDATE = DateTime.Now;

                }
                else
                {
                    //create
                    su = new SYS_User();
                    su.U_ID = Guid.NewGuid();
                    su.R_UID = Guid.Parse("1F6B0217-0C38-4DFD-9CCC-C70651290AE0");  //講師權限
                    su.U_Account = cl.L_ID;
                    String pw = HashPassword(cl.L_ID.Substring(0, 1).ToUpper() + cl.L_ID.Substring(6, 4) + cl.L_BRITHDAY.Value.Month.ToString().PadLeft(2, '0') + cl.L_BRITHDAY.Value.Day.ToString().PadLeft(2, '0'));
                    su.U_Password = pw; //身分證字母(大寫)+身分證後4碼+生日後4碼
                    su.U_Name = cl.L_NAME;
                    su.U_Email = cl.L_Email;
                    su.U_Phone = cl.L_Phone;
                    su.U_Birthday = cl.L_BRITHDAY;
                    su.isOutSide = false;
                    su.U_Status = string.IsNullOrEmpty(cl.L_Active) ? false : cl.L_Active.Equals("N") ? true : false;
                    //su.U_Status = true;
                    su.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                    su.CREDATE = DateTime.Now;
                    db.SYS_User.Add(su);

                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("例外狀況");
                Log("例外狀況");
                Console.WriteLine(ex.ToString());
                Log(ex.ToString());
            }
        }
        #endregion

        #region 密碼加密
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // 將密碼轉為字節數組
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                //byte[] passwordBytes = Encoding.UTF8.GetBytes("123");


                // 計算hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                //將hash轉為十六進制字符串
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }

        }
        #endregion

        static void Main(string[] args)
        {
            Log("=============開始執行轉檔程式=============");
            Console.WriteLine("=============開始執行轉檔程式=============");

            CELA_WEBDBEntities db = new CELA_WEBDBEntities();

            #region Sys_User
            List<View_Bridge_C_Lector> V_LectorListC = db.View_Bridge_C_Lector.ToList();

            Log("開始處理Sys_User，強制確認有無帳號");
            Console.WriteLine("開始處理Sys_User，強制確認有無帳號");

            try
            {
                foreach (var item in V_LectorListC)
                {
                    //建立帳號
                    CreateUser(item);
                }
                db.SaveChanges();

                Log("結束Sys_User");
                Console.WriteLine("結束Sys_User");
                Log("==========================");
                Console.WriteLine("==========================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("例外狀況");
                Log("例外狀況");
                Console.WriteLine(ex.ToString());
                Log(ex.ToString());
            }


            #endregion

            #region C_Lector
            List<View_Bridge_C_Lector> V_LectorList = db.View_Bridge_C_Lector.ToList();
            List<C_Lector> C_LectorList = db.C_Lector.ToList();

            Log("開始處理C_Lector");
            Console.WriteLine("開始處理C_Lector");

            int count = 1;

            try
            {
                foreach (var item in V_LectorList)
                {
                    C_Lector cl = C_LectorList.Where(x => x.L_ID == item.L_ID).FirstOrDefault();
                    SYS_User su = db.SYS_User.Where(x => x.U_Account == item.L_ID).FirstOrDefault();
                    if (cl != null)
                    {

                        //update
                        cl.L_UID = su.U_ID;
                        cl.L_ID = item.L_ID;
                        cl.L_NAME = item.L_NAME;
                        cl.L_BRITHDAY = item.L_BRITHDAY;
                        cl.L_Edu = item.L_Edu;
                        cl.L_EduSchool = item.L_EduSchool;
                        cl.L_PosCode = string.IsNullOrEmpty(item.L_PosCode) ? 0 : Convert.ToInt32(item.L_PosCode);
                        cl.L_Address = item.L_Address;
                        cl.L_CPosCode = string.IsNullOrEmpty(item.L_CPosCode) ? 0 : Convert.ToInt32(item.L_CPosCode);
                        cl.L_CAddress = item.L_CAddress;
                        cl.L_Phone = item.L_Phone;
                        cl.L_CellPhone = item.L_CellPhone;
                        cl.L_Email = item.L_Email;
                        cl.L_Type = item.L_Type;
                        cl.L_TravelExpenses = item.L_TravelExpenses;
                        cl.L_IsCheck = string.IsNullOrEmpty(item.L_IsCheck) ? false : item.L_IsCheck.Equals("Y") ? true : false;
                        cl.L_Active = string.IsNullOrEmpty(item.L_Active) ? false : item.L_Active.Equals("N") ? true : false;
                        cl.L_Memo = item.L_Memo;
                        //cl.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //cl.CREDATE = item.CREDATE;
                        if (item.UPDUSER != null)
                        {
                            //cl.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                            //cl.UPDDATE = item.UPDDATE;
                        }

                    }
                    else
                    {
                        //建立帳號
                        //create
                        cl = new C_Lector();
                        cl.L_UID = su.U_ID;
                        cl.L_ID = item.L_ID;
                        cl.L_NAME = item.L_NAME;
                        cl.L_BRITHDAY = item.L_BRITHDAY;
                        cl.L_Edu = item.L_Edu;
                        cl.L_EduSchool = item.L_EduSchool;
                        cl.L_PosCode = string.IsNullOrEmpty(item.L_PosCode) ? 0 : Convert.ToInt32(item.L_PosCode);
                        cl.L_Address = item.L_Address;
                        cl.L_CPosCode = string.IsNullOrEmpty(item.L_CPosCode) ? 0 : Convert.ToInt32(item.L_CPosCode);
                        cl.L_CAddress = item.L_CAddress;
                        cl.L_Phone = item.L_Phone;
                        cl.L_CellPhone = item.L_CellPhone;
                        cl.L_Email = item.L_Email;
                        cl.L_Type = item.L_Type;
                        cl.L_TravelExpenses = item.L_TravelExpenses;
                        cl.L_IsCheck = string.IsNullOrEmpty(item.L_IsCheck) ? false : item.L_IsCheck.Equals("Y") ? true : false;
                        cl.L_Active = string.IsNullOrEmpty(item.L_Active) ? false : item.L_Active.Equals("N") ? true : false;
                        cl.L_Memo = item.L_Memo;
                        //cl.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //cl.CREDATE = item.CREDATE;

                        if (item.UPDUSER != null)
                        {
                            //cl.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                            //cl.UPDDATE = item.UPDDATE;
                        }

                        db.C_Lector.Add(cl);


                    }
                    count++;
                    db.SaveChanges();
                }

                Log("結束C_Lector");
                Console.WriteLine("結束C_Lector");
                Log("==========================");
                Console.WriteLine("==========================");
            }
            catch (Exception ex)
            {
                Console.WriteLine(count);
                Console.WriteLine("例外狀況");
                Log("例外狀況");
                Console.WriteLine(ex.ToString());
                Log(ex.ToString());
            }
            #endregion

            #region C_Book C_BookDetail 不塞
            //List<View_Bridge_C_BookPublish> V_BookPublishList = db.View_Bridge_C_BookPublish.ToList();
            //List<C_Book> C_BookList = db.C_Book.ToList();

            //try
            //{
            //    foreach (var item in V_BookPublishList)
            //    {
            //        C_Book cb = C_BookList.Where(x => x.M_Index.Equals(item.M_Index)).FirstOrDefault();
            //        if (cb != null)
            //        {
            //            //update
            //            cb.M_Index = item.M_Index;
            //            cb.M_Name = item.M_Name;
            //            //cb.M_Publish = string.IsNullOrEmpty(item.M_Publish) ? null : item.M_Publish;
            //            //cb.M_Number = string.IsNullOrEmpty(item.M_Number) ? null : item.M_Number;
            //            //if (!string.IsNullOrEmpty(item.M_Version) && item.M_Index != "08180")
            //            //{
            //            //    String Year = (Convert.ToDateTime(item.M_Version).Year + 1911).ToString();
            //            //    String Month = (Convert.ToDateTime(item.M_Version).Month).ToString();
            //            //    String Day = (Convert.ToDateTime(item.M_Version).Day).ToString();
            //            //    String Date = Year + "/" + Month + "/" + Day;
            //            //    cb.M_Version = Convert.ToDateTime(Date);
            //            //}
            //            //else
            //            //{
            //            //    cb.M_Version = null;
            //            //}
            //            cb.M_Order = item.M_Order;
            //            //cb.M_Memo = item.M_Memo;
            //            cb.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
            //            cb.CREDATE = DateTime.Now;
            //            //cb.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
            //            //cb.CREDATE = item.CREDATE;
            //            //cb.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
            //            //cb.UPDDATE = item.UPDDATE;
            //            db.SaveChanges();

            //        }
            //        else
            //        {
            //            //create
            //            cb = new C_Book();
            //            cb.M_ID = Guid.NewGuid();
            //            cb.M_Index = item.M_Index;
            //            cb.M_Name = item.M_Name;
            //            //cb.M_Publish = string.IsNullOrEmpty(item.M_Publish) ? null : item.M_Publish;
            //            //cb.M_Number = string.IsNullOrEmpty(item.M_Number) ? null : item.M_Number;
            //            //if (!string.IsNullOrEmpty(item.M_Version) && item.M_Index != "08180")
            //            //{
            //            //    String Year = (Convert.ToDateTime(item.M_Version).Year + 1911).ToString();
            //            //    String Month = (Convert.ToDateTime(item.M_Version).Month).ToString();
            //            //    String Day = (Convert.ToDateTime(item.M_Version).Day).ToString();
            //            //    String Date = Year + "/" + Month + "/" + Day;
            //            //    cb.M_Version = Convert.ToDateTime(Date);
            //            //}
            //            //else
            //            //{
            //            //    cb.M_Version = null;
            //            //}
            //            cb.M_Order = item.M_Order;
            //            //cb.M_Memo = item.M_Memo;
            //            cb.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
            //            cb.CREDATE = DateTime.Now;
            //            db.C_Book.Add(cb);
            //            db.SaveChanges();
            //            //cb.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
            //            //cb.CREDATE = item.CREDATE;
            //            //cb.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
            //            //cb.UPDDATE = item.UPDDATE;
            //        }



            //        C_Book cbC = db.C_Book.Where(x => x.M_Index.Equals(item.M_Index)).FirstOrDefault();
            //        C_BookDetail bd = db.C_BookDetail.Where(x => x.M_ID.Equals(cb.M_ID)).FirstOrDefault();
            //        if (bd != null)
            //        {
            //            //update                       
            //            bd.M_ID = cb.M_ID;
            //            //if (cbC.M_Publish != null)
            //            //{
            //            //    bd.MD_Publish = db.C_BookPublish.Where(x => x.BP_Name.Equals(cbC.M_Publish)).FirstOrDefault().BP_ID;
            //            //}
            //            bd.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
            //            bd.CREDATE = DateTime.Now;
            //            db.SaveChanges();
            //        }
            //        else
            //        {
            //            //create
            //            bd = new C_BookDetail();
            //            bd.MD_ID = Guid.NewGuid();
            //            bd.M_ID = cb.M_ID;
            //            //if (cbC.M_Publish != null)
            //            //{
            //            //    bd.MD_Publish = db.C_BookPublish.Where(x => x.BP_Name.Equals(cbC.M_Publish)).FirstOrDefault().BP_ID;
            //            //}
            //            bd.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
            //            bd.CREDATE = DateTime.Now;
            //            db.C_BookDetail.Add(bd);
            //            db.SaveChanges();
            //            //bd.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
            //            //bd.CREDATE = item.CREDATE;
            //            //bd.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
            //            //bd.UPDDATE = item.UPDDATE;
            //        }
            //    }

            //    db.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //}

            #endregion

            #region C_Class
            List<View_Bridge_C_Class> V_ClassClass = db.View_Bridge_C_Class.ToList();
            List<C_Class> C_ClassList = db.C_Class.ToList();

            Log("開始處理C_ClassList");
            Console.WriteLine("開始處理C_ClassList");

            try
            {
                foreach (var item in V_ClassClass)
                {
                    C_Class cc = C_ClassList.Where(x => x.C_ID.Equals(item.C_ID)).FirstOrDefault();
                    if (cc != null)
                    {
                        //update
                        cc.C_ID = item.C_ID;
                        cc.C_Name = item.C_Name;
                        cc.C_Type = item.C_Type;
                        cc.C_BookNum = item.C_BookNum;
                        cc.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                        cc.CREDATE = DateTime.Now;
                        //cc.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //cc.CREDATE = item.CREDATE;
                        //cc.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                        //cc.UPDDATE = item.UPDDATE;


                    }
                    else
                    {
                        //create
                        cc = new C_Class();
                        cc.C_UID = Guid.NewGuid();
                        cc.C_ID = item.C_ID;
                        cc.C_Name = item.C_Name;
                        cc.C_Type = item.C_Type;
                        cc.C_BookNum = item.C_BookNum;
                        cc.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                        cc.CREDATE = DateTime.Now;
                        //cc.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //cc.CREDATE = item.CREDATE;
                        //cc.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                        //cc.UPDDATE = item.UPDDATE;

                        db.C_Class.Add(cc);

                    }
                }
                db.SaveChanges();

                Log("結束C_ClassList");
                Console.WriteLine("結束C_ClassList");
                Log("==========================");
                Console.WriteLine("==========================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("例外狀況");
                Log("例外狀況");
                Console.WriteLine(ex.ToString());
                Log(ex.ToString());
            }

            #endregion

            #region C_ClassSubject
            List<View_Bridge_C_ClassSubject> V_ClassSubjectList = db.View_Bridge_C_ClassSubject.ToList();
            List<C_ClassSubject> C_ClassSubjectList = db.C_ClassSubject.ToList();

            Log("開始處理C_ClassSubject");
            Console.WriteLine("開始處理C_ClassSubject");

            try
            {
                foreach (var item in V_ClassSubjectList)
                {
                    C_ClassSubject cb = C_ClassSubjectList.Where(x => x.D_ID.Equals(item.D_ID)).FirstOrDefault();
                    if (cb != null)
                    {
                        //update
                        cb.C_UID = cb.C_UID = db.C_Class.Where(x => x.C_ID == item.C_UID).FirstOrDefault().C_UID;
                        cb.D_ID = item.D_ID;
                        cb.D_Name = item.D_Name;
                        cb.D_Hour = Convert.ToInt32(item.D_Hour);
                        cb.D_HourlyRate = Convert.ToInt32(item.C_UID);
                        cb.D_Type = item.D_Type;
                        cb.D_IsTest = string.IsNullOrEmpty(item.D_IsTest) ? false : item.D_IsTest.Equals("Y") ? true : false;
                        cb.D_Memo = item.D_Memo;
                        cb.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                        cb.CREDATE = DateTime.Now;
                        //cb.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //cb.CREDATE = item.CREDATE;
                        //cb.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                        //cb.UPDDATE = item.UPDDATE;

                    }
                    else
                    {
                        //create
                        cb = new C_ClassSubject();
                        cb.D_UID = Guid.NewGuid();
                        cb.C_UID = db.C_Class.Where(x => x.C_ID == item.C_UID).FirstOrDefault().C_UID;
                        cb.D_ID = item.D_ID;
                        cb.D_Name = item.D_Name;
                        cb.D_Hour = Convert.ToInt32(item.D_Hour);
                        cb.D_HourlyRate = Convert.ToInt32(item.C_UID);
                        cb.D_Type = item.D_Type;
                        cb.D_IsTest = string.IsNullOrEmpty(item.D_IsTest) ? false : item.D_IsTest.Equals("Y") ? true : false; ;
                        cb.D_Memo = item.D_Memo;
                        cb.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                        cb.CREDATE = DateTime.Now;
                        //cb.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //cb.CREDATE = item.CREDATE;
                        //cb.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                        //cb.UPDDATE = item.UPDDATE;
                        db.C_ClassSubject.Add(cb);

                    }
                }
                db.SaveChanges();

                Log("結束C_ClassSubject");
                Console.WriteLine("結束C_ClassSubject");
                Log("==========================");
                Console.WriteLine("==========================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("例外狀況");
                Log("例外狀況");
                Console.WriteLine(ex.ToString());
                Log(ex.ToString());
            }

            #endregion

            #region C_ClassLector
            List<View_Bridge_C_ClassLector> V_ClassLectorList = db.View_Bridge_C_ClassLector.ToList();
            List<C_ClassLector> C_ClassLectorList = db.C_ClassLector.ToList();

            Log("開始處理C_ClassLector");
            Console.WriteLine("開始處理C_ClassLector");

            try
            {
                foreach (var item in V_ClassLectorList)
                {
                    C_ClassLector ccl = C_ClassLectorList.Where(x => x.CL_UID.Equals(item.CL_UID)).FirstOrDefault();
                    if (ccl != null)
                    {
                        //update
                        ccl.CL_UID = item.CL_UID;
                        ccl.C_UID = db.C_Class.Where(x => x.C_ID == item.C_UID).FirstOrDefault().C_UID; //課程UID C_Class
                        ccl.D_UID = db.C_ClassSubject.Where(x => x.D_ID.Equals(item.D_UID)).FirstOrDefault().D_UID; //課程明細UID C_ClassSubject
                        ccl.L_UID = db.SYS_User.Where(x => x.U_Account.Equals(item.L_UID)).FirstOrDefault().U_ID; //講師UID
                        ccl.CL_Order = item.CL_Order;
                        ccl.CL_HourlyRate = Convert.ToInt32(item.CL_HourlyRate);
                        ccl.CL_Qualify = item.CL_Qualify;
                        ccl.CL_IsActive = string.IsNullOrEmpty(item.CL_IsActive) ? false : item.CL_IsActive.Equals("Y") ? true : false;
                        ccl.IsEvaluate = false;
                        ccl.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                        ccl.CREDATE = DateTime.Now;
                        //ccl.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //ccl.CREDATE = item.CREDATE;
                        //ccl.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                        //ccl.UPDDATE = item.UPDDATE;

                    }
                    else
                    {
                        //create
                        ccl = new C_ClassLector();
                        ccl.CL_UID = item.CL_UID;
                        ccl.C_UID = db.C_Class.Where(x => x.C_ID == item.C_UID).FirstOrDefault().C_UID; //課程UID C_Class
                        ccl.D_UID = db.C_ClassSubject.Where(x => x.D_ID.Equals(item.D_UID)).FirstOrDefault().D_UID; //課程明細UID C_ClassSubject
                        ccl.L_UID = db.SYS_User.Where(x => x.U_Account.Equals(item.L_UID)).FirstOrDefault().U_ID; //講師UID
                        ccl.CL_Order = item.CL_Order;
                        ccl.CL_HourlyRate = Convert.ToInt32(item.CL_HourlyRate);
                        ccl.CL_Qualify = item.CL_Qualify;
                        ccl.CL_IsActive = string.IsNullOrEmpty(item.CL_IsActive) ? false : item.CL_IsActive.Equals("Y") ? true : false;
                        ccl.IsEvaluate = false;
                        ccl.CREUSER = Guid.Parse("5357509C-6E68-4588-BC01-B5E005E30EF9"); //後台管理員
                        ccl.CREDATE = DateTime.Now;
                        //ccl.CREUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.CREUSER)).FirstOrDefault().U_ID;
                        //ccl.CREDATE = item.CREDATE;
                        //ccl.UPDUSER = db.SYS_User.Where(x => x.U_Name.Equals(item.UPDUSER)).FirstOrDefault().U_ID;
                        //ccl.UPDDATE = item.UPDDATE;
                        db.C_ClassLector.Add(ccl);

                    }
                }
                db.SaveChanges();

                Log("結束C_ClassLector");
                Console.WriteLine("結束C_ClassLector");
                Log("==========================");
                Console.WriteLine("==========================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("例外狀況");
                Log("例外狀況");
                Console.WriteLine(ex.ToString());
                Log(ex.ToString());
            }

            #endregion

            #region E_Evaluate/E_EvaluationSche

            //List<String> lst_mail = new List<string>();
            //Int32 Count = 0;

            //Log("開始處理E_Evaluate/E_EvaluationSche");
            //Console.WriteLine("開始處理E_Evaluate/E_EvaluationScher");

            //try
            //{
            //    List<C_ClassLector> cClassLectors = db.C_ClassLector.Where(x => x.IsEvaluate == false).OrderBy(x => x.C_UID).ToList();


            //    if (cClassLectors != null && cClassLectors.Count > 0)
            //    {
            //        foreach (C_ClassLector ccl in cClassLectors)
            //        {
            //            E_Evaluate eEvaluateOri = db.E_Evaluate.Where(x => x.matchKey == ccl.C_UID).FirstOrDefault() ?? null;
            //            if (eEvaluateOri != null)
            //            {
            //                E_EvaluationSche eEvaluationSche = new E_EvaluationSche()
            //                {
            //                    ES_ID = Guid.NewGuid(),
            //                    E_ID = eEvaluateOri.E_ID,
            //                    matchKey = ccl.CL_UID,
            //                    Reception = ccl.L_UID,
            //                    ChkNum = 0,
            //                    Status = 0,
            //                    IsMail = false,
            //                    ScheNum = 0,
            //                    IsSche = true,
            //                    IsClose = false,
            //                    IsPass = null,
            //                    CREUSER = ccl.CREUSER.Value,
            //                    CREDATE = ccl.CREDATE.Value
            //                };
            //                db.E_EvaluationSche.Add(eEvaluationSche);
            //                db.SaveChanges();
            //            }
            //            else
            //            {
            //                E_Evaluate eEvaluate = new E_Evaluate()
            //                {
            //                    E_ID = Guid.NewGuid(),
            //                    E_Type = 0,
            //                    E_Year = DateTime.Now.Year,
            //                    matchKey = ccl.C_UID.Value,
            //                    CREUSER = ccl.CREUSER.Value,
            //                    CREDATE = ccl.CREDATE.Value
            //                };
            //                db.E_Evaluate.Add(eEvaluate);

            //                E_EvaluationSche eEvaluationSche = new E_EvaluationSche()
            //                {
            //                    ES_ID = Guid.NewGuid(),
            //                    E_ID = eEvaluate.E_ID,
            //                    matchKey = ccl.CL_UID,
            //                    Reception = ccl.L_UID,
            //                    ChkNum = 0,
            //                    Status = 0,
            //                    IsMail = false,
            //                    ScheNum = 0,
            //                    IsSche = true,
            //                    IsClose = false,
            //                    CREUSER = ccl.CREUSER.Value,
            //                    CREDATE = ccl.CREDATE.Value
            //                };
            //                db.E_EvaluationSche.Add(eEvaluationSche);
            //                db.SaveChanges();
            //            }
            //            Count++;
            //            //CLector? cLector = db.CLectors.Find(ccl.LUid);
            //            //if (cLector != null)
            //            //{
            //            //    if (string.IsNullOrEmpty(cLector.LEmail))
            //            //    {
            //            //        //lst_mail.Add(cLector.LName);
            //            //        continue;
            //            //    }
            //            //    else
            //            //    {
            //            //        lst_mail.Add(cLector.LEmail);
            //            //    }
            //            //}
            //            ccl.IsEvaluate = true;
            //            db.SaveChanges();
            //        }
            //    }
            //    else
            //    {
            //        //Console.WriteLine("目前無新資料");
            //        //Log("目前無新資料");
            //    }

            //    Log("結束E_Evaluate/E_EvaluationSche");
            //    Console.WriteLine("結束E_Evaluate/E_EvaluationSche");
            //    Log("==========================");
            //    Console.WriteLine("==========================");

            //    //_smtpService.SendMail(lst_mail, "[通知]-CLEA授課資訊填寫", "老師您好，請至本會網站進行課程授課內容填寫，謝謝您。");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("例外狀況");
            //    Log("例外狀況");
            //    Console.WriteLine(ex.ToString());
            //    Log(ex.ToString());
            //}

            #endregion

            Log("=============結束程式=============");
            Console.WriteLine("=============結束程式=============");
            Console.ReadKey();



        }
    }
}

