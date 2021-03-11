using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.EquipmentManage;
using System.Linq;
using System.Text;
using System;
using ERCHTMS.Service.BaseManage;
using System.Data;
using BSFramework.Util;
using Newtonsoft.Json;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.IService.PublicInfoManage;
using ERCHTMS.Service.PublicInfoManage;
using BSFramework.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：工器具审核表
    /// </summary>
    public class ToolsAuditService : RepositoryFactory<ToolsAuditEntity>, ToolsAuditIService
    {
        IFileInfoService fileService = new FileInfoService();
        PeopleReviewIService peopleReview = new PeopleReviewService();
        private ProjecttoolsService projecttoolsservice = new ProjecttoolsService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ToolsAuditEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ToolsAuditEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public ToolsAuditEntity GetAuditEntity(string TOOLSID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from EPG_TOOLSAUDIT t where t.TOOLSID='{0}'", TOOLSID);
            return new RepositoryFactory().BaseRepository().FindList<ToolsAuditEntity>(strSql.ToString()).ToList().FirstOrDefault();
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["aptitudeid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.toolsid='{0}' ", queryParam["aptitudeid"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
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
        public void SaveFormToolAudit(string keyValue, ToolsAuditEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 通过工器具信息ID获取审核信息
        /// </summary>
        /// <param name="toolid"></param>
        /// <returns></returns>
        public List<ToolsAuditEntity> GetAuditList(string toolid)
        {
            string sql = string.Format(@"select * from epg_toolsaudit t where t.toolsid='{0}'", toolid);
            return new RepositoryFactory().BaseRepository().FindList<ToolsAuditEntity>(sql).ToList();
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="moduleName">模块名称（值：设备工器具、特种设备工器具）</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ToolsAuditEntity entity, string moduleName)
        {
            var res = DbFactory.Base().BeginTrans();
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                string state = string.Empty;

                string outengineerid = keyValue;
                
                
                #region  保存工器具
                Repository<ToolsEntity> repAudit = new Repository<ToolsEntity>(DbFactory.Base());
                ToolsEntity smEntity = repAudit.FindEntity(entity.TOOLSID);

                #region //审核信息表
                ToolsAuditEntity aidEntity = new ToolsAuditEntity();
                aidEntity.AUDITID = Guid.NewGuid().ToString();
                aidEntity.AUDITRESULT = entity.AUDITRESULT; //通过
                aidEntity.AUDITTIME = DateTime.Now; //审核时间
                aidEntity.AUDITPEOPLE = entity.AUDITPEOPLE;  //审核人员姓名
                aidEntity.AUDITPEOPLEID = entity.AUDITPEOPLEID;//审核人员id
                aidEntity.TOOLSID = smEntity.TOOLSID;  //关联的业务ID 
                aidEntity.AUDITDEPTID = entity.AUDITDEPTID;//审核部门id
                aidEntity.AUDITDEPT = entity.AUDITDEPT; //审核部门
                aidEntity.AUDITOPINION = entity.AUDITOPINION; //审核意见
                aidEntity.AUDITFILE = entity.AUDITFILE;//审核附件
                aidEntity.REMARK = entity.REMARK; //备注
                aidEntity.FlowId = entity.FlowId;
                SaveFormToolAudit("", aidEntity);
                #endregion
                Boolean IsNextFlow = true; //判断是否走下一步流程; 当安全专工跟电气专工验收工器具时没有验收完所有工器具时会继续到专工验收
                Boolean IsPass = true; //判断是否所有工器具都没有验收通过  false 为所有工器具都没有验收通过
                #region   审核通过
                if (entity.AUDITRESULT == "0")
                {
                    ManyPowerCheckEntity powerentity = new ManyPowerCheckService().GetEntity(entity.FlowId);
                    if (powerentity.FLOWNAME.Contains("验收"))
                    {
                        List<ProjecttoolsEntity> toollist = projecttoolsservice.GetList(" and toolsid ='" + smEntity.TOOLSID + "'").ToList();
                        if (toollist.Where(t => t.Status == null).Count() > 0) //判断是否有没有验收的数据
                        {
                            IsNextFlow = false;
                        }
                        else
                        {
                            if (toollist.Where(t => t.Status == "1").Count() == toollist.Count()) //判断验收不通过的数量是否等于工器具数量
                            {
                                IsPass = false;
                            }
                        }
                    }
                    if (IsNextFlow && IsPass)
                    {
                        ManyPowerCheckEntity mpcEntity = peopleReview.CheckAuditPower(curUser, out state, moduleName, outengineerid, false, entity.FlowId);
                        //0表示流程未完成，1表示流程结束
                        if (null != mpcEntity)
                        {
                            smEntity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                            smEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                            smEntity.FLOWROLE = mpcEntity.CHECKROLEID;
                            smEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                            smEntity.ISSAVED = "1";
                            smEntity.ISOVER = "0";
                            smEntity.FlowId = mpcEntity.ID;
                            smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                        }
                        else
                        {
                            smEntity.FLOWDEPT = "";
                            smEntity.FLOWDEPTNAME = "";
                            smEntity.FLOWROLE = "";
                            smEntity.FLOWROLENAME = "";
                            smEntity.ISSAVED = "1";
                            smEntity.ISOVER = "1";
                            smEntity.FLOWNAME = "";
                            #region 更新工程流程状态
                            Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                            StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", smEntity.OUTENGINEERID)).ToList().FirstOrDefault();
                            startProecss.EQUIPMENTTOOLSTATUS = "1";
                            res.Update<StartappprocessstatusEntity>(startProecss);
                            #endregion
                            #region 同步设备
                            if (moduleName == "特种设备工器具")
                                new SpecialEquipmentService().SyncSpecificTools(smEntity.OUTENGINEERID, smEntity.OUTPROJECTID, smEntity.TOOLSID);
                            #endregion
                        }
                    }
                    
                }
                #endregion
                #region  审核不通过
                if (entity.AUDITRESULT=="1" || IsPass==false)
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.ISSAVED = "0"; //处于登记阶段
                    smEntity.ISOVER = "0"; //是否完成状态赋值为未完成
                    smEntity.FLOWNAME = "";
                    #region 审核不通过保存工器具历史数据
                    var str = JsonConvert.SerializeObject(smEntity);
                    HistoryToolsEntity histools = JsonConvert.DeserializeObject<HistoryToolsEntity>(str);
                    histools.ID = Guid.NewGuid().ToString();
                    histools.TOOLSID = smEntity.TOOLSID;
                    res.Insert<HistoryToolsEntity>(histools);
                    //施工机具
                    Repository<ProjecttoolsEntity> protools = new Repository<ProjecttoolsEntity>(DbFactory.Base());
                    List<ProjecttoolsEntity> protools_list = protools.FindList(string.Format(@"select * from epg_projecttools t where t.toolsid='{0}'", smEntity.TOOLSID)).ToList();
                    res.ExecuteBySql(string.Format("update epg_projecttools set Status='',CHECKOPTION=''  where toolsid='{0}'", smEntity.TOOLSID));
                    if (protools_list.Count > 0)
                    {
                        var str_protools = JsonConvert.SerializeObject(protools_list);
                        List<HistoryProtoolsEntity> hisprolist = JsonConvert.DeserializeObject<List<HistoryProtoolsEntity>>(str_protools);
                        for (int i = 0; i < hisprolist.Count; i++)
                        {
                            hisprolist[i].ID = Guid.NewGuid().ToString();
                            //同步附件
                            var file1 = fileService.GetFiles(protools_list[i].PROJECTTOOLSID);
                            if (file1.Rows.Count > 0)
                            {
                                var key = hisprolist[i].ID;
                                foreach (DataRow item in file1.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                            hisprolist[i].TOOLSID = histools.ID;
                        }
                        res.Insert<HistoryProtoolsEntity>(hisprolist);
                    }
                    //特种设备
                    Repository<SpecificToolsEntity> spetools = new Repository<SpecificToolsEntity>(DbFactory.Base());
                    List<SpecificToolsEntity> spetools_list = spetools.FindList(string.Format(@"select * from epg_specifictools t where t.toolsid='{0}'", smEntity.TOOLSID)).ToList();
                    if (spetools_list.Count > 0)
                    {
                        var str_spetools = JsonConvert.SerializeObject(spetools_list);
                        List<HistorySpecificToolsEntity> hisspelist = JsonConvert.DeserializeObject<List<HistorySpecificToolsEntity>>(str_spetools);
                        for (int i = 0; i < hisspelist.Count; i++)
                        {
                            hisspelist[i].SPECIFICTOOLSID = Guid.NewGuid().ToString();
                            hisspelist[i].TOOLSID = histools.ID;
                            //同步附件
                            var file1 = fileService.GetFiles(spetools_list[i].REGISTERCARDFILE);
                            var file2 = fileService.GetFiles(spetools_list[i].QUALIFIED);
                            var file3 = fileService.GetFiles(spetools_list[i].CHECKREPORTFILE);
                            var file4 = fileService.GetFiles(spetools_list[i].SPECIFICTOOLSFILE);
                            if (file1.Rows.Count > 0)
                            {
                                hisspelist[i].REGISTERCARDFILE = Guid.NewGuid().ToString();
                                var key = hisspelist[i].REGISTERCARDFILE;
                                foreach (DataRow item in file1.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                            if (file2.Rows.Count > 0)
                            {
                                hisspelist[i].QUALIFIED = Guid.NewGuid().ToString();
                                var key = hisspelist[i].QUALIFIED;
                                foreach (DataRow item in file2.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                            if (file3.Rows.Count > 0)
                            {
                                hisspelist[i].CHECKREPORTFILE = Guid.NewGuid().ToString();
                                var key = hisspelist[i].CHECKREPORTFILE;
                                foreach (DataRow item in file3.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                            if (file4.Rows.Count > 0)
                            {
                                hisspelist[i].SPECIFICTOOLSFILE = Guid.NewGuid().ToString();
                                var key = hisspelist[i].SPECIFICTOOLSFILE;
                                foreach (DataRow item in file4.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                        }
                        res.Insert<HistorySpecificToolsEntity>(hisspelist);
                    }
                    //获取当前业务对象的所有审核记录
                    var shlist = GetAuditList(entity.TOOLSID);
                    //批量更新审核记录关联ID
                    foreach (ToolsAuditEntity mode in shlist)
                    {
                        mode.TOOLSID = histools.ID; //对应新的ID
                        //同步附件
                        var file = fileService.GetFiles(mode.AUDITFILE);
                        if (file.Rows.Count > 0)
                        {
                            var key = histools.ID;
                            foreach (DataRow item in file.Rows)
                            {
                                FileInfoEntity itemFile = new FileInfoEntity();
                                itemFile.FileName = item["FileName"].ToString();
                                itemFile.FilePath = item["filepath"].ToString();
                                itemFile.FileSize = item["filesize"].ToString();
                                itemFile.RecId = key;
                                fileService.SaveForm(itemFile.FileId, itemFile);
                            }
                        }
                        res.Update<ToolsAuditEntity>(mode);
                    }
                    #endregion
                }
                res.Update<ToolsEntity>(smEntity);
                res.Commit();
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                res.Rollback();
            }
        }

        #endregion
    }
}
