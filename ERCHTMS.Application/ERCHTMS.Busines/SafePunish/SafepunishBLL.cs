using ERCHTMS.Entity.SafePunish;
using ERCHTMS.IService.SafePunish;
using ERCHTMS.Service.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Busines.SafePunish
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    public class SafepunishBLL
    {
        private SafepunishIService service = new SafepunishService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafepunishEntity> GetList(string queryJson)
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
        public SafepunishEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }



        public string GetPunishStatisticsCount(string year, string statMode)
        {
            return service.GetPunishStatisticsCount(year, statMode);
        }

        public string GetPunishStatisticsList(string year, string statMode)
        {
            return service.GetPunishStatisticsList(year, statMode);
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
        /// <param name="kpiEntity">������Ϣ</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafepunishEntity entity ,SafekpidataEntity kpiEntity)
        {
            try
            {
                service.SaveForm(keyValue, entity, kpiEntity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.CommitApply(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Flow GetFlow(string keyValue)
        {
          return  service.GetFlow(keyValue);
        }

        public string GetPunishCode()
        {
           return service.GetPunishCode();
        }

        public string GetPunishNum()
        {
            return service.GetPunishNum();
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
