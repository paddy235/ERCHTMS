using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using ERCHTMS.Service.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人详情表
    /// </summary>
    public class OccupationalstaffdetailBLL
    {
        private OccupationalstaffdetailIService service = new OccupationalstaffdetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取职业病人员清单人员数量
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        public int GetStaffListSum(string queryJson, string where)
        {
            return service.GetStaffListSum(queryJson, where);
        }
        /// <summary>
        /// 获取新的职业病统计表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetStatisticsUserTable(string year, string where)
        {
            return service.NewGetStatisticsUserTable(year, where);
        }
        /// <summary>
        /// 获取职业病人员清单(全部的)
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        public DataTable GetStaffList(string queryJson, string where)
        {
            return service.GetStaffList(queryJson, where);
        }
        /// <summary>
        /// 获取职业病统计表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetStatisticsUserTable(string year, string where)
        {
            return service.GetStatisticsUserTable(year, where);
        }
        /// <summary>
        /// 获取部门职业病统计表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetStatisticsDeptTable(string year, string where)
        {
            return service.GetStatisticsDeptTable(year, where);
        }
        /// <summary>
        /// 获取年度职业病统计表
        /// </summary>
        /// <param name="yearType">近几年数据</param>
        /// <param name="Dept">部门EnCode</param>
        /// <returns></returns>
        public DataTable GetStatisticsYearTable(int yearType, string Dept, string where)
        {
            return service.GetStatisticsYearTable(yearType, Dept, where);
        }
        /// <summary>
        /// 获取用户id下的所有体检记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserTable(string userid)
        {
            return service.GetUserTable(userid);
        }

        /// <summary>
        /// 获取职业病人员清单
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetStaffListPage(Pagination pagination, string queryJson)
        {
            return service.GetStaffListPage(pagination, queryJson);
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
        /// 根据父id和是否生病查询表中信息
        /// </summary>
        /// <param name="Pid">父id</param>
        /// <param name="SickType">是否生病</param>
        /// <returns></returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string Pid, int Issick)
        {
            return service.GetList(Pid, Issick);
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

        /// <summary>
        /// 获取用户id下的所有体检记录 新
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetUserTable(string userid)
        {
            return service.NewGetUserTable(userid);
        }

        /// <summary>
        /// 获取用户的接触危害因素
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserHazardfactor(string useraccount)
        {
            return service.GetUserHazardfactor(useraccount);
        }

        /// <summary>
        /// 判断是否有权限 厂领导/Ehs与人力资源部
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool IsPer()
        {
            bool IsPermission = false;
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return true;
            }

            //获取当前操作用户
            var Appuser = OperatorProvider.Provider.Current();   //获取用户基本信息
            //EHS部与人力资源部配置在字典中 通过字典查找
            var Perdeptname = Appuser.DeptCode;
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'SelectDept'").ToList();
            if (data != null)
            {
                foreach (var Peritem in data)
                {
                    string value = Peritem.ItemValue;
                    string[] values = value.Split('|');
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] == Perdeptname) //如果部门编码对应则是有权限的人
                        {
                            return true;
                        }
                    }
                }
            }

            //如果是厂领导也有权限
            if (Appuser.RoleName.Contains("厂级部门用户") ||  Appuser.RoleName.Contains("公司领导"))
            {
                IsPermission = true;
            }

            return IsPermission;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OccupationalstaffdetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, OccupationalstaffdetailEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据条件删除健康用户 重新添加
        /// </summary>
        /// <param name="time">体检时间</param>
        /// <param name="parenid">关联id</param>
        /// <returns></returns>
        public int Delete(string parenid, int SickType)
        {
            return service.Delete(parenid, SickType);
        }

        /// <summary>
        /// 根据关联id批量修改体检时间
        /// </summary>
        /// <param name="time">体检时间</param>
        /// <param name="parenid">关联id</param>
        /// <returns></returns>
        public int UpdateTime(DateTime time, string parenid)
        {
            return service.UpdateTime(time, parenid);
        }
        #endregion
    }
}
