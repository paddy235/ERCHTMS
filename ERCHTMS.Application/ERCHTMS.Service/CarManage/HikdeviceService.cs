using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� �����Ž��豸����
    /// </summary>
    public class HikdeviceService : RepositoryFactory<HikdeviceEntity>, HikdeviceIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HikdeviceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //�豸����
            if (!queryParam["DeviceName"].IsEmpty())
            {
                string DeviceName = queryParam["DeviceName"].ToString();

                pagination.conditionJson += string.Format(" and DeviceName like '%{0}%'", DeviceName);

            }
            //��������
            if (!queryParam["OutType"].IsEmpty())
            {
                int OutType = Convert.ToInt32(queryParam["OutType"]);

                pagination.conditionJson += string.Format(" and OutType ={0}", OutType);

            }
            //�豸�����Ÿ�
            if (!queryParam["AreaName"].IsEmpty())
            {
                string AreaName = queryParam["AreaName"].ToString();
                pagination.conditionJson += string.Format(" and AreaName like '%{0}%'", AreaName);
            }
            //�豸IP
            if (!queryParam["DeviceIP"].IsEmpty())
            {
                string DeviceIP = queryParam["DeviceIP"].ToString();
                pagination.conditionJson += string.Format(" and DeviceIP like '%{0}%'", DeviceIP);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikdeviceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikdeviceEntity GetDeviceEntity(string HikID)
        {
            return this.BaseRepository().FindEntity(it => it.HikID == HikID);
        }


        /// <summary>
        ///  �����豸�������� ����ȡ�����������е��豸
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public List<HikdeviceEntity> GetDeviceByArea(string areaName)
        {
            return this.BaseRepository().IQueryable(x => x.AreaName.Equals(areaName) && x.DeviceType.Equals("�Ž��豸")).ToList();
        }

        /// <summary>
        /// ��ȡ��ǰ�糧���е��Ž��豸����
        /// ���ý��ڱ��������  ϵͳ����->�����Ž��豸����
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataItemEntity> GetDeviceArea()
        {
            var db = new RepositoryFactory().BaseRepository();
            var query = from q1 in db.IQueryable<DataItemEntity>()
                        join q2 in db.IQueryable<DataItemEntity>() on q1.ParentId equals q2.ItemId
                        where q2.ItemName == "�����Ž��豸" orderby q1.SortCode ascending
                        select new { q1.ItemId, q1.ItemName };
            var result = query.ToList();
          var data=  result.Select(x => {
                return new DataItemEntity()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName
                };
            });
            return data;
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
        public void SaveForm(string keyValue, HikdeviceEntity entity)
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
