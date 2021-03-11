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
    /// �� ��������ȫ���ƻ�
    /// </summary>
    public class MatrixsafecheckService : RepositoryFactory<MatrixsafecheckEntity>, MatrixsafecheckIService
    {
        #region ��ȡ����
        

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
            //  ������Լ�
            conditionJson += string.Format(" and  (sysdate ) < (t.checktime+1) and  t.checkid is null and t.ISOVER = 1 and t.createuserid  = '{0}' ", user.UserId);
            var num = this.BaseRepository().FindObject("select count(1) from BIS_MATRIXSAFECHECK t where t.ISOVER = 1 " + conditionJson);
            return num.ToString();
        }

        /// <summary>
        ///  ������ȡ����
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

            // ������ʾ�Ѿ��ύ��
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
        /// ��ȡ�б��ҳ
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
            
            // ���ύ���߱���δ�ύ��
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

                // ����
                if (!queryParam["qtype"].IsEmpty())
                {
                    if (queryParam["qtype"].ToString() == "1")
                    {
                        DataTable dval = this.BaseRepository().FindTable("select itemvalue from base_dataitemdetail where createdate is not null  and itemid in(select itemid from base_dataitem  where itemcode = 'MatrixSafe')");

                        if (dval.Rows.Count > 0)
                        {
                            pagination.conditionJson += " and  (sysdate ) > (t.checktime - "+ dval.Rows[0]["ITEMVALUE"] + ")  ";
                        }
                        //  ������Լ�
                        pagination.conditionJson += string.Format(" and   (sysdate ) < (t.checktime+1)  and  t.checkid is null and t.ISOVER = 1 and t.createuserid  = '{0}' ", user.UserId);
                    }
                }

                //ʱ�䷶Χ
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
        /// ����sql������ؼ���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// ִ��sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteBySql(string sql)
        {
            return this.BaseRepository().ExecuteBySql(sql);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<MatrixsafecheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
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
        /// ��ȡ�������
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

                    if (deptentity.IsOrg == 0)  // ������ǳ���,��ѯ����
                    {
                        where += string.Format(" and a.ISROLE = '1'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }
                    else  // ����ǳ�������ѯ��ȫ����������
                    {
                        //where += " and a.ISROLE = '0'";
                        where += string.Format(" and a.ISROLE = '0'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }


                }
                else
                {
                    if (user.RoleName.Contains("����"))
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
            else // û��������ʱ��鵱ǰ��
            {
                if (user.RoleName.Contains("����"))
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
        /// ��ȡ��鲿���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetDeptPageJson(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string where = "";
            //if (user.RoleName.Contains("����"))
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

                    if (deptentity.IsOrg == 0)  // ������ǳ���,��ѯ����
                    {
                        where += string.Format(" and a.ISROLE = '1'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }
                    else  // ����ǳ�������ѯ��ȫ����������
                    {
                        //where += " and a.ISROLE = '0'";
                        where += string.Format(" and a.ISROLE = '0'  and a.CREATEUSERDEPTCODE ='{0}' ", deptentity.EnCode);
                    }


                }
                else
                {
                    if (user.RoleName.Contains("����"))
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
            else // û��������ʱ��鵱ǰ��
            {
                if (user.RoleName.Contains("����"))
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, MatrixsafecheckEntity entity)
        {
            entity.ID = keyValue;
            //��ʼ����
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
