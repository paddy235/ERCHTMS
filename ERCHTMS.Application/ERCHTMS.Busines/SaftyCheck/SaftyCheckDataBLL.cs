using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Code;
using System.Data;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SaftyCheckDataBLL
    {
        private SaftyCheckDataIService service = new SaftyCheckDataService();

        #region ��ȡ����
        /// <summary>
        /// ��ȫ��������ֵ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetCheckNamePageList(Pagination pagination, string queryJson)
        {
            return service.GetCheckNamePageList(pagination, queryJson);
        }
            /// <summary>
            /// ͨ��folderId ��ȡ��Ӧ���ļ�
            /// </summary>
            /// <param name="folderId"></param>
            /// <returns></returns>
            public DataTable GetListByObject(string folderId)
        {
            return service.GetListByObject(folderId);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        public DataTable GetCheckStat(ERCHTMS.Code.Operator user, int category = 2)
        {
            return service.GetCheckStat(user, category);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaftyCheckDataEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaftyCheckDataEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȫ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<SaftyCheckDataEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }


        /// <summary>
        /// ��ҳ��������
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int[] GetCheckCount(ERCHTMS.Code.Operator user, int mode)
        {
            return service.GetCheckCount(user, mode);
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
        /// ɾ���������
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveCheckName(string keyValue)
        {
            service.RemoveCheckName(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, SaftyCheckDataEntity entity)
        {
            try
            {
                return service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int SaveCheckName(Operator user, List<CheckNameSetEntity> list)
        {
            return service.SaveCheckName(user, list);
        }
        #endregion

        #region �ֻ���
            public IEnumerable<SaftyCheckDataEntity> selectCheckExcel(Operator user)
        {
            return service.selectCheckExcel(user);
        }

        public List<DistinctArray> getDistinctGroup(string recid)
        {
            return service.getDistinctGroup(recid);
        }

        public List<DistinctArray> getDistinctGroupDj(string recid, string checkdatatype, Operator user)
        {
            return service.getDistinctGroupDj(recid, checkdatatype, user);
        }

        public DataTable selectCheckContent(string risknameid, string userAccount, string type)
        {
            return service.selectCheckContent(risknameid, userAccount, type);
        }

        public DataTable getCheckPlanList(Operator user, string ctype)
        {
            return service.getCheckPlanList(user, ctype);
        }

        public object GetCheckStatistics(Operator user, string deptCode = "")
        {
            return service.GetCheckStatistics(user, deptCode);
        }
        public DataTable GetCheckObjects(string recId, int mode = 0)
        {
            return service.GetCheckObjects(recId, mode);
        }
        public DataTable GetCheckItems(string checkObjId, string recId, int mode = 0)
        {
            return service.GetCheckItems(checkObjId, recId, mode);
        }
        public List<object> GetCheckContents(string checkId, int mode = 0)
        {
            return service.GetCheckContents(checkId, mode);
        }
        /// <summary>
        /// ��ȡ������Υ��������˳������Ϊ������Υ�£�
        /// </summary>
        /// <param name="checkId">����¼Id</param>
        /// <param name="mode">��ѯ��ʽ��0����ȡ��������¼������������Υ��������1����ȡ�����Ŀ�Ǽǵ�������Υ��������</param>
        /// <returns></returns>
        public List<int> GetHtAndWzCount(string recId, int mode)
        {
            return service.GetHtAndWzCount(recId, mode);
        }
        /// <summary>
        /// ��ȡ����еǼǵ�Υ���б�
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetWzList(string recId, int mode)
        {
            return service.GetWzList(recId, mode);
        }
        /// <summary>
        /// ��ȡ����еǼǵ������б�
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetHtList(string recId, int mode)
        {
            return service.GetHtList(recId, mode);
        }
        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public DataTable GetCheckContentInfo(string itemid)
        {
            return service.GetCheckContentInfo(itemid);
        }
        /// <summary>
        /// ��ȡ�豸��Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetEquimentInfo(string id)
        {
            return service.GetEquimentInfo(id);
        }

        /// <summary>
        /// ִ�������Լƻ������ݹ����Զ��������ƻ�
        /// </summary>
        /// <returns></returns>
        public string AutoCreateCheckPlan()
        {
            return service.AutoCreateCheckPlan();
        }
        /// <summary>
        /// �����Ƿ���ֹ�����Լƻ�����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int SetStatus(string id, int status)
        {
            return service.SetStatus(id, status);
        }
        #endregion
    }
}
