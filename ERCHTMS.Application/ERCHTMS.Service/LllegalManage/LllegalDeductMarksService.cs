using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：违章档案扣分表
    /// </summary>
    public class LllegalDeductMarksService : RepositoryFactory<LllegalDeductMarksEntity>, LllegalDeductMarksIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalDeductMarksEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalDeductMarksEntity GetEntity(string keyValue)
        {
            try
            {
                string sql = string.Format(@"select a.id,a.autoid,a.createuserid,a.createuserdeptcode,a.createuserorgcode,a.createdate,a.createusername,a.modifydate,a.modifyuserid,a.modifyusername,a.lllegalid,a.punishid,a.userid,a.username,
                                        b.dutyname dutyname,a.deptid ,a.deptname,a.teamid,a.teamname,a.punishdate,a.punishresult,a.punishpoint,a.lllegaltypename,a.lllegaltype,a.lllegaldescribe,a.appsign from bis_lllegaldeductmarks a 
                                        left join base_user b on a.userid =b.userid where a.id = '{0}'", keyValue);
                return this.BaseRepository().FindList(sql).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LllegalDeductMarksEntity entity)
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

        /// <summary>
        /// 获取单个实例
        /// </summary>
        /// <param name="punishid"></param>
        /// <returns></returns>
        public LllegalDeductMarksEntity GetLllegalRecordEntity(string punishid)
        {
            try
            {
                string sql = string.Format(@" select id ,0 autoid,createuserid,'' createuserdeptcode,'' createuserorgcode, null createdate,'' createusername,null  modifydate,'' modifyuserid,
                                            '' modifyusername, lllegalid,  punishid,userid,username,dutyname, deptid,deptname,teamid,teamname,punishdate,punishresult, punishpoint, lllegaltypename,lllegaltype ,lllegaldescribe,appsign   
                                            from v_lllegalrecordinfo where punishid='{0}' ", punishid);
                return this.BaseRepository().FindList(sql).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LllegalDeductMarksEntity> GetLllegalRecorList(string punishdate,string userid, string describe, string deptid="",string teamid="")
        {
            try
            {
                string strWhere = string.Empty;
                if (!string.IsNullOrEmpty(userid))
                {
                    strWhere += string.Format(@" and userid='{0}'", userid);
                }
                if (!string.IsNullOrEmpty(describe))
                {
                    strWhere += string.Format(@" and lllegaldescribe='{0}'", describe);
                }
                if (!string.IsNullOrEmpty(deptid))
                {
                    strWhere += string.Format(@" and deptid='{0}'", deptid);
                }
                if (!string.IsNullOrEmpty(teamid))
                {
                    strWhere += string.Format(@" and teamid='{0}'", teamid);
                }
                if (!string.IsNullOrEmpty(punishdate))
                {
                    strWhere += string.Format(@" and to_char(punishdate,'yyyy-MM-dd') ='{0}'", punishdate);
                }
                string sql = string.Format(@" select id ,0 autoid,createuserid,'' createuserdeptcode,'' createuserorgcode, null createdate,'' createusername,null  modifydate,'' modifyuserid,
                                            '' modifyusername, lllegalid,  punishid,userid,username,dutyname, deptid,deptname,teamid,teamname,punishdate,punishresult, punishpoint, lllegaltypename,lllegaltype ,lllegaldescribe,appsign   
                                            from v_lllegalrecordinfo where 1=1 {0} ", strWhere);

                return this.BaseRepository().FindList(sql).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region  违章扣分信息查询
        /// <summary>
        /// 违章扣分信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetLllegalRecordInfo(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            try
            {
                pagination.p_kid = "id";

                pagination.conditionJson = " 1=1";

                var queryParam = queryJson.ToJObject();

                //当前用户
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                pagination.p_tablename = @"v_lllegalrecordinfo a";

                pagination.p_fields = @"lllegalid,punishid,lllegaltype,lllegaltypename,flowstate,addtype,lllegaldescribe,lllegalpoint,economicspunish ,assessobject,
                deptid,deptcode,deptname,teamid,teamcode,teamname,userid,username,dutyname,punishpoint,punishresult,punishdate,appsign,deptsort,sortcode,createuserid";


                //部门编码
                if (!queryParam["code"].IsEmpty())
                {
                    string code = queryParam["code"].ToString();
                    if (code.StartsWith("ls100_") || code.StartsWith("cs100_"))
                    {
                        code = code.Substring(6);
                    }
                    pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", code);
                }
                //查询模式
                if (!queryParam["mode"].IsEmpty())
                {
                    if (queryParam["mode"].ToString() == "0")
                    {
                
                        if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司"))
                        {
                            pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", curUser.OrganizeCode);
                        }
                        else if (curUser.RoleName.Contains("班组") || curUser.RoleName.Contains("专业"))
                        {
                            pagination.conditionJson += string.Format(@" and teamcode like '{0}%'", curUser.DeptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", curUser.DeptCode);
                        }

                        pagination.conditionJson += string.Format(@" and nature != '承包商'");
                    }
                    else
                    {
                        if (curUser.RoleName.Contains("承包商"))
                        {
                            pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", curUser.DeptCode);
                        }
                        pagination.conditionJson += string.Format(@" and nature = '承包商'");
                    }
                }
                //处罚时间开始时间
                if (!queryParam["stdate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and punishdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["stdate"].ToString());
                }
                //处罚时间结束时间
                if (!queryParam["etdate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and punishdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["etdate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
                //违章类型
                if (!queryParam["lllegaltype"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and  lllegaltype='{0}' ", queryParam["lllegaltype"].ToString());
                }
                //违章描述 
                if (!queryParam["lllegaldescribe"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and lllegaldescribe like '%{0}%'", queryParam["lllegaldescribe"].ToString());
                }
                //搜索条件
                if (!queryParam["keyword"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and  (username like '%{0}%'  or  deptname like '%{0}%'  or  teamname like '%{0}%'  or  dutyname like '%{0}%')", queryParam["keyword"].ToString());
                }
                //待办事项 
                if (!queryParam["special"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and deptid = '{0}' and to_number(appsign) > 0 and userid is null ", curUser.DeptId);
                } 
                var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region  违章得分信息查询
        /// <summary>
        /// 违章得分信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetLllegalPointInfo(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            try
            {
                pagination.p_kid = "userid";

                pagination.conditionJson = " 1=1";

                var queryParam = queryJson.ToJObject();

                //当前用户
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string strWhere = string.Empty;

                //处罚时间开始时间
                if (!queryParam["stdate"].IsEmpty())
                {
                    strWhere += string.Format(@" and punishdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["stdate"].ToString());
                }
                //处罚时间结束时间
                if (!queryParam["etdate"].IsEmpty())
                {
                    strWhere += string.Format(@" and punishdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["etdate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
                //违章类型
                if (!queryParam["lllegaltype"].IsEmpty())
                {
                    strWhere += string.Format(@" and  lllegaltype='{0}' ", queryParam["lllegaltype"].ToString());
                }
                //违章描述 
                if (!queryParam["lllegaldescribe"].IsEmpty())
                {
                    strWhere += string.Format(@" and lllegaldescribe like '%{0}%'", queryParam["lllegaldescribe"].ToString());
                }
                //待办事项 
                if (!queryParam["special"].IsEmpty())
                {
                    strWhere += string.Format(@"  and deptid = '{0}' and to_number(appsign) > 0 and userid is null ", curUser.DeptId);
                }

                pagination.p_tablename = string.Format(@"(select (select to_number(itemvalue) itemvalue from base_dataitemdetail where itemname ='LllegalRecordPoint') initpoint, 
                    ((select to_number(itemvalue) itemvalue from base_dataitemdetail where itemname ='LllegalRecordPoint') - sum(punishpoint)) point ,
                     sum(punishpoint) punishpoint ,userid,username,deptid,deptcode,deptname,teamid,teamcode,teamname,dutyname,nature,deptsort,sortcode from v_lllegalrecordinfo  a
                     where userid is not null {0} group by userid,username,deptid,deptcode,deptname,teamid,teamcode,teamname,dutyname,nature,deptsort,sortcode ) a", strWhere);

                pagination.p_fields = @"initpoint,point,punishpoint, username,deptid,deptcode,deptname,teamid,teamcode,teamname,dutyname";


                //部门编码
                if (!queryParam["code"].IsEmpty())
                {
                    string code = queryParam["code"].ToString();
                    if (code.StartsWith("ls100_") || code.StartsWith("cs100_"))
                    {
                        code = code.Substring(6);
                    }
                    pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", code);
                }
                //查询模式
                if (!queryParam["mode"].IsEmpty())
                {
                    if (queryParam["mode"].ToString() == "0")
                    {
                        if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司"))
                        {
                            pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", curUser.OrganizeCode);
                        }
                        else if (curUser.RoleName.Contains("班组") || curUser.RoleName.Contains("专业"))
                        {
                            pagination.conditionJson += string.Format(@" and teamcode like '{0}%'", curUser.DeptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", curUser.DeptCode);
                        }

                        pagination.conditionJson += string.Format(@" and nature != '承包商'");
                    }
                    else
                    {
                        if (curUser.RoleName.Contains("承包商"))
                        {
                            pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", curUser.DeptCode);
                        }
                        pagination.conditionJson += string.Format(@" and nature = '承包商'");
                    }
                }
                //搜索条件
                if (!queryParam["keyword"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and  (username like '%{0}%'  or  deptname like '%{0}%'  or  teamname like '%{0}%'  or  dutyname like '%{0}%')", queryParam["keyword"].ToString());
                }
      
                var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}