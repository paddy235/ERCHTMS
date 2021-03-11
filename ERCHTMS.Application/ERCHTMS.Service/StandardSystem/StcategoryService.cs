using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System;
using System.Data;
using System.Text;
using BSFramework.Util;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// �� ������׼����
    /// </summary>
    public class StcategoryService : RepositoryFactory<StcategoryEntity>, StcategoryIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StcategoryEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StcategoryEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// �жϽڵ��������ӽڵ�����
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return this.BaseRepository().FindObject(string.Format("select count(1) from hrs_stcategory where parentid='{0}'", parentId)).ToInt() > 0 ? true : false;
        }
        public IEnumerable<StcategoryEntity> GetCategoryList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from HRS_STCATEGORY where parentid in(
                                                        select id from HRS_STCATEGORY t where typecode in(5,6) and parentid='0') order by encode asc");
            return this.BaseRepository().FindList(strSql.ToString());
        }
        public IEnumerable<StcategoryEntity> GetRankList(string Category)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from HRS_STCATEGORY where parentid='{0}'",Category);
            return this.BaseRepository().FindList(strSql.ToString());
        }

        public StcategoryEntity GetQueryEntity(string queryJson)
        {
            try
            {
                var expression = LinqExtensions.True<StcategoryEntity>();
                if (queryJson.Length > 0)
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["name"].IsEmpty())
                    {
                        string name = queryParam["name"].ToString();
                        expression = expression.And(t => t.NAME == name);
                    }
                    if (!queryParam["parentid"].IsEmpty())
                    {
                        string parentid = queryParam["parentid"].ToString();
                        expression = expression.And(t => t.PARENTID == parentid);
                    }

                    var list = this.BaseRepository().IQueryable(expression).ToList();
                    return list.FirstOrDefault();
                }
                else {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StcategoryEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    StcategoryEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.ID = keyValue;
                        var node = this.BaseRepository().FindEntity(entity.PARENTID);
                        string enCode = node == null ? "" : node.ENCODE;
                        object obj = BaseRepository().FindObject(string.Format("select max(encode) from hrs_stcategory where parentid='{0}'", entity.PARENTID));
                        if (obj == null || obj == DBNull.Value)
                        {
                            enCode += "001";
                        }
                        else
                        {
                            enCode += GetSno(int.Parse(obj.ToString().Substring(obj.ToString().Length - 3)) + 1, 3);
                        }
                        entity.ENCODE = enCode;
                        entity.Create();
                        this.BaseRepository().Insert(entity);
                    }
                    else
                    {
                        if (this.BaseRepository().FindEntity(keyValue).PARENTID != entity.PARENTID)
                        {
                            //IList<StcategoryEntity> childlist = this.BaseRepository().FindList("").Where(t => t.ENCODE.StartsWith(this.BaseRepository().FindEntity(keyValue).ENCODE) && t.TYPECODE == entity.TYPECODE).OrderBy(p => p.ENCODE).ToList();
                            DataTable dt = this.BaseRepository().FindTable(string.Format("select * from hrs_stcategory where encode like '{0}%' and typecode='{1}' order by encode asc", this.BaseRepository().FindEntity(keyValue).ENCODE, entity.TYPECODE));
                            StringBuilder sb = new StringBuilder();
                            foreach (DataRow dr in dt.Rows)
                            {
                                sb.AppendFormat("'{0}',", dr[0].ToString());
                            }
                            if (!sb.IsEmpty())
                            {
                                sb.Remove(sb.Length - 1, 1);
                            }
                            this.BaseRepository().ExecuteBySql(string.Format("update hrs_stcategory set encode='' where id in({0})", sb.ToString()));
                            foreach (DataRow item in dt.Rows)
                            {
                                var oldentity =  this.BaseRepository().FindEntity(item["id"]);
                                if (item["id"].ToString() == keyValue)
                                {
                                    oldentity.PARENTID = entity.PARENTID;
                                    oldentity.NAME = entity.NAME;
                                }
                                var node = this.BaseRepository().FindEntity(oldentity.PARENTID);
                                string enCode = node == null ? "" : node.ENCODE;
                                object obj = BaseRepository().FindObject(string.Format("select max(encode) from hrs_stcategory where parentid='{0}'", oldentity.PARENTID));
                                if (obj == null || obj == DBNull.Value)
                                {
                                    enCode += "001";
                                }
                                else
                                {
                                    enCode += GetSno(int.Parse(obj.ToString().Substring(obj.ToString().Length - 3)) + 1, 3);
                                }
                                oldentity.ENCODE = enCode;
                                oldentity.Modify(oldentity.ID);
                                this.BaseRepository().Update(oldentity);
                            }
                        }
                        else
                        {
                            entity.Modify(keyValue);
                            this.BaseRepository().Update(entity);
                        }
                    }

                }
                else
                {
                    var node = this.BaseRepository().FindEntity(entity.PARENTID);
                    string enCode = node == null ? "" : node.ENCODE;
                    object obj = BaseRepository().FindObject(string.Format("select max(encode) from hrs_stcategory where parentid='{0}'", entity.PARENTID));
                    if (obj == null || obj == DBNull.Value)
                    {
                        enCode += "001";
                    }
                    else
                    {
                        enCode += GetSno(int.Parse(obj.ToString().Substring(obj.ToString().Length - 3)) + 1, 3);
                    }
                    entity.ENCODE = enCode;
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetSno(int count, int len)
        {
            int length = len - count.ToString().Length;
            length = length > 0 ? length : 0;
            string code = "";
            for (int j = 0; j < length; j++)
            {
                code += "0";
            }
            string val = "";
            if (count.ToString().Contains("E"))
            {
                val = Decimal.Parse(count.ToString(), System.Globalization.NumberStyles.Float).ToString();
            }
            else
            {
                val = count.ToString();
            }
            return code + val;
        }
        #endregion
    }
}
