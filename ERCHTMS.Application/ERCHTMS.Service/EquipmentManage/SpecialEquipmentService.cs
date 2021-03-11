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
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;
using System.Text;

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备基本信息表
    /// </summary>
    public class SpecialEquipmentService : RepositoryFactory<SpecialEquipmentEntity>, SpecialEquipmentIService
    {
        private DepartmentService DepartmentService = new DepartmentService();
        #region 获取数据
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

            //是否离场
            if (!queryParam["ispresence"].IsEmpty())
            {
                pagination.conditionJson += " and state in (select itemvalue from base_dataitemdetail where itemid in( select itemid from base_dataitem where itemcode ='EQUIPMENTSTATE') and itemname='离厂')";
            }
            else
            {
                pagination.conditionJson += " and state not in (select itemvalue from base_dataitemdetail where itemid in( select itemid from base_dataitem where itemcode ='EQUIPMENTSTATE') and itemname='离厂')";
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
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                pagination.conditionJson += string.Format(" and purchasetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                pagination.conditionJson += string.Format(" and purchasetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            var checkStartTime = queryParam["checkstartTime"];
            if (checkStartTime.IsEmpty() == false)
            {
                pagination.conditionJson += string.Format(" and NextCheckDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", checkStartTime.ToString());
            }
            var checkEndTime = queryParam["checkendTime"];
            if (checkEndTime.IsEmpty() == false)
            {
                pagination.conditionJson += string.Format(" and NextCheckDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(checkEndTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SpecialEquipmentEntity> GetList(string sql)
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
        public SpecialEquipmentEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 根据Id获取特种设备或普通设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetEquimentList(string id)
        {
            return BaseRepository().FindTable(string.Format("select * from (select id,equipmentno,equipmentname,DistrictId,district,districtcode from BIS_EQUIPMENT union select id,equipmentno,equipmentname,DistrictId,district,districtcode from BIS_SPECIALEQUIPMENT) a where id='{0}'", id));
           
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
            if(dt.Rows.Count>0)
            {
                string no = "0";
                if (dt != null && dt.Rows.Count > 0)
                {
                    no = dt.Rows[0][0].ToString();
                    no = no.Replace(EquipmentNo, "");
                }

                return no;
            }
            else
            {
                return "0";
            }
            
        }

        /// <summary>
        /// 获取设备类别统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="se"></param>
        /// <returns></returns>
        public DataTable GetEquipmentTypeStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            string sqlwhere1 = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["StartTime"].IsEmpty())
            {
                string startTime = queryParam["StartTime"].ToString();
                string endTime = queryParam["EndTime"].ToString();
                if (queryParam["EndTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                sqlwhere = string.Format(" and t.purchasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                //sqlwhere1 = string.Format(" and c.purchasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            string sql = string.Format(@"select s.itemname,s.itemvalue,(select count(1) from  BIS_specialequipment t
 where t.state!='5' and  t.affiliation='1' and t.equipmenttype=s.itemvalue and t.createuserorgcode='{1}' {0})  as OwnEquipment ,(select count(1) from  BIS_specialequipment t
 where t.state!='5' and  t.affiliation='2' and t.equipmenttype=s.itemvalue and t.createuserorgcode='{1}' {0}) as ExternalEquipment,s.sortcode
 from (select a.itemname,a.itemvalue,a.sortcode from BASE_DATAITEMDETAIL a
 left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='EQUIPMENTTYPE' ) s 
 group by s.itemname,s.itemvalue,s.sortcode order by s.sortcode ", sqlwhere, user.OrganizeCode);
            DataTable data = this.BaseRepository().FindTable(sql);

            return data;

            #region 注释
            //List<object> data = new List<object>();
            //dynamic item = new ExpandoObject();
            //            string EquipmentType = "[";//设备类别字符串
            //            string OwnEquipment = "[";//自有设备
            //            string ExternalEquipment = "[";//外委设备
            //            //查询设备类别
            //            string sql = string.Format(@"select a.itemname,a.itemvalue from BASE_DATAITEMDETAIL a
            // left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='EQUIPMENTTYPE' order by a.sortcode ");
            //            DataTable dtType = this.BaseRepository().FindTable(sql);
            //            if (dtType != null && dtType.Rows.Count > 0)
            //            {
            //                foreach (DataRow drType in dtType.Rows)
            //                {
            //                    EquipmentType += "'" + drType["itemname"].ToString() + "'" + ",";
            //                    OwnEquipment += se.Count(a => a.EquipmentType == drType["itemvalue"].ToString() && a.Affiliation == "1").ToString() + ",";//本单位自有
            //                    ExternalEquipment += se.Count(a => a.EquipmentType == drType["itemvalue"].ToString() && a.Affiliation == "2").ToString() + ",";//外委单位所有
            //                }
            //                EquipmentType = EquipmentType.Substring(0, EquipmentType.Length - 1) + "]";
            //                OwnEquipment = OwnEquipment.Substring(0, OwnEquipment.Length - 1) + "]";
            //                ExternalEquipment = ExternalEquipment.Substring(0, ExternalEquipment.Length - 1) + "]";
            //                item.EquipmentType = EquipmentType;
            //                item.OwnEquipment = OwnEquipment;
            //                item.ExternalEquipment = ExternalEquipment;
            //                data.Add(item);
            //            }
            //            else {
            //                return data;
            //            }
            #endregion

        }


        /// <summary>
        /// 获取省级设备类别列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentTypeStatGridForSJ(string queryJson)

        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            string sqlwhere1 = string.Empty;
            DataTable dt = new DataTable();
            dt.Columns.Add("orgcode");
            dt.Columns.Add("name");
            dt.Columns.Add("newcode");
            var queryParam = queryJson.ToJObject();
            if (!queryParam["StartTime"].IsEmpty())
            {
                string startTime = queryParam["StartTime"].ToString();
                string endTime = queryParam["EndTime"].ToString();
                if (queryParam["EndTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                sqlwhere = string.Format(" and purchasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                //sqlwhere1 = string.Format(" and c.purchasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            string forsql = "select '#orgcode' as orgcode, '#deptname' as name, '#newcode' as newcode ";
            //查询设备类别
            string strsql = @"select a.itemname,a.itemvalue,a.sortcode from BASE_DATAITEMDETAIL a
                                 left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='EQUIPMENTTYPE'";
            DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            if (dtsqlde != null && orgcodelist.Count() > 0)
            {
                for (int i = 1; i <= dtsqlde.Rows.Count; i++)
                {
                    dt.Columns.Add("type" + i);
                    forsql += " ,(select count(1) || ',"+ dtsqlde.Rows[i - 1][0].ToString() + "' || ',"+ dtsqlde.Rows[i - 1][1].ToString() + "' from  BIS_specialequipment  where equipmenttype = '" + dtsqlde.Rows[i - 1][1].ToString() + "' and createuserorgcode = '#orgcode' " + sqlwhere + " ) as type" + i;
                }
                dt.Columns.Add("type" + (dtsqlde.Rows.Count + 1));
                
                forsql += " ,(select count(1) || ',总计,00' from  BIS_specialequipment  where 1=1 and createuserorgcode = '#orgcode' " + sqlwhere + " ) as type" + (dtsqlde.Rows.Count + 1) + " from dual";
                DataRow dtrow = dt.NewRow();
                DataTable dtresult1 = new DataTable();
                string strorgcodelist = string.Empty;
                foreach (DepartmentEntity item in orgcodelist)
                {
                    dtrow = dt.NewRow();
                    dtresult1 = this.BaseRepository().FindTable(forsql.Replace("#deptname", item.FullName).Replace("#orgcode", item.EnCode).Replace("#newcode", item.DeptCode));
                    if (dtresult1 != null)
                    {
                        dtrow = dtresult1.Rows[0];
                        dt.Rows.Add(dtrow.ItemArray);
                    }
                    strorgcodelist += "'" + item.EnCode + "',";
                }
                dtrow = dt.NewRow();
                if (!string.IsNullOrEmpty(strorgcodelist))
                {
                    strorgcodelist = strorgcodelist.Substring(0, strorgcodelist.Length - 1);
                }
                dtresult1 = this.BaseRepository().FindTable(forsql.Replace("#deptname", "总计").Replace("createuserorgcode = '#orgcode'", "createuserorgcode in (" + strorgcodelist + ")").Replace("#orgcode", user.OrganizeCode).Replace("#newcode", user.DeptCode));
                if (dtresult1 != null)
                {
                    dtrow = dtresult1.Rows[0];
                    dt.Rows.Add(dtrow.ItemArray);
                }
            }
            
            return dt;
            
        }

        /// <summary>
        /// 获取省级设备类别图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStatDataForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            string sqlwhere1 = string.Empty;
            List<object> data = new List<object>();
            List<string> x = new List<string>();
            List<int> y = new List<int>();
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            var queryParam = queryJson.ToJObject();
            if (!queryParam["StartTime"].IsEmpty())
            {
                string startTime = queryParam["StartTime"].ToString();
                string endTime = queryParam["EndTime"].ToString();
                if (queryParam["EndTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                sqlwhere = string.Format(" and purchasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                //sqlwhere1 = string.Format(" and c.purchasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            string forsql = "select '#deptname' as name ";
            //查询设备类别
            string strsql = @"select a.itemname,a.itemvalue,a.sortcode from BASE_DATAITEMDETAIL a
                                 left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='EQUIPMENTTYPE'";
            DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            if (dtsqlde != null && orgcodelist.Count() > 0)
            {
                for (int i = 1; i <= dtsqlde.Rows.Count; i++)
                {
                    x.Add(dtsqlde.Rows[i - 1][0].ToString());
                    dt.Columns.Add("type" + i);
                    forsql += " ,(select count(1) from  BIS_specialequipment  where equipmenttype = '" + dtsqlde.Rows[i - 1][1].ToString() + "' and createuserorgcode = '#orgcode' " + sqlwhere + " ) as type" + i;
                }
                forsql += " from dual";
                DataRow dtrow = dt.NewRow();
                DataTable dtresult1 = new DataTable();
                foreach (DepartmentEntity item in orgcodelist)
                {
                    y = new List<int>();
                    dtrow = dt.NewRow();
                    dtresult1 = this.BaseRepository().FindTable(forsql.Replace("#deptname", item.FullName).Replace("#orgcode", item.EnCode));
                    if (dtresult1 != null)
                    {
                        for (int i = 1; i < dtresult1.Columns.Count; i++)
                        {
                            y.Add(int.Parse(dtresult1.Rows[0][i].ToString()));
                        }
                        data.Add(new { name = item.FullName, data = y });
                    }
                    
                }
                y = new List<int>();
                dtrow = dt.NewRow();
                dtresult1 = this.BaseRepository().FindTable(forsql.Replace("#deptname", "总计").Replace("and createuserorgcode = '#orgcode'", ""));
                if (dtresult1 != null)
                {
                    for (int i = 1; i < dtresult1.Columns.Count; i++)
                    {
                        y.Add(int.Parse(dtresult1.Rows[0][i].ToString()));
                    }
                    data.Add(new { name = "总计", data = y });
                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { data = data, x = x });
        }



        /// <summary>
        /// 获取省级隐患数量图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentHidDataForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            List<object> data = new List<object>();
            List<int> data1 = new List<int>();
            List<string> x = new List<string>();
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }
            sqlwhere = "  and to_char(t2.checkdate,'yyyy')='" + year + "'";
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            foreach (DepartmentEntity item in orgcodelist)
            {
                x.Add(item.FullName);
            }
            x.Add("总计");
            if ( orgcodelist.Count() > 0)
            {
                string forsql = string.Format(@"select t2.createuserorgcode,count(1) from v_basehiddeninfo t1 left join BIS_specialequipment t2 on t1.deviceid=t2.id   where 1=1  {0} group by t2.createuserorgcode", sqlwhere);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                int total = 0;
                if (dtresult != null)
                {
                    int count1 = 0;
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                        data1.Add(count1);
                        total += count1;
                    }
                    data1.Add(total);

                }
                data.Add(new { name = year, data = data1 });
                
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = x, data = data });
        }

        /// <summary>
        /// 获取省级隐患数量列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentHidGridForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            DataTable dt = new DataTable();
            dt.Columns.Add("typename");
            dt.Columns.Add("num");
            dt.Columns.Add("orgcode");
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }
            sqlwhere = "  and to_char(t2.checkdate,'yyyy')='" + year + "'";
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            if (orgcodelist.Count() > 0)
            {
                string forsql = string.Format(@"select t2.createuserorgcode,count(1) from v_basehiddeninfo t1 left join BIS_specialequipment t2 on t1.deviceid=t2.id   where 1=1  {0} group by t2.createuserorgcode", sqlwhere);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                int total = 0;
                if (dtresult != null)
                {
                    int count1 = 0;
                    DataRow dtRow;
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        dtRow = dt.NewRow();
                        count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                        dtRow["typename"] = item.FullName;
                        dtRow["num"] = count1;
                        dtRow["orgcode"] = item.EnCode;
                        total += count1;
                        dt.Rows.Add(dtRow);
                    }
                    dtRow = dt.NewRow();
                    dtRow["typename"] = "总计";
                    dtRow["num"] = total;
                    dtRow["orgcode"] = "";
                    dt.Rows.Add(dtRow);
                }

            }
            return dt;
            
        }


        /// <summary>
        /// 获取省级检查次数图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentCheckDataForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            List<object> data = new List<object>();
            List<int> data1 = new List<int>();
            List<string> x = new List<string>();
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }
            sqlwhere = "  and to_char(t2.purchasetime,'yyyy')='" + year + "'";
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            foreach (DepartmentEntity item in orgcodelist)
            {
                x.Add(item.FullName);
            }
            x.Add("总计");
            if (orgcodelist.Count() > 0)
            {
                string forsql = string.Format(@"select t2.createuserorgcode,count(1) from bis_saftycheckdatarecord t3 left join  bis_saftycheckdatadetailed t1 on t3.id=t1.recid left join BIS_specialequipment t2 on t1.checkobjectid=t2.id   where t1.checkobjecttype ='0'  {0} group by t2.createuserorgcode", sqlwhere);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                int total = 0;
                if (dtresult != null)
                {
                    int count1 = 0;
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                        data1.Add(count1);
                        total += count1;
                    }
                    data1.Add(total);

                }
                data.Add(new { name = year, data = data1 });

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = x, data = data });
        }

        /// <summary>
        /// 获取省级安全检查列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentCheckGridForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            DataTable dt = new DataTable();
            dt.Columns.Add("orgcode");
            dt.Columns.Add("typename");
            dt.Columns.Add("num");
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }
            sqlwhere = "  and to_char(t2.purchasetime,'yyyy')='" + year + "'";
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            if (orgcodelist.Count() > 0)
            {
                string forsql = string.Format(@"select t2.createuserorgcode,count(1) from bis_saftycheckdatarecord t3 left join  bis_saftycheckdatadetailed t1 on t3.id=t1.recid left join BIS_specialequipment t2 on t1.checkobjectid=t2.id   where t1.checkobjecttype ='0'  {0} group by t2.createuserorgcode", sqlwhere);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                int total = 0;
                if (dtresult != null)
                {
                    int count1 = 0;
                    DataRow dtRow;
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        dtRow = dt.NewRow();
                        count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                        dtRow["orgcode"] = item.EnCode;
                        dtRow["typename"] = item.FullName;
                        dtRow["num"] = count1;
                        total += count1;
                        dt.Rows.Add(dtRow);
                    }
                    dtRow = dt.NewRow();
                    dtRow["orgcode"] = user.OrganizeCode;
                    dtRow["typename"] = "总计";
                    dtRow["num"] = total;
                    dt.Rows.Add(dtRow);
                }

            }
            return dt;

        }


        /// <summary>
        /// 获取省级运行故障图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentFailureDataForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            List<object> data = new List<object>();
            List<int> data1 = new List<int>();
            List<string> x = new List<string>();
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }
            sqlwhere = "  and to_char(t2.purchasetime,'yyyy')='" + year + "'";
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            foreach (DepartmentEntity item in orgcodelist)
            {
                x.Add(item.FullName);
            }
            x.Add("总计");
            if (orgcodelist.Count() > 0)
            {
                string forsql = string.Format(@"select t2.createuserorgcode,count(1) from bis_operationfailure t1 left join BIS_specialequipment t2 on t1.equipmentid=t2.id   where 1=1  {0} group by t2.createuserorgcode", sqlwhere);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                int total = 0;
                if (dtresult != null)
                {
                    int count1 = 0;
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                        data1.Add(count1);
                        total += count1;
                    }
                    data1.Add(total);

                }
                data.Add(new { name = year, data = data1 });

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = x, data = data });
        }

        /// <summary>
        /// 获取省级运行故障列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentFailureGridForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            DataTable dt = new DataTable();
            dt.Columns.Add("orgcode");
            dt.Columns.Add("typename");
            dt.Columns.Add("num");
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }
            sqlwhere = "  and to_char(t2.purchasetime,'yyyy')='" + year + "'";
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "厂级");
            if (orgcodelist.Count() > 0)
            {
                string forsql = string.Format(@"select t2.createuserorgcode,count(1) from bis_operationfailure t1 left join BIS_specialequipment t2 on t1.equipmentid=t2.id    where 1=1  {0} group by t2.createuserorgcode", sqlwhere);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                int total = 0;
                if (dtresult != null)
                {
                    int count1 = 0;
                    DataRow dtRow;
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        dtRow = dt.NewRow();
                        count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                        dtRow["orgcode"] = item.EnCode;
                        dtRow["typename"] = item.FullName;
                        dtRow["num"] = count1;
                        total += count1;
                        dt.Rows.Add(dtRow);
                    }
                    dtRow = dt.NewRow();
                    dtRow["orgcode"] = user.OrganizeCode;
                    dtRow["typename"] = "总计";
                    dtRow["num"] = total;
                    dt.Rows.Add(dtRow);
                }

            }
            return dt;

        }

        /// <summary>
        /// 获取设备运行故障统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="se"></param>
        /// <returns></returns>
        public object GetOperationFailureStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            dynamic dy = new ExpandoObject();
            var queryParam = queryJson.ToJObject();
            int count = 0;
            int count1 = 0;
            if (!queryParam["type"].IsEmpty())
            {
                //获取月度统计年度信息
                string year = queryParam["year"].ToString();
                string m = "[";//月度
                string OwnEquipment = "[";//自有设备
                string ExternalEquipment = "[";//外委设备
                string SumNum = "[";
                string sql = string.Empty;
                for (int i = 1; i < 13; i++)
                {
                    string mm = i.ToString();
                    if (i < 10)
                    {
                        mm = "0" + i.ToString();
                    }
                    m += i + ",";
                    sql = string.Format(@"select count(e.id) from BIS_specialequipment t left join BIS_operationFailure e on t.id=e.equipmentid
 where  to_char(e.registerdate,'yyyy-MM')='{0}' and t.affiliation='1' and t.createuserorgcode='{1}'", year + "-" + mm,user.OrganizeCode);
                    count =int.Parse(this.BaseRepository().FindObject(sql).ToString());
                    OwnEquipment += count + ",";
                    sql = string.Format(@"select count(e.id) from BIS_specialequipment t left join BIS_operationFailure e on t.id=e.equipmentid
 where  to_char(e.registerdate,'yyyy-MM')='{0}' and t.affiliation='2' and t.createuserorgcode='{1}' ", year + "-" + mm, user.OrganizeCode);
                    count1 = int.Parse(this.BaseRepository().FindObject(sql).ToString());
                    ExternalEquipment += count1 + ",";
                    SumNum += (count + count1) + ",";
                }
                SumNum = SumNum.Substring(0, SumNum.Length - 1) + "]";
                m = m.Substring(0, m.Length - 1) + "]";
                OwnEquipment = OwnEquipment.Substring(0, OwnEquipment.Length - 1) + "]";
                ExternalEquipment = ExternalEquipment.Substring(0, ExternalEquipment.Length - 1) + "]";
                dy.y = m;
                dy.OwnEquipment = OwnEquipment;
                dy.ExternalEquipment = ExternalEquipment;
                dy.SumNum = SumNum;
            }
            else
            {
                string y = "[";//年度
                string OwnEquipment = "[";//自有设备
                string ExternalEquipment = "[";//外委设备
                //获取当前年度
                int year = DateTime.Now.Year - 5;
                string SumNum = "[";
                string sql = string.Empty;
                for (int i = 0; i < 6; i++)
                {
                    y += year + ",";
                    sql = string.Format(@"select count(e.id) from BIS_specialequipment t left join BIS_operationFailure e on t.id=e.equipmentid
 where  to_char(e.registerdate,'yyyy')='{0}' and t.affiliation='1'", year);
                    count = int.Parse(this.BaseRepository().FindObject(sql).ToString());
                    OwnEquipment += count + ",";
                    sql = string.Format(@"select count(e.id) from BIS_specialequipment t left join BIS_operationFailure e on t.id=e.equipmentid
 where  to_char(e.registerdate,'yyyy')='{0}' and t.affiliation='2'", year);
                    count1 = int.Parse(this.BaseRepository().FindObject(sql).ToString());
                    ExternalEquipment += count1 + ",";
                    SumNum += (count + count1) + ",";
                    year++;
                }
                SumNum = SumNum.Substring(0, SumNum.Length - 1) + "]";
                y = y.Substring(0, y.Length - 1) + "]";
                OwnEquipment = OwnEquipment.Substring(0, OwnEquipment.Length - 1) + "]";
                ExternalEquipment = ExternalEquipment.Substring(0, ExternalEquipment.Length - 1) + "]";
                dy.y = y;
                dy.OwnEquipment = OwnEquipment;
                dy.ExternalEquipment = ExternalEquipment;
                dy.SumNum = SumNum;
            }


            return dy;
        }


        /// <summary>
        /// 获取省级安全检查记录
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetSafetyCheckRecordForSJ(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            DataTable dt = new DataTable();
            var queryParam = queryJson.ToJObject();
            string year = string.Empty;
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            else
            {
                year = DateTime.Now.ToString("yyyy");
            }

            sqlwhere = " and to_char(e.purchasetime,'yyyy')='" + year + "'";
            sqlwhere += " and e.createuserorgcode in (select encode from base_department where deptcode like '" + user.NewDeptCode + "%')";
            if (!queryParam["code"].IsEmpty())
            {
                if (queryParam["code"].ToString() != user.OrganizeCode)
                {
                    sqlwhere += " and e.createuserorgcode ='" + queryParam["code"].ToString() + "'";
                }
            }
            string forsql = string.Format(@"  select b.id as hdid ,e.id,e.certificateid,e.checkfileid,e.acceptance,f.organizeid,b.checkendtime,b.checkdatarecordname,case when b.checkdatatype=1 then b.checkman else (select wm_concat(modifyusername) from BIS_SAFTYCHECKDATADETAILED 
                      where recid = b.id and checkobjectid = d.checkobjectid) end as CHECKMAN,(select count(id) from bis_htbaseinfo o where o.safetycheckobjectid = b.id and o.deviceid = d.checkobjectid) as Count
                      from bis_saftycheckdatarecord b left join BIS_SAFTYCHECKDATADETAILED d on b.id = d.recid left join BIS_specialequipment e on d.checkobjectid = e.id left join base_department f on e.createuserorgcode = f.encode where d.checkobjecttype = '0' {0} order by b.createdate desc", sqlwhere);
            dt = this.BaseRepository().FindTable(forsql);
            return dt;
        }



        /// <summary> 
        /// 通过设备id获取特种设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSpecialEquipmentTable(string[] ids)
        {
            var strSql = new StringBuilder();
            string sql = string.Join(",", ids).Replace(",", "','");
            strSql.Append(string.Format(@"SELECT * FROM bis_specialequipment WHERE ID IN('{0}')", sql));
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
        public void SaveForm(string keyValue, SpecialEquipmentEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                SpecialEquipmentEntity se = this.BaseRepository().FindEntity(keyValue);
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
        /// 同步外包特种设备到双控平台设备
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <param name="deptId">外包单位Id</param>
        /// <returns></returns>
        public bool SyncSpecificTools(string projectId, string deptId,string toolsid)
        {
            var res = this.BaseRepository().BeginTrans();
            try
            {
                //获取需同步设备
                string sql = string.Format(@"select t1.CREATEDATE,t1.CREATEUSERDEPTCODE,t1.CREATEUSERORGCODE,t1.CREATEUSERID,t1.CREATEUSERNAME,
 t1.TOOLSNAME,t1.TOOLSTYPE,t1.SAFEMANAGER,t1.SAFEMANAGERID,
 t1.PHONENUMBER,t1.TOOLSMODEL,t1.AREANAME,t1.AREAID,t1.AREACODE,t1.CHECKDATE,
 t1.NEXTCHECKDATE,t1.REGISTERNUMBER,t1.REGISTERCARDFILE,t1.OPERATIONPEOPLE,t1.OPERATIONPEOPLEID,
 t1.TOOLSMADECOMPANY,t1.TOOLSINITNUMBER,t1.TOOLSMADEDATE,t1.MANAGERDEPT,t1.TOOLSBUYDATE,t1.MANAGERDEPTID,
 t1.MANAGERDEPTCODE,t1.CHECKDAYS,t1.OUTCOMPANYNAME,t1.OUTCOMPANYID,t1.OUTPROJECTNAME,t1.OUTPROJECTID
 from epg_specifictools t1 where t1.OUTCOMPANYID='{0}' and t1.OUTPROJECTID='{1}' and TOOLSID='{2}'", deptId, projectId,toolsid);
                Repository<SpecificToolsEntity> proecss = new Repository<SpecificToolsEntity>(DbFactory.Base());
                DataTable dt = proecss.FindTable(sql);
                string sql1 = string.Empty;
                string affiliation = "2";//所属关系默认外包单位所有
                string ISCHECK = "是";//是否检查验收默认是
                string STATE = "1";//默认未启用(开工申请通过后改为在用)
                if (dt != null && dt.Rows.Count>0) {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string id = Guid.NewGuid().ToString();
                        string equipmentno = "";//设备编号自动生成
                        string a = "T2-";
                        string b = string.Empty;
                        switch (dr["TOOLSTYPE"].ToString())
                        {
                            case "1":
                                b = "GL";
                                break;
                            case "2":
                                b = "RQ";
                                break;
                            case "3":
                                b = "GD";
                                break;
                            case "4":
                                b = "QZ";
                                break;
                            case "5":
                                b = "CL";
                                break;
                            case "6":
                                b = "FJ";
                                break;
                            case "7":
                                b = "DT";
                                break;
                        }
                        string no = GetEquipmentNo(a + b, dr["CREATEUSERORGCODE"].ToString());
                        int num = int.Parse(no) + 1;
                        switch (num.ToString().Length)
                        {
                            case 1:
                                no = "00" + num;
                                break;
                            case 2:
                                no = "0" + num;
                                break;
                        }
                        equipmentno = a + b + no;
                        sql1 = string.Format(@" insert into bis_specialequipment t (t.createdate,t.createuserdeptcode,t.createuserorgcode,t.createuserid,t.createusername,
 t.equipmentname,t.equipmenttype,t.SECURITYMANAGERUSER,t.SECURITYMANAGERUSERID,
 t.TELEPHONE,t.SPECIFICATIONS,t.DISTRICT,t.DISTRICTID,t.DISTRICTCODE,t.CHECKDATE,
 t.NEXTCHECKDATE,t.CERTIFICATENO,t.CERTIFICATEID,t.OPERUSER,t.OPERUSERID,
 t.OUTPUTDEPTNAME,t.FACTORYNO,t.FACTORYDATE,t.CONTROLDEPT,t.PURCHASETIME,t.CONTROLDEPTID,
 t.CONTROLDEPTCODE,t.CHECKDATECYCLE,t.EPIBOLYDEPT,t.EPIBOLYDEPTID,t.EPIBOLYPROJECT,
t.EPIBOLYPROJECTID,affiliation,equipmentno,ISCHECK,STATE,t.id) 
values(to_date('{0}','yyyy-MM-dd hh24:mi:ss'),'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'
,'{13}',to_date('{14}','yyyy-MM-dd hh24:mi:ss'),to_date('{15}','yyyy-MM-dd hh24:mi:ss'),'{16}','{17}','{18}','{19}','{20}','{21}',to_date('{22}','yyyy-MM-dd hh24:mi:ss'),'{23}',to_date('{24}','yyyy-MM-dd hh24:mi:ss'),'{25}','{26}'
,'{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}')", dr["CREATEDATE"], dr["CREATEUSERDEPTCODE"], dr["CREATEUSERORGCODE"],
                    dr["CREATEUSERID"], dr["CREATEUSERNAME"], dr["TOOLSNAME"], dr["TOOLSTYPE"], dr["SAFEMANAGER"], dr["SAFEMANAGERID"]
                                                                                   , dr["PHONENUMBER"], dr["TOOLSMODEL"], dr["AREANAME"]
                                                                                   , dr["AREAID"], dr["AREACODE"], dr["CHECKDATE"]
                                                                                   , dr["NEXTCHECKDATE"], dr["REGISTERNUMBER"], dr["REGISTERCARDFILE"]
                                                                                   , dr["OPERATIONPEOPLE"], dr["OPERATIONPEOPLEID"], dr["TOOLSMADECOMPANY"]
                                                                                   , dr["TOOLSINITNUMBER"], dr["TOOLSMADEDATE"], dr["MANAGERDEPT"]
                                                                                   , dr["TOOLSBUYDATE"], dr["MANAGERDEPTID"], dr["MANAGERDEPTCODE"]
                                                                                   , dr["CHECKDAYS"], dr["OUTCOMPANYNAME"], dr["OUTCOMPANYID"]
                                                                                   , dr["OUTPROJECTNAME"], dr["OUTPROJECTID"], affiliation, equipmentno, ISCHECK, STATE, id);
                        res.ExecuteBySql(sql1);
                    }
                }
                
                res.Commit();
                return true;
            }
            catch
            {
                res.Rollback();
                return false;
            }
        }


        /// <summary>
        /// 特种设备离场
        /// </summary>
        /// <param name="equipmentId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        public int SetLeave(string specialequipmentId, string leaveTime, string DepartureReason)
        {
            DataItemDetailService service = new DataItemDetailService();
            leaveTime = "to_date('" + leaveTime + " 00:00:00','yyyy-mm-dd hh24:mi:ss')";
            return this.BaseRepository().ExecuteBySql(string.Format("update bis_specialequipment set state={0},DepartureTime={1},DepartureReason='{3}' where id in('{2}')", service.GetItemValue("离厂", "EQUIPMENTSTATE"), leaveTime, specialequipmentId.Replace(",", "','"), DepartureReason));

        }
        /// <summary>
        /// 特种设备批量修改检验日期（同时需要修改下次检验时间）
        /// </summary>
        /// <param name="specialequipmentId"></param>
        /// <param name="CheckDate"></param>
        /// <returns></returns>
        public int SetCheck(string specialequipmentId, string CheckDate)
        {
            CheckDate = "to_date('" + CheckDate + " 00:00:00','yyyy-mm-dd hh24:mi:ss')";
            return this.BaseRepository().ExecuteBySql(string.Format("update bis_specialequipment set CheckDate={0},NextCheckDate={0}+CHECKDATECYCLE  where id in('{1}')", CheckDate, specialequipmentId.Replace(",", "','")));
        }
        #endregion

        #region app接口
        /// <summary>
        /// 查询sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable SelectData(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 修改sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int UpdateData(string sql)
        {
            return this.BaseRepository().ExecuteBySql(sql);
        }
        #endregion

    }
}
