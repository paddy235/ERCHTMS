using System;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�������Ʒ��
    /// </summary>
    public class LaborinfoService : RepositoryFactory<LaborinfoEntity>, LaborinfoIService
    {
        #region ��ȡ����

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //����
            if (!queryParam["Name"].IsEmpty())
            {
                string name = queryParam["Name"].ToString();

                pagination.conditionJson += string.Format(" and info.name like '%{0}%'", name);

            }
            //���
            if (!queryParam["no"].IsEmpty())
            {
                string no = queryParam["no"].ToString();
                pagination.conditionJson += string.Format(" and no like '%{0}%'", no);
            }
            //����
            if (!queryParam["DeptCode"].IsEmpty())
            {

                string deptcode = queryParam["DeptCode"].ToString();
                pagination.conditionJson += string.Format(" and DEPTCODE  like '{0}%'", deptcode);
            }
            //���ڵ�ѡ����
            if (!queryParam["TreeDeptCode"].IsEmpty())
            {

                string deptcode = queryParam["TreeDeptCode"].ToString();
                pagination.conditionJson += string.Format(" and DEPTCODE  like '{0}%'", deptcode);
            }
            //��λ
            if (!queryParam["PostId"].IsEmpty())
            {
                string postid = queryParam["PostId"].ToString();
                pagination.conditionJson += string.Format(" and postid  ='{0}'", postid);
            }
            //����
            if (!queryParam["Type"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  TYPE = '{0}'", queryParam["Type"].ToString().Trim());
            }
            //���֤��
            if (!queryParam["NextTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  TO_char(NEXTTIME,'yyyy-mm-dd') = '{0}'", queryParam["NextTime"].ToString().Trim());
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson, string where)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string Sql = @"select ID,NO,info.NAME,TYPE,model,ORGNAME,DEPTNAME,POSTNAME,SHOULDNUM,UNIT,TIMENUM,TIMETYPE,to_char(RECENTTIME,'yyyy-mm-dd') RECENTTIME,to_char(NEXTTIME,'yyyy-mm-dd') NEXTTIME,ISSUENUM,'' InStock,yj.value from BIS_LABORINFO info left join (select name,value from bis_laboreamyj where createuserorgcode='" + orgcode + "') yj on info.name=yj.name where 1=1";
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //����
            if (!queryParam["Name"].IsEmpty())
            {
                string name = queryParam["Name"].ToString();

                where += string.Format(" and name like '%{0}%'", name);

            }
            //���
            if (!queryParam["no"].IsEmpty())
            {
                string no = queryParam["no"].ToString();
                where += string.Format(" and no like '%{0}%'", no);
            }
            //����
            if (!queryParam["DeptCode"].IsEmpty())
            {

                string deptcode = queryParam["DeptCode"].ToString();
                where += string.Format(" and DEPTCODE  like '{0}%'", deptcode);
            }
            //���ڵ�ѡ����
            if (!queryParam["TreeDeptCode"].IsEmpty())
            {

                string deptcode = queryParam["TreeDeptCode"].ToString();
                where += string.Format(" and DEPTCODE  like '{0}%'", deptcode);
            }
            //��λ
            if (!queryParam["PostId"].IsEmpty())
            {
                string postid = queryParam["PostId"].ToString();
                where += string.Format(" and postid  ='{0}'", postid);
            }
            //����
            if (!queryParam["Type"].IsEmpty())
            {
                where += string.Format(" and  TYPE = '{0}'", queryParam["Type"].ToString().Trim());
            }
            //���֤��
            if (!queryParam["NextTime"].IsEmpty())
            {
                where += string.Format(" and  TO_char(NEXTTIME,'yyyy-mm-dd') = '{0}'", queryParam["NextTime"].ToString().Trim());
            }

            where += " order by info.CreateDate desc";
            if (where != "")
            {
                Sql += where;
            }

            return this.BaseRepository().FindTable(Sql);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LaborinfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LaborinfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ����ids��ȡ����������������
        /// </summary>
        /// <param name="InfoId"></param>
        /// <returns></returns>
        public DataTable Getplff(string InfoId)
        {
            string sql = string.Format(
                @"select id,name,model,orgname,deptname,postname,usercount,1 percount,(usercount*1)count from bis_laborinfo  info
            left join (select assid, Count(assid) usercount from bis_laborequipmentinfo where labortype=0 group by assid)  eq on info.id=eq.assid
            where id in({0})", InfoId);
            return BaseRepository().FindTable(sql);
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
        public void ImportSaveForm(List<LaborinfoEntity> entity, List<LaborprotectionEntity> prolist,List<LaborequipmentinfoEntity> eqlist)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                
                res.Insert<LaborprotectionEntity>(prolist);
                res.Insert<LaborinfoEntity>(entity);
                res.Insert<LaborequipmentinfoEntity>(eqlist);
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LaborinfoEntity entity, string json, string ID)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //�������IDΪ����Ϊ����������һ������
                if (entity.LId == null || entity.LId == "")
                {
                    LaborprotectionEntity labor = new LaborprotectionEntity();
                    labor.LaborOperationTime = DateTime.Now;
                    labor.LaborOperationUserName = OperatorProvider.Provider.Current().UserName;
                    labor.Model = entity.Model;
                    labor.Name = entity.Name;
                    labor.No = entity.No;
                    labor.Note = entity.Note;
                    labor.TimeNum = entity.TimeNum;
                    labor.TimeType = entity.TimeType;
                    labor.Type = entity.Type;
                    labor.Unit = entity.Unit;
                    labor.Create();
                    labor.ID = ID;
                    entity.LId = labor.ID;
                    res.Insert<LaborprotectionEntity>(labor);

                }

                Repository<DepartmentEntity> inlogdb = new Repository<DepartmentEntity>(DbFactory.Base());
                DepartmentEntity dept = inlogdb.FindEntity(entity.DeptId);

                entity.DeptCode = dept.DeptCode;

                Repository<DepartmentEntity> orgdb = new Repository<DepartmentEntity>(DbFactory.Base());
                DepartmentEntity org = orgdb.FindEntity(dept.OrganizeId);

                if (org.Nature.Contains("ʡ��"))
                {
                    entity.OrgCode = org.DeptCode;
                    entity.OrgId = org.OrganizeId;
                    entity.OrgName = org.FullName;
                }
                else
                {
                    entity.OrgCode = org.EnCode;
                    entity.OrgId = org.OrganizeId;
                    entity.OrgName = org.FullName;
                }


                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<LaborinfoEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<LaborinfoEntity>(entity);
                }


                //��ɾ����Σ�������µ�������Ա
                string sql = string.Format("delete from BIS_LABOREQUIPMENTINFO where ASSID='{0}'", entity.ID);
                res.ExecuteBySql(sql);
                json = json.Replace("&nbsp;", "");
                List<LaborequipmentinfoEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LaborequipmentinfoEntity>>(json);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Create();
                        list[i].AssId = entity.ID;
                        list[i].LaborType = 0;
                    }
                    res.Insert<LaborequipmentinfoEntity>(list);
                }

               
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
                throw e;
            }

        }
        #endregion
    }
}
