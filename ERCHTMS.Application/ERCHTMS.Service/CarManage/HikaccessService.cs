using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� ���������Ž����豸����
    /// </summary>
    public class HikaccessService : RepositoryFactory<HikaccessEntity>, HikaccessIService
    {
        #region ��ȡ����
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

            if (!queryParam["AreaId"].IsEmpty())
            {
                string AreaId = queryParam["AreaId"].ToString();
                pagination.conditionJson += string.Format(" and AreaId = '{0}'", AreaId);
            }

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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HikaccessEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikaccessEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ����hikID��ȡ�豸��Ϣ
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikaccessEntity HikGetEntity(string HikId)
        {
            string sql = string.Format("select * from bis_hikaccess  where hikid = '{0}'", HikId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }

        #endregion

        #region �ύ����
        /// <summary>
        /// �Ž�״̬����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="type"></param>
        /// <param name="pitem"></param>
        /// <param name="url"></param>
        public void ChangeControl(string keyValue, int type, string pitem, string url)
        {
            //�����2�ſ��򲻸���״̬
            if (type != 2)
            {
                string sql = string.Format("update bis_hikaccess set status={0} where hikid in ({1})", type, keyValue);
                this.BaseRepository().ExecuteBySql(sql);
            }

            keyValue = keyValue.Replace("'", "");
            string[] ids = keyValue.Split('\'');
            List<string> doorIndexCodes = new List<string>();
            foreach (var str in ids)
            {
                doorIndexCodes.Add(str);
            }
            string apiurl = "/artemis/api/acs/v1/door/doControl";
            var model = new
            {
                doorIndexCodes,
                controlType = type
            };
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }
            SocketHelper.LoadCameraList(model, url, apiurl, key, sign);
        }

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
        public void SaveForm(string keyValue, HikaccessEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.Status = 1;
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
