using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Data;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Text;
using System.IO;
using ERCHTMS.Code;
using System.Reflection;
using System.Drawing.Imaging;

namespace ERCHTMS.Web.Areas.PublicInfoManage.Controllers
{
    /// <summary>
    /// �� ����app�汾
    /// </summary>
    public class PackageController : MvcControllerBase
    {
        private PackageBLL packagebll = new PackageBLL();

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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CodeEncoder()
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
            pagination.p_kid = "ID";
            pagination.p_fields = "CreateDate,AppName,ReleaseVersion,PublishVersion,ReleaseDate";
            pagination.p_tablename = "bis_Package t";
            pagination.conditionJson = " 1=1";
            var watch = CommonHelper.TimerStart();
            var data = packagebll.GetPageList(pagination, queryJson);
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
            var data = packagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        #region ҳ�������ʼ��
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            string FileName = Guid.NewGuid().ToString();
            //����ֵ
            var josnData = new
            {
                FileName = FileName
            };

            return Content(josnData.ToJson());
        }
        #endregion
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
            packagebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, PackageEntity entity)
        {
            packagebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ���ɶ�ά��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Bulid(string keyValue)
        {
            //��ά��·���̶�
            string appName = GlobalUtil.GetApplicationPath;
            if (appName == "/" || appName == @"\") appName = "/ERCHTMS";
            string url = string.Format("http://{0}{1}{2}", HttpContext.Request.Url.Host, appName, "/Resource/AppFile");
            //�жϵ�ǰ��ά��·���治����
            string filePath = GlobalUtil.AppFile.LocalPath;
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

            }
            catch (Exception e)
            {
                return Error("·������");
            }

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeForegroundColor = Color.Black;
            Bitmap bmp = qrCodeEncoder.Encode(keyValue, Encoding.UTF8);//ָ��utf-8���룬 ֧������
            bmp.Save(Path.Combine(filePath, "Download.jpg"));

            return Success("���ɳɹ���");
        }
        
        /// <summary>
        /// ��ȡĬ��·��
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDefaltPath()
        {
            string appName = GlobalUtil.GetApplicationPath;
            if (appName == "/" || appName == @"\") appName = "/ERCHTMS";
            var url = string.Format("http://{0}{1}{2}", HttpContext.Request.Url.Host, appName, "/Resource/AppFile/˫�ر�.apk");
            return Content(url);
        }
        /// <summary>
        /// ��ȡ�汾��
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVersion()
        {
            var version = "";
            object[] att = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (att.Length > 0)
            {
                version = ((AssemblyFileVersionAttribute)att[0]).Version;
            }
            return Content(version);
        }
        #endregion
    }
}
