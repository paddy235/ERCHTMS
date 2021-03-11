using System;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// �� ����������Աץ�ļ�¼��
    /// </summary>
    public class WorkcameracaptureService : RepositoryFactory<WorkcameracaptureEntity>, WorkcameracaptureIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<WorkcameracaptureEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ���ݹ��������ѯץ��ͼƬ
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        public List<WorkcameracaptureEntity> GetCaptureList(string workid, string userid,string cameraid) 
        {
            Expression<Func<WorkcameracaptureEntity, bool>> condition= it=>true;
            if (!string.IsNullOrEmpty(workid))
            {
                condition = condition.And(it => it.WorkId == workid);
            }

            if (!string.IsNullOrEmpty(userid))
            {
                condition = condition.And(it => it.UserId == userid);
            }

            if (!string.IsNullOrEmpty(cameraid))
            {
                condition = condition.And(it => it.CameraId == cameraid);
            }

            return BaseRepository().IQueryable(condition).OrderBy(it => it.CreateDate).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WorkcameracaptureEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WorkcameracaptureEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
