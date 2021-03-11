using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using Newtonsoft.Json;
using System;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：入厂许可申请
    /// </summary>
    public class IntromissionService : RepositoryFactory<IntromissionEntity>, IntromissionIService
    {
        private OutsouringengineerService outsouringengineerservice = new OutsouringengineerService();
        private DepartmentService departmentservice = new DepartmentService();
        private ManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<IntromissionEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public IntromissionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 通过语句查询返回表集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }


        #region 通过入厂许可获取外包工程相关信息

        /// <summary>
        /// 通过入厂许可获取外包工程相关信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetOutSourcingProjectByIntromId(string keyValue)
        {
            string sql = string.Format(@"select b.engineername,b.engineercode,a.outengineerid, c.fullname,to_char(a.createdate,'yyyymmdd') applyno,b.engineerletpeople,b.engineerletpeoplephone from  epg_intromission  a
                                        left join epg_outsouringengineer b  on a.outengineerid=b.id  
                                        left join base_department c on b.outprojectid=c.departmentid  where a.id  ='{0}' ", keyValue);
            return this.BaseRepository().FindTable(sql);

        } 
        #endregion

        /// <summary>  //获取审查记录信息
        /// 通过入厂许可申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        /// 
        public DataTable GetDtRecordList(string keyValue)
        {
            string sql = string.Format(@"  select  sortid  serialnumber,  a.id,a.investigatecontentid, a.investigatecontent ,a.investigateresult,a.investigatepeople ,a.investigatepeopleid,a.signpic from epg_investigatedtrecord a
                                   left join epg_investigaterecord b on a.investigaterecordid = b.id
                                   left join epg_intromission c on b.intofactoryid = c.id left join EPG_INVESTIGATECONTENT d on a.INVESTIGATECONTENTID=d.id  where c.id ='{0}' order by to_number(d.sortid) asc ", keyValue);
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>  //获取审查记录信息
        /// 通过入场许可申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetStartRecordList(string keyValue)
        {
            string sql = string.Format(@" select a.id , a.investigatecontent ,a.investigateresult,a.investigatepeople ,a.investigatepeopleid,a.signpic from epg_investigatedtrecord a 
                                   left join  epg_investigaterecord b on a.investigaterecordid = b.id 
                                   left join epg_startapplyfor c on  c.id = b.intofactoryid left join EPG_INVESTIGATECONTENT d on a.INVESTIGATECONTENTID=d.id  where c.id ='{0}' order by  to_number(d.sortid) asc ", keyValue);
            return this.BaseRepository().FindTable(sql);
        }
        public DataTable GetHistoryStartRecordList(string keyValue)
        {
            string sql = string.Format(@" select a.id , a.investigatecontent ,a.investigateresult,a.investigatepeople ,a.investigatepeopleid,a.signpic from epg_investigatedtrecord a 
                                   left join  epg_investigaterecord b on a.investigaterecordid = b.id 
                                   left join epg_historystartapplyfor c on  c.id = b.intofactoryid  left join EPG_INVESTIGATECONTENT d on a.INVESTIGATECONTENTID=d.id  where c.id ='{0}' order  by to_number(d.sortid) asc ", keyValue);
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>  //获取历史审查记录信息
        /// 通过入厂许可申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetHistoryDtRecordList(string keyValue)
        {
            string sql = string.Format(@"  select  a.id,a.investigatecontentid, a.investigatecontent ,a.investigateresult,a.investigatepeople ,a.investigatepeopleid,a.signpic from epg_investigatedtrecord a
                                   left join epg_investigaterecord b on a.investigaterecordid = b.id
                                   left join  epg_intromissionhistory c on b.intofactoryid = c.id  left join EPG_INVESTIGATECONTENT d on a.INVESTIGATECONTENTID=d.id  where c.id ='{0}' order by to_number(d.sortid) asc  ", keyValue);
            return this.BaseRepository().FindTable(sql);
        }

        #region 获取审查数据
        /// <summary>
        /// 获取审查数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetIntromissionPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                //时间范围
                if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
                {
                    string startTime = queryParam["sTime"].ToString();
                    string endTime = queryParam["eTime"].ToString();
                    if (queryParam["sTime"].IsEmpty())
                    {
                        startTime = "1899-01-01";
                    }
                    if (queryParam["eTime"].IsEmpty())
                    {
                        endTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and to_date(to_char(a.applytime,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
                //查询条件
                if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
                }
                if (!queryParam["indexState"].IsEmpty())//首页代办
                {
                    string strCondition = "";
                    strCondition = string.Format(" and a.createuserorgcode='{0}' and a.investigatestate !='3'", user.OrganizeCode);
                    DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        var engineerEntity = outsouringengineerservice.GetEntity(data.Rows[i]["outengineerid"].ToString());
                        var excutdept = departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                        var outengineerdept = departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                        var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        //获取下一步审核人
                        string str = powerCheck.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                        data.Rows[i]["approveuserids"] = str;
                    }

                    string[] applyids = data.Select(" approveuserids like '%" + user.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                    pagination.conditionJson += string.Format(" and a.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }
                if (!queryParam["projectid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and b.id ='{0}'", queryParam["projectid"].ToString());
                }
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, IntromissionEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}