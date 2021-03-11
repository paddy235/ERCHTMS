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
    /// �� ������ϵ�����ļ��嵥
    /// </summary>
    public class FileListBLL
    {
        private FileListIService service = new FileListService();

        #region ��������
        private DataTable GenerateTemplat(string filetype)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tmpltid");
            dt.Columns.Add("tmpltname");
            dt.Columns.Add("tmpltpath");
            if (filetype == "0")
            {
                dt.LoadDataRow(new object[] { 1, "˫��Ԥ����ϵ�����쵼����", "~/Resource/Temp/FileList/01/1.˫��Ԥ����ϵ�����쵼����.docx" }, false);
                dt.LoadDataRow(new object[] { 2, "˫��Ԥ����ϵ����ʵʩ����", "~/Resource/Temp/FileList/01/2.˫��Ԥ����ϵ����ʵʩ����.docx" }, false);
                dt.LoadDataRow(new object[] { 3, "˫��Ԥ����ϵ��ѵ�ƻ�", "~/Resource/Temp/FileList/01/3.˫��Ԥ����ϵ��ѵ�ƻ�.doc" }, false);
                dt.LoadDataRow(new object[] { 4, "˫��Ԥ����ϵ��ѵ��¼", "~/Resource/Temp/FileList/01/4.˫��Ԥ����ϵ��ѵ��¼.docx" }, false);
                dt.LoadDataRow(new object[] { 5, "��ȫ���������ƣ�˫��ְ��", "~/Resource/Temp/FileList/01/5.��ȫ���������ƣ�˫��ְ��.docx" }, false);
                dt.LoadDataRow(new object[] { 6, "��ȫ���շּ��ܿ��ƶ�", "~/Resource/Temp/FileList/01/6.��ȫ���շּ��ܿ��ƶ�.doc" }, false);
                dt.LoadDataRow(new object[] { 7, "��ȫ��鼰�¹������Ų������ƶ�", "~/Resource/Temp/FileList/01/7.��ȫ��鼰�¹������Ų������ƶ�.docx" }, false);
                dt.LoadDataRow(new object[] { 8, "�ڲ���ͨ���ⲿ��ϵ�ƶ�", "~/Resource/Temp/FileList/01/8.�ڲ���ͨ���ⲿ��ϵ�ƶ�.docx" }, false);
                dt.LoadDataRow(new object[] { 9, "˫��Ԥ����ϵ���п����ƶ�", "~/Resource/Temp/FileList/01/9.˫��Ԥ����ϵ���п����ƶ�.docx" }, false);
            }
            else if (filetype == "1")
            {
                dt.LoadDataRow(new object[] { 11, "��ҵ��嵥�����յ㣩", "~/Resource/Temp/FileList/02/1.��ҵ��嵥�����յ㣩.docx" }, false);
                dt.LoadDataRow(new object[] { 12, "�豸��ʩ�嵥�����յ㣩", "~/Resource/Temp/FileList/02/2.�豸��ʩ�嵥�����յ㣩.docx" }, false);
                dt.LoadDataRow(new object[] { 13, "��ҵ�����嵥�����յ㣩", "~/Resource/Temp/FileList/02/3.��ҵ�����嵥�����յ㣩.docx" }, false);
                dt.LoadDataRow(new object[] { 14, "���յ�̨��", "~/Resource/Temp/FileList/02/4.���յ�̨��.docx" }, false);
                dt.LoadDataRow(new object[] { 15, "��ҵ���ȫ���շ������ۼ�¼����ҵΣ��������", "~/Resource/Temp/FileList/02/5.��ҵ���ȫ���շ������ۼ�¼����ҵΣ��������.docx" }, false);
                dt.LoadDataRow(new object[] { 16, "�豸��ʩ��ȫ���շ������ۼ�¼����ȫ���������", "~/Resource/Temp/FileList/02/6.�豸��ʩ��ȫ���շ������ۼ�¼����ȫ���������.docx" }, false);
                dt.LoadDataRow(new object[] { 17, "���շּ��ܿ��嵥�����", "~/Resource/Temp/FileList/02/7.���շּ��ܿ��嵥�����.docx" }, false);
                dt.LoadDataRow(new object[] { 18, "��λ���չܿ�Ӧ֪Ӧ�Ῠ", "~/Resource/Temp/FileList/02/8.��λ���չܿ�Ӧ֪Ӧ�Ῠ.doc" }, false);
                dt.LoadDataRow(new object[] { 19, "��λ�¹�Ӧ�����ÿ�", "~/Resource/Temp/FileList/02/9.��λ�¹�Ӧ�����ÿ�.doc" }, false);
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

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<FileListEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
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
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FileListEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
                        e.ConstructionStage = "���տ⽨��";
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
