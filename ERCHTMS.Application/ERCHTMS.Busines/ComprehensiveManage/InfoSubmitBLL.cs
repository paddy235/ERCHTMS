using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.IService.ComprehensiveManage;
using ERCHTMS.Service.ComprehensiveManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.JPush;
using System.Linq;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.ComprehensiveManage
{
    /// <summary>
    /// �� ������Ϣ���ͱ�
    /// </summary>
    public class InfoSubmitBLL
    {
        private InfoSubmitIService service = new InfoSubmitService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<InfoSubmitEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// ��ҳ����
        /// </summary>
        /// <param name="indexType">�������ͣ�1�����Ϣ��2����������Ҫ��</param>
        /// <returns></returns>
        public int CountIndex(string indexType)
        {
            return service.CountIndex(indexType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public InfoSubmitEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InfoSubmitEntity entity)
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

        #region ����Ϣ����
        /// <summary>
        /// ���Ͷ���Ϣ����EHS�����û��ϱ�EHS��Ϣ
        /// </summary>
        public void SendMessage()
        {
            var now = DateTime.Now;
            SendMessage(now);
        }
        /// <summary>
        /// ���Ͷ���Ϣ����EHS�����û��ϱ�EHS��Ϣ
        /// </summary>
        /// <param name="now">��ǰʱ��</param>
        public void SendMessage(DateTime now)
        {
            var deptBll = new DepartmentBLL();            
            var listEHS = new DataItemDetailBLL().GetDataItemListByItemCode("'EHSDepartment'").ToList();
            //ö�ٽ���Ӧ���ѵ��ϱ����ͣ��磺�±������������걨���걨��
            var list = EnumWarTypeOfDay(now);
            foreach (var wType in list)
            {
                foreach (var ehsDepart in listEHS)
                {//���糧��EHS���ţ�ͳһ���Ͷ���Ϣ��
                    var ehsdeptcode = ehsDepart.ItemValue;//EHS���ű���
                    var dept = deptBll.GetDeptOrgInfo(ehsDepart.ItemName);//EHS������������
                    if (dept != null)
                    {//ESH��������
                        var orgCode = dept.EnCode;
                        var orgName = dept.FullName;                        
                        var msgBody = GenMessageBody(wType, now, orgName);
                        if (!HasInfo(orgCode, wType, msgBody.Item4) && !ExistMsg(msgBody.Item3))
                        {//δ�ϱ�����δ������Ϣ��
                            var msg = GenEntity(msgBody, ehsdeptcode);
                            if (msg != null)
                            {//���Ͷ���Ϣ
                                if (new MessageBLL().SaveForm("", msg))
                                {
                                    JPushApi.PublicMessage(msg);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// ö��ָ�����ڿ����ѵ�����
        /// </summary>
        /// <param name="now">ָ������</param>
        /// <returns>�������ͼ���</returns>
        private List<WarningType> EnumWarTypeOfDay(DateTime now)
        {
            var list = new List<WarningType>();

            //�¶�����
            var day = now.Day;
            var monEnd = DateTime.Parse(now.ToString("yyyy-MM-2"));
            if (day >= 2)//2���Ժ�ȡ������Ϊ����ʱ��
                monEnd = monEnd.AddMonths(1);
            var monStart = monEnd.AddDays(-14);
            if (now >= monStart && now < monEnd)
                list.Add(WarningType.Month);

            //��������(��һ���ȡ���������)
            var q1End = DateTime.Parse(string.Format("{0}-4-2", now.Year));
            var q1Start = q1End.AddDays(-14);
            var q3End = DateTime.Parse(string.Format("{0}-10-2", now.Year));
            var q3Start = q3End.AddDays(-14);
            if ((now >= q1Start && now < q1End)|| (now >= q3Start && now < q3End))
                list.Add(WarningType.Quarter);

            //���������
            var halfYearEnd = DateTime.Parse(string.Format("{0}-7-2", now.Year));
            var halfYearStart = halfYearEnd.AddDays(-14);           
            if (now >= halfYearStart && now < halfYearEnd)
                list.Add(WarningType.HalfYear);

            //�������
            var yearEnd = DateTime.Parse(now.ToString("yyyy-1-2"));
            if (day >= 2)//2���Ժ�ȡ��һ����Ϊ����ʱ��
                yearEnd = yearEnd.AddYears(1);
            var yearStart = yearEnd.AddDays(-14);
            if (now >= yearStart && now < yearEnd)
                list.Add(WarningType.Year);

            return list;
        }
        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="warType">��������</param>
        /// <param name="now">����</param>
        /// <param name="orgName">�糧����</param>
        /// <returns>��Ϣ���ݣ����⡢���ݡ���ע���·ݻ򼾶ȣ�</returns>
        private Tuple<string,string,string,int> GenMessageBody(WarningType warType,DateTime now,string orgName="")
        {
            Tuple<string, string, string,int> msg = null;
            var title = "EHS��Ϣ�ϱ�����";
            var content = "";
            var remark = "";
            var mark = 0;
            switch (warType)
            {
                case WarningType.Month:
                    if (now.Day == 1)
                    {//1��ȡ��һ����
                        now = now.AddMonths(-1);
                    }
                    remark = now.ToString("yyyy��MM�·�");
                    mark = now.Month;                    
                    content = string.Format("�뼰ʱ�ϱ�{0}��{1}�·�EHS��Ϣ", now.Year, mark);
                    break;
                case WarningType.Quarter:                    
                    int q = (int)Math.Ceiling(now.Month * 1.0 / 3);
                    remark = string.Format("{0}��{1}����", now.Year, q);
                    mark = q;
                    if ((now.Month == 4 || now.Month == 10) && now.Day == 1)
                    {//1��ȡ��һ������
                        remark = string.Format("{0}��{1}����", now.Year, (q - 1));
                        mark = q - 1;
                    }
                    content = string.Format("�뼰ʱ�ϱ�{0}���{1}����EHS��Ϣ",now.Year,mark);
                    break;
                case WarningType.HalfYear:
                    remark = now.ToString("yyyy���ϰ���");
                    content = string.Format("�뼰ʱ�ϱ�{0}���ϰ���EHS��Ϣ",now.Year);
                    break;
                case WarningType.Year:
                    if (now.Month == 1 && now.Day == 1)
                    {//1��ȡ��һ���
                        now = now.AddYears(-1);
                    }
                    remark = now.ToString("yyyy���");
                    mark = now.Year;                    
                    content = string.Format("�뼰ʱ�ϱ�{0}�����EHS��Ϣ",mark);
                    break;
            }
            remark += "��" + orgName + "��";//�ñ�ע��Ϊ�ж��ظ�����Ϣ��������
            msg = new Tuple<string, string, string,int>(title, content, remark,mark);

            return msg;
        }
        /// <summary>
        /// �ж��Ƿ��Ѿ��ϱ�EHS��Ϣ
        /// </summary>
        /// <param name="orgCode">�糧��λ����</param>
        /// <param name="wType">��������</param>
        /// <param name="mark">�·ݻ򼾶�</param>
        /// <returns>�����true:��,false:�ޣ�</returns>
        private bool HasInfo(string orgCode,WarningType wType,int mark)
        {
            var r = false;

            var now = DateTime.Now;
            var sqlWhere = string.Format(" and createuserorgcode='{0}'", orgCode);
            if (wType == WarningType.Month)
                sqlWhere += string.Format(" and infotype='�±�' and extract(year from starttime)='{0}' and extract(month from starttime)='{1}' ", now.Year, mark);

            if (wType == WarningType.Quarter)
            {
                DateTime qstart = DateTime.Parse(string.Format("{0}-{1}-1", now.Year, mark * 3 - 2));
                DateTime qEnd = qstart.AddMonths(3);
                sqlWhere += string.Format(" and infotype='����' and starttime>=TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') and starttime<TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss') ", qstart.ToString("yyyy-MM-dd"),qEnd.ToString("yyyy-MM-dd"));
            }

            if (wType == WarningType.HalfYear)
                sqlWhere += string.Format(" and infotype='���걨' and extract(year from starttime)='{0}' ", now.Year);

            if (wType == WarningType.Year)
                sqlWhere += string.Format(" and infotype='�걨' and extract(year from starttime)='{0}' ", now.Year);

            var countInfo = new InfoSubmitBLL().GetList(sqlWhere).Count();
            r = countInfo > 0;

            return r;

        }
        /// <summary>
        /// �ж��Ƿ��Ѿ����Ͷ���Ϣ�������ظ�������ͬ�Ķ���Ϣ��
        /// </summary>
        /// <param name="msgRemark">�ظ���Ϣ����</param>
        /// <returns>�����true:�У�false���ޣ�</returns>
        private bool ExistMsg(string msgRemark)
        {
            var r = false;

            var count = new MessageBLL().GetList(string.Format(" and Remark='{0}'", msgRemark)).Count();
            r = count > 0;

            return r;
        }
        /// <summary>
        /// ������Ϣʵ��
        /// </summary>
        /// <param name="msgBody">��Ϣ���ݣ����⡢���ݡ���ע���·ݻ򼾶ȣ�</param>
        /// <param name="ehsDeptCode">EHS���ű���</param>
        /// <returns>����Ϣʵ��</returns>
        private MessageEntity GenEntity(Tuple<string, string, string,int> msgBody,string ehsDeptCode)
        {
            MessageEntity msg = null;           
            var aList = new UserBLL().GetListForCon(x => x.DepartmentCode.Contains(ehsDeptCode));
            msg = new MessageEntity()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = string.Join(",", aList.Take(10).Select(x => x.Account)),
                UserName = string.Join(",", aList.Take(10).Select(x => x.RealName)),
                SendTime = DateTime.Now,
                SendUser = "System",
                SendUserName = "ϵͳ����Ա",
                Title = msgBody.Item1,
                Content = msgBody.Item2,
                Remark = msgBody.Item3,
                Category = "����"
            };

            return msg;
        }
        #endregion
    }
    /// <summary>
    /// ����Ϣ��������
    /// </summary>
    public enum WarningType
    {
        /// <summary>
        /// �±�
        /// </summary>
        Month,
        /// <summary>
        /// ����
        /// </summary>
        Quarter,
        /// <summary>
        /// ���걨
        /// </summary>
        HalfYear,
        /// <summary>
        /// �걨
        /// </summary>
        Year
    }
}
