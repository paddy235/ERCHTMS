using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// �� ������ǩ����
    /// </summary>
    public class LablemanageBLL
    {
        private LablemanageIService service = new LablemanageService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public List<LablemanageEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var list = service.GetPageList(pagination, queryJson);
            //�Ƿ�����
            //if (!queryParam["selectStatus"].IsEmpty())
            //{
            //    string selectStatus = queryParam["selectStatus"].ToString();

            //    list = list.Where(it => it.State == selectStatus).ToList();
            //}
            //if (!queryParam["selectType"].IsEmpty())
            //{
            //    string selectType = queryParam["selectType"].ToString();
            //    list = list.Where(it => it.LableTypeId == selectType).ToList();
            //}
            //if (!queryParam["deptCode"].IsEmpty())
            //{
            //    string deptCode = queryParam["deptCode"].ToString();
            //    list = list.Where(it => it.DeptCode.Contains(deptCode)).ToList();
            //}
            //if (!queryParam["Search"].IsEmpty())
            //{
            //    string Search = queryParam["Search"].ToString();
            //    list = list.Where(it => it.LableId.Contains(Search) || it.DeptName.Contains(Search) || it.LableTypeName.Contains(Search) || it.Name.Contains(Search)).ToList();
            //}

            return list;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public List<LablemanageEntity> GetPageList(string LableID, string Search)
        {
            var list = service.GetPageList(null, "");
            if (!string.IsNullOrEmpty(LableID))
            {
                list = list.Where(it => it.LableId == LableID).ToList();
            }
            if (!string.IsNullOrEmpty(Search))
            {
                list = list.Where(it => it.DeptName == Search || it.Name == Search).ToList();
            }
            return list;
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public List<LablemanageEntity> GetList(string LableTypeId, string DeptCode, string BindTime, int type)
        {
            var list = service.GetPageList(null, "");
            if (type != -1)
            {
                list = list.Where(it => it.Type == type).ToList();
            }
            //�Ƿ�����
            if (!string.IsNullOrEmpty(LableTypeId))
            {
                list = list.Where(it => it.LableTypeId == LableTypeId).ToList();
            }
            if (!string.IsNullOrEmpty(DeptCode))
            {
                list = list.Where(it => it.DeptCode.Contains(DeptCode)).ToList();
            }
            if (!string.IsNullOrEmpty(BindTime))
            {
                list = list.Where(it => it.BindTime.ToString().Contains(BindTime)).ToList();
            }

            return list;
        }

        /// <summary>
        /// ��ȡ���б�ǩ�б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<LablemanageEntity> GetList(string queryJson)
        {
            return service.GetList("");
        }



        /// <summary>
        /// ��ȡ��ǩ����
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return service.GetCount();
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LablemanageEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��ǩͳ��ͼ
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            return service.GetLableChart();
        }

        /// <summary>
        /// ��ȡ��ǩͳ����Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetLableStatistics()
        {
            return service.GetLableStatistics();
        }

        /// <summary>
        /// ��ȡ�û��󶨱�ǩ
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LablemanageEntity GetUserLable(string userid)
        {
            return service.GetUserLable(userid);
        }

        /// <summary>
        /// ��ȡ�����Ƿ�󶨱�ǩ
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LablemanageEntity GetCarLable(string CarNo)
        {
            return service.GetCarLable(CarNo);
        }

        /// <summary>
        /// ��ȡ��ǩ�Ƿ��ظ���
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public bool GetIsBind(string LableId)
        {
            return service.GetIsBind(LableId);
        }

        /// <summary>
        /// ����lableId��ȡ�Ƿ��а���Ϣ
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public LablemanageEntity GetLable(string LableId)
        {
            return service.GetLable(LableId);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ����ǩ
        /// </summary>
        /// <param name="keyValue"></param>
        public void Untie(string keyValue)
        {
            try
            {
                service.Untie(keyValue);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LablemanageEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
