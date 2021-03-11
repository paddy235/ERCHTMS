using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置实例表
    /// </summary>
    public class WfInstanceBLL
    {
        private WfInstanceIService service = new WfInstanceService();
        private WfSettingBLL wfsettingbll = new WfSettingBLL();
        private WfConditionBLL wfconditionbll = new WfConditionBLL();
        private WfConditionAddtionBLL wfconditionaddtionbll = new WfConditionAddtionBLL();


        #region  流程配置实例信息
        /// <summary>
        /// 流程配置实例信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetWfInstanceInfoPageList(Pagination pagination, string queryJson)
        {
            return service.GetWfInstanceInfoPageList(pagination, queryJson);
        }
        #endregion

        /// <summary>
        /// 获取流程对象
        /// </summary>
        /// <param name="instanceid"></param>
        /// <returns></returns>
        public DataTable GetActivityData(string instanceid)
        {
            return service.GetActivityData(instanceid);
        }

        #region 获取特定的流程配置实例
        /// <summary>
        /// 获取特定的流程配置实例
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="rankname"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public DataTable GetProcessData()
        {
            return service.GetProcessData();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WfInstanceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WfInstanceEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, WfInstanceEntity entity)
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

        #region 通过向导模式创建的新流程
        /// <summary>
        /// 通过向导模式创建的新流程
        /// </summary>
        /// <param name="queryJson"></param>
        public void SaveNewInstance(string queryJson)
        {
            try
            {
                List<ResultData> rlist = new List<ResultData>();
                var queryParam = queryJson.ToJObject();
                List<ResultData> resultList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultData>>(queryParam["RESULTDATA"].ToString());  //resultdata  
                string intanceid = queryParam["INSTANCEID"].ToString(); //实例id
                string isenable = queryParam["ISENABLE"].ToString(); //是否启用
                string instancename = queryParam["INSTANCENAME"].ToString(); //是否启用 
                #region 实例对象处理
                //获取当前实例
                WfInstanceEntity historyEntity = service.GetEntity(intanceid);
                //实例化新流程
                WfInstanceEntity entity = new WfInstanceEntity();
                entity = historyEntity;
                entity.INSTANCETYPE = "自有流程";
                entity.ID = null;
                entity.ISENABLE = isenable;
                entity.INSTANCENAME = instancename;
                //固定配置点 --机构
                rlist = resultList.Where(p => p.name == historyEntity.ORGANIZEID).ToList();
                //机构id
                if (rlist.Count() == 1)
                {
                    entity.ORGANIZEID = rlist.FirstOrDefault().value;
                }
                rlist = resultList.Where(p => p.name == historyEntity.ORGANIZENAME).ToList();
                //机构名称
                if (rlist.Count() == 1)
                {
                    entity.ORGANIZENAME = rlist.FirstOrDefault().value;
                }
                service.SaveForm("", entity); //成功创建新流程实例 
                #endregion

                #region 实例流程转向处理
                //处理流程转向配置
                List<WfSettingEntity> settingList = wfsettingbll.GetList(intanceid).ToList();
                foreach (WfSettingEntity historySetting in settingList)
                {
                    string settingId = historySetting.ID;
                    WfSettingEntity settingEntity = new WfSettingEntity();
                    settingEntity = historySetting;
                    settingEntity.ID = null;
                    settingEntity.INSTANCEID = entity.ID; //新的流程实例ID
                    string replaceStr = historySetting.FLOWCODE.Split('-')[0].ToString();
                    settingEntity.FLOWCODE = historySetting.FLOWCODE.Replace(replaceStr, Str.PinYin(entity.ORGANIZENAME).ToUpper());
                    //固定配置点 --起始脚本
                    if (!string.IsNullOrEmpty(historySetting.SCRIPTCURCONTENT))
                    {
                        foreach (ResultData resultdata in resultList)
                        {
                            if (historySetting.SCRIPTCURCONTENT.Contains(resultdata.name))
                            {
                                historySetting.SCRIPTCURCONTENT = historySetting.SCRIPTCURCONTENT.Replace(resultdata.name, resultdata.value);
                            }
                        }
                        settingEntity.SCRIPTCURCONTENT = historySetting.SCRIPTCURCONTENT;
                    }

                    //固定配置点--目标脚本
                    if (!string.IsNullOrEmpty(historySetting.SCRIPTCONTENT))
                    {
                        foreach (ResultData resultdata in resultList)
                        {
                            if (historySetting.SCRIPTCONTENT.Contains(resultdata.name))
                            {
                                historySetting.SCRIPTCONTENT = historySetting.SCRIPTCONTENT.Replace(resultdata.name, resultdata.value);
                            }
                        }
                        settingEntity.SCRIPTCONTENT = historySetting.SCRIPTCONTENT;
                    }
                    wfsettingbll.SaveForm("", settingEntity);


                    //新配置项
                    #region 新配置项
                    if (!string.IsNullOrEmpty(settingEntity.ID))
                    {
                        //新条件设置
                        List<WfConditionEntity> clist = wfconditionbll.GetList(settingId).ToList(); //历史流程转向条件明细
                        foreach (WfConditionEntity historyCondition in clist)
                        {
                            string conditionId = historyCondition.ID;
                            WfConditionEntity conditionEntity = new WfConditionEntity();
                            conditionEntity = historyCondition;
                            conditionEntity.ID = null;
                            conditionEntity.SETTINGID = settingEntity.ID;
                            //固定配置点 --机构
                            rlist = resultList.Where(p => p.name == historyCondition.ORGANIZEID).ToList();
                            //机构id
                            if (rlist.Count() == 1)
                            {
                                conditionEntity.ORGANIZEID = rlist.FirstOrDefault().value;
                            }
                            //机构名称
                            rlist = resultList.Where(p => p.name == historyCondition.ORGANIZENAME).ToList();
                            if (rlist.Count() == 1)
                            {
                                conditionEntity.ORGANIZENAME = rlist.FirstOrDefault().value;
                            }
                            //固定配置点--目标脚本
                            if (!string.IsNullOrEmpty(historyCondition.SQLCONTENT))
                            {
                                foreach (ResultData resultdata in resultList)
                                {
                                    if (historyCondition.SQLCONTENT.Contains(resultdata.name))
                                    {
                                        historyCondition.SQLCONTENT = historyCondition.SQLCONTENT.Replace(resultdata.name, resultdata.value);
                                    }
                                }
                                conditionEntity.SQLCONTENT = historyCondition.SQLCONTENT;
                            }

                            wfconditionbll.SaveForm("", conditionEntity);
                            //新具体条件内容明细
                            #region 新具体条件内容明细
                            if (!string.IsNullOrEmpty(conditionEntity.ID))
                            {
                                List<WfConditionAddtionEntity> calist = wfconditionaddtionbll.GetList(conditionId).ToList();
                                foreach (WfConditionAddtionEntity historyAddtion in calist)
                                {
                                    WfConditionAddtionEntity addtionEntiy = new WfConditionAddtionEntity();
                                    addtionEntiy = historyAddtion;
                                    addtionEntiy.ID = string.Empty;
                                    addtionEntiy.WFCONDITIONID = conditionEntity.ID;
                                    //固定配置点 --机构
                                    rlist = resultList.Where(p => p.name == historyAddtion.ORGANIZEID).ToList();
                                    //机构id
                                    if (rlist.Count() == 1)
                                    {
                                        addtionEntiy.ORGANIZEID = rlist.FirstOrDefault().value;
                                    }
                                    //机构名称
                                    rlist = resultList.Where(p => p.name == historyAddtion.ORGANIZENAME).ToList();
                                    if (rlist.Count() == 1)
                                    {
                                        addtionEntiy.ORGANIZENAME = rlist.FirstOrDefault().value;
                                    }
                                    //固定配置点 --部门
                                    rlist = resultList.Where(p => p.name == historyAddtion.DEPTNAME).ToList();
                                    if (rlist.Count() == 1)
                                    {
                                        addtionEntiy.DEPTNAME = rlist.FirstOrDefault().value;
                                    }
                                    rlist = resultList.Where(p => p.name == historyAddtion.DEPTID).ToList();
                                    if (rlist.Count() == 1)
                                    {
                                        addtionEntiy.DEPTID = rlist.FirstOrDefault().value;
                                    }
                                    rlist = resultList.Where(p => p.name == historyAddtion.DEPTCODE).ToList();
                                    if (rlist.Count() == 1)
                                    {
                                        addtionEntiy.DEPTCODE = rlist.FirstOrDefault().value;
                                    }
                                    //固定配置点 --人员
                                    rlist = resultList.Where(p => p.name == historyAddtion.USERNAME).ToList();
                                    if (rlist.Count() == 1)
                                    {
                                        addtionEntiy.USERNAME = rlist.FirstOrDefault().value;
                                    }
                                    rlist = resultList.Where(p => p.name == historyAddtion.USERACCOUNT).ToList();
                                    if (rlist.Count() == 1)
                                    {
                                        addtionEntiy.USERACCOUNT = rlist.FirstOrDefault().value;
                                    }
                                    wfconditionaddtionbll.SaveForm("", addtionEntiy);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                }
                #endregion

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 批量更新流程实例
        /// <summary>
        /// 批量更新流程实例Id
        /// </summary>
        /// <param name="typename"></param>
        public void BatchUpdateInstance(string typename)
        {
            try
            {
                service.BatchUpdateInstance(typename);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}