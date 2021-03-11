using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Busines.JPush;
using System.Collections;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTBaseInfoBLL
    {
        private HTBaseInfoIService service = new HTBaseInfoService();
        private ChangePlanDetailIService cpservice = new ChangePlanDetailService(); //���ļƻ�����
        private ExpirationTimeSettingIService epservice = new ExpirationTimeSettingService(); //�������ö���
        private HTChangeInfoIService chservice = new HTChangeInfoService(); //��������

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HTBaseInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        public IList<HTBaseInfoEntity> GetListByCode(string hidcode)
        {
            return service.GetListByCode(hidcode);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HTBaseInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��ȫԤ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetHidSafetyWarning(int type, string orgcode)
        {
            return service.GetHidSafetyWarning(type, orgcode);
        }

        #region ��ȡ��ҳ����ͳ��
        /// <summary>
        /// ��ȡ��ҳ����ͳ��
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="curYear"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public DataTable GetHomePageHiddenByHidType(Operator curUser, int curYear, int topNum, int qType)
        {
            return service.GetHomePageHiddenByHidType(curUser, curYear, topNum, qType);
        }
        #endregion

        #region ���ݲ��ű����ȡ
        /// <summary>
        /// ���ݲ��ű����ȡ
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="encode"></param>
        /// <param name="curYear"></param>
        /// <param name="qType"></param>
        /// <returns></returns>
        public DataTable GetHomePageHiddenByDepart(string orginezeId, string encode, string curYear, int qType)
        {
            return service.GetHomePageHiddenByDepart(orginezeId, encode, curYear, qType);
        }
        #endregion
        /// <summary>
        /// ������ȫ�����Ų���������±���
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="curdate"></param>
        /// <returns></returns>
        public DataTable GetHiddenSituationOfMonth(string deptcode, string curdate, Operator curUser)
        {
            return service.GetHiddenSituationOfMonth(deptcode, curdate, curUser);
        }

        /// <summary>
        /// Υ���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetRulerPageList(Pagination pagination, string queryJson)
        {
            return service.GetRulerPageList(pagination, queryJson);
        }


        public DataTable GetGeneralQuery(string sql, Pagination pagination)
        {
            return service.GetGeneralQuery(sql, pagination);
        }

        /// <summary>
        /// ��ȡͨ�ò�ѯ
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetGeneralQueryBySql(string sql)
        {
            return service.GetGeneralQueryBySql(sql);
        }

        /// <summary>
        /// ����ͳ��
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <param name="hidrank"></param>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsByAction(StatisticsEntity sentity)
        {
            return service.QueryStatisticsByAction(sentity);
        }


        public DataTable GetList(string checkId, string checkman, string districtcode, string workstream)
        {
            return service.GetList(checkId, checkman, districtcode, workstream);
        }

        /// <summary>
        /// ��ȡ��ǰ�û��µ���������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetDescribeListByUserId(string userId, string hiddescribe)
        {
            return service.GetDescribeListByUserId(userId, hiddescribe);
        }


        /// <summary>
        /// ��ȡ����Ԥ����Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetHiddenInfoOfWarning(Operator user, string startDate, string endDate)
        {
            return service.GetHiddenInfoOfWarning(user, startDate, endDate);
        }


        /// <summary>
        /// ��ȡָ�����µ�����ָ���¼
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public decimal GetHiddenWarning(Operator user, string startDate)
        {
            return service.GetHiddenWarning(user, startDate);
        }

        #region ��Ҫָ��(ʡ��)
        /// <summary>
        /// ��Ҫָ��(ʡ��)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetImportantIndexForProvincial(int action, Operator user)
        {
            try
            {
                return service.GetImportantIndexForProvincial(action, user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// ��ȡ˫�ع����б�
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable QueryHidWorkList(Operator curUser)
        {
            return service.QueryHidWorkList(curUser);
        }

        /// <summary>
        /// �����¼
        /// </summary>
        /// <param name="value"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable QueryHidBacklogRecord(string value, string userId)
        {
            return service.QueryHidBacklogRecord(value, userId);
        }

        /// <summary>
        /// �����ع��¼
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureHid(string num)
        {
            return service.QueryExposureHid(num);
        }

        public DataTable GetAppHidStatistics(string code, int mode, int category = 2)
        {
            return service.GetAppHidStatistics(code, mode, category);
        }

        public DataTable GetBaseInfoForApp(Pagination pagination)
        {
            return service.GetBaseInfoForApp(pagination);
        }
        /// <summary>
        /// ����ͳ�Ƶ���Ǽǵ�Υ�²��ҵ���δ���յ�Υ�������͵���Ǽǵ�Υ�µ�������
        /// </summary>
        /// <param name="currDate">ʱ��</param>
        ///  <param name="deptCode">����Code</param>
        /// <returns></returns>
        public DataTable GetLllegalRegisterNumByMonth(string currDate, string deptCode)
        {
            return service.GetLllegalRegisterNumByMonth(currDate, deptCode);
        }

        public object GetHiddenInfoOfEveryMonthWarning(Operator user, string startDate, string endDate)
        {
            return service.GetHiddenInfoOfEveryMonthWarning(user, startDate, endDate);
        }

        #region ��ȡ���������µ�������Ϣ
        /// <summary>
        /// ��ȡ���������µ�������Ϣ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHiddenByRelevanceId(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetHiddenByRelevanceId(pagination, queryJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ʡ��ͳ������
        /// <summary>
        /// ʡ��ͳ������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable QueryProvStatisticsByAction(ProvStatisticsEntity entity)
        {
            try
            {
                return service.QueryProvStatisticsByAction(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

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
        public void SaveForm(string keyValue, HTBaseInfoEntity entity)
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

        #region ��ȡ������ҳ�б�
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetHiddenBaseInfoPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetHiddenBaseInfoPageList(pagination, queryJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ��ȡ��������
        public DataTable GetHiddenByKeyValue(string keyValue)
        {
            try
            {
                return service.GetHiddenByKeyValue(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// ������ȫ����Ӧ����������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetHiddenOfSafetyCheck(string keyValue, int mode)
        {
            try
            {
                return service.GetHiddenOfSafetyCheck(keyValue, mode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion




        #region �������ļƻ�����
        /// <summary>
        /// �������ļƻ�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveChangePlan(string keyValue, ChangePlanDetailEntity entity)
        {
            try
            {
                cpservice.SaveForm(keyValue, entity);
                //������һ��
                if (string.IsNullOrEmpty(keyValue))
                {
                    HTBaseInfoEntity bentity = service.GetEntity(entity.HIDDENID);
                    HTChangeInfoEntity centity = chservice.GetEntityByHidCode(bentity.HIDCODE);
                    string pushcode = "YH017"; //���ļƻ�

                    string sql = string.Format(@"select  f.createuser  account, d.realname username,c.encode deptcode ,c.fullname deptname   from bis_htbaseinfo  a   
                                                left join sys_wftbinstance  g on a.id = g.objectid    
                                                left join sys_wftbinstancedetail f on g.currentdetailid = f.id 
                                                left  join base_department c on f.createuserdeptcode= c.encode  
                                                left  join base_user  d on f.createuser =d.account   where a.id='{0}' ", entity.HIDDENID);

                    var dt = service.GetGeneralQueryBySql(sql);


                    string pushaccount = string.Empty; //������Ա�˻�
                    string pushname = string.Empty; //������Ա����
                    string message = string.Format(@"���ã�{0} {1}��{2}��ԡ�{3}�����������ύ�����ļƻ�������ǰ������̨�˲鿴�������������ļƻ���",
                        centity.CHANGEDUTYDEPARTNAME, centity.CHANGEPERSONNAME, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), bentity.HIDDESCRIBE);

                    if (dt.Rows.Count > 0)
                    {
                        pushaccount = dt.Rows[0]["account"].ToString();
                        pushname = dt.Rows[0]["username"].ToString();
                    }
                    JPushApi.PushMessage(pushaccount, pushname, pushcode, "���ļƻ���Ϣ", message, entity.HIDDENID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ��ȡ���ļƻ�����
        /// <summary>
        /// ��ȡ���ļƻ�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ChangePlanDetailEntity GetChangePlanEntity(string keyValue)
        {
            try
            {
                return cpservice.GetEntity(keyValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region ��ȡ�������ü���

        /// <summary>
        /// ��ȡ�������ü���
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public List<ExpirationTimeSettingEntity> GetExpList(string orgId, string modulename)
        {
            try
            {
                List<ExpirationTimeSettingEntity> list = epservice.GetList("").Where(p => p.ORGANIZEID == orgId && p.MODULENAME == modulename).ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region ���浽��ʱ�����ö���
        /// <summary>
        /// ���浽��ʱ�����ö���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveExpirationTimeEntity(string keyValue, ExpirationTimeSettingEntity entity)
        {
            try
            {
                epservice.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ������ҳ-δ��������

        /// <summary>
        /// ��ȡ������ҳ-δ��������
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetNoChangeHidList(string code)
        {
            try
            {
                return service.GetNoChangeHidList(code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ��ȡ������δ���ĵ�����
        /// </summary>
        /// <param name="areaCodes">�����б�</param>
        /// <returns></returns>
        public IList GetCountByArea(List<string> areaCodes)
        {
            return service.GetCountByArea(areaCodes);
        }
        #endregion
    }
}
