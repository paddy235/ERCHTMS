using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤������
    /// </summary>
    public class SafetyworksuperviseService : RepositoryFactory<SafetyworksuperviseEntity>, SafetyworksuperviseIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyworksuperviseEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            if (!string.IsNullOrEmpty(pagination.p_tablename))
            {

            }
            else {
                pagination.conditionJson += string.Format(" and (t.flowstate!='0' or t.createuserid='{0}')", user.UserId);
                if (user.RoleName.Contains("ʡ���û�") || user.RoleName.Contains("�����û�"))
                {
                    pagination.conditionJson += string.Format(@" and  i.deptcode  like '{0}%' ", user.OrganizeCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and  t.createuserorgcode='{0}'", user.OrganizeCode);
                }

                #region ���
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.CreateUserId,t.DutyPersonId,t.SupervisePersonId,t.FlowState,to_char(t.SuperviseDate,'yyyy-MM-dd') as SuperviseDate,
                        t.WorkTask,t.DutyDeptName,t.DutyPerson,t.SupervisePerson,to_char(t.FinishDate,'yyyy-MM-dd') as FinishDate,t.Remark,t1.id as fid,t1.FinishInfo,t1.SignUrl,t2.SignUrl as SignUrlT,
(select count(1) from BIS_SuperviseConfirmation c2 where c2.flag='1' and c2.superviseid=t.id) as btgnum";
                pagination.p_tablename = @"BIS_SafetyWorkSupervise t left join (select * from
( select id, superviseid,FinishInfo,SignUrl ,row_number() over(partition by superviseid order by autoid desc ) rn 
from BIS_SafetyWorkFeedback) where rn=1) t1 
on t.id=t1.superviseid left join (select c1.* from BIS_SuperviseConfirmation c1 where c1.flag='0') t2 
on t1.id=t2.feedbackid left join base_department i on t.dutydeptcode = i.encode ";
                if (pagination.sidx == null)
                {
                    pagination.sidx = "t.createdate";
                }
                if (pagination.sord == null)
                {
                    pagination.sord = "desc";
                }
                #endregion
            }
            
            //����ʱ��
            if (!queryParam["supervisedate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and to_char(t.supervisedate,'yyyy-MM')='{0}'", queryParam["supervisedate"].ToString());

            }
            //��ѯ����
            if (!queryParam["title"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.title like '%{0}%'", queryParam["title"].ToString());
            }
            if (!queryParam["code"].IsEmpty())
            {
                string deptCode = queryParam["code"].ToString();
                var status = "";
                if (!queryParam["flowstate"].IsEmpty()) {
                    status = queryParam["flowstate"].ToString();
                    if (!queryParam["appflag"].IsEmpty()) {
                        status = "";
                    }
                }
                if (status == "-1")
                {
                    //����ҳͳ��ͼ��ת�����ű�������
                    pagination.conditionJson += string.Format(" and t.dutydeptcode = '{0}'", deptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.dutydeptcode like '%{0}%'", deptCode);
                }

            }
            //�鿴��Χ
            if (!queryParam["showrange"].IsEmpty())
            {
                var showRange = queryParam["showrange"].ToString();
                if (showRange == "1")//�����˴����
                {
                    pagination.conditionJson += string.Format(" and ((t.dutypersonid='{0}' and t.flowstate='1') or (t.supervisepersonid='{0}' and t.flowstate='2') or (t.createuserid='{0}' and t.flowstate='0')) ", user.UserId);
                }
                else if (showRange == "2")//���˴�����
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
            }
            //״̬
            if (!queryParam["flowstate"].IsEmpty())
            {
                var status = queryParam["flowstate"].ToString();
                if (!status.IsNullOrWhiteSpace()) {
                    if (status == "-1")
                    {
                        pagination.conditionJson += " and t.flowstate!=0";
                    }
                    else {
                        pagination.conditionJson += string.Format(" and t.flowstate='{0}'", status);
                    }
                    
                } 
            }
            //��������
            if (!queryParam["keyword"].IsEmpty())
            {
                var keyword = queryParam["keyword"].ToString();
                if (!keyword.IsNullOrWhiteSpace()) pagination.conditionJson += string.Format(" and t.worktask like '%{0}%'", keyword);
            }

            //��ҳԤ��ָ��0��ʾ��������,1��ʾ����
            if (!queryParam["yjtype"].IsEmpty())
            {
                var yjtype = queryParam["yjtype"].ToString();
                if (!yjtype.IsNullOrWhiteSpace()) {
                    if (yjtype == "0")
                    {
                        pagination.conditionJson += " and t.flowstate='1' and (t.finishdate - 2 <= sysdate  and sysdate <= t.finishdate + 1)";
                    }
                    else {
                        pagination.conditionJson += string.Format(" and t.flowstate='1' and to_date('{0}','yyyy-mm-dd hh24:mi:ss')>t.finishdate+1", DateTime.Now);
                    }
                }
                    
            }
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyworksuperviseEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��/�����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="fid">������ʷ����id</param>
        /// <returns></returns>
        public DataTable GetEntityByT(string keyValue, string fid)
        {
            string sql = string.Format(@"select t.*,t1.id as fid,t1.FinishInfo,t1.FeedbackDate,t1.SignUrl,t1.CreateDate as FCreateDate,t2.id as cid,t2.SuperviseResult,t2.SuperviseOpinion,t2.ConfirmationDate,t2.SignUrl as SignUrlT,t2.CreateDate as CCreateDate
 from BIS_SafetyWorkSupervise t left join (select * from  BIS_SafetyWorkFeedback where flag='0') t1 
on t.id=t1.superviseid left join (select * from BIS_SuperviseConfirmation where flag='0') t2 on t1.id=t2.feedbackid
where t.id='{0}'", keyValue);
            if (!string.IsNullOrEmpty(fid)) {
                sql = string.Format(@"select t.*,t1.id as fid,t1.FinishInfo,t1.FeedbackDate,t1.SignUrl,t1.CreateDate as FCreateDate,t2.id as cid,t2.SuperviseResult,t2.SuperviseOpinion,t2.ConfirmationDate,t2.SignUrl as SignUrlT,t2.CreateDate as CCreateDate
 from BIS_SafetyWorkSupervise t left join (select * from BIS_SafetyWorkFeedback where id='{1}') t1 
on t.id=t1.superviseid left join (select * from BIS_SuperviseConfirmation  where feedbackid='{1}') t2 on t1.id=t2.feedbackid
where t.id='{0}'", keyValue, fid);
            }
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// ����ͼ��Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue) {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            DataTable entity = GetEntityByT(keyValue, "");
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            flow.activeID = entity.Rows[0]["FlowState"].ToString();
            string[] flowname = new string[] { "�����·�", "���β���(��λ)������", "�����˶���ȷ��"};
            string FlowState = entity.Rows[0]["FlowState"].ToString();
            string userid = entity.Rows[0]["CreateUserId"].ToString();
            UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(userid);
            for (int i = 0; i < 3; i++)
            {
                nodes nodes = new nodes();
                nodes.alt = true;
                nodes.isclick = false;
                nodes.css = "";
                nodes.id = entity.Rows[0]["Id"].ToString() + i; //����
                nodes.img = "";
                nodes.name = flowname[i];
                nodes.type = "stepnode";
                nodes.width = 150;
                nodes.height = 60;
                //λ��
                int m = i % 4;
                int n = i / 4;
                if (m == 0)
                {
                    nodes.left = 120;
                }
                else
                {
                    nodes.left = 120 + ((150 + 60) * m);
                }
                if (n == 0)
                {
                    nodes.top = 54;
                }
                else
                {
                    nodes.top = (n * 100) + 54;
                }
                setInfo sinfo = new setInfo();
                if (int.Parse(FlowState) > i)
                {
                    sinfo.Taged = 1;
                }
                else if (int.Parse(FlowState) == i)
                {
                    sinfo.Taged = 0;
                }
                sinfo.NodeName = nodes.name;
                List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                NodeDesignateData nodedesignatedata = new NodeDesignateData();
                if (i == 0)
                {
                    nodedesignatedata.createdate = entity.Rows[0]["CreateDate"].ToString();//����ʱ��
                    nodedesignatedata.creatdept = uInfor.DeptName;//����λ
                    nodedesignatedata.createuser = entity.Rows[0]["CreateUserName"].ToString();//������
                    nodedesignatedata.status = "��";//����״̬
                    if (i == 0)
                    {
                        nodedesignatedata.prevnode = "��";
                    }
                    else
                    {
                        nodedesignatedata.prevnode = flowname[i - 1];//��һ����
                    }
                }
                else if (i == 1)
                {
                    nodedesignatedata.createdate = string.IsNullOrEmpty(entity.Rows[0]["FCreateDate"].ToString()) ? "��" : entity.Rows[0]["FeedbackDate"].ToString();//����ʱ��
                    nodedesignatedata.creatdept = entity.Rows[0]["DutyDeptName"].ToString();//����λ
                    nodedesignatedata.createuser = entity.Rows[0]["DutyPerson"].ToString();//������
                    nodedesignatedata.status = "��";//����״̬
                    nodedesignatedata.prevnode = flowname[i - 1];//��һ����
                }
                else if (i == 2) {

                    nodedesignatedata.createdate = string.IsNullOrEmpty(entity.Rows[0]["CCreateDate"].ToString()) ? "��" : entity.Rows[0]["ConfirmationDate"].ToString();//����ʱ��
                    nodedesignatedata.creatdept = entity.Rows[0]["SuperviseDeptName"].ToString();//����λ
                    nodedesignatedata.createuser = entity.Rows[0]["SupervisePerson"].ToString();//������
                    nodedesignatedata.status = "��";//����״̬
                    nodedesignatedata.prevnode = flowname[i - 1];//��һ����
                }
                

                nodelist.Add(nodedesignatedata);
                sinfo.NodeDesignateData = nodelist;
                nodes.setInfo = sinfo;
                nlist.Add(nodes);
            }
            //���̽����ڵ�
            nodes nodes_end = new nodes();
            nodes_end.alt = true;
            nodes_end.isclick = false;
            nodes_end.css = "";
            nodes_end.id = Guid.NewGuid().ToString();
            nodes_end.img = "";
            nodes_end.name = "���̽���";
            nodes_end.type = "endround";
            nodes_end.width = 150;
            nodes_end.height = 60;
            //ȡ���һ���̵�λ�ã������λ
            nodes_end.left = nlist[nlist.Count - 1].left;
            nodes_end.top = nlist[nlist.Count - 1].top + 100;
            if (FlowState == "3") {
                setInfo sinfo = new setInfo();
                sinfo.NodeName = nodes_end.name;
                sinfo.Taged = 1;
                nodes_end.setInfo = sinfo;
            }
            nlist.Add(nodes_end);
            
            #region ����line����

            for (int i = 0; i < 3; i++)
            {
                lines lines = new lines();
                lines.alt = true;
                lines.id = Guid.NewGuid().ToString();
                lines.from = entity.Rows[0]["Id"].ToString() + i;
                if (i == 2)
                {
                    lines.to = nodes_end.id;
                }
                else {
                    lines.to = entity.Rows[0]["Id"].ToString() + (i + 1);
                }
                
                lines.name = "";
                lines.type = "sl";
                llist.Add(lines);
            }

            lines lines_end = new lines();
            lines_end.alt = true;
            lines_end.id = Guid.NewGuid().ToString();
            lines_end.from = entity.Rows[0]["Id"].ToString() + "3";
            lines_end.to = nodes_end.id;
            llist.Add(lines_end);
            #endregion

            flow.nodes = nlist;
            flow.lines = llist;
            return flow;
        }
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetSuperviseNum(string userid,string type) {
            int count = 0;
            try
            {
                string sql = "select count(1) from BIS_SafetyWorkSupervise t {0}";
                string sqlWhere = string.Empty;
                if (type == "1")
                {
                    sqlWhere = string.Format(@" where t.flowstate='1' and t.dutypersonid='{0}' ", userid);
                }
                else
                {
                    sqlWhere = string.Format(@" where t.flowstate='2' and t.supervisepersonid='{0}' ", userid);
                }
                sql = string.Format(sql, sqlWhere);
                count = BaseRepository().FindObject(sql).ToInt();
            }
            catch
            {
                return 0;
            }
            return count;
        }

        /// <summary>
        /// ��ȡ��ҳͳ��ͼ
        /// </summary>
        /// <param name="keyValue">1��ʾ�ϸ���</param>
        /// <returns></returns>
        public DataTable GetSuperviseStat(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DateTime time = DateTime.Now;
            string timestr = time.ToString("yyyy-MM");
            if (keyValue == "1") {
                DateTime t2 = time.AddMonths(-1);
                timestr = t2.ToString("yyyy-MM");
            }
            string sql = string.Format(@"select t2.*,round(t2.ybhnum/t2.num*100,1) as lv from (select t.dutydeptcode,t.dutydeptid, t.dutydeptname,t.num,(select count(1) 
from BIS_SafetyWorkSupervise t1 
where t1.dutydeptid=t.dutydeptid and  t1.flowstate!='0' and t1.flowstate!='1' and to_char(t1.supervisedate,'yyyy-MM')='{0}' ) as yblnum,
(select count(1) from BIS_SafetyWorkSupervise t1 
where t1.dutydeptid=t.dutydeptid and  t1.flowstate='1' and sysdate-1>t1.finishdate and to_char(t1.supervisedate,'yyyy-MM')='{0}' ) as yqnum,
(select count(1) from BIS_SafetyWorkSupervise t1 
where t1.dutydeptid=t.dutydeptid and  t1.flowstate='3' and to_char(t1.supervisedate,'yyyy-MM')='{0}' ) as ybhnum
 from (select t.dutydeptcode,t.dutydeptname,t.dutydeptid,count(1) as num from BIS_SafetyWorkSupervise t
 where to_char(t.supervisedate,'yyyy-MM')='{0}' and flowstate!='0'
 group by t.dutydeptcode,t.dutydeptname,t.dutydeptid ) t ) t2 
 where t2.dutydeptcode  like '{1}%' order by LV desc", timestr, user.OrganizeCode);
            return this.BaseRepository().FindTable(sql);
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyworksuperviseEntity entity)
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
        }
        #endregion
    }
}
