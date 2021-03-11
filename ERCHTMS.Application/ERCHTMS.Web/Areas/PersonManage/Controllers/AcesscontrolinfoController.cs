using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Entity.CarManage;
using System.Web;
using System.IO;
using ERCHTMS.Busines.SystemManage;
using System;
using ERCHTMS.Code;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// �� �����û��Ž�������
    /// </summary>
    public class AcesscontrolinfoController : MvcControllerBase
    {
        private AcesscontrolinfoBLL acesscontrolinfobll = new AcesscontrolinfoBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
        private TemporaryGroupsBLL Tempbll = new TemporaryGroupsBLL();
        private UserBLL userBLL = new UserBLL();
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

        /// <summary>
        /// ��Ա�ݷ�¼��������ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CarUserForm()
        {
            return View();
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportFace()
        {
            return View();
        }


        #endregion

        #region ��ȡ����

        /// <summary>
        /// ��ȡѡ���û���������ָ������
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetUserList(string userids)
        {
            var data = acesscontrolinfobll.GetUserList(userids);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�ݷ���Ա������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetCarUserList(string userids, string type = "0")
        {
            string[] users = userids.Split(',');
            string userid = "";
            for (int i = 0; i < users.Length; i++)
            {
                if (i == 0)
                {
                    userid = "'" + users[i] + "'";
                }
                else
                {
                    userid += ",'" + users[i] + "'";
                }
                #region ��ʱ��Ա�ж�
                if (type == "1")
                {
                    var tempentity = new TemporaryGroupsBLL().GetEmptyUserEntity(users[i]);
                    if (tempentity == null)
                    {
                        var Us = userBLL.GetEntity(users[i]);
                        if (Us != null)
                        {
                            List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                            //�������������ʱ�б�������һ������
                            TemporaryUserEntity inserttuser = new TemporaryUserEntity();
                            inserttuser.Tel = Us.Account;
                            inserttuser.ComName = "";
                            inserttuser.CreateDate = Us.CreateDate;
                            inserttuser.CreateUserId = Us.CreateUserId;
                            inserttuser.USERID = Us.UserId;
                            inserttuser.Gender = Us.Gender;
                            inserttuser.ISDebar = 0;
                            inserttuser.Istemporary = 0;
                            inserttuser.Identifyid = Us.IdentifyID;
                            inserttuser.Postname = Us.DutyName;
                            inserttuser.UserName = Us.RealName;
                            inserttuser.Groupsid = Us.DepartmentId;
                            inserttuser.startTime = Us.CreateDate;
                            var dept1 = departmentBLL.GetEntity(Us.DepartmentId);
                            if (dept1 != null)
                            {
                                inserttuser.GroupsName = dept1.FullName;
                            }
                            list.Add(inserttuser);
                            new TemporaryGroupsBLL().SaveTemporaryList("", list);
                        }
                    }
                }
                #endregion
            }
            string sql = string.Empty;
            if (type == "1")
            {
                sql = string.Format(@"  select d.userid as id, realname as username from v_userinfo d   where d.userid in ({0})", userid);
            }
            else
            {
                sql = string.Format(@" select d.id,d.userimg,d.username,d.baseid,d.imgdata from bis_usercarfileimg d   where d.id in ({0})", userid);
            }

            var data = Opertickebll.GetDataTable(sql);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = acesscontrolinfobll.GetList(queryJson);
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
            var data = acesscontrolinfobll.GetEntity(keyValue);
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
            acesscontrolinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, string userid, string imgData, int type)
        {
            AcesscontrolinfoEntity ac = new AcesscontrolinfoEntity();
            ac.TID = keyValue;
            ac.UserId = userid;
            ac.Face = imgData;
            if (type == 1)
            {
                ac.IsFace = 1;
            }
            else
            {
                ac.IsFinger = 1;
            }
            acesscontrolinfobll.SaveForm(keyValue, ac);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��������ͷ¼��������Ƭ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFileImgForm(string keyValue, string imgData, string type, string NewType)
        {
            try
            {
                #region ��ȡ������ַ����Կ
                DataItemDetailBLL data = new DataItemDetailBLL();
                var pitem = data.GetItemValue("Hikappkey");//������������Կ
                var baseurl = data.GetItemValue("HikBaseUrl");//������������ַ
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                #endregion
                if (type == "1")
                {//�Ÿڹ���
                    List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                    var tempdate = Tempbll.GetEmptyUserEntity(keyValue);
                    if (tempdate != null)
                    {
                        var facedata = ImgToBase64(HttpUtility.UrlDecode(imgData));
                        //���ú���ƽ̨�ӿ���֤������Ƭ�Ƿ�ϸ�
                        int NewTypes = NewType == "" ? 0 : 1;
                        string msg = SocketHelper.FaceImgIsQualified(facedata.ImgData, baseurl, Key, Signature, NewTypes);
                        if (msg != null && !string.IsNullOrEmpty(msg))
                        {
                            FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                            if (!ress.data.checkResult)
                            {
                                return Content("false", "1");
                            }
                        }
                        if (string.IsNullOrEmpty(tempdate.UserImg))
                        {//δ��Ȩ
                            FacedataEntity face = new FacedataEntity();
                            List<FacedataEntity> FaceList = new List<FacedataEntity>();
                            face.UserId = tempdate.USERID;
                            face.ImgData = facedata.ImgData;
                            FaceList.Add(face);
                            SocketHelper.UploadFace(FaceList, baseurl, Key, Signature, NewTypes);
                            tempdate.UserImg = facedata.UserImg;
                            tempdate.ImgData = facedata.ImgData;
                            list.Add(tempdate);
                            Tempbll.SaveTemporaryList(tempdate.USERID, list);
                        }
                        else if (!string.IsNullOrEmpty(tempdate.PassPost))
                        {//����Ȩ
                            tempdate.UserImg = facedata.UserImg;
                            tempdate.ImgData = facedata.ImgData;
                            tempdate.Remark = "1";
                            if (NewTypes == 1)
                            {//�°汾https����
                                new PersonNewBLL().SaveUserFace(tempdate, keyValue, true);
                            }
                            else
                            {
                                Tempbll.SaveUserFace(tempdate, keyValue, true);
                            }

                        }
                        return Success("�����ɹ���");
                    }
                    else
                    {
                        return Success("1", "1");
                    }
                }
                else
                {//�ݷó���(����)
                    CarUserFileImgEntity entity = new CarUserFileImgEntity();
                    var facedata = ImgToBase64(HttpUtility.UrlDecode(imgData));
                    //���ú���ƽ̨�ӿ���֤������Ƭ�Ƿ�ϸ�
                    string msg = SocketHelper.FaceImgIsQualified(facedata.ImgData, baseurl, Key, Signature);
                    if (msg != null && !string.IsNullOrEmpty(msg))
                    {
                        FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                        if (!ress.data.checkResult)
                        {
                            return Content("false", "1");
                        }
                    }
                    entity.ID = keyValue;
                    entity.Userimg = facedata.UserImg;
                    entity.Imgdata = facedata.ImgData;
                    new CarUserBLL().SaveFileImgForm(entity);
                    return Success("�����ɹ���");
                }
            }
            catch (System.Exception er)
            {
                return Success(er.Message);
            }
        }


        public TemporaryUserEntity ImgToBase64(string imgData)
        {
            TemporaryUserEntity UserEntity = new TemporaryUserEntity();
            string fileurl = string.Empty;
            if (!string.IsNullOrEmpty(imgData))
            {//���յ���Ƭ
                UserEntity.ImgData = imgData;
                DataItemDetailBLL dd = new DataItemDetailBLL();
                string path = dd.GetItemValue("imgPath") + "\\Resource\\" + "TemporaryUser";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string sdd = imgData.Substring(0, imgData.IndexOf(',') + 1);
                string base64 = imgData.Replace(sdd, "");
                byte[] arr2 = Convert.FromBase64String(base64);
                using (MemoryStream ms2 = new MemoryStream(arr2))
                {//��png��ʽͼƬ�洢Ϊjpg��ʽ�ļ�

                    System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                    bmp2 = KiResizelmage(bmp2, 600, 450);
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    bmp2.Save(path + "\\" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fileurl = "\\Resource\\" + "TemporaryUser" + "\\" + fileName;
                }
                string srcPath = Server.MapPath("~" + fileurl);
                if (System.IO.File.Exists(srcPath))
                {
                    //��jpgͼƬתΪBase64String
                    var nbase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(srcPath));
                    UserEntity.ImgData = nbase64;
                }
                UserEntity.UserImg = fileurl;
            }
            return UserEntity;
        }

        /// <summary>
        /// ��������ͼƬ��С
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="newW"></param>
        /// <param name="newH"></param>
        /// <returns></returns>
        public Bitmap KiResizelmage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }


        #endregion


    }

}
