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
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Service.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ������ۼƻ�
    /// </summary>
    public class EvaluatePlanService : RepositoryFactory<EvaluatePlanEntity>, EvaluatePlanIService
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
                    pagination.conditionJson += string.Format(" and Year='{0}'", queryParam["Year"].ToString());
                }
                //��������
                if (!queryParam["WorkTitle"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and WorkTitle like'{0}%'", queryParam["WorkTitle"].ToString());
                }
            }
            var userid = OperatorProvider.Provider.Current().UserId;
            pagination.conditionJson += string.Format(" and (CreateUserId='{0}' or IsSubmit>=1)", userid);
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<EvaluatePlanEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EvaluatePlanEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, EvaluatePlanEntity entity)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.Id = System.Guid.NewGuid().ToString();
            }
            else
            {
                entity.Id = keyValue;
            }
            //������������
            if (entity.CheckState == 0 || entity.CheckState == null)
            {
                entity.DoneDeptNum = 0;
                if (entity.IsSubmit == 1)
                {
                    entity.CheckState = 0;//0�������ύ 1���۱��汣�� 2���۱����ύ 3��˱��� 4����ύ
                    entity.Dept = AddEvaluate(entity, 1);
                }
                else
                {
                    entity.Dept = AddEvaluate(entity, 0);
                }
                try
                {
                    string str = entity.Dept.TrimEnd(',');
                    string[] strArr = str.Split(',');
                    entity.DeptNum = strArr.Length;
                }
                catch { }
            }
            //��˽��� �޸�����״̬
            if (entity.CheckState == 4)
            {
                UpdateRectifyState(entity);
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Year = DateTime.Now.Year;//���
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            
        }
        /// <summary>
        /// ��ӺϹ������۱�
        /// </summary>
        /// <param name="entity">�Ϲ������ۼƻ�</param>
        /// <param name="type">0 ����ѯ����  1��ѯ�������ӱ�</param>
        public string AddEvaluate(EvaluatePlanEntity entity,int type)
        {

            try
            {
                int deptnum = 0;
                string deptstr = "";
                Operator curUser = OperatorProvider.Provider.Current();
                //��ȡ��Ҫ���۵Ĳ���
                DataTable dt = BaseRepository().FindTable("select DEPARTMENTID,DEPTCODE,FULLNAME,ENCODE from BASE_DEPARTMENT t where nature='����' and description is null and organizeid='"+ curUser.OrganizeId + "'");
                
                //��������
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows) {
                        deptnum++;
                        deptstr += item["fullname"].ToString() + ",";
                        if (type == 1)
                        {
                            IRepository db = new RepositoryFactory().BaseRepository();
                            EvaluateEntity fe = new EvaluateEntity();
                            fe.WorkTitle = entity.WorkTitle;
                            fe.DutyDept = item["fullname"].ToString();
                            fe.DutyDeptCode = item["deptcode"].ToString();
                            fe.EvaluateState = 0; //0δ���� 1������ 2������
                            fe.EvaluatePlanId = entity.Id;
                            fe.RectifyState = 99;
                            fe.Create();
                            db.Insert<EvaluateEntity>(fe);
                        }
                    }
                }
                return deptstr;
            }
            catch (Exception ex) {
                return "";
            }
        }

        /// <summary>
        /// ��˽������޸Ĵ��ڲ�������Ĳ�������״̬
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateRectifyState(EvaluatePlanEntity entity)
        {
            try
            {
                string idstr = "";
                DataTable dt = BaseRepository().FindTable(@"select t1.id from HRS_EVALUATE t1
                            left join  HRS_EVALUATEDETAILS t2 on t2.mainid = t1.id
                            where t2.IsConform = 1 and t1.EvaluatePlanId = '" + entity.Id + "'");
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        idstr += "'" + item["id"].ToString() + "',";
                    }
                    string str = idstr.TrimEnd(',');
                    DataTable dt1 = BaseRepository().FindTable(@"update HRS_EVALUATE set rectifystate=1 where id in(" + str + ")");
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// һ������
        /// </summary>
        public DataTable GetRemindUser(string keyValue)
        {

            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                //��ȡ��Ҫ���۵Ĳ���
                DataTable dt = BaseRepository().FindTable(@"select b.MANAGERID as userid,b.MANAGER as username
                                                            from HRS_EVALUATE a
                                                            left join BASE_DEPARTMENT b on a.dutydeptcode=b.deptcode
                                                            where a.evaluateplanid='"+ keyValue + "' and a.evaluatestate<2");
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
