using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data;
using BSFramework.Data.Repository;
using ERCHTMS.Code;
using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.IService.SaftProductTargetManage;


using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产目标
    /// </summary>
    public class SafeProductService : RepositoryFactory<SafeProductEntity>, SafeProductIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafeProductEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafeProductEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSafeByCondition(string dateYear, string belongId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"select id,SaftTargetName,DateYear,RealCount,ShouldCount,AgreementRate,SendStatus from bis_safeproduct where DateYear=@dateYear and BelongDeptId=@belongId");
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@dateYear", dateYear));
            parameter.Add(DbParameters.CreateDbParameter("@belongId", belongId));
            return this.BaseRepository().FindTable(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 计算目标值
        /// </summary>
        /// <param name="belongtype"></param>
        /// <returns></returns>
        public string calculateGoal(string belongtype, string belongdeptid,string year)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = string.Format("select encode from base_department where departmentid='{0}' union select encode from base_organize where organizeid='{0}'", belongdeptid);
            //本单位code
            var deptcode = this.BaseRepository().FindObject(sql).ToString();
            //承包商根节点code
            string sqlcbs = string.Format("select encode from base_department where  Description = '外包工程承包商' and encode like '{0}%'", user.OrganizeCode);
            var cbsnodecode = this.BaseRepository().FindObject(sqlcbs).ToString();
            string sqlstr = "";
            string sqlstr1 = "";
            double result = 0, num = 0, num1 = 0;
            switch (belongtype)
            {
                //1.本单位及下属单位本年度在场特种作业人员持证人员数量/本单位及下属单位本年度在场特种作业人员数量（单位内部，不包含承包商数据）
                case "1":
                    //在场的持证特种作业人员
                    sqlstr = string.Format(@"select count(1) from base_user where  departmentcode like '{0}%' and  departmentcode not like '{1}%' and IsSpecial='是' and IsPresence='1' and userid in(select userid from Bis_Certificate where certname='特种作业操作证')", deptcode, cbsnodecode);
                    num = this.BaseRepository().FindObject(sqlstr).ToDouble();
                    //在场的特种作业人员
                    sqlstr1 = string.Format(@"select count(1) from base_user where  departmentcode like '{0}%' and  departmentcode not like '{1}%' and IsSpecial='是' and IsPresence='1'", deptcode, cbsnodecode);
                    num1 = this.BaseRepository().FindObject(sqlstr1).ToDouble();
                    result = (num1 > 0 && num > 0) ? Math.Round(num / num1 * 100, 2) : 0;
                    break;
                //2.责任单位为本单位及下属单位本年度发生的重大及以上的设备事故数量
                case "2":
                    sqlstr = string.Format(@"select count(1) from aem_bulletin_deal where sgtype_deal='4' and sglevel_deal>='4' and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}' and  departmentid_deal in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt();
                    break;
                //3.责任单位为本单位及下属单位本年度发生的“电力生产人身伤亡事故”和“电力建设人身伤亡事故”数量
                case "3":
                    sqlstr = string.Format(@"select count(1) from aem_bulletin_deal where (sgtype_deal='1' or sgtype_deal='2')  and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}' and  departmentid_deal in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt();
                    break;
                //5.责任单位为本单位及下属单位本年度发生的一般事故是设备事故的数量
                case "5":
                    sqlstr = string.Format(@"select count(1) from aem_bulletin_deal where sgtype_deal='4' and sglevel_deal='1' and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}' and  departmentid_deal in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    sqlstr1 = string.Format(@"select count(1) from BIS_POWERPLANTINSIDE where AccidentEventType='02' and AccidentEventProperty='06' and IsOver=1 and to_char(HappenTime,'yyyy')='{0}' and  belongdeptid in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt() + this.BaseRepository().FindObject(sqlstr1).ToInt();
                    break;
                //6.责任单位为本单位及下属单位本年度发生的事故事件类型为环保的数量
                case "6":
                    sqlstr = string.Format(@"select count(1) from BIS_POWERPLANTINSIDE where AccidentEventType='05'  and IsOver=1 and to_char(HappenTime,'yyyy')='{0}' and  BELONGDEPTID in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt();
                    break;
                //7.责任单位为本单位及下属单位本年度发生的“人身轻伤及未遂事件”数量
                case "7":
                    sqlstr = string.Format(@"select count(1) from aem_wssjbg_deal where wssjtype_deal='1' and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}'  and  zrdepartid in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    sqlstr1 = string.Format(@"select count(1) from BIS_POWERPLANTINSIDE where AccidentEventType='01' and AccidentEventProperty in ('02','03','04','06')  and IsOver=1 and to_char(HappenTime,'yyyy')='{0}' and  BELONGDEPTID in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt() + this.BaseRepository().FindObject(sqlstr1).ToInt();
                    break;
                //9.责任单位为本单位及下属单位本年度发生的“电力生产人身伤亡事故”和“电力建设人身伤亡事故”中事故类别中的道路交通数量
                case "9":
                    sqlstr = string.Format(@"select count(1) from aem_bulletin_deal where (sgtype_deal='1' or sgtype_deal='2') and  rsshsgtype_deal='9' and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}' and  departmentid_deal in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    sqlstr1 = string.Format(@"select count(1) from BIS_POWERPLANTINSIDE where AccidentEventType='04' and AccidentEventProperty='06'  and IsOver=1 and to_char(HappenTime,'yyyy')='{0}' and  BELONGDEPTID in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt() + this.BaseRepository().FindObject(sqlstr1).ToInt();
                    break;
                //10.责任单位为本单位及下属单位本年度发生的“电力生产人身伤亡事故”和“电力建设人身伤亡事故”中事故类别中的火灾数量
                case "10":
                    sqlstr = string.Format(@"select count(1) from aem_bulletin_deal where (sgtype_deal='1' or sgtype_deal='2') and  rsshsgtype_deal='8' and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}' and  departmentid_deal in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt();
                    break;
                //11.责任单位为本单位及下属单位本年度发生的“一类障碍”数量
                case "11":
                    sqlstr = string.Format(@"select count(1) from aem_wssjbg_deal where wssjtype_deal='2' and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}'  and  zrdepartid in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    sqlstr1 = string.Format(@"select count(1) from BIS_POWERPLANTINSIDE where AccidentEventProperty='05' and IsOver=1 and to_char(HappenTime,'yyyy')='{0}' and  BELONGDEPTID in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt() + this.BaseRepository().FindObject(sqlstr1).ToInt();
                    break;
                //12.责任单位为本单位及下属单位本年度发生的“二类障碍”数量
                case "12":
                    sqlstr = string.Format(@"select count(1) from aem_wssjbg_deal where wssjtype_deal='3' and IsSubmit_Deal=1 and  to_char(happentime_deal,'yyyy')='{0}'  and  zrdepartid in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    sqlstr1 = string.Format(@"select count(1) from BIS_POWERPLANTINSIDE where AccidentEventProperty='01' and IsOver=1 and to_char(HappenTime,'yyyy')='{0}' and  BELONGDEPTID in(select departmentid  from base_department where encode like '{1}%' and encode not like '{2}%' union select organizeid from base_organize where organizeid='{3}')", year, deptcode, cbsnodecode, belongdeptid);
                    result = this.BaseRepository().FindObject(sqlstr).ToInt() + this.BaseRepository().FindObject(sqlstr1).ToInt();
                    break;
                //13.截止到目前时间，管控责任部门为该部门及下属单位,通过检查验收的特种设备数量/截止到目前时间
                case "13":
                    //所有在场设备
                    sqlstr = string.Format(@"select count(1) from bis_specialequipment  where  controldeptcode like '{0}%' and  controldeptcode not like '{1}%'  and State='2'", deptcode, cbsnodecode);
                    num = this.BaseRepository().FindObject(sqlstr).ToDouble();
                    //所有设备
                    sqlstr1 = string.Format(@"select count(1) from bis_specialequipment  where  controldeptcode like '{0}%' and  controldeptcode not like '{1}%'", deptcode, cbsnodecode);
                    num1 = this.BaseRepository().FindObject(sqlstr1).ToDouble();
                    result = (num1 > 0 && num > 0) ? Math.Round(num / num1 * 100, 2) : 0;
                    break;
                //14.本年度隐患，整改单位为本单位及下属单位已整改隐患数量/本年度整改单位为本单位及下属单位的隐患数量
                case "14":
                    //已整改数量（时间根据排查时间）
                    sqlstr = string.Format(@"select count(1) from v_basehiddeninfo where workstream !='隐患整改' and  changedutydepartcode like '{0}%' and  changedutydepartcode not like '{1}%' and  to_char(checkdate,'yyyy')='{2}'", deptcode, cbsnodecode, year);
                    num = this.BaseRepository().FindObject(sqlstr).ToDouble();
                    //隐患数量
                    sqlstr1 = string.Format(@"select count(1) from v_basehiddeninfo where changedutydepartcode like '{0}%' and  changedutydepartcode not like '{1}%' and  to_char(checkdate,'yyyy')='{2}'", deptcode, cbsnodecode, year);
                    num1 = this.BaseRepository().FindObject(sqlstr1).ToDouble();
                    result = (num1 > 0 && num > 0) ? Math.Round(num / num1 * 100, 2) : 0;
                    break;
                default:
                    break;
            }
            return result.ToString();
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<SafeProductEntity>(keyValue);
                db.Delete<SafeProductProjectEntity>(t => t.ProductId.Equals(keyValue));
                db.Delete<SafeProductDutyBookEntity>(t => t.ProductId.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, SafeProductEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                return this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                return this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
