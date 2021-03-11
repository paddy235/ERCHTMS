using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using ERCHTMS.Service.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�������Ʒ��
    /// </summary>
    public class LaborinfoBLL
    {
        private LaborinfoIService service = new LaborinfoService();

        #region ��ȡ����

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
        public IEnumerable<LaborinfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// �ж��Ƿ���Ի�ȡ��Ȩ��
        /// </summary>
        /// <returns></returns>
        public bool GetPer()
        {
            var deptid = OperatorProvider.Provider.Current().DeptId;
            var deptname = OperatorProvider.Provider.Current().DeptCode;
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'SelectDept'").ToList();
            foreach (var item in data)
            {
                string value = item.ItemValue;
                string[] values = value.Split('|');
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == deptname)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LaborinfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ����ids��ȡ����������������
        /// </summary>
        /// <param name="InfoId"></param>
        /// <returns></returns>
        public DataTable Getplff(string InfoId)
        {
            return service.Getplff(InfoId);
        }

        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson, string where)
        {
            return service.GetTable(queryJson, where);
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
        public void ImportSaveForm(List<LaborinfoEntity> entity, List<LaborprotectionEntity> prolist, List<LaborequipmentinfoEntity> eqlist)
        {
            try
            {
                service.ImportSaveForm(entity, prolist, eqlist);
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
        public void SaveForm(string keyValue, LaborinfoEntity entity, string json, string ID)
        {
            try
            {
                service.SaveForm(keyValue, entity, json, ID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
