using System;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ձ��ϱ�����
    /// </summary>
    public class LaborrecyclingService : RepositoryFactory<LaborrecyclingEntity>, LaborrecyclingIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LaborrecyclingEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.SueId==queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LaborrecyclingEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveListForm(string json)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                json = json.Replace("&nbsp;", "");
                List<Laborff> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Laborff>>(json);

                string ids = "";
                foreach (Laborff item in list)
                {
                    if (ids == "")
                    {
                        ids = "'" + item.ID + "'";
                    }
                    else
                    {
                        ids += ",'" + item.ID + "'";
                    }
                }

                LaborissueEntity issue = new LaborissueEntity();
                issue.Create();
                issue.LaborOperationTime = DateTime.Now;
                issue.LaborOperationUserName = OperatorProvider.Provider.Current().UserName;
                issue.Type = 1;//��ʾ���ռ�¼
                //������¼ĸ��
                res.Insert<LaborissueEntity>(issue);

                Repository<LaborinfoEntity> labordb = new Repository<LaborinfoEntity>(DbFactory.Base());
                string sql = string.Format(@"select * from bis_laborinfo where id in({0})", ids);
                List<LaborinfoEntity> laborlist = labordb.FindList(sql).ToList();
                List<LaborrecyclingEntity> detaillist = new List<LaborrecyclingEntity>();
                for (int i = 0; i < laborlist.Count; i++)
                {
                    Laborff ff = list.Where(it => it.ID == laborlist[i].ID).FirstOrDefault();
                    LaborrecyclingEntity le = new LaborrecyclingEntity();
                    le.PostId = laborlist[i].PostId;
                    le.DeptCode = laborlist[i].DeptCode;
                    le.DeptId = laborlist[i].DeptId;
                    le.DeptName = laborlist[i].DeptName;
                    le.InfoId = laborlist[i].ID;
                    le.IssueNum = ff.Count;
                    le.LaborOperationTime = DateTime.Now;
                    le.LaborOperationUserName = OperatorProvider.Provider.Current().UserName;
                    le.Name = laborlist[i].Name;
                    le.RecentTime = DateTime.Now;
                    le.Model = laborlist[i].Model;
                    le.Note = laborlist[i].Note;
                    le.No = laborlist[i].No;
                    le.OrgCode = laborlist[i].OrgCode;
                    le.OrgId = laborlist[i].OrgId;
                    le.OrgName = laborlist[i].OrgName;
                    le.PostName = laborlist[i].PostName;
                    le.SueId = issue.ID;
                    le.Type = laborlist[i].Type;
                    le.Unit = laborlist[i].Unit;
                    le.Create();
                    detaillist.Add(le);



                    //�Ȼ�ȡ��ǰ��Ʒ��׼�������跢����Ա
                    Repository<LaborequipmentinfoEntity> equdb = new Repository<LaborequipmentinfoEntity>(DbFactory.Base());
                    string equsql = string.Format("select * from bis_laborequipmentinfo where assid='{0}'",
                        laborlist[i].ID);
                    List<LaborequipmentinfoEntity> equlist = equdb.FindList(equsql).ToList();
                    List<LaborequipmentinfoEntity> laborque = new List<LaborequipmentinfoEntity>();
                    foreach (LaborequipmentinfoEntity equ in equlist)
                    {
                        LaborequipmentinfoEntity eq = new LaborequipmentinfoEntity();
                        eq.UserName = equ.UserName;
                        eq.AssId = le.ID;
                        eq.Brand = "";
                        eq.LaborType = 2;
                        eq.Reson = "";
                        eq.ShouldNum = ff.PerCount;
                        eq.Size = "";
                        eq.UserId = equ.UserId;
                        eq.Create();
                        laborque.Add(eq);
                    }



                    res.Insert<LaborequipmentinfoEntity>(laborque);
                }


                res.Insert<LaborrecyclingEntity>(detaillist);
                res.Update<LaborinfoEntity>(laborlist);

                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LaborrecyclingEntity entity, string json, string InfoId)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                LaborissueEntity issue = new LaborissueEntity();
                issue.Create();
                issue.LaborOperationTime = DateTime.Now;
                issue.LaborOperationUserName = OperatorProvider.Provider.Current().UserName;
                issue.Type = 1;//��ʾ���ż�¼
                //������¼ĸ��
                res.Insert<LaborissueEntity>(issue);

                Repository<DepartmentEntity> inlogdb = new Repository<DepartmentEntity>(DbFactory.Base());
                DepartmentEntity dept = inlogdb.FindEntity(entity.DeptId);

                Repository<DepartmentEntity> orgdb = new Repository<DepartmentEntity>(DbFactory.Base());
                DepartmentEntity org = orgdb.FindEntity(dept.OrganizeId);

                if (org.Nature.Contains("ʡ��"))
                {
                    entity.OrgCode = org.DeptCode;
                }
                else
                {
                    entity.OrgCode = org.EnCode;
                }

                entity.OrgId = org.OrganizeId;
                entity.OrgName = org.FullName;
                entity.InfoId = InfoId;
                entity.SueId = issue.ID;
                entity.RecentTime = DateTime.Now;
                

                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<LaborrecyclingEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<LaborrecyclingEntity>(entity);
                }
                json = json.Replace("&nbsp;", "");
                List<LaborequipmentinfoEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LaborequipmentinfoEntity>>(json);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].ID = "";
                    list[i].Create();
                    list[i].AssId = entity.ID;
                    list[i].LaborType = 2;
                }

                res.Insert<LaborequipmentinfoEntity>(list);
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
                throw e;
            }
        }
        #endregion
    }
}
