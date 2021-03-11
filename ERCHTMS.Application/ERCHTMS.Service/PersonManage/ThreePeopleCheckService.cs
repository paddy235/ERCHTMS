using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Service.SystemManage;
using BSFramework.Data;
namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    public class ThreePeopleCheckService : RepositoryFactory<ThreePeopleCheckEntity>, ThreePeopleCheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ThreePeopleCheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty() && !queryParam["condition"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString().Trim();
                string condition = queryParam["condition"].ToString();
                pagination.conditionJson += string.Format(" and t.{0} like '%{1}%'", condition, keyword);
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;

        }
        /// <summary>
        /// ��ȡ��Ա��Ϣ
        /// </summary>
        /// <param name="applyId">�����Id</param>
        /// <returns></returns>
        public IEnumerable<ThreePeopleInfoEntity> GetUserList(string applyId)
        {
            return new Repository<ThreePeopleInfoEntity>(DbFactory.Base()).FindList("select *from BIS_THREEPEOPLEINFO where applyid='" + applyId+"'");
        }
        public DataTable GetItemList(string id)
        {
            return this.BaseRepository().FindTable(string.Format("select *from BIS_THREEPEOPLEINFO where applyid='{0}'",id));
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ThreePeopleCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <returns></returns>
        public string GetSno(string orgCode)
        {
            string code = "001";
            string sql = string.Format("select APPLYSNO from BIS_THREEPEOPLECHECK where CreateUserOrgCode='{1}' and APPLYSNO like '%{0}%' order by CREATETIME desc", DateTime.Now.ToString("yyyyMMdd"),orgCode);
            DataTable tb = this.BaseRepository().FindTable(sql);
            if (tb.Rows.Count > 0)
            {
                int number = tb.Rows.Count + 1;
                if (number < 10)
                {
                   code="00" + number.ToString();
                }
                else if (number < 100)
                {
                    code="0" + number.ToString();
                }
                else
                {
                    code= number.ToString();
                }
            }
            else
            {
                code= "001";
            }
            string val = new DataItemDetailService().GetItemValue("�����˱��");
            if (string.IsNullOrEmpty(val))
            {
                code = "Q/CRPHZHB 2203.01.02-JL11-" + DateTime.Now.ToString("yyyyMMdd") + code;
            }
            else
            {
                code= val+"-" + DateTime.Now.ToString("yyyyMMdd") + code;
            }
            return code;
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ThreePeopleCheckEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            if (entity.IsOver == 1)
            {
                DataTable dt = BaseRepository().FindTable(string.Format("select tickettype,idcard from BIS_THREEPEOPLEINFO where applyid='{0}'", entity.Id));
                foreach (DataRow dr in dt.Rows)
                {
                    BaseRepository().ExecuteBySql(string.Format("update base_user set isfourperson='��',FOURPERSONTYPE='{0}' where IDENTIFYID='{1}'", dr[0].ToString(), dr[1].ToString()));
                }
            }
        }
        public bool SaveForm(string keyValue, ThreePeopleCheckEntity entity,List<ThreePeopleInfoEntity> list,AptitudeinvestigateauditEntity auditInfo=null)
        {
            try
            {
                int count = 0;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Id = keyValue;
                    var tp = BaseRepository().FindEntity(keyValue);
                    if (tp == null)
                    {
                        entity.Create();
                        entity.ApplySno = GetSno(entity.CreateUserOrgCode);
                        count = this.BaseRepository().Insert(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        count = this.BaseRepository().Update(entity);
                    }
                }
                else
                {
                    entity.Create();
                    entity.ApplySno = GetSno(entity.CreateUserOrgCode);
                    count = this.BaseRepository().Insert(entity);
                }
                if (count > 0)
                {
                    if (auditInfo!=null)
                    {
                        if (auditInfo.AUDITRESULT=="0")
                        {
                            if (entity.IsOver == 1)
                            {
                                DataTable dt = BaseRepository().FindTable(string.Format("select tickettype,idcard from BIS_THREEPEOPLEINFO where applyid='{0}'", entity.Id));
                                foreach (DataRow dr in dt.Rows)
                                {
                                    BaseRepository().ExecuteBySql(string.Format("update base_user set isfourperson='��',FOURPERSONTYPE='{0}' where IDENTIFYID='{1}'", dr[0].ToString(), dr[1].ToString()));
                                }
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_THREEPEOPLEINFO where applyid='{0}'", entity.Id));
                        new RepositoryFactory<ThreePeopleInfoEntity>().BaseRepository().Insert(list);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
        #endregion

        #region  ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string applyId="")
        {
            ManyPowerCheckEntity nextCheck = null;//��һ�����
            IManyPowerCheckService powerCheck = new ManyPowerCheckService();
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);
            bool hasRight = false;
            string deptId = "-1";
            ThreePeopleCheckEntity entity = new ThreePeopleCheckEntity();
            if (!string.IsNullOrEmpty(applyId))
            {
                deptId = createdeptid;
                entity = BaseRepository().FindEntity(applyId);
                if (entity!=null)
                {
                    if (!string.IsNullOrEmpty(entity.NodeId))
                    {
                        ManyPowerCheckEntity mp = powerCheck.GetEntity(entity.NodeId);
                        if (mp != null)
                        {
                            if (mp.CHECKDEPTID == "-1")
                            {
                                if (entity.ApplyType == "�ڲ�")
                                {
                                    deptId=mp.CHECKDEPTID = entity.BelongDeptId;
                                }
                                else
                                {
                                    string sql = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'",entity.ProjectId);
                                    DataTable dt=BaseRepository().FindTable(sql);
                                    if (dt.Rows.Count>0)
                                    {
                                       deptId= mp.CHECKDEPTID=dt.Rows[0][0].ToString();
                                    }
                                }
                                if (mp.CHECKDEPTID== currUser.DeptId)
                                {
                                    var rolelist = currUser.RoleName.Split(',');
                                    for (int j = 0; j < rolelist.Length; j++)
                                    {
                                        if (mp.CHECKROLENAME.Contains(rolelist[j]))
                                        {
                                            hasRight = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (mp.CHECKDEPTID == "-3")
                            {
                                deptId = entity.CreateUserDeptId;
                                if (entity.CreateUserDeptId == currUser.DeptId)
                                {
                                    var rolelist = currUser.RoleName.Split(',');
                                    for (int j = 0; j < rolelist.Length; j++)
                                    {
                                        if (mp.CHECKROLENAME.Contains(rolelist[j]))
                                        {
                                            hasRight = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                deptId = mp.CHECKDEPTID;
                                if (mp.CHECKDEPTID == currUser.DeptId)
                                {
                                    var rolelist = currUser.RoleName.Split(',');
                                    for (int j = 0; j < rolelist.Length; j++)
                                    {
                                        if (mp.CHECKROLENAME.Contains(rolelist[j]))
                                        {
                                            hasRight = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    hasRight = false;
                                }
                             
                            }
                        }
                    }
                }
            }
            if (powerList.Count > 0)
            {
                var dept = new DepartmentService().GetEntity(deptId);
                if (dept != null)
                {
                    foreach (var item in powerList)
                    {
                        if (item.CHECKDEPTID == "-3" || item.CHECKDEPTID == "-1")
                        {
                            item.CHECKDEPTID = deptId;
                            item.CHECKDEPTCODE = dept.DeptCode;
                            item.CHECKDEPTNAME = dept.FullName;
                        }
                         
                    }
                    List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                    //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].CHECKDEPTID == currUser.DeptId)
                        {
                            var rolelist = currUser.RoleName.Split(',');
                            for (int j = 0; j < rolelist.Length; j++)
                            {
                                if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                                {
                                    checkPower.Add(powerList[i]);
                                    break;
                                }
                            }
                        }
                    }
                    powerList.GroupBy(t => t.SERIALNUM).ToList().Count();
                    if (checkPower.Count > 0)
                    {
                        if(!string.IsNullOrEmpty(applyId))
                        {
                            if (hasRight)
                            {
                                state = "1";
                            }
                            else
                            {
                                state = "0";
                            }
                        }
                        else
                        {
                            state = "1";
                        }
                        
                        ManyPowerCheckEntity check = checkPower.Last();//��ǰ

                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (check.ID == powerList[i].ID)
                            {
                                if ((i + 1) >= powerList.Count)
                                {
                                    nextCheck = null;
                                }
                                else
                                {
                                    nextCheck = powerList[i + 1];
                                }
                            }
                        }
                    }
                    else
                    {
                        state = "0";
                        nextCheck = powerList.First();
                    }

                    if (null != nextCheck)
                    {
                        //��ǰ�������µĶ�Ӧ����
                        var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                        //���ϼ�¼����1�����ʾ���ڲ�����ˣ���飩�����
                        if (serialList.Count() > 1)
                        {
                            string flowdept = string.Empty;  // ��ȡֵ��ʽ a1,a2
                            string flowdeptname = string.Empty; // ��ȡֵ��ʽ b1,b2
                            string flowrole = string.Empty;   // ��ȡֵ��ʽ c1|c2|  (c1���ݹ��ɣ� cc1,cc2,cc3)
                            string flowrolename = string.Empty; // ��ȡֵ��ʽ d1|d2| (d1���ݹ��ɣ� dd1,dd2,dd3)

                            ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                            slastEntity = serialList.LastOrDefault();
                            foreach (ManyPowerCheckEntity model in serialList)
                            {
                                flowdept += model.CHECKDEPTID + ",";
                                flowdeptname += model.CHECKDEPTNAME + ",";
                                flowrole += model.CHECKROLEID + "|";
                                flowrolename += model.CHECKROLENAME + "|";
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                            }
                            if (!flowdeptname.IsEmpty())
                            {
                                slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                            }
                            nextCheck = slastEntity;
                        }
                    }
                    return nextCheck;
                }
                else
                {
                    state = "0";
                    return nextCheck;
                }

            }
            else
            {
                state = "0";
                return nextCheck;
            }

        }
        #endregion
    }
}
