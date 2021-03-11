using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.NosaManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// �� ������ѵ�ļ�
    /// </summary>
    public class NosatrafilesController : MvcControllerBase
    {
        //private NosatrafilesBLL nosatrafilesbll = new NosatrafilesBLL();
        private NosatrafilesBsBLL nosatrafilesbll = new NosatrafilesBsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL();
        private NosatratypeBLL nosatratypebll = new NosatratypeBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

        #region ��ͼ����        
        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoFiles()
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosatrafilesbll.GetList(pagination, queryJson);
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
            var data = nosatrafilesbll.GetEntity(keyValue);
            if (data != null)
            {//���²��Ĵ���
                data.ViewTimes = !data.ViewTimes.HasValue ? 1 : data.ViewTimes.Value + 1;
                nosatrafilesbll.SaveForm(data.ID, data);
            }
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
            nosatrafilesbll.RemoveForm(keyValue);
            DeleteFiles(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosatrafilesEntity entity)
        {
            var isNew = nosatrafilesbll.GetEntity(keyValue) == null;
            nosatrafilesbll.SaveForm(keyValue, entity);
            if (isNew == true)
                SendMessage(entity);

            return Success("�����ɹ���");
        }
        private void SendMessage(NosatrafilesEntity entity)
        {
            if (!entity.MsgUserId.IsNullOrWhiteSpace())
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var aList = new UserBLL().GetListForCon(x => entity.MsgUserId.Contains(x.UserId)).Select(x => x.Account);
                MessageEntity msg = new MessageEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = string.Join(",",aList),
                    UserName = entity.MsgUserName,
                    SendTime = DateTime.Now,
                    SendUser = user.Account,
                    SendUserName = entity.CREATEUSERNAME,
                    Title = "�µ�NOSA��ѵ�ļ�����",
                    Content = string.Format("�����µ�NOSA��ѵ�ļ���{0}�����뼴ʱ���ġ�", entity.FileName),
                    Category= "����"
                };
                if (new MessageBLL().SaveForm("", msg))
                {
                    JPushApi.PublicMessage(msg);
                }
            }
        }
        #endregion

        #region ������ѵ�ļ�
        /// <summary>
        /// ������ѵ�ļ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportTraFiles()
        {
            int error = 0;
            int sussceed = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                KeyValuePair<string, string> result = decompress(out message);
                if (!string.IsNullOrWhiteSpace(message))
                    return message;

                DataTable dt = ExcelHelper.ExcelImport(result.Key);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, userbll, nosatratypebll,result.Value, out msg) == true)
                    {
                        KeyValuePair<NosatrafilesEntity,FileInfoEntity> kvEntity = GenEntity(vals, userbll, nosatratypebll,result.Value);
                        nosatrafilesbll.SaveForm(kvEntity.Key.ID, kvEntity.Key);
                        fileinfobll.SaveForm("", kvEntity.Value);
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
                var tempXlsfl = result.Key;
                if (System.IO.File.Exists(tempXlsfl))
                    System.IO.File.Delete(tempXlsfl);
                var tempZipfl = result.Key.Replace("xlsx", "zip").Replace("xls", "zip");
                if (System.IO.File.Exists(tempZipfl))
                    System.IO.File.Delete(tempZipfl);
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
        private KeyValuePair<string,string> decompress(out string message)
        {
            string fileName = "";
            string decompressionDirectory = Server.MapPath("~/Resource/NosaTraFiles/") + DateTime.Now.ToString("yyyyMMdd") + "\\";

            message = "";
            if (HttpContext.Request.Files.Count != 2)
            {
                message = "�밴��ȷ�ķ�ʽ���������ļ�.";
            }
            else
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                {
                    message = "��ѡ���ļ���ʽ��ȷ���ļ��ٵ���!";
                }
                string sufx1 = System.IO.Path.GetExtension(file.FileName);
                string sufx2 = System.IO.Path.GetExtension(file2.FileName);
                string sufx = sufx1 + sufx2;
                if (!(sufx.Contains(".zip") && (sufx.Contains(".xls") || sufx.Contains(".xlsx"))))
                {
                    message = "��ѡ���ļ���ʽ��ȷ���ļ��ٵ���!";
                }
                if (string.IsNullOrWhiteSpace(message))
                {
                    string fstr = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileName1 = fstr + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string fileName2 = fstr + System.IO.Path.GetExtension(file2.FileName);
                    file2.SaveAs(Server.MapPath("~/Resource/temp/" + fileName2));

                    if (sufx1.Contains("zip"))
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);
                        fileName = Server.MapPath("~/Resource/temp/" + fileName2);
                    }
                    else
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName2), decompressionDirectory, "", true);
                        fileName = Server.MapPath("~/Resource/temp/" + fileName1);
                    }
                }
            }

            return new KeyValuePair<string, string>(fileName, decompressionDirectory);
        }
        private void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();

            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);
                    if (fileName != "")
                    {
                        if ((System.IO.File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!System.IO.File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = System.IO.File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
        }
        private bool Validate(int index, object[] vals, UserBLL userbll, NosatratypeBLL nosatratypebll,string filePath, out string msg)
        {
            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 7)
            {
                msg += "����ʽ����ȷ";
                r = false;
            }
            var obj = vals[1];
            if (IsNull(obj))
            {
                msg += "���ļ�������Ϊ��";
                r = false;
            }
            obj = vals[2];
            if (IsNull(obj))
            {
                msg += "�������Ϊ��";
                r = false;
            }
            else
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = nosatratypebll.GetList(String.Format(" and createuserorgcode='{0}' and name='{1}'", user.OrganizeCode, obj.ToString().Trim())).ToList();
                if (list.Count() == 0)
                {
                    msg += "����𲻴���";
                    r = false;
                }
            }
            obj = vals[3];
            if (IsNull(obj))
            {
                msg += "��������λ�����ţ�����Ϊ��";
                r = false;
            }

            obj = vals[4];
            if (IsNull(obj))
            {
                msg += "�������˲���Ϊ��";
                r = false;
            }
            else if (!IsNull(vals[3]))
            {
                var entity = userbll.GetUserInfoByName(vals[3].ToString().Trim(), obj.ToString().Trim());
                if (entity == null)
                {
                    msg += "��������λ�����ţ��в�������Ӧ�ķ����û�";
                    r = false;
                }
            }

            obj = vals[5];
            if (IsNull(obj))
            {
                msg += "���������ڲ���Ϊ��";
                r = false;
            }
            else 
            {
                DateTime pubDate = new DateTime();
                if (!DateTime.TryParse(obj.ToString(),out pubDate))
                {
                    msg += "���������ڸ�ʽ��ȷ";
                    r = false;
                }
            }

            obj = vals[6];
            if (IsNull(obj))
            {
                msg += "���������Ʋ���Ϊ��";
                r = false;
            }
            else
            {
                string fn = System.IO.Path.Combine(filePath, obj.ToString().Trim());
                if (!System.IO.File.Exists(fn))
                {
                    msg += "���������Ʋ�����";
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
        private KeyValuePair<NosatrafilesEntity, FileInfoEntity> GenEntity(object[] vals, UserBLL userbll, NosatratypeBLL nosatratypebll,string filePath)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            NosatrafilesEntity entity = new NosatrafilesEntity() { ID = Guid.NewGuid().ToString() };
            entity.FileName = vals[1].ToString().Trim();
            object obj = vals[2].ToString().Trim();
            var list = nosatratypebll.GetList(String.Format(" and createuserorgcode='{0}' and name='{1}'", user.OrganizeCode, obj.ToString())).ToList();
            entity.RefId = list[0].ID;
            entity.RefName = list[0].Name;            
            entity.PubDepartName = vals[3].ToString().Trim();
            entity.PubUserName = vals[4].ToString().Trim();
            var uEntity = userbll.GetUserInfoByName(entity.PubDepartName, entity.PubUserName);
            entity.PubUserId = uEntity.UserId;
            entity.PubDepartId = uEntity.DepartmentId;
            entity.PubDate = DateTime.Parse(vals[5].ToString());
            obj = vals[6];

            string fn = System.IO.Path.Combine(filePath, obj.ToString().Trim());
            var fileinfo = new FileInfo(fn);
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            fileInfoEntity.RecId = entity.ID; //����ID
            fileInfoEntity.FolderId = "NosaTraFiles";
            fileInfoEntity.FileName = obj.ToString().Trim();
            fileInfoEntity.FilePath = string.Format("~/Resource/NosaTraFiles/{0}/{1}", DateTime.Now.ToString("yyyyMMdd"), obj.ToString().Trim());
            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
            fileInfoEntity.FileExtensions = fileinfo.Extension;
            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");

            return new KeyValuePair<NosatrafilesEntity, FileInfoEntity>(entity, fileInfoEntity);
        }
        #endregion
    }
}
