using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：分项指标表
    /// </summary>
    public class ClassificationService : RepositoryFactory<ClassificationEntity>, IClassificationService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ClassificationEntity> GetList(string AffiliatedOrganizeId)
        {
            return this.BaseRepository().IQueryable().Where(p => p.AffiliatedOrganizeId == AffiliatedOrganizeId).OrderBy(p => p.ClassificationCode).ToList();
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ClassificationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion


        #region 初始化权重及考核项目
        /// <summary>
        /// 初始化权重及考核项目
        /// </summary>
        /// <param name="organizeId"></param>
        public void AddClassificationList(string organizeId)
        {
            IDepartmentService departmentservice = new DepartmentService();

            IClassificationIndexService classindexservice = new ClassificationIndexService();

            try
            {
                DepartmentEntity entity = departmentservice.GetEntity(organizeId);

                //删除已存在的权重比
                string sql = string.Format(@"delete bis_classification where affiliatedorganizeid ='{0}'", organizeId);

                this.BaseRepository().ExecuteBySql(sql);

                //插入权重比例
                sql = string.Format(@"insert into bis_classification  
                                select lower(substr(sys_guid(),1,8)||'-'||substr(sys_guid(),9,4)||'-'||substr(sys_guid(),13,4)||'-'||substr(sys_guid(),17,4)||'-'||substr(sys_guid(),20,12)) as id,
                                classificationindex,weightcoeffcient,'{0}' as affiliatedorganizecode,'{1}' as affiliatedorganizeid ,'{2}' as affiliatedorganizename,'{3}' as affiliatedyear,classificationcode,cisenable
                                from bis_classification where affiliatedorganizeid ='0'", entity.EnCode, entity.OrganizeId, entity.FullName, DateTime.Now.Year.ToString());

                this.BaseRepository().ExecuteBySql(sql);

                //删除已存在的考核项
                sql = string.Format(@"delete bis_classificationindex where affiliatedorganizeid ='{0}'", organizeId);

                this.BaseRepository().ExecuteBySql(sql);


                var list = GetList(organizeId);

                foreach (ClassificationEntity classentity in list)
                {
                    string classificationId = classentity.Id;
                    string classificationCode = classentity.ClassificationCode;
                    string classificationIndex = classentity.ClassificationIndex;

                    sql = string.Format(@"insert into bis_classificationindex 
                                          select lower(substr(sys_guid(),1,8)||'-'||substr(sys_guid(),9,4)||'-'||substr(sys_guid(),13,4)||'-'||substr(sys_guid(),17,4)||'-'||substr(sys_guid(),20,12)) as id,
                                          '{0}' as classificationid,'{1}'as classificationindex,indexname,indexcode,indexscore,indexstandard,indexstandardformat,indexargsvalue,
                                          '{2}' as affiliatedorganizecode,'{3}' as affiliatedorganizeid,'{4}' as affiliatedorganizename,calculatestandard,'{5}' as classificationcode,isenable
                                          from bis_classificationindex where classificationcode ='{5}' and affiliatedorganizeid ='0'", classificationId, classificationIndex, entity.EnCode, entity.OrganizeId, entity.FullName, classificationCode);

                    this.BaseRepository().ExecuteBySql(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region 删除对应机构下的考核内容
        /// <summary>
        /// 删除对应机构下的考核内容
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteClassification(string keyValue) 
        {
            string sql = string.Format("delete bis_classification where affiliatedorganizeid='{0}'", keyValue);

            this.BaseRepository().ExecuteBySql(sql);

            sql = string.Format("delete bis_classificationindex where affiliatedorganizeid='{0}'", keyValue);

            this.BaseRepository().ExecuteBySql(sql);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ClassificationEntity entity)
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