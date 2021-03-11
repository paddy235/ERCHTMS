using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using ERCHTMS.Web.Areas.KbsDeviceManage.Models;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// �� ������ǩ����
    /// </summary>
    public class LablemanageController : MvcControllerBase
    {
        private LablemanageBLL lablemanagebll = new LablemanageBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

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
        public ActionResult Form(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.id = id;

            var user = OperatorProvider.Provider.Current();
            ViewBag.orgid = user.OrganizeId;

            var items = dataItemDetailBLL.GetListItems("��ǩ����");
            items = items.Where(x => x.Description.Contains("0"));
            ViewData["list"] = items.Select(x => new SelectListItem { Value = x.ItemValue, Text = x.ItemName });

            var model = new Models.LableModel { BindTime = DateTime.Now, Operator = user.UserName, Power = "100" };

            return View(model);
        }

        /// <summary>
        /// ���ύ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit(string id, Models.LableModel model)
        {
            var user = OperatorProvider.Provider.Current();
            model.LabelId = model.LabelId.PadLeft(6, '0');
            if (lablemanagebll.GetIsBind(model.LabelId))
            {
                return Json(new AjaxResult { type = ResultType.error, message = "��ǩ�Ѿ��󶨣�" });
            }
            if (!string.IsNullOrEmpty(model.UserId) && lablemanagebll.GetUserLable(model.UserId) != null)
            {
                return Json(new AjaxResult { type = ResultType.error, message = model.Name + "�Ѿ��󶨣�" });
            }
            var entity = new LablemanageEntity()
            {
                ID = Guid.NewGuid().ToString(),
                DeptId = model.DeptId,
                DeptCode = model.DeptCode,
                DeptName = model.DeptName,
                BindTime = model.BindTime,
                CreateDate = DateTime.Now,
                CreateUserDeptCode = user.DeptCode,
                CreateUserId = user.UserId,
                ModifyDate = DateTime.Now,
                ModifyUserId = user.DeptId,
                CreateUserOrgCode = user.OrganizeCode,
                IdCardOrDriver = model.IdCardOrDriver,
                IsBind = 1,
                LableId = model.LabelId,
                LableTypeName = model.LableTypeName,
                LableTypeId = model.LableTypeId,
                Name = model.Name,
                OperUserId = user.UserName,
                Phone = model.Phone,
                Power = "100%",
                Type = 0,
                State = "����",
                UserId = model.UserId
            };
            lablemanagebll.SaveForm(id, entity);
            if (string.IsNullOrEmpty(id))
            {
                //����ǩ��Ϣͬ������̨���������
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "LableEntity";
                sd.EntityString = JsonConvert.SerializeObject(entity);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            return Json(new AjaxResult { type = ResultType.success, message = "����ɹ���" });
        }


        /// <summary>
        /// ͳ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Static()
        {
            return View();
        }

        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            // string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var data = lablemanagebll.GetPageList(pagination, queryJson);
            int total = pagination.records / pagination.rows;
            if (pagination.records % pagination.rows != 0)
            {
                total += 1;
            }
            var jsonData = new
            {
                rows = data,
                total = total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };

            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// ��ȡ��ǩ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetCount()
        {
            return lablemanagebll.GetCount().ToString();
        }

        ///// <summary>
        ///// ��ȡ�б�
        ///// </summary>
        ///// <param name="queryJson">��ѯ����</param>
        ///// <returns>�����б�Json</returns>
        //[HttpGet]
        //public ActionResult GetListJson(string queryJson)
        //{
        //    var data = lablemanagebll.GetList(queryJson);
        //    return ToJsonResult(data);
        //}

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = lablemanagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ǩͳ��ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLableChart()
        {
            return lablemanagebll.GetLableChart();
        }

        /// <summary>
        /// ��ǩͳ�Ʊ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLableStatistics()
        {
            var dt = lablemanagebll.GetLableStatistics();
            List<LablemanageEntity> lblist = lablemanagebll.GetList("").Where(it => it.IsBind == 1).ToList(); ;
            DataItemBLL di = new DataItemBLL();
            //�Ȼ�ȡ���ֵ���
            DataItemEntity DataItems = di.GetEntityByCode("LabelType");

            DataItemDetailBLL did = new DataItemDetailBLL();
            //�����ֵ����ȡֵ
            List<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId).ToList();
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
            foreach (var item in didList)
            {
                KbsEntity kbs = new KbsEntity();
                kbs.Name = item.ItemName;
                int num = 0;
                int zxnum = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["labletypeid"].ToString() == item.ItemValue)
                    {
                        num = Convert.ToInt32(dt.Rows[i]["cou"]);
                        zxnum = lblist.Where(it => it.LableTypeId == item.ItemValue && it.State == "����").Count();
                        break;
                    }
                }
                kbs.Num = num;
                Znum += num;
                kbs.Num2 = zxnum;
                klist.Add(kbs);
            }
            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].Num / Znum;
                    Proportion = Proportion * 100;
                }
                klist[j].Proportion = Proportion.ToString("0") + "%";
            }
            return Content(klist.ToJson());
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
            lablemanagebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ����ǩ
        /// </summary>
        /// <param name="keyValue"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Untie(string keyValue)
        {
            lablemanagebll.Untie(keyValue);
            //���°󶨵ı�ǩ��Ϣͬ������̨���������
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            sd.DataName = "UntieLable";
            sd.EntityString = keyValue;
            rh.SendMessage(JsonConvert.SerializeObject(sd));
            return Success("���ɹ���");
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
        public ActionResult SaveForm(string keyValue, LablemanageEntity entity)
        {
            lablemanagebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
