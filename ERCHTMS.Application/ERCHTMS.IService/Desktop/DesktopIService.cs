using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.Home;
using ERCHTMS.Entity.Common;

namespace ERCHTMS.IService.Desktop
{
    /// <summary>
    /// �� ����
    /// </summary>
    public interface DesktopIService
    {
        #region  ͨ�ð汾���쵼��ʻ��(�糧�㼶)

        #region  Ԥ��ָ�ꡢ��ȫָ��
        /// <summary>
        /// Ԥ��ָ�ꡢ��ȫָ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<DesktopPageIndex> GetPowerPlantWarningIndex(Operator user);
        #endregion

        #region ��ȡ�����ʵ��ڶ��ٵĵ糧����
        /// <summary>
        /// ��ȡ�����ʵ��ڶ��ٵĵ糧����
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rankname"></param>
        /// <returns></returns>
        DataTable GetRectificationRateUnderHowMany(Operator user, string rankname, decimal num);
        #endregion

        #region  δ�ջ�����ͳ��
        /// <summary>
        /// δ�ջ�����ͳ��
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetNoCloseLoopHidStatistics(Operator user, int mode);

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetHiddenChangeForLeaderCockpit(Operator user);
        #endregion

        #endregion

        #region ������δ�ջ�Υ��ͳ��
        /// <summary>
        ///������δ�ջ�Υ��ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        DataTable GetNoCloseLoopLllegalStatistics(Operator user);
        #endregion

        #region ������Υ��������ͳ��
        /// <summary>
        /// ������Υ��������ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetLllegalChangeForLeaderCockpit(Operator user);
        #endregion

        #region ������ҵ����/�߷�����ҵͳ��
        /// <summary>
        /// ������ҵ����/�߷�����ҵͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetHighRiskWorkingForLeaderCockpit(Operator user, int mode);

        #endregion

        #endregion

        #region ��ȡ����
        /// <summary>
        /// �߷�����ҵ����ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetWorkTypeChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ�ճ���ȫ���ͳ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetSafetyCheckOfEveryDay(ERCHTMS.Code.Operator user);

        #region ��ȡ���������ͼ(������������ȫ�������)
        /// <summary>
        /// ��ȡ���������ͼ(������������ȫ�������)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetTendencyChart(ERCHTMS.Code.Operator user);
        #endregion


        /// <summary>
        /// ����ͳ��ͼ��
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        string GetHTChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ����������ͳ���������
        /// </summary>
        /// <param name="orgCode">��������</param>
        /// <returns></returns>
        DataTable GetProjectChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// �����̷��յȼ�ͳ���������
        /// </summary>
        /// <param name="orgCode">��������</param>
        /// <returns></returns>
        DataTable GetProjectChartByLevel(ERCHTMS.Code.Operator user);
        /// <summary>
        /// �����Ա�����仯����ͼ
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        string GetProjectPersonChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ����������ͳ������
        /// </summary>
        /// <param name="orgCode">��������</param>
        /// <returns></returns>
        DataTable GetHTTypeChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��鷢�ֵ�����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetCheckHtNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ���������仯����ͼ 
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        string GetHTChangeChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ֪ͨ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetNotices(ERCHTMS.Code.Operator user);


        /// <summary>
        /// ��ȡһ�Ÿڴ�������
        /// </summary>        
        /// <returns></returns>
        DataTable GetScreenTitle();

