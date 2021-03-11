using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// 描 述：作业现场安全管控 
    /// </summary>
    public class SafeworkcontrolBLL
    {
        private SafeworkcontrolIService service = new SafeworkcontrolService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafeworkcontrolEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 根据状态获取现场作业信息
        /// </summary>
        /// <param name="State">1开始 2结束</param>
        /// <returns></returns>
        public IEnumerable<SafeworkcontrolEntity> GetStartList(int Stete)
        {
            return service.GetStartList(Stete);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();


            if (!queryParam["selectStatus"].IsEmpty())
            {//作业类型
                string Type = queryParam["selectStatus"].ToString();
                pagination.conditionJson += string.Format(" and taskType = '{0}' ", Type);
            }
            if (!queryParam["LevelName"].IsEmpty())
            {//作业类型
                string Type = queryParam["LevelName"].ToString();
                pagination.conditionJson += string.Format(" and DangerLevel = '{0}' ", Type);
            }
            if (!queryParam["deptCode"].IsEmpty())
            {//部门
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(deptcode,'{0}')=1", deptCode);
            }
            if (!queryParam["AreaCode"].IsEmpty())
            {//区域 
                string AreaCode = queryParam["AreaCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(Taskregioncode,'{0}')=1", AreaCode);
            }
            if (!queryParam["State"].IsEmpty())
            {//作业状态 1作业中 2作业结束
                string State = queryParam["State"].ToString();
                pagination.conditionJson += string.Format(" and State ={0} ", State);
            }
            //根据时间进行筛选
            if (!queryParam["startTime"].IsEmpty() || !queryParam["endTime"].IsEmpty())
            {

                string startTime = queryParam["startTime"].ToString();
                string endTime = queryParam["endTime"].ToString();
                if (!string.IsNullOrEmpty(startTime))
                {
                    pagination.conditionJson += string.Format("and ActualStartTime >= TO_Date('{0}','yyyy-mm-dd hh24:mi') ", startTime);
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    pagination.conditionJson += string.Format("and ActualStartTime <= TO_Date('{0}', 'yyyy-mm-dd hh24:mi') ", endTime);
                }

                //pagination.conditionJson +=
                //    string.Format(
                //        " and ActualStartTime >= TO_Date('{0}','yyyy-mm-dd hh24:mi') and  ActualStartTime <= TO_Date('{1}','yyyy-mm-dd hh24:mi') ",
                //        startTime, endTime);
            }
            if (!queryParam["Search"].IsEmpty())
            {//关键字查询
                string Search = queryParam["Search"].ToString();
                pagination.conditionJson += string.Format(" and  (workno like '%{0}%' or taskname like '%{0}%' or tasktype like '%{0}%' or deptname like '%{0}%') ", Search);
            }
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetUserPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            if (!queryParam["deptCode"].IsEmpty())
            {//部门
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(t.deptcode,'{0}')=1", deptCode);
            }
            //根据时间进行筛选
            if (!queryParam["startTime"].IsEmpty() || !queryParam["endTime"].IsEmpty())
            {

                string startTime = queryParam["startTime"].ToString();
                string endTime = queryParam["endTime"].ToString();
                if (!string.IsNullOrEmpty(startTime))
                {
                    pagination.conditionJson +=
                    string.Format(
                        " and warningtime >= TO_Date('{0}','yyyy-mm-dd hh24:mi')", startTime);
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    pagination.conditionJson +=
                     string.Format(
                         " and warningtime <= TO_Date('{0}','yyyy-mm-dd hh24:mi')", endTime);
                }

                //pagination.conditionJson +=
                //    string.Format(
                //        " and warningtime >= TO_Date('{0}','yyyy-mm-dd hh24:mi') and  warningtime <= TO_Date('{1}','yyyy-mm-dd hh24:mi') ",
                //        startTime, endTime);
            }
            if (!queryParam["Search"].IsEmpty())
            {//关键字查询
                string Search = queryParam["Search"].ToString();
                pagination.conditionJson += string.Format(" and  (d.warningcontent like '%{0}%' or d.liablename like '%{0}%' or t.deptname like '%{0}%') ", Search);
            }
            return service.GetPageList(pagination, queryJson);
        }

        public DataTable GetWaringPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            if (!queryParam["deptCode"].IsEmpty())
            {//部门
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(t.departmentcode,'{0}')=1", deptCode);
            }
            //根据时间进行筛选
            if (!queryParam["startTime"].IsEmpty() || !queryParam["endTime"].IsEmpty())
            {

                string startTime = queryParam["startTime"].ToString();
                string endTime = queryParam["endTime"].ToString();
                if (!string.IsNullOrEmpty(startTime))
                {
                    pagination.conditionJson +=
                    string.Format(
                        " and warningtime >= TO_Date('{0}','yyyy-mm-dd hh24:mi')", startTime);
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    pagination.conditionJson +=
                     string.Format(
                         " and warningtime <= TO_Date('{0}','yyyy-mm-dd hh24:mi')", endTime);
                }
            }
            if (!queryParam["Search"].IsEmpty())
            {//关键字查询
                string Search = queryParam["Search"].ToString();
                pagination.conditionJson += string.Format(" and  (d.warningcontent like '%{0}%' or d.liablename like '%{0}%' or t.deptname like '%{0}%') ", Search);
            }
            return service.GetPageList(pagination, queryJson);
        }



        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafeworkcontrolEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取预警列表信心
        /// </summary>
        /// <param name="type">0人员预警 1现场工作</param>
        /// <returns></returns>
        public List<WarningInfoEntity> GetWarningInfoList(int type)
        {
            return service.GetWarningInfoList(type);
        }
        /// <summary>
        /// 获取所有预警信息列表
        /// </summary>
        /// <returns></returns>
        public List<WarningInfoEntity> GetWarningAllList()
        {
            return service.GetWarningAllList();
        }
        /// <summary>
        /// 获取人员安全管控各个时段人数
        /// </summary>
        /// <returns></returns>
        public List<KbsEntity> GetDayTimeIntervalUserNum()
        {
            return service.GetDayTimeIntervalUserNum();
        }

        /// <summary>
        /// 获取到所有当前开始的作业
        /// </summary>
        /// <returns></returns>
        public List<SafeworkcontrolEntity> GetNowWork()
        {
            return service.GetNowWork();
        }


        /// <summary>
        /// 获取今日高风险作业
        /// </summary>
        /// <returns></returns>    
        public List<SafeworkcontrolEntity> GetDangerWorkToday(string level)
        {
            return service.GetDangerWorkToday(level);
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
        public void SaveForm(string keyValue, SafeworkcontrolEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 更新作业成员是否监管区域内状态
        /// </summary>
        /// <returns></returns>
        public void SaveSafeworkUserStateIofo(string workid, string userid, int state)
        {
            try
            {
                service.SaveSafeworkUserStateIofo(workid, userid, state);
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
        public void AppSaveForm(string keyValue, SafeworkcontrolEntity entity)
        {
            try
            {
                service.AppSaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        #endregion

        #region 预警信息
        /// <summary>
        /// 获取预警信息实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public WarningInfoEntity GetWarningInfoEntity(string keyValue)
        {
            try
            {
                return service.GetWarningInfoEntity(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 保存预警（新增、修改）
        /// </summary>
        /// <param name="type">0新增 1修改</param>
        /// <param name="list"></param>
        public void SaveWarningInfoForm(int type, IList<WarningInfoEntity> list)
        {
            try
            {
                service.SaveWarningInfoForm(type, list);
            }
            catch (Exception er)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存预警（新增、修改）
        /// </summary>
        /// <param name="type">0新增 1修改</param>
        /// <param name="list"></param>
        public void SaveWarningInfoForm(string keyValue, WarningInfoEntity entity)
        {
            try
            {
                service.SaveWarningInfoForm(keyValue, entity);
            }
            catch (Exception er)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除预警信息
        /// </summary>
        /// <param name="keyValue"></param>
        public void SaveWarningInfoForm(string keyValue)
        {
            try
            {
                service.DelWarningInForm(keyValue);
            }
            catch (Exception er)
            {
                throw;
            }
        }
        /// <summary>
        /// 批量删除预警信息（通过主表记录Id）
        /// </summary>
        /// <param name="BaseId"></param>
        public void DelBatchWarningInForm(string BaseId)
        {
            try
            {
                service.DelBatchWarningInForm(BaseId);
            }
            catch (Exception er)
            {
                throw;
            }
        }
        #endregion



        /// <summary>
        /// 获取已结束的预警信息
        /// </summary>
        public List<WarningInfoEntity> GetBatchWarningInfoList(string WorkId)
        {
            return service.GetBatchWarningInfoList(WorkId);
        }

        public List<RiskAssessEntity> GetDistrictLevel()
        {
            return service.GetDistrictLevel();
        }
    }
}
