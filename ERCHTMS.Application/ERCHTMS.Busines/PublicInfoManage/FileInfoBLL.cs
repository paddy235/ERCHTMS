using BSFramework.Util.Extension;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.IService.PublicInfoManage;
using ERCHTMS.Service.PublicInfoManage;
using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ERCHTMS.Busines.PublicInfoManage
{
    /// <summary>
    /// 描 述：文件信息
    /// </summary>
    public class FileInfoBLL
    {
        private IFileInfoService service = new FileInfoService();

        #region 获取数据
        public List<FileInfoEntity> GetFileList(string recId)
        {
            return service.GetFileList(recId);
        }
        /// <summary>
        /// 所有文件（夹）列表
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetList(string folderId, string userId, string fileName)
        {
            return service.GetList(folderId, userId, fileName);
        }
        /// <summary>
        /// 文档列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetDocumentList(string userId)
        {
            return service.GetDocumentList(userId);
        }
        /// <summary>
        /// 图片列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetImageList(string userId)
        {
            return service.GetImageList(userId);
        }
        /// <summary>
        /// 回收站文件（夹）列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetRecycledList(string userId)
        {
            return service.GetRecycledList(userId);
        }
        /// <summary>
        /// 我的文件（夹）共享列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetMyShareList(string userId)
        {
            return service.GetMyShareList(userId);
        }
        /// <summary>
        /// 他人文件（夹）共享列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetOthersShareList(string userId)
        {
            return service.GetOthersShareList(userId);
        }

        /// <summary>
        /// 通过folderId 获取对应的图片文件
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetImageListByRecid(string folderId)
        {
            return service.GetImageListByRecid(folderId);
        }

        /// <summary>
        /// 获取图片列表
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetImageListByObject(string folderId)
        {
            return service.GetImageListByObject(folderId);
        }

        /// <summary>
        /// 获取apk文件列表
        /// </summary>
        public IEnumerable<FileInfoEntity> GetApkListByObject(string folderId)
        {
            return service.GetApkListByObject(folderId);
        }
        /// <summary>
        /// 前五张图片列表
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetImageListByTop5Object(string folderId)
        {
            return service.GetImageListByTop5Object(folderId);
        }
        /// <summary>
        /// 文件信息实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FileInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="recId">关联的业务记录Id</param>
        /// <param name="fileName">附件名称</param>
        /// <returns></returns>
        public FileInfoEntity GetEntity(string recId, string fileName)
        {
            return service.GetEntity(recId, fileName);
        }
        /// <summary>
        ///获取记录相关联的附件数量
        /// </summary>
        /// <param name="recId">记录ID</param>
        /// <returns></returns>
        public DataTable GetFiles(string recId)
        {
            return service.GetFiles(recId);
        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="zipedFile"></param>
        /// <param name="strDirectory"></param>
        /// <param name="password"></param>
        /// <param name="overWrite"></param>
        public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 还原文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RestoreFile(string keyValue)
        {
            try
            {
                service.RestoreFile(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 删除文件信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 彻底删除文件信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void ThoroughRemoveForm(string keyValue)
        {
            try
            {
                service.ThoroughRemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存文件信息表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fileInfoEntity">文件信息实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, FileInfoEntity fileInfoEntity)
        {
            try
            {
                service.SaveForm(keyValue, fileInfoEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 共享文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="IsShare">是否共享：1-共享 0取消共享</param>
        public void ShareFile(string keyValue, int IsShare = 1)
        {
            try
            {
                service.ShareFile(keyValue, IsShare);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="keyIds"></param>
        /// <returns></returns>
        public int DeleteFileForm(string keyIds)
        {
            try
            {
                return service.DeleteFileForm(keyIds);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 删除文件(根据recId删除)
        /// </summary>
        /// <param name="recId"></param>
        public int DeleteFileByRecId(string recId)
        {
            try
            {
                return service.DeleteFileByRecId(recId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///删除附件信息及物理文件 
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int DeleteFile(string recId, string fileName, string filePath)
        {
            return service.DeleteFile(recId, fileName, filePath);
        }
        #endregion
    }
}
