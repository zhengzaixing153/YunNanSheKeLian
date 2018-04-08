using Learun.Application.Base.OrganizationModule;
using Learun.Application.Base.SystemModule;
using Learun.Util;
using Learun.Util.Operat;
using Nancy;

namespace Learun.Application.WebApi
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.05.12
    /// 描 述：用户信息
    /// </summary>
    public class UserApi : BaseApi
    {
         /// <summary>
        /// 注册接口
        /// </summary>
        public UserApi()
            : base("/learun/adms")
        {
            Post["/login"] = Login;
        }
        private UserIBLL userBll = new UserBLL();

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response Login(dynamic _)
        {
            LoginModel loginModel = this.GetReqData<LoginModel>();

            #region 内部账户验证
            UserEntity userEntity = userBll.CheckLogin(loginModel.username, loginModel.password);

            #region 写入日志
            LogEntity logEntity = new LogEntity();
            logEntity.F_CategoryId = 1;
            logEntity.F_OperateTypeId = ((int)OperationType.Login).ToString();
            logEntity.F_OperateType = EnumAttribute.GetDescription(OperationType.Login);
            logEntity.F_OperateAccount = loginModel.username + "(" + userEntity.F_RealName + ")";
            logEntity.F_OperateUserId = !string.IsNullOrEmpty(userEntity.F_UserId) ? userEntity.F_UserId : loginModel.username;
            logEntity.F_Module = Config.GetValue("SoftName");
            #endregion

            if (!userEntity.LoginOk)//登录失败
            {
                //写入日志
                logEntity.F_ExecuteResult = 0;
                logEntity.F_ExecuteResultJson = "登录失败:" + userEntity.LoginMsg;
                logEntity.WriteLog();
                return Fail(userEntity.LoginMsg);
            }
            else
            {
                string token = OperatorHelper.Instance.AddLoginUser(userEntity.F_Account, "Learun_ADMS_6.1_App", this.loginMark, false);//写入缓存信息
                //写入日志
                logEntity.F_ExecuteResult = 1;
                logEntity.F_ExecuteResultJson = "登录成功";
                logEntity.WriteLog();
                return Success<string>(token);
            }
            #endregion
        }
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginModel {
        /// <summary>
        /// 账号
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}