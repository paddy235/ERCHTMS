using ERCHTMS.Entity.PublicInfoManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.PublicInfoManage
{
    /// <summary>
    /// 描 述：文件信息
    /// </summary>
    public interface IFileInfoService
    {
        #region 获取数据
        List<FileInfoEntity> GetFileList(string recId);
        /// <summary>
        /// 所有文件（夹）列表
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetList(string folderId, string userId, string fileName);
        /// <summary>
        /// 文档列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetDocumentList(string userId);
        /// <summary>
        /// 图片列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetImageList(string userId);

        /// <summary>
        /// 前五张图片列表
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetImageListByTop5Object(string folderId);

        IEnumerable<FileInfoEntity> GetApkListByObject(string folderId);
        /// <summary>
        /// 回收站文件（夹）列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetRecycledList(string userId);
        /// <summary>
        /// 我的文件（夹）共享列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetMyShareList(string userId);
        /// <summary>
        /// 他人文件（夹）共享列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetOthersShareList(string userId);

        /// <summary>
        /// 图片列表
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetImageListByObject(string folderId);
        /// <summary>
        /// 文件信息实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        FileInfoEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取记录相关联的附件数量
        /// </summary>
        /// <param name="recId">记录ID</param>
        /// <returns></returns>
        DataTable GetFiles(string recId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="recId">关联的业务记录Id</param>
        /// <param name="fileName">附件名称</param>
        /// <returns></returns>
        FileInfoEntity GetEntity(string recId, string fileName);
        #endregion

        #region 提交数据
        /// <summary>
        /// 还原文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RestoreFile(string keyValue);
        /// <summary>
        /// 删除文件信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 彻底删除文件信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        void ThoroughRemoveForm(string keyValue);
        /// <summary>
        /// 保存文件信息表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fileInfoEntity">文件信息实体</param>
        /// <returns></returns>
        void SaveForm(string keyValue, FileInfoEntity fileInfoEntity);
        /// <summary>
        /// 共享文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="IsShare">是否共享：1-共享 0取消共享</param>
        void ShareFile(string keyValue, int IsShare);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="keyIds"></param>
        /// <returns></returns>
        int DeleteFileForm(string keyIds);

        /// <summary>
        /// 删除文件(根据recId删除)
        /// </summary>
        /// <param name="recId"></param>
        int DeleteFileByRecId(string recId);
        /// <summary>
        ///删除附件信息及物理文件 
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        int DeleteFile(string recId, string fileName, string filePath);

        /// <summary>
        /// 通过folderId 获取对应的图片文件
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        IEnumerable<FileInfoEntity> GetImageListByRecid(string folderId);
        #endregion
    }
}
