using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using ERCHTMS.Service.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护用品表
    /// </summary>
    public class LaborinfoBLL
    {
        private LaborinfoIService service = new LaborinfoService();

        #region 获取数据

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LaborinfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 判断是否可以获取到权限
        /// </summary>
        /// <returns></returns>
        public bool GetPer()
        {
            var deptid = OperatorProvider.Provider.Current().DeptId;
            var deptname = OperatorProvider.Provider.Current().DeptCode;
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'SelectDept'").ToList();
            foreach (var item in data)
            {
                string value = item.ItemValue;
                string[] values = value.Split('|');
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == deptname)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LaborinfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据ids获取批量发放所需数据
        /// </summary>
        /// <param name="InfoId"></param>
        /// <returns></returns>
        public DataTable Getplff(string InfoId)
        {
            return service.Getplff(InfoId);
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
        public void ImportSaveForm(List<LaborinfoEntity> entity, List<LaborprotectionEntity> prolist, List<LaborequipmentinfoEntity> eqlist)
        {
            try
            {
                service.ImportSaveForm(entity, prolist, eqlist);
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
        public void SaveForm(string keyValue, LaborinfoEntity entity, string json, string ID)
        {
            try
            {
                service.SaveForm(keyValue, entity, json, ID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
