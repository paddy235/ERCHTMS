using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using System.Data;
using ERCHTMS.Service.BaseManage;
using System;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;

namespace ERCHTMS.Service.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�������Ϣ
    /// </summary>
    public class PowerplanthandledetailService : RepositoryFactory<PowerplanthandledetailEntity>, PowerplanthandledetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PowerplanthandledetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PowerplanthandledetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (queryJson.Length > 0)
                {
                    var queryParam = queryJson.ToJObject();
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// �����¹��¼������¼ID��ȡ�¹��¼�������Ϣ�б�
        /// </summary>
        /// <param name="keyValue">�¹��¼������¼I</param>
        /// <returns></returns>
        public IList<PowerplanthandledetailEntity> GetHandleDetailList(string keyValue)
        {
            string sql = string.Format("select * from bis_powerplanthandledetail where powerplanthandleid='{0}'", keyValue);
            return this.BaseRepository().FindList(sql).ToList();
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
        public void SaveForm(string keyValue, PowerplanthandledetailEntity entity)
        {
            var res = GetEntity(keyValue);
            //���������β��Ÿ�ֵ
            if (!string.IsNullOrWhiteSpace(entity.RectificationDutyPersonId))
            {
                entity.RectificationDutyDeptId = "";
                entity.RectificationDutyDept = "";
                string[] dutyuserlist = entity.RectificationDutyPersonId.Split(',');
                for (int i = 0; i < dutyuserlist.Length; i++)
                {
                    var user = new UserInfoService().GetUserInfoEntity(dutyuserlist[i].ToString());
                    if (user != null)
                    {
                        entity.RectificationDutyDept += entity.RectificationDutyDeptId.Contains(user.DepartmentId) ? "" : user.DeptName + ",";
                        entity.RectificationDutyDeptId += entity.RectificationDutyDeptId.Contains(user.DepartmentId) ? "" : user.DepartmentId + ",";
                    }
                }
                if (!string.IsNullOrWhiteSpace(entity.RectificationDutyDeptId))
                {
                    entity.RectificationDutyDept = entity.RectificationDutyDept.Substring(0, entity.RectificationDutyDept.Length - 1);
                    entity.RectificationDutyDeptId = entity.RectificationDutyDeptId.Substring(0, entity.RectificationDutyDeptId.Length - 1);
                }
            }
            if (res == null)
            {
                entity.Create();
                entity.ApplyState = 0;
                entity.ApplyCode = DateTime.Now.ToString("yyyyMMddHHmmss");
                this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
        }
        #endregion
    }
}
