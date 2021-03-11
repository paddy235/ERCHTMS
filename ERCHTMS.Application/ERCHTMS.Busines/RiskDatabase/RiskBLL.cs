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
using BSFramework.Util.Offices;
namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public class RiskBLL
    {
        private RiskIService service = new RiskService();
       
        #region 获取数据
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
        public IEnumerable<RiskEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            return service.GetList(areaCode, areaId, grade, accType, deptCode, keyWord);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 风险清单导出
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <param name="riskType">风险类型</param>
        /// <param name="authType">授权范围</param>
        /// <param name="IndexState">是否首页跳转</param>
        /// <returns></returns>
        public DataTable GetPageExportList(string queryJson, string riskType, string authType,string IndexState)
        {
            return service.GetPageExportList(queryJson, riskType, authType, IndexState);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskEntity GetEntity(string keyValue)
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
        /// 首页风险工作指标统计
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public decimal[] GetHomeStat(ERCHTMS.Code.Operator user)
        {
            return service.GetHomeStat(user);
        }
         /// <summary>
        /// 首页风险排名
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public string GetRiskRank(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskRank(user);
        }
          /// <summary>
        /// 首页风险预警
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public object GetRiskWarn(ERCHTMS.Code.Operator user,string time="")
        {
            return service.GetRiskWarn(user, time);

        }
           /// <summary>
        /// 计算最近半年的风险预警值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRiskValues(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskValues(user);
        }
        /// <summary>
        /// 根据月份计算当月得分
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">月份，如2017-10-01</param>
        /// <returns></returns>
        public decimal GetRiskValueByTime(ERCHTMS.Code.Operator user, string time)
        {
            return service.GetRiskValueByTime(user, time);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
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
        public int SaveForm(string keyValue, RiskEntity entity)
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
        #endregion

        #region 处理数据
        /// <summary>
        /// 导出风险清单
        /// </summary>
        /// <returns></returns>
        public void ExportExcel(Pagination pagination, string queryJson,string fileName)
        {
            //取出数据源
            DataTable exportTable = service.GetPageList(pagination, queryJson);
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.FileName = fileName+".xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "postname", ExcelColumn = "岗位" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "部门班组" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "区域" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dangersource", ExcelColumn = "风险描述" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "harmtype", ExcelColumn = "危害属性" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "风险类别" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "result", ExcelColumn = "风险后果" });
           // excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "manager", ExcelColumn = "风险控制措施" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "way", ExcelColumn = "评价方式" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itema", ExcelColumn = "事件发生的可能性" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itemb", ExcelColumn = "暴露于危险环境的频繁程度" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itemc", ExcelColumn = "事件后果的严重性" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itemr", ExcelColumn = "风险分值" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "grade", ExcelColumn = "风险等级" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accidentname", ExcelColumn = "可能导致的事故类型" });
            //调用导出方法
            ExcelHelper.ExcelDownload(exportTable, excelconfig);
        }
        #endregion

        #region 手机app端使用
        #region  11.1-12.3 风险清单及辨识
        /// <summary>
        /// 11.1 风险清单列表
        /// </summary>
        /// <param name="type">查询方式，1：我的，2：重大风险，3：全部</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险级别</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetAreaList(int type, string areaId, string grade, ERCHTMS.Code.Operator user, string kind, string deptcode, string keyWord = "")
        {
            return service.GetAreaList(type, areaId, grade, user, kind,deptcode, keyWord);
        }
        /// <summary>
        /// 11.2 根据区域ID获取风险清单
        /// </summary>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        public DataTable GetRiskList(Pagination pag)
        {
            return service.GetRiskList(pag);
        }
        /// <summary>
        /// 11.3 根据风险ID获取风险详细信息
        /// </summary>
        /// <param name="riskId">风险记录ID</param>
        /// <returns></returns>
        public object GetRisk(string riskId)
        {
            return service.GetRisk(riskId);
        }

        /// <summary>11.4 区域</summary>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetAreas(ERCHTMS.Code.Operator user, string planId)
        {

            return service.GetAreas(user, planId);
        }
        /// <summary>12.1	获取辨识计划列表</summary>
        /// <returns></returns>
        public DataTable GetPlanList(ERCHTMS.Code.Operator user, string condition)
        {
            return service.GetPlanList(user, condition);
        }
        /// <summary>12.2	根据计划ID获取辨识区域</summary>
        /// <param name="planId">计划ID</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetAreaByPlanId(string planId, int status, ERCHTMS.Code.Operator user)
        {

            return service.GetAreaByPlanId(planId, status, user);
        }
        /// <summary>12.3	根据计划ID和区域Code获取风险清单</summary>
        /// <param name="planId">计划ID</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetRiskListByPlanId(string planId, string areaCode, int status, ERCHTMS.Code.Operator user)
        {
            return service.GetRiskListByPlanId(planId, areaCode, status, user);
        }
        /// <summary>
        /// 12.4 新增风险辨识信息
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public int SaveRisk(RiskAssessEntity assess, ERCHTMS.Code.Operator user)
        {
            return service.SaveRisk(assess, user);
        } /// <summary>
        /// 12.5 获取管控责任单位列表
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetDeptList(ERCHTMS.Code.Operator user)
        {
            return service.GetDeptList(user);
        }

        /// <summary>
        /// 12.9 获取风险辨识岗位列表
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetPostList(string deptCode, ERCHTMS.Code.Operator user)
        {
            return service.GetPostList(deptCode, user);
        }
        #endregion

        #region 13.1-13.2 岗位风险卡
        /// <summary>
        /// 13.1-13.2获取本人或全部岗位风险卡列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode">查询方式，0：本人，1：全部</param>
        /// <returns></returns>
        public List<object> GetPostCardList(ERCHTMS.Code.Operator user, int mode = 0)
        {
            return service.GetPostCardList(user, mode);
        }
        /// <summary>
        /// 13.3 根据岗位Id获取风险详情
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="postId">岗位Id</param>
        /// <returns></returns>
        public List<object> GetPostRiskList(ERCHTMS.Code.Operator user, string postId,string deptCode)
        {
            return service.GetPostRiskList(user, postId,deptCode);
        }
        #endregion

        #region 12.6-12.8 获取编码相关数据
        /// <summary>
        /// 获取危害属性，危害分类等编码
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public DataTable GetCodeList(string itemCode)
        {
            return service.GetCodeList(itemCode);
        }
        #endregion

        #region 14.2 统计
        /// <summary>
        /// 风险统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object GetStat(ERCHTMS.Code.Operator user, string deptcode="")
        {
            return service.GetStat(user, deptcode);
        }
        #endregion

        #endregion
    }
}
