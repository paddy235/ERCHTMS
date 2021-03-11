using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using System;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// �� ����ְҵ���������
    /// </summary>
    public class OccupationalstaffdetailController : MvcControllerBase
    {
        private OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();

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
        /// ѡ��ְҵ��
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// �鿴�����Ա
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "OCCDETAILID";
            pagination.p_fields = "USERNAME,USERNAMEPINYIN,INSPECTIONTIME,ISSICK,SICKTYPE,SICKTYPENAME,UNUSUALNOTE";//ע���˴�Ҫ�滻����Ҫ��ѯ����
            pagination.p_tablename = "BIS_OCCUPATIONALSTAFFDETAIL";
            pagination.conditionJson = "1=1";
            pagination.sidx = "INSPECTIONTIME";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson += " and " + where;
            }

            var data = occupationalstaffdetailbll.GetPageListByProc(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);

            //var data = occupationalstaffdetailbll.GetList(queryJson);
            //return ToJsonResult(data);
        }

        /// <summary>
        /// �����û�id��ѯ����¼
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserList(string UserId)
        {
            var data = occupationalstaffdetailbll.GetUserTable(UserId);
            return ToJsonResult(data);
        }

        /// <summary>
        /// �����û�id��ѯ����¼
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewGetUserList(string UserId)
        {
            var data = occupationalstaffdetailbll.NewGetUserTable(UserId);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡ�Ƿ��ǳ��쵼 ���������õ�Ȩ��
        /// </summary>
        /// <returns></returns>
        public string GetPer()
        {

            return occupationalstaffdetailbll.IsPer().ToString();
        }

        /// <summary>
        /// ��ȡ�û��ĽӴ�Σ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserHazardfactor(string UserId)
        {
            UserEntity user = new UserBLL().GetEntity(UserId);
            DataTable data = new DataTable();
            //û��Ȩ������ʾ������
            if (occupationalstaffdetailbll.IsPer() || UserId == OperatorProvider.Provider.Current().UserId || (OperatorProvider.Provider.Current().RoleName.Contains("������") && OperatorProvider.Provider.Current().RoleName.Contains("���ż��û�")))
            {
                data = occupationalstaffdetailbll.GetUserHazardfactor(user.Account);
            }

            return ToJsonResult(data);
        }

        public ActionResult GetList(string Pid, int type)
        {
            var data = occupationalstaffdetailbll.GetList(Pid, type);
            return Content(data.ToJson());
        }

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = occupationalstaffdetailbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�ֵ�ֵ
        /// </summary>
        /// <param name="Code">�����ֵ�ֵ</param>
        /// <returns></returns>
        public ActionResult GetCmbJson(string Code)
        {
            DataItemBLL di = new DataItemBLL();
            //�Ȼ�ȡ���ֵ���
            DataItemEntity DataItems = di.GetEntityByCode(Code);

            DataItemDetailBLL did = new DataItemDetailBLL();
            //�����ֵ����ȡֵ
            IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

            return Content(didList.ToJson());
        }

        //[HttpGet]
        //public ActionResult GetTreeJson(string Code)
        //{
        //    DataItemBLL di = new DataItemBLL();
        //    //�Ȼ�ȡ���ֵ���
        //    DataItemEntity DataItems = di.GetEntityByCode(Code);

        //    DataItemDetailBLL did = new DataItemDetailBLL();
        //    //�����ֵ����ȡֵ
        //    IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);


        //    var treeList = new List<TreeEntity>();
        //    foreach (DataItemDetailEntity item in didList)
        //    {
        //        TreeEntity tree = new TreeEntity();
        //        bool hasChildren = didList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
        //        tree.id = item.DepartmentId;
        //        tree.text = item.FullName;
        //        tree.value = item.DepartmentId;
        //        tree.isexpand = true;
        //        tree.complete = true;
        //        tree.hasChildren = hasChildren;
        //        tree.parentId = item.ParentId;
        //        tree.Attribute = "Code";
        //        tree.AttributeValue = item.EnCode;
        //        tree.AttributeA = "IsOrg";
        //        tree.AttributeValueA = item.IsOrg.ToString();
        //        treeList.Add(tree);
        //    }
        //    return Content(treeList.TreeToJson(id));
        //}

        /// <summary>
        /// ���ݲ�ѯ������ȡְҵ������
        /// </summary>
        /// <param name="Code">ְҵ���ֵ�Code</param>
        /// <param name="deptIds">ҳ���������ְҵ��ids</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="checkMode">��ѡ���ѡ��0:��ѡ��1:��ѡ</param>
        /// <returns>��������Json</returns>
        [HttpGet]
        public ActionResult GetOccpuationalTreeJson(string Code, int checkMode = 0, int mode = 0, string deptIds = "0")
        {

            DataItemBLL di = new DataItemBLL();
            //�Ȼ�ȡ���ֵ���
            DataItemEntity DataItems = di.GetEntityByCode(Code);

            DataItemDetailBLL did = new DataItemDetailBLL();
            //�����ֵ����ȡֵ
            List<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId).OrderBy(t => t.ItemValue).ToList();


            string parentId = "0";
            OrganizeBLL orgBLL = new OrganizeBLL();
            var treeList = new List<TreeEntity>();

            //��ȡ���и��ڵ㼯��
            List<DataItemDetailEntity> parentList = didList.Where(it => it.ItemValue.Length == 3).ToList();

            //��ȡ�����ӽڵ�ڵ㼯��
            List<DataItemDetailEntity> SunList = didList.Where(it => it.ItemValue.Length > 3).ToList();

            foreach (DataItemDetailEntity oe in parentList)
            {
                treeList.Add(new TreeEntity
                {
                    id = oe.ItemValue,
                    text = oe.ItemName,
                    value = oe.ItemValue,
                    parentId = "0",
                    isexpand = true,
                    complete = true,
                    showcheck = false,
                    hasChildren = true,
                    Attribute = "Sort",
                    AttributeValue = "Parent",
                    AttributeA = "manager",
                    AttributeValueA = "" + "," + "" + ",1"
                });
            }

            foreach (DataItemDetailEntity item in SunList)
            {
                int chkState = 0;
                string[] arrids = deptIds.Split(',');
                if (arrids.Contains(item.ItemValue))
                {
                    chkState = 1;
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = false;
                tree.id = item.ItemValue;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.isexpand = true;
                tree.complete = true;
                tree.checkstate = chkState;
                tree.showcheck = checkMode == 0 ? false : true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ItemValue.Substring(0, 3);
                tree.Attribute = "Sort";
                tree.AttributeValue = "Sun";
                tree.AttributeA = "manager";
                tree.AttributeValueA = "" + "," + "" + "," + "2";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// ������id��������
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        public ActionResult GetUserJson(string UserIDs)
        {
            UserBLL ubll = new UserBLL();
            DataTable dt = ubll.GetUserTable(UserIDs.Split(','));
            //DataTable dt = new DataTable();
            return Content(dt.ToJson());
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
            occupationalstaffdetailbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OccupationalstaffdetailEntity entity)
        {
            //entity.Issick = 1;
            entity.InspectionTime = DateTime.Now;
            //������ת��Ϊƴ��
            entity.UserNamePinYin = Str.PinYin(entity.UserName);
            occupationalstaffdetailbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string column = "IDNUM,OCCDETAILID,USERNAME,USERNAMEPINYIN,INSPECTIONTIME,ISSICK,SICKTYPE,ISENDEMIC,ISUNUSUAL,UNUSUALNOTE";
            string stringcolumn = "ISSICK,SICKTYPE";
            string[] columns = column.Split(',');
            string[] stringcolumns = stringcolumn.Split(',');

            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            DataTable dt = occupationalstaffdetailbll.GetTable(queryJson, wheresql);
            DataTable Newdt = AsposeExcelHelper.UpdateDataTable(dt, columns, stringcolumns);//�������ֶ�ת����string �����޸�

            //�Ȼ�ȡְҵ������
            DataItemBLL di = new DataItemBLL();
            //�Ȼ�ȡ���ֵ���
            DataItemEntity DataItems = di.GetEntityByCode("SICKTYPE");
            DataItemDetailBLL did = new DataItemDetailBLL();
            //�����ֵ����ȡֵ
            IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

            //ѭ���޸���Ϣ
            for (int i = 0; i < Newdt.Rows.Count; i++)
            {
                Newdt.Rows[i]["IDNUM"] = i + 1;
                Newdt.Rows[i]["INSPECTIONTIME"] = Convert.ToDateTime(Newdt.Rows[i]["INSPECTIONTIME"]).ToString("yyyy-MM-dd");
                if (Convert.ToInt32(Newdt.Rows[i]["ISSICK"]) == 1)
                {
                    Newdt.Rows[i]["ISSICK"] = "��";
                    foreach (DataItemDetailEntity item in didList)
                    {
                        if (item.ItemValue == Newdt.Rows[i]["SICKTYPE"].ToString())
                        {
                            Newdt.Rows[i]["SICKTYPE"] = item.ItemName;
                        }
                    }
                }
                else
                {
                    Newdt.Rows[i]["ISSICK"] = "��";
                    Newdt.Rows[i]["SICKTYPE"] = "";
                }
            }

            string FileUrl = "";

            var queryParam = queryJson.ToJObject();
            string keyord = queryParam["keyword"].ToString();
            if (keyord.ToInt() == 1) //��ѯְҵ��
            {
                FileUrl = @"\Resource\ExcelTemplate\ְҵ����ְҵ����Ա�б�_����ģ��.xlsx";
                AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "ְҵ����Ա�б�", "ְҵ����Ա�б�");
            }
            else if (keyord.ToInt() == 2)  //��ѯ�쳣��Ա
            {
                FileUrl = @"\Resource\ExcelTemplate\ְҵ�����쳣��Ա�б�_����ģ��.xlsx";
                AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "�쳣��Ա�б�", "�쳣��Ա�б�");
            }
            else if (keyord.ToInt() == 3)  //��ѯ�����Ա
            {
                FileUrl = @"\Resource\ExcelTemplate\ְҵ���������Ա�б�_����ģ��.xlsx";
                AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "�����Ա�б�", "�����Ա�б�");
            }
            

           

            return Success("�����ɹ���");
        }
    }
}
