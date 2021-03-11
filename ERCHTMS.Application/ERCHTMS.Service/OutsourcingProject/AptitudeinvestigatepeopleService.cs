using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Code;
using System;
using ERCHTMS.Service.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա��
    /// </summary>
    public class AptitudeinvestigatepeopleService : RepositoryFactory<AptitudeinvestigatepeopleEntity>, AptitudeinvestigatepeopleIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AptitudeinvestigatepeopleEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AptitudeinvestigatepeopleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["peoplereviewid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.peoplereviewid='{0}'", queryParam["peoplereviewid"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        public bool IsAuditByUserId(string userid)
        {
            var user = new UserService().GetEntity(userid);
            if (user == null) return false;
            if (string.IsNullOrWhiteSpace(user.ProjectId)) return false;
            else
            {
                var project = new OutsouringengineerService().GetEntity(user.ProjectId);
                if (project == null) return false;
                else
                {
                    var proess = new StartappprocessstatusService().GetList("").Where(x => x.OUTPROJECTID == project.ID).FirstOrDefault();
                    if (proess == null) return false;
                    else
                    {
                        if (proess.PEOPLESTATUS == "1") return true;
                        else return false;
                    }
                }
            }
        }
        #region �ύ����
        /// <summary>
        /// ���֤�����ظ�
        /// </summary>
        /// <param name="IdentifyID">���֤��</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        public bool ExistIdentifyID(string IdentifyID, string keyValue)
        {
            var expression = LinqExtensions.True<AptitudeinvestigatepeopleEntity>();
            expression = expression.And(t => t.IDENTIFYID == IdentifyID && t.SubmitState == 1);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.ID != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }

        /// <summary>
        /// �˺Ų����ظ�
        /// </summary>
        /// <param name="Account">�˺�</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        public bool ExistAccount(string Account, string keyValue)
        {
            var list = (from c in GetList("")
                        join
                       d in new PeopleReviewService().GetList("") on c.PEOPLEREVIEWID equals d.ID into join1
                        from tt in join1.DefaultIfEmpty()
                        where (tt == null  || tt.ISAUDITOVER == "0" || tt.ISAUDITOVER == "" || tt.ISAUDITOVER == null) && c.ACCOUNTS == Account
                        select c).ToList();
            //var expression = LinqExtensions.True<AptitudeinvestigatepeopleEntity>();
            //expression = expression.And(t => t.ACCOUNTS == Account && t.SubmitState == 1);
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    expression = expression.And(t => t.ID != keyValue);
            //}
            if (list.Count == 0)
            {
                return true;
            }
            else
            {
                if (list.Where(t => t.ID == keyValue).Count() == list.Count) //�жϲ�ѯ���������Ƿ��ֻ��������ѯ��������  ����Ǳ�ʾ�����޸�
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public IEnumerable<AptitudeinvestigatepeopleEntity> GetPersonInfo(string projectid, string pageindex, string pagesize)
        {
            var startnum = 0;
            if (pageindex == "0" || pagesize == "0")
                startnum = 0;
            else
                startnum = Convert.ToInt32(pageindex) * Convert.ToInt32(pagesize);
            var endnum = 0;
            if (pageindex == "0")
                endnum = Convert.ToInt32(pagesize);
            else
                endnum = Convert.ToInt32(pageindex) * Convert.ToInt32(pagesize);
            string sql = string.Format("select rownum,t.* from EPG_APTITUDEINVESTIGATEPEOPLE t where t.peoplereviewid='{0}' and rownum>{1} and rownum<{2}", projectid, startnum, endnum);
            return this.BaseRepository().FindList(sql);
        }
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
        public void SaveForm(string keyValue, AptitudeinvestigatepeopleEntity entity)
        {
            entity.ID = keyValue;
            List<DepartmentEntity> listdept = new DepartmentService().GetList().Where(x => x.DepartmentId == entity.OUTPROJECTID).ToList();
            if (listdept.Count > 0)
            {
                var dept = listdept.FirstOrDefault();
                if (dept != null)
                    entity.OUTPROJECTCODE = dept.EnCode;
                else
                    entity.OUTPROJECTCODE = OperatorProvider.Provider.Current().DeptCode;
            }
            else
            {
                entity.OUTPROJECTCODE = OperatorProvider.Provider.Current().DeptCode;
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                AptitudeinvestigatepeopleEntity e = this.BaseRepository().FindEntity(keyValue);
                if (e != null)
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
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// �������������Ա�������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SummitPhyInfo(string keyValue, PhyInfoEntity entity)
        {
            string[] ids = entity.ids.Split(',');

            string strIds = string.Join(",", ids).Replace(",", "','");
          
            string strUpdate = string.Empty;
            if (!string.IsNullOrWhiteSpace(entity.PhysicalUnit))
            {
                strUpdate += string.Format(" PhysicalUnit='{0}',", entity.PhysicalUnit);
            }
            if (!string.IsNullOrWhiteSpace(entity.IsComtraindication))
            {
                strUpdate += string.Format(" IsComtraindication='{0}',", entity.IsComtraindication);
            }
            if (entity.PhysicalTime!=null)
            {
                strUpdate += string.Format(" PhysicalTime=to_date('{0}','yyyy-MM-dd'),", entity.PhysicalTime.Value.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrWhiteSpace(entity.ComtraindicationName))
            {
                strUpdate += string.Format(" ComtraindicationName='{0}',", entity.ComtraindicationName);
            }
            var files = new FileInfoService().GetFiles(keyValue);
            if (files.Rows.Count > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    var key = ids[i]+ "01";
                    foreach (DataRow item in files.Rows)
                    {
                        FileInfoEntity itemFile = new FileInfoEntity();
                        itemFile.FileName = item["FileName"].ToString();
                        itemFile.FilePath = item["filepath"].ToString();
                        itemFile.FileSize = item["filesize"].ToString();
                        itemFile.RecId = key;
                        new FileInfoService().SaveForm("", itemFile);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(strUpdate))
            {
                strUpdate = strUpdate.Substring(0, strUpdate.Length - 1);
                var sql = string.Format("update epg_aptitudeinvestigatepeople set {0} where id in('{1}')", strUpdate, strIds);
                this.BaseRepository().ExecuteBySql(sql);
            }
            
        }
        #endregion
    }
}
