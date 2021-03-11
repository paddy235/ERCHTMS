using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� ��������·��������
    /// </summary>
    public class RouteconfigService : RepositoryFactory<RouteconfigEntity>, RouteconfigIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<RouteconfigEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ��ȡ���ڵ������
        /// </summary>
        /// <returns></returns>
        public List<RouteconfigEntity> GetTree(int type)
        {
            return this.BaseRepository().IQueryable(it => it.LineType == type).OrderBy(it => it.Level).ThenBy(it => it.Sort).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetVisitParentid()
        {
            RouteconfigEntity route = this.BaseRepository().IQueryable(it => it.LineType == 1 && it.Level == 2).FirstOrDefault();
            return route.ID;
        }

        /// <summary>
        /// ��ȡ·����������
        /// </summary>
        /// <returns></returns>
        public List<RouteconfigEntity> RouteDropdown()
        {
            return this.BaseRepository().IQueryable(it => it.LineType == 1 && it.Level == 3).OrderBy(it => it.Sort).ToList();
        }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns></returns>
        public List<Route> GetRoute()
        {
            //return this.BaseRepository().IQueryable(it => it.Level == 3).OrderBy(it => it.Sort).ToList();
            string sql = string.Format(
                "select route1.Itemname,route1.sort as sort1,route2.itemname as TypeName,route2.sort as sort2,route3.itemname as RouteName,route3.sort as" +
                " sort3,route3.gid,route3.pointlist,route3.id from bis_routeconfig route1" +
                " left join bis_routeconfig route2 on route1.id=route2.parentid" +
                " left join bis_routeconfig route3 on route2.id=route3.parentid" +
                " where route1.\"LEVEL\"=1 and route3.isenable=1 order by route1.sort,route2.sort");
            Repository<Route> inlogdb = new Repository<Route>(DbFactory.Base());
            return inlogdb.FindList(sql).ToList();
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetWlList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  d.enabledmark = 1 and d.deletemark = 0  and d.itemid in  (select itemid from base_dataitem where parentid in (select itemid from base_dataitem where itemcode='MaterialsRoute'))
                                order by d.sortcode asc");
            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public RouteconfigEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// Ӧ��·��
        /// </summary>
        /// <param name="ID"></param>
        public void SelectLine(string ID)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
                Repository<RouteconfigEntity> inlogdb = new Repository<RouteconfigEntity>(DbFactory.Base());
                RouteconfigEntity old = inlogdb.FindEntity(ID);
                if (old.Level == 3)
                {
                    List<RouteconfigEntity> routelist = inlogdb.IQueryable(it => it.ParentId == old.ParentId).ToList();
                    for (int i = 0; i < routelist.Count; i++)
                    {
                        if (routelist[i].ID == ID)
                        {
                            routelist[i].IsEnable = 1;
                        }
                        else
                        {
                            routelist[i].IsEnable = 0;
                        }
                    }

                    res.Update<RouteconfigEntity>(routelist);
                    res.Commit();
                }

            }
            catch (Exception e)
            {
                res.Rollback();
                throw e;
            }
        }

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
        public void SaveForm(string keyValue, RouteconfigEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="rlist"></param>
        public void SaveList(List<RouteconfigEntity> rlist)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.Insert<RouteconfigEntity>(rlist);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        #endregion
    }
}
