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

namespace ERCHTMS.Service.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�������Ʒ
    /// </summary>
    public class LaborprotectionService : RepositoryFactory<LaborprotectionEntity>, LaborprotectionIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LaborprotectionEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LaborprotectionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["name"].IsEmpty())
            {
                string name = queryParam["name"].ToString();

                pagination.conditionJson += string.Format(" and NAME  like '%{0}%'", name);

                
            }
            if (!queryParam["type"].IsEmpty())
            {
                string unit = queryParam["type"].ToString();

                pagination.conditionJson += string.Format(" and type  like '%{0}%'", unit);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ��ǰ������������
        /// </summary>
        /// <returns></returns>
        public List<LaborprotectionEntity> GetLaborList()
        {
            string sql = string.Format("select * from BIS_LABORPROTECTION where CREATEUSERORGCODE='{0}' order by no desc",
                OperatorProvider.Provider.Current().OrganizeCode);
            List<LaborprotectionEntity> LaborList = BaseRepository().FindList(sql).ToList();
            return LaborList;
        }

        /// <summary>
        /// ��ȡ����ǰ
        /// </summary>
        /// <returns></returns>
        public string GetNo()
        {
            string time = DateTime.Now.ToString("yyyyMM");
            string sql = string.Format("select No from (select NO from BIS_LABORPROTECTION where CREATEUSERORGCODE='{0}' and no like '{1}%' order by no desc) where  rownum<=1",
                OperatorProvider.Provider.Current().OrganizeCode, time);
            object no = BaseRepository().FindObject(sql);
            if (no == null)
            {
                return time + "001";
            }
            else
            {
                int newno = Convert.ToInt32(no) + 1;
                return newno.ToString();
            }
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
        public void SaveForm(string keyValue, LaborprotectionEntity entity)
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
