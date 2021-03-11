using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Text;
using ERCHTMS.Code;
using System.Collections;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// �� ����������ʩ
    /// </summary>
    public class FirefightingService : RepositoryFactory<FirefightingEntity>, FirefightingIService
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
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                
                //��ѯ���� ����
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EquipmentNameNo='{0}'", queryParam["EquipmentNameNo"].ToString());
                }
                //��ѯ���� ����
                if (!queryParam["ExtinguisherTypeNo"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and ExtinguisherTypeNo='{0}'", queryParam["ExtinguisherTypeNo"].ToString());
                }
                //����
                if (!queryParam["DutyDeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                }
                //��� ��ʼʱ��
                if (!queryParam["ExamineStartDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and ExamineDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["ExamineStartDate"].ToString());
                }
                //��� ����ʱ��
                if (!queryParam["ExamineEndDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and ExamineDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["ExamineEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
                //ά�� ��ʼʱ��
                if (!queryParam["DetectionStartDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and DetectionDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["DetectionStartDate"].ToString());
                }
                //ά�� ����ʱ��
                if (!queryParam["DetectionEndDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and DetectionDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["DetectionEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<FirefightingEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FirefightingEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡͳ������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable StatisticsData(string queryJson) {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select max(EquipmentNameNo) as EquipmentNameNo,sum(amount) as EquipmentNum from HRS_FIREFIGHTING");
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //��ѯ���� ����
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    strSql.AppendFormat(" where 1=1");
                    strSql.AppendFormat(" and EquipmentNameNo='{0}'", queryParam["EquipmentNameNo"].ToString());
                    //��ѯ���� ����
                    if (!queryParam["ExtinguisherTypeNo"].IsEmpty())
                    {
                        strSql.AppendFormat(" and ExtinguisherTypeNo='{0}'", queryParam["ExtinguisherTypeNo"].ToString());
                    }
                    //����
                    if (!queryParam["DutyDeptCode"].IsEmpty())
                    {
                        strSql.AppendFormat(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                    }
                    //��� ��ʼʱ��
                    if (!queryParam["ExamineStartDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and ExamineDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["ExamineStartDate"].ToString());
                    }
                    //��� ����ʱ��
                    if (!queryParam["ExamineEndDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and ExamineDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["ExamineEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    //ά�� ��ʼʱ��
                    if (!queryParam["DetectionStartDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and DetectionDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["DetectionStartDate"].ToString());
                    }
                    //ά�� ����ʱ��
                    if (!queryParam["DetectionEndDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and DetectionDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["DetectionEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                }
                else {
                    string whereStr = "where 1=1";
                    //����
                    if (!queryParam["DutyDeptCode"].IsEmpty())
                    {
                        whereStr += string.Format(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                    }

                    //��� ��ʼʱ��
                    if (!queryParam["ExamineStartDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and ExamineDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["ExamineStartDate"].ToString());
                    }
                    //��� ����ʱ��
                    if (!queryParam["ExamineEndDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and ExamineDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["ExamineEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    //ά�� ��ʼʱ��
                    if (!queryParam["DetectionStartDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and DetectionDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["DetectionStartDate"].ToString());
                    }
                    //ά�� ����ʱ��
                    if (!queryParam["DetectionEndDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and DetectionDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["DetectionEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    strSql.AppendFormat(@" {0} group by EquipmentNameNo
                                           UNION ALL
                                          select null as EquipmentNameNo,sum(amount) as EquipmentNum 
                                           from HRS_FIREFIGHTING {0}", whereStr);
                    
                }
            }
            return new RepositoryFactory().BaseRepository().FindTable(strSql.ToString());
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ����id��������ɾ��
        /// </summary>
        /// <param name="Ids"></param>
        public void Remove(string Ids)
        {
            string[] id = Ids.Split(',');
            this.BaseRepository().Delete(it => id.Contains(it.Id));
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
        public void SaveForm(string keyValue, FirefightingEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                FirefightingEntity fe = this.BaseRepository().FindEntity(keyValue);
                if (fe == null)
                {
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

        /// <summary>
        /// ͬһ���ͣ���Ų����ظ�
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool ExistCode(string Type, string Code, string keyValue)
        {
            var expression = LinqExtensions.True<FirefightingEntity>();
            expression = expression.And(t => t.EquipmentNameNo == Type);
            expression = expression.And(t => t.EquipmentCode == Code);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.Id != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }

        /// <summary>
        /// ���������ȡ������µ�������ʩ
        /// </summary>
        /// <param name="areaCodes">�������</param>
        /// <returns></returns>
        public IList GetCountByArea(List<string> areaCodes)
        {
            var query = BaseRepository().IQueryable(x => areaCodes.Contains(x.DistrictCode)).GroupBy(x => new { x.DistrictCode, x.District, x.EquipmentName }).Select(x => new
            {
                DistrictName = x.Key.District,
                DistrictID = x.Key.DistrictCode,
                x.Key.EquipmentName,
                Count = x.Count()
            });
            var data = query.ToList();
            return data;
        }
    }
}
