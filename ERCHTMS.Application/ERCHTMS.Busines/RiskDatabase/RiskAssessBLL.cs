using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using System.Text;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：企业风险辨识库
    /// </summary>
    public class RiskAssessBLL
    {
        private RiskAssessIService service = new RiskAssessService();

        #region 获取数据

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<RiskAssessEntity> GetListFor(string queryJson)
        {
            return service.GetListFor(queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="workId">作业步骤ID</param>
        /// <param name="areaCode">区域编码</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        /// <param name="accType">事故类型</param>
        /// <param name="deptCode">部门编码</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskAssessEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            return service.GetList(areaCode, areaId, grade, accType, deptCode, keyWord);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public DataTable GetPageControlListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageControlListJson(pagination, queryJson);
        }
        public IEnumerable<RiskAssessEntity> GetPageListRiskAll(string sql)
        {
            return service.GetPageListRiskAll(sql);
        }
        public IEnumerable<RiskAssessEntity> GetPageListRisk(Pagination pagination, string queryJson)
        {
            return service.GetPageListRisk(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskAssessEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据风险点ID获取部门Code（多个用英文逗号分割）
        /// </summary>
        /// <param name="dangerId">风险点ID</param>
        /// <returns></returns>
        public string GetDeptCode(string dangerId)
        {
            return service.GetDeptCode(dangerId);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        ///  <param name="workId">作业步骤ID</param>
        /// <param name="dangerId">危险点ID</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        public DataTable GetEntity(string workId, string dangerId, string areaId)
        {
            return service.GetEntity(workId, dangerId, areaId);
        }
        /// <summary>
        /// 获取区域选择项下拉选项信息
        /// </summary>
        /// <param name="OrgId">机构ID,不传默认为当前用户所属机构</param>
        /// <returns></returns>
        public string GetAreasOptionsString(string OrgId = "")
        {
            DistrictBLL districtBLL = new DistrictBLL();
            var data = districtBLL.GetList(OrgId);
            StringBuilder sb = new StringBuilder();
            foreach (DistrictEntity dist in data)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dist.DistrictID, dist.DistrictName);
            }
            return sb.ToString();
        }
        public string GetAreasOptionsStringByAreaId(string areaId)
        {
            if (string.IsNullOrEmpty(areaId))
            {
                return GetAreasOptionsString();
            }
            else
            {
                DistrictBLL districtBLL = new DistrictBLL();
                var data = districtBLL.GetNameAndID(areaId);
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in data.Rows)
                {
                    sb.AppendFormat("<option value='{0}'>{1}</option>", dr["DistrictID"].ToString(), dr["DistrictName"].ToString());
                }
                return sb.ToString();
            }

        }
        /// <summary>
        /// 根据部门编码和查询关键字获取岗位风险卡列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns></returns>
        public int GetPostCardCount(string queryCondition)
        {
            return service.GetPostCardCount(queryCondition);
        }
        /// <summary>
        /// 根据区域获取风险点或作业内容选项内容
        /// </summary>
        /// <param name="parentId">区域ID或风险点ID</param>
        /// <returns></returns>
        public string GetRisksOptionsString(string parentId)
        {
            DangerSourceBLL dangerBLL = new DangerSourceBLL();
            List<DangerSourceEntity> listDS = dangerBLL.GetList(parentId, "").ToList();
            StringBuilder sb = new StringBuilder();
            foreach (DangerSourceEntity dist in listDS)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dist.Id, dist.Name);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 根据部门编码获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetRiskCountByDeptCode(string deptCode, string year = "")
        {
            return service.GetRiskCountByDeptCode(deptCode, year);
        }
        /// <summary>
        /// 根据部门编码按区域获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetAreaRiskCountByDeptCode(string deptCode, string year = "")
        {
            return service.GetAreaRiskCountByDeptCode(deptCode, year);
        }
        public string GetYearRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险", string areaCode = "")
        {
            return service.GetYearRiskCountByDeptCode(deptCode, year, riskGrade, areaCode);
        }
        /// <summary>
        /// 根据部门编码获取风险对比分析图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetRatherRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险", string areaCode = "")
        {
            return service.GetRatherRiskCountByDeptCode(deptCode, year, riskGrade, areaCode);
        }
        /// <summary>
        ///风险统计图表格
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetAreaRiskList(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险")
        {
            return service.GetAreaRiskList(deptCode, year, riskGrade);
        }
        /// <summary>
        /// 根据部门编码获取风险对比分析图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetAreaRatherRiskStat(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险")
        {
            return service.GetAreaRatherRiskStat(deptCode, year, riskGrade);
        }
        /// <summary>
        ///按部门获取风险对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetDeptRiskList(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险")
        {
            return service.GetDeptRiskList(deptCode, year, riskGrade);
        }
        /// <summary>
        /// 根据区域获取设备信息
        /// </summary>
        /// <param name="areaId">电厂区域Id</param>
        /// <returns></returns>
        public DataTable GetEuqByAreaId(string areaId)
        {
            return service.GetEuqByAreaId(areaId);
        }


        /// <summary>
        /// 获取首页风险指标
        /// 省级使用
        /// </summary>
        /// <param name="riskGrade">风险等级</param>
        /// <param name="type">1:重大风险 2 较大风险 3 重大风险超过3个</param>
        /// <returns></returns>
        public DataTable GetIndexRiskTarget(string riskGrade, Operator currUser, int type)
        {
            return service.GetIndexRiskTarget(riskGrade, currUser, type);
        }

        public DataTable FindTableBySql(string sql)
        {
            return service.FindTableBySql(sql);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue, string planId = "")
        {
            try
            {
                service.RemoveForm(keyValue, planId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, RiskAssessEntity entity)
        {
            try
            {
                return service.SaveForm(keyValue, entity);

            }
            catch (Exception)
            {
                //throw;
                return 0;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(string ids)
        {
            return service.Delete(ids);
        }
        #endregion


        public List<RiskAssessEntity> RiskCount(bool includeDynamic)
        {
            return service.RiskCount(includeDynamic);
        }

        public List<RiskAssessEntity> RiskCount2(string id)
        {
            return service.RiskCount2(id);
        }

        public List<RiskAssessEntity> RiskCount3(string id)
        {
            return service.RiskCount3(id);
        }

        public List<RiskAssessEntity> RiskCount4(string id)
        {
            return service.RiskCount4(id);
        }
    }
}
