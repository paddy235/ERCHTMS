using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;
using ERCHTMS.Service.BaseManage;
using System;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：矩阵安全检查计划
    /// </summary>
    public class MatrixsafecheckService : RepositoryFactory<MatrixsafecheckEntity>, MatrixsafecheckIService
    {
        #region 获取数据
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetActionNum()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dval = this.BaseRepository().FindTable("select itemvalue from base_dataitemdetail where createdate is not null  and itemid in(select itemid from base_dataitem  where itemcode = 'MatrixSafe')");
            var conditionJson = "";
            if (dval.Rows.Count > 0)
            {
                conditionJson += " and  (sysdate ) > (t.checktime - " + dval.Rows[0]["ITEMVALUE"] + ")  ";
            }
            //  待办查自己
            conditionJson += string.Format(" and  (sysdate ) < (t.checktime+1) and  t.checkid is null and t.ISOVER = 1 and t.createuserid  = '{0}' ", user.UserId);
            var num = this.BaseRepository().FindObject("select count(1) from BIS_MATRIXSAFECHECK t where t.ISOVER = 1 " + conditionJson);
            return num.ToString();
        }

        /// <summary>
        ///  日历获取数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetCanlendarListJson(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string p_fields = @"  t.id, t.createuserid,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate,
                                       t.createusername,
                                       t.modifydate,
                                       t.modifyuserid,
                                       t.modifyusername,
                                       t.contentid,
                                       t.content,
                                       to_char(t.checktime,'yyyy-mm-dd') checktime,
                                       to_char(t.checktime,'mm') checktimemonth,
                                       (case  when t.checkid is not null  then 1 when (sysdate) < (t.checktime+1) then 2 else 3 end) issubmit,
                                       t.checkdept,
                                       t.checkdeptcode,
                                       t.checkdeptname,
                                       t.checkuser,
                                       t.checkusercode,
                                       t.checkusername,
                                       t.isover,
                                       t.checkid,
                                       t.contentnum,
                                       t.checkdeptnum,
                                       t.checkdeptsel ";
            string p_tablename = @" BIS_MATRIXSAFECHECK t  ";

            string conditionJson = " 1=1 ";

            // 日历显示已经提交的
            conditionJson += " and (t.ISOVER = 1)  ";

            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    string deptid = queryParam["examinetodeptid"].ToString();
                    var deptentity = new DepartmentService().GetEntity(deptid);

                    conditionJson += string.Format(" and t.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                }
                else
                {
                    conditionJson += string.Format(" and t.CREATEUSERDEPTCODE ='{0}' ", user.DeptCode);
                }

                if (!queryParam["year"].IsEmpty() && !queryParam["month"].IsEmpty() && !queryParam["timetype"].IsEmpty())
                {
                    if (queryParam["timetype"].ToString() == "0")
                    {
                        conditionJson += string.Format(" and to_char(t.CHECKTIME,'yyyy-mm')  = '{0}' ", queryParam["year"].ToString() + "-" + queryParam["month"].ToString());
                    }
                    else
                    {
                        conditionJson += string.Format(" and to_char(t.CHECKTIME,'yyyy')  = '{0}' ", queryParam["year"].ToString());
                    }

                }
            }

            DataTable data = this.BaseRepository().FindTable("select " + p_fields+ " from  " + p_tablename +" where  " + conditionJson);
            return data;
        }


        /// <summary>
        /// 获取列表分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "t.id";
            pagination.p_fields = @"   t.createuserid,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate,
                                       t.createusername,
                                       t.modifydate,
                                       t.modifyuserid,
                                       t.modifyusername,
                                       t.contentid,
                                       t.content,
                                       to_char(t.checktime,'yyyy-mm-dd') checktime,
                                       (case  when t.checkid is not null  then 1 when (sysdate) < (t.checktime+1) then 2 else 3 end) issubmit,
                                       t.checkdept,
                                       t.checkdeptcode,
                                       t.checkdeptname,
                                       t.checkuser,
                                       t.checkusercode,
                                       t.checkusername,
                                       t.isover,
                                       t.checkid,
                                       t.contentnum,
                                       t.checkdeptnum,
                                       t.checkdeptsel, '' as checkorgin ,'' as checkdeptaction";
            pagination.p_tablename = @" BIS_MATRIXSAFECHECK t  ";

            pagination.conditionJson = " 1=1 ";
            
            // 已提交或者本人未提交的
            pagination.conditionJson += " and ((t.ISOVER = 0 and t.CREATEUSERID = '" + user.UserId + "') or (t.ISOVER = 1)) ";

            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    string deptid = queryParam["examinetodeptid"].ToString();
                    var deptentity = new DepartmentService().GetEntity(deptid);

                    pagination.conditionJson += string.Format(" and t.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.CREATEUSERDEPTCODE ='{0}' ", user.DeptCode);
                }

                if (!queryParam["year"].IsEmpty() && !queryParam["month"].IsEmpty() && !queryParam["timetype"].IsEmpty())
                {
                    if (queryParam["timetype"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and to_char(t.CHECKTIME,'yyyy-mm')  = '{0}' ", queryParam["year"].ToString() + "-" + queryParam["month"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and to_char(t.CHECKTIME,'yyyy')  = '{0}' ", queryParam["year"].ToString());
                    }
                   
                }

                // 待办
                if (!queryParam["qtype"].IsEmpty())
                {
                    if (queryParam["qtype"].ToString() == "1")
                    {
                        DataTable dval = this.BaseRepository().FindTable("select itemvalue from base_dataitemdetail where createdate is not null  and itemid in(select itemid from base_dataitem  where itemcode = 'MatrixSafe')");

                        if (dval.Rows.Count > 0)
                        {
                            pagination.conditionJson += " and  (sysdate ) > (t.checktime - "+ dval.Rows[0]["ITEMVALUE"] + ")  ";
                        }
                        //  待办查自己
                        pagination.conditionJson += string.Format(" and   (sysdate ) < (t.checktime+1)  and  t.checkid is null and t.ISOVER = 1 and t.createuserid  = '{0}' ", user.UserId);
                    }
                }

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
                    pagination.conditionJson += string.Format(" and to_date(to_char(t.CHECKTIME,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            foreach (DataRow dr in data.Rows)
            {
                if (dr["CHECKDEPTNAME"].ToString() != "")
                {
                    var chearr = dr["CHECKDEPTNAME"].ToString().Split(',');
                    for (int i = 0; i < chearr.Length; i++)
                    {
                        if (i == 0)
                        {
                            dr["CHECKORGIN"] = chearr[i];
                        }
                        else if (i == 1)
                        {
                            dr["CHECKDEPTACTION"] = chearr[i];
                        }
                        else 
                        {
                            dr["CHECKDEPTACTION"] += ","+ chearr[i];
                        }
                    }
                }
            }
            return data;
        }


        /// <summary>
        /// 根据sql查出返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteBySql(string sql)
        {
            return this.BaseRepository().ExecuteBySql(sql);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MatrixsafecheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MatrixsafecheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public MatrixsafecheckEntity SetFormJson(string keyValue, string recid)
        {
            MatrixsafecheckEntity en = GetEntity(keyValue);

            if (en.CHECKID != "")
            {
                en.CHECKID += "," +recid;
            }
            else
            {
                en.CHECKID =  recid;
            }
            
            SaveForm(keyValue, en);
            return en;
        }

        /// <summary>
        /// 获取检查内容
        /// </summary>
        /// <returns></returns>
        public DataTable GetContentPageJson(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string where = "";


            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    string deptid = queryParam["examinetodeptid"].ToString();
                    var deptentity = new DepartmentService().GetEntity(deptid);

                    if (deptentity.IsOrg == 0)  // 如果不是厂级,查询部门
                    {
                        where += string.Format(" and a.ISROLE = '1'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }
                    else  // 如果是厂级，查询的全部厂级数据
                    {
                        //where += " and a.ISROLE = '0'";
                        where += string.Format(" and a.ISROLE = '0'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }


                }
                else
                {
                    if (user.RoleName.Contains("厂级"))
                    {
                        //where += " and a.ISROLE = '0'";
                        where += " and a.ISROLE = '0' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                    }
                    else
                    {
                        where += " and a.ISROLE = '1' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                    }
                }
                
            }
            else // 没有条件的时候查当前人
            {
                if (user.RoleName.Contains("厂级"))
                {
                    //where += " and a.ISROLE = '0'";
                    where += " and a.ISROLE = '0' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
                else
                {
                    where += " and a.ISROLE = '1' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
            }



            string sql = string.Format(@"select id,
                                           createuserid,
                                           createuserdeptcode,
                                           createuserorgcode,
                                           createdate,
                                           createusername,
                                           modifydate,
                                           modifyuserid,
                                           modifyusername,
                                           code,
                                           content,  1 as edit ,(select count(1) from bis_matrixsafecheck c where instr(c.CONTENTID,a.id)>0  ) isdel
                                      from bis_matrixcontent a where 1=1 {0}
                                     order by code asc
                                     ", where);

            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// 获取检查部门列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetDeptPageJson(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string where = "";
            //if (user.RoleName.Contains("厂级"))
            //{
            //    where += " and a.ISROLE = '0'";
            //}
            //else
            //{
            //    where += " and a.ISROLE = '1' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
            //}

            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    string deptid = queryParam["examinetodeptid"].ToString();
                    var deptentity = new DepartmentService().GetEntity(deptid);

                    if (deptentity.IsOrg == 0)  // 如果不是厂级,查询部门
                    {
                        where += string.Format(" and a.ISROLE = '1'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }
                    else  // 如果是厂级，查询的全部厂级数据
                    {
                        //where += " and a.ISROLE = '0'";
                        where += string.Format(" and a.ISROLE = '0'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }


                }
                else
                {
                    if (user.RoleName.Contains("厂级"))
                    {
                        //where += " and a.ISROLE = '0'";
                        where += " and a.ISROLE = '0' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                    }
                    else
                    {
                        where += " and a.ISROLE = '1' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                    }
                }

            }
            else // 没有条件的时候查当前人
            {
                if (user.RoleName.Contains("厂级"))
                {
                    //where += " and a.ISROLE = '0'";
                    where += " and a.ISROLE = '0' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
                else
                {
                    where += " and a.ISROLE = '1' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
            }

            string sql = string.Format(@"select id,
                                       createuserid,
                                       createuserdeptcode,
                                       createuserorgcode,
                                       createdate,
                                       createusername,
                                       modifydate,
                                       modifyuserid,
                                       modifyusername,
                                       code,
                                       dept,
                                       deptname,
                                       deptcode,
                                        1 as edit,(select count(1) from bis_matrixsafecheck c where instr(c.CHECKDEPTSEL,a.id)>0  ) isdel 
                                  from bis_matrixdept a where 1=1 {0}
                                   order by code asc
                                     ", where);

            return this.BaseRepository().FindTable(sql);
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
        public void SaveForm(string keyValue, MatrixsafecheckEntity entity)
        {
            entity.ID = keyValue;
            //开始事务
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    MatrixsafecheckEntity se = this.BaseRepository().FindEntity(keyValue);
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
