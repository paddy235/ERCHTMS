using System;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data.Common;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：危险因素清单
    /// </summary>
    public class HazardfactorsService : RepositoryFactory<HazardfactorsEntity>, HazardfactorsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazardfactorsEntity> GetList(string queryJson)
        {
            if (queryJson != "")
            {
                return this.BaseRepository().IQueryable(it => it.AreaId == queryJson).ToList();
            }
            else
            {
                return this.BaseRepository().IQueryable().ToList();
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="AreaId">区域id</param>
        /// <param name="where">其他查询条件</param>
        /// <returns></returns>
        public DataTable GetList(string AreaId, string where)
        {
            string sql = "select HID,AREAID,AREAVALUE,RISKID,RISKVALUE,CONTACTNUMBER from BIS_HAZARDFACTORS where 1=1 ";
            if (where != "")
            {
                sql += where;
            }
            if (AreaId != "")
            {
                sql += string.Format(" and AREAID='{0}'", AreaId);
            }

            sql += " order by CREATEDATE asc";
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazardfactorsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 验证区域名称是否重复//区分不同公司用户
        /// </summary>
        /// <param name="AreaValue">区域名称</param>
        /// <returns></returns>
        public bool ExistDeptJugement(string AreaValue, string orgCode, string RiskName)
        {
            DbParameter[] dbParameter  = {
                DbParameters.CreateDbParameter(":AREAVALUE", AreaValue),
                 DbParameters.CreateDbParameter(":CREATEUSERORGCODE", orgCode),
                  DbParameters.CreateDbParameter(":RISKVALUE", RiskName)
            };

            IEnumerable<HazardfactorsEntity> count = this.BaseRepository().FindList("select HID  from BIS_HAZARDFACTORS t where t.AREAVALUE=:AREAVALUE and t.CREATEUSERORGCODE=:CREATEUSERORGCODE and t.RISKVALUE=:RISKVALUE", dbParameter);
            if (count.Count() > 0)
                return false;
            else return true;
        }

        /// <summary>
        /// 验证区域id和危险源是否重复//区分不同公司用户
        /// </summary>
        /// <param name="Areaid">区域名称</param>
        /// <returns></returns>
        public bool ExistAreaidJugement(string Areaid, string orgCode, string RiskName)
        {
            IEnumerable<HazardfactorsEntity> count = this.BaseRepository().FindList(string.Format("select HID  from BIS_HAZARDFACTORS t where t.AREAID='{0}' and t.CREATEUSERORGCODE='{1}' and t.RISKVALUE='{2}'", Areaid, orgCode, RiskName));
            if (count.Count() > 0)
                return false;
            else return true;
        }

        /// <summary>
        /// 验证区域id和危险源是否重复//区分不同公司用户
        /// </summary>
        /// <param name="Areaid">区域名称</param>
        /// <returns></returns>
        public bool ExistAreaidJugement(string Areaid, string orgCode, string RiskName, string Hid)
        {
            IEnumerable<HazardfactorsEntity> count = this.BaseRepository().FindList(string.Format("select HID  from BIS_HAZARDFACTORS t where t.AREAID='{0}' and t.CREATEUSERORGCODE='{1}' and t.RISKVALUE='{2}' and t.HID!='{3}'", Areaid, orgCode, RiskName, Hid));
            if (count.Count() > 0)
                return false;
            else return true;
        }

        /// <summary>
        /// 验证是否有该危险源，如果有返回Code 如果没有返回空字符串
        /// </summary>
        /// <param name="code">字典的Code</param>
        /// <param name="RiskName">危险源名称</param>
        /// <returns></returns>
        public string IsRisk(string code, string RiskName)
        {
            object itemvalue = this.BaseRepository().FindObject(string.Format(@"select det.itemvalue from base_dataitemdetail det
                                        left join base_dataitem di on di.itemid=det.itemid
                                        where  di.Itemcode='{0}'
                                        and det.Itemname='{1}' and length(det.itemvalue)>2
                                        order by det.sortcode", code, RiskName));
            if (itemvalue != null && itemvalue != "")
            {
                return itemvalue.ToString();
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson, string where)
        {
            string Sql = @"select ha.hid,areavalue,riskvalue,LISTAGG(hauser.username,',') WITHIN GROUP (ORDER BY hauser.username) AS us from BIS_HAZARDFACTORs  ha
            left join BIS_HAZARDFACTORUSER hauser on ha.hid=hauser.hid
            where 1=1
            ";
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                switch (condition)
                {
                    case "Area":          //姓名
                        Sql += string.Format(" and ha.AREAVALUE  like '%{0}%'", keyord);
                        break;
                    case "Riskname": //职业病危害因素名称
                        Sql += string.Format(" and ha.RISKVALUE  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }

            }

            if (where != "")
            {
                Sql += where;
            }

            Sql += " group by ha.hid,ha.areavalue,ha.riskvalue ,ha.CREATEDATE ORDER BY ha.CREATEDATE DESC";
            return this.BaseRepository().FindTable(Sql);
        }


        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "Area":          //姓名
                        pagination.conditionJson += string.Format(" and AREAVALUE  like '%{0}%'", keyord);
                        break;
                    case "Riskname": //职业病危害因素名称
                        pagination.conditionJson += string.Format(" and RISKVALUE  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.Delete<HazardfactorsEntity>(keyValue);
                //先删除该危害因素下的所有人员
                string sql = string.Format("delete from BIS_HAZARDFACTORUSER where hid='{0}'", keyValue);
                res.ExecuteBySql(sql);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HazardfactorsEntity entity, string UserName, string UserId)
        {

            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string id = "";
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<HazardfactorsEntity>(entity);
                    id = entity.Hid;
                }
                else
                {
                    entity.Create();
                    res.Insert<HazardfactorsEntity>(entity);
                    id = entity.Hid;
                }
                //先删除该危害因素下的所有人员
                string sql = string.Format("delete from BIS_HAZARDFACTORUSER where hid='{0}'", id);
                res.ExecuteBySql(sql);

                //新增关联用户表
                List<HazardfactoruserEntity> hulist = new List<HazardfactoruserEntity>();
                string[] names = UserName.Split(',');
                string[] ids = UserId.Split(',');

                for (int i = 0; i < ids.Length; i++)
                {
                    HazardfactoruserEntity hu = new HazardfactoruserEntity();
                    hu.Hid = id;
                    hu.UserId = ids[i];
                    hu.UserName = names[i];
                    hu.Create();
                    hulist.Add(hu);
                }

                res.Insert<HazardfactoruserEntity>(hulist);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }

        }
        #endregion
        #region 手机端
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazardfactorsEntity> PhoneGetList(string queryJson, string orgid)
        {
            if (queryJson != "")
            {
                return this.BaseRepository().IQueryable(it => it.AreaId == queryJson && it.CreateUserOrgCode == orgid).ToList();
            }
            else
            {
                return this.BaseRepository().IQueryable(it => it.CreateUserOrgCode == orgid).ToList();
            }
        }
        #endregion
    }
}
