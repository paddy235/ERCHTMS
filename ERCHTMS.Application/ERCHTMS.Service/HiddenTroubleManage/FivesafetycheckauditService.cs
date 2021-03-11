using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ��� �������ձ�
    /// </summary>
    public class FivesafetycheckauditService : RepositoryFactory<FivesafetycheckauditEntity>, FivesafetycheckauditIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ѯ���ŷָ���id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public DataTable GeDataTableByIds(string ids)
        {
            DataTable dt = null;
            try
            {
                string strsql = "'" + ids.Replace(",", "','") + "'";
                dt = this.BaseRepository().FindTable("SELECT ID,FINDQUESTION FROM  BIS_FIVESAFETYCHECKAUDIT WHERE ID IN (" + strsql + ")");

            }
            catch (System.Exception ex)
            {

                dt = null;
            }
            
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<FivesafetycheckauditEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FivesafetycheckauditEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��������б��ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.createuserid,
                                   t.createuserdeptcode,
                                  t.createuserorgcode,
                                   t.createdate,
                                   t.createusername,
                                   t.modifydate,
                                   t.modifyuserid,
                                   t.modifyusername,
                                   t.findquestion,
                                   t.actioncontent,
                                   t.dutyusername,
                                   t.dutyuserid,
                                   t.dutydeptcode,
                                   t.dutydept,
                                   t.dutydeptid,
                                    to_char(t.finishdate,'yyyy-mm-dd') finishdate,
                                   t.acceptuser,
                                   t.acceptuserid,
                                   t.actionresult,
                                   to_char(t.actualdate,'yyyy-mm-dd') actualdate,
                                   t.beizhu,
                                   t.acceptreuslt,
                                   t.acceptcontent,
                                   t.checkid";
            pagination.p_tablename = @" bis_fivesafetycheckaudit t  left join base_user usr on usr.userid = t.ACCEPTUSERID";

            pagination.conditionJson = " t.CHECKPASS = '1' and t.checkid in (select id from bis_fivesafetycheck where ISOVER in ('1','2','3')) "; // ֻ�������̵�����

            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();


                if (!queryParam["checkid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.CHECKID ='{0}' ", queryParam["checkid"].ToString());
                }

                // ����
                if (!queryParam["qtype"].IsEmpty())
                {
                    if (queryParam["istype"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and (t.actionresult <> '0' or t.actionresult is null)  and dutyuserid = '{0}'  ", user.UserId);
                    }
                    else if (queryParam["istype"].ToString() == "1")
                    {
                        pagination.conditionJson += string.Format(" and t.actionresult = '0' and (t.ACCEPTREUSLT <> '0' or t.ACCEPTREUSLT is null)  and acceptuserid = '{0}'  ", user.UserId);
                    }
                }
                // �ؼ���
                if (!queryParam["keyword"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.FINDQUESTION like '%{0}%' ", queryParam["keyword"].ToString());
                }
                // ��֯����
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    if (queryParam["istype"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and t.DUTYDEPTID = '{0}' ", queryParam["examinetodeptid"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and usr.departmentid = '{0}' ", queryParam["examinetodeptid"].ToString());
                    }
                    
                }
                // ���ಿ���쵼���Կ����е�
                if (user.RoleName.Contains("�쵼") || user.RoleName.Contains("���������û�"))
                {
                    pagination.conditionJson += " and 1=1 ";
                }
                else //���ĵ����ݲ����������ڲ��Ŷ����Բ鿴�����յ��������ղ��Ŷ����Բ鿴
                {
                    // 0:����  1����
                    if (!queryParam["istype"].IsEmpty())
                    {
                        if (queryParam["istype"].ToString() == "0")
                        {
                            var deptentity = new DepartmentService().GetEntity(user.DeptId);
                            while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                            {
                                deptentity = new DepartmentService().GetEntity(deptentity.ParentId);
                            }
                            // ��ǰ�����ڵĲ���ȫ�������Բ鿴
                            pagination.conditionJson += string.Format(" and t.DUTYUSERID  in (select userid from base_user where DEPARTMENTID in (select departmentid from base_department where encode like '{0}%')) ", deptentity.EnCode);

                            //pagination.conditionJson += string.Format(" and t.DUTYUSERID = '{0}' ", user.UserId);

                        }
                        else if (queryParam["istype"].ToString() == "1")
                        {
                            var deptentity = new DepartmentService().GetEntity(user.DeptId);
                            while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                            {
                                deptentity = new DepartmentService().GetEntity(deptentity.ParentId);
                            }
                            // ��ǰ�����ڵĲ���ȫ�������Բ鿴
                            pagination.conditionJson += string.Format(" and t.ACCEPTUSERID in (select userid from base_user where DEPARTMENTID in (select departmentid from base_department where encode like '{0}%')) ", deptentity.EnCode);
                            //pagination.conditionJson += string.Format(" and t.ACCEPTUSERID = '{0}' and t.ACTIONRESULT = '0' ", user.UserId);
                        }
                    }
                }

            }
            else
            {
                pagination.conditionJson = " 1 <> 0";
            }



            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        public void SaveForm(string keyValue, FivesafetycheckauditEntity entity)
        {
            entity.ID = keyValue;
            //��ʼ����
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    FivesafetycheckauditEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        this.BaseRepository().Insert(entity);


                    }
                    else
                    {
                        entity.Modify(keyValue);
                        this.BaseRepository().Update(entity);
                        if (entity.ACTUALDATE == null)
                        {
                            this.BaseRepository().ExecuteBySql("update bis_fivesafetycheckaudit set ACTUALDATE = null  where id = '"+ keyValue + "' ");

                        }
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
