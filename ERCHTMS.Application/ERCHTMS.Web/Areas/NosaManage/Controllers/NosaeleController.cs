using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.NosaManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// �� ����Ԫ�ر�
    /// </summary>
    public class NosaeleController : MvcControllerBase
    {
        private NosaeleBLL nosaelebll = new NosaeleBLL();
        private UserBLL userbll = new UserBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private NosaareaBLL nosaareabll = new NosaareaBLL();

        #region ��ͼ����
        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// ѡ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)//EHS����Code
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)//EHS����Code
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region ��ȡ����      
        /// <summary>
        /// �Ƿ������ͬ��ŵ�Ԫ��
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <param name="No">���</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult ExistEleNo(string keyValue,string No)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}' and id<>'{2}'", user.OrganizeCode, No, keyValue)).ToList();
            var r = oldList.Count > 0;

            return Success("�Ѵ��ڸ�Ԫ��", r);
        }  
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();           
            var data = nosaelebll.GetList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nosaelebll.GetEntity(keyValue);
            //����ֵ
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
        }
        /// <summary>
        /// ��ȡԪ�����ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEleTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            var data = nosaelebll.GetList(where).OrderBy(t => t.NO).ToList();
            foreach (var item in data)
            {
                var hasChild = data.Where(x=>x.ParentId==item.ID).Count()>0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "No";
                tree.AttributeA = "DutyDepartId";
                tree.AttributeB = "DutyDepartName";
                tree.AttributeC = "DutyUserId";
                tree.AttributeD = "DutyUserName";
                tree.AttributeValue = item.NO;
                tree.AttributeValueA = item.DutyDepartId;
                tree.AttributeValueB = item.DutyDepartName;
                tree.AttributeValueC = item.DutyUserId;
                tree.AttributeValueD = item.DutyUserName;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("-1"));
        }

        /// <summary>
        /// ��ȡ�������ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAreaTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            var data = nosaareabll.GetList(where).OrderBy(t => t.NO).ToList();
            foreach (var item in data)
            {
                var hasChild =  false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = "";
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "No";
                tree.AttributeA = "DutyDepartId";
                tree.AttributeB = "DutyDepartName";
                tree.AttributeC = "DutyUserId";
                tree.AttributeD = "DutyUserName";
                tree.AttributeValue = item.NO;
                tree.AttributeValueA = item.DutyDepartId;
                tree.AttributeValueB = item.DutyDepartName;
                tree.AttributeValueC = item.DutyUserId;
                tree.AttributeValueD = item.DutyUserName;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(""));
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            nosaelebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, NosaeleEntity entity)
        {
            nosaelebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����Ԫ��
        /// <summary>
        /// ����������׼
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEle()
        {
            int error = 0;
            int sussceed = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }               
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);
                DataTable dt = ExcelHelper.ExcelImport(filePath);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, userbll, nosaelebll, out msg) == true)
                    {
                        var entity = GenEntity(vals, userbll, nosaelebll);
                        nosaelebll.SaveForm(entity.ID, entity);
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "��" + (i + 1) + "��" + msg + "<br/>";
                        error++;
                    }
                }
                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + sussceed + "����ʧ��" + error + "��";
                message += "<br/>" + falseMessage;
                //ɾ����ʱ�ļ�
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool IsNull(object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == "";
        }
        private bool Validate(int index, object[] vals,UserBLL userbll, NosaeleBLL nosaelebll, out string msg)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 6)
            {
                msg += "����ʽ����ȷ";
                r = false;
            }
            var obj = vals[1];
            if (IsNull(obj))
            {
                msg += "��Ԫ�ر�Ų���Ϊ��";
                r = false;
            }
            else
            {                
                var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, obj.ToString().Trim())).ToList();
                if (oldList.Count > 0)
                    msg += "��Ԫ�ر���Ѵ���";
            }
            obj = vals[2];
            if (IsNull(obj))
            {
                msg += "��Ԫ�����Ʋ���Ϊ��";
                r = false;
            }           
            obj = vals[3];
            if (IsNull(obj))
            {
                msg += "��Ԫ�����β��Ų���Ϊ��";
                r = false;
            }
          
            obj = vals[4];
            if (IsNull(obj))
            {
                msg += "��Ԫ�������˲���Ϊ��";
                r = false;
            }
            else if (!IsNull(vals[3]))
            {
                var entity = userbll.GetUserInfoByName(vals[3].ToString().Trim(), obj.ToString().Trim());
                if (entity == null)
                {
                    msg += "�����β����в�������Ӧ�ĸ������û�";
                    r = false;
                }
            }

            obj = vals[5];//�ϼ�Ԫ�ر��
            if (!IsNull(obj))
            {
                var list = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'",user.OrganizeCode, obj.ToString().Trim()));
                if (list.Count()==0)
                {
                    msg += "���ϼ�Ԫ�ر�Ų�����";
                    r = false;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg += "��";
                r = false;
            }

            return r;
        }
        private NosaeleEntity GenEntity(object[] vals,UserBLL userbll, NosaeleBLL nosaelebll)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            NosaeleEntity entity = new NosaeleEntity() { ID = Guid.NewGuid().ToString() };
            var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, vals[1].ToString().Trim())).ToList();
            if (oldList.Count > 0)
                entity = oldList[0];

            entity.NO = vals[1].ToString().Trim();        
            entity.Name = vals[2].ToString().Trim();        
            entity.DutyDepartName = vals[3].ToString().Trim();
            entity.DutyUserName = vals[4].ToString().Trim();
            var uEntity = userbll.GetUserInfoByName(entity.DutyDepartName, entity.DutyUserName);
            entity.DutyUserId = uEntity.UserId;
            entity.DutyDepartId = uEntity.DepartmentId;
            var obj = vals[5];//�ϼ�Ԫ�ر��
            if (!IsNull(obj))
            {
                var list = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'",user.OrganizeCode, obj.ToString().Trim())).ToList();
                entity.ParentId = list[0].ID;
                entity.ParentName = list[0].Name;
            }

            return entity;
        }
        #endregion
    }
}
