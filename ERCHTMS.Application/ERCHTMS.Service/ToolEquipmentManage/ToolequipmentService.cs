using System;
using ERCHTMS.Entity.ToolEquipmentManage;
using ERCHTMS.IService.ToolEquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：工器具基础信息表
    /// </summary>
    public class ToolequipmentService : RepositoryFactory<ToolequipmentEntity>, ToolequipmentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ToolequipmentEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ToolequipmentEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            //工器具大类(1:电动工器具；2:安全工器具；3:手工器具)
            if (!queryParam["ToolType"].IsEmpty())
            {
                if (!string.IsNullOrEmpty(queryParam["ToolType"].ToString()) && queryParam["ToolType"].ToString().Length < 3)
                    pagination.conditionJson += string.Format(" and ToolType='{0}'", queryParam["ToolType"].ToString());
            }
            //设备名称
            if (!queryParam["Etype"].IsEmpty())
            {
                if (!string.IsNullOrEmpty(queryParam["Etype"].ToString()) && queryParam["Etype"].ToString().Length < 3)
                    pagination.conditionJson += string.Format(" and EquipmentType='{0}'", queryParam["Etype"].ToString());
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }

            if (user.RoleName.Contains("省级用户"))
            {
                if (!queryParam["code"].IsEmpty())
                {
                    if (queryParam["code"].ToString() != user.OrganizeCode)
                    {
                        pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  in (select encode from base_department where deptcode like  '{0}%')", queryParam["code"].ToString());
                    }
                }
            }
            else
            {
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    string deptCode = queryParam["code"].ToString();
                    string orgType = queryParam["isOrg"].ToString();
                    if (orgType == "District")
                    {
                        pagination.conditionJson += string.Format(" and DISTRICTCODE  like '{0}%'", deptCode);
                    }
                    else if (orgType == "厂级")
                    {
                        pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ControlDeptCode like '{0}%'", deptCode);
                    }
                }
            }
            //时间范围
            if (!queryParam["startTime"].IsEmpty() || !queryParam["endTime"].IsEmpty())
            {
                string startTime = queryParam["startTime"].ToString() + " 00:00:00";
                string endTime = queryParam["endTime"].ToString() + " 23:59:59";
                if (queryParam["startTime"].IsEmpty())
                {
                    startTime = "1899-01-01" + " 00:00:00";
                }
                if (queryParam["endTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
                }
                pagination.conditionJson += string.Format(" and checkdate between to_date('{0}','yyyy-MM-dd HH24:mi:ss') and  to_date('{1}','yyyy-MM-dd HH24:mi:ss')", startTime, endTime);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <param name="EquipmentNo">设备类别</param>
        /// <returns></returns>
        public string GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            //获取最新创建的设备编号
            string sql = string.Format("select t.equipmentno from BIS_specialequipment t where t.equipmentno like '{0}%' and t.createuserorgcode='{1}' order by t.createdate desc", EquipmentNo, orgcode);
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
        /// 获取设备类别统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStat(string queryJson)
        {
            List<object> dic = new List<object>();
            List<string> listtypes = new List<string>();

            List<Object> numInts = new List<Object>();
            string sqlwhere = string.Empty;
            var queryParam = queryJson.ToJObject();
            //时间范围
            if (!queryParam["StartTime"].IsEmpty() || !queryParam["EndTime"].IsEmpty())
            {
                string startTime = queryParam["StartTime"].ToString() + " 00:00:00";
                string endTime = queryParam["EndTime"].ToString() + " 23:59:59";
                if (queryParam["StartTime"].IsEmpty())
                {
                    startTime = "1899-01-01" + " 00:00:00";
                }
                if (queryParam["EndTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
                }
                sqlwhere += string.Format(" and checkdate between to_date('{0}','yyyy-MM-dd HH24:mi:ss') and  to_date('{1}','yyyy-MM-dd HH24:mi:ss')", startTime, endTime);
            }
            if (!queryParam["EquipmentType"].IsEmpty())
            {
                string equipmenttype = queryParam["EquipmentType"].ToString();
                string tpyecode = "";
                if (equipmenttype == "1")
                {
                    tpyecode = "SafeTool";
                }
                else if(equipmenttype == "2")
                {
                    tpyecode = "JxHxSafeTool";
                }

                string strsql = @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = '"+tpyecode+"') order by SORTCODE ";
                DataTable dt = this.BaseRepository().FindTable(strsql);

                sqlwhere += string.Format(" and equipmenttype ='{0}'", equipmenttype);
                
                foreach (DataRow item in dt.Rows)
                {                   
                    listtypes.Add(item["itemname"].ToString());
                        string forsql =
                            string.Format(
                                @"select nvl(count(id),0) as cou from BIS_TOOLEQUIPMENT where tooltype = 2 and equipmentName ='{0}'  {1} ",
                                item["itemvalue"].ToString(), sqlwhere );
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        numInts.Add(num);

                    
                }
                dic.Add(new { name = "aaa", data = numInts });
            }


            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = listtypes, y = dic });
        }


        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetToolStatisticsList(string queryJson)
        {
            string sqlwhere = string.Empty;
            var queryParam = queryJson.ToJObject();
            //时间范围
            if (!queryParam["StartTime"].IsEmpty() || !queryParam["EndTime"].IsEmpty())
            {
                string startTime = queryParam["StartTime"].ToString() + " 00:00:00";
                string endTime = queryParam["EndTime"].ToString() + " 23:59:59";
                if (queryParam["StartTime"].IsEmpty())
                {
                    startTime = "1899-01-01" + " 00:00:00";
                }
                if (queryParam["EndTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
                }
                sqlwhere += string.Format(" and checkdate between to_date('{0}','yyyy-MM-dd HH24:mi:ss') and  to_date('{1}','yyyy-MM-dd HH24:mi:ss')", startTime, endTime);
            }


            DataTable dt = new DataTable();
            if (!queryParam["EquipmentType"].IsEmpty())
            {
                string equipmenttype = queryParam["EquipmentType"].ToString();
                sqlwhere += string.Format(" and equipmenttype ='{0}'", equipmenttype);

                string tpyecode = "";
                if (equipmenttype == "1")
                {
                    tpyecode = "SafeTool";
                }
                else if (equipmenttype == "2")
                {
                    tpyecode = "JxHxSafeTool";
                }

                string strsql =
                    @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = '" +
                    tpyecode + "') order by SORTCODE ";
                DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
                double Total = 0;
                dt.Columns.Add("EquipmentName");
                for (int i = 1; i <= dtsqlde.Rows.Count; i++)
                {
                    dt.Columns.Add("num" + i, typeof(int));
                }

                dt.Columns.Add("Total");
                DataRow row = dt.NewRow();
                row["EquipmentName"] = "数量";
                for (int k = 1; k <= dtsqlde.Rows.Count; k++)
                {
                    string forsql =
                        string.Format(
                            @"select nvl(count(id),0) as cou from BIS_TOOLEQUIPMENT where tooltype = 2 and equipmentName ='{0}'  {1}",
                            k.ToString(), sqlwhere);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    row["num" + k] = num;
                    Total += num;
                }

                row["Total"] = Total.ToString();
                dt.Rows.Add(row);
            }
                   
            return dt;
        }


        public DataTable GetToolRecordList(string keyValue)
        {
            string sql = string.Format("select * from BIS_TOOLRECORD where toolequipmentid ='{0}' order by createdate desc", keyValue);
            return this.BaseRepository().FindTable(sql);
            //List<object> objects = new List<object>();
            //if (!string.IsNullOrEmpty(keyValue))
            //{

            //    string sql = string.Format("select * from BIS_TOOLRECORD where toolequipmentid ='{0}'",keyValue);
            //    return this.BaseRepository().FindTable(sql);
            //    DataTable dt = this.BaseRepository().FindTable(sql);
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dataRow in dt.Rows)
            //        {
            //            objects.Add(new
            //            {
            //                id = dataRow["id"].ToString(),
            //                toolequipmentid = dataRow["toolequipmentid"].ToString(),
            //                equipmentname = dataRow["equipmentname"].ToString(),
            //                equipmentno = dataRow["equipmentno"].ToString(),
            //                voltagelevel = dataRow["voltagelevel"].ToString(),
            //                trialvoltage = dataRow["trialvoltage"].ToString(),
            //                checkdate = dataRow["checkdate"].ToString(),
            //                nextcheckdate = dataRow["nextcheckdate"].ToString(),
            //                appraise = dataRow["appraise"].ToString(),
            //                operuser = dataRow["operuser"].ToString()
            //            });
            //        }
            //    }
            //}        
            //return objects;
        }

        public object GetToolName(string tooltype)
        {
            string codeName = "";
            if (tooltype == "1")
            {
                codeName = "电动工器具";
            }
            else if (tooltype == "2")
            {
                codeName = "安全工器具";
            }
            else if (tooltype == "3")
            {
                codeName = "手工器具";
            }
            string sql = string.Format(@"select  itemname,itemvalue from base_dataitemdetail where itemid =(select itemid from base_dataitem  where itemname = '{0}') order by sortcode", codeName);
            DataTable dt = this.BaseRepository().FindTable(sql);
            List<object> objects = new List<object>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    objects.Add(new
                    {
                        itemname = row["ITEMNAME"].ToString(),
                        itemvalue = row["ITEMVALUE"].ToString()
                    });
                }

            }

            return objects;
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            ToolrecordService safekpidata = new ToolrecordService();
            List<ToolrecordEntity> list = safekpidata.GetList("").Where(p => p.ToolEquipmentId == keyValue).ToList();
            foreach (var toolrecordEntity in list)
            {
                safekpidata.RemoveForm(toolrecordEntity.Id);
            }           
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ToolequipmentEntity entity)
        {
            try
            {
                entity.Id = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    ToolequipmentEntity eEntity = GetEntity(keyValue);
                    if (eEntity != null)
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
            catch (Exception ex)
            {
                
                throw;
            }
            
        }

        /// <summary>
        /// 保存试验记录
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveToolrecord(string keyValue, ToolrecordEntity entity)
        {
            if (entity != null)
            {
                new ToolrecordService().SaveForm(keyValue, entity);
                if (!string.IsNullOrEmpty(entity.ToolEquipmentId))
                {
                    ToolequipmentEntity toolequipment =  GetEntity(entity.ToolEquipmentId);
                    toolequipment.CheckDate = entity.CheckDate;
                    toolequipment.NextCheckDate = entity.NextCheckDate;
                    toolequipment.OperUser = entity.OperUser;
                    toolequipment.OperUserId = entity.OperUserId;
                    this.BaseRepository().Update(toolequipment);
                }
            }        
        }
        #endregion
    }
}
