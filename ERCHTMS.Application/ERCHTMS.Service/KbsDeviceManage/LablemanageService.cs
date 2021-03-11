using System;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// �� ������ǩ����
    /// </summary>
    public class LablemanageService : RepositoryFactory<LablemanageEntity>, LablemanageIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public List<LablemanageEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var query = this.BaseRepository().IQueryable();
            query = query.Where(x => x.IsBind == 1);

            if (!string.IsNullOrEmpty(queryJson))
            {

                var queryParam = queryJson.ToJObject();

                if (!queryParam["selectStatus"].IsEmpty())
                {
                    string selectStatus = queryParam["selectStatus"].ToString();
                    query = query.Where(it => it.State == selectStatus);
                }

                if (!queryParam["selectType"].IsEmpty())
                {
                    string selectType = queryParam["selectType"].ToString();
                    query = query.Where(it => it.LableTypeId == selectType);
                }

                if (!queryParam["deptCode"].IsEmpty())
                {
                    string deptCode = queryParam["deptCode"].ToString();
                    query = query.Where(it => it.DeptCode.Contains(deptCode));
                }

                if (!queryParam["Search"].IsEmpty())
                {
                    string Search = queryParam["Search"].ToString();
                    query = query.Where(it => it.LableId.Contains(Search) || it.DeptName.Contains(Search) || it.LableTypeName.Contains(Search) || it.Name.Contains(Search));
                }
            }

            if (pagination != null)
            {
                pagination.records = query.Count();
                return query.OrderByDescending(it => it.CreateDate).Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// ��ȡ��ǩ����
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return this.BaseRepository().IQueryable(it => it.IsBind == 1).ToList().Count;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LablemanageEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LablemanageEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ��ǩͳ��ͼ
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            List<object[]> list = new List<object[]>();
            string sql = string.Format("select labletypename,count(labletypename)as cou from BIS_LABLEMANAGE where isbind = 1 group by labletypename");
            DataTable dt = this.BaseRepository().FindTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object[] arr = { dt.Rows[i][0].ToString(), Convert.ToInt32(dt.Rows[i][1]) };
                list.Add(arr);

            }
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// ��ȡ��ǩͳ����Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetLableStatistics()
        {
            List<object[]> list = new List<object[]>();
            string sql = string.Format("select labletypename,labletypeid,count(labletypename)as cou from BIS_LABLEMANAGE where isbind = 1  group by labletypename,labletypeid");
            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }

        /// <summary>
        /// ��ȡ�û��󶨱�ǩ
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LablemanageEntity GetUserLable(string userid)
        {
            return BaseRepository().FindEntity(it => it.UserId == userid && it.IsBind == 1);
        }

        /// <summary>
        /// ��ȡ�����Ƿ�󶨱�ǩ
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LablemanageEntity GetCarLable(string CarNo)
        {
            return BaseRepository().FindEntity(it => it.Type == 1 && it.Name == CarNo && it.IsBind == 1);
        }

        /// <summary>
        /// ��ȡ��ǩ�Ƿ��ظ���
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public bool GetIsBind(string LableId)
        {
            if (BaseRepository().FindEntity(it => it.LableId == LableId && it.IsBind == 1) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// ����lableId��ȡ�Ƿ��а���Ϣ
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public LablemanageEntity GetLable(string LableId)
        {
            return BaseRepository().FindEntity(it => it.LableId == LableId && it.IsBind == 1);
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
        /// ����ǩ
        /// </summary>
        /// <param name="keyValue"></param>
        public void Untie(string keyValue)
        {
            var info = BaseRepository().FindEntity(keyValue);
            info.Modify(keyValue);
            info.IsBind = 0;
            BaseRepository().Update(info);
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LablemanageEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                //entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
