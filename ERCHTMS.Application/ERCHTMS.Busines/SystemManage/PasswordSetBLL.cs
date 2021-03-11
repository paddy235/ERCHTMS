using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using BSFramework.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ERCHTMS.Busines.SystemManage
{
    /// <summary>
    /// 描 述：区域管理
    /// </summary>
    public class PasswordSetBLL
    {
        private IPasswordSetService service = new PasswordSetService();
       

        #region 获取数据
        /// <summary>
        /// 区域列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PasswordSetEntity> GetList(string orgCode)
        {
            return service.GetList(orgCode);
        }
        /// <summary>
        /// 区域实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PasswordSetEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public List<string> IsPasswordRuleStatus(ERCHTMS.Code.Operator user)
        {
            List<string> list = new List<string>();
            PasswordSetEntity ps = GetList(user.OrganizeCode).Where(t => t.OrgCode==user.OrganizeCode &&　t.Status == 1).FirstOrDefault();
            if (ps!=null)
            {
                // var reg1 = /^.*(?=.{8,})(?=.*\d)(?=.*[A-Za-z]{1,})(?=.*[~_!=@#\$%^&\*\?\(\)]).*$/;
                if (ps.Status==1)
                {
                    string reg = "(?=.{" + ps.Len + ",})";
                    string[] arr = ps.Rule.Split(';');
                    if (arr[2].Trim().Length > 0)
                    {
                        reg += @"(?=.*\d)";
                    }
                    if (arr[0].Trim().Length > 0)
                    {
                        reg += @"(?=.*[A-Z]{1,})";
                    }
                    if (arr[1].Trim().Length > 0)
                    {
                        reg += @"(?=.*[a-z]{1,})";
                    }
                    if (arr[3].Trim().Length > 0)
                    {
                        reg += @"(?=.*[~_!=@#\$%^&\*\?\(\)])";
                    }
                    string rule = string.Format("^.*{0}.*$", reg);
                    list.Add("true");
                    list.Add(ps.Rule);
                    list.Add(ps.Remark);
                    list.Add(ps.Len.ToString());
                    list.Add(rule);
                    list.Add(ps.Num.ToString());
                }
            }
            else
            {
                list.Add("");
                list.Add("");
                list.Add("");
                list.Add("6");
                list.Add("");
                list.Add("0");
            }
            return list;
         
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool RemoveForm(string keyValue)
        {
            try
            {
               return service.RemoveForm(keyValue);
               
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 保存区域表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="{">区域实体</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, PasswordSetEntity areaEntity)
        {
            try
            {
                return service.SaveForm(keyValue, areaEntity);
                 
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
