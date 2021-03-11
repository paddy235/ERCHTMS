using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using BSFramework.Data;
using System.Data;
using System.Dynamic;
using System;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Service.SystemManage;
using System.Text;

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// 描 述：普通设备基本信息表
    /// </summary>
    public class EquipmentService : RepositoryFactory<EquipmentEntity>, EquipmentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //设备类别
            if (!queryParam["Etype"].IsEmpty())
            {
                if (!string.IsNullOrEmpty(queryParam["Etype"].ToString()) && queryParam["Etype"].ToString().Length < 3)
                    pagination.conditionJson += string.Format(" and EQUIPMENTTYPE='{0}'", queryParam["Etype"].ToString());
            }
            //所属关系
            if (!queryParam["Affiliation"].IsEmpty())
            {
                if (!string.IsNullOrEmpty(queryParam["Affiliation"].ToString()) && queryParam["Affiliation"].ToString().Length < 3)
                    pagination.conditionJson += string.Format(" and Affiliation='{0}'", queryParam["Affiliation"].ToString());
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            //离场设备
            if (!queryParam["ispresence"].IsEmpty())
            {
                pagination.conditionJson += " and state in (select itemvalue from base_dataitemdetail where itemid in( select itemid from base_dataitem where itemcode ='EQUIPMENTSTATE') and itemname='离厂')";
            }
            else
            {
                pagination.conditionJson += " and state not in (select itemvalue from base_dataitemdetail where itemid in( select itemid from base_dataitem where itemcode ='EQUIPMENTSTATE') and itemname='离厂')";
            }

            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                string deptCode = queryParam["code"].ToString();
                string orgType = queryParam["isOrg"].ToString();
                if (orgType == "District")
                {
                    pagination.conditionJson += string.Format(" and DISTRICTCODE  like '{0}%'", deptCode);
                }
                else if (orgType == "Organize")
                {
                    pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and ControlDeptCode like '{0}%'", deptCode);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回列表</returns>
        public IEnumerable<EquipmentEntity> GetList(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return this.BaseRepository().IQueryable().ToList();
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EquipmentEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <param name="EquipmentNo">设备类别</param>
        /// <returns></returns>
        public string GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            //获取最新创建的设备编号
            string sql = string.Format("select t.equipmentno from BIS_equipment t where t.equipmentno like '{0}%' and t.createuserorgcode='{1}' order by t.createdate desc", EquipmentNo, orgcode);
            DataTable dt = this.BaseRepository().FindTable(sql);
            string no = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                no = dt.Rows[0][0].ToString();
                no = no.Replace(EquipmentNo, "");
            }

            return no;
        }

        /// <summary> 
        /// 通过用户id获取用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetEquipmentTable(string[] ids)
        {
            var strSql = new StringBuilder();
            string sql = string.Join(",", ids).Replace(",", "','");
            strSql.Append(string.Format(@"SELECT * FROM BIS_EQUIPMENT WHERE ID IN('{0}')", sql));
            return this.BaseRepository().FindTable(strSql.ToString());
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
        public void SaveForm(string keyValue, EquipmentEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EquipmentEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 普通设备离场
        /// </summary>
        /// <param name="equipmentId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        public int SetLeave(string equipmentId, string leaveTime, string DepartureReason)
        {
            DataItemDetailService service = new DataItemDetailService();
            leaveTime = "to_date('" + leaveTime + " 00:00:00','yyyy-mm-dd hh24:mi:ss')";
            return this.BaseRepository().ExecuteBySql(string.Format("update BIS_EQUIPMENT set state={0},DepartureTime={1},DepartureReason='{3}' where id in('{2}')", service.GetItemValue("离厂", "EQUIPMENTSTATE"), leaveTime, equipmentId.Replace(",", "','"), DepartureReason));

        }
        #endregion
    }
}
