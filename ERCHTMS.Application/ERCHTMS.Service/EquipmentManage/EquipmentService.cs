using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using BSFramework.Data;
using System.Data;
using System.Dynamic;
using System;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Service.SystemManage;
using System.Text;

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// �� ������ͨ�豸������Ϣ��
    /// </summary>
    public class EquipmentService : RepositoryFactory<EquipmentEntity>, EquipmentIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //�豸���
            if (!queryParam["Etype"].IsEmpty())
            {
                if (!string.IsNullOrEmpty(queryParam["Etype"].ToString()) && queryParam["Etype"].ToString().Length < 3)
                    pagination.conditionJson += string.Format(" and EQUIPMENTTYPE='{0}'", queryParam["Etype"].ToString());
            }
            //������ϵ
            if (!queryParam["Affiliation"].IsEmpty())
            {
                if (!string.IsNullOrEmpty(queryParam["Affiliation"].ToString()) && queryParam["Affiliation"].ToString().Length < 3)
                    pagination.conditionJson += string.Format(" and Affiliation='{0}'", queryParam["Affiliation"].ToString());
            }
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            //�볡�豸
            if (!queryParam["ispresence"].IsEmpty())
            {
                pagination.conditionJson += " and state in (select itemvalue from base_dataitemdetail where itemid in( select itemid from base_dataitem where itemcode ='EQUIPMENTSTATE') and itemname='�볧')";
            }
            else
            {
                pagination.conditionJson += " and state not in (select itemvalue from base_dataitemdetail where itemid in( select itemid from base_dataitem where itemcode ='EQUIPMENTSTATE') and itemname='�볧')";
            }

            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                string deptCode = queryParam["code"].ToString();
                string orgType = queryParam["isOrg"].ToString();
                if (orgType == "District")
                {
                    pagination.conditionJson += string.Format(" and DISTRICTCODE  like '{0}%'", deptCode);
                }
                else if (orgType == "Organize")
                {
                    pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and ControlDeptCode like '{0}%'", deptCode);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <returns>�����б�</returns>
        public IEnumerable<EquipmentEntity> GetList(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return this.BaseRepository().IQueryable().ToList();
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EquipmentEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ�豸���
        /// </summary>
        /// <param name="EquipmentNo">�豸���</param>
        /// <returns></returns>
        public string GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            //��ȡ���´������豸���
            string sql = string.Format("select t.equipmentno from BIS_equipment t where t.equipmentno like '{0}%' and t.createuserorgcode='{1}' order by t.createdate desc", EquipmentNo, orgcode);
            DataTable dt = this.BaseRepository().FindTable(sql);
            string no = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                no = dt.Rows[0][0].ToString();
                no = no.Replace(EquipmentNo, "");
            }

            return no;
        }

        /// <summary> 
        /// ͨ���û�id��ȡ�û��б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetEquipmentTable(string[] ids)
        {
            var strSql = new StringBuilder();
            string sql = string.Join(",", ids).Replace(",", "','");
            strSql.Append(string.Format(@"SELECT * FROM BIS_EQUIPMENT WHERE ID IN('{0}')", sql));
            return this.BaseRepository().FindTable(strSql.ToString());
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
        public void SaveForm(string keyValue, EquipmentEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EquipmentEntity se = this.BaseRepository().FindEntity(keyValue);
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

        /// <summary>
        /// ��ͨ�豸�볡
        /// </summary>
        /// <param name="equipmentId">�û�Id</param>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <returns></returns>
        public int SetLeave(string equipmentId, string leaveTime, string DepartureReason)
        {
            DataItemDetailService service = new DataItemDetailService();
            leaveTime = "to_date('" + leaveTime + " 00:00:00','yyyy-mm-dd hh24:mi:ss')";
            return this.BaseRepository().ExecuteBySql(string.Format("update BIS_EQUIPMENT set state={0},DepartureTime={1},DepartureReason='{3}' where id in('{2}')", service.GetItemValue("�볧", "EQUIPMENTSTATE"), leaveTime, equipmentId.Replace(",", "','"), DepartureReason));

        }
        #endregion
    }
}
