using System;
using System.Net;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage;
using System.Linq;
using BSFramework.Util.Attributes;
using ERCHTMS.Entity.Common;
using ERCHTMS.Busines.Desktop;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// �� ����ת����Ϣ��
    /// </summary>
    public class TransferController : MvcControllerBase
    {
        private TransferBLL transferbll = new TransferBLL();
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL deptbll = new DepartmentBLL();
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
        /// ת����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Transfering()
        {
            return View();
        }

        /// <summary>
        /// ת��ȷ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TransferConfim()
        {
            return View();
        }

        #endregion

        #region ��ȡ����

        /// <summary>
        /// ���ݵ�ǰ�û�����id
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetOrganizeId()
        {
            //�����ǰ�û��ǹ���Ա
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "0";
            }
            else
            {
                return OperatorProvider.Provider.Current().OrganizeId;
            }


        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public ActionResult GetTransferList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "TID";
            pagination.p_fields = "UserName,InDeptName,InPostName,InJobName,TransferTime,CreateUserName";//ע���˴�Ҫ�滻����Ҫ��ѯ����
            pagination.p_tablename = "BIS_TRANSFER";
            pagination.conditionJson = "1=1 and IsConfirm=1 ";
            pagination.sidx = "CREATEDATE";
            pagination.sord = "desc";

            var data = transferbll.GetPageListByProc(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = transferbll.GetList(queryJson);
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
            var data = transferbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// �����û�id��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetUserTraJson(string keyValue)
        {
            var data = transferbll.GetUsertraEntity(keyValue);
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
            transferbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TransferEntity entity)
        {
            Operator curuser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string uid = entity.UserId;//��ȡ����Ҫת�ڵ��û�id
            UserEntity ue = userBLL.GetEntity(uid);
            entity.InDeptId = ue.DepartmentId;
            entity.InDeptName = deptbll.GetEntity(ue.DepartmentId).FullName;
            entity.InDeptCode = ue.DepartmentCode;
            entity.InJobId = ue.PostId;
            entity.InJobName = ue.PostName;
            entity.InPostId = ue.DutyId;
            entity.InPostName = ue.DutyName;
            if (entity.InDeptId == entity.OutDeptId && entity.InPostId == entity.OutPostId)
            {
                return Error("ת��ʧ��,ת���λ���ָ�λ����Ϊͬһ����λ��");
            }
            transferbll.SaveForm(keyValue, entity);
            HikUpdateUserInfo(entity);
            if (entity.IsConfirm == 0)
            {
                SyZg(entity, true);
                DataItemDetailBLL di = new DataItemDetailBLL();
                string way = di.GetItemValue("WhatWay");
                DepartmentBLL departmentBLL = new DepartmentBLL();
                DepartmentEntity org = departmentBLL.GetEntity(ue.OrganizeId);
                if (org.IsTrain == 1)
                {
                    //�Խ�.net��ѵƽ̨
                    if (way == "0")
                    {

                    }
                    //�Խ�java��ѵƽ̨
                    if (way == "1")
                    {
                        var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        DepartmentEntity dept = departmentBLL.GetEntity(entity.OutDeptId);
                        if (dept != null)
                        {
                            string deptId = dept.DepartmentId;
                            string enCode = dept.EnCode;
                            if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                            {
                                string[] arr = dept.DeptKey.Split('|');
                                deptId = arr[0];
                                if (arr.Length > 1)
                                {
                                    enCode = arr[1];
                                }
                            }
                            Task.Factory.StartNew(() =>
                            {
                                object obj = new
                                {
                                    action = "edit",
                                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    userId = ue.UserId,
                                    userName = ue.RealName,
                                    account = ue.Account,
                                    deptId = deptId,
                                    deptCode = enCode,
                                    password = "", //Ϊnullʱ��Ҫ�޸�����!
                                    sex = ue.Gender,
                                    idCard = ue.IdentifyID,
                                    email = ue.Email,
                                    mobile = ue.Mobile,
                                    birth = ue.Birthday == null ? "" : ue.Birthday.Value.ToString("yyyy-MM-dd"),//����
                                    postId = entity.OutPostId,
                                    postName = entity.OutPostName,//��λ
                                    age = ue.Age,//����
                                    native = ue.Native, //����
                                    nation = ue.Nation, //����
                                    encode = ue.EnCode,//����
                                    jobTitle = entity.OutJobName,
                                    techLevel = ue.TechnicalGrade,
                                    workType = ue.Craft,
                                    companyId = org.InnerPhone,
                                    trainRoles = ue.TrainRoleId,
                                    role = ue.IsTrainAdmin == null ? 0 : ue.IsTrainAdmin //��ɫ��0:ѧԱ��1:��ѵ����Ա��
                                };
                                List<object> list = new List<object>();
                                list.Add(obj);
                                Busines.JPush.JPushApi.PushMessage(list, 1);

                                LogEntity logEntity = new LogEntity();
                                logEntity.CategoryId = 5;
                                logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                                logEntity.OperateType = "��Աת��";
                                logEntity.OperateAccount = user.Account + "��" + user.UserName + "��";
                                logEntity.OperateUserId = user.UserId;

                                logEntity.ExecuteResult = 1;
                                logEntity.ExecuteResultJson = string.Format("ͬ����Ա(ת��)��java��ѵƽ̨,ͬ����Ϣ:\r\n{0}", list.ToJson());
                                logEntity.Module = "��Ա����";
                                logEntity.ModuleId = "ea93dc6b-83fc-4ac2-a1b7-56ef6909445c";
                                logEntity.WriteLog();
                            });
                        }

                    }

                }
            }
            else
            {
                SyZg(entity, false);
                string GdhcUrl = new DataItemDetailBLL().GetItemValue("GdhcUrl");
                string pcrul = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/" + Request.ApplicationPath;
                string LogUrl = Server.MapPath("~/logs/RYZG");
                try
                {
                    
                    DesktopBLL deskBll = new DesktopBLL();
                    if (!string.IsNullOrWhiteSpace(GdhcUrl))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            string[] receiverList = userBLL.GetList().Where(t => ((t.RoleName.Contains("������") || t.RoleName.Contains("��˾�쵼") || t.RoleName.Contains("���������û�")) && entity.OutDeptCode.StartsWith(t.DepartmentCode)) || (entity.OutDeptCode.StartsWith(t.DepartmentCode) && entity.OutDeptCode != t.DepartmentCode) && t.Account != "System" && !string.IsNullOrWhiteSpace(t.EnCode)).Select(t => t.EnCode).ToArray();
                            if (receiverList.Count()>0)
                            {
                                if (!string.IsNullOrWhiteSpace(LogUrl))
                                {
                                    if (!System.IO.Directory.Exists(LogUrl))
                                    {
                                        System.IO.Directory.CreateDirectory(LogUrl);
                                    }
                                }
                                System.IO.File.AppendAllText(LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "׼�����ʹ��죺����*********" + string.Join(",", receiverList) + "********\r\n");
                            }
                            
                            for (int i = 0; i < receiverList.Length; i++)
                            {
                                if (!string.IsNullOrWhiteSpace(receiverList[i]))
                                {
                                    GdhcDbsxEntity DbEntity = new GdhcDbsxEntity();
                                    DbEntity.syscode = "GSI";
                                    DbEntity.flowid = entity.TID;
                                    DbEntity.requestname = ue.RealName + "ת������";
                                    DbEntity.workflowname = "RYZG";
                                    DbEntity.nodename = "��Աת������";
                                    DbEntity.pcurl = pcrul + "/Login/TransferSignIn?itemcode=TransferNum&account=" + BSFramework.Util.DESEncrypt.Encrypt(receiverList[i]);
                                    DbEntity.appurl = "";
                                    DbEntity.creator = curuser.Code;
                                    DbEntity.createdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    DbEntity.receiver = receiverList[i];
                                    DbEntity.receivedatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    DbEntity.requestlevel = "0";
                                    DbEntity.GdhcUrl = GdhcUrl;
                                    DbEntity.LogUrl = LogUrl;
                                    deskBll.GdhcDbsxSyncJS(DbEntity);
                                }
                            }

                        });
                    }
                }                                                                                             
                catch (Exception ex)
                {
                    if (!string.IsNullOrWhiteSpace(LogUrl))
                    {
                        if (!System.IO.Directory.Exists(LogUrl))
                        {
                            System.IO.Directory.CreateDirectory(LogUrl);
                        }
                    }
                    System.IO.File.AppendAllText(LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "�쳣��Ϣ��" + ex.ToString());
                }
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ת��ͬ��
        /// </summary>
        private void SyZg(TransferEntity entity, bool i)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            try
            {


                BzAppTransfer tr = new BzAppTransfer();
                tr.Id = entity.TID;
                tr.RoleDutyId = entity.OutPostId;
                tr.RoleDutyName = entity.OutPostName;
                tr.allocationtime = Convert.ToDateTime(entity.TransferTime).ToString("yyyy-MM-dd");
                tr.department = entity.OutDeptName;
                tr.departmentid = entity.OutDeptId;
                tr.iscomplete = i;
                tr.leaveremark = "";
                tr.leavetime = "";
                tr.oldRoleDutyName = entity.InPostName;
                tr.olddepartment = entity.InDeptName;
                tr.olddepartmentid = entity.InDeptId;
                tr.oldquarters = entity.InJobName;
                tr.quarters = entity.OutJobName;
                tr.quartersid = entity.OutJobId;
                tr.userId = entity.UserId;
                tr.username = entity.UserName;


                BzBase bs = new BzBase();
                bs.data = tr;
                bs.userId = OperatorProvider.Provider.Current().UserId;

                string Json = "json=" + bs.ToJson();
                string massge = HttpMethods.HttpPost(new DataItemDetailBLL().GetItemValue("yjbzUrl") + "/UserWorkAllocation/OperationEntity", Json);
                AppInTransfer appreturn = JsonConvert.DeserializeObject<AppInTransfer>(massge);
                if (appreturn.code == "0")
                {
                    if (i)
                    {
                        BzWc(entity);
                    }
                }
                ////����Ǵ�����״̬ ��ͬ���������Ǳ�
                //System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(bs));
                //wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                //wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("yjbzUrl") + "/UserWorkAllocation/OperationEntity"), nc);



            }
            catch (Exception e)
            {

            }
        }

        public void BzWc(TransferEntity entity)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                BxwcTransfer bt = new BxwcTransfer();
                bt.RoleDutyId = entity.InPostId;
                bt.RoleDutyName = entity.InPostName;
                bt.id = entity.TID;
                bt.quarters = entity.OutJobName;
                bt.quartersid = entity.OutJobId;
                BzBase bs = new BzBase();
                bs.data = bt;
                bs.userId = OperatorProvider.Provider.Current().UserId;
                //����Ǵ�����״̬ ��ͬ���������Ǳ�
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(bs));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("yjbzUrl") + "/UserWorkAllocation/updatetoerchtms"), nc);
            }
            catch (Exception e)
            {

            }

        }

        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                //��ͬ�����д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
            }
            catch (Exception ex)
            {

            }


        }

        /// <summary>
        /// ת��ȷ�ϲ���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Update(string keyValue, TransferEntity entity)
        {
            Operator curuser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            transferbll.Update(keyValue, entity);
            SyZg(entity, true);
            HikUpdateUserInfo(entity);
            string uid = entity.UserId;//��ȡ����Ҫת�ڵ��û�id
            UserEntity ue = userBLL.GetEntity(uid);
            string GdhcUrl = new DataItemDetailBLL().GetItemValue("GdhcUrl");
            string pcrul = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/" + Request.ApplicationPath;
            string LogUrl = Server.MapPath("~/logs/RYZG");
            try
            {
                DesktopBLL deskBll = new DesktopBLL();
                string[] receiverList = userBLL.GetList().Where(t => ((t.RoleName.Contains("������") || t.RoleName.Contains("��˾�쵼") || t.RoleName.Contains("���������û�")) && entity.OutDeptCode.StartsWith(t.DepartmentCode)) || (entity.OutDeptCode.StartsWith(t.DepartmentCode) && entity.OutDeptCode != t.DepartmentCode) && t.Account != "System" && !string.IsNullOrWhiteSpace(t.EnCode)).Select(t => t.EnCode).ToArray();
                if (!string.IsNullOrWhiteSpace(GdhcUrl))
                {
                    Task.Factory.StartNew(() =>
                    {
                        GdhcDbsxEntity DbEntity = new GdhcDbsxEntity();
                        DbEntity.syscode = "GSI";
                        DbEntity.flowid = entity.TID;
                        DbEntity.requestname = ue.RealName + "ת������";
                        DbEntity.workflowname = "RYZG";
                        DbEntity.nodename = "��Աת���������";
                        DbEntity.receiver = string.Join(",", receiverList);
                        DbEntity.requestlevel = "0";
                        DbEntity.GdhcUrl = GdhcUrl;
                        DbEntity.LogUrl = LogUrl;
                        deskBll.GdhcDbsxSyncYB(DbEntity);
                        DbEntity.receiver = string.Join(",", receiverList);
                        DbEntity.nodename = "��Աת���������";
                        deskBll.GdhcDbsxSyncBJ(DbEntity);
                    });
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(LogUrl))
                {
                    if (!System.IO.Directory.Exists(LogUrl))
                    {
                        System.IO.Directory.CreateDirectory(LogUrl);
                    }
                }
                System.IO.File.AppendAllText(LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "�쳣��Ϣ��" + ex.ToString());
            }
            DataItemDetailBLL di = new DataItemDetailBLL();
            string way = di.GetItemValue("WhatWay");
            DepartmentBLL departmentBLL = new DepartmentBLL();
            DepartmentEntity org = departmentBLL.GetEntity(ue.OrganizeId);
            if (org.IsTrain == 1)
            {
                //�Խ�.net��ѵƽ̨
                if (way == "0")
                {

                }
                //�Խ�java��ѵƽ̨
                if (way == "1")
                {
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    DepartmentEntity dept = departmentBLL.GetEntity(entity.OutDeptId);
                    if (dept != null)
                    {
                        string deptId = dept.DepartmentId;
                        string enCode = dept.EnCode;
                        if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                        {
                            string[] arr = dept.DeptKey.Split('|');
                            deptId = arr[0];
                            if (arr.Length > 1)
                            {
                                enCode = arr[1];
                            }
                        }
                        Task.Factory.StartNew(() =>
                        {
                            object obj = new
                            {
                                action = "edit",
                                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                userId = ue.UserId,
                                userName = ue.RealName,
                                account = ue.Account,
                                deptId = deptId,
                                deptCode = enCode,
                                password = "", //Ϊnullʱ��Ҫ�޸�����!
                                sex = ue.Gender,
                                idCard = ue.IdentifyID,
                                email = ue.Email,
                                mobile = ue.Mobile,
                                birth = ue.Birthday == null ? "" : ue.Birthday.Value.ToString("yyyy-MM-dd"),//����
                                postId = entity.OutPostId,
                                postName = entity.OutPostName,//��λ
                                age = ue.Age,//����
                                native = ue.Native, //����
                                nation = ue.Nation, //����
                                encode = ue.EnCode,//����
                                jobTitle = entity.OutJobName,
                                techLevel = ue.TechnicalGrade,
                                workType = ue.Craft,
                                companyId = org.InnerPhone,
                                trainRoles = ue.TrainRoleId,
                                role = ue.IsTrainAdmin == null ? 0 : ue.IsTrainAdmin //��ɫ��0:ѧԱ��1:��ѵ����Ա��
                            };
                            List<object> list = new List<object>();
                            list.Add(obj);
                            Busines.JPush.JPushApi.PushMessage(list, 1);

                            LogEntity logEntity = new LogEntity();
                            logEntity.CategoryId = 5;
                            logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                            logEntity.OperateType = "��Աת��";
                            logEntity.OperateAccount = user.Account + "��" + user.UserName + "��";
                            logEntity.OperateUserId = user.UserId;

                            logEntity.ExecuteResult = 1;
                            logEntity.ExecuteResultJson = string.Format("ͬ����Ա(ת��)��java��ѵƽ̨,ͬ����Ϣ:\r\n{0}", list.ToJson());
                            logEntity.Module = "��Ա����";
                            logEntity.ModuleId = "ea93dc6b-83fc-4ac2-a1b7-56ef6909445c";
                            logEntity.WriteLog();
                        });
                    }
                }
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ͬ���޸ĺ�������ƽ̨��Ա��֯��Ϣ
        /// </summary>
        public void HikUpdateUserInfo(TransferEntity entity)
        {
            try
            {
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                string KMIndex = itemBll.GetItemValue("KMIndexUrl");
                string HikHttps = itemBll.GetItemValue("HikHttps");
                List<UserEntity> list = new List<UserEntity>();
                if (!string.IsNullOrEmpty(KMIndex) || !string.IsNullOrEmpty(HikHttps))
                {//ֻ����ִ�иò��������ŵ糧���ƽ𲺣�
                    if (entity != null && entity.IsConfirm == 0)
                    {
                        UserEntity ue = userBLL.GetEntity(entity.UserId);
                        ue.DepartmentId = entity.OutDeptId;
                        HikUpdateUserInfo(ue);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ͬ���޸ĺ�����Ա��Ϣ
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string HikUpdateUserInfo(UserEntity item)
        {
            DataItemDetailBLL data = new DataItemDetailBLL();
            var pitem = data.GetItemValue("Hikappkey");//������������Կ
            var baseurl = data.GetItemValue("HikBaseUrl");//������������ַ
            string HikHttps = data.GetItemValue("HikHttps");//����1.4�����ϰ汾https
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/single/update";
            int Gender = item.Gender == "��" ? 1 : 0;
            var model = new
            {
                personId = item.UserId,
                personName = item.RealName,
                orgIndexCode = item.DepartmentId,
                gender = Gender,
                phoneNo = item.Mobile,
                certificateType = "111",
                certificateNo = item.IdentifyID,
                jobNo = item.EnCode
            };
            string rtnMsg = string.Empty;
            if (!string.IsNullOrEmpty(HikHttps))
            {
                HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(model), 20);
                if (result != null)
                {
                    rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                }
            }
            else
            {
                rtnMsg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
            }
            return rtnMsg;
        }


        #endregion
    }
}
