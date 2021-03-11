using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using ERCHTMS.Service.SafetyWorkSupervise;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Busines.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办
    /// </summary>
    public class SafetyworksuperviseBLL
    {
        private SafetyworksuperviseIService service = new SafetyworksuperviseService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyworksuperviseEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyworksuperviseEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable GetEntityByT(string keyValue, string fid)
        {
            return service.GetEntityByT(keyValue,fid);
        }
        /// <summary>
        /// 流程图信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue)
        {
            return service.GetFlow(keyValue);
        }
        public int GetSuperviseNum(string userid, string type)
        {
            return service.GetSuperviseNum(userid, type);
        }
        /// <summary>
        /// 获取首页统计图
        /// </summary>
        /// <param name="keyValue">1表示上个月</param>
        /// <returns></returns>
        public DataTable GetSuperviseStat(string keyValue)
        {
            return service.GetSuperviseStat(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyworksuperviseEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                if (entity.FlowState == "1") {
                    UserBLL userbll = new UserBLL();
                    UserEntity userEntity = userbll.GetEntity(entity.DutyPersonId);//获取责任人用户信息
                    JPushApi.PushMessage(userEntity.Account, entity.DutyPerson, "GZDB001", "例行安全工作", entity.Id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 批量确认
        /// </summary>
        /// <param name="SuperviseIds">督办任务ID(多个用逗号分隔)</param>
        /// <param name="result">审核结果（0：同意，1：不同意）</param>
        /// <param name="signImg">签名照片路径</param>
        /// <returns></returns>
        public bool MutilConfirm(string SuperviseIds, string result, string signImg,string remark="")
        {

            try
            {
                SuperviseconfirmationBLL superviseconfirmationbll = new SuperviseconfirmationBLL();
                DepartmentBLL departmentBLL = new DepartmentBLL();
                string sql = string.Format("update BIS_SAFETYWORKSUPERVISE set FlowState='3',remark='{1}' where id in('{0}')", SuperviseIds.Trim(',').Replace(",", "','"),remark);
                int count = new DepartmentBLL().ExecuteSql(sql);
                if (count > 0)
                {
                    string[] arr = SuperviseIds.Trim(',').Split(',');
                    foreach (string str in arr)
                    {
                        DataTable dt = departmentBLL.GetDataTable(string.Format(@"select t1.id
 from BIS_SafetyWorkSupervise t left join (select id, superviseid from BIS_SafetyWorkFeedback where flag = '0') t1
on t.id = t1.superviseid left join(select feedbackid from BIS_SuperviseConfirmation where flag = '0') t2 on t1.id = t2.feedbackid
where t.id = '{0}'", str));
                        if (dt.Rows.Count > 0)
                        {
                            SuperviseconfirmationEntity sf = new SuperviseconfirmationEntity
                            {
                                SuperviseResult = result,
                                ConfirmationDate = DateTime.Now,
                                SignUrl = signImg,
                                FeedbackId = dt.Rows[0][0].ToString(),
                                SuperviseId = str,
                                Flag = "0"
                            };
                            superviseconfirmationbll.SaveForm(str, sf);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
