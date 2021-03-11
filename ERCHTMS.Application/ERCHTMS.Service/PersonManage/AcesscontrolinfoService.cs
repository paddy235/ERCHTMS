using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// �� �����û��Ž�������
    /// </summary>
    public class AcesscontrolinfoService : RepositoryFactory<AcesscontrolinfoEntity>, AcesscontrolinfoIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�û�����ָ����������
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        public DataTable GetUserList(string userids)
        {
            string[] users=userids.Split(',');
            string userid = "";
            for (int i = 0; i < users.Length; i++)
            {
                if (i == 0)
                {
                    userid = "'" + users[i] + "'";
                }
                else
                {
                    userid += ",'" + users[i] + "'";
                }
            }
            string sql = string.Format(@"select u.userid,realname,mobile,isfinger,isface,tid from base_user u
            left join BIS_ACESSCONTROLINFO ace on u.userid=ace.userid
            where u.userid in ({0})", userid);
            return BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AcesscontrolinfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AcesscontrolinfoEntity GetEntity(string keyValue)
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, AcesscontrolinfoEntity entity)
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
