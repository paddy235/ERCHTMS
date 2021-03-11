using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Busines.JPush;
using System.Collections;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患基本信息表
    /// </summary>
    public class HTBaseInfoBLL
    {
        private HTBaseInfoIService service = new HTBaseInfoService();
        private ChangePlanDetailIService cpservice = new ChangePlanDetailService(); //整改计划对象
        private ExpirationTimeSettingIService epservice = new ExpirationTimeSettingService(); //到期设置对象
        private HTChangeInfoIService chservice = new HTChangeInfoService(); //隐患整改

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTBaseInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        public IList<HTBaseInfoEntity> GetListByCode(string hidcode)
        {
            return service.GetListByCode(hidcode);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTBaseInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取安全预警
        /// </summary>
        /// <returns></returns>
        public DataTable GetHidSafetyWarning(int type, string orgcode)
        {
            return service.GetHidSafetyWarning(type, orgcode);
        }

        #region 获取首页隐患统计
        /// <summary>
        /// 获取首页隐患统计
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="curYear"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public DataTable GetHomePageHiddenByHidType(Operator curUser, int curYear, int topNum, int qType)
        {
            return service.GetHomePageHiddenByHidType(curUser, curYear, topNum, qType);
        }
        #endregion

        #region 根据部门编码获取
        /// <summary>
        /// 根据部门编码获取
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="encode"></param>
        /// <param name="curYear"></param>
        /// <param name="qType"></param>
        /// <returns></returns>
        public DataTable GetHomePageHiddenByDepart(string orginezeId, string encode, string curYear, int qType)
        {
            return service.GetHomePageHiddenByDepart(orginezeId, encode, curYear, qType);
        }
        #endregion
        /// <summary>
        /// 电力安全隐患排查治理情况月报表
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="curdate"></param>
        /// <returns></returns>
        public DataTable GetHiddenSituationOfMonth(string deptcode, string curdate, Operator curUser)
        {
            return service.GetHiddenSituationOfMonth(deptcode, curdate, curUser);
        }

        /// <summary>
        /// 违章列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetRulerPageList(Pagination pagination, string queryJson)
        {
            return service.GetRulerPageList(pagination, queryJson);
        }


        public DataTable GetGeneralQuery(string sql, Pagination pagination)
        {
            return service.GetGeneralQuery(sql, pagination);
        }

        /// <summary>
        /// 获取通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetGeneralQueryBySql(string sql)
        {
            return service.GetGeneralQueryBySql(sql);
        }

        /// <summary>
        /// 隐患统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <param name="hidrank"></param>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsByAction(StatisticsEntity sentity)
        {
            return service.QueryStatisticsByAction(sentity);
        }


        public DataTable GetList(string checkId, string checkman, string districtcode, string workstream)
        {
            return service.GetList(checkId, checkman, districtcode, workstream);
        }

        /// <summary>
        /// 获取当前用户下的隐患描述
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetDescribeListByUserId(string userId, string hiddescribe)
        {
            return service.GetDescribeListByUserId(userId, hiddescribe);
        }


        /// <summary>
        /// 获取隐患预警信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetHiddenInfoOfWarning(Operator user, string startDate, string endDate)
        {
            return service.GetHiddenInfoOfWarning(user, startDate, endDate);
        }


        /// <summary>
        /// 获取指定年月的隐患指标记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public decimal GetHiddenWarning(Operator user, string startDate)
        {
            return service.GetHiddenWarning(user, startDate);
        }

        #region 重要指标(省级)
        /// <summary>
        /// 重要指标(省级)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetImportantIndexForProvincial(int action, Operator user)
        {
            try
            {
                return service.GetImportantIndexForProvincial(action, user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 获取双控工作列表
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable QueryHidWorkList(Operator curUser)
        {
            return service.QueryHidWorkList(curUser);
        }

        /// <summary>
        /// 待办记录
        /// </summary>
        /// <param name="value"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable QueryHidBacklogRecord(string value, string userId)
        {
            return service.QueryHidBacklogRecord(value, userId);
        }

        /// <summary>
        /// 隐患曝光记录
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureHid(string num)
        {
            return service.QueryExposureHid(num);
        }

        public DataTable GetAppHidStatistics(string code, int mode, int category = 2)
        {
            return service.GetAppHidStatistics(code, mode, category);
        }

        public DataTable GetBaseInfoForApp(Pagination pagination)
        {
            return service.GetBaseInfoForApp(pagination);
        }
        /// <summary>
        /// 按月统计当天登记的违章并且当天未验收的违章数量和当天登记的违章的总数量
        /// </summary>
        /// <param name="currDate">时间</param>
        ///  <param name="deptCode">部门Code</param>
        /// <returns></returns>
        public DataTable GetLllegalRegisterNumByMonth(string currDate, string deptCode)
        {
            return service.GetLllegalRegisterNumByMonth(currDate, deptCode);
        }

        public object GetHiddenInfoOfEveryMonthWarning(Operator user, string startDate, string endDate)
        {
            return service.GetHiddenInfoOfEveryMonthWarning(user, startDate, endDate);
        }

        #region 获取关联对象下的隐患信息
        /// <summary>
        /// 获取关联对象下的隐患信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHiddenByRelevanceId(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetHiddenByRelevanceId(pagination, queryJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 省级统计数据
        /// <summary>
        /// 省级统计数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable QueryProvStatisticsByAction(ProvStatisticsEntity entity)
        {
            try
            {
                return service.QueryProvStatisticsByAction(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

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
        public void SaveForm(string keyValue, HTBaseInfoEntity entity)
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
        #endregion

        #region 获取隐患分页列表
        /// <summary>
        /// 获取隐患列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetHiddenBaseInfoPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetHiddenBaseInfoPageList(pagination, queryJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取隐患详情
        public DataTable GetHiddenByKeyValue(string keyValue)
        {
            try
            {
                return service.GetHiddenByKeyValue(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 导出安全检查对应的隐患内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetHiddenOfSafetyCheck(string keyValue, int mode)
        {
            try
            {
                return service.GetHiddenOfSafetyCheck(keyValue, mode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion




        #region 保存整改计划对象
        /// <summary>
        /// 保存整改计划对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveChangePlan(string keyValue, ChangePlanDetailEntity entity)
        {
            try
            {
                cpservice.SaveForm(keyValue, entity);
                //仅推送一次
                if (string.IsNullOrEmpty(keyValue))
                {
                    HTBaseInfoEntity bentity = service.GetEntity(entity.HIDDENID);
                    HTChangeInfoEntity centity = chservice.GetEntityByHidCode(bentity.HIDCODE);
                    string pushcode = "YH017"; //整改计划

                    string sql = string.Format(@"select  f.createuser  account, d.realname username,c.encode deptcode ,c.fullname deptname   from bis_htbaseinfo  a   
                                                left join sys_wftbinstance  g on a.id = g.objectid    
                                                left join sys_wftbinstancedetail f on g.currentdetailid = f.id 
                                                left  join base_department c on f.createuserdeptcode= c.encode  
                                                left  join base_user  d on f.createuser =d.account   where a.id='{0}' ", entity.HIDDENID);

                    var dt = service.GetGeneralQueryBySql(sql);


                    string pushaccount = string.Empty; //推送人员账户
                    string pushname = string.Empty; //推送人员名称
                    string message = string.Format(@"您好，{0} {1}于{2}针对《{3}》的隐患已提交了整改计划，请您前往隐患台账查看该条隐患的整改计划。",
                        centity.CHANGEDUTYDEPARTNAME, centity.CHANGEPERSONNAME, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), bentity.HIDDESCRIBE);

                    if (dt.Rows.Count > 0)
                    {
                        pushaccount = dt.Rows[0]["account"].ToString();
                        pushname = dt.Rows[0]["username"].ToString();
                    }
                    JPushApi.PushMessage(pushaccount, pushname, pushcode, "整改计划消息", message, entity.HIDDENID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取整改计划对象
        /// <summary>
        /// 获取整改计划对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ChangePlanDetailEntity GetChangePlanEntity(string keyValue)
        {
            try
            {
                return cpservice.GetEntity(keyValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region 获取到期配置集合

        /// <summary>
        /// 获取到期配置集合
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public List<ExpirationTimeSettingEntity> GetExpList(string orgId, string modulename)
        {
            try
            {
                List<ExpirationTimeSettingEntity> list = epservice.GetList("").Where(p => p.ORGANIZEID == orgId && p.MODULENAME == modulename).ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region 保存到期时间设置对象
        /// <summary>
        /// 保存到期时间设置对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveExpirationTimeEntity(string keyValue, ExpirationTimeSettingEntity entity)
        {
            try
            {
                epservice.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 江凌首页-未整改隐患

        /// <summary>
        /// 获取江凌首页-未整改隐患
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetNoChangeHidList(string code)
        {
            try
            {
                return service.GetNoChangeHidList(code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取区域下未整改的隐患
        /// </summary>
        /// <param name="areaCodes">区域列表</param>
        /// <returns></returns>
        public IList GetCountByArea(List<string> areaCodes)
        {
            return service.GetCountByArea(areaCodes);
        }
        #endregion
    }
}
