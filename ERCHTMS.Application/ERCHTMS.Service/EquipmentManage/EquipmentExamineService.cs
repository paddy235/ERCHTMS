using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// �� ���������豸�����¼
    /// </summary>
    public class EquipmentExamineService : RepositoryFactory<EquipmentExamineEntity>, EquipmentExamineIService
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
        public IEnumerable<EquipmentExamineEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EquipmentExamineEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, EquipmentExamineEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EquipmentExamineEntity ee = this.BaseRepository().FindEntity(keyValue);
                if (ee == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    SavEquipmentEntity(entity);
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
                SavEquipmentEntity(entity);
            }
        }
        public void SavEquipmentEntity(EquipmentExamineEntity entity)
        {

            try
            {
                //���������´μ��ʱ�䡢��ⵥλ��
                IRepository db = new RepositoryFactory().BaseRepository();
                SpecialEquipmentEntity se = db.FindEntity<SpecialEquipmentEntity>(entity.EquipmentId);
                if (se != null)
                {
                    se.CheckDate = entity.ExamineDate;
                    se.CheckDateCycle = entity.ExaminePeriod.ToString();
                    if (!string.IsNullOrEmpty(entity.ExaminePeriod.Value.ToString()))
                    {
                        se.NextCheckDate = entity.ExamineDate.Value.AddDays(entity.ExaminePeriod.Value);
                    }
                    //���鵥λ
                    se.ExamineUnit = entity.ExamineUnit;
                    se.ReportNumber = entity.ReportNumber;
                    se.ExamineVerdict = entity.ExamineVerdict;
                    db.Update<SpecialEquipmentEntity>(se);
                }
            }
            catch { }
        }
        #endregion
    }
}
