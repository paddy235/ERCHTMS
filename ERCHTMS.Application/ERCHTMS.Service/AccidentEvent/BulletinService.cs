using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.IService.AccidentEvent;
using System.Linq.Expressions;
using ERCHTMS.Service.CommonPermission;

namespace ERCHTMS.Service.AccidentEvent
{
    /// <summary>
    /// �� �����¹��¼��챨
    /// </summary>
    public class BulletinService : RepositoryFactory<BulletinEntity>, IBulletinService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<BulletinEntity> GetListForCon(Expression<Func<BulletinEntity, bool>> condition)
        {

            return this.BaseRepository().IQueryable(condition).ToList();
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["keyword"].IsEmpty())
                {

                    string keyord = queryParam["keyword"].ToString();
                    //keyord
                    pagination.conditionJson += string.Format(" and (SGNAME  like '%{0}%' or SGKBUSERNAME like '%{1}%' or AREANAME like '%{2}%') ", keyord, keyord, keyord);
                }

                if (!queryParam["sgtypeLevel"].IsEmpty())
                {
                    string sgtypeLevel = queryParam["sgtypeLevel"].ToString();
                    pagination.conditionJson += " and sgtype='" + sgtypeLevel + "'";
                }

                //�¹�ԭ��
                if (!queryParam["sgyy"].IsEmpty())
                {
                    string keyord = queryParam["sgyy"].ToString();
                    if (keyord == "IsXW")
                        pagination.conditionJson += string.Format(" and BAQXWNAME is not null ");
                    else if (keyord == "IsZT")
                        pagination.conditionJson += string.Format(" and BAQZTNAME is not null ");
                    else
                        pagination.conditionJson += string.Format(" and (JJYYNAME  like '%{0}%' or BAQXWNAME like '%{1}%' or BAQZTNAME like '%{2}%') ", keyord, keyord, keyord);
                }
                if (!queryParam["sgtype"].IsEmpty())
                {
                    string sgtype = queryParam["sgtype"].ToString();
                    pagination.conditionJson += string.Format(" and sgtype = '{0}'", sgtype);
                } if (!queryParam["sgtypename"].IsEmpty())
                {
                    string sgtypename = queryParam["sgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and sgtypename = '{0}'", sgtypename);
                }

                if (!queryParam["rsshsgtypename"].IsEmpty())
                {
                    string rsshsgtypename = queryParam["rsshsgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtypename = '{0}'", rsshsgtypename);
                }
                if (!queryParam["sglevelname"].IsEmpty())
                {
                    string sglevelname = queryParam["sglevelname"].ToString();
                    pagination.conditionJson += string.Format(" and sglevelname = '{0}'", sglevelname);
                }
                if (!queryParam["rsshsgtype_deal"].IsEmpty())
                {
                    string rsshsgtype_deal = queryParam["rsshsgtype_deal"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtype_deal = '{0}'", rsshsgtype_deal);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    if (happentimeend.Length == 10)
                        happentimeend = Convert.ToDateTime(happentimeend).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }
                //����ʱ��
                if (!queryParam["happentimestart_deal"].IsEmpty())
                {
                    string happentimestart_deal = queryParam["happentimestart_deal"].ToString();
                    pagination.conditionJson += string.Format(" and happentime_deal >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart_deal);
                }
                if (!queryParam["happentimeend_deal"].IsEmpty())
                {
                    string happentimeend_deal = queryParam["happentimeend_deal"].ToString();
                    if (happentimeend_deal.Length == 10)
                        happentimeend_deal = Convert.ToDateTime(happentimeend_deal).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime_deal <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend_deal);
                }
                if (!queryParam["IsSubmit"].IsEmpty())
                {
                    var IsSubmit = int.Parse(queryParam["IsSubmit"].ToString());
                    pagination.conditionJson += string.Format(" and IsSubmit = '{0}'", IsSubmit);
                }

                if (!queryParam["IsSubmit_Deal"].IsEmpty())
                {
                    var IsSubmit_Deal = int.Parse(queryParam["IsSubmit_Deal"].ToString());
                    if (IsSubmit_Deal == 0)
                        pagination.conditionJson += string.Format(" and ( IsSubmit_Deal = '{0}' OR IsSubmit_Deal is null)", IsSubmit_Deal);
                    else
                        pagination.conditionJson += string.Format(" and IsSubmit_Deal = '{0}'", IsSubmit_Deal);
                }
                //��������
                if (!queryParam["type"].IsEmpty())
                {
                    var type = queryParam["type"].ToString();
                    if (type == "swrs")
                        pagination.conditionJson += string.Format(" and sgtypename like '%���������¹�%'");
                }
                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetGenericPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["keyword"].IsEmpty())
                {

                    string keyord = queryParam["keyword"].ToString();
                    //keyord
                    pagination.conditionJson += string.Format(" and (SGNAME  like '%{0}%' or SGKBUSERNAME like '%{1}%' or AREANAME like '%{2}%') ", keyord, keyord, keyord);
                }

