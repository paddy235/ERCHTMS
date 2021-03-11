using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.IService.HazardsourceManage;
using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Service.HazardsourceManage;

namespace ERCHTMS.Busines.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ��Դ��ʶ����
    /// </summary>
    public class HazardsourceBLL
    {
        private IHazardsourceService service = new HazardsourceService();

        #region ��ȡ����
        public DataTable FindTableBySql(string sql)
        {
            return service.FindTableBySql(sql);

        }
        /// <summary>
        /// ִ��sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteBySql(string sql)
        {
            return service.ExecuteBySql(sql);

        }

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazardsourceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazardsourceEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HazardsourceEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