        /// <summary>
        /// ������ļ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetAllotCheckCount(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��ȫ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetMeets(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��ȫ��̬
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetTrends(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��ڰ�
        /// </summary>
        /// <param name="user"></param>
        ///  <param name="mode">0:���1:�ڰ�</param>
        /// <returns></returns>
        DataTable GetRedBlack(ERCHTMS.Code.Operator user, int mode);
        /// <summary>
        /// ������̸ſ�ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetProjectStat(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡΣ��Դ����������Ϊ���������ش�Σ��Դ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetDangerSourceNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��ԱΥ����Ϣ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetWZInfo(Pagination pagination, string queryJson); 

        DataTable GetWZInfo(string userid, int mode = 0);
        DataTable GetWZInfoByUserId(string userId,int mode=0);
        /// <summary>
        /// ��ȡδǩ���Ļ�������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetMeetNum(string userId);
        /// <summary>
        /// ��ȡʩ����Σ�󹤳���
        /// <param name="user"></param>
        /// </summary>
        /// <returns></returns>
        int GetProjectNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��������(����Ϊ���������ش�����������һ����������)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetHtNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ�ش��������(����Ϊ���������ش�����������ϴ����������һ������������ͷ�������)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetRiskNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ�������������Ϣ�����������������Ա�ڳ���������������������ڳ������λ���������½������Ա�������λΥ�´�����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetWBProjectNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ�߷�����ҵ(����Ϊ�����������ල��������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetWorkNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��Σ��ҵ��ȫ���֤�������죨����Ϊ�ߴ���ҵ�����ص�װ��ҵ��������ҵ����·��ҵ��������ҵ��ä������ҵ�����޿ռ���ҵ���豸����������ҵ������ʩȷ�ϡ���ͣ�硢�������������ա����͵磩
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetJobSafetyCardNum(ERCHTMS.Code.Operator user);

        /// <summary>
        /// ��ȡ���ּ�ͳ�ƣ������ա�����ˣ�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetScaffoldNum(ERCHTMS.Code.Operator user);


        /// <summary>
        /// ��ȡ����ˮ�����(��)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetFireWaterNum(ERCHTMS.Code.Operator user);

        /// <summary>
        /// ��ȡ�����豸����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetEquimentNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡΥ��������Ϣ������Ϊ��Υ��������������׼�������ġ������ա�����δ�����������������������������Ƶ�Υ�£�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetlllegalNum(ERCHTMS.Code.Operator user);

        /// <summary>
        /// Ӧ����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetDrillRecordNum(ERCHTMS.Code.Operator user);

                /// <summary>
        /// ��ȡ����������Ϣ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
         List<int> GetQuestionNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ�¹�������Ϣ������Ϊ�¹�����������������������Ա��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetAccidentNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡΥ��������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        decimal GetlllegalRatio(ERCHTMS.Code.Operator user);
        /// <summary>
        /// �����յȼ���ͼ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetRiskCounChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��ȫ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetWorks(ERCHTMS.Code.Operator user);
        /// <summary>
        /// �������ڻ�ȡ���˰�ȫ������¼
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">ʱ��</param>
        /// <returns></returns>
        DataTable GetWorkInfoByTime(ERCHTMS.Code.Operator user, string time);
        /// <summary>
        /// ��ȡ������������յȼ���Ϣ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        DataTable GetAreaStatus(ERCHTMS.Code.Operator user, string areaCode, int mode = 1);

        /// <summary>
        /// ��ȡ������������յȼ���Ϣ(����ʲ�汾)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        List<AreaRiskLevel> GetKbsAreaStatus();


        /// <summary>
        /// ��ȡ������������յȼ���Ϣ(���Źܿ����ġ�������ɫͼ���汾)
        /// </summary>
        /// <returns></returns>
        List<AreaRiskLevel> GetKMAreaStatus();

        int GetSafetyChangeNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȫԤ����Ŀ��ʡ��˾����������Ϊ�����ش������ĵ糧��������������С��80%�ĵ糧��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<decimal> GetWarnItems(ERCHTMS.Code.Operator user);

        /// <summary>
        /// ������Ŀ���(ʡ��˾��)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<decimal> GetRiskAnalyzeItems(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��ȫ�����������Ϊ��ȫ����������ִ�еİ�ȫ�������ʡ��˾����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetSafetyCheckForGroup(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ��ȫ�����������Ϊ��ȫ����������ִ�еİ�ȫ�������ʡ��˾����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetHtForGroup(ERCHTMS.Code.Operator user);

        /// <summary>
        /// �糧��������
        /// </summary>
        /// <param name="deptCode">ʡ��˾deptCode</param>
        /// <param name="mode">������ʽ��0������������������1��������������������2����δ�ջ�����������</param>
        /// <returns></returns>
        DataView GetRatioDataOfFactory(ERCHTMS.Code.Operator user, int mode = 0);
        /// <summary>
        /// �糧��ȫ����������Ϣͳ��
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        List<decimal> GetHt2CheckOfFactory(string orgId, string time, string orgCode = "");

        /// <summary>
        /// ��ȡ��ǰ�û���������������ָ����Ŀ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetDeptDataSet(ERCHTMS.Code.Operator user, string itemType);

        /// <summary>
        /// ��ȡ�糧����������
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<decimal> GetHtZgl(string orgId);

        /// <summary>
        /// ��ȡ�������߷��յ�ͳ����Ŀ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetHtOrRiskItems(ERCHTMS.Code.Operator user, int mode);
        /// <summary>
        /// ��ȡʡ��˾�·��İ�ȫ�������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetSafetyCheckTask(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��������ʵʱ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<RealTimeWorkModel> GetRealTimeWork(Operator user);

        /// <summary>
        /// ���N����Ԥ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<RealTimeWorkModel> GetWarningCenterWork(Operator user);
        int GetJobApprovalFormNum(Operator user);
        #endregion

        #region MyRegion
        /// <summary>
        /// ��ȡָ��ֵ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        decimal GetSafetyAssessedValue(SafetyAssessedArguments entity);
        #endregion

        #region MyRegion
        /// <summary>
        /// ��ȡָ�����
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        List<SafetyAssessedModel> GetSafetyAssessedData(SafetyAssessedArguments argument);
        #endregion

        #region ��ȡģ���¶�Ӧ��ָ��
        /// <summary>
        /// ��ȡģ���¶�Ӧ��ָ��
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        List<SafetyAssessedChildModel> GetSafetyAssessedChildData(SafetyAssessedArguments argument, List<ClassificationIndexEntity> list);
        #endregion

        
        #region ��ȡ�ع���Ϣ
        /// <summary>
        /// ��ȡ�ع���Ϣ
        /// </summary>
        /// <returns></returns>
        DataTable GetExposureInfo(ERCHTMS.Code.Operator user);
        #endregion

        /// <summary>
        /// ����ƻ���������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetSafeMeasureNum(Operator user);

        /// <summary>
        /// ���纺���ԽӴ������
        /// </summary>
        /// <param name="entity"></param>
        void GdhcDbsxSyncJS(GdhcDbsxEntity entity);
        /// <summary>
        /// ���纺���ԽӴ����Ѱ�
        /// </summary>
        /// <param name="entity"></param>
        void GdhcDbsxSyncYB(GdhcDbsxEntity entity);
        /// <summary>
        /// ���纺���ԽӴ�����
        /// </summary>
        /// <param name="entity"></param>
        void GdhcDbsxSyncBJ(GdhcDbsxEntity entity);
    }
}
