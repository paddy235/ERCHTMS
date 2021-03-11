using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.IService.PublicInfoManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ERCHTMS.Service.PublicInfoManage
{
    /// <summary>
    /// 描 述：文件信息
    /// </summary>
    public class FileInfoService : RepositoryFactory<FileInfoEntity>, IFileInfoService
    {
        #region 获取数据
        /// <summary>
        /// 所有文件（夹）列表
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetList(string folderId, string userId,string fileName)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    FolderId AS FileId ,
                                                ParentId AS FolderId ,
                                                FolderName AS FileName ,
                                                '' AS FileSize ,
                                                'folder' AS FileType ,
                                                CreateUserId,
                                                ModifyDate,
                                                IsShare,
                                                '' as  FilePath
                                      FROM      Base_FileFolder  where DeleteMark = 0
                                      UNION
                                      SELECT    FileId ,
                                                FolderId ,
                                                FileName ,
                                                FileSize ,
                                                FileType ,
                                                CreateUserId,
                                                ModifyDate,
                                                IsShare,
                                                FilePath
                                      FROM      Base_FileInfo where DeleteMark = 0
                                    ) t WHERE ");
            var parameter = new List<DbParameter>();
                strSql.Append(" CreateUserId = @userId");
                parameter.Add(DbParameters.CreateDbParameter("@userId", userId));
            if (!folderId.IsEmpty())
            {
                    strSql.Append(" AND FolderId = @folderId");
                    parameter.Add(DbParameters.CreateDbParameter("@folderId", folderId));
            }
            else
            {
                strSql.Append(" AND FolderId = '0'");
            }
            if (!fileName.IsEmpty())
            {
                strSql.Append(" AND FileName like @FileName");
                parameter.Add(DbParameters.CreateDbParameter("@FileName", "%"+fileName+"%"));
            }
            strSql.Append(" ORDER BY ModifyDate ASC");
            return this.BaseRepository().FindList(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 文档列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetDocumentList(string userId)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
                strSql.Append(@"SELECT  FileId ,
                                    FolderId ,
                                    FileName ,
                                    FileSize ,
                                    FileType ,
                                    CreateUserId ,
                                    ModifyDate,
                                    IsShare
                            FROM    Base_FileInfo
                            WHERE   DeleteMark = 0
                                    AND FileType IN ( 'log', 'txt', 'pdf', 'doc', 'docx', 'ppt', 'pptx',
                                                      'xls', 'xlsx' )
                                    AND CreateUserId = @userId");
               
                parameter.Add(DbParameters.CreateDbParameter("@userId", userId));
            strSql.Append(" ORDER BY ModifyDate ASC");
            return this.BaseRepository().FindList(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 图片列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetImageList(string userId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  FileId ,
                                    FolderId ,
                                    FileName ,
                                    FileSize ,
                                    FileType ,
                                    CreateUserId ,
                                    ModifyDate,
                                    IsShare
                            FROM    Base_FileInfo
                            WHERE   DeleteMark = 0
                                    AND FileType IN ( 'ico', 'gif', 'jpeg', 'jpg', 'png', 'psd' )
                                    AND CreateUserId = @userId");
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@userId", userId));
            strSql.Append(" ORDER BY ModifyDate ASC");
            return this.BaseRepository().FindList(strSql.ToString(), parameter.ToArray());
        }


        #region 获取前五张图片
        /// <summary>
        /// 获取前五张图片
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetImageListByTop5Object(string folderId)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(@"select a.* from (SELECT  FileId ,
                                    FolderId ,
                                    FileName ,
                                    FileSize ,
                                    FileType ,
                                    CreateUserId ,
                                    ModifyDate,
                                    IsShare,FilePath,rownum as rn
                            FROM    Base_FileInfo
                            WHERE   DeleteMark = 0
                                    AND FileType IN ( 'ico', 'gif', 'jpeg', 'jpg', 'png', 'psd', 'bmp' )
                                    AND folderId = '{0}'  ORDER BY ModifyDate desc ) a where a.rn <=5", folderId);
            return this.BaseRepository().FindList(strSql.ToString());
        }
        #endregion

        /// <summary>
        /// 通过folderId 获取对应的图片文件
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetImageListByObject(string folderId)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  FileId ,
                                    FolderId ,
                                    FileName ,
                                    FileSize ,
                                    FileType ,
                                    CreateUserId ,
                                    ModifyDate,
                                    IsShare,FilePath
                            FROM    Base_FileInfo
                            WHERE   DeleteMark = 0
                                    AND FileType IN ( 'ico', 'gif', 'jpeg', 'jpg', 'png', 'psd', 'bmp' )
                                    AND folderId = '{0}'  ORDER BY ModifyDate ASC", folderId);
            return this.BaseRepository().FindList(strSql.ToString());
        }


        /// <summary>
        /// 通过folderId 获取对应的apk文件
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetApkListByObject(string folderId)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  FileId ,
                                    FolderId ,
                                    FileName ,
                                    FileSize ,
                                    FileType ,
                                    CreateUserId ,
                                    ModifyDate,
                                    IsShare,FilePath
                            FROM    Base_FileInfo
                            WHERE   DeleteMark = 0
                                    AND FileType='apk'
                                    AND folderId = '{0}'  ORDER BY ModifyDate ASC", folderId);
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 回收站文件（夹）列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetRecycledList(string userId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    FolderId AS FileId ,
                                                ParentId AS FolderId ,
                                                FolderName AS FileName ,
                                                '' AS FileSize ,
                                                'folder' AS FileType ,
                                                CreateUserId,
                                                ModifyDate
                                      FROM      Base_FileFolder  where DeleteMark = 1
                                      UNION
                                      SELECT    FileId ,
                                                FolderId ,
                                                FileName ,
                                                FileSize ,
                                                FileType ,
                                                CreateUserId,
                                                ModifyDate
                                      FROM      Base_FileInfo where DeleteMark = 1
                                    ) t WHERE CreateUserId = @userId");
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@userId", userId));
            strSql.Append(" ORDER BY ModifyDate DESC");
            return this.BaseRepository().FindList(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 我的文件（夹）共享列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetMyShareList(string userId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    FolderId AS FileId ,
                                                ParentId AS FolderId ,
                                                FolderName AS FileName ,
                                                '' AS FileSize ,
                                                'folder' AS FileType ,
                                                CreateUserId,
                                                ModifyDate
                                      FROM      Base_FileFolder  WHERE DeleteMark = 0 AND IsShare = 1
                                      UNION
                                      SELECT    FileId ,
                                                FolderId ,
                                                FileName ,
                                                FileSize ,
                                                FileType ,
                                                CreateUserId,
                                                ModifyDate
                                      FROM      Base_FileInfo WHERE DeleteMark = 0 AND IsShare = 1
                                    ) t WHERE CreateUserId = @userId");
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@userId", userId));
            strSql.Append(" ORDER BY ModifyDate DESC");
            return this.BaseRepository().FindList(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 他人文件（夹）共享列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<FileInfoEntity> GetOthersShareList(string userId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    FolderId AS FileId ,
                                                ParentId AS FolderId ,
                                                FolderName AS FileName ,
                                                '' AS FileSize ,
                                                'folder' AS FileType ,
                                                CreateUserId,
                                                CreateUserName,
                                                ShareTime AS ModifyDate
                                      FROM      Base_FileFolder  WHERE DeleteMark = 0 AND IsShare = 1
                                      UNION
                                      SELECT    FileId ,
                                                FolderId ,
                                                FileName ,
                                                FileSize ,
                                                FileType ,
                                                CreateUserId,
                                                CreateUserName,
                                                ShareTime AS ModifyDate
                                      FROM      Base_FileInfo WHERE DeleteMark = 0 AND IsShare = 1
                                    ) t WHERE CreateUserId != @userId");
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@userId", userId));
            strSql.Append(" ORDER BY ModifyDate DESC");
            return this.BaseRepository().FindList(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 文件实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FileInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取记录相关联的附件数量
        /// </summary>
        /// <param name="recId">记录ID</param>
        /// <returns></returns>
        public DataTable GetFiles(string recId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT FileId,FileName  FROM  Base_FileInfo where DeleteMark = 0 ");
            var parameter = new List<DbParameter>();
            strSql.Append(" AND FolderId = @folderId");
            parameter.Add(DbParameters.CreateDbParameter("@folderId", recId));
            return this.BaseRepository().FindTable(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 还原文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RestoreFile(string keyValue)
        {
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            fileInfoEntity.Modify(keyValue);
            fileInfoEntity.DeleteMark = 0;
            this.BaseRepository().Update(fileInfoEntity);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            fileInfoEntity.Modify(keyValue);
            fileInfoEntity.DeleteMark = 1;
            this.BaseRepository().Update(fileInfoEntity);
        }
        /// <summary>
        /// 彻底删除文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void ThoroughRemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存文件表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fileInfoEntity">文件信息实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, FileInfoEntity fileInfoEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                fileInfoEntity.Modify(keyValue);
                this.BaseRepository().Update(fileInfoEntity);
            }
            else
            {
                fileInfoEntity.Create();
                this.BaseRepository().Insert(fileInfoEntity);
            }
        }
        /// <summary>
        /// 共享文件
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="IsShare">是否共享：1-共享 0取消共享</param>
        public void ShareFile(string keyValue, int IsShare)
        {
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            fileInfoEntity.FileId = keyValue;
            fileInfoEntity.IsShare = IsShare;
            fileInfoEntity.ShareTime = DateTime.Now;
            this.BaseRepository().Update(fileInfoEntity);
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="keyIds"></param>
        public int DeleteFileForm(string keyIds)
        {
            string sql = string.Format(@" delete base_fileinfo where fileid in ({0})", keyIds);

            return this.BaseRepository().ExecuteBySql(sql);
        }

        #endregion
    }
}
