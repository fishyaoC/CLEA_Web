namespace Clea_Web.ViewModels
{
    public class BaseViewModel
    {
        //public errorMsg Error { get; set; }

        #region 新增、更新人員與時間
        public Guid Creuser { get; set; }

        public DateTime Credate { get; set; }

        public Guid? Upduser { get; set; }

        public DateTime? Upddate { get; set; }

        #endregion


        #region Error Message
        public class errorMsg { 
            public Boolean CheckMsg { get; set; }
            public String ErrorMsg { get; set; }

        }
        #endregion

        #region 選單模組
        public class SearchDropDownItem
        {
            public String Text { get; set; }
            public String Value { get; set; }
        }
        #endregion
    }
}
