using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Busines.SafetyLawManage;
using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// �� ������׼�ƶȷ����
    /// </summary>
    public class StdsysTypeController : MvcControllerBase
    {
        private StdsysTypeBLL stdsystypebll = new StdsysTypeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
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
        /// ��ȡ����ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            stdsystypebll.SynchroOrg();//���µ糧������Ϣ

            var root = departmentBLL.GetCompany(user.DeptId);
            var depts = departmentBLL.GetSubDepartments(root.DepartmentId, "ʡ��,����,����");

            var where = string.Format(" and CreateUserOrgCode in ('{0}')", string.Join("','", depts.Select(x => x.EnCode)));
            var data = stdsystypebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();

            var treeList = new List<TreeEntity>();

            foreach (var item in depts)
            {
                bool hasChild = depts.Where(x => x.ParentId == item.DepartmentId).Count() > 0 ? true : false || data.Where(x => x.CREATEUSERORGCODE == item.EnCode).Count() > 0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                //tree.AttributeA = "Scope";
                //tree.AttributeValueA = item.Scope;
                //tree.AttributeB = "Dept";
                //tree.AttributeValueB = item.Scope;

                treeList.Add(tree);
            }

            foreach (var item in data)
            {
                //bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = item.ParentId == "-1" ? depts.FirstOrDefault(x => x.EnCode == item.CREATEUSERORGCODE).DepartmentId : item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = data.Where(x => x.ParentId == tree.id).Count() > 0;
                tree.Attribute = "Code";
                tree.AttributeValue = item.Code;
                tree.AttributeA = "Scope";
                tree.AttributeValueA = item.Scope;
                tree.AttributeB = "Dept";
                tree.AttributeValueB = item.Scope;

                treeList.Add(tree);
            }

            return Content(treeList.TreeToJson(root.ParentId));
        }


        /// <summary>
        /// ��ȡ��ɫ����ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRoleTypeTreeJson(string keyword)
        {
            Operator user = OperatorProvider.Provider.Current();

            stdsystypebll.SynchroOrg();//���µ糧������Ϣ

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            if (!(user.IsSystem || user.RoleName.Contains("��˾����Ա") || user.RoleName.Contains("���������û�")))
            {
                where += string.Format(" and Scope like '{0}%'", user.DeptCode);
            }
            var hasKeyword = !string.IsNullOrWhiteSpace(keyword);
            if (hasKeyword)
            {
                where += string.Format(" and Name like '%{0}%'", keyword);
            }
            var data = stdsystypebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            foreach (var item in data)
            {
                bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                hasChild = hasKeyword ? false : hasChild;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = hasKeyword ? "-1" : item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.Code;
                tree.AttributeA = "Scope";
                tree.AttributeValueA = item.Scope;
                tree.AttributeB = "Dept";
                tree.AttributeValueB = item.Scope;

                treeList.Add(tree);
            }
            var parentId = hasKeyword ? "-1" : data[0].ParentId;

            return Content(treeList.TreeToJson(parentId));
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
            var data = stdsystypebll.GetList(pagination, queryJson);
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
            var data = stdsystypebll.GetEntity(keyValue);
            //����ֵ
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
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
            stdsystypebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StdsysTypeEntity entity)
        {
            entity.ParentId = !string.IsNullOrWhiteSpace(entity.ParentId) ? entity.ParentId : "-1";
            var parent = stdsystypebll.GetEntity(entity.ParentId);
            if (parent != null)
                entity.Scope = parent.Scope;

            if (string.IsNullOrWhiteSpace(entity.Scope) || entity.Scope == "05")
            {
                entity.Scope = ERCHTMS.Code.OperatorProvider.Provider.Current().DeptCode;
            }
            stdsystypebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
