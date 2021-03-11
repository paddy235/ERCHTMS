using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class HistoryStartapplyService : RepositoryFactory<HistoryStartapplyEntity>, HistoryStartapplyIService
    {

        public HistoryStartapplyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取开工申请历史记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetApplyList(string keyValue)
        {
            string sql = string.Format(@"select s.id appid,s.applyno,s.applytype,s.applypeople applyuser,
                                          s.applyreturntime startdate,s.applytime,e.engineername projectname  from  
                                          epg_historystartapplyfor s
                                              left join epg_startapplyfor a on s.applyid = a.id
                                              left join epg_outsouringengineer e on e.id = a.outengineerid
                                            where s.applyid='{0}'", keyValue);
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取历史详情信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetApplyInfo(string keyValue)
        {
            string sql = string.Format(@"select e.ENGINEERLETDEPT deptname,s.applyno,s.applycause,s.applytype,s.applypeople applyuser,s.dutyman,s.safetyman,s.htnum,
                                           b.fullname unitname,
                                           e.engineername projectname,ENGINEERCODE projectcode,e.engineerareaname areaname,f.itemname projecttype,g.itemname projectlevel,ENGINEERCONTENT projectcontent,
                                          s.applyreturntime startdate,s.checkresult,s.checkusers,s.applytime,a.OUTENGINEERID projectid,a.applypeople,a.applyendtime enddate from  
                                          epg_historystartapplyfor s
                                              left join epg_startapplyfor a on s.applyid = a.id
                                              left join epg_outsouringengineer e on e.id = a.outengineerid
                                              left join base_department b on b.departmentid = a.outprojectid
                                    left join base_dataitemdetail f on e.engineertype=f.itemvalue 
                                    left join base_dataitemdetail g on e.engineertype=g.itemvalue
                                    where f.itemid=(select itemid from base_dataitem where itemcode='ProjectType')
                                    and g.itemid=(select itemid from base_dataitem where itemcode='ProjectLevel') and s.id='{0}'", keyValue);
            return BaseRepository().FindTable(sql);
        }
        public DataTable GetHisPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["applyid"].IsEmpty())
            {
                pagination.conditionJson += string.Format("  s.applyid='{0}'", queryParam["applyid"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }


        public void SaveForm(string keyValue, HistoryStartapplyEntity entity) {
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
    }
}
