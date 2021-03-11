using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.IService.EnvironmentalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using BSFramework.Util;
using System.Data;
using BSFramework.Util.Extension;
using BSFramework.Data;

namespace ERCHTMS.Service.EnvironmentalManage
{
    /// <summary>
    /// �� �������м��
    /// </summary>
    public class OwncheckService : RepositoryFactory<OwncheckEntity>, OwncheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OwncheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OwncheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public string GetMaxCode()
        {
            var entity = this.BaseRepository().FindList(" select max(checkcode) as CheckCode from BIS_OWNCHECK").FirstOrDefault();
            if (entity == null || entity.CheckCode == null)
                return DateTime.Now.ToString("yyyy") + "0001";
            if (entity.CheckCode.Substring(0,4)== DateTime.Now.ToString("yyyy"))
                return (int.Parse(entity.CheckCode) + 1).ToString();
            return DateTime.Now.ToString("yyyy") + "0001";

        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["dataname"].IsEmpty())
                    {
                        pagination.conditionJson += " and dataname like '%" + queryParam["dataname"].ToString() + "%'";
                    }
                    if (!queryParam["stime"].IsEmpty())
                    {
                        pagination.conditionJson += " and uploadtime >= to_date('" + queryParam["stime"].ToString() + "','yyyy-MM-dd')";
                    }
                    if (!queryParam["etime"].IsEmpty())
                    {
                        pagination.conditionJson += " and uploadtime <= to_date('" + queryParam["etime"].ToString() + "','yyyy-MM-dd')";
                    }
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw;
            }

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
        public void SaveForm(string keyValue, OwncheckEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                OwncheckEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Id = keyValue;
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
