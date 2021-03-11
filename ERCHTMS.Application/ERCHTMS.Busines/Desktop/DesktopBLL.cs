using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.Desktop;
using ERCHTMS.IService.Desktop;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using System.Linq;
using System.Collections;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Busines.SafeReward;
using ERCHTMS.Busines.SafePunish;
using ERCHTMS.Busines.ComprehensiveManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PowerPlantInside;
using FrameWork.Extension;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Busines.WorkPlan;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Entity.Home;
using ERCHTMS.Busines.SafetyWorkSupervise;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Entity.CarManage;
using Newtonsoft.Json;
using ERCHTMS.Entity.Common;

namespace ERCHTMS.Busines.Desktop
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class DesktopBLL
    {
        private DesktopIService service = new DesktopService();


        #region  ͨ�ð汾���쵼��ʻ��(�糧�㼶)

        #region  Ԥ��ָ�ꡢ��ȫָ��
        /// <summary>
        /// Ԥ��ָ�ꡢ��ȫָ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DesktopPageIndex> GetPowerPlantWarningIndex(Operator user)
        {
            //ͨ�ó����쵼��ʻ��ָ��
            List<DesktopPageIndex> list = service.GetPowerPlantWarningIndex(user);

            #region ������̹���
            var data = new OutprojectblacklistBLL().ToAuditOutPeoject(user);
            var data1 = new OutprojectblacklistBLL().ToIndexData(user);
            //data:  0 ��λ�����������(��) , 1 ��Ա������� , 2 ������������� ,3 �����豸,4 ��ͨ�豸, 5 �볡���,6 ������������(��);
            //data1: 0 ��֤�� ,1 ��ͬ ,2 Э�� ,3 ��ȫ��������, 4 ��(��)����,5 ��ȫ����
            #region ������̹���

            DesktopPageIndex WBWF001 = new DesktopPageIndex();
            WBWF001.code = "WBWF001";
            WBWF001.name = "��ͬЭ��";
            WBWF001.modulecode = "WBWFMG-01";
            WBWF001.modulename = "�����������";
            WBWF001.count = data1[1].ToString(); // ��ͬ
            WBWF001.isdecimal = false;
            list.Add(WBWF001);

            DesktopPageIndex WBWF002 = new DesktopPageIndex();
            WBWF002.code = "WBWF002";
            WBWF002.name = "��ͬЭ��";
            WBWF002.modulecode = "WBWFMG-01";
            WBWF002.modulename = "�����������";
            WBWF002.count = data1[2].ToString(); // Э��
            WBWF002.isdecimal = false;
            list.Add(WBWF002);

            DesktopPageIndex WBWF003 = new DesktopPageIndex();
            WBWF003.code = "WBWF003";
            WBWF003.name = "��λ�������";
            WBWF003.modulecode = "WBWFMG-01";
            WBWF003.modulename = "�����������";
            WBWF003.count = data[0].ToString(); // ��λ�����������
            WBWF003.isdecimal = false;
            list.Add(WBWF003);

            DesktopPageIndex WBWF004 = new DesktopPageIndex();
            WBWF004.code = "WBWF004";
            WBWF004.name = "��Ա�������";
            WBWF004.modulecode = "WBWFMG-01";
            WBWF004.modulename = "�����������";
            WBWF004.count = data[1].ToString(); // ��Ա�������
            WBWF004.isdecimal = false;
            list.Add(WBWF004);

            DesktopPageIndex WBWF005 = new DesktopPageIndex();
            WBWF005.code = "WBWF005";
            WBWF005.name = "��֤��";
            WBWF005.modulecode = "WBWFMG-01";
            WBWF005.modulename = "�����������";
            WBWF005.count = data1[0].ToString(); // ��֤��
            WBWF005.isdecimal = false;
            list.Add(WBWF005);


            DesktopPageIndex WBWF006 = new DesktopPageIndex();
            WBWF006.code = "WBWF006";
            WBWF006.name = "��ȫ��������";
            WBWF006.modulecode = "WBWFMG-01";
            WBWF006.modulename = "�����������";
            WBWF006.count = data1[3].ToString(); // ��ȫ��������
            WBWF006.isdecimal = false;
            list.Add(WBWF006);


            DesktopPageIndex WBWF007 = new DesktopPageIndex();
            WBWF007.code = "WBWF007";
            WBWF007.name = "��������";
            WBWF007.modulecode = "WBWFMG-01";
            WBWF007.modulename = "�����������";
            WBWF007.count = data[2].ToString(); // ��������
            WBWF007.isdecimal = false;
            list.Add(WBWF007);

            DesktopPageIndex WBWF008 = new DesktopPageIndex();
            WBWF008.code = "WBWF008";
            WBWF008.name = "����������";
            WBWF008.modulecode = "WBWFMG-01";
            WBWF008.modulename = "�����������";
            WBWF008.count = data[3].ToString(); // ����������-�����豸����
            WBWF008.isdecimal = false;
            list.Add(WBWF008);

            DesktopPageIndex WBWF009 = new DesktopPageIndex();
            WBWF009.code = "WBWF009";
            WBWF009.name = "����������";
            WBWF009.modulecode = "WBWFMG-01";
            WBWF009.modulename = "�����������";
            WBWF009.count = data[4].ToString(); // ����������-��ͨ�豸����
            WBWF009.isdecimal = false;
            list.Add(WBWF009);


            DesktopPageIndex WBWF010 = new DesktopPageIndex();
            WBWF010.code = "WBWF010";
            WBWF010.name = "�볡���";
            WBWF010.modulecode = "WBWFMG-01";
            WBWF010.modulename = "�����������";
            WBWF010.count = data[5].ToString(); // �볡���
            WBWF010.isdecimal = false;
            list.Add(WBWF010);

            DesktopPageIndex WBWF011 = new DesktopPageIndex();
            WBWF011.code = "WBWF011";
            WBWF011.name = "��������";
            WBWF011.modulecode = "WBWFMG-01";
            WBWF011.modulename = "�����������";
            WBWF011.count = data[6].ToString(); // ��������
            WBWF011.isdecimal = false;
            list.Add(WBWF011);

            DesktopPageIndex WBWF012 = new DesktopPageIndex();
            WBWF012.code = "WBWF012";
            WBWF012.name = "��(��)����";
            WBWF012.modulecode = "WBWFMG-01";
            WBWF012.modulename = "�����������";
            WBWF012.count = data1[4].ToString(); // ��(��)����
            WBWF012.isdecimal = false;
            list.Add(WBWF012);


            DesktopPageIndex WBWF013 = new DesktopPageIndex();
            WBWF013.code = "WBWF013";
            WBWF013.name = "��ȫ����";
            WBWF013.modulecode = "WBWFMG-01";
            WBWF013.modulename = "�����������";
            WBWF013.count = data1[5].ToString(); //��ȫ����
            WBWF013.isdecimal = false;
            list.Add(WBWF013);

            #endregion
            #endregion

            return list;
        }
        #endregion

        #region ��ȡ�����ʵ��ڶ��ٵĵ糧����
        /// <summary>
        /// ��ȡ�����ʵ��ڶ��ٵĵ糧����
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rankname"></param>
        /// <returns></returns>
        public DataTable GetRectificationRateUnderHowMany(Operator user, string rankname, decimal num)
        {
            try
            {
                return service.GetRectificationRateUnderHowMany(user, rankname, num);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region  δ�ջ�����ͳ��
        /// <summary>
        /// δ�ջ�����ͳ��
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetNoCloseLoopHidStatistics(Operator user, int mode)
        {
            return service.GetNoCloseLoopHidStatistics(user, mode);
        }

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetHiddenChangeForLeaderCockpit(Operator user)
        {
            return service.GetHiddenChangeForLeaderCockpit(user);
        }
        #endregion

        #region ������δ�ջ�Υ��ͳ��
        /// <summary>
        ///������δ�ջ�Υ��ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public DataTable GetNoCloseLoopLllegalStatistics(Operator user)
        {
            return service.GetNoCloseLoopLllegalStatistics(user);
        }
        #endregion

        #region ������Υ��������ͳ��
        /// <summary>
        /// ������Υ��������ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetLllegalChangeForLeaderCockpit(Operator user)
        {
            return service.GetLllegalChangeForLeaderCockpit(user);
        }
        #endregion

        #endregion

        #region ������ҵ����/�߷�����ҵͳ��
        /// <summary>
        /// ������ҵ����/�߷�����ҵͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetHighRiskWorkingForLeaderCockpit(Operator user, int mode)
        {
            return service.GetHighRiskWorkingForLeaderCockpit(user, mode);
        }
        #endregion

        #region  �����쵼��ʻ����������(ͨ�ð汾)
        /// <summary>
        /// �����쵼��ʻ����������(ͨ�ð汾)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemType"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetDataSetForCommon(ERCHTMS.Code.Operator user, string itemType, int mode = 0)
        {
            DataTable dt = GetDeptDataSet(user, itemType);

            if (dt.Rows.Count > 0)
            {
                List<DesktopPageIndex> list = itemType == "SSJK" ? GetPowerPlantWarningIndex(user) : null;
                dt.Columns.Add("Num"); //��������
                dt.Columns.Add("Code");//��Ŀ����
                dt.Columns.Add("TypeCode");//�������
                foreach (DataRow dr in dt.Rows)
                {
                    var entity = list.Where(p => p.code == dr["itemcode"].ToString()).FirstOrDefault();
                    if (null != entity)
                    {
                        dr["Num"] = entity.count;
                        dr["Code"] = entity.code;
                        dr["TypeCode"] = entity.modulecode;
                        if (!string.IsNullOrWhiteSpace(dr["callback"].ToString()))
                        {
                            dr["callback"] = dr["callback"].ToString().Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Url}", dr["address"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(dr["itemstyle"].ToString()))
                        {
                            dr["itemstyle"] = dr["itemstyle"].ToString().Replace("{Callback}", dr["Callback"].ToString()).Replace("{Icon}", dr["Icon"].ToString()).Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Num}", dr["Num"].ToString()).Replace("{Url}", dr["address"].ToString()).Replace("{Code}", entity.code);
                        }
                    }
                }
            }
            return dt;
        }
        #endregion


        #endregion

        public string GetWorkTypeChart(ERCHTMS.Code.Operator user)
        {
            return service.GetWorkTypeChart(user);
        }
        public DataTable GetFactoryCheckListForGroup(Operator user)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select encode deptcode ,fullname  deptname,count(1) num from base_department d
 left join BIS_SAFTYCHECKDATARECORD r on d.departmentid=r.CheckedDepartID where 
 datatype=0 and  nature='����' and deptcode like '{0}%'   group by encode,fullname", user.OrganizeCode));
            return dt;
        }

        /// <summary>
        /// �Ƿ�����ҵͨ�ð�
        /// </summary>
        /// <returns></returns>
        public bool IsGeneric()
        {
            string industry = OperatorProvider.Provider.Current().Industry;
            if (industry.IsNullOrEmpty() || industry == "����") //���Ϊ�� ����Ϊ���� ��Ϊ����˫�� �������Ϊͨ��˫��
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region ��ȡ�������߷��յ�ͳ����Ŀ
        /// <summary>
        /// ��ȡ�������߷��յ�ͳ����Ŀ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetHtOrRiskItems(ERCHTMS.Code.Operator user, int mode)
        {
            return service.GetHtOrRiskItems(user, mode);
        }
        #endregion

        /// <summary>
        /// ��ȡ�ճ���ȫ���ͳ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetSafetyCheckOfEveryDay(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyCheckOfEveryDay(user);
        }
        /// <summary>
        /// ʡ���鿴��ȫ���
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetCheckListForGroup(ERCHTMS.Code.Operator user)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select encode deptcode ,fullname  deptname,count(1) from base_department d
 left join BIS_SAFTYCHECKDATARECORD r on d.departmentid=r.CheckedDepartID where 
 datatype=0 and  nature='����' and deptcode like '{0}%'   group by encode,fullname", user.OrganizeCode));
            return dt;
        }



        /// <summary>
        /// ��ҳʵʱ�������ָ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Hashtable GetMonitorData(ERCHTMS.Code.Operator user, string itemType = "SSJK")
        {
            List<string> lstItems = GetDeptDataSet(user, itemType).AsEnumerable().Select(t => t.Field<string>("itemcode")).ToList();
            Hashtable data = new Hashtable();
            HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
            var dt = htbaseinfobll.QueryHidWorkList(user);
            decimal zdnum = 0;
            foreach (DataRow rows in dt.Rows) 
            {
                string itemvalue = rows["itemvalue"].ToString();
                if (itemvalue.Contains("�ش�����")) 
                {
                    zdnum += Convert.ToDecimal(rows["total"].ToString());
                }
            }
            data.Add("HTZDYH", zdnum);//�ش�������
            data.Add("HTYZG", decimal.Parse(dt.Rows[0][3].ToString()));//������������
            data.Add("HTWZG", decimal.Parse(dt.Rows[0][4].ToString()));//δ����������
            data.Add("HTYQWZG", decimal.Parse(dt.Rows[0][5].ToString()));//����δ����������
            data.Add("HTZGL", decimal.Parse(dt.Rows[0][6].ToString()));//����������
            List<int> list = GetHtNum(user);
            if (itemType == "AQZB")
            {
                data.Add("HTSUM", list[0]);//������
            }
            else
            {
                data.Add("HTSUM", decimal.Parse(dt.Rows[0][2].ToString()));//������
            }
            data.Add("HTYQZGL", list[3]);//��������������

            if (lstItems.Contains("DSSUM"))
            {
                list = GetDangerSourceNum(user);
                data.Add("DSSUM", list[0]);//Σ��Դ����
                data.Add("DSZD", list[1]);//�ش�Σ��Դ��
            }
           
            if(lstItems.Contains("SGSUM"))
            {
                list = GetAccidentNum(user);
                data.Add("SGSUM", list[0]);//�¹�����
                data.Add("SGSW", list[1]);//�¹���������
                data.Add("SGZS", list[2]);//�¹���������
            }
            SaftyCheckDataBLL saftycheckdatabll = new SaftyCheckDataBLL();
            var dtCheck = saftycheckdatabll.GetCheckStat(user);
            data.Add("CKSUM", decimal.Parse(dtCheck.Rows[0][0].ToString()));//��ȫ������
            //�ճ���ȫ������
            string rcNum = GetSafetyCheckOfEveryDay(user);
            data.Add("CKRC", int.Parse(rcNum));//�ճ�������
            data.Add("CHFXHT", decimal.Parse(GetCheckHtNum(user).ToString()));//��鷢��������

            list = GetWBProjectNum(user);
            data.Add("WBZJSUM", list[0]);//�ڽ����������
            data.Add("WBZCRS", list[1]);//��������ڳ�����
            data.Add("WBGCSUM", list[2]);//�����������
            data.Add("WBDW", list[3]);//�ڳ������λ��
            data.Add("WBBYXJ", list[4]);//�����½������Ա
            data.Add("WBWZNUM", list[5]);//�����λΥ�´���



            list = GetRiskNum(user);
            data.Add("FXSUM", list[0]);//��������
            data.Add("FXZD", list[1]);//�ش������

                list = GetWorkNum(user);
                data.Add("ZYSUM", list[3]);//�߷�����ҵ����
                data.Add("ZYJX", list[4]);//���ڽ��еĸ߷�����ҵ��
                data.Add("ZYJR", list[7]);//���ո߷�����ҵ
            if(lstItems.Contains("JRGWZYZS"))
            {
                data.Add("JRGWZYZS", GetTodayWorkForDangerJob()); //���ո�Σ��ҵ
            }

            if (lstItems.Contains("SCHSUM"))
            {
                list = GetSafetyCheckForGroup(user);
                data.Add("SCHSUM", list[0]);//ʡ��˾��ȫ������
                data.Add("SCHRW", list[2]);//ʡ��˾�·��������糧�ļ��������
            }

            if (lstItems.Contains("SCHFXHT"))
            {
                list = GetHtForGroup(user);
                data.Add("SCHFXHT", list[3]);//ʡ��˾����������
                data.Add("SHTWWCZDYH", list[4]);//δ��������ش�������
                data.Add("SHTYQWZG", list[2]); //����δ����������
            }

            //if (lstItems.Contains("SHTZGL"))
            //{
                List<decimal> list1 = GetWarnItems(user);
                data.Add("SHTZGL", list1[2]);//����������
                data.Add("SHT80", list1[1]); //���������ʵ���80%�ĵ糧
                data.Add("CZZDYHDC", list1[3]); //�����ش������ĵ糧
            //}

            var datas = new HTBaseInfoBLL().QueryHidBacklogRecord("10", user.UserId);
            if (datas.Rows.Count == 2)
            {
                if (datas.Rows[0]["serialnumber"].ToString() == "1")
                {
                    int uploadHtNum = int.Parse(datas.Rows[0]["pnum"].ToString());
                    data.Add("HTWSC", uploadHtNum); //���ϴ�������
                }

            }
            if (lstItems.Contains("YJFXCGSGDC"))
            {
                var fxlist = GetRiskAnalyzeItems(user);
                data.Add("YJFXCGSGDC", fxlist[0]); //һ�����ճ���3���ĵ糧
            }
            list = GetlllegalNum(user);//Υ����Ϣ
            data.Add("WZSUM", list[0]);//Υ������
            data.Add("WZWZG", list[5]);//δ���ĵ�Υ��
            data.Add("WZYQ", list[4]);//����δ����Υ������
            if (lstItems.Contains("WZZGL"))
            {
                data.Add("WZZGL", GetlllegalRatio(user));//Υ��������
            }

            int count=0;
            if (lstItems.Contains("SBTZ"))
            {
                count = GetEquimentNum(user);//�����豸����
                data.Add("SBTZ", count);
            }
            if (lstItems.Contains("WDGC"))
            {
                count = GetProjectNum(user);//ʩ���е�Σ�󹤳�����
                data.Add("WDGC", count);
            }

            if (lstItems.Contains("JRLSGC"))
            {
                count = new WorkMeetingBLL().GetTodayTempProject(user);//������ʱ�������
                data.Add("JRLSGC", count);
            }
            if(lstItems.Contains("ZCWBNUM"))
            {
                count = GetWBUsersCount(user);//�ڳ������������(����ɽ)
                data.Add("ZCWBNUM", count);
            }
            return data;
        }
        /// <summary>
        ///��ȡ�ڳ������������(����ɽ)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetWBUsersCount(ERCHTMS.Code.Operator user)
        {
            string softName = BSFramework.Util.Config.GetValue("SoftName").ToLower() ;
            int count = 0;
            if (softName.StartsWith("xss"))
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtCards = deptBll.GetDataTable(string.Format("select distinct idcard from XSS_ENTERRECORD where remark='0' and idcard in(select identifyid from base_user where isepiboly='1')"));
                List<string> list = new List<string>();
                foreach (DataRow dr in dtCards.Rows)
                {
                    DataTable dtRows = deptBll.GetDataTable(string.Format("select *from (select remark from XSS_ENTERRECORD where idcard='{0}' order by time desc) where rownum=1", dr[0].ToString()));
                    if (dtRows.Rows.Count>0)
                    {
                        if (dtRows.Rows[0][0].ToString() == "0")
                        {
                            if (!list.Contains(dr[0].ToString()))
                            {
                                count++;
                                list.Add(dr[0].ToString());
                            }

                        }
                    }
                   
                }
            }
            return count;
        }
        /// <summary>
        /// ������ļ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetAllotCheckCount(ERCHTMS.Code.Operator user)
        {
            return service.GetAllotCheckCount(user);
        }
        /// <summary>
        /// ��ҳ��������ͳ��ָ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Hashtable GetWorkData(ERCHTMS.Code.Operator user, int mode = 0)
        {
            List<string> arrItems = GetDeptDataSet(user, "DBSX").AsEnumerable().Select(t=>t.Field<string>("itemcode")).ToList();

            Hashtable data = new Hashtable();
            string execCheckNum = "0"; //��ִ�еİ�ȫ�����
            string changeplanHiddenNum = "0"; //���ƶ����ļƻ�
            string perfectionHiddenNum = "0";//������������
            string approvalHiddenNum = "0";//������������
            string reformHiddenNum = "0";//������������
            string delayHiddenNum = "0";//����(��)����������������
            string reviewHiddenNum = "0";//�����յ�������
            string recheckHiddenNum = "0"; //��������֤��������
            string assessHiddenNum = "0";//������Ч������������
            string dailyexamineNum = "0";//������ճ�����
            int changJobNum = 0;//��Աת�ڴ����
            string planNum = "0";//�����еķ��������ƻ���
            string accidenteventNum = "0";//������¹��¼�
            int planApplyBMNum = 0;//����˲��Ź����ƻ�����
            int planApplyGRNum = 0;//����˸��˹����ƻ�����
            string lifthoistNum = "0";//��������ص�װ��ҵ
            int FeedbackNum = 0;//����������
            int ConfirmationNum = 0;//������ȷ������
            RiskPlanBLL planBll = new RiskPlanBLL();
         
            SaftyCheckDataBLL saftbll = new SaftyCheckDataBLL();
           

            //������������
            var dtHt = new HTBaseInfoBLL().QueryHidBacklogRecord(mode.ToString(), user.UserId);
            if (dtHt.Rows.Count == 8)
            {
                approvalHiddenNum = dtHt.Rows[0]["pnum"].ToString();//������������
                perfectionHiddenNum = dtHt.Rows[5]["pnum"].ToString();//������������
                reformHiddenNum = dtHt.Rows[1]["pnum"].ToString();//������������
                delayHiddenNum = dtHt.Rows[2]["pnum"].ToString();//����(��)����������������
                reviewHiddenNum = dtHt.Rows[3]["pnum"].ToString();//�����յ�������
                recheckHiddenNum = dtHt.Rows[6]["pnum"].ToString();//��������֤������
                assessHiddenNum = dtHt.Rows[4]["pnum"].ToString();//������Ч������������
                changeplanHiddenNum = dtHt.Rows[7]["pnum"].ToString(); //���ƶ����ļƻ�

                data.Add("HTPG", approvalHiddenNum);
                data.Add("HTDZG", reformHiddenNum);
                data.Add("HTYQZG", delayHiddenNum);
                data.Add("HTYS", reviewHiddenNum);
                data.Add("HTFCYZ", recheckHiddenNum);
                data.Add("HTXGPG", assessHiddenNum);
                data.Add("HTWS", perfectionHiddenNum);
                data.Add("HTZDZGJH", changeplanHiddenNum);
            }
            //���ձ�ʶ����
            if (arrItems.Contains("FXBSJH"))
            {
                planNum = planBll.GetPlanCount(user, mode).ToString();
                data.Add("FXBSJH", planNum);
            }
            //��ȫ���
            if (arrItems.Contains("CKPLAN"))
            {
                int[] countcheck = saftbll.GetCheckCount(user, mode);
                execCheckNum = countcheck.Sum() + "," + countcheck[0] + "," + countcheck[1] + "," + countcheck[2] + "," + countcheck[3] + "," + countcheck[4];
                data.Add("CKPLAN", execCheckNum);
            }
          
          

            //�ճ�����
            if (arrItems.Contains("RCKHDSH"))
            {
                DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
                dailyexamineNum = dailyexaminebll.CountIndex(user).ToString();
                data.Add("RCKHDSH", dailyexamineNum);
            }
            //������̹ܿ�
            if (arrItems.Contains("WBDWZZ"))
            {
                var wb = new OutprojectblacklistBLL().ToAuditOutPeoject(user);
                data.Add("WBDWZZ", wb[0]);//��λ����
                data.Add("WBRYZZ", wb[1]);//��Ա����
                data.Add("WBSCLA", wb[2]);//��������
                data.Add("WBDDSB", wb[4]);//�綯�豸
                data.Add("WBTZSB", wb[3]);//�����豸
                data.Add("WBRCSQ", wb[5]);//�볡���
                data.Add("WBKGSQ", wb[6]);//��������
                data.Add("DSHAQJSJD", wb[7]);//��ȫ��������
            }
            var work =GetWorkNum(user);
            data.Add("ZYTY", work[1] + work[0]);//�߷���ͨ�ô����(��)��ҵ
            data.Add("ZYJD", work[2]);//�߷��մ��ල
            List<int> list = new List<int>();
            if (arrItems.Contains("ZYJSJ"))
            {
                list = GetScaffoldNum(user);
                data.Add("ZYJSJ", list[1]);//���ּܴ����
            }
            if (arrItems.Contains("ZYSSBD"))
            {
                data.Add("ZYSSBD", GetSafetyChangeNum(user));//��ȫ��ʩ�䶯���������
            }

            if (arrItems.Contains("ZYXFS"))
            {
                list =GetFireWaterNum(user);
                data.Add("ZYXFS", list[0]);//����ˮ�����
            }

            list = GetlllegalNum(user);//Υ��
            data.Add("WZHZ", list[1]);//����׼Υ��
            data.Add("WZDZG", list[2]);//������Υ��
            data.Add("WZDYS", list[3]);//������Υ��
            data.Add("WZDWS", list[6]);//������Υ��
            data.Add("WZDZDZGJH", list[7]);//������ȷ��Υ��
            data.Add("WZDYSQR", list[8]);//������ȷ��Υ��
            data.Add("WZDZGYQSP", list[9]);//������������������
            data.Add("WZZGQR", list[10]);//Υ������ȷ��
            data.Add("WZDAKFDWS", list[11]);//Υ�µ����۷ִ�����

            list = GetJobSafetyCardNum(user);
            data.Add("DSPGCZY", list[0]);//�������ߴ���ҵ
            data.Add("DSPQZDZZY", list[1]);//���������ص�װ
            data.Add("DSPDTZY", list[2]);//������������ҵ
            data.Add("DSPDLZY", list[3]);//��������·��ҵ
            data.Add("DSPDHZY", list[4]);//������������ҵ
            data.Add("DSPMBCDZY", list[5]);//������ä������ҵ
            data.Add("DSPSXKJZY", list[6]);//���������޿ռ�
            data.Add("DSPSBJXQLZY", list[7]);//�������豸����������ҵ
            data.Add("DCSQRZYAQZ", list[8]);//����ʩȷ����ҵ��ȫ֤
            data.Add("DTDZYAQZ", list[9]);//��ͣ����ҵ��ȫ֤
            data.Add("DBAZYAQZ", list[10]);//��������ҵ��ȫ֤
            data.Add("DYSZYAQZ", list[11]);//��������ҵ��ȫ֤
            data.Add("DSDZYAQZ", list[12]);//���͵���ҵ��ȫ֤

            if (arrItems.Contains("YJYLJLWS"))
            {
                list = GetDrillRecordNum(user);
                data.Add("YJYLJLWS", list[0]);//������Ӧ��������¼
            }
            //��������
            if (arrItems.Contains("WTDZG"))
            {
              
                list = GetQuestionNum(user);//����
                data.Add("WTDZG", list[0]);//����������
                data.Add("WTDYZ", list[1]);//����֤����
                data.Add("FXWTDPG", list[2]);//�������ķ�������
            }
            //�����׼
            if (arrItems.Contains("BZSQ"))
            {
                StandardApplyBLL sa = new StandardApplyBLL();//��׼�ޣ�������˷���
                data.Add("BZSQ", sa.CountIndex("1"));//����������ı�׼�ޣ�����
                data.Add("BZDSH", sa.CountIndex("2"));//����ˣ�������׼�ޣ�����
                data.Add("BZDSFP", sa.CountIndex("3"));//������ı�׼�ޣ�����
            }
            //EHS�ƻ�
            if (arrItems.Contains("EHSPLAN"))
            {
                int countInfo = new ERCHTMS.Busines.ComprehensiveManage.InfoSubmitBLL().CountIndex("1");
                data.Add("EHSPLAN", countInfo);//���ϱ�EHS��Ϣ
            }
            //��ȫ����
            if (arrItems.Contains("AQJL"))
            {
                string count = new SaferewardBLL().GetRewardNum();//����˵İ�ȫ����
                data.Add("AQJL", count);
            }
            //��ȫ�ͷ�
            if (arrItems.Contains("AQCF"))
            {
                string count = new SafepunishBLL().GetPunishNum();//����˵İ�ȫ�ͷ�
                data.Add("AQCF", count);
            }
            //����NOSA
            if (arrItems.Contains("NOSAUPLOAD"))
            {
                var nosaworkbll = new NosaManage.NosaworksBLL();
                int countNOSA = nosaworkbll.CountIndex("1");//���ϴ���NOSA�����嵥
                data.Add("NOSAUPLOAD", countNOSA);
            }
            if (arrItems.Contains("NOSACHECK"))
            {
                var nosaworkbll = new NosaManage.NosaworksBLL();
                int countNOSA = nosaworkbll.CountIndex("3"); //����˵�NOSA�����嵥
                data.Add("NOSACHECK", countNOSA);
            }
            //��Աת��
            if (arrItems.Contains("TransferNum"))
            {
                changJobNum = new TransferBLL().GetTransferNum();//��Աת�ڴ����
                data.Add("TransferNum", changJobNum);
            }
            //�¹��¼�
            if (arrItems.Contains("AccidentEventNum"))
            {
                accidenteventNum = new PowerplantinsideBLL().GetAccidentEventNum();//������¹��¼�
                data.Add("AccidentEventNum", accidenteventNum);
            }

            if (arrItems.Contains("DSHSGSJCL"))
            {
                var sgsj = new PowerplanthandleBLL().ToAuditPowerHandle();//�¹��¼��������
                data.Add("DSHSGSJCL", sgsj[0]);
                data.Add("DZGSGSJ", sgsj[1]);
                data.Add("DQSSGSJCL", sgsj[2]);
                data.Add("DYSSGSJ", sgsj[3]);
            }

            if (arrItems.Contains("QZDZZY"))
            {
                lifthoistNum = new LifthoistjobBLL().GetLifthoistjobNum();//��������ص�װ
                data.Add("QZDZZY", lifthoistNum);
            }
            if (arrItems.Contains("AllotCheckNum"))
            {
                data.Add("AllotCheckNum", GetAllotCheckCount(user));//����������Ƶļ������
            }

            if (arrItems.Contains("YJYLJL"))
            {
                var evaluatenum = new DrillplanrecordBLL().GetDrillPlanRecordEvaluateNum(user);
                data.Add("YJYLJL", evaluatenum);
            }
            if (arrItems.Contains("YJYLJLPG"))
            {
                var assessnum = new DrillplanrecordBLL().GetDrillPlanRecordAssessNum(user);
                data.Add("YJYLJLPG", assessnum);
            }
            if (arrItems.Contains("WHPLY"))
            {
                //��ȡ��������Σ��Ʒ����
                data.Add("WHPLY", GetWhplyCount(user, "XLD_DANGEROUSCHEMICALRECEIVE"));
            }
            if (arrItems.Contains("WHPBF"))
            {
                //��ȡ��������Σ��Ʒ����
                data.Add("WHPBF", GetCheckCount(user, "XLD_DANGEROUSCHEMICALSCRAP"));
            }

            if (arrItems.Contains("BMGZJH"))
            {
                //�ۺ���Ϣ����
                planApplyBMNum = new PlanApplyBLL().GetPlanApplyBMNum(user);//����˲��Ź����ƻ�
                data.Add("BMGZJH", planApplyBMNum);
            }
            if (arrItems.Contains("GRGZJH"))
            {
                planApplyGRNum = new PlanApplyBLL().GetPlanApplyGRNum(user);//����˸��˹����ƻ�
                data.Add("GRGZJH", planApplyGRNum);
            }
            if (arrItems.Contains("SZRSH"))
            {
                //��ȡ����˵�����������
                data.Add("SZRSH", GetThreeCount(user).Count);
            }
            if (arrItems.Contains("AqdtCount"))
            {
                //��ȡ�������İ�ȫ��̬
                data.Add("AqdtCount", GetAqdtCount(user));
            }
            if (arrItems.Contains("DFKSL"))
            {
                FeedbackNum = new SafetyworksuperviseBLL().GetSuperviseNum(user.UserId, "1");//����������
                data.Add("DFKSL", FeedbackNum);
            }
            if (arrItems.Contains("DDBSJ"))
            {
                ConfirmationNum = new SafetyworksuperviseBLL().GetSuperviseNum(user.UserId, "2");//������ȷ������
                data.Add("DDBSJ", ConfirmationNum);
            }
            if (arrItems.Contains("JOBAPPROVAL"))
            {
                var num =GetJobApprovalFormNum(user);
                data.Add("JOBAPPROVAL", num);
            }

            if (arrItems.Contains("DSPYJWZLY"))
            {
                data.Add("DSPYJWZLY", GetSuppliesAccept());//������Ӧ��������ȡ
            }
            if (arrItems.Contains("DSHJGAQYS"))
            {
                //��ȡ�������Ŀ�����ȫ����
                data.Add("DSHJGAQYS", GetCheckCount(user, "EPG_SAFETYCOLLECT"));
            }
            // ��ȫ���˴�����
            if (arrItems.Contains("APPLYSAFE"))
            {
                string safeApplyNum = "0";
                safeApplyNum = new SafetyAssessmentBLL().GetApplyNum();
                data.Add("APPLYSAFE", safeApplyNum);
            }

            #region �嶨��ȫ���
            // ��ȫ��鿵��ʲ
            if (arrItems.Contains("FIVESAFECHECKNUM")) //��ȫ���
            {
                data.Add("FIVESAFECHECKNUM", new FivesafetycheckBLL().GetApplyNum("1","0","0"));

            }
            if (arrItems.Contains("FIVESAFELPJC")) //��Ʊ��������
            {
                data.Add("FIVESAFELPJC", new FivesafetycheckBLL().GetApplyNum("2", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFEWWDWJC")) //��ί��λ��������
            {
                data.Add("FIVESAFEWWDWJC", new FivesafetycheckBLL().GetApplyNum("3", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFEFWZJC")) //��Υ�¼�������
            {
                data.Add("FIVESAFEFWZJC", new FivesafetycheckBLL().GetApplyNum("4", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFESMXTJC")) // ��úϵͳ��������
            {
                data.Add("FIVESAFESMXTJC", new FivesafetycheckBLL().GetApplyNum("5", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFEMCAQJC")) // ú����ȫ��������
            {
                data.Add("FIVESAFEMCAQJC", new FivesafetycheckBLL().GetApplyNum("6", "0", "0"));

            }

            if (arrItems.Contains("FIVESAFERCAQJC")) // �ճ���ȫ��������
            {
                data.Add("FIVESAFERCAQJC", new FivesafetycheckBLL().GetApplyNum("7", "1", "0"));

            }
            if(arrItems.Contains("FIVESAFEZXAQJC")) // ר�ȫ��������
            {
                data.Add("FIVESAFEZXAQJC", new FivesafetycheckBLL().GetApplyNum("8", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEJRJC")) // 	����ǰ��ȫ��������
            {
                data.Add("FIVESAFEJRJC", new FivesafetycheckBLL().GetApplyNum("9", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEJJXJC")) // 	�����԰�ȫ���
            {
                data.Add("FIVESAFEJJXJC", new FivesafetycheckBLL().GetApplyNum("10", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEZHJC")) // 	�ۺϰ�ȫ���
            {
                data.Add("FIVESAFEZHJC", new FivesafetycheckBLL().GetApplyNum("11", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEQTJC")) // 	������ȫ��������
            {
                data.Add("FIVESAFEQTJC", new FivesafetycheckBLL().GetApplyNum("12", "1", "0"));

            }


            if (arrItems.Contains("FIVEQUACTION")) //������������
            {
                data.Add("FIVEQUACTION", new FivesafetycheckBLL().GetApplyNum("", "", "1"));

            }
            if (arrItems.Contains("FIVEQUACCEPT")) //������������
            {
                data.Add("FIVEQUACCEPT", new FivesafetycheckBLL().GetApplyNum("", "", "2"));

            }
            #endregion

            // ��ȫ���˴�����
            if (arrItems.Contains("ACJHNum"))
            {
                data.Add("ACJHNum", service.GetSafeMeasureNum(user));//���罭�갲��ƻ�
            }

            if (arrItems.Contains("RYLCDSP"))
            {
                data.Add("RYLCDSP", new LeaveApproveBLL().GetDBSXNum(user));//�����Ա�볧����
            }

            // �������
            if (arrItems.Contains("MatrixAction"))
            {
                data.Add("MatrixAction", new MatrixsafecheckBLL().GetActionNum());
            }

            return data;
        }
        public int GetCheckCount(ERCHTMS.Code.Operator user, string tableName)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select * from " + tableName + " t where flowdept like'%{0}%'", user.DeptId));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                {

                    var roleArr = user.RoleName.Split(','); //��ǰ��Ա��ɫ
                    var roleName = dr["flowrolename"].ToString(); //�����ɫ
                    for (var i = 0; i < roleArr.Length; i++)
                    {
                        //������˲���ͬ��ǰ�˲���idһ�£��е�ǰ�˽�ɫ��������˽�ɫ��
                        if (roleName.IndexOf(roleArr[i]) >= 0)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }
        public int GetWhplyCount(ERCHTMS.Code.Operator user, string tableName)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select * from " + tableName + " t"));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowdept"].ToString()))
                {
                    if (dr["flowdept"].ToString() == user.DeptId)
                    {
                        if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                        {

                            var roleArr = user.RoleName.Split(','); //��ǰ��Ա��ɫ
                            var roleName = dr["flowrolename"].ToString(); //�����ɫ
                            for (var i = 0; i < roleArr.Length; i++)
                            {
                                //������˲���ͬ��ǰ�˲���idһ�£��е�ǰ�˽�ɫ��������˽�ɫ��
                                if (roleName.IndexOf(roleArr[i]) >= 0)
                                {
                                    count++;
                                    break;
                                }
                            }
                        }
                    }
                }
                DataTable whpList = new DepartmentBLL().GetDataTable(string.Format(@"select * from XLD_DANGEROUSCHEMICAL t where id='{0}'", dr["mainid"].ToString()));
                if (whpList != null && whpList.Rows.Count > 0)
                {
                    if (whpList.Rows[0]["grantpersonid"].ToString() == user.UserId)
                    {
                        if (dr["grantstate"].ToString() == "2")//
                        {
                            count++;
                            continue;
                        }
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// ��ȡ�������İ�ȫ��̬
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetAqdtCount(ERCHTMS.Code.Operator user)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select * from BIS_SECURITYDYNAMICS t where flowdept='{0}'", user.DeptId));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                {

                    var roleArr = user.RoleName.Split(','); //��ǰ��Ա��ɫ
                    var roleName = dr["flowrolename"].ToString(); //�����ɫ
                    for (var i = 0; i < roleArr.Length; i++)
                    {
                        //������˲���ͬ��ǰ�˲���idһ�£��е�ǰ�˽�ɫ��������˽�ɫ��
                        if (roleName.IndexOf(roleArr[i]) >= 0)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }
        public DataTable GetDeptDataStat(ERCHTMS.Code.Operator user, string itemType, int mode = 0)
        {
            DataTable dt = GetDeptDataSet(user, itemType);

            if (dt.Rows.Count > 0)
            {
                Hashtable ht = new Hashtable();
                if (itemType == "SSJK" || itemType == "AQZB")
                {
                    ht = GetMonitorData(user, itemType);
                }
                if (itemType == "DBSX")
                {
                    ht = GetWorkData(user, mode);
                }
                dt.Columns.Add("Num");
                foreach (DataRow dr in dt.Rows)
                {
                    if (ht.ContainsKey(dr["itemcode"].ToString()))
                    {
                        dr["Num"] = ht[dr["itemcode"].ToString()];

                        if (!string.IsNullOrWhiteSpace(dr["callback"].ToString()))
                        {
                            dr["callback"] = dr["callback"].ToString().Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Url}", dr["address"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(dr["itemstyle"].ToString()))
                        {
                            dr["itemstyle"] = dr["itemstyle"].ToString().Replace("{Callback}", dr["Callback"].ToString()).Replace("{Icon}", dr["Icon"].ToString()).Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Num}", dr["Num"].ToString()).Replace("{Url}", dr["address"].ToString());
                        }

                    }

                }
            }
            return dt;
        }

        private Hashtable GetIndexData(Operator user)
        {
            throw new NotImplementedException();
        }

        #region ��ȡ���������ͼ(������������ȫ�������)
        /// <summary>
        /// ��ȡ���������ͼ(������������ȫ�������)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetTendencyChart(ERCHTMS.Code.Operator user)
        {
            return service.GetTendencyChart(user);
        }
        #endregion
        /// <summary>
        /// ��ȡ�������������Ϣ���ڽ����������������Ա�ڳ���������������������ڳ������λ���������½������Ա�������λΥ�´�����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetWBProjectNum(ERCHTMS.Code.Operator user)
        {
            return service.GetWBProjectNum(user);
        }
        /// <summary>
        /// ����ͳ��ͼ��
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GetHTChart(ERCHTMS.Code.Operator user)
        {
            return service.GetHTChart(user);
        }
        /// <summary>
        /// ��ȡ��ǰ�û���Ҫ�����ļ�¼ID������ö��ŷָ���
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<string> GetThreeCount(Operator user)
        {
            List<string> list = new List<string>();
           
            DepartmentBLL deptBll = new DepartmentBLL();
            DataTable dt = deptBll.GetDataTable(string.Format("select t.id,m.ApplyType,t.applytype type1,t.createuserdeptid,ProjectId,belongdeptid,nodeid,m.checkdeptid,m.checkroleid,m.scriptcurcontent,CheckUserId from BIS_THREEPEOPLECHECK t left join bis_manypowercheck m on t.nodeid=m.id where isover=0 and issumbit=1 and nodeid is not null"));
            UserBLL userBll = new UserBLL();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["applytype"].ToString() == "0")
                {
                    string deptId = "";
                    if (dr["checkdeptid"].ToString() == "-1")
                    {
                      
                        if (dr["type1"].ToString() == "�ڲ�")
                        {
                            deptId = dr["belongdeptid"].ToString();
                        }
                        else
                        {
                            string sql = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'", dr["projectid"].ToString());
                            DataTable dtPeoject = deptBll.GetDataTable(sql);
                            if (dtPeoject.Rows.Count > 0)
                            {
                                deptId = dtPeoject.Rows[0][0].ToString();
                            }
                        }
                       
                    }
                    if (dr["checkdeptid"].ToString() == "-3")
                    {
                        deptId = dr["createuserdeptid"].ToString();
                    }
                    string userIds = userBll.GetWFUserListByDeptRoleOrg(user.OrganizeId, deptId, "", dr["checkroleid"].ToString(), "").Cast<UserInfoEntity>().Aggregate("", (current, u) => current + (u.UserId + ",")).Trim(',');
                    if (userIds.Contains(user.UserId))
                    {
                        list.Add(dr["id"].ToString());
                    }
                }
                if (dr["applytype"].ToString() == "1")
                {
                    string []arr=deptBll.GetDataTable(dr["scriptcurcontent"].ToString()).AsEnumerable().Select(t => t.Field<string>("userid")).ToArray();
                    string userIds = string.Join(",", arr);
                    if (userIds.Contains(user.UserId))
                    {
                        list.Add(dr["id"].ToString());
                    }
                }
                if (dr["applytype"].ToString() == "2")
                {

                    if (dr["CheckUserId"].ToString().Contains(user.UserId))
                    {
                        list.Add(dr["id"].ToString());
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// ��ȡ����������������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetThreeCount1(ERCHTMS.Code.Operator user)
        {
            string count = new DepartmentBLL().GetDataTable(string.Format(@"select count(1) from (SELECT c.checkroleid,case when c.checkdeptid='-3' then t.createuserdeptid else to_char(c.checkdeptid) end as checkdeptid from BIS_THREEPEOPLECHECK t left join bis_manypowercheck c on t.nodeid=c.id) b where checkdeptid='{0}' and iscontaions(checkroleid,'{1}')=1", user.DeptId, user.RoleId)).Rows[0][0].ToString();
            return int.Parse(count);
        }
        /// <summary>
        /// ����������ͳ���������
        /// </summary>
        /// <param name="orgCode">��������</param>
        /// <returns></returns>
        public DataTable GetProjectChart(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectChart(user);
        }
        /// <summary>
        /// �����̷��յȼ�ͳ���������
        /// </summary>
        /// <param name="orgCode">��������</param>
        /// <returns></returns>
        public DataTable GetProjectChartByLevel(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectChartByLevel(user);
        }

        /// <summary>
        /// ��ȡ��鷢�ֵ�����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetCheckHtNum(ERCHTMS.Code.Operator user)
        {
            return service.GetCheckHtNum(user);
        }
        /// <summary>
        /// �����Ա�����仯����ͼ
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GetProjectPersonChart(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectPersonChart(user);
        }
        /// <summary>
        /// ����������ͳ������
        /// </summary>
        /// <param name="orgCode">��������</param>
        /// <returns></returns>
        public DataTable GetHTTypeChart(ERCHTMS.Code.Operator user)
        {
            return service.GetHTTypeChart(user);
        }
        /// <summary>
        /// ���������仯����ͼ 
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GetHTChangeChart(ERCHTMS.Code.Operator user)
        {
            return service.GetHTChangeChart(user);
        }
        /// <summary>
        /// ��ȡ֪ͨ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetNotices(ERCHTMS.Code.Operator user)
        {
            return service.GetNotices(user);
        }

        /// <summary>
        /// ��ȡһ�Ÿڴ�������
        /// </summary>        
        /// <returns></returns>
        public DataTable GetScreenTitle()
        {
            return service.GetScreenTitle();
        }

        /// <summary>
        /// ��ȡ��ȫ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetMeets(ERCHTMS.Code.Operator user)
        {
            return service.GetMeets(user);
        }
        /// <summary>
        /// ��ȡ��ȫ��̬
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetTrends(ERCHTMS.Code.Operator user)
        {
            return service.GetTrends(user);
        }
        /// <summary>
        /// ��ȡ��ڰ�
        /// </summary>
        /// <param name="user"></param>
        ///  <param name="mode">0:���1:�ڰ�</param>
        /// <returns></returns>
        public DataTable GetRedBlack(ERCHTMS.Code.Operator user, int mode)
        {
            return service.GetRedBlack(user, mode);
        }
        /// <summary>
        /// ������̸ſ�ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetProjectStat(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectStat(user);
        }
        /// <summary>
        /// ��ȡ��ԱΥ����Ϣ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetWZInfo(Pagination pagination, string queryJson)
        {
            return service.GetWZInfo(pagination, queryJson);
        }
        public DataTable GetWZInfo(string userid, int mode=0)
        {
            return service.GetWZInfo(userid, mode);
        }
        public DataTable GetWZInfoByUserId(string userId,int mode=0)
        {
            return service.GetWZInfoByUserId(userId, mode);
        }
        /// <summary>
        /// ��ȡδǩ���Ļ�������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetMeetNum(string userId)
        {
            return service.GetMeetNum(userId);
        }
        /// <summary>
        /// ��ȡʩ����Σ�󹤳���
        /// <param name="user"></param>
        /// </summary>
        /// <returns></returns>
        public int GetProjectNum(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectNum(user);
        }
        /// <summary>
        ///��ȡ��������(����Ϊ���������ش�����������һ����������,�������ڵ�������,����δ����������,������������δ��������)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetHtNum(ERCHTMS.Code.Operator user)
        {
            return service.GetHtNum(user);
        }
        /// <summary>
        /// ��ȡΣ��Դ����������Ϊ���������ش�Σ��Դ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetDangerSourceNum(ERCHTMS.Code.Operator user)
        {
            return service.GetDangerSourceNum(user);
        }
        /// <summary>
        /// ��ȡ�¹�������Ϣ������Ϊ�¹�����������������������Ա��
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetAccidentNum(ERCHTMS.Code.Operator user)
        {
            return service.GetAccidentNum(user);
        }
        /// <summary>
        /// ��ȡ�ش��������(����Ϊ���������ش�����������ϴ����������һ������������ͷ�������)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetRiskNum(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskNum(user);
        }
        /// <summary>
        /// ��ȡ�߷�����ҵ(����Ϊ�߷���ͨ�ô�ȷ����ҵ�������߷���ͨ�ô����(��)��ҵ����,���ල������,�߷�����ҵ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetWorkNum(ERCHTMS.Code.Operator user)
        {
            return service.GetWorkNum(user);
        }

        /// <summary>
        /// ��ȡ��Σ��ҵ��ȫ���֤�������죨����Ϊ�ߴ���ҵ�����ص�װ��ҵ��������ҵ����·��ҵ��������ҵ��ä������ҵ�����޿ռ���ҵ���豸����������ҵ������ʩȷ�ϡ���ͣ�硢�������������ա����͵磩
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetJobSafetyCardNum(ERCHTMS.Code.Operator user)
        {
            return service.GetJobSafetyCardNum(user);
        }

        /// <summary>
        /// ��ȡ���ּ�ͳ�ƣ������ա�����ˣ�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetScaffoldNum(ERCHTMS.Code.Operator user)
        {
            return service.GetScaffoldNum(user);
        }

        /// <summary>
        /// ��ȡ����ˮ�����(��)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetFireWaterNum(ERCHTMS.Code.Operator user)
        {
            return service.GetFireWaterNum(user);
        }
        /// <summary>
        /// ��ȡ�����豸����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetEquimentNum(ERCHTMS.Code.Operator user)
        {
            return service.GetEquimentNum(user);
        }
        /// <summary>
        /// ��ȡΥ��������Ϣ������Ϊ��Υ��������������׼�������ġ������ա�����δ�����������������������������Ƶ�Υ��������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetlllegalNum(ERCHTMS.Code.Operator user)
        {
            return service.GetlllegalNum(user);
        }


        /// <summary>
        /// Ӧ����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetDrillRecordNum(ERCHTMS.Code.Operator user) 
        {
            return service.GetDrillRecordNum(user);
        }
        /// <summary>
        /// ��ȡ����������Ϣ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetQuestionNum(ERCHTMS.Code.Operator user)
        {
            return service.GetQuestionNum(user);
        }
        /// <summary>
        /// ��ȡΥ��������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public decimal GetlllegalRatio(ERCHTMS.Code.Operator user)
        {
            return service.GetlllegalRatio(user);
        }
        /// <summary>
        /// �����յȼ���ͼ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRiskCounChart(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskCounChart(user);
        }
        /// <summary>
        /// ��ȡ��ȫ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetWorks(ERCHTMS.Code.Operator user)
        {
            return service.GetWorks(user);
        }
        /// <summary>
        /// �������ڻ�ȡ���˰�ȫ������¼
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">ʱ��</param>
        /// <returns></returns>
        public DataTable GetWorkInfoByTime(ERCHTMS.Code.Operator user, string time)
        {
            return service.GetWorkInfoByTime(user, time);
        }
        /// <summary>
        /// ��ȡ������������յȼ���Ϣ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode">����Ψһ��ʶ�룬��һ����������룬�����Ӣ�Ķ��ŷָ�</param>
        /// <returns></returns>
        public DataTable GetAreaStatus(ERCHTMS.Code.Operator user, string areaCode, int mode = 0)
        {
            return service.GetAreaStatus(user,areaCode, mode);
        }

        /// <summary>
        /// ��ȡ������������յȼ���Ϣ(����ʲ�汾)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public List<AreaRiskLevel> GetKbsAreaStatus()
        {
            return service.GetKbsAreaStatus();
        }


        /// <summary>
        /// ��ȡ������������յȼ���Ϣ(���Źܿ����ġ�������ɫͼ���汾)
        /// </summary>
        /// <returns></returns>
        public List<AreaRiskLevel> GetKMAreaStatus()
        {
            return service.GetKMAreaStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetSafetyChangeNum(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyChangeNum(user);
        }
        /// <summary>
        /// ��ȫԤ����Ŀ��ʡ��˾����������Ϊ�����ش������ĵ糧��������������С��80%�ĵ糧��������������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<decimal> GetWarnItems(ERCHTMS.Code.Operator user)
        {
            return service.GetWarnItems(user);
        }

        public List<decimal> GetRiskAnalyzeItems(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskAnalyzeItems(user);
        }

        /// <summary>
        /// ��ȡ��ȫ�����������Ϊ��ȫ����������ִ�еİ�ȫ�������ʡ��˾����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetSafetyCheckForGroup(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyCheckForGroup(user);
        }
        /// <summary>
        /// ��ȡ������Ϣ������Ϊ�ش�����������������������������δ���������������ֵ�����������ʡ��˾����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetHtForGroup(ERCHTMS.Code.Operator user)
        {
            return service.GetHtForGroup(user);
        }
        /// <summary>
        /// ��ȡ�糧Ԥ������ֵ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public decimal GetScore(ERCHTMS.Code.Operator user, string time = "")
        {
            RiskBLL riskBLL = new RiskBLL();//��ȫ����
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//��ȫ���
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//�¹�����
            ClassificationBLL classBLL = new ClassificationBLL();
            List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();
            if (list.Count == 0)
            {
                list = classBLL.GetList("0").ToList();
            }
            decimal totalScore = 0;
            //�����¹������ܵ÷�
            decimal score = htBLL.GetHiddenWarning(user, time);
            totalScore = score * decimal.Parse(list[0].WeightCoeffcient);
            //���㰲ȫ����ܵ÷�
            score = saBLL.GetSafeCheckSumCount(user);
            totalScore += score * decimal.Parse(list[1].WeightCoeffcient);
            //���㰲ȫ�����ܵ÷�
            score = riskBLL.GetRiskValueByTime(user, time);
            totalScore += score * decimal.Parse(list[2].WeightCoeffcient);
            return totalScore;
        }
        /// <summary>
        /// ��ȡ�糧Ԥ��������Ϣ���Դ�Ϊ�÷֣���ȫ����
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<string> GetScoreInfo(ERCHTMS.Code.Operator user, string time = "")
        {
            RiskBLL riskBLL = new RiskBLL();//��ȫ����
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//��ȫ���
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//�¹�����
            ClassificationBLL classBLL = new ClassificationBLL();
            List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();
            if (list.Count == 0)
            {
                list = classBLL.GetList("0").ToList();
            }
            decimal totalScore = 0;
            //�����¹������ܵ÷�
            decimal score = htBLL.GetHiddenWarning(user, time);
            totalScore = score * decimal.Parse(list[0].WeightCoeffcient);
            //���㰲ȫ����ܵ÷�
            score = saBLL.GetSafeCheckSumCount(user);
            totalScore += score * decimal.Parse(list[1].WeightCoeffcient);
            ////���㰲ȫ�����ܵ÷�
            //score = riskBLL.GetRiskValueByTime(user, time);
            //totalScore += score * decimal.Parse(list[2].WeightCoeffcient);

            List<string> data = new List<string>();
            data.Add(totalScore.ToString());

            DataItemDetailBLL itemBLL = new DataItemDetailBLL();
            string val = itemBLL.GetItemValue("����Ԥ�������ֵ����");
            int idx = 0;
            string[] arr = val.Split('|');
            if (!string.IsNullOrEmpty(val))
            {
                int j = 0;
                foreach (string str in arr)
                {
                    string[] arrVal = str.Split(',');
                    if (totalScore > decimal.Parse(arrVal[0]) && totalScore <= decimal.Parse(arrVal[1]))
                    {
                        idx = j;
                        break;
                    }
                    j++;
                }
            }
            arr = new string[] { "��ȫ", "ע��", "����", "Σ��" };
            data.Add(arr[idx]);
            return data;
        }
        /// <summary>
        /// �糧��������
        /// </summary>
        /// <param name="deptCode">ʡ��˾deptCode</param>
        /// <param name="mode">������ʽ��0������������������1��������������������2����δ�ջ�����������</param>
        /// <returns></returns>
        public DataView GetRatioDataOfFactory(ERCHTMS.Code.Operator user, int mode = 0)
        {
            return service.GetRatioDataOfFactory(user, mode);
        }
        /// <summary>
        /// �糧��ȫ����������Ϣͳ��
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<decimal> GetHt2CheckOfFactory(string orgId, string time, string orgCode = "")
        {
            return service.GetHt2CheckOfFactory(orgId, time, orgCode);
        }
        /// <summary>
        /// ��ȡ��ǰ�û���������������ָ����Ŀ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetDeptDataSet(ERCHTMS.Code.Operator user, string itemType)
        {
            return service.GetDeptDataSet(user, itemType);
        }
        /// <summary>
        /// ��ȡ�糧����������
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<decimal> GetHtZgl(string orgId)
        {
            return service.GetHtZgl(orgId);
        }
        /// <summary>
        /// ��ȡʡ��˾�·��İ�ȫ�������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetSafetyCheckTask(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyCheckTask(user);
        }

        /// <summary>
        /// ��ȡ�ش��������(����Ϊ���������ش�����������ϴ����������һ������������ͷ�������)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetRiskNumForGDXY(ERCHTMS.Code.Operator user)
        {
            DepartmentBLL deptBll = new DepartmentBLL();
            List<int> list = new List<int>();
            string sql = "";
            if (user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�"))
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0  and t.deptcode like '{0}%' and risktype in('����','�豸','����')", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("����") || user.RoleName.Contains("ʡ��"))
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0 and risktype in('����','�豸','����') and t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "����");
            }
            else
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0  and t.deptcode like '{0}%' and risktype in('����','�豸','����')", user.DeptCode);
            }
            int count = int.Parse(deptBll.GetDataTable(sql).Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='�ش����'").Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='�ϴ����'").Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='һ�����'").Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='�ͷ���'").Rows[0][0].ToString());
            list.Add(count);
            return list;
        }

        #region ��ȫ״̬����ֵ
        #region MyRegion
        /// <summary>
        /// ��ȡָ��ֵ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public decimal GetSafetyAssessedValue(SafetyAssessedArguments entity)
        {
            return service.GetSafetyAssessedValue(entity);
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// ��ȡָ�����
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public List<SafetyAssessedModel> GetSafetyAssessedData(SafetyAssessedArguments argument)
        {
            return service.GetSafetyAssessedData(argument);
        }
        #endregion

        #region ��ȡģ���¶�Ӧ��ָ��
        /// <summary>
        /// ��ȡģ���¶�Ӧ��ָ��
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<SafetyAssessedChildModel> GetSafetyAssessedChildData(SafetyAssessedArguments argument, List<ClassificationIndexEntity> list)
        {
            return service.GetSafetyAssessedChildData(argument, list);
        }
        #endregion
        #endregion


        #region ��ȡ�ع���Ϣ
        /// <summary>
        /// ��ȡ�ع���Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetExposureInfo(ERCHTMS.Code.Operator user)
        {
            try
            {
                return service.GetExposureInfo(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// ��������ʵʱ����
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<RealTimeWorkModel> GetRealTimeWork(Operator user)
        {
            return service.GetRealTimeWork(user);
        }
        /// <summary>
        /// ���N����Ԥ������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<RealTimeWorkModel> GetWarningCenterWork(Operator user)
        {
            return service.GetWarningCenterWork(user);
        }
        public int GetJobApprovalFormNum(ERCHTMS.Code.Operator user)
        {
            return service.GetJobApprovalFormNum(user);
        }

        /// <summary>
        /// ��ȡ���ո�Σ��ҵ
        /// </summary>
        /// <returns></returns>
        public int GetTodayWorkForDangerJob()
        {
            JobSafetyCardApplyBLL BLL = new JobSafetyCardApplyBLL();
            Pagination pagenation = new Pagination();
            pagenation.rows = 100000;
            pagenation.page = 1;
            DataTable dt = BLL.GetTodayWorkList(pagenation, JsonConvert.SerializeObject(new { }));
            return dt.Rows.Count;
        }

        /// <summary>
        /// ��ȡӦ���������ô���
        /// </summary>
        /// <returns></returns>
        public int GetSuppliesAccept()
        {
            try
            {
                SuppliesacceptBLL BLL = new SuppliesacceptBLL();
                Pagination pagenation = new Pagination();
                pagenation.rows = 100000;
                pagenation.page = 1;
                pagenation.conditionJson = "1=1";
                DataTable dt = BLL.GetPageList(pagenation, JsonConvert.SerializeObject(new { dbsx = "0" }));
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }


        #region ���纺���ԽӴ�������
        /// <summary>
        /// ���纺���ԽӴ�������(����)
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncJS(GdhcDbsxEntity entity)
        {

            service.GdhcDbsxSyncJS(entity);
        }

        /// <summary>
        /// ���纺���ԽӴ�������(�Ѱ�)
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncYB(GdhcDbsxEntity entity)
        {

            service.GdhcDbsxSyncYB(entity);
        }

        /// <summary>
        /// ���纺���ԽӴ�������(���)
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncBJ(GdhcDbsxEntity entity)
        {

            service.GdhcDbsxSyncBJ(entity);
        }
        #endregion
    }

}
