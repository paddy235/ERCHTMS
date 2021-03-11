using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.IService.EvaluateManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using System;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ�������
    /// </summary>
    public class EvaluateService : RepositoryFactory<EvaluateEntity>, EvaluateIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //���
                if (!queryParam["Year"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and to_char(CreateDate,'yyyy')='{0}'", queryParam["Year"].ToString());
                }
                //��������
                if (!queryParam["WorkTitle"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and WorkTitle like'{0}%'", queryParam["WorkTitle"].ToString());
                }
                //���ģ���ѯ�����ġ������С����������ݣ�
                if (!queryParam["RectifyState"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and RectifyState<>{0}", queryParam["RectifyState"].ToString());
                }
                //����
                if (!queryParam["DutyDeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                }
                //���������ۼƻ�
                if (!queryParam["EvaluatePlanId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EvaluatePlanId='{0}' and EvaluateState>=2", queryParam["EvaluatePlanId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<EvaluateEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EvaluateEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, EvaluateEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                if (entity.EvaluateState == 2)//1 ���� 2 �ύ
                {
                    UpdateDoneDeptNum(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// �Զ���ȡ
        /// </summary>
        /// <param name="keyValue">����ID</param>
        /// <param name="fileSpotList">ʵ�����</param>
        /// <param name="EvaluatePlanId">���ۼƻ�ID</param>
        /// <returns></returns>
        public void SaveForm3(string keyValue, IEnumerable<FileSpotEntity> fileSpotList, string EvaluatePlanId)
        {
            var user = OperatorProvider.Provider.Current();
            String insertSql = "";
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    if (fileSpotList.Count() > 0)
                    {
                        foreach (var item in fileSpotList)
                        {
                            insertSql += string.Format(@"insert into 
                            HRS_EVALUATEDETAILS(ID,
                                                AUTOID,
                                                CreateUserId,
                                                CreateUserDeptCode,
                                                CreateUserOrgCode,
                                                CreateDate,
                                                CreateUserName,
                                                ModifyDate,
                                                ModifyUserId,
                                                ModifyUserName,
                                                Category,
                                                Rank,
                                                DutyDept,
                                                DutyDeptCode,
                                                Clause,
                                                Describe,
                                                Opinion,
                                                FinishTime,
                                                Remake,
                                                MainId,
                                                FileName,
                                                Year,
                                                IsConform,
                                                Measure,
                                                PracticalFinishTime,
                                                EvaluatePlanId,
                                                PutDate,
                                                EvaluatePerson,
                                                EvaluatePersonId) 
values('{0}','','{1}','{2}','{3}','{4}','{5}','','','','','','{6}','{7}','','','','','','{8}','{9}',{10},0,'','','{11}','','{12}',
(select USERID from base_user where REALNAME='{12}' and rownum<2));",
    Guid.NewGuid().ToString(), user.UserId, user.DeptCode, user.OrganizeCode, DateTime.Now, user.UserName, user.DeptName, user.DeptCode, keyValue, item.FileName, DateTime.Now.Year, EvaluatePlanId,item.UserName);
                        }
                        BaseRepository().FindTable(insertSql);
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// �ύ���¼ƻ�������
        /// </summary>
        /// <param name="entity">�Ϲ������ۼƻ�</param>
        public void UpdateDoneDeptNum(EvaluateEntity entity)
        {

            try
            {
                IRepository db = new RepositoryFactory().BaseRepository();
                //��ȡ��Ҫ���۵Ĳ���
                EvaluatePlanEntity ee = db.FindEntity<EvaluatePlanEntity>(entity.EvaluatePlanId);

                //��������
                if (ee != null)
                {
                    ee.DoneDeptNum = ee.DoneDeptNum + 1;
                    db.Update<EvaluatePlanEntity>(ee);
                }
            }
            catch (Exception ex) { }
        }
        #endregion
    }
}
