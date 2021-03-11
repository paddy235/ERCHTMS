using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ��
    /// </summary>
    public class RisktrainService : RepositoryFactory<RisktrainEntity>, RisktrainIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<RisktrainEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(queryJson))
            {
               var queryParam = queryJson.ToJObject();
                //��ҵ���
               if (!queryParam["workType"].IsEmpty())
               { 
                  string workType = queryParam["workType"].ToString();
                  pagination.conditionJson += string.Format(" and workType='{0}'", workType);
               }
               //��λ
               if (!queryParam["deptName"].IsEmpty())
               {
                   string deptName = queryParam["deptName"].ToString().Trim();
                   pagination.conditionJson += string.Format(" and createuserdeptname like '%{0}%'", deptName);
               }
               //״̬
              if (!queryParam["status"].IsEmpty())
              {
                 string status = queryParam["status"].ToString();
                 pagination.conditionJson += string.Format(" and status={0}", status);
              }
              if (!queryParam["WorkStartTime"].IsEmpty())
              {
                  pagination.conditionJson += string.Format(" and to_char(workstarttime,'yyyy-MM-dd')>= '{0}'", Convert.ToDateTime(queryParam["WorkStartTime"]).ToString("yyyy-MM-dd"));
              }
              if (!queryParam["WorkEndTime"].IsEmpty())
              {
                  pagination.conditionJson += string.Format(" and to_char(workendtime,'yyyy-MM-dd')<='{0}' ", Convert.ToDateTime(queryParam["WorkEndTime"]).AddDays(1).ToString("yyyy-MM-dd"));
              }
              //��ѯ�ؼ���
              if (!queryParam["keyWord"].IsEmpty())
              {
                  string keyWord = queryParam["keyWord"].ToString().Trim();
                  pagination.conditionJson += string.Format(" and taskname like '%{0}%'", keyWord.Trim());
               }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<RisktrainEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public RisktrainEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, RisktrainEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                RisktrainEntity re = this.BaseRepository().FindEntity(keyValue);
                if (re==null)
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
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="listMesures">��ع������񼰴�ʩ</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RisktrainEntity entity,List<TrainmeasuresEntity> listMesures)
        {
            entity.Id = keyValue;
            for (int i = 0; i < listMesures.Count; i++)
            {
                listMesures[i].Seq = i;
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                var res = new Repository<TrainmeasuresEntity>(DbFactory.Base());
                RisktrainEntity re = this.BaseRepository().FindEntity(keyValue);
                if (re == null)
                {
                    entity.Create();
                    if(this.BaseRepository().Insert(entity)>0)
                    {
                        res.Insert(listMesures);
                    }
                }
                else
                {

                    entity.Modify(keyValue);
                    if(this.BaseRepository().Update(entity)>0)
                    {
                        int count=res.ExecuteBySql(string.Format("delete from BIS_TRAINMEASURES where workid='{0}'",entity.Id));
                       
                        res.Insert(listMesures);
                    }
                }

                if (entity.IsCommit=="1"&&entity.TrainType == "2") {
                    RisktrainlibEntity lib = new RisktrainlibService().GetEntity(entity.TrainlibWorkId);
                    if (lib != null) {
                        lib.UserNum += 1;
                        new RisktrainlibService().SaveForm(lib.ID,lib);
                    }
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