                if (!queryParam["sgtypeLevel"].IsEmpty())
                {
                    string sgtypeLevel = queryParam["sgtypeLevel"].ToString();
                    pagination.conditionJson += " and sgtype='" + sgtypeLevel + "'";
                }

                //�¹�ԭ��
                if (!queryParam["sgyy"].IsEmpty())
                {
                    string keyord = queryParam["sgyy"].ToString();
                    if (keyord == "IsXW")
                        pagination.conditionJson += string.Format(" and BAQXWNAME is not null ");
                    else if (keyord == "IsZT")
                        pagination.conditionJson += string.Format(" and BAQZTNAME is not null ");
                    else
                        pagination.conditionJson += string.Format(" and (JJYYNAME  like '%{0}%' or BAQXWNAME like '%{1}%' or BAQZTNAME like '%{2}%') ", keyord, keyord, keyord);
                }
                if (!queryParam["sgtype"].IsEmpty())
                {
                    string sgtype = queryParam["sgtype"].ToString();
                    pagination.conditionJson += string.Format(" and RSSHSGTYPE = '{0}'", sgtype);
                } if (!queryParam["sgtypename"].IsEmpty())
                {
                    string sgtypename = queryParam["sgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and sgtypename = '{0}'", sgtypename);
                }

                if (!queryParam["rsshsgtypename"].IsEmpty())
                {
                    string rsshsgtypename = queryParam["rsshsgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtypename = '{0}'", rsshsgtypename);
                }
                if (!queryParam["sglevelname"].IsEmpty())
                {
                    string sglevelname = queryParam["sglevelname"].ToString();
                    pagination.conditionJson += string.Format(" and sglevelname = '{0}'", sglevelname);
                }
                if (!queryParam["rsshsgtype_deal"].IsEmpty())
                {
                    string rsshsgtype_deal = queryParam["rsshsgtype_deal"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtype_deal = '{0}'", rsshsgtype_deal);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    if (happentimeend.Length == 10)
                        happentimeend = Convert.ToDateTime(happentimeend).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }
                //����ʱ��
                if (!queryParam["happentimestart_deal"].IsEmpty())
                {
                    string happentimestart_deal = queryParam["happentimestart_deal"].ToString();
                    pagination.conditionJson += string.Format(" and happentime_deal >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart_deal);
                }
                if (!queryParam["happentimeend_deal"].IsEmpty())
                {
                    string happentimeend_deal = queryParam["happentimeend_deal"].ToString();
                    if (happentimeend_deal.Length == 10)
                        happentimeend_deal = Convert.ToDateTime(happentimeend_deal).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime_deal <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend_deal);
                }
                if (!queryParam["IsSubmit"].IsEmpty())
                {
                    var IsSubmit = int.Parse(queryParam["IsSubmit"].ToString());
                    pagination.conditionJson += string.Format(" and IsSubmit = '{0}'", IsSubmit);
                }

                if (!queryParam["IsSubmit_Deal"].IsEmpty())
                {
                    var IsSubmit_Deal = int.Parse(queryParam["IsSubmit_Deal"].ToString());
                    if (IsSubmit_Deal == 0)
                        pagination.conditionJson += string.Format(" and ( IsSubmit_Deal = '{0}' OR IsSubmit_Deal is null)", IsSubmit_Deal);
                    else
                        pagination.conditionJson += string.Format(" and IsSubmit_Deal = '{0}'", IsSubmit_Deal);
                }
                //��������
                if (!queryParam["type"].IsEmpty())
                {
                    var type = queryParam["type"].ToString();
                    if (type == "swrs")
                        pagination.conditionJson += string.Format(" and sgtypename like '%���������¹�%'");
                }
                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<BulletinEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from v_aem_Bulletin_deal where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public BulletinEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, BulletinEntity entity)
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
