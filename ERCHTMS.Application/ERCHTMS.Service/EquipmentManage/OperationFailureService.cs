using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using ERCHTMS.Code;
using System;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// �� �������й��ϼ�¼��
    /// </summary>
    public class OperationFailureService : RepositoryFactory<OperationFailureEntity>, OperationFailureIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OperationFailureEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OperationFailureEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡʡ�����й���ͳ�Ƽ�¼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetOperationFailureRecordForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            DataTable dt = new DataTable();
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }
            
            sqlwhere = " to_char(t1.purchasetime,'yyyy')='" + year + "'";
            sqlwhere += " and t1.createuserorgcode in (select encode from base_department where deptcode like '" + user.NewDeptCode + "%')";
            if (!queryParam["code"].IsEmpty())
            {
                if (queryParam["code"].ToString() != user.OrganizeCode)
                {
                    sqlwhere += " and t1.createuserorgcode ='" + queryParam["code"].ToString() + "'";
                } 
            }
            string forsql = string.Format("select t1.id,t1.certificateid,t1.checkfileid,t1.acceptance,c.organizeid,t2.failurenature,t2.failurereason,t2.takesteps,t2.handleresult from bis_operationfailure t2  left join BIS_specialequipment t1 on  t1.id=t2.equipmentid left join base_department c on t1.createuserorgcode =c.encode where {0}", sqlwhere);
            dt = this.BaseRepository().FindTable(forsql);
            return dt;
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
        public void SaveForm(string keyValue, OperationFailureEntity entity)
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
