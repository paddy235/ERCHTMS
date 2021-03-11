using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System.Data.Common;
using System.Text;
using System;
using ERCHTMS.Service.SystemManage;
namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：辨识评估计划表
    /// </summary>
    public class RiskPlanService : RepositoryFactory<RiskPlanEntity>, RiskPlanIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskPlanEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
  
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            if (!queryParam["planName"].IsEmpty())
            {
                string planName = queryParam["planName"].ToString();
                pagination.conditionJson += string.Format(" and planName like '%{0}%'", planName);
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;

        }
        /// <summary>
        ///根据辨识计划ID获取相关风险点的各状态数量信息
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="startDate">计划开始时间</param>
        /// <param name="endDate">计划结束时间</param>
        /// <returns>依次为原有风险数量，新增风险数量，完善风险数量，删除风险数量和现有风险数量</returns>
        public List<int> GetNumbers(string planId, string startDate, string endDate, int status, string areaId)   
        {
            DataItemDetailService detailservice = new DataItemDetailService();
            string gxhs = detailservice.GetItemValue("广西华昇版本");
            string tableName = status == 0 ? "BIS_RISKASSESS" : "BIS_RISKHISTORY";
            List<int> list = new List<int>();
            //原有风险数量
            string sql = string.Format("select risknumbers from BIS_RISKPLAN where  id='{0}'", planId);
            int num1 = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(num1); 
            //新增风险数量
            int num2 = this.BaseRepository().FindObject(string.Format("select count(1) from {1} where planid='{0}' and EnabledMark=0 and deletemark=0 and status=2", planId, tableName)).ToInt();
            list.Add(num2);
            //完善风险数量
            int num3 = this.BaseRepository().FindObject(string.Format("select count(1) from {1} where planid='{0}' and state=1 and status=1", planId, tableName)).ToInt();
            list.Add(num3);
            //消除风险数量
            int num4 = this.BaseRepository().FindObject(string.Format("select count(1) from {1} where planid='{0}' and status=1 and state=2", planId, tableName)).ToInt();
            list.Add(num4);
            //现有风险数量
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                sql = string.Format("select count(1) from {1} where  deletemark=0 and createdate<=to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and deptcode in (select DEPTCODE from bis_riskpplandata t where t.planid ='{2}') ", DateTime.Parse(endDate.ToString()).ToString("yyyy-MM-dd 23:59:59"), tableName, planId);
                if (status == 1)
                {
                    sql = string.Format("select count(1) from {1} where  deletemark=0 and createdate<=to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and newplanid='{2}'", DateTime.Parse(endDate.ToString()).ToString("yyyy-MM-dd 23:59:59"), tableName, planId);
                }
                num4 = this.BaseRepository().FindObject(sql).ToInt();
                list.Add(num4);
            }
            else
            {
                sql = string.Format("select count(1) from {1} where  deletemark=0 and createdate<=to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and districtid in('{2}')", DateTime.Parse(endDate.ToString()).ToString("yyyy-MM-dd 23:59:59"), tableName, areaId.Replace(",", "','"));
                if (DbHelper.DbType == DatabaseType.MySql)
                {
                    sql = string.Format("select count(1) from {1} where  deletemark=0 and createdate<='{0}'  and districtid in('{2}')", DateTime.Parse(endDate.ToString()).ToString("yyyy-MM-dd 23:59:59"), tableName, areaId.Replace(",", "','"));
                }
                if (status == 1)
                {
                    sql = string.Format("select count(1) from {1} where  deletemark=0 and createdate<=to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and newplanid='{2}'", DateTime.Parse(endDate.ToString()).ToString("yyyy-MM-dd 23:59:59"), tableName, planId);
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql = string.Format("select count(1) from {1} where  deletemark=0 and createdate<='{0}' and newplanid='{2}'", DateTime.Parse(endDate.ToString()).ToString("yyyy-MM-dd 23:59:59"), tableName, planId);
                    }
                }
                num4 = this.BaseRepository().FindObject(sql).ToInt();
                list.Add(num4);
            }
            
            //辨识部门、评估部门原有风险数量
            sql = string.Format("select count(1) from BIS_RISKASSESS where createdate<to_date('{0}','yyyy-mm-dd hh24:mi:ss') and status=1 and deletemark=0 and EnabledMark=0 and deptcode in (select DEPTCODE from bis_riskpplandata t where t.planid ='{1}') ", DateTime.Parse(startDate).ToString("yyyy-MM-dd 00:00:01"), planId);
            int num5= this.BaseRepository().FindObject(sql).ToInt();
            list.Add(num5);
            return list;

        }
        /// <summary>
        /// 获取计划原有风险数量
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="areaIds">计划关联区域</param>
        /// <param name="startDate">计划开始时间</param>
        /// <returns></returns>
        public int GetRiskNumbers(string areaIds,string startDate,string planId)
        {
            DataItemDetailService detailservice = new DataItemDetailService();
            string gxhs = detailservice.GetItemValue("广西华昇版本");
            string sql = "";
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS where createdate<to_date('{0}','yyyy-mm-dd hh24:mi:ss') and status=1 and deletemark=0 and EnabledMark=0 and deptcode in (select DEPTCODE from bis_riskpplandata t where t.planid='{1}' )", DateTime.Parse(startDate).ToString("yyyy-MM-dd 00:00:01"), planId);
            }
            else
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS where createdate<to_date('{0}','yyyy-mm-dd hh24:mi:ss') and status=1 and deletemark=0 and EnabledMark=0 and districtid in('{1}')", DateTime.Parse(startDate).ToString("yyyy-MM-dd 00:00:01"), areaIds.Replace(",", "','"));
            }
            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS where createdate<'{0}' and status=1 and deletemark=0 and EnabledMark=0 and districtid in('{1}')", DateTime.Parse(startDate).ToString("yyyy-MM-dd 00:00:01"), areaIds.Replace(",", "','"));
            }
            return this.BaseRepository().FindObject(sql).ToInt();

        }
        /// <summary>
        ///根据辨识计划ID获取辨识和评估人员（多个人员账号用逗号分隔,辨识与评估人员间用|分割）
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="dataType">数据类型，0：获取辨识人员，1：获取评估人员</param>
        /// <returns>人员账号，多个用英文逗号分隔</returns>
        public string GetUsers(string planId,int dataType=-1)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            string sql = string.Format("select userid,datatype from BIS_RISKPPLANDATA where planid='{0}'", planId);
            if (dataType>=0)
            {
                sql += " and datatype="+dataType;
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            foreach(DataRow dr in dt.Rows)
            {
                //计划中设置的辨识人员
                if(dr[1].ToString()=="0")
                {
                     sb.Append(dr[0].ToString()+",");
                }
                //计划中设置的评估人员
                if (dr[1].ToString() == "1")
                {
                    sb1.Append(dr[0].ToString() + ",");
                }
            }
            dt.Dispose();
            return sb.ToString().TrimEnd(',')+"|"+sb1.ToString().TrimEnd(',');

        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskPlanEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 首页待办事项获取辨识评估计划
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode">10:待辨识的，0:待辨识或评估的,1:部门所有待辨识或评估的</param>
        /// <returns></returns>
        public int GetPlanCount(ERCHTMS.Code.Operator user, int mode)
        {
            int count = 0;
            string sql = string.Format("select distinct planid from BIS_RISKPPLANDATA a left join BIS_RISKPLAN b on a.planid=b.id where b.STATUS=0 and (a.userid='{0}' or b.createuserid='{1}')", user.Account, user.UserId);
            DataTable dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append(dr[0].ToString() + ",");
                }
                sql = "select count(1) from BIS_RISKPLAN a  where status=0 ";
                //当前用户待辨识的计划  
                if (mode ==10)
                {
                    sql += string.Format(" and id in(select planid from BIS_RISKPPLANDATA where  planid in('{0}') and datatype=0 and userid='{1}') ", sb.ToString().TrimEnd(',').Replace(",", "','"), user.Account);
                    count = this.BaseRepository().FindObject(sql).ToInt();
                }
                //当前用户待辨识或评估的计划  
                else if (mode == 0)
                {
                    sql += string.Format(" and (id in('{0}') or createuserid='{1}')", sb.ToString().TrimEnd(',').Replace(",", "','"), user.UserId);
                    count = this.BaseRepository().FindObject(sql).ToInt();
                }
                //当前用户所在部门待辨识或评估的计划
                else
                {
                    sql += string.Format(" and (',' || userids || ',' like '{0}' or createuserid='{1}' or  deptcode='{2}'", user.Account, user.UserId, user.DeptCode);
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql += string.Format(" and (CONCAT(',',userids,',') like '{0}' or createuserid='{1}' or  deptcode='{2}'", user.Account, user.UserId, user.DeptCode);
                    }
                    sql += string.Format(" or id in(select planid from BIS_RISKPPLANDATA where deptcode='{0}' and status=0))", user.DeptCode);

                    count = this.BaseRepository().FindObject(sql).ToInt();
                }
            }
            return count;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public int RemoveForm(string keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, RiskPlanEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                return this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                return this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 设置计划完成状态并同步相关联风险记录到历史记录表
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="areaIds">区域ID(多个用逗号分隔)</param>
        /// <returns></returns>
        public bool SetComplate(string planId,string areaIds)
        {
            this.BaseRepository().BeginTrans();
            DataItemDetailService detailservice = new DataItemDetailService();
            var gxhs = detailservice.GetItemValue("广西华昇版本");
            try
            {
                string sql = string.Format("update BIS_RISKPLAN set status=1,modifydate=sysdate where id='{0}'", planId);
                if (this.BaseRepository().ExecuteBySql(sql) > 0)
                {
                    string id = Guid.NewGuid().ToString();
                    string guid =string.Format("'{0}'",id);
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        guid = "uuid()";
                    }
                    if (!string.IsNullOrWhiteSpace(gxhs)) //广西华昇版本查询管控部门是辨识部门、评估部门的数据
                    {
                        sql = string.Format(@"insert into BIS_RISKHISTORY(
                    id,areaid,areaname,dangersource,way,itema,itemb,itemc,itemr,
                                    grade,deptcode,deptname,postid,postname,createuserid,createdate,
                                    createusername,createuserdeptcode,createuserorgcode,status,
                                    gradeval,result,accidentname,harmtype,risktype,DeleteMark,
                                    planid,state,districtid,districtname,areacode,newplanid,
                                    WorkTask,Process,EquipmentName,Parts,RiskDesc,ResultType,
                                    measure,majorname,Description,HarmProperty,levelname,faulttype,
                                    jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                    project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                    packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,isconventional,remark,workcontent)
                                    select id || {0},areaid,t.areaname,t.dangersource,way,itema,
                                    itemb,itemc,itemr,grade,deptcode,deptname,postid,postname,
                                    createuserid,createdate,createusername,createuserdeptcode,
                                    createuserorgcode,status,gradeval,result,accidentname,harmtype,
                                    risktype,DeleteMark,planid,state,districtid,districtname,
                                    areacode,'{1}',WorkTask,Process,EquipmentName,Parts,RiskDesc,
                                    ResultType,measure,majorname,Description,HarmProperty,levelname,
                                    faulttype,jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                    project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                    packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,isconventional,remark,workcontent
                                    from bis_riskassess t
where (t.deptcode in(select DEPTCODE from bis_riskpplandata t where t.planid ='{1}' ) and status=1) or (planid='{1}' and status=2) order by id", guid, planId, areaIds.Replace(",", "','"));
                    }
                    else
                    {
                        sql = string.Format(@"insert into BIS_RISKHISTORY(
                    id,areaid,areaname,dangersource,way,itema,itemb,itemc,itemr,
                                    grade,deptcode,deptname,postid,postname,createuserid,createdate,
                                    createusername,createuserdeptcode,createuserorgcode,status,
                                    gradeval,result,accidentname,harmtype,risktype,DeleteMark,
                                    planid,state,districtid,districtname,areacode,newplanid,
                                    WorkTask,Process,EquipmentName,Parts,RiskDesc,ResultType,
                                    measure,majorname,Description,HarmProperty,levelname,faulttype,
                                    jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                    project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                    packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,isconventional,remark)
                                    select id || {0},areaid,t.areaname,t.dangersource,way,itema,
                                    itemb,itemc,itemr,grade,deptcode,deptname,postid,postname,
                                    createuserid,createdate,createusername,createuserdeptcode,
                                    createuserorgcode,status,gradeval,result,accidentname,harmtype,
                                    risktype,DeleteMark,planid,state,districtid,districtname,
                                    areacode,'{1}',WorkTask,Process,EquipmentName,Parts,RiskDesc,
                                    ResultType,measure,majorname,Description,HarmProperty,levelname,
                                    faulttype,jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                    project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                    packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,isconventional,remark
                                    from bis_riskassess t
where (t.districtid in('{2}') and status=1) or (planid='{1}' and status=2) order by id", guid, planId, areaIds.Replace(",", "','"));

                    }

                    this.BaseRepository().ExecuteBySql(sql);
                    //DataTable dt = this.BaseRepository().FindTable(string.Format("select id from bis_riskassess where (districtid in('{1}') and status=1) or (planid='{0}' and status=2) and id in(select riskid from BIS_MEASURES)",planId, areaIds.Replace(",", "','")));
                    //foreach(DataRow dr in dt.Rows)
                    //{
                      //id = Guid.NewGuid().ToString();
                    sql = string.Format("insert into BIS_MEASURES(id,content,riskid,typename) select  '{0}' || rownum,content,riskid || {3},typename from BIS_MEASURES where riskid in(select id from bis_riskassess where (districtid in('{2}') and status=1) or (planid='{1}' and status=2)) order by riskid", id, planId, areaIds.Replace(",", "','"), guid);
                    this.BaseRepository().ExecuteBySql(sql);

                        //sql = string.Format("insert into BIS_MEASURES(id,content,riskid,typename) select '{0}' || rownum,t.content,'{1}',typename from BIS_MEASURES t where t.riskid='{2}'", Guid.NewGuid().ToString(), dr[0].ToString() + id, dr[0].ToString());
                        //this.BaseRepository().ExecuteBySql(sql);
                    //}
                    sql = string.Format("update bis_riskassess set status=1,planId='' where planId='{0}'", planId);
                    this.BaseRepository().ExecuteBySql(sql);
                     
                    this.BaseRepository().Commit();
                }
                return true;
            }
            catch
            {
                this.BaseRepository().Rollback();
                return false;
            }    
           
         
        }
        /// <summary>
        /// 根据用户辨识评估的区域ID（多个用逗号分割）
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="userAccount">账号</param>
        /// <returns></returns>
        public string GetCurrUserAreaId(string planId,string userAccount)
        {
            List<string> list = new List<string>();
            DataTable dt = this.BaseRepository().FindTable(string.Format("select areaid from BIS_RISKPPLANDATA where userid='{0}' and planid='{1}'",userAccount,planId));
            foreach(DataRow dr in dt.Rows)
            {
                string[] arr = dr[0].ToString().Split(',');
                foreach(string str in arr)
                {
                    if(!list.Contains(str))
                    {
                        list.Add(str);
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach(string str in list)
            {
                sb.Append(str+",");
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 获取所有未完成计划相关联的区域ID（多个用逗号分割）
        /// </summary>
        /// <param name="status">状态，0：未完成，1：已完成</param>
        /// <returns></returns>
        public string GetPlanAreaIds(int status=0,string planId="")
        {
            List<string> list = new List<string>();
            string sql = string.Format("select areaid from BIS_RISKPLAN where status={0}", status);
            if (!string.IsNullOrEmpty(planId))
            {
                sql += " and id!='"+planId+"'";
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string[] arr = dr[0].ToString().Split(',');
                foreach (string str in arr)
                {
                    if (!list.Contains(str))
                    {
                        list.Add(str);
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (string str in list)
            {
                sb.Append(str + ",");
            }
            return sb.ToString().TrimEnd(',');
        }
        #endregion
    }
}
