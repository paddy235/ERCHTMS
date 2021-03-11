using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using System.Dynamic;
using ERCHTMS.Code;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ��ڰ�
    /// </summary>
    public class SecurityRedListService : RepositoryFactory<SecurityRedListEntity>, SecurityRedListIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["state"].IsEmpty()) {
                pagination.conditionJson += string.Format(" and state='{0}'", queryParam["state"].ToString());
            }
            //ʱ�䷶Χ
            if (!queryParam["sTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and releasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //��ѯ����
            if (!queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and title like '%{0}%'", queryParam["txtSearch"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SecurityRedListEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SecurityRedListEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ��ȫ��ڰ�ͳ������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public object GetSecurityRedListStat(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            dynamic dy = new ExpandoObject();
            var queryParam = queryJson.ToJObject();
            string sqlWhere = string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                string deptCode = queryParam["code"].ToString();
                if (queryParam["isOrg"].ToString() == "Organize")
                {
                    
                }
                else
                {
                    sqlWhere += string.Format(" and PublisherDeptCode like '{0}%'", deptCode);
                }
            }
            if (!queryParam["year"].IsEmpty())
            {
                //��ȡ�¶�ͳ�������Ϣ
                string year = queryParam["year"].ToString();
                
                string m = "[";//�¶�
                string red = "[";//�������
                string black = "[";//�ڰ�����
                string sql = string.Empty;
                string sqlWhere1 = string.Empty;
                for (int i = 1; i < 13; i++)
                {
                    string mm = i.ToString();
                    if (i < 10)
                    {
                        mm = "0" + i.ToString();
                    }
                    m += i + ",";
                    if (year != "ȫ��")
                    {
                        sqlWhere1 = sqlWhere + string.Format(" and to_char(ReleaseTime,'yyyy-MM')='{0}'", year + "-" + mm);
                    }
                    else {
                        sqlWhere1 = sqlWhere + string.Format(" and to_char(ReleaseTime,'MM')='{0}'", mm);
                    }
                    sql = string.Format(@"select count(id) from BIS_SecurityRedList where IsSend='0' and State='0' {0}", sqlWhere1);
                    red += this.BaseRepository().FindObject(sql).ToString() + ",";
                    sql = string.Format(@"select count(id) from BIS_SecurityRedList where IsSend='0' and State='1' {0}", sqlWhere1);
                    black += this.BaseRepository().FindObject(sql).ToString() + ",";
                }
                m = m.Substring(0, m.Length - 1) + "]";
                red = red.Substring(0, red.Length - 1) + "]";
                black = black.Substring(0, black.Length - 1) + "]";
                dy.y = m;
                dy.red = red;
                dy.black = black;
            }
            return dy;
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
        public void SaveForm(string keyValue, SecurityRedListEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                SecurityRedListEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
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
