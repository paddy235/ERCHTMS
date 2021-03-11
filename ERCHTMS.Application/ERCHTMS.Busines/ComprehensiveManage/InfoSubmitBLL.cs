using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.IService.ComprehensiveManage;
using ERCHTMS.Service.ComprehensiveManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.JPush;
using System.Linq;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.ComprehensiveManage
{
    /// <summary>
    /// 描 述：信息报送表
    /// </summary>
    public class InfoSubmitBLL
    {
        private InfoSubmitIService service = new InfoSubmitService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InfoSubmitEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 首页提醒
        /// </summary>
        /// <param name="indexType">提醒类型（1：填报信息，2：新增报送要求）</param>
        /// <returns></returns>
        public int CountIndex(string indexType)
        {
            return service.CountIndex(indexType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public InfoSubmitEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InfoSubmitEntity entity)
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
        #endregion

        #region 短消息提醒
        /// <summary>
        /// 发送短消息提醒EHS部门用户上报EHS信息
        /// </summary>
        public void SendMessage()
        {
            var now = DateTime.Now;
            SendMessage(now);
        }
        /// <summary>
        /// 发送短消息提醒EHS部门用户上报EHS信息
        /// </summary>
        /// <param name="now">当前时间</param>
        public void SendMessage(DateTime now)
        {
            var deptBll = new DepartmentBLL();            
            var listEHS = new DataItemDetailBLL().GetDataItemListByItemCode("'EHSDepartment'").ToList();
            //枚举今日应提醒的上报类型，如：月报、季报、半年报、年报。
            var list = EnumWarTypeOfDay(now);
            foreach (var wType in list)
            {
                foreach (var ehsDepart in listEHS)
                {//各电厂的EHS部门，统一发送短消息。
                    var ehsdeptcode = ehsDepart.ItemValue;//EHS部门编码
                    var dept = deptBll.GetDeptOrgInfo(ehsDepart.ItemName);//EHS部门所属机构
                    if (dept != null)
                    {//ESH所属机构
                        var orgCode = dept.EnCode;
                        var orgName = dept.FullName;                        
                        var msgBody = GenMessageBody(wType, now, orgName);
                        if (!HasInfo(orgCode, wType, msgBody.Item4) && !ExistMsg(msgBody.Item3))
                        {//未上报，且未发短消息。
                            var msg = GenEntity(msgBody, ehsdeptcode);
                            if (msg != null)
                            {//发送短消息
                                if (new MessageBLL().SaveForm("", msg))
                                {
                                    JPushApi.PublicMessage(msg);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 枚举指定日期可提醒的类型
        /// </summary>
        /// <param name="now">指定日期</param>
        /// <returns>提醒类型集合</returns>
        private List<WarningType> EnumWarTypeOfDay(DateTime now)
        {
            var list = new List<WarningType>();

            //月度提醒
            var day = now.Day;
            var monEnd = DateTime.Parse(now.ToString("yyyy-MM-2"));
            if (day >= 2)//2号以后取下月作为结束时间
                monEnd = monEnd.AddMonths(1);
            var monStart = monEnd.AddDays(-14);
            if (now >= monStart && now < monEnd)
                list.Add(WarningType.Month);

            //季度提醒(第一季度、第三季度)
            var q1End = DateTime.Parse(string.Format("{0}-4-2", now.Year));
            var q1Start = q1End.AddDays(-14);
            var q3End = DateTime.Parse(string.Format("{0}-10-2", now.Year));
            var q3Start = q3End.AddDays(-14);
            if ((now >= q1Start && now < q1End)|| (now >= q3Start && now < q3End))
                list.Add(WarningType.Quarter);

            //半年度提醒
            var halfYearEnd = DateTime.Parse(string.Format("{0}-7-2", now.Year));
            var halfYearStart = halfYearEnd.AddDays(-14);           
            if (now >= halfYearStart && now < halfYearEnd)
                list.Add(WarningType.HalfYear);

            //年度提醒
            var yearEnd = DateTime.Parse(now.ToString("yyyy-1-2"));
            if (day >= 2)//2号以后取下一年作为结束时间
                yearEnd = yearEnd.AddYears(1);
            var yearStart = yearEnd.AddDays(-14);
            if (now >= yearStart && now < yearEnd)
                list.Add(WarningType.Year);

            return list;
        }
        /// <summary>
        /// 构建消息内容
        /// </summary>
        /// <param name="warType">提醒类型</param>
        /// <param name="now">日期</param>
        /// <param name="orgName">电厂名称</param>
        /// <returns>消息内容（标题、内容、备注、月份或季度）</returns>
        private Tuple<string,string,string,int> GenMessageBody(WarningType warType,DateTime now,string orgName="")
        {
            Tuple<string, string, string,int> msg = null;
            var title = "EHS信息上报提醒";
            var content = "";
            var remark = "";
            var mark = 0;
            switch (warType)
            {
                case WarningType.Month:
                    if (now.Day == 1)
                    {//1号取上一个月
                        now = now.AddMonths(-1);
                    }
                    remark = now.ToString("yyyy年MM月份");
                    mark = now.Month;                    
                    content = string.Format("请及时上报{0}年{1}月份EHS信息", now.Year, mark);
                    break;
                case WarningType.Quarter:                    
                    int q = (int)Math.Ceiling(now.Month * 1.0 / 3);
                    remark = string.Format("{0}年{1}季度", now.Year, q);
                    mark = q;
                    if ((now.Month == 4 || now.Month == 10) && now.Day == 1)
                    {//1号取上一个季度
                        remark = string.Format("{0}年{1}季度", now.Year, (q - 1));
                        mark = q - 1;
                    }
                    content = string.Format("请及时上报{0}年第{1}季度EHS信息",now.Year,mark);
                    break;
                case WarningType.HalfYear:
                    remark = now.ToString("yyyy年上半年");
                    content = string.Format("请及时上报{0}年上半年EHS信息",now.Year);
                    break;
                case WarningType.Year:
                    if (now.Month == 1 && now.Day == 1)
                    {//1号取上一年度
                        now = now.AddYears(-1);
                    }
                    remark = now.ToString("yyyy年度");
                    mark = now.Year;                    
                    content = string.Format("请及时上报{0}年年度EHS信息",mark);
                    break;
            }
            remark += "【" + orgName + "】";//用备注作为判断重复短消息的条件。
            msg = new Tuple<string, string, string,int>(title, content, remark,mark);

            return msg;
        }
        /// <summary>
        /// 判断是否已经上报EHS信息
        /// </summary>
        /// <param name="orgCode">电厂单位编码</param>
        /// <param name="wType">提醒类型</param>
        /// <param name="mark">月份或季度</param>
        /// <returns>结果（true:有,false:无）</returns>
        private bool HasInfo(string orgCode,WarningType wType,int mark)
        {
            var r = false;

            var now = DateTime.Now;
            var sqlWhere = string.Format(" and createuserorgcode='{0}'", orgCode);
            if (wType == WarningType.Month)
                sqlWhere += string.Format(" and infotype='月报' and extract(year from starttime)='{0}' and extract(month from starttime)='{1}' ", now.Year, mark);

            if (wType == WarningType.Quarter)
            {
                DateTime qstart = DateTime.Parse(string.Format("{0}-{1}-1", now.Year, mark * 3 - 2));
                DateTime qEnd = qstart.AddMonths(3);
                sqlWhere += string.Format(" and infotype='季报' and starttime>=TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') and starttime<TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss') ", qstart.ToString("yyyy-MM-dd"),qEnd.ToString("yyyy-MM-dd"));
            }

            if (wType == WarningType.HalfYear)
                sqlWhere += string.Format(" and infotype='半年报' and extract(year from starttime)='{0}' ", now.Year);

            if (wType == WarningType.Year)
                sqlWhere += string.Format(" and infotype='年报' and extract(year from starttime)='{0}' ", now.Year);

            var countInfo = new InfoSubmitBLL().GetList(sqlWhere).Count();
            r = countInfo > 0;

            return r;

        }
        /// <summary>
        /// 判断是否已经发送短消息，避免重复发送相同的短消息。
        /// </summary>
        /// <param name="msgRemark">重复消息条件</param>
        /// <returns>结果（true:有，false：无）</returns>
        private bool ExistMsg(string msgRemark)
        {
            var r = false;

            var count = new MessageBLL().GetList(string.Format(" and Remark='{0}'", msgRemark)).Count();
            r = count > 0;

            return r;
        }
        /// <summary>
        /// 构建消息实体
        /// </summary>
        /// <param name="msgBody">消息内容（标题、内容、备注、月份或季度）</param>
        /// <param name="ehsDeptCode">EHS部门编码</param>
        /// <returns>短消息实体</returns>
        private MessageEntity GenEntity(Tuple<string, string, string,int> msgBody,string ehsDeptCode)
        {
            MessageEntity msg = null;           
            var aList = new UserBLL().GetListForCon(x => x.DepartmentCode.Contains(ehsDeptCode));
            msg = new MessageEntity()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = string.Join(",", aList.Take(10).Select(x => x.Account)),
                UserName = string.Join(",", aList.Take(10).Select(x => x.RealName)),
                SendTime = DateTime.Now,
                SendUser = "System",
                SendUserName = "系统管理员",
                Title = msgBody.Item1,
                Content = msgBody.Item2,
                Remark = msgBody.Item3,
                Category = "其它"
            };

            return msg;
        }
        #endregion
    }
    /// <summary>
    /// 短消息提醒类型
    /// </summary>
    public enum WarningType
    {
        /// <summary>
        /// 月报
        /// </summary>
        Month,
        /// <summary>
        /// 季报
        /// </summary>
        Quarter,
        /// <summary>
        /// 半年报
        /// </summary>
        HalfYear,
        /// <summary>
        /// 年报
        /// </summary>
        Year
    }
}
