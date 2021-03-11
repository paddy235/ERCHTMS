using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System;
using System.Data;
using BSFramework.Util.Offices;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ������ȫ�������ݱ�
    /// </summary>
    public class SafestandarditemController : MvcControllerBase
    {
        private SafestandarditemBLL safestandarditembll = new SafestandarditemBLL();
        private SafestandardBLL safestandardbll = new SafestandardBLL();

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

        /// <summary>
        /// ������ 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ItemForm()
        {
            return View();
        }

        /// <summary>
        /// ѡ���׼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectItems()
        {
            return View();
        }


        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            pagination.p_kid = "a.id";
            pagination.p_fields = "content,require,num,norm,stid,case when d.name is not null then d.name || '>' || c.name || '>' || b.name when e.name is not  null then e.name || '>' || d.name || '>' || c.name || '>' || b.name when f.name is not  null then f.name || '>' || e.name || '>' || d.name || '>' || c.name || '>' || b.name else c.name || '>' || b.name end name, b.name as dname," +
                "(case when d.PARENTID = '0' then d.name  when e.PARENTID = '0' then e.name when f.PARENTID = '0'   then f.name  when c.PARENTID = '0'   then c.name else b.name end ) as typename";
            pagination.p_tablename = "EPG_SAFESTANDARDITEM a left join EPG_SAFESTANDARD    b on a.stid=b.id left join EPG_SAFESTANDARD    c on b.parentid=c.id left join EPG_SAFESTANDARD    d on c.parentid=d.id left join EPG_SAFESTANDARD    e on d.parentid=e.id left join EPG_SAFESTANDARD    f on e.parentid=f.id";
            //if (queryJson.ToJObject()["enCode"].IsEmpty())
            pagination.conditionJson = "a.createuserorgcode='" + user.OrganizeCode + "'";
            //else
            //    pagination.conditionJson = " 1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = safestandarditembll.GetPageList(pagination, queryJson);
            //foreach (DataRow dr in data.Rows)
            //{
            //    dr["num"] = safestandarditembll.GetNumber(dr["id"].ToString());
            //}
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safestandarditembll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safestandarditembll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ����������֯���˵�
        /// </summary>
        /// <returns></returns>
       //[HttpGet]
        public ActionResult GetStandardTreeJson(string isCheck, string ids = "", string selIds = "", string tid = "")
        {
            //��ʱȡ���У������û�Ȩ�޿�����ɣ����ݽ�ͨ��Ȩ����������չʾ 
            Operator user = OperatorProvider.Provider.Current();
            selIds = "," + selIds.Trim(',') + ",";
            var treeList = new List<TreeEntity>();
            var where = string.Format(" and (CreateUserOrgCode='{0}' or Id='0')", user.OrganizeCode);
            var data = safestandardbll.GetList(where).OrderBy(t => t.ENCODE).ToList();
            //var data = safestandardbll.GetList(where).ToList();
            try
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(tid))
                {
                    SafestandardEntity hs = safestandardbll.GetEntity(tid);
                    if (hs != null)
                    {
                        DataTable dt = new ERCHTMS.Busines.BaseManage.DepartmentBLL().GetDataTable(string.Format("select id from EPG_SAFESTANDARD t where instr('{0}',encode)=1", hs.ENCODE));
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.AppendFormat("{0},", dr[0].ToString());
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    ids = ids.Trim(',');
                    ids = "," + ids + ",";
                }
                foreach (SafestandardEntity item in data)
                {
                    if (!selIds.Contains("," + item.ID + ","))
                    {
                        string hasChild = safestandardbll.IsHasChild(item.ID) ? "1" : "0";
                        TreeEntity tree = new TreeEntity();
                        tree.id = item.ID;
                        tree.text = item.NAME.Replace("\n", "").Replace("\\", "�v");
                        tree.value = item.ID;
                        tree.parentId = item.PARENTID;
                        tree.isexpand = (item.PARENTID == "-1") ? true : false;
                        tree.complete = true;
                        if (!string.IsNullOrWhiteSpace(ids))
                        {
                            tree.checkstate = ids.Contains("," + item.ID + ",") ? 1 : 0;
                        }
                        tree.hasChildren = hasChild == "1" ? true : false;
                        tree.Attribute = "Code";
                        tree.showcheck = string.IsNullOrEmpty(isCheck) ? false : true;
                        if (!string.IsNullOrEmpty(tid))
                        {
                            if (sb.ToString().Contains(item.ID))
                            {
                                tree.isexpand = true;
                            }
                        }
                        tree.AttributeValue = item.ENCODE + "|" + hasChild;
                        treeList.Add(tree);
                    }


                }
                return Content(treeList.TreeToJson("-1"));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// ��ȡ��ȫ��������ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetSafeStandardFormJson(string keyValue)
        {
            var data = safestandardbll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            safestandarditembll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafestandarditemEntity entity)
        {
            var st = safestandardbll.GetEntity(entity.STID);
            entity.STCODE = st.ENCODE;
            safestandarditembll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ���濼�˷����������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveStandardForm(string keyValue, SafestandardEntity entity)
        {
            safestandardbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ɾ�����˷���������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveStandardForm(string keyValue)
        {
            safestandardbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ����������׼��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "���밲ȫ���˱�׼��")]
        public string ImportData()
        {
            int error = 0;
            string message = "���ݵ���ɹ�";
            StringBuilder sb = new StringBuilder("�������λ�ã�<br />");
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                count = dt.Rows.Count - 1;
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    string one = dt.Rows[j][1].ToString();
                    string content = dt.Rows[j][6].ToString();
                    string require = dt.Rows[j][7].ToString();
                    if (string.IsNullOrEmpty(one) || string.IsNullOrEmpty(content))
                    {
                        sb.AppendFormat("��{0}�У�һ����׼����򿼺�����Ҫ��Ϊ�գ�<br />", j);
                        error++;
                        continue;
                    }
                    string two = dt.Rows[j][2].ToString();
                    string three = dt.Rows[j][3].ToString();
                    string four = dt.Rows[j][4].ToString();
                    string five = dt.Rows[j][5].ToString();

                    string norm = dt.Rows[j][8].ToString();
                    try
                    {
                        safestandardbll.Save(one, two, three, four, five, content, require, norm);
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("��{0}�У�{1}<br />", j, ex.Message);
                        error++;
                    }

                }
            }
            if (error > 0)
            {
                message = string.Format("����{0}�����ݣ�����ʧ��{1}�����ݣ�{2}", count, error, sb.ToString());
            }
            return message;
        }
        #endregion
    }
}
