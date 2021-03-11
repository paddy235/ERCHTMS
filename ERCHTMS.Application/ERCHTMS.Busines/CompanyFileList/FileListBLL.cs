using ERCHTMS.Entity.FileListManage;
using ERCHTMS.IService.FileListManage;
using ERCHTMS.Service.FileListManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Linq;
using ERCHTMS.Busines.EnterPriseManage;

namespace ERCHTMS.Busines.FileListManage
{
    /// <summary>
    /// 描 述：体系建设文件清单
    /// </summary>
    public class FileListBLL
    {
        private FileListIService service = new FileListService();

        #region 辅助方法
        private DataTable GenerateTemplat(string filetype)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tmpltid");
            dt.Columns.Add("tmpltname");
            dt.Columns.Add("tmpltpath");
            if (filetype == "0")
            {
                dt.LoadDataRow(new object[] { 1, "双重预防体系建设领导机构", "~/Resource/Temp/FileList/01/1.双重预防体系建设领导机构.docx" }, false);
                dt.LoadDataRow(new object[] { 2, "双重预防体系建设实施方案", "~/Resource/Temp/FileList/01/2.双重预防体系建设实施方案.docx" }, false);
                dt.LoadDataRow(new object[] { 3, "双重预防体系培训计划", "~/Resource/Temp/FileList/01/3.双重预防体系培训计划.doc" }, false);
                dt.LoadDataRow(new object[] { 4, "双重预防体系培训记录", "~/Resource/Temp/FileList/01/4.双重预防体系培训记录.docx" }, false);
                dt.LoadDataRow(new object[] { 5, "安全生产责任制（双控职责）", "~/Resource/Temp/FileList/01/5.安全生产责任制（双控职责）.docx" }, false);
                dt.LoadDataRow(new object[] { 6, "安全风险分级管控制度", "~/Resource/Temp/FileList/01/6.安全风险分级管控制度.doc" }, false);
                dt.LoadDataRow(new object[] { 7, "安全检查及事故隐患排查治理制度", "~/Resource/Temp/FileList/01/7.安全检查及事故隐患排查治理制度.docx" }, false);
                dt.LoadDataRow(new object[] { 8, "内部沟通与外部联系制度", "~/Resource/Temp/FileList/01/8.内部沟通与外部联系制度.docx" }, false);
                dt.LoadDataRow(new object[] { 9, "双重预防体系运行考核制度", "~/Resource/Temp/FileList/01/9.双重预防体系运行考核制度.docx" }, false);
            }
            else if (filetype == "1")
            {
                dt.LoadDataRow(new object[] { 11, "作业活动清单（风险点）", "~/Resource/Temp/FileList/02/1.作业活动清单（风险点）.docx" }, false);
                dt.LoadDataRow(new object[] { 12, "设备设施清单（风险点）", "~/Resource/Temp/FileList/02/2.设备设施清单（风险点）.docx" }, false);
                dt.LoadDataRow(new object[] { 13, "作业场所清单（风险点）", "~/Resource/Temp/FileList/02/3.作业场所清单（风险点）.docx" }, false);
                dt.LoadDataRow(new object[] { 14, "风险点台账", "~/Resource/Temp/FileList/02/4.风险点台账.docx" }, false);
                dt.LoadDataRow(new object[] { 15, "作业活动安全风险分析评价记录（作业危害分析）", "~/Resource/Temp/FileList/02/5.作业活动安全风险分析评价记录（作业危害分析）.docx" }, false);
                dt.LoadDataRow(new object[] { 16, "设备设施安全风险分析评价记录（安全检查表分析）", "~/Resource/Temp/FileList/02/6.设备设施安全风险分析评价记录（安全检查表分析）.docx" }, false);
                dt.LoadDataRow(new object[] { 17, "风险分级管控清单评审表", "~/Resource/Temp/FileList/02/7.风险分级管控清单评审表.docx" }, false);
                dt.LoadDataRow(new object[] { 18, "岗位风险管控应知应会卡", "~/Resource/Temp/FileList/02/8.岗位风险管控应知应会卡.doc" }, false);
                dt.LoadDataRow(new object[] { 19, "岗位事故应急处置卡", "~/Resource/Temp/FileList/02/9.岗位事故应急处置卡.doc" }, false);
            }
            return dt;
        }
        private void MergeTemplatFiles(DataTable dtTmp,DataTable dtFiles)
        {
            if(dtTmp!=null && dtFiles != null)
            {
                for(var i=0;i< dtFiles.Columns.Count; i++)
                {
                    dtTmp.Columns.Add(dtFiles.Columns[i].ColumnName);
                }
                for(var i = 0; i < dtTmp.Rows.Count; i++)
                {
                    var r = dtTmp.Rows[i];
                    var rows = dtFiles.Select(string.Format("tmpltnum={0}", r["tmpltid"]));
                    if (rows.Length > 0)
                    {
                        var row = rows[0];
                        for (var j = 0; j < dtFiles.Columns.Count; j++)
                        {
                            var colName = dtFiles.Columns[j].ColumnName;
                            r[colName] = row[colName];
                        }
                    }
                    else
                    {
                        r["id"] = Guid.NewGuid().ToString();                        
                    }
                }
            }
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FileListEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            var dt = service.GetList(pagination, queryJson);
            var dtTmp = GenerateTemplat(queryJson.ToJObject()["filetype"].ToString());
            MergeTemplatFiles(dtTmp, dt);
            return dtTmp;
        }        
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FileListEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, FileListEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                var num = service.GetList(string.Format(" and comid='{0}'", entity.ComId)).Count();
                if (num == 18)
                {
                    EnterpriseBLL ebll = new EnterpriseBLL();
                    var e = ebll.GetEntity(entity.ComId);
                    if (e != null && e.ModifyDate.IsEmpty())
                    {
                        e.ConstructionStage = "风险库建设";
                        ebll.SaveForm(e.ID, e);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
