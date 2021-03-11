using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ����¼
    /// </summary>
    public class SaftyCheckDataRecordBLL
    {
        private SaftyCheckDataRecordIService service = new SaftyCheckDataRecordService();

        #region ��ȡ����

         /// <summary>
        /// �������Ǽ���ѡ�����¼���й���
        /// </summary>
        /// <param name="recId">��ȫ����¼Id</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRecordFromHT(string recId,Operator user)
        {
            return service.GetRecordFromHT(recId,user);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�����豸��������¼�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListJsonByTz(Pagination pagination)
        {
            return service.GetPageListJsonByTz(pagination);
        }
        /// <summary>
        /// ��ȫ�������
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetCheckTaskList(Pagination pagination, string queryJson)
        {
            return service.GetCheckTaskList(pagination, queryJson);
        }
        /// <summary>
        ///��ȡͳ�Ʊ������
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        public string GetSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetSaftyList(deptCode, year, belongdistrictcode, ctype);
        }
        public string GetGrpSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetGrpSaftyList(deptCode, year, belongdistrictcode, ctype);
        }
        public List<SaftyCheckCountEntity> GetSaftyList(string deptCode, string year, string ctype)
        {
            return service.GetSaftyList(deptCode, year, ctype);
        }
        /// <summary>
        ///��ȡͳ�Ʊ������(�Ա�)
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        public string GetSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetSaftyListDB(deptCode, year, belongdistrictcode, ctype);
        }
        public string GetGrpSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetGrpSaftyListDB(deptCode, year, belongdistrictcode, ctype);
        }
        /// <summary>
        ///��ȡ�Ա�����
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        public string GetAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetAreaSaftyState(deptCode, year, belongdistrictcode, ctype);
        }
        public string GetGrpAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetGrpAreaSaftyState(deptCode, year, belongdistrictcode, ctype);
        }
        /// <summary>
        /// ����ͼͳ������
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrictcode">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        public string getRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.getRatherCheckCount(deptCode, year, belongdistrictcode, ctype);
        }
        public string getGrpRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.getGrpRatherCheckCount(deptCode, year, belongdistrictcode, ctype);
        }
        /// <summary>
        /// ����ͼͳ������
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        public string getMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return service.getMonthCheckCount(deptCode, year, belongdistrict, ctype);
        }
        public string getGrpMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return service.getGrpMonthCheckCount(deptCode, year, belongdistrict, ctype);
        }
        /// <summary>
        /// ר��������б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetPageListForType(Pagination pagination, string queryJson)
        {
            return service.GetPageListForType(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaftyCheckDataRecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ����
        /// </summary>
        public DataTable ExportData(Pagination pagination, string queryJson)
        {
            return service.ExportData(pagination, queryJson);
        }

        /// <summary>
        /// ���ݲ��ź����ͻ�ȡ���ŵļ�������
        /// </summary>
        /// <param name="DeptCode">����code����</param>
        /// <returns></returns>
        public DataTable AddDeptCheckTable(string DeptCode, string Type) {
            return service.AddDeptCheckTable(DeptCode, Type);
        }

        /// <summary>
        /// ���ݲ���CODE��ȡ������Ա����
        /// </summary>
        /// <param name="Encode">����Code</param>
        /// <returns>���ض���Json</returns>
        public DataTable GetPeopleByEncode(string Encode)
        {
            return service.GetPeopleByEncode(Encode);
        }

        /// <summary>
        /// ���ݼ�����ͼ���������ƻ�ȡ�������ID�ͼ�����ID(�м��ö��ŷָ�������Ϊ�������ID��������ID)������ƥ��ʱ���ؿ��ַ���
        /// </summary>
        ///  <param name="chkId">����¼Id</param>
        /// <param name="checkObject">����������</param>
        /// <param name="checkContent">�����������</param>
        /// <param name="user">��ǰ�û�</param>
        /// <returns></returns>
        public string GetCheckContentId(string chkId, string checkObject, string checkContent, Operator user)
        {
            string id = "";
             DepartmentBLL deptBll=new DepartmentBLL();
            if (string.IsNullOrWhiteSpace(checkObject) && string.IsNullOrWhiteSpace(checkContent))
            {
                id = GetRecordFromHT(chkId, user);
            }
            else
            {

                string sql = string.Format("select id from BIS_SAFTYCHECKDATADETAILED where recid='{2}' and checkobject='{0}' and checkcontent='{1}'", checkObject.Trim(), checkContent.Trim(), chkId);
             
                SaftyCheckDataRecordEntity sd = GetEntity(chkId);
                if (sd!=null)
                {
                   if(sd.CheckDataType!=1)
                   {
                       sql+=string.Format(" and (',' || checkmanid || ',') like '%,{0},%'",user.Account);
                   }
                }
                 DataTable dt =deptBll.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0][0].ToString();
                    DataTable dtItem = deptBll.GetDataTable(string.Format("select id from BIS_SAFTYCONTENT where checkobject='{0}' and detailid='{1}' and recid='{2}'", checkObject.Trim(), id, chkId));
                    if (dtItem.Rows.Count>0)
                    {
                        id = "";
                    }
                    
                }
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                DataTable dt = deptBll.GetDataTable(string.Format("select checkobjectid from BIS_SAFTYCHECKDATADETAILED where  id='{0}' and recid='{1}'",  id, chkId));
                if(dt.Rows.Count>0)
                {
                    id += ","+dt.Rows[0][0].ToString();
                }
            }
            return id;
        }

        #endregion

        #region �ύ����
        /// <summary>
        /// ���ĵǼ���
        /// </summary>
        public void RegisterPer(string userAccount,string id)
        {
            try
            {
                service.RegisterPer(userAccount,id);
            }
            catch (Exception)
            {
                throw;
            }
        }
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
        public int SaveForm(string keyValue, SaftyCheckDataRecordEntity entity,ref string recid)
        {
            try
            {
                return service.SaveForm(keyValue, entity, ref recid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// �޸��Ѽ����Ա
        /// </summary>
        public void UpdateCheckMan(string userAccount)
        {
            try
            {
                service.UpdateCheckMan(userAccount);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ��ȡ�����ֻ���
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="safeCheckTypeId">�������</param>
        /// <param name="searchString">����¼����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user,string deptCode="")
        {
            return service.GetSaftyDataList(safeCheckTypeId, searchString,user);
        }
        public IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user, string deptCode, int page, int size, out int total)
        {
            
            return service.GetSaftyDataList(safeCheckTypeId, searchString, user, deptCode, page, size,out total);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="safeCheckIdItem">����¼</param>
        /// <returns>���ط�ҳ�б�</returns>
        public SaftyCheckDataRecordEntity getSaftyCheckDataRecordEntity(string safeCheckIdItem)
        {
            return service.getSaftyCheckDataRecordEntity(safeCheckIdItem);
        }
        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <param name="safeCheckIdItem">����¼</param>
        /// <param name="safeCheckIdItem">����¼</param>
        /// <returns>����ʵ��</returns>
        public DataTable getCheckRecordDetail(string safeCheckIdItem, string riskPointId)
        {
            return service.getCheckRecordDetail(safeCheckIdItem, riskPointId);
        }
        /// <summary>
        /// �����ճ����ƻ�
        /// </summary>
        public int addDailySafeCheck(SaftyCheckDataRecordEntity se, Operator user)
        {
            return service.addDailySafeCheck(se,user);
        }

        /// <summary>
        /// ѡ������Ա
        /// </summary>
        public DataTable selectCheckPerson(Operator user)
        {
            return service.selectCheckPerson(user);
        }

        public List<SaftyCheckDataRecordEntity> GetSaftDataIndexList(ERCHTMS.Code.Operator user)
        {
            return service.GetSaftDataIndexList(user);
        }
        public DataTable GetSaftyCheckDataList(string safeCheckTypeId, long status, Operator user, string deptCode, long page, long size, out int total, string startTime, string endTime)
        {
            return service.GetSaftyCheckDataList(safeCheckTypeId, status, user, deptCode, page, size, out total, startTime, endTime);
        }
        #endregion

        #region ��ҳԤ��
        public DataTable GetSafeCheckWarning(Operator user, string mode = "0")
        {
            return service.GetSafeCheckWarning(user, mode);
        }
        public decimal GetSafeCheckWarningM(Operator user, string date,int mode=0)
        {
            return service.GetSafeCheckWarningM(user,date,mode);
        }
        public string GetSafeCheckWarningS()
        {
            return service.GetSafeCheckWarningS();
        }

        public decimal GetSafeCheckSumCount(Operator user) 
        {
            return service.GetSafeCheckSumCount(user);
        }
           /// <summary>
        /// ��ȡ��ҳ��ȫ��鿼�˽������
        /// </summary>
        /// <param name="user">��ǰ��¼��user</param>
        /// <param name="time">ͳ��ʱ��</param>
        /// <returns></returns>
        public object GetSafeCheckWarningByTime(ERCHTMS.Code.Operator user, string time,int mode=0)
        {
            return service.GetSafeCheckWarningByTime(user, time,mode);
        }
        #endregion

        #region ͳ��
          /// <summary>
        /// ͳ�ƶ��������糧�·����������ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        /// <returns></returns>
        public string getCheckTaskCount(Operator user,string startDate = "", string endDate="")
        {
            return service.getCheckTaskCount(user, startDate, endDate);
        }
        public DataTable getCheckTaskData(Operator user, string startDate = "", string endDate = "")
        {
            return service.getCheckTaskData(user, startDate, endDate);
        }
        #endregion
          /// <summary>
        /// ������ȫ����¼�ļ����Ա
        /// </summary>
        public void UpdateCheckUsers()
        {
            service.UpdateCheckUsers();
        }
    }
}
