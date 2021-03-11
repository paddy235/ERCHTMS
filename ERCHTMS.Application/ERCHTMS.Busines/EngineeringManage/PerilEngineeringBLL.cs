using ERCHTMS.Entity.EngineeringManage;
using ERCHTMS.IService.EngineeringManage;
using ERCHTMS.Service.EngineeringManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.EngineeringManage
{
    /// <summary>
    /// �� ����Σ�󹤳̹���
    /// </summary>
    public class PerilEngineeringBLL
    {
        private PerilEngineeringIService service = new PerilEngineeringService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PerilEngineeringEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PerilEngineeringEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, PerilEngineeringEntity entity)
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

        #region ͳ��
        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringCount(string year = "")
        {
            return service.GetEngineeringCount(year);
        }

        /// <summary>
        ///��ȡͳ�Ʊ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringList(string year = "")
        {
            return service.GetEngineeringList(year);
        }


        /// <summary>
        ///��ȡʩ������,��������ͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringFile(string year = "")
        {
            return service.GetEngineeringFile(year);
        }

        /// <summary>
        ///��ȡʩ������,��������ͳ�����ݣ����
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringFileGrid(string year = "")
        {
            return service.GetEngineeringFileGrid(year);
        }

        /// <summary>
        ///Σ�󹤳�������ͳ��
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringCase(string year = "")
        {
            return service.GetEngineeringCase(year);
        }

        /// <summary>
        ///Σ�󹤳�������ͳ��(���)
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringCaseGrid(string year = "")
        {
            return service.GetEngineeringCaseGrid(year);
        }

        /// <summary>
        ///��λ�ڲ����������λ�Ա�
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringContrast(string year = "", string month = "")
        {
            return service.GetEngineeringContrast(year, month);
        }

        /// <summary>
        ///��λ�ڲ����������λ�Աȣ����
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringContrastGrid(string year = "", string month = "")
        {
            return service.GetEngineeringContrastGrid(year, month);
        }
        #endregion

        #region ʡ��ͳ��
        /// <summary>
        ///���糧��λ�Ա�
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public string GetEngineeringContrastForSJ(string year = "")
        {
            return service.GetEngineeringContrastForSJ(year);
        }

        /// <summary>
        /// ���糧��λ�Աȱ��
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public DataTable GetEngineeringContrastGridForSJ(string year = "")
        {
            return service.GetEngineeringContrastGridForSJ(year);
        }

        /// <summary>
        /// �������ͳ�Ʊ��
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public DataTable GetEngineeringCategoryGridForSJ(string year = "")
        {
            return service.GetEngineeringCategoryGridForSJ(year);
        }

        /// <summary>
        /// �������ͼ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetEngineeringCategoryForSJ(string year = "")
        {
            return service.GetEngineeringCategoryForSJ(year);
        }

        /// <summary>
        /// �¶�����ͼ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetEngineeringMonthForSJ(string year = "")
        {
            return service.GetEngineeringMonthForSJ(year);
        }

        /// <summary>
        /// �¶����Ʊ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetEngineeringMonthGridForSJ(string year = "")
        {
            return service.GetEngineeringMonthGridForSJ(year);
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <returns></returns>
        public DataTable GetEngineeringType()
        {
            return service.GetEngineeringType();
        }
        #endregion

        #region �б�ҳ���ͳ��
        public string GetPeril(string code = "", string st = "", string et = "", string keyword = "")
        {
            return service.GetPeril(code, st, et, keyword);
        }

        public string GetPerilForSJIndex(string queryJson)
        {
            return service.GetPerilForSJIndex(queryJson);
        }
        #endregion

        #region app�ӿ�
        public DataTable GetPerilEngineeringList(string sqlwhere)
        {
            return service.GetPerilEngineeringList(sqlwhere);
        }
        #endregion
    }
}
