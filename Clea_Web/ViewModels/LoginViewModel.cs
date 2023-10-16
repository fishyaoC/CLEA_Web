﻿using Clea_Web.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Login loginVM { get; set; }
        #region
        public class Login
        {
            [Required(ErrorMessage ="{0} 為必填!")]
            [DisplayName("帳號")]
            [DataType(DataType.Text)]
            public String Account { get; set; }

            [Required(ErrorMessage = "{0} 為必填!")]
            [DisplayName("密碼")]
            [DataType(DataType.Text)]
            public String PassWord { get; set; }

            /// <summary>
            /// 是否為測試模式
            /// </summary>
            public Boolean IsTest { get; set; }=false;
        }
        #endregion

        #region LoginRole
        public class LoginRoleInfo
        {
            public String U_Account { get; set; }
            public String U_Name { get; set; }
            public String R_UID { get; set; }
            public String U_ID { get; set; }
            public String R_BackEnd { get; set; }
        }
        #endregion
    }
}
