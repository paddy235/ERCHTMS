using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// �� ������ҵ�ֳ���ȫ�ܿ� 
    /// </summary>
    public class SafeworkcontrolBLL
    {
        private SafeworkcontrolIService service = new SafeworkcontrolService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafeworkcontrolEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ����״̬��ȡ�ֳ���ҵ��Ϣ
        /// </summary>
        /// <param name="State">1��ʼ 2����</param>
        /// <returns></returns>
        public IEnumerable<SafeworkcontrolEntity> GetStartList(int Stete)
        {
            return service.GetStartList(Stete);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();


            if (!queryParam["selectStatus"].IsEmpty())
            {//��ҵ����
                string Type = queryParam["selectStatus"].ToString();
                pagination.conditionJson += string.Format(" and taskType = '{0}' ", Type);
            }
            if (!queryParam["LevelName"].IsEmpty())
            {//��ҵ����
                string Type = queryParam["LevelName"].ToString();
                pagination.conditionJson += string.Format(" and DangerLevel = '{0}' ", Type);
            }
            if (!queryParam["deptCode"].IsEmpty())
            {//����
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(deptcode,'{0}')=1", deptCode);
            }
            if (!queryParam["AreaCode"].IsEmpty())
            {//���� 
                string AreaCode = queryParam["AreaCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(Taskregioncode,'{0}')=1", AreaCode);
            }
            if (!queryParam["State"].IsEmpty())
            {//��ҵ״̬ 1��ҵ�� 2��ҵ����
                string State = queryParam["State"].ToString();
                pagination.conditionJson += string.Format(" and State ={0} ", State);
            }
            //����ʱ�����ɸѡ
            if (!queryParam["startTime"].IsEmpty() || !queryParam["endTime"].IsEmpty())
            {

                string startTime = queryParam["startTime"].ToString();
                string endTime = queryParam["endTime"].ToString();
                if (!string.IsNullOrEmpty(startTime))
                {
                    pagination.conditionJson += string.Format("and ActualStartTime >= TO_Date('{0}','yyyy-mm-dd hh24:mi') ", startTime);
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    pagination.conditionJson += string.Format("and ActualStartTime <= TO_Date('{0}', 'yyyy-mm-dd hh24:mi') ", endTime);
                }

                //pagination.conditionJson +=
                //    string.Format(
                //        " and ActualStartTime >= TO_Date('{0}','yyyy-mm-dd hh24:mi') and  ActualStartTime <= TO_Date('{1}','yyyy-mm-dd hh24:mi') ",
                //        startTime, endTime);
            }
            if (!queryParam["Search"].IsEmpty())
            {//�ؼ��ֲ�ѯ
                string Search = queryParam["Search"].ToString();
                pagination.conditionJson += string.Format(" and  (workno like '%{0}%' or taskname like '%{0}%' or tasktype like '%{0}%' or deptname like '%{0}%') ", Search);
            }
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetUserPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            if (!queryParam["deptCode"].IsEmpty())
            {//����
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(t.deptcode,'{0}')=1", deptCode);
            }
            //����ʱ�����ɸѡ
            if (!queryParam["startTime"].IsEmpty() || !queryParam["endTime"].IsEmpty())
            {

                string startTime = queryParam["startTime"].ToString();
                string endTime = queryParam["endTime"].ToString();
                if (!string.IsNullOrEmpty(startTime))
                {
                    pagination.conditionJson +=
                    string.Format(
                        " and warningtime >= TO_Date('{0}','yyyy-mm-dd hh24:mi')", startTime);
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    pagination.conditionJson +=
                     string.Format(
                         " and warningtime <= TO_Date('{0}','yyyy-mm-dd hh24:mi')", endTime);
                }

                //pagination.conditionJson +=
                //    string.Format(
                //        " and warningtime >= TO_Date('{0}','yyyy-mm-dd hh24:mi') and  warningtime <= TO_Date('{1}','yyyy-mm-dd hh24:mi') ",
                //        startTime, endTime);
            }
            if (!queryParam["Search"].IsEmpty())
            {//�ؼ��ֲ�ѯ
                string Search = queryParam["Search"].ToString();
                pagination.conditionJson += string.Format(" and  (d.warningcontent like '%{0}%' or d.liablename like '%{0}%' or t.deptname like '%{0}%') ", Search);
            }
            return service.GetPageList(pagination, queryJson);
        }

        public DataTable GetWaringPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            if (!queryParam["deptCode"].IsEmpty())
            {//����
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and  instr(t.departmentcode,'{0}')=1", deptCode);
            }
            //����ʱ�����ɸѡ
            if (!queryParam["startTime"].IsEmpty() || !queryParam["endTime"].IsEmpty())
            {

                string startTime = queryParam["startTime"].ToString();
                string endTime = queryParam["endTime"].ToString();
                if (!string.IsNullOrEmpty(startTime))
                {
                    pagination.conditionJson +=
                    string.Format(
                        " and warningtime >= TO_Date('{0}','yyyy-mm-dd hh24:mi')", startTime);
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    pagination.conditionJson +=
                     string.Format(
                         " and warningtime <= TO_Date('{0}','yyyy-mm-dd hh24:mi')", endTime);
                }
            }
            if (!queryParam["Search"].IsEmpty())
            {//�ؼ��ֲ�ѯ
                string Search = queryParam["Search"].ToString();
                pagination.conditionJson += string.Format(" and  (d.warningcontent like '%{0}%' or d.liablename like '%{0}%' or t.deptname like '%{0}%') ", Search);
            }
            return service.GetPageList(pagination, queryJson);
        }



        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafeworkcontrolEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡԤ���б�����
        /// </summary>
        /// <param name="type">0��ԱԤ�� 1�ֳ�����</param>
        /// <returns></returns>
        public List<WarningInfoEntity> GetWarningInfoList(int type)
        {
            return service.GetWarningInfoList(type);
        }
        /// <summary>
        /// ��ȡ����Ԥ����Ϣ�б�
        /// </summary>
        /// <returns></returns>
        public List<WarningInfoEntity> GetWarningAllList()
        {
            return service.GetWarningAllList();
        }
        /// <summary>
        /// ��ȡ��Ա��ȫ�ܿظ���ʱ������
        /// </summary>
        /// <returns></returns>
        public List<KbsEntity> GetDayTimeIntervalUserNum()
        {
            return service.GetDayTimeIntervalUserNum();
        }

        /// <summary>
        /// ��ȡ�����е�ǰ��ʼ����ҵ
        /// </summary>
        /// <returns></returns>
        public List<SafeworkcontrolEntity> GetNowWork()
        {
            return service.GetNowWork();
        }


        /// <summary>
        /// ��ȡ���ո߷�����ҵ
        /// </summary>
        /// <returns></returns>    
        public List<SafeworkcontrolEntity> GetDangerWorkToday(string level)
        {
            return service.GetDangerWorkToday(level);
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
        public void SaveForm(string keyValue, SafeworkcontrolEntity entity)
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

        /// <summary>
        /// ������ҵ��Ա�Ƿ���������״̬
        /// </summary>
        /// <returns></returns>
        public void SaveSafeworkUserStateIofo(string workid, string userid, int state)
        {
            try
            {
                service.SaveSafeworkUserStateIofo(workid, userid, state);
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
        public void AppSaveForm(string keyValue, SafeworkcontrolEntity entity)
        {
            try
            {
                service.AppSaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        #endregion

        #region Ԥ����Ϣ
        /// <summary>
        /// ��ȡԤ����Ϣʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public WarningInfoEntity GetWarningInfoEntity(string keyValue)
        {
            try
            {
                return service.GetWarningInfoEntity(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// ����Ԥ�����������޸ģ�
        /// </summary>
        /// <param name="type">0���� 1�޸�</param>
        /// <param name="list"></param>
        public void SaveWarningInfoForm(int type, IList<WarningInfoEntity> list)
        {
            try
            {
                service.SaveWarningInfoForm(type, list);
            }
            catch (Exception er)
            {
                throw;
            }
        }
        /// <summary>
        /// ����Ԥ�����������޸ģ�
        /// </summary>
        /// <param name="type">0���� 1�޸�</param>
        /// <param name="list"></param>
        public void SaveWarningInfoForm(string keyValue, WarningInfoEntity entity)
        {
            try
            {
                service.SaveWarningInfoForm(keyValue, entity);
            }
            catch (Exception er)
            {
                throw;
            }
        }

        /// <summary>
        /// ɾ��Ԥ����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        public void SaveWarningInfoForm(string keyValue)
        {
            try
            {
                service.DelWarningInForm(keyValue);
            }
            catch (Exception er)
            {
                throw;
            }
        }
        /// <summary>
        /// ����ɾ��Ԥ����Ϣ��ͨ�������¼Id��
        /// </summary>
        /// <param name="BaseId"></param>
        public void DelBatchWarningInForm(string BaseId)
        {
            try
            {
                service.DelBatchWarningInForm(BaseId);
            }
            catch (Exception er)
            {
                throw;
            }
        }
        #endregion



        /// <summary>
        /// ��ȡ�ѽ�����Ԥ����Ϣ
        /// </summary>
        public List<WarningInfoEntity> GetBatchWarningInfoList(string WorkId)
        {
            return service.GetBatchWarningInfoList(WorkId);
        }

        public List<RiskAssessEntity> GetDistrictLevel()
        {
            return service.GetDistrictLevel();
        }
    }
}
