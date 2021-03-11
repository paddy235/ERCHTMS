using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using BSFramework.Data;
using BSFramework.Util;
using ERCHTMS.Code;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class SafetyCreditEvaluateService : RepositoryFactory<SafetyCreditEvaluateEntity>, SafetyCreditEvaluateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取项目基本信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public DataTable GetEngineerDataById( string orgid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select e.engineername engineername,
                                         e.id engineerid,e.outprojectid unitid,d.fullname unitname,d.encode,
                                            engineerletdept deptname,engineerletdeptid deptid,engineercode projectcode,
                                            r.districtname areaname,f.itemname projecttype,g.itemname projectlevel,
                                            engineercontent projectcontent
                                    from epg_outsouringengineer e 
                                    left join epg_startappprocessstatus p on p.outengineerid=e.id
                                    left join base_department d on e.outprojectid=d.departmentid
                                    left join bis_district r on e.engineerarea=r.districtid
                                    left join base_dataitemdetail f on e.engineertype=f.itemvalue 
                                    left join base_dataitemdetail g on e.engineertype=g.itemvalue
                                    where f.itemid=(select itemid from base_dataitem where itemcode='ProjectType')
                                    and g.itemid=(select itemid from base_dataitem where itemcode='ProjectLevel') and e.id = '"+ orgid + "'");
            return this.BaseRepository().FindTable(strSql.ToString());
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyCreditEvaluateEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyCreditEvaluateEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public string RemoveForm(string keyValue)
        {
            string num = "0";
            num = DbFactory.Base().FindObject("select count(1) from epg_safetycreditscore where SAFETYCREDITEVALUATEID ='" + keyValue + "'").ToString();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (num == "0" || user.RoleName.Contains("公司管理员")) // 公司管理员可以删除
            {
                this.BaseRepository().Delete(keyValue);
            }

            return num;
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyCreditEvaluateEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.ID = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    SafetyCreditEvaluateEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        res.Insert<SafetyCreditEvaluateEntity>(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        res.Update<SafetyCreditEvaluateEntity>(entity);
                    }
                }
                else
                {
                    entity.Create();
                    res.Insert<SafetyCreditEvaluateEntity>(entity);
                }

            }
            catch (System.Exception)
            {

                res.Rollback();
            }
            res.Commit();
        }


        /// <summary>
        /// 结束评价
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void FinishForm(string keyValue)
        {
            int number = DbFactory.Base().ExecuteBySql(string.Format("UPDATE EPG_SAFETYCREDITEVALUATE SET EVALUATESTATE = 1  WHERE  ID ='{0}'", keyValue));
        }
        #endregion
    }
}
