using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：分项指标表
    /// </summary>
    public class ClassificationController : MvcControllerBase
    {
        private ClassificationBLL classificationbll = new ClassificationBLL();
        private ClassificationIndexBLL classificationindexbll = new ClassificationIndexBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditIndex() 
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        //获取电厂
        [HttpGet]
        public ActionResult GetAllFactory() 
        {
            Operator curUser = OperatorProvider.Provider.Current();
            List<DepartmentEntity> dlist = new List<DepartmentEntity>();

            var dtDept = departmentBLL.GetAllFactory(curUser);
            foreach (DataRow row in dtDept.Rows)
            {
                DepartmentEntity entity = new DepartmentEntity();
                entity.DepartmentId = row["departmentid"].ToString();
                entity.EnCode = row["encode"].ToString();
                entity.FullName = row["fullname"].ToString();
                entity.DeptCode = row["deptcode"].ToString();
                entity.Manager = row["manager"].ToString();
                dlist.Add(entity);
            }

            return ToJsonResult(dlist);
        }

        #region 获取预警分项指标权重列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string orgId)
        {
            var curUser = new OperatorProvider().Current();
            if (string.IsNullOrEmpty(orgId)) 
            {
                orgId = curUser.OrganizeId;
            }
            var data = classificationbll.GetList(orgId);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取当前对应的预警设置值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWarningInfo()
        {
            var data = dataitemdetailbll.GetDataItemListByItemCode("'aqyj'").FirstOrDefault();
            return ToJsonResult(data);
        }



        /// <summary>
        /// 获取预警分项指标项目数据
        /// </summary>
        /// <param name="classificationcode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetClassificationIndexListJson(string classificationcode,string orgId)
        {
            string classificationId = string.Empty;
            var curUser = new OperatorProvider().Current();
            if (string.IsNullOrEmpty(orgId))
            {
                orgId = curUser.OrganizeId;
            }
            var data = classificationbll.GetList(orgId);

            var classificationEntity = data.Where(p => p.ClassificationCode == classificationcode).FirstOrDefault();
            if (null != classificationEntity)
            {
                classificationId = classificationEntity.Id;
            }
            var result = classificationindexbll.GetList(classificationId);
            return ToJsonResult(result);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = classificationbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交预警分值区间
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveWarningForm()
        {
            string keyValue = Request.Form["keyValue"].ToString();
            string s1 = Request.Form["s1"].ToString();
            string s2 = Request.Form["s2"].ToString();
            string s3 = Request.Form["s3"].ToString();
            string s4 = Request.Form["s4"].ToString();
            string description = s4 + "|" + s3 + "|" + s2 + "|" + s1;
            DataItemDetailEntity entity = dataitemdetailbll.GetEntity(keyValue);
            entity.ItemValue = description;
            dataitemdetailbll.SaveForm(keyValue, entity);
            return Success("保存成功!");
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            classificationbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        #endregion

        #region 预警分项指标权重设置
        /// <summary>
        /// 预警分项指标权重设置
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="WeightCoeffcient"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string WeightCoeffcient,string IsEnable)
        {
            ClassificationEntity entity = classificationbll.GetEntity(keyValue);
            entity.WeightCoeffcient = WeightCoeffcient;
            entity.CisEnable = IsEnable;
            classificationbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 预警分项指标项目设置
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult SaveClassificationForm(string keyValue, string IndexArgsValue, string IndexScore,string IsEnable)
        {
            ClassificationIndexEntity entity = classificationindexbll.GetEntity(keyValue);
            if (!string.IsNullOrEmpty(IndexArgsValue))
            {
                string[] array = IndexArgsValue.Split('|');
                int index = array.Length;
                string formatValue = string.Empty;
                switch (index)
                {
                    case 1:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0]);
                        break;
                    case 2:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1]);
                        break;
                    case 3:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2]);
                        break;
                    case 4:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2], array[3]);
                        break;
                    case 5:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2], array[3], array[4]);
                        break;
                    case 6:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2], array[3], array[4], array[5]);
                        break;
                    case 7:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2], array[3], array[4], array[5], array[6]);
                        break;
                    case 8:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
                        break;
                    case 9:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8]);
                        break;
                    case 10:
                        formatValue = string.Format(entity.IndexStandardFormat, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9]);
                        break;
                }
                entity.IndexStandard = formatValue;
            }
            entity.IndexArgsValue = IndexArgsValue;
            entity.IndexScore = IndexScore;
            entity.IsEnable = IsEnable;
            classificationindexbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 初始化权重及考核项目
        [HttpPost]
        [AjaxOnly]
        public ActionResult InitOrganizeData()
        {
            //组织机构ID
            string keyValue = Request.Form["keyValue"].ToString();

            classificationbll.AddClassificationList(keyValue);

            return Success("保存成功!");
        }
        #endregion

    }
}
