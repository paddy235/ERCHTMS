using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using ERCHTMS.Service.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：危险因素清单
    /// </summary>
    public class HazardfactorsBLL
    {
        private HazardfactorsIService service = new HazardfactorsService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazardfactorsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="AreaId">区域id</param>
        /// <param name="where">其他查询条件</param>
        /// <returns></returns>
        public DataTable GetList(string AreaId, string where)
        {
            return service.GetList(AreaId, where);
        }


        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson, string where)
        {
            return service.GetTable(queryJson, where);
        }

        /// <summary>
        /// 验证区域名称是否重复
        /// </summary>
        /// <param name="AreaValue">区域名称</param>
        /// <returns></returns>
        public bool ExistDeptJugement(string AreaValue, string orgCode, string RiskName)
        {
            return service.ExistDeptJugement(AreaValue, orgCode, RiskName);
        }

        /// <summary>
        /// 验证区域id和危险源是否重复//区分不同公司用户
        /// </summary>
        /// <param name="Areaid">区域名称</param>
        /// <returns></returns>
        public bool ExistAreaidJugement(string Areaid, string RiskName, string Hid)
        {
            if (Hid == null || Hid == "")
            {
                Hid = "1";
            }
            return service.ExistAreaidJugement(Areaid, OperatorProvider.Provider.Current().OrganizeCode, RiskName, Hid);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazardfactorsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            return service.GetPageListByProc(pagination, queryJson);
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
        public void SaveForm(string keyValue, HazardfactorsEntity entity, string UserName, string UserId)
        {
            try
            {
                service.SaveForm(keyValue, entity,UserName,UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 从风险清单加入数据
        /// </summary>
        /// <param name="Areaid">区域id</param>
        /// <param name="AreaValue">区域名称</param>
        /// <param name="RiskValue">危险源名称</param>
        public void Add(string Areaid, string AreaValue, string RiskValue)
        {
            try
            {
                if (service.ExistAreaidJugement(Areaid, OperatorProvider.Provider.Current().OrganizeCode, RiskValue)) //没有重复就添加数据
                {

                    string RiskId = service.IsRisk("Risk", RiskValue);
                    if (RiskId != "")
                    {
                        HazardfactorsEntity entity = new HazardfactorsEntity();
                        entity.AreaId = Areaid;
                        entity.AreaValue = AreaValue;
                        entity.Riskid = RiskId;
                        entity.RiskValue = RiskValue;
                        //service.SaveForm("", entity);
                    }
                }
            }
            catch (Exception)
            {

                throw;
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
            return service.PhoneGetList(queryJson, orgid);
        }
        #endregion
    }
}
