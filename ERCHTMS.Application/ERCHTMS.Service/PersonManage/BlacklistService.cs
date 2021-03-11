using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class BlacklistService : RepositoryFactory<BlacklistEntity>, BlacklistIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns>�����б�</returns>
        public IEnumerable<BlacklistEntity> GetList(string userId)
        {
            return this.BaseRepository().IQueryable(t=>t.UserId==userId).ToList();
        }
        /// <summary>
        /// ��ȡ�����������������Ա
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetBlacklistUsers(ERCHTMS.Code.Operator user)
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("userid");
            dtTemp.Columns.Add("realname");
            dtTemp.Columns.Add("deptname");
            dtTemp.Columns.Add("itemname");
            dtTemp.Columns.Add("remark");
            DataTable dtItems = BaseRepository().FindTable(string.Format("select itemvalue,itemcode,remark from BIS_BLACKSET where status=1 and deptcode='{0}'", user.OrganizeCode));
            foreach (DataRow dr in dtItems.Rows)
            {
                DataTable dt =null;
                string sql = "";
                //�����ж�
                if (dr[1].ToString() == "01")
                {
                    string[] arr = dr[0].ToString().Split('|');
                    sql = string.Format("select userid,realname,u.DEPTNAME,'" + dr[2].ToString() + @"' itemname,('��������Ϊ��' || to_char(birthday,'yyyy-MM-dd')) remark from v_userinfo u where isblack=0 and  gender='{0}' and birthday is not null and u.DEPARTMENTCODE like '{3}%'
and (round(sysdate-to_date(to_char(u.birthday,'yyyy-MM-dd'),'yyyy-MM-dd'))/365<{1} or round(sysdate-to_date(to_char(u.birthday,'yyyy-MM-dd'),'yyyy-MM-dd'))/365>{2})", "��", arr[0], arr[1], user.DeptCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);

                    sql = string.Format("select userid,realname,u.DEPTNAME,'" + dr[2].ToString() + @"' itemname,('��������Ϊ��' || to_char(birthday,'yyyy-mm-dd')) remark  from v_userinfo u where isblack=0 and gender='{0}' and birthday is not null and u.DEPARTMENTCODE like '{3}%'
and (round(sysdate-to_date(to_char(u.birthday,'yyyy-mm-dd'),'yyyy-MM-dd'))/365<to_number({1}) or round(sysdate-to_date(to_char(u.birthday,'yyyy-mm-dd'),'yyyy-MM-dd'))/365>to_number({2})) ", "Ů", arr[2], arr[3], user.DeptCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
                 //һ��Υ��
                if (dr[1].ToString() == "03")
                {
                    sql = string.Format("select t.lllegalpersonid userid,t.lllegalperson realname,'" + dr[2].ToString() + @"' itemname,LLLEGALTEAM deptname,('һ��Υ�´���:' || count(1)) remark from V_LLLEGALBASEINFO t where t.flowstate='���̽���' and LLLEGALLEVEL='fc53ff18-b212-4763-9760-baf476eea5f3' and LLLEGALTEAMcode like '{1}%' and lllegalpersonid in(select userid from v_userinfo where isblack=0 and organizecode='{2}' ) group by 
lllegalpersonid,lllegalperson,LLLEGALTEAM having count(1)>{0}",  dr[0].ToString(), user.DeptCode,user.OrganizeCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
                //����Υ��
                if (dr[1].ToString() == "04")
                {
                    sql = string.Format(@"select t.lllegalpersonid userid,t.lllegalperson realname,'" + dr[2].ToString() + @"' itemname,LLLEGALTEAM deptname,('����Υ��:' || count(1)) remark from V_LLLEGALBASEINFO t where t.flowstate='���̽���' and LLLEGALLEVEL='5aae9e88-c06d-4383-afec-6165d5c1a312' and LLLEGALTEAMcode like '{1}%' and lllegalpersonid in(select userid from v_userinfo where isblack=0 and organizecode='{2}' ) group by 
lllegalpersonid,lllegalperson,LLLEGALTEAM having count(1)>{0}", dr[0].ToString(), user.DeptCode,user.OrganizeCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
            }
            return dtTemp;
        }
        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //״̬
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                pagination.conditionJson += string.Format(" and status ={0}", status);
            }
            //����Code
            if (!queryParam["areaCode"].IsEmpty())
            {
                string areaCode = queryParam["areaCode"].ToString();
                pagination.conditionJson += string.Format(" and areaCode like '{0}%'", areaCode);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public BlacklistEntity GetEntity(string keyValue)
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
            BlacklistEntity entity = this.BaseRepository().FindEntity(keyValue);
            entity.DeleteMark = entity.EnableMark = 1;
            if(this.BaseRepository().Update(entity)>0)
            {
                this.BaseRepository().ExecuteBySql(string.Format("update base_user set isblack=0 where userid='{0}'", entity.UserId));
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, BlacklistEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                string[] arr = entity.UserId.Split(',');
                List<BlacklistEntity> list = new List<BlacklistEntity>();
                foreach(string userId in arr)
                {
                    BlacklistEntity newEntity = new BlacklistEntity();
                    newEntity.Create();
                    newEntity.UserId = userId;
                    newEntity.Reason = entity.Reason;
                    newEntity.JoinTime = DateTime.Now;
                    list.Add(newEntity);
                }
                if(this.BaseRepository().Insert(list)>0)
                {
                    this.BaseRepository().ExecuteBySql(string.Format("update base_user set isblack=1 where userid in('{0}')", entity.UserId.Replace(",","','")));
                }
            }
        }
        #endregion
    }
}
