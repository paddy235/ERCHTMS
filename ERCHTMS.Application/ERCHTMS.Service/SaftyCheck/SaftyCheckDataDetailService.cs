using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
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
using ERCHTMS.Code;
using System;
using System.Collections;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表详情
    /// </summary>
    public class SaftyCheckDataDetailService : RepositoryFactory<SaftyCheckDataDetailEntity>, SaftyCheckDataDetailIService
    {

        #region 获取数据
        /// <summary>
        /// 更改登记状态
        /// </summary>
        public void RegisterPer(string userAccount, string id)
        {
            //IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindList("select *from BIS_SAFTYCHECKDATADETAILED where recid='" + id + "' and instr(CHECKMANID,'" + userAccount + "')>0");
            //foreach (SaftyCheckDataDetailEntity item in list)
            //{
            //    item.CheckState = 2;
            //    this.BaseRepository().Update(item);
            //}
            var user = OperatorProvider.Provider.Current();
            this.BaseRepository().ExecuteBySql(string.Format("update BIS_SAFTYCHECKDATADETAILED set CheckState=2,ModifyUserId=ModifyUserId||'" + user.UserId + "|',ModifyUserName=ModifyUserName||'" + user.UserName + ",' where recid='{0}' and instr(CHECKMANID,'{1}')>0", id, userAccount));
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 安全检查的时候点击检查表
        /// </summary>
        public DataTable GetDetails(string ids)
        {
            //返回检查项
            DataTable dt = this.BaseRepository().FindTable("select * from BIS_SAFTYCHECKDATADETAILED where recid in ('" + ids + "')  order by autoid,checkobject,checkobjectid");
            return dt;
        }
        /// <summary>
        /// 获取检查记录检查内容的数量
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public int GetCount(string recId)
        {
            //返回检查项
            return this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATADETAILED t left join bis_saftycontent a on t.id=a.detailid where t.recid='" + recId + "' and a.issure is not null").ToInt();
            
        }

        /// <summary>
        /// 获取检查记录检查内容的数量
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public int GetCheckItemCount(string recId)
        {
            //返回检查项
            return this.BaseRepository().FindObject(string.Format("select count(1) from BIS_SAFTYCHECKDATADETAILED t left join bis_saftycontent a on t.id=a.detailid where t.recid='{0}'", recId)).ToInt();

        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaftyCheckDataDetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 安全检查表列表(系统生成)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageOfSysCreate(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["chargedeptcode"].IsEmpty())
            {
                string chargedeptcode = queryParam["chargedeptcode"].ToString();
                StringBuilder sb = new StringBuilder();
                var d = OperatorProvider.Provider.Current();
                if (d.RoleName != null)
                {
                    if (!d.RoleName.Contains("厂级"))
                        sb.Append(" and instr(t.deptcode,'" + chargedeptcode + "')>0");
                    else
                        sb.Append(" and instr(t.deptcode,'" + chargedeptcode.Substring(0, 3) + "')>0");
                }
                pagination.conditionJson += sb.ToString();
            }
            pagination.conditionJson += string.Format(" and  status>0  order by AREACODE");
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }


        /// <summary>
        /// 安全检查表列表(系统生成)
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        
        public DataTable GetListOfSysCreate(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string sql = "select status,ID,DISTRICTNAME,DANGERSOURCE,DISTRICTID,AreaCode from BIS_RISKASSESS t where 1=1";
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["chargedeptcode"].IsEmpty())
            {
                string chargedeptcode = queryParam["chargedeptcode"].ToString();
                StringBuilder sb = new StringBuilder();
                var d = OperatorProvider.Provider.Current();
                if (d.RoleName != null)
                {
                    if (!d.RoleName.Contains("厂级"))
                        sb.Append(string.Format(" and  t.deptcode like '{0}%'", chargedeptcode));
                    else
                        sb.Append(string.Format(" and  t.deptcode like '{0}%'", chargedeptcode.Substring(0, 3)));
                }
                sql += sb.ToString();
            }
            sql += string.Format(" and  status=1  and deleteMark='0' order by AREACODE");
            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// 获取 检查内容
        /// </summary>
        /// <param name="baseID">风险点ID</param>
        public DataTable GetPageContent(string baseID)
        {
            DataTable dt = this.BaseRepository().FindTable("select content,riskid from BIS_MEASURES ");
            return dt;
        }
        /// <summary>
        /// 安全检查表列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //选择的类型
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (!type.Contains("all"))
                {
                    pagination.conditionJson += string.Format(" and t.recid in (select id from BIS_SAFTYCHECKDATA where CHECKDATATYPE in ('{0}') and t.CHECKDATATYPE is not null)", type);
                }
            }
            //搜索关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.CheckContent like '%{0}%'", keyword);
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department where encode like '{0}%')", queryParam["code"].ToString());
            }
            //选取已经检查过的
            //if (!queryParam["newstate"].IsEmpty())
            //{
            //    string state = queryParam["newstate"].ToString();
            //    pagination.conditionJson += string.Format(" and t.CHECKSTATE='{0}'", state);
            //}
            //属于自己的检查
            if (!queryParam["userAccount"].IsEmpty())
            {
                string userAccount = queryParam["userAccount"].ToString();
                string recid = queryParam["recid"].ToString();
                if (OperatorProvider.Provider.Current().IsSystem == false)
                {
                    if (!queryParam["pFrom"].IsEmpty())
                    {
                        string pfrom=queryParam["pFrom"].ToString();
                        if (pfrom=="1")
                        {
                            pagination.conditionJson += string.Format(" and instr(',' || t.CHECKMANID || ',',',{0},')>0 ", userAccount);
                        }
                    }
                    else{
                        pagination.conditionJson += string.Format(" and instr(',' || t.CHECKMANID || ',',',{0},')>0 and id not in(select detailid from BIS_SAFTYCONTENT where recid='{1}')", userAccount, recid);
                    }
                   
                }
                   
            }
            //关联检查表
            if (!queryParam["recid"].IsEmpty())
            {
                string recid = queryParam["recid"].ToString();
                pagination.conditionJson += string.Format(" and t.recid='{0}'", recid);
            }
            else pagination.conditionJson += string.Format(" and t.recid='{0}'", "");
            if(string.IsNullOrEmpty(pagination.sidx))
            {
                pagination.conditionJson += string.Format(" order by autoid,CheckObject");
            }
            Hashtable ht = new Hashtable();
            int count = 0, WzCount = 0,WtCount = 0;
            IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType).ToList();
            list=list.Select(r =>
            {
                
                //得到检查内容和检查人员
                if (!queryParam["userAccount"].IsEmpty())
                {
                    if (OperatorProvider.Provider.Current().IsSystem == false)
                    {
                        DataTable dtContent = this.BaseRepository().FindTable("select CheckContent,CheckManName,CheckManAccount,t.CheckObject,t.CheckObjectId,t.CheckObjectType,b.issure,b.remark from BIS_SAFTYCHECKDATADETAILED t left join BIS_SAFTYCONTENT b on t.id=b.detailid where instr(',' || checkmanid || ',','," + queryParam["userAccount"].ToString() + ",')>0 and t.recid='" + r.RecID + "' and t.id='" + r.ID + "'");
                        string SaftyContent = "";
                        string CheckManName = "";
                        string CheckManAccount = "";
                        string checkobj = "";
                        string checkobjid = "";
                        string checkobjtype = "";
                        foreach (DataRow item in dtContent.Rows)
                        {
                            SaftyContent= item[0].ToString();
                            CheckManName += item[1].ToString() + ",";
                            CheckManAccount += item[2].ToString() + ",";
                            checkobj = item[3].ToString();
                            checkobjid= item[4].ToString();
                            checkobjtype= item[5].ToString();
                        }
                        r.CheckContent = SaftyContent;
                        r.CheckMan = CheckManName.TrimEnd(',');
                        r.CheckManID = CheckManAccount.TrimEnd(',');
                        r.CheckObject = checkobj;
                        //r.CheckObjectId = checkobjid;
                        r.CheckObjectType = checkobjtype;
                        r.IsSure = dtContent.Rows.Count > 0 ? dtContent.Rows[0]["IsSure"].ToString(): "";
                        r.Remark = dtContent.Rows.Count > 0 ? dtContent.Rows[0]["Remark"].ToString() : "";
                    }
                }
                if (!queryParam["pMode"].IsEmpty())
                {
                    if (queryParam["pMode"].ToString()=="0")
                    {
                        DataTable dtContent = this.BaseRepository().FindTable("select t.checkcontent,CHECKMANNAME,t.CheckObject,b.remark,b.issure,saftycontent from BIS_SAFTYCHECKDATADETAILED t left join BIS_SAFTYCONTENT b on t.id=b.detailid where t.recid='" + r.RecID + "' and t.ID='" + r.ID + "'");
                        string SaftyContent = "";
                        string CheckManName = "";
                        string CheckManAccount = "";
                        string checkobj = "";
                        string checkobjid = "";
                        string checkobjtype = "";
                        foreach (DataRow item in dtContent.Rows)
                        {
                            if (string.IsNullOrEmpty(item["saftycontent"].ToString()))
                            {
                                SaftyContent= item["checkcontent"].ToString();
                            }
                            else
                            {
                               SaftyContent = item["saftycontent"].ToString();
                            }
                            if (!CheckManName.Contains(item[1].ToString() + ","))
                            {
                                CheckManName += item[1].ToString() + ",";
                            }
                            checkobj = item[2].ToString();
                        }
                        r.CheckContent = SaftyContent;
                        r.CheckMan = CheckManName.TrimEnd(',');
                        //r.CheckManID = CheckManAccount.TrimEnd('|');
                        r.CheckObject = checkobj;
                        //r.CheckObjectId = checkobjid.TrimEnd('|');
                        //r.CheckObjectType = checkobjtype.TrimEnd('|');
                       
                        r.IsSure = dtContent.Rows.Count > 0 ? dtContent.Rows[0]["IsSure"].ToString():"";
                        r.Remark = dtContent.Rows.Count > 0 ? dtContent.Rows[0]["Remark"].ToString() : "";
                    }
                }
                if (!queryParam["allcount"].IsEmpty())
                {
                    string i = queryParam["allcount"].ToString();
                    if (i == "1")
                    {
                        //得到该区域所有的隐患数量                        
                        if (!ht.ContainsKey(r.CheckObjectId))
                        {
                            count = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + queryParam["recid"].ToString() + "' and relevanceid in(select id from bis_saftycheckdatadetailed where checkobjectid='" + r.CheckObjectId + "')").ToInt();                            
                            WzCount = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + queryParam["recid"].ToString() + "' and resevertwo in(select id from bis_saftycheckdatadetailed where checkobjectid='" + r.CheckObjectId + "')").ToInt();
                            WtCount = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where  o.CHECKID='" + queryParam["recid"].ToString() + "' and RELEVANCEID='" + r.CheckObjectId + "'").ToInt();     
                            ht.Add(r.CheckObjectId, count);
                        }
                        r.Count = count;
                        r.WzCount = WzCount.ToString();
                        r.WtCount = WtCount.ToString();
                        int countHt = 0, countWz = 0;
                        if (count > 0) 
                            countHt = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + queryParam["recid"].ToString() + "' and relevanceid='" + r.ID + "'").ToInt();
                        if(WzCount>0)
                            countWz = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + queryParam["recid"].ToString() + "' and resevertwo='" + r.ID + "'").ToInt();
                        if (string.IsNullOrWhiteSpace(r.IsSure) && (countHt > 0 || countWz > 0))
                        {
                            r.IsSure = "0";
                        }
                    }
                }  
                else
                {
                    //计算所含隐患个数
                    int count1 = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + queryParam["recid"].ToString() + "' and  relevanceid in(select id from bis_saftycheckdatadetailed where checkobjectid='" + r.CheckObjectId + "')").ToInt();
                    r.Count = count1;
                    //违章数量
                    count1 = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + queryParam["recid"].ToString() + "' and resevertwo in(select id from bis_saftycheckdatadetailed where checkobjectid='" + r.CheckObjectId + "')").ToInt();
                    r.WzCount = count1.ToString();
                    //问题数量
                    count1 = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where  o.CHECKID='" + queryParam["recid"].ToString() + "' and RELEVANCEID='" + r.CheckObjectId + "'").ToInt();
                    r.WtCount = count1.ToString();

                    object obj = BaseRepository().FindObject(string.Format("select issure from BIS_SAFTYCONTENT t where t.detailid='{0}' and t.recid='{1}'",r.ID,r.RecID));
                    if (obj==null)
                    {
                        r.IsSure = "";
                    }
                    else
                    {
                        r.IsSure = obj.ToString();
                    }
                }
                return r;
            });

            return list;

        }
        public DataTable GetDataTableList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //选择的类型
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (!type.Contains("all"))
                {
                    pagination.conditionJson += string.Format(" and t.recid in (select id from BIS_SAFTYCHECKDATA where CHECKDATATYPE in ('{0}') and t.CHECKDATATYPE is not null)", type);
                }
            }
            //搜索关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.CheckContent like '%{0}%'", keyword);
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department where encode like '{0}%')", queryParam["code"].ToString());
            }
            //选取已经检查过的
            //if (!queryParam["newstate"].IsEmpty())
            //{
            //    string state = queryParam["newstate"].ToString();
            //    pagination.conditionJson += string.Format(" and t.CHECKSTATE='{0}'", state);
            //}
            //属于自己的检查
            if (!queryParam["userAccount"].IsEmpty())
            {
                string userAccount = queryParam["userAccount"].ToString();
                string recid = queryParam["recid"].ToString();
                if (OperatorProvider.Provider.Current().IsSystem == false)
                    pagination.conditionJson += string.Format(" and instr(',' || t.CHECKMANID || ',',',{0},')>0 and t.id not in(select detailid from BIS_SAFTYCONTENT where recid='{1}')", userAccount, recid);
            }
            //关联检查表
            if (!queryParam["recid"].IsEmpty())
            {
                string recid = queryParam["recid"].ToString();
                pagination.conditionJson += string.Format(" and t.recid='{0}'", recid);
            }
            else pagination.conditionJson += string.Format(" and t.recid='{0}'", "");
            if (string.IsNullOrEmpty(pagination.sidx))
            {
                pagination.sidx += string.Format("autoid,t.CheckObject");
            }
            Hashtable ht = new Hashtable();
            int count = 0, WtCount = 0, WzCount = 0;
            DataTable  dt= this.BaseRepository().FindTableByProcPager(pagination, dataType);
            if (dt.Rows.Count == 0 && !queryParam["recid"].IsEmpty() && !queryParam["isDay"].IsEmpty())
            {
                string recId = queryParam["recid"].ToString();
                DataRow dr = dt.NewRow();
                count = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + recId + "'").ToInt();
                WzCount = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + recId + "'").ToInt();
                WtCount = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where  o.CHECKID='" + recId + "'").ToInt();
                dr["count"] = count;
                dr["pkid"] = recId;
                dr["name"] = "";
                dr["wzcount"] = WzCount;
                dr["wtcount"] = WtCount;
                dr["stid"] = recId;

                decimal count1 = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where workstream='整改结束' and  o.safetycheckobjectid='" + recId + "'").ToInt();
                if (count > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / count).ToString()), 2);
                    dr["Count1"] = count1 * 100;
                }
                count1 = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where flowstate='流程结束' and  o.reseverone='" + recId + "'").ToInt();
                if (WzCount > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / WzCount).ToString()), 2);
                    dr["WzCount1"] = count1 * 100;
                }
                count1 = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where flowstate='流程结束' and  o.CHECKID='" + recId + "'").ToInt();
                if (WtCount > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / WtCount).ToString()), 2);
                    dr["WtCount1"] = count1 * 100;
                }
                dt.Rows.Add(dr);
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!queryParam["allcount"].IsEmpty())
                    {
                        string i = queryParam["allcount"].ToString();
                        if (i == "1")
                        {
                            //得到该区域所有的隐患数量                        
                            if (!ht.ContainsKey(dr["stid"].ToString()))
                            {
                                count = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + queryParam["recid"].ToString() + "' and relevanceid in(select id from bis_saftycheckdatadetailed where checkobjectid='" + dr["stid"].ToString() + "')").ToInt();
                                WzCount = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + queryParam["recid"].ToString() + "' and resevertwo in(select id from bis_saftycheckdatadetailed where checkobjectid='" + dr["stid"].ToString() + "')").ToInt();
                                WtCount = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where  o.CHECKID='" + queryParam["recid"].ToString() + "' and RELEVANCEID='" + dr["stid"].ToString() + "'").ToInt();
                                ht.Add(dr["stid"].ToString(), count);
                            }
                            dr["count"] = count;
                            dr["wzCount"] = WzCount.ToString();
                            dr["wtCount"] = WtCount.ToString();
                            int countHt = 0, countWz = 0, countWt = 0;
                            if (count > 0)
                                countHt = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + queryParam["recid"].ToString() + "' and relevanceid='" + dr["pkid"].ToString() + "'").ToInt();
                            if (WzCount > 0)
                                countWz = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + queryParam["recid"].ToString() + "' and resevertwo='" + dr["pkid"].ToString() + "'").ToInt();

                            if (countWt > 0)
                                countWt = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where  o.CHECKID='" + queryParam["recid"].ToString() + "' and CORRELATIONID='" + dr["pkid"].ToString() + "'").ToInt();
                            if (string.IsNullOrWhiteSpace(dr["issure"].ToString()) && (countHt > 0 || countWz > 0 || countWt > 0))
                            {
                                dr["issure"] = "0";
                            }

                            decimal count1 = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where workstream='整改结束' and  o.safetycheckobjectid='" + queryParam["recid"].ToString() + "'").ToInt();
                            if (count > 0)
                            {
                                count1 = Math.Round(decimal.Parse((count1 / count).ToString()), 2);
                                dr["Count1"] = count1 * 100;
                            }
                            count1 = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where flowstate='流程结束' and  o.reseverone='" + queryParam["recid"].ToString() + "'").ToInt();
                            if (WzCount > 0)
                            {
                                count1 = Math.Round(decimal.Parse((count1 / WzCount).ToString()), 2);
                                dr["WzCount1"] = count1 * 100;
                            }
                            count1 = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where flowstate='流程结束' and  o.CHECKID='" + queryParam["recid"].ToString() + "'").ToInt();
                            if (WtCount > 0)
                            {
                                count1 = Math.Round(decimal.Parse((count1 / WtCount).ToString()), 2);
                                dr["WtCount1"] = count1 * 100;
                            }
                        }
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// 获取人员需要检查的项目数量
        /// </summary>
        /// <param name="recId">检查计划Id</param>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
        public int GetCheckCount(string recId,string userAccount)
        {
            string sql = string.Format(" select count(1) from BIS_SAFTYCHECKDATADETAILED where recid='{0}' and instr(',' || CHECKMANID || ',',',{1},')>0 and id not in(select detailid from BIS_SAFTYCONTENT where recid='{0}')",recId, userAccount);
            return BaseRepository().FindObject(sql).ToInt();
        }
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
        /// 根据检查记录删除检查项目
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public int Remove(string recId)
        {
           return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_SAFTYCHECKDATADETAILED where recid='{0}'",recId));
        }
        public void Update(string keyValue,SaftyCheckDataDetailEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var sc = BaseRepository().FindEntity(keyValue);
                if (sc == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(entity.ID);
                    this.BaseRepository().Update(entity);
                }

            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="list">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            BaseRepository().Insert(list);
            //foreach (SaftyCheckDataDetailEntity entity in list)
            //{
            //    if (!string.IsNullOrEmpty(entity.ID))
            //    {
            //        var sc=BaseRepository().FindEntity(entity.ID);
            //        if (sc==null)
            //        {
            //            entity.Create();
            //            if (!string.IsNullOrEmpty(entity.BelongDistrict))
            //            {
            //                entity.BelongDistrict = entity.BelongDistrict;
            //            }
            //            entity.RecID = keyValue;
            //            this.BaseRepository().Insert(entity);
            //        }
            //        else
            //        {
            //            entity.Modify(entity.ID);
            //            this.BaseRepository().Update(entity);
            //        }
                   
            //    }
            //    else
            //    {
            //        entity.Create();
            //        if (!string.IsNullOrEmpty(entity.BelongDistrict))
            //        {
            //             entity.BelongDistrict = entity.BelongDistrict;
            //        }
            //        entity.RecID = keyValue;
            //        this.BaseRepository().Insert(entity);
            //    }

            //}
        }
        /// <summary>
        /// 保存检查项目信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="list">检查项目</param>
        /// <param name="deptCode">参与检查的部门（多个英文逗号分隔）</param>
        public void Save(string keyValue, List<SaftyCheckDataDetailEntity> list,string deptCode="")
        {
            if (BaseRepository().Insert(list) > 0 && !string.IsNullOrEmpty(deptCode))
            {
                string sql = "";
               StringBuilder sb = new StringBuilder();
               foreach(string code in deptCode.Split(','))
               {
                   if (!string.IsNullOrEmpty(code))
                   {
                       sb.AppendFormat("insert into BIS_SAFETYCHECKDEPT(id,recid,deptcode) values('{0}','{1}','{2}');\r\n", Guid.NewGuid().ToString(), list[0].RecID, code);
                   }
                 
               }
                if(sb.Length>0)
                {
                    sql = string.Format("begin\r\n{0} end\r\n commit;",sb.ToString());
                    BaseRepository().ExecuteBySql(sql);
                }
               
            }
        }
        public void Save(string keyValue, List<SaftyCheckDataDetailEntity> list, SaftyCheckDataRecordEntity entity, Operator user,string deptCode = "")
        {
            if (BaseRepository().Insert(list) > 0 && !string.IsNullOrEmpty(deptCode))
            {
                string sql = ""; 
                StringBuilder sb = new StringBuilder();
                foreach (string code in deptCode.Split(','))
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        sb.AppendFormat("insert into BIS_SAFETYCHECKDEPT(id,recid,deptcode) values('{0}','{1}','{2}');\r\n", Guid.NewGuid().ToString(), list[0].RecID, code);
                    }
                }
                if (sb.Length > 0)
                {
                    sql = string.Format("begin\r\n{0} end\r\n commit;", sb.ToString());
                    BaseRepository().ExecuteBySql(sql);
                }
            }
            string id = entity.ID;
            string recId = "";
            if (entity.DataType == 1 && entity.IsSubmit == 1)
            {
                
                SaftyCheckDataRecordService service = new SaftyCheckDataRecordService();
                int j = 0;
                foreach (string deptId in entity.CheckedDepartID.Split(','))
                {

                    SaftyCheckDataRecordEntity sd = new SaftyCheckDataRecordEntity();
                    sd.BelongDept = new DepartmentService().GetEntity(deptId).EnCode;
                    sd.BelongDeptID = deptId;
                    sd.CheckBeginTime = entity.CheckBeginTime;
                    sd.CheckDataRecordName = entity.CheckDataRecordName;
                    sd.CheckDataType = entity.CheckDataType;

                    sd.CheckDept = entity.CheckDept;
                    sd.CheckDeptID = entity.CheckDeptID;
                    if (!string.IsNullOrEmpty(entity.CheckDeptCode))
                    {
                         sd.CheckDeptCode = entity.CheckDeptCode.Trim(',');
                    }
                    sd.CheckEndTime = entity.CheckEndTime;
                    sd.CheckLevel = entity.CheckLevel;
                    sd.CheckLevelID = entity.CheckLevelID;
                    sd.CheckMan = entity.CheckMan;
                    sd.CheckManageMan = entity.CheckManageMan;
                    sd.CheckManageManID = entity.CheckManageManID;
                    sd.CheckManID = entity.CheckManID;
                    sd.CheckUserIds = entity.CheckUserIds;
                    sd.CheckUsers = entity.CheckUsers;
                    sd.DataType = entity.DataType;
                    sd.EndDate = entity.EndDate;
                    sd.ReceiveUserIds = entity.ReceiveUserIds;
                    sd.ReceiveUsers = entity.ReceiveUsers;
                    sd.Remark = entity.Remark;
                    sd.Aim = entity.Aim;
                    sd.AreaName = entity.AreaName;
                    sd.StartDate = entity.StartDate;
                    sd.Status = 0;
                    sd.ID = Guid.NewGuid().ToString();
                    sd.CheckedDepartID = deptId;
                    sd.CheckedDepart = entity.CheckedDepart.Split(',')[j];
                    sd.DutyDept = sd.BelongDept;
                    sd.DutyUserId = id;
                    sd.RId = id;
                    if (user.RoleName.Contains("省级") || user.RoleName.Contains("公司级"))
                    {
                        entity.DutyDept = user.OrganizeCode;
                       
                    }
                    else
                    {
                        entity.DutyDept = sd.BelongDept;
                    }
                  
                    if (service.SaveForm(sd.ID, sd, ref recId) > 0)
                    {
                        string newId = Guid.NewGuid().ToString();
                        BaseRepository().ExecuteBySql(string.Format(@"insert into bis_saftycheckdatadetailed(id,riskname,checkcontent,belongdeptid,belongdept,recid,checkman,checkmanid,checkdataid,checkobject,checkobjectid,checkobjecttype,checkstate,autoid) 
select id || '" + newId + "',t.riskname,t.checkcontent,t.belongdeptid,t.belongdept,'{1}',t.checkman,t.checkmanid,t.checkdataid,t.checkobject,t.checkobjectid,t.checkobjecttype,t.checkstate,autoid from bis_saftycheckdatadetailed t where recid='{0}'", id, sd.ID));

                        newId = new Random().Next(1,10000).ToString();
                        BaseRepository().ExecuteBySql(string.Format(@"insert into BASE_FILEINFO(fileid,folderid,filename,filepath,filesize,fileextensions,filetype,isshare,recid,DeleteMark,createdate)
select t.fileid || '{2}' || rownum,t.folderid,t.filename,t.filepath,t.filesize,t.fileextensions,t.filetype,t.isshare,'{1}',DeleteMark,createdate from BASE_FILEINFO t where recid='{0}'", id, sd.ID, newId));

                        j++;
                    }
                }
            }
        }
        /// <summary>
        /// 保存到检查详情表（登记检查记录单的时候修改）
        /// </summary>
        public void SaveResultForm(List<SaftyCheckDataDetailEntity> list)
        {
            foreach (SaftyCheckDataDetailEntity entity in list)
            {
                entity.Modify(entity.ID);
                this.BaseRepository().Update(entity);
            }
        }
        /// <summary>
        /// 专项检查制定计划保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="list">实体对象</param>
        /// <returns></returns>
        public void SaveFormToContent(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            foreach (SaftyCheckDataDetailEntity entity in list)
            {
                entity.Create();
                entity.RecID = keyValue;
                this.BaseRepository().Insert(entity);
            }
        }
       
        #endregion

        #region 获取数据(手机端)
        public IEnumerable<SaftyCheckDataDetailEntity> GetSaftyDataDetail(string safeCheckIdItem)
        {
            string sqlWhere = "select *from BIS_SAFTYCHECKDATADETAILED where 1=1";
            if (!string.IsNullOrEmpty(sqlWhere))
            {
                sqlWhere += " and recid='" + safeCheckIdItem + "'";
            }
            IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindList(sqlWhere).Select(x =>
            {
                //得到该区域所有的隐患数量
                DataTable dt = this.BaseRepository().FindTable(string.Format("select id from bis_htbaseinfo o where HIDPOINT='" + x.BelongDistrictCode + "' and  o.safetycheckobjectid='{0}'", safeCheckIdItem));
                // in(select recid from bis_saftycheckdatadetailed where  BelongDistrictID='" + x.BelongDistrictID + "')
                x.Count = dt.Rows.Count;
                return x;
            });
            return list;
        }

        public void insertIntoDetails(string checkExcelId, string recid)
        {
            string sqlWhere = string.Format("select * from Bis_Saftycheckdatadetailed where 1=1 and recid='{0}'", checkExcelId);
            IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindList(sqlWhere);
            foreach (SaftyCheckDataDetailEntity item in list)
            {
                item.ID = Guid.NewGuid().ToString();
                item.RecID = recid;
                item.CheckDataId = checkExcelId;
                this.BaseRepository().Insert(item);
            }
        }
        #endregion
    }
}
