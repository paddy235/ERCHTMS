using System;
using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.IService.EnvironmentalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.EnvironmentalManage
{
    /// <summary>
    /// �� ����ˮ�ʷ�����¼
    /// </summary>
    public class WaterrecordService : RepositoryFactory<WaterrecordEntity>, WaterrecordIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<WaterrecordEntity> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            StringBuilder strSql = new StringBuilder();
            //��ѯ����
            if (!queryParam["sampletype"].IsEmpty())
            {
                string id = queryParam["sampletype"].ToString();
                strSql.Append(String.Format("select projectcode,kpitarget from bis_waterrecord where sampletype ='{0}'", id));
                return this.BaseRepository().FindList(strSql.ToString());
            }   

            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WaterrecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WaterrecordEntity entity)
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
