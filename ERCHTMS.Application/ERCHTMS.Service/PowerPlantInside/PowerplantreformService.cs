using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼���������
    /// </summary>
    public class PowerplantreformService : RepositoryFactory<PowerplantreformEntity>, PowerplantreformIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["sgtype"].IsEmpty())
                {
                    string sgtype = queryParam["sgtype"].ToString();
                    pagination.conditionJson += string.Format(" and accidenteventtype = '{0}'", sgtype);
                }
                if (!queryParam["sgproperty"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and a.accidenteventproperty='{0}'", queryParam["sgproperty"].ToString());
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    string sgtypename = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and accidenteventname like '%{0}%'", sgtypename);
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
                if (!queryParam["type"].IsEmpty())
                {
                    if (queryParam["type"].ToString() == "0") //����
                    {
                        pagination.conditionJson += string.Format(" and d.applystate=3 and (d.rectificationdutypersonid like '%{0}%' or e.intransferuseraccount like '%{1}%') and (e.outtransferuseraccount is null or e.outtransferuseraccount not like '%{1}%')", user.UserId, user.Account + ",");
                    }
                    else if (queryParam["type"].ToString() == "1") //����
                    {
                        string[] roles = user.RoleName.Split(',');
                        string roleWhere = "";
                        foreach (var r in roles)
                        {
                            roleWhere += string.Format("or d.flowrolename like '%{0}%'", r);
                        }
                        roleWhere = roleWhere.Substring(2);
                        pagination.conditionJson += string.Format(" and d.applystate=4 and d.flowdept like '%{0}%' and ({1})", user.DeptId, roleWhere);
                    }
                    else if (queryParam["type"].ToString() == "2") //ǩ��
                    {
                        pagination.conditionJson += string.Format(" and d.applystate=6 and d.signpersonid like '%{0}%'", user.UserId);
                    }
                    else if (queryParam["type"].ToString() == "3") //����+����
                    {
                        pagination.conditionJson += string.Format(" and ((d.applystate=3 and (d.rectificationdutypersonid like '%{0}%' or e.intransferuseraccount like '%{1}%') and (e.outtransferuseraccount is null or e.outtransferuseraccount not like '%{1}%')) or (d.applystate=6 and d.signpersonid like '%{0}%'))", user.UserId, user.Account + ",");
                    }
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PowerplantreformEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PowerplantreformEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, PowerplantreformEntity entity)
        {
            var res = GetEntity(keyValue);
            if (res == null || string.IsNullOrEmpty(keyValue))
            {
                entity.Id = string.IsNullOrEmpty(keyValue) ? "" : keyValue;
                entity.Create();
                this.BaseRepository().Insert(entity);

            }
            else
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
        }
        #endregion
    }
}
