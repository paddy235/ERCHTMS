using ERCHTMS.Entity.SafeReward;
using ERCHTMS.IService.SafeReward;
using ERCHTMS.Service.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Busines.SafeReward
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SaferewardBLL
    {
        private SaferewardIService service = new SaferewardService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaferewardEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaferewardEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        ///��ȡͳ�Ʊ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public DataTable GetRewardStatisticsList(string year)
        {
            return service.GetRewardStatisticsList(year);
        }

        /// <summary>
        ///��ȡ���������������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public DataTable GetRewardStatisticsTimeList(string year)
        {
            return service.GetRewardStatisticsTimeList(year);
        }


        public string GetRewardStatisticsCount(string year)
        {
            return service.GetRewardStatisticsCount(year);
        }

        public string GetRewardStatisticsTime(string year)
        {
            return service.GetRewardStatisticsTime(year);
        }


        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <returns></returns>
        public string GetRewardCode()
        {
            return service.GetRewardCode();
        }

        public string GetRewardStatisticsExcel(string year = "")
        {
            return service.GetRewardStatisticsExcel(year);
        }

        public string GetRewardStatisticsTimeExcel(string year = "")
        {
            return service.GetRewardStatisticsTimeExcel(year);
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
        public void SaveForm(string keyValue, SaferewardEntity entity)
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

        public void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity, string leaderShipId)
        {
            try
            {
                service.CommitApply(keyValue, entity, leaderShipId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object GetStandardJson()
        {
            try
            {
               return service.GetStandardJson();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Flow GetFlow(string keyValue)
        {
          return  service.GetFlow(keyValue);
        }

        public List<object> GetLeaderList()
        {
            return service.GetLeaderList();
        }


        public string GetRewardNum()
        {
            return service.GetRewardNum();
        }

        public string GetDeptPId(string deptId)
        {
            return service.GetDeptPId(deptId);
        }

        public List<object> GetSpecialtyPrincipal(string applyDeptId)
        {
            return service.GetSpecialtyPrincipal(applyDeptId);
        }

        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetAptitudeInfo(string keyValue)
        {
            return service.GetAptitudeInfo(keyValue);
        }
    }
}
