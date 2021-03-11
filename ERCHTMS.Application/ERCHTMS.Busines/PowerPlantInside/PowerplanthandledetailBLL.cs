using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using ERCHTMS.Service.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Busines.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理信息
    /// </summary>
    public class PowerplanthandledetailBLL
    {
        private PowerplanthandledetailIService service = new PowerplanthandledetailService();
        private UserService userservice = new UserService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PowerplanthandledetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PowerplanthandledetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 根据事故事件处理记录ID获取事故事件处理信息列表
        /// </summary>
        /// <param name="keyValue">事故事件处理记录I</param>
        /// <returns></returns>
        public IList<PowerplanthandledetailEntity> GetHandleDetailList(string keyValue)
        {
            return service.GetHandleDetailList(keyValue);
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
        public void SaveForm(string keyValue, PowerplanthandledetailEntity entity)
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
        /// 获取操作人
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public string GetApproveUserName(string keyValue)
        {
            Pagination pagination = new Pagination();
            pagination.p_kid = "a.ID";
            pagination.p_fields = "a.powerplanthandleid,a.rectificationmeasures,a.rectificationdutyperson,a.rectificationdutydept,a.APPLYSTATE,a.rectificationtime,b.id as PowerPlantReformId,a.reasonandproblem,'' as applystatename,a.signpersonname,e.outtransferuseraccount,e.intransferuseraccount,e.outtransferusername,e.intransferusername,a.flowdept,a.flowrole";
            pagination.p_tablename = @" bis_powerplanthandledetail a left join bis_powerplantreform b on a.id=b.powerplanthandledetailid and b.disable=0
                                            left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and e.num=1";
            pagination.conditionJson = string.Format("a.id='{0}'", keyValue);
            pagination.sidx = "a.createdate";
            pagination.sord = "desc";
            pagination.page = 1;
            pagination.rows = 1;
            var data = GetPageList(pagination, "");
            string approveusername = "";
            foreach (DataRow item in data.Rows)
            {
                switch (item["applystate"].ToString())
                {
                    case "0":
                    case "1":
                    case "2":
                    case "5":
                        approveusername = "";
                        break;
                    case "3":
                        string rectificationdutyperson = item["rectificationdutyperson"].ToString(); //整改负责人
                        string outtransferusername = item["outtransferusername"].IsEmpty() ? "" : item["outtransferusername"].ToString();//转交申请人
                        string intransferuseruser = item["intransferusername"].IsEmpty() ? "" : item["intransferusername"].ToString();//转交接收人
                        string[] outtransferusernamelist = outtransferusername.Split(',');
                        string[] intransferuseruserlist = intransferuseruser.Split(',');
                        foreach (var temp in intransferuseruserlist)
                        {
                            if (!temp.IsEmpty() && !rectificationdutyperson.Contains(temp + ","))
                            {
                                rectificationdutyperson += (temp + ",");//将转交接收人加入整改人中
                            }
                        }
                        foreach (var temp in outtransferusernamelist)
                        {
                            if (!temp.IsEmpty() && rectificationdutyperson.Contains(temp + ","))
                            {
                                rectificationdutyperson = rectificationdutyperson.Replace(temp + ",", "");//将转交申请人从整改人中移除
                            }
                        }
                        approveusername = rectificationdutyperson;
                        break;
                    case "4":
                        string[] deptlist = item["flowdept"].ToString().Split(',');
                        string[] rolelist = item["flowrole"].ToString().Split(',');
                        IList<UserEntity> userlist = userservice.GetUserListByDeptId("'" + string.Join("','", deptlist) + "'", "'" + string.Join("','", rolelist) + "'", true, "");
                        string username = "";
                        if (userlist.Count > 0)
                        {
                            foreach (var temp in userlist)
                            {
                                username += temp.RealName + ",";
                            }
                            username = string.IsNullOrEmpty(username) ? "" : username.Substring(0, username.Length - 1);
                        }
                        approveusername = username;
                        break;

                    case "6":
                        approveusername = item["signpersonname"].ToString();
                        break;
                    default:
                        approveusername = "";
                        break;
                }
            }
            return approveusername;
        }
        #endregion
    }
}
