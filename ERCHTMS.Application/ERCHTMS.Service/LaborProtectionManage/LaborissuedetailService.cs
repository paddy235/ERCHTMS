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
    /// 描 述：劳动防护发放表详情
    /// </summary>
    public class LaborissuedetailService : RepositoryFactory<LaborissuedetailEntity>, LaborissuedetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LaborissuedetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.SueId == queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LaborissuedetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据物品表id获取最近一次发放记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public LaborissuedetailEntity GetOrderLabor(string keyValue)
        {
            return this.BaseRepository().IQueryable(it => it.InfoId == keyValue).OrderByDescending(it => it.CreateDate).ToList().FirstOrDefault();
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
                issue.Type = 0;//表示发放记录
                //新增记录母表
                res.Insert<LaborissueEntity>(issue);

                Repository<LaborinfoEntity> labordb = new Repository<LaborinfoEntity>(DbFactory.Base());
                string sql = string.Format(@"select * from bis_laborinfo where id in({0})", ids);
                List<LaborinfoEntity> laborlist = labordb.FindList(sql).ToList();
                List<LaborissuedetailEntity> detaillist = new List<LaborissuedetailEntity>();
                for (int i = 0; i < laborlist.Count; i++)
                {
                    Laborff ff = list.Where(it => it.ID == laborlist[i].ID).FirstOrDefault();
                    LaborissuedetailEntity le = new LaborissuedetailEntity();
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
                    le.ShouldNum = laborlist[i].ShouldNum;
                    le.SueId = issue.ID;
                    le.TimeNum = laborlist[i].TimeNum;
                    le.TimeType = laborlist[i].TimeType;
                    le.Type = laborlist[i].Type;
                    le.Unit = laborlist[i].Unit;
                    
                    if (le.TimeType == "年")
                    {
                        le.NextTime = DateTime.Now.AddYears(Convert.ToInt32(le.TimeNum));
                    }
                    else if (le.TimeType == "月")
                    {
                        le.NextTime = DateTime.Now.AddMonths(Convert.ToInt32(le.TimeNum));
                    }
                    else if (le.TimeType == "日")
                    {
                        le.NextTime = DateTime.Now.AddDays(Convert.ToInt32(le.TimeNum));
                    }
                    le.Create();
                    detaillist.Add(le);
                    //修改原记录的发放数量与发放时间
                    if (laborlist[i].IssueNum != null)
                    {
                        laborlist[i].IssueNum += le.IssueNum;
                    }
                    else
                    {
                        laborlist[i].IssueNum = le.IssueNum;
                    }

                    laborlist[i].RecentTime = le.RecentTime;
                    laborlist[i].NextTime = le.NextTime;

                    //先获取当前用品标准下所有需发放人员
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
                        eq.LaborType = 1;
                        eq.Reson = "";
                        eq.ShouldNum = ff.PerCount;
                        eq.Size = equ.Size; //eq.Size = "";
                        eq.UserId = equ.UserId;
                        eq.Create();
                        laborque.Add(eq);
                    }



                    res.Insert<LaborequipmentinfoEntity>(laborque);
                }


                res.Insert<LaborissuedetailEntity>(detaillist);
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LaborissuedetailEntity entity, string json, string InfoId)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                LaborissueEntity issue = new LaborissueEntity();
                issue.Create();
                issue.LaborOperationTime = DateTime.Now;
                issue.LaborOperationUserName = OperatorProvider.Provider.Current().UserName;
                issue.Type = 0;//表示发放记录
                //新增记录母表
                res.Insert<LaborissueEntity>(issue);

                Repository<DepartmentEntity> inlogdb = new Repository<DepartmentEntity>(DbFactory.Base());
                DepartmentEntity dept = inlogdb.FindEntity(entity.DeptId);

                Repository<DepartmentEntity> orgdb = new Repository<DepartmentEntity>(DbFactory.Base());
                DepartmentEntity org = orgdb.FindEntity(dept.OrganizeId);

                if (org.Nature.Contains("省级"))
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
                if (entity.TimeType == "年")
                {
                    entity.NextTime = DateTime.Now.AddYears(Convert.ToInt32(entity.TimeNum));
                }
                else if (entity.TimeType == "月")
                {
                    entity.NextTime = DateTime.Now.AddMonths(Convert.ToInt32(entity.TimeNum));
                }
                else if (entity.TimeType == "日")
                {
                    entity.NextTime = DateTime.Now.AddDays(Convert.ToInt32(entity.TimeNum));
                }

                //修改原记录的发放数量与发放时间
                Repository<LaborinfoEntity> laborinfodb = new Repository<LaborinfoEntity>(DbFactory.Base());
                LaborinfoEntity laborinfo = laborinfodb.FindEntity(InfoId);

                if (laborinfo.IssueNum != null)
                {
                    laborinfo.IssueNum += entity.IssueNum;
                }
                else
                {
                    laborinfo.IssueNum = entity.IssueNum;
                }

                laborinfo.RecentTime = entity.RecentTime;
                laborinfo.NextTime = entity.NextTime;

                res.Update<LaborinfoEntity>(laborinfo);


                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<LaborissuedetailEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<LaborissuedetailEntity>(entity);
                }

                json = json.Replace("&nbsp;", "");
                List<LaborequipmentinfoEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LaborequipmentinfoEntity>>(json);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].ID = "";
                    list[i].Create();
                    list[i].AssId = entity.ID;
                    list[i].LaborType = 1;
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
