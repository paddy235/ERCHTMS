using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：日常考核表
    /// </summary>
    public class DailyexamineService : RepositoryFactory<DailyexamineEntity>, DailyexamineIService
    {
        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                if (!string.IsNullOrEmpty(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["qtype"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and ((flowdept ='{0}'", currUser.DeptId);

                        string[] arr = currUser.RoleName.Split(',');
                        if (arr.Length > 0)
                        {
                            pagination.conditionJson += " and (";
                            foreach (var item in arr)
                            {
                                pagination.conditionJson += string.Format(" flowrolename  like '%{0}%' or", item);
                            }
                            pagination.conditionJson = pagination.conditionJson.Substring(0, pagination.conditionJson.Length - 2);
                            pagination.conditionJson += " )";
                        }
                        pagination.conditionJson += string.Format(") and isover='0' and issaved='1')");
                    }
                    if (!queryParam["examinetype"].IsEmpty())
                    {
                        pagination.conditionJson += " and examinetype='" + queryParam["examinetype"].ToString() + "'";
                    }
                    if (!queryParam["examinecontent"].IsEmpty())
                    {
                        pagination.conditionJson += " and examinecontent like '%" + queryParam["examinecontent"].ToString() + "%'";
                    }
                    if (!queryParam["examinetodeptid"].IsEmpty())
                    {
                        pagination.conditionJson += " and examinetodeptid ='" + queryParam["examinetodeptid"].ToString() + "'";
                    }
                    //开始时间
                    if (!queryParam["sTime"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                    }
                    //结束时间
                    if (!queryParam["eTime"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    ////时间范围
                    //if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
                    //{
                    //    string startTime = queryParam["sTime"].ToString();
                    //    string endTime = queryParam["eTime"].ToString();
                    //    if (queryParam["sTime"].IsEmpty())
                    //    {
                    //        startTime = "1899-01-01";
                    //    }
                    //    if (queryParam["eTime"].IsEmpty())
                    //    {
                    //        endTime = DateTime.Now.ToString("yyyy-MM-dd");
                    //    }
                    //    pagination.conditionJson += string.Format(" and to_date(to_char(examinetime,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                    //}
                    if (!queryParam["contractid"].IsEmpty())
                    {
                        pagination.conditionJson += " and contractid='" + queryParam["contractid"].ToString() + "'";
                    }
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// 日常考核汇总
        /// </summary>
        /// <param name="pagination">查询语句</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>

        public DataTable GetExamineCollent(Pagination pagination, string queryJson)
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                string strWhere = string.Empty;
                DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
           
                dt.Columns.Add("level", typeof(Int32));
                dt.Columns.Add("parent", typeof(string));
                dt.Columns.Add("isLeaf", typeof(bool));

                DataTable CloneDt = dt.Clone();
                foreach (DataRow item in dt.Rows)
                {
                    item["level"] = 0;
                    item["parent"] = null;

                    if (!string.IsNullOrEmpty(queryJson))
                    {
                        var queryParam = queryJson.ToJObject();
                        if (!queryParam["examinetodeptid"].IsEmpty())
                        {
                            strWhere += " and t.examinetodeptid ='" + queryParam["examinetodeptid"].ToString() + "'";
                        }
                        if (!queryParam["examinetype"].IsEmpty())
                        {
                            strWhere += " and t.examinetype='" + queryParam["examinetype"].ToString() + "'";
                        }
                        if (!queryParam["examinecontent"].IsEmpty())
                        {
                            strWhere += " and t.examinecontent like '%" + queryParam["examinecontent"].ToString() + "%'";
                        }
                        //开始时间
                        if (!queryParam["sTime"].IsEmpty())
                        {
                            strWhere += string.Format(@" and t.examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                        }
                        //结束时间
                        if (!queryParam["eTime"].IsEmpty())
                        {
                            strWhere += string.Format(@" and t.examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                        }
                    }
                    string subSql = string.Format(@"  select id,examinetodeptid,
                                           examinetodept,
                                           examineperson,
                                           to_char(examinetime,'yyyy-MM-dd') examinetime,
                                           examinemoney,
                                           examinetype
                                      from epg_dailyexamine t 
                                            where  t.examinetodeptid='{0}' {1}", item["examinetodeptid"].ToString(),strWhere);
                   
                    DataTable itemDt = this.BaseRepository().FindTable(subSql);
                    if (itemDt.Rows.Count > 1)
                    {
                        item["isLeaf"] = false;
                    }
                    else {
                        item["isLeaf"] = true;
                    }
                    DataRow crow = CloneDt.NewRow();
                    crow["examinetodeptid"] = item["examinetodeptid"];
                    crow["examinetodept"] = item["examinetodept"];
                    crow["examinetype"] = item["examinetype"];
                    crow["examinemoney"] = item["examinemoney"];
                    crow["examineperson"] = item["examineperson"];
                    crow["examinetime"] = item["examinetime"];
                    crow["level"] = item["level"];
                    crow["parent"] = item["parent"];
                    crow["isLeaf"] = item["isLeaf"];
                    crow["id"] = item["id"];
                    CloneDt.Rows.Add(crow);
                    itemDt.Columns.Add("level", typeof(Int32));
                    itemDt.Columns.Add("parent", typeof(string));
                    itemDt.Columns.Add("isLeaf", typeof(bool));
                    foreach (DataRow itRow in itemDt.Rows)
                    {
                        bool flag = false;
                        foreach (DataRow clrow in CloneDt.Rows)
                        {
                            if (clrow["id"].ToString() == itRow["id"].ToString()) {
                                flag = true;
                                break;
                            }
                        }
                        if (flag) {
                            continue;
                        }
                        itRow["level"] = 1;
                        itRow["parent"] = item["id"];
                        itRow["isLeaf"] = true;
                        DataRow row = CloneDt.NewRow();
                        row["examinetodeptid"] = itRow["examinetodeptid"];
                        row["examinetodept"] = itRow["examinetodept"];
                        row["examinetype"] = itRow["examinetype"];
                        row["examinemoney"] = itRow["examinemoney"];
                        row["examineperson"] = itRow["examineperson"];
                        row["examinetime"] = itRow["examinetime"];
                        row["level"] = itRow["level"];
                        row["parent"] = itRow["parent"];
                        row["isLeaf"] = itRow["isLeaf"];
                        row["id"] = itRow["id"];
                        CloneDt.Rows.Add(row);
                    }
           
                }

                return CloneDt;
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// 导出使用
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetExportExamineCollent(Pagination pagination, string queryJson)
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                
                DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
                string strWhere = string.Empty;
                dt.Columns.Add("level", typeof(Int32));
                dt.Columns.Add("parent", typeof(string));
                dt.Columns.Add("isLeaf", typeof(bool));

                DataTable CloneDt = dt.Clone();
                foreach (DataRow item in dt.Rows)
                {
                    item["level"] = 0;
                    item["parent"] = null;

                    if (!string.IsNullOrEmpty(queryJson))
                    {
                        var queryParam = queryJson.ToJObject();
                        if (!queryParam["examinetodeptid"].IsEmpty())
                        {
                            strWhere += " and t.examinetodeptid ='" + queryParam["examinetodeptid"].ToString() + "'";
                        }
                        if (!queryParam["examinetype"].IsEmpty())
                        {
                            strWhere += " and t.examinetype='" + queryParam["examinetype"].ToString() + "'";
                        }
                        if (!queryParam["examinecontent"].IsEmpty())
                        {
                            strWhere += " and t.examinecontent like '%" + queryParam["examinecontent"].ToString() + "%'";
                        }
                        //开始时间
                        if (!queryParam["sTime"].IsEmpty())
                        {
                            strWhere += string.Format(@" and t.examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                        }
                        //结束时间
                        if (!queryParam["eTime"].IsEmpty())
                        {
                            strWhere += string.Format(@" and t.examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                        }
                    }
                    string subSql = string.Format(@"  select id,examinetodeptid,
                                           examinetodept,
                                           examineperson,
                                           to_char(examinetime,'yyyy-MM-dd') examinetime,
                                           examinemoney,
                                           examinetype
                                      from epg_dailyexamine t 
                                            where  t.examinetodeptid='{0}' {1}", item["examinetodeptid"].ToString(), strWhere);
                    DataTable itemDt = this.BaseRepository().FindTable(subSql);
                    if (itemDt.Rows.Count > 1)
                    {
                        item["isLeaf"] = false;
                    }
                    else
                    {
                        item["isLeaf"] = true;
                    }
                    DataRow crow = CloneDt.NewRow();
                    crow["examinetodeptid"] = item["examinetodeptid"];
                    crow["examinetodept"] = item["examinetodept"];
                    crow["examinetype"] = item["examinetype"];
                    crow["examinemoney"] = item["examinemoney"];
                    crow["examineperson"] = item["examineperson"];
                    crow["examinetime"] = item["examinetime"];
                    crow["level"] = item["level"];
                    crow["parent"] = item["parent"];
                    crow["isLeaf"] = item["isLeaf"];
                    crow["id"] = item["id"];
                  
                    itemDt.Columns.Add("level", typeof(Int32));
                    itemDt.Columns.Add("parent", typeof(string));
                    itemDt.Columns.Add("isLeaf", typeof(bool));
                    foreach (DataRow itRow in itemDt.Rows)
                    {
                        bool flag = false;
                        foreach (DataRow clrow in dt.Rows)
                        {
                            if (clrow["id"].ToString() == itRow["id"].ToString())
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            continue;
                        }
                        itRow["level"] = 1;
                        itRow["parent"] = item["id"];
                        itRow["isLeaf"] = true;
                        DataRow row = CloneDt.NewRow();
                        row["examinetodeptid"] = itRow["examinetodeptid"];
                        row["examinetodept"] = itRow["examinetodept"];
                        row["examinetype"] = itRow["examinetype"];
                        row["examinemoney"] = itRow["examinemoney"];
                        row["examineperson"] = itRow["examineperson"];
                        row["examinetime"] = itRow["examinetime"];
                        row["level"] = itRow["level"];
                        row["parent"] = itRow["parent"];
                        row["isLeaf"] = itRow["isLeaf"];
                        row["id"] = itRow["id"];
                        CloneDt.Rows.Add(row);
                    }
                    CloneDt.Rows.Add(crow);
                }

                return CloneDt;
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// 首页提醒
        /// </summary>
        /// <returns></returns>
        public int CountIndex(ERCHTMS.Code.Operator currUser)
        {
            int num = 0;
            string sqlwhere = "";

            sqlwhere += string.Format(" and ((flowdept ='{0}'", currUser.DeptId);

            string[] arr = currUser.RoleName.Split(',');
            if (arr.Length > 0)
            {
                sqlwhere += " and (";
                foreach (var item in arr)
                {
                    sqlwhere += string.Format(" flowrolename  like '%{0}%' or", item);
                }
                sqlwhere = sqlwhere.Substring(0, sqlwhere.Length - 2);
                sqlwhere += " )";
            }
            sqlwhere += string.Format(") and isover='0' and issaved='1')");
            string sql = string.Format("select count(1) from epg_dailyexamine where  createuserorgcode='{0}' {1}", currUser.OrganizeCode, sqlwhere);
            object obj = this.BaseRepository().FindObject(sql);
            int.TryParse(obj.ToString(), out num);

            return num;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DailyexamineEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DailyexamineEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>返回列表</returns>
        public string GetMaxCode()
        {
            var entity = this.BaseRepository().FindList(" select max(ExamineCode) as ExamineCode from epg_DailyExamine").FirstOrDefault();
            if (entity == null || entity.ExamineCode == null)
                return DateTime.Now.ToString("yyyyMMdd") + "0001";
            return (Int64.Parse(entity.ExamineCode) + 1).ToString();
        }

        #region  当前登录人是否有权限审核并获取下一次审核权限实体
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);

            if (powerList.Count > 0)
            {
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(createdeptid).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(createdeptid).DepartmentId;
                    }
                    if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                    {
                        var createdeptentity = new DepartmentService().GetEntity(createdeptid);
                        while (createdeptentity.Nature == "专业" || createdeptentity.Nature == "班组")
                        {
                            createdeptentity = new DepartmentService().GetEntity(createdeptentity.ParentId);
                        }
                        powerList[i].CHECKDEPTCODE = createdeptentity.EnCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
                    }
                }
                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                //登录人是否有审核权限--有审核权限直接审核通过
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTID == currUser.DeptId)
                    {
                        var rolelist = currUser.RoleName.Split(',');
                        for (int j = 0; j < rolelist.Length; j++)
                        {
                            if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                            {
                                checkPower.Add(powerList[i]);
                                break;
                            }
                        }
                    }
                }
                powerList.GroupBy(t => t.SERIALNUM).ToList().Count();
                if (checkPower.Count > 0)
                {
                    state = "1";
                    ManyPowerCheckEntity check = checkPower.Last();//当前

                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (check.ID == powerList[i].ID)
                        {
                            if ((i + 1) >= powerList.Count)
                            {
                                nextCheck = null;
                            }
                            else
                            {
                                nextCheck = powerList[i + 1];
                            }
                        }
                    }
                }
                else
                {
                    state = "0";
                    nextCheck = powerList.First();
                }

                if (null != nextCheck)
                {
                    //当前审核序号下的对应集合
                    var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                    //集合记录大于1，则表示存在并行审核（审查）的情况
                    if (serialList.Count() > 1)
                    {
                        string flowdept = string.Empty;  // 存取值形式 a1,a2
                        string flowdeptname = string.Empty; // 存取值形式 b1,b2
                        string flowrole = string.Empty;   // 存取值形式 c1|c2|  (c1数据构成： cc1,cc2,cc3)
                        string flowrolename = string.Empty; // 存取值形式 d1|d2| (d1数据构成： dd1,dd2,dd3)

                        ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                        slastEntity = serialList.LastOrDefault();
                        foreach (ManyPowerCheckEntity model in serialList)
                        {
                            flowdept += model.CHECKDEPTID + ",";
                            flowdeptname += model.CHECKDEPTNAME + ",";
                            flowrole += model.CHECKROLEID + "|";
                            flowrolename += model.CHECKROLENAME + "|";
                        }
                        if (!flowdept.IsEmpty())
                        {
                            slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                        }
                        if (!flowdeptname.IsEmpty())
                        {
                            slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                        }
                        if (!flowdept.IsEmpty())
                        {
                            slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                        }
                        if (!flowdept.IsEmpty())
                        {
                            slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                        }
                        nextCheck = slastEntity;
                    }
                }
                return nextCheck;
            }
            else
            {
                state = "0";
                return nextCheck;
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DailyexamineEntity entity)
        {
            entity.Id = keyValue;
            //开始事务
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    DailyexamineEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        this.BaseRepository().Insert(entity);


                    }
                    else
                    {
                        entity.Modify(keyValue);
                        this.BaseRepository().Update(entity);
                    }
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
