using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;
using Newtonsoft.Json;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 临时分组管理
    /// </summary>
    public class TemporaryGroupsService : RepositoryFactory<TemporaryGroupsEntity>, TemporaryGroupsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TemporaryGroupsEntity> GetList(string userId)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取满足黑名单条件的人员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetBlacklistUsers(ERCHTMS.Code.Operator user)
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("userid");
            dtTemp.Columns.Add("realname");
            dtTemp.Columns.Add("deptname");
            dtTemp.Columns.Add("itemname");
            dtTemp.Columns.Add("remark");
            DataTable dtItems = BaseRepository().FindTable(string.Format("select itemvalue,itemcode,remark from BIS_BLACKSET where status=1 and deptcode='{0}'", user.OrganizeCode));
            foreach (DataRow dr in dtItems.Rows)
            {
                DataTable dt = null;
                string sql = "";
                //年龄判断
                if (dr[1].ToString() == "01")
                {
                    string[] arr = dr[0].ToString().Split('|');
                    sql = string.Format("select userid,realname,u.DEPTNAME,'" + dr[2].ToString() + @"' itemname,('出生日期为：' || to_char(birthday,'yyyy-MM-dd')) remark from v_userinfo u where isblack=0 and  gender='{0}' and birthday is not null and u.DEPARTMENTCODE like '{3}%'
and (round(sysdate-to_date(to_char(u.birthday,'yyyy-MM-dd'),'yyyy-MM-dd'))/365<{1} or round(sysdate-to_date(to_char(u.birthday,'yyyy-MM-dd'),'yyyy-MM-dd'))/365>{2})", "男", arr[0], arr[1], user.DeptCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);

                    sql = string.Format("select userid,realname,u.DEPTNAME,'" + dr[2].ToString() + @"' itemname,('出生日期为：' || to_char(birthday,'yyyy-mm-dd')) remark  from v_userinfo u where isblack=0 and gender='{0}' and birthday is not null and u.DEPARTMENTCODE like '{3}%'
and (round(sysdate-to_date(to_char(u.birthday,'yyyy-mm-dd'),'yyyy-MM-dd'))/365<to_number({1}) or round(sysdate-to_date(to_char(u.birthday,'yyyy-mm-dd'),'yyyy-MM-dd'))/365>to_number({2})) ", "女", arr[2], arr[3], user.DeptCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
                //一般违章
                if (dr[1].ToString() == "03")
                {
                    sql = string.Format("select t.lllegalpersonid userid,t.lllegalperson realname,'" + dr[2].ToString() + @"' itemname,LLLEGALTEAM deptname,('一般违章次数:' || count(1)) remark from V_LLLEGALBASEINFO t where t.flowstate='流程结束' and LLLEGALLEVEL='fc53ff18-b212-4763-9760-baf476eea5f3' and LLLEGALTEAMcode like '{1}%' and lllegalpersonid in(select userid from v_userinfo where isblack=0 and organizecode='{2}' ) group by 
lllegalpersonid,lllegalperson,LLLEGALTEAM having count(1)>{0}", dr[0].ToString(), user.DeptCode, user.OrganizeCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
                //严重违章
                if (dr[1].ToString() == "04")
                {
                    sql = string.Format(@"select t.lllegalpersonid userid,t.lllegalperson realname,'" + dr[2].ToString() + @"' itemname,LLLEGALTEAM deptname,('严重违章:' || count(1)) remark from V_LLLEGALBASEINFO t where t.flowstate='流程结束' and LLLEGALLEVEL='5aae9e88-c06d-4383-afec-6165d5c1a312' and LLLEGALTEAMcode like '{1}%' and lllegalpersonid in(select userid from v_userinfo where isblack=0 and organizecode='{2}' ) group by 
lllegalpersonid,lllegalperson,LLLEGALTEAM having count(1)>{0}", dr[0].ToString(), user.DeptCode, user.OrganizeCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
            }
            return dtTemp;
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString().Trim();
                switch (condition)
                {
                    case "RealName":          //姓名
                        pagination.conditionJson += string.Format(" and USERNAME  like '%{0}%'", keyord);
                        break;
                    case "Mobile":          //手机
                        pagination.conditionJson += string.Format(" and TEL like '%{0}%'", keyord);
                        break;
                    case "identifyid":          //身份证号
                        pagination.conditionJson += string.Format(" and identifyid like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TemporaryGroupsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取临时人员实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public TemporaryUserEntity GetUserEntity(string keyValue)
        {
            Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
            TemporaryUserEntity old = inlogdb.FindEntity(keyValue);
            if (old == null)
            {//临时表没记录取用户表基础信息
                UserService userbll = new UserService();
                DepartmentService deptbll = new DepartmentService();
                var uentity = userbll.GetEntity(keyValue);
                old = new TemporaryUserEntity();
                old.UserName = uentity.RealName;
                old.Tel = uentity.Mobile;
                old.Identifyid = uentity.IdentifyID;
                old.startTime = uentity.CreateDate;
                old.Istemporary = 2;
                //old.UserImg = uentity.HeadIcon;
            }
            return old;
        }

        /// <summary>
        /// 获取临时人员实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public TemporaryUserEntity GetEmptyUserEntity(string keyValue)
        {
            Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
            TemporaryUserEntity old = inlogdb.FindEntity(keyValue);
            return old;
        }


        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            TemporaryGroupsEntity entity = this.BaseRepository().FindEntity(keyValue);
            this.BaseRepository().Delete(entity);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<TemporaryGroupsEntity> entity, List<TemporaryGroupsEntity> Updatelist)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<TemporaryGroupsEntity> list = new List<TemporaryGroupsEntity>();
                List<TemporaryGroupsEntity> updatelist = new List<TemporaryGroupsEntity>();
                if (!string.IsNullOrEmpty(keyValue))
                {
                    //entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    string sql = string.Empty;
                    if (entity != null)
                    {
                        int num = 0;
                        foreach (var item in entity)
                        {
                            TemporaryGroupsEntity newEntity = new TemporaryGroupsEntity();
                            newEntity.Create();
                            newEntity.GroupsName = item.GroupsName;
                            newEntity.ORDERNUM = num++;
                            newEntity.ISEnable = "1";
                            list.Add(newEntity);
                        }
                    }
                    if (Updatelist != null)
                    {
                        foreach (var upentity in Updatelist)
                        {
                            var updaentity = GetEntity(upentity.ID);
                            if (updaentity != null)
                            {
                                updaentity.GroupsName = upentity.GroupsName;
                                updatelist.Add(updaentity);
                            }
                        }
                    }
                    res.Insert<TemporaryGroupsEntity>(list);
                    res.Update<TemporaryGroupsEntity>(updatelist);
                    res.Commit();
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 删除临时人员
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void DeleteTemporaryList(string keyValue, TemporaryUserEntity entity)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.Delete<TemporaryUserEntity>(entity);
                res.Commit();
            }
            catch (Exception er)
            {
                res.Rollback();
            }
        }


        #endregion

        #region 海康平台

        /// <summary>
        /// 保存临时人员
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public List<AddJurisdictionEntity> SaveUForm(string keyValue, TemporaryUserEntity entity)
        {
            List<AddJurisdictionEntity> taskIds = new List<AddJurisdictionEntity>();
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    list.Add(entity);
                }
                else
                {
                    entity.Create();
                    list.Add(entity);
                }
                res.Insert<TemporaryUserEntity>(list);
                res.Commit();
                var tasks = InsertTempUserHiK(entity, list);
                taskIds.Add(tasks);
            }
            catch (Exception er)
            {
                res.Rollback();
            }
            return taskIds;
        }


        /// <summary>
        /// 单条同步更换海康人脸信息
        /// </summary>
        public string UpdateHumanFace(TemporaryUserEntity entity, string baseUrl, string Key, string Signature)
        {
            string res = string.Empty;
            try
            {
                string url = "/artemis/api/resource/v2/person/advance/personList";
                var model = new
                {
                    personIds = entity.USERID,
                    pageNo = 1,
                    pageSize = 100
                };
                //查询人脸图片唯一标识
                string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                UserPicEntity re = JsonConvert.DeserializeObject<UserPicEntity>(msg);
                if (re != null && re.data.list.Count > 0)
                {
                    string picfileid = string.Empty;
                    foreach (var item in re.data.list)
                    {
                        foreach (var items in item.personPhoto)
                        {
                            picfileid = items.personPhotoIndexCode;
                        }
                    }
                    //更换人脸图片
                    string upurl = "/artemis/api/resource/v1/face/single/update";
                    var delmodel = new
                    {
                        faceId = picfileid,
                        faceData = entity.ImgData
                    };
                    res = SocketHelper.LoadCameraList(delmodel, baseUrl, upurl, Key, Signature);
                }
                else if (!string.IsNullOrEmpty(entity.ImgData))
                {//没查到人脸唯一标示，表示人脸上传未成功就重新上传人脸信息
                    List<FacedataEntity> FaceList = new List<FacedataEntity>();
                    FacedataEntity faces = new FacedataEntity();
                    faces.UserId = entity.USERID;
                    faces.ImgData = entity.ImgData;
                    FaceList.Add(faces);
                    res = SocketHelper.UploadFace(FaceList, baseUrl, Key, Signature);
                }
            }
            catch (Exception)
            {
            }
            return res;
        }


        /// <summary>
        /// 删除对应设备中出入权限配置记录
        /// </summary>
        public AddJurisdictionEntity DelEquipmentRecord(List<TemporaryUserEntity> list, string baseUrl, string Key, string Signature)
        {
            AddJurisdictionEntity Juentity = new AddJurisdictionEntity();
            if (list.Count > 0)
            {
                var url = "/artemis/api/acps/v1/auth_config/delete";
                List<personDatas1> personDatas = new List<personDatas1>();
                personDatas1 entity = new personDatas1();
                List<string> codes = new List<string>();
                foreach (var Item in list)
                {
                    codes.Add(Item.USERID);//人员Id
                }
                entity.indexCodes = codes;
                entity.personDataType = "person";
                personDatas.Add(entity);

                #region  查询设备唯一编号

                #region 新代码
                if (!string.IsNullOrEmpty(list[0].PassPost))
                {
                    var db = new RepositoryFactory().BaseRepository();
                    var areaArray = list[0].PassPost.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    //获取设备
                    var deviceList = (from area in db.IQueryable<DataItemEntity>()
                                      join parent in db.IQueryable<DataItemEntity>() on area.ParentId equals parent.ItemId
                                      join device in db.IQueryable<DataItemDetailEntity>() on area.ItemId equals device.ItemId
                                      where parent.ItemName == "海康门禁设备" && areaArray.Contains(area.ItemName)
                                      select new { device.ItemName, device.ItemValue,device.ItemCode }).ToList();
                    if (deviceList != null && deviceList.Count > 0)
                    {
                        List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//设备信息集合
                        deviceList.ForEach(x =>
                        {
                            List<int> nos = new List<int>();
                            if (x.ItemCode=="1")
                            {//门禁通道
                                nos.Add(1);
                                nos.Add(2);
                            }
                            else
                            {//门禁
                                nos.Add(1);
                            }
                            resourceInfos1 entity1 = new resourceInfos1();
                            entity1.resourceIndexCode = x.ItemValue;//设备唯一编号
                            entity1.resourceType = "acsDevice";
                            entity1.channelNos = nos;
                            resourceInfos.Add(entity1);
                        });


                        var model = new
                        {
                            personDatas,
                            resourceInfos
                        };
                        string msgs = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                        JurisdictionEntity p1 = JsonConvert.DeserializeObject<JurisdictionEntity>(msgs);
                        if (p1 != null && p1.code == "0")
                        {
                            Juentity.taskId = p1.data.taskId;
                            Juentity.resourceInfos = resourceInfos;
                            Juentity.type = "del";
                        }
                    }

                }
                #endregion

                #region 旧代码
                //string Qres = string.Empty;
                //if (!string.IsNullOrEmpty(list[0].PassPost))
                //{
                //    string[] post = list[0].PassPost.Split(',');
                //    foreach (var item in post)
                //    {
                //        if (string.IsNullOrEmpty(item)) continue;
                //        switch (item)
                //        {
                //            case "一号岗":
                //                Qres += "'equipment1'" + ",";
                //                break;
                //            case "码头岗":
                //                Qres += "'equipmentmt'" + ",";
                //                break;
                //            case "三号岗":
                //                Qres += "'equipment3'" + ",";
                //                break;
                //            case "生活区岗":
                //                break;
                //            default:
                //                break;
                //        }
                //    }
                //}
                //string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode in ({0}) order by t.sortcode asc", Qres.TrimEnd(','));

                //if (!string.IsNullOrEmpty(Qres))
                //{
                //    //出入权限
                //    DataTable dt = this.BaseRepository().FindTable(sql);
                //    if (dt.Rows.Count > 0)
                //    {
                //        List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//设备信息集合
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            List<int> nos = new List<int>();
                //            if (dt.Rows[i][2].ToString() == "1")
                //            {//门禁通道
                //                nos.Add(1);
                //                nos.Add(2);
                //            }
                //            else
                //            {//门禁
                //                nos.Add(1);
                //            }
                //            resourceInfos1 entity1 = new resourceInfos1();
                //            entity1.resourceIndexCode = dt.Rows[i][1].ToString();//设备唯一编号
                //            entity1.resourceType = "acsDevice";
                //            entity1.channelNos = nos;
                //            resourceInfos.Add(entity1);
                //}
                //        var model = new
                //        {
                //            personDatas,
                //            resourceInfos
                //        };
                //        string msgs = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                //        JurisdictionEntity p1 = JsonConvert.DeserializeObject<JurisdictionEntity>(msgs);
                //        if (p1 != null && p1.code == "0")
                //        {
                //            Juentity.taskId = p1.data.taskId;
                //            Juentity.resourceInfos = resourceInfos;
                //            Juentity.type = "del";
                //        }
                //    }
                //}
                #endregion

                #endregion
            }
            return Juentity;
        }

        /// <summary>
        /// 单条记录授权或重新授权
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="keyValue"></param>
        /// <param name="Power">是否受过权 true 更改权限 false 添加权限</param>
        public List<AddJurisdictionEntity> SaveUserFace(TemporaryUserEntity entity1, string keyValue, bool Power)
        {
            List<AddJurisdictionEntity> taskIds = new List<AddJurisdictionEntity>();
            var res = DbFactory.Base().BeginTrans();
            try
            {
                DataItemDetailService data = new DataItemDetailService();
                var pitem = data.GetItemValue("Hikappkey");//密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
                TemporaryUserEntity uploadEntity = inlogdb.FindEntity(keyValue);
                TemporaryUserEntity downloadEntity = new TemporaryUserEntity();
                if (uploadEntity != null)
                {
                    List<TemporaryUserEntity> downloadList = new List<TemporaryUserEntity>();
                    downloadEntity.USERID = uploadEntity.USERID;
                    downloadEntity.PassPost = uploadEntity.PassPost;
                    if (entity1.Remark == "1")
                    {
                        uploadEntity.UserImg = entity1.UserImg;
                        uploadEntity.ImgData = entity1.ImgData;
                    }
                    uploadEntity.PassPost = entity1.PassPost;
                    uploadEntity.startTime = entity1.startTime;
                    uploadEntity.EndTime = entity1.EndTime;
                    res.Update(uploadEntity);
                    res.Commit();  
                    if (Power)
                    {//更改权限
                        if (entity1.Remark == "1")
                        {//更换人脸(仅用于页面状态临时判断)
                            string msg = UpdateHumanFace(uploadEntity, baseurl, Key, Signature);
                        }
                        //删除权限集
                        if (!string.IsNullOrEmpty(downloadEntity.PassPost))
                        {
                            downloadList.Add(downloadEntity);
                            var delEntity = DelEquipmentRecord(downloadList, baseurl, Key, Signature);
                            taskIds.Add(delEntity);
                        }
                    }
                    else
                    {//分配卡
                        SetLoadUserCarNo(uploadEntity, baseurl, Key, Signature);
                    }
                    //添加权限集
                    if (!string.IsNullOrEmpty(entity1.PassPost))
                    {
                        List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                        list.Add(entity1);
                        var addEntity = UploadUserlimits(list, baseurl, Key, Signature);
                        taskIds.Add(addEntity);
                    }

                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
            return taskIds;
        }


        /// <summary>
        /// 批量权限设置
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> SaveCycle(List<TemporaryUserEntity> list)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            List<AddJurisdictionEntity> taskIds = new List<AddJurisdictionEntity>();
            try
            {
                List<TemporaryUserEntity> list1 = new List<TemporaryUserEntity>();
                List<TemporaryUserEntity> addlist = new List<TemporaryUserEntity>();
                List<TemporaryUserEntity> updatelist = new List<TemporaryUserEntity>();
                Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
                DataItemDetailService data = new DataItemDetailService();
                var pitem = data.GetItemValue("Hikappkey");//密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                foreach (var entity in list)
                {
                    TemporaryUserEntity old = inlogdb.FindEntity(entity.USERID);
                    if (old != null)
                    {
                        old.EndTime = entity.EndTime;
                        old.startTime = entity.startTime;
                        if (old.Istemporary == 0 && string.IsNullOrEmpty(old.PassPost))
                        {//添加权限
                            old.PassPost = entity.PassPost;
                            addlist.Add(old);
                        }
                        else
                        {//更改权限
                            updatelist.Add(old);
                            old.PassPost = entity.PassPost;
                        }
                        list1.Add(old);
                    }
                }
                SetLoadUserCarNo1(addlist, baseurl, Key, Signature);
                var delEntity = DelEquipmentRecord(updatelist, baseurl, Key, Signature);
                var addEntity = UploadUserlimits(list1, baseurl, Key, Signature);
                taskIds.Add(delEntity);//删除权限集
                taskIds.Add(addEntity);//添加权限集

                res.Update<TemporaryUserEntity>(list1);
                res.Commit();
            }
            catch (Exception er)
            {
                res.Rollback();
            }
            return taskIds;
        }

        /// <summary>
        /// 加入禁入名单
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> SaveForbidden(List<TemporaryUserEntity> list)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            List<AddJurisdictionEntity> taskIds = new List<AddJurisdictionEntity>();
            try
            {
                Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
                Repository<ForbiddenRecordEntity> Frecord = new Repository<ForbiddenRecordEntity>(DbFactory.Base());
                DataItemDetailService data = new DataItemDetailService();
                var pitem = data.GetItemValue("Hikappkey");//密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                List<TemporaryUserEntity> list1 = new List<TemporaryUserEntity>();
                List<ForbiddenRecordEntity> list2 = new List<ForbiddenRecordEntity>();
                List<TemporaryUserEntity> list3 = new List<TemporaryUserEntity>();
                foreach (var entity in list)
                {
                    TemporaryUserEntity old = inlogdb.FindEntity(entity.USERID);
                    if (old != null)
                    {
                        old.ISDebar = 1;
                        old.Remark = entity.Remark;
                        list1.Add(old);
                        old.EndTime = entity.EndTime;
                        //禁入记录
                        ForbiddenRecordEntity RecordEntity = new ForbiddenRecordEntity();
                        RecordEntity.Create();
                        RecordEntity.StartTime = DateTime.Now;
                        RecordEntity.Remark = entity.Remark;
                        RecordEntity.UserId = old.USERID;
                        list2.Add(RecordEntity);
                        list3.Add(old);
                    }
                }
                var Invalidentity = UploadUserlimits(list3, baseurl, Key, Signature);
                taskIds.Add(Invalidentity);
                res.Update<TemporaryUserEntity>(list1);
                res.Insert<ForbiddenRecordEntity>(list2);
                res.Commit();
            }
            catch (Exception er)
            {
                res.Rollback();
            }
            return taskIds;
        }

        /// <summary>
        /// 删除海康设备上人脸数据
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> DeleteRightFromDevice(List<TemporaryUserEntity> list)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            List<AddJurisdictionEntity> taskIds = new List<AddJurisdictionEntity>();
            try
            {
                Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
                Repository<ForbiddenRecordEntity> Frecord = new Repository<ForbiddenRecordEntity>(DbFactory.Base());
                DataItemDetailService data = new DataItemDetailService();
                var pitem = data.GetItemValue("Hikappkey");//密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                List<TemporaryUserEntity> list1 = new List<TemporaryUserEntity>();
                List<ForbiddenRecordEntity> list2 = new List<ForbiddenRecordEntity>();
                foreach (var entity in list)
                {
                    TemporaryUserEntity old = inlogdb.FindEntity(entity.USERID);
                    if (old != null)
                    {
                        old.ISDebar = 1;
                        old.Remark = entity.Remark;
                        old.PassPost = "";
                        old.PassPostId = "";    
                        old.EndTime = entity.EndTime;
                        list1.Add(old);
                        //禁入记录
                        ForbiddenRecordEntity RecordEntity = new ForbiddenRecordEntity();
                        RecordEntity.Create();
                        RecordEntity.StartTime = DateTime.Now;
                        RecordEntity.Remark = entity.Remark;
                        RecordEntity.UserId = old.USERID;
                        list2.Add(RecordEntity);
                    }
                }
                var Invalidentity = DelEquipmentRecord(list, baseurl, Key, Signature);
                taskIds.Add(Invalidentity);
                res.Update<TemporaryUserEntity>(list1);
                res.Insert<ForbiddenRecordEntity>(list2);
                res.Commit();
                downloadUserlimits(Invalidentity.resourceInfos, baseurl, Key, Signature);               
            }
            catch (Exception er)
            {
                res.Rollback();
            }
            return taskIds;
        }


        /// <summary>
        /// 移除禁入名单
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> RemoveForbidden(string list)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            List<AddJurisdictionEntity> taskIds = new List<AddJurisdictionEntity>();
            try
            {
                Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
                Repository<ForbiddenRecordEntity> Frecord = new Repository<ForbiddenRecordEntity>(DbFactory.Base());
                DataItemDetailService data = new DataItemDetailService();
                var pitem = data.GetItemValue("Hikappkey");//密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                List<TemporaryUserEntity> list1 = new List<TemporaryUserEntity>();
                List<ForbiddenRecordEntity> list2 = new List<ForbiddenRecordEntity>();
                foreach (var UserId in list.Split(','))
                {
                    if (string.IsNullOrEmpty(UserId)) continue;
                    TemporaryUserEntity old = inlogdb.FindEntity(UserId);
                    if (old != null)
                    {
                        old.ISDebar = 0;
                        list1.Add(old);
                        //移除禁入记录
                        List<ForbiddenRecordEntity> Rlist = Frecord.IQueryable(it => it.UserId == UserId).OrderByDescending(t => t.CreateDate).ToList();
                        if (Rlist != null)
                        {
                            Rlist[0].EndTime = DateTime.Now;
                            list2.Add(Rlist[0]);
                        }
                    }
                }
                var Invalidentity = UploadUserlimits(list1, baseurl, Key, Signature);
                taskIds.Add(Invalidentity);
                res.Update<TemporaryUserEntity>(list1);
                res.Update<ForbiddenRecordEntity>(list2);
                res.Commit();
            }
            catch (Exception er)
            {
                res.Rollback();
            }
            return taskIds;
        }


        /// <summary>
        /// 批量导入临时人员
        /// </summary>
        public void SaveTempImport(string type, List<TemporaryUserEntity> templist)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                foreach (TemporaryUserEntity entity in templist)
                {
                    entity.Create();
                    entity.Istemporary = 1;
                    entity.ISDebar = 0;
                    list.Add(entity);
                }
                res.Insert<TemporaryUserEntity>(list);
                res.Commit();
                //同步海康
                InsertTempUserHiK(new TemporaryUserEntity(), list);
            }
            catch (Exception er)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 批量保存临时人员
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveTemporaryList(string keyValue, List<TemporaryUserEntity> list)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    res.Update<TemporaryUserEntity>(list);
                    res.Commit();
                }
                else
                {
                    res.Insert<TemporaryUserEntity>(list);
                    res.Commit();
                }
            }
            catch (Exception er)
            {
                res.Rollback();
            }
        }


        /// <summary>
        /// 将临时人员信息上传到海康平台
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        public AddJurisdictionEntity InsertTempUserHiK(TemporaryUserEntity entity, List<TemporaryUserEntity> list)
        {
            DataItemDetailService data = new DataItemDetailService();
            var pitem = data.GetItemValue("Hikappkey");//密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
            string Key = string.Empty;
            string Signature = string.Empty;
            string no = new Random().Next(10, 888888).ToString();
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/single/add";
            foreach (var item in list)
            {
                //人脸信息（base64必须为jpg格式）
                List<FaceEntity> faces = new List<FaceEntity>();
                FaceEntity face = new FaceEntity();
                face.faceData = item.ImgData;
                faces.Add(face);
                string orgcode = "root000000";
                if (item.Istemporary == 0) { orgcode = item.Groupsid; }
                var model = new
                {
                    personId = item.USERID,
                    personName = item.UserName,
                    gender = "1",
                    orgIndexCode = orgcode,
                    birthday = "1990-01-01",
                    phoneNo = item.Tel,
                    email = "person1@person.com",
                    certificateType = "990",
                    certificateNo = no,
                    faces
                };
                string msg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                parkList1 pl = JsonConvert.DeserializeObject<parkList1>(msg);
                if (pl != null && pl.code == "0")
                {
                    SetLoadUserCarNo(item, baseurl, Key, Signature);
                }
            }
            //添加权限
            return UploadUserlimits(list, baseurl, Key, Signature);
        }

        /// <summary>
        /// 给新添加的人员单个分配卡号
        /// </summary>
        private void SetLoadUserCarNo(TemporaryUserEntity Item, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/cis/v1/card/bindings";
            List<cardList1> cardList = new List<cardList1>();
            List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
            list.Add(Item);
            cardList1 entity = new cardList1();
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            var no = Str.PinYin(Item.UserName).ToUpper() + time;//卡号唯一
            if (Item.Istemporary == 0) { no = Item.Tel.Trim(); }//非临时人员
            entity.cardNo = no;
            entity.personId = Item.USERID;
            entity.cardType = 1;
            cardList.Add(entity);
            var model = new
            {
                startDate = Convert.ToDateTime(Item.startTime).ToString("yyyy-MM-dd"),
                endDate = Convert.ToDateTime(Item.EndTime).ToString("yyyy-MM-dd"),
                cardList
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
            /// parkList2 pl = JsonConvert.DeserializeObject<parkList2>(msg);

        }

        /// <summary>
        /// 添加出入权限配置
        /// </summary>
        private AddJurisdictionEntity UploadUserlimits(List<TemporaryUserEntity> list, string baseUrl, string Key, string Signature)
        {
            AddJurisdictionEntity Juentity = new AddJurisdictionEntity();
            if (list.Count > 0)
            {
                var url = "/artemis/api/acps/v1/auth_config/add";
                List<personDatas1> personDatas = new List<personDatas1>();
                personDatas1 entity = new personDatas1();
                List<string> codes = new List<string>();
                foreach (var item in list)
                {//将所有人放集合做一次任务下发
                    codes.Add(item.USERID);//人员Id
                }
                entity.indexCodes = codes;
                entity.personDataType = "person";
                personDatas.Add(entity);
                #region 查询设备唯一标识
                #region 新代码
                if (!string.IsNullOrEmpty(list[0].PassPost))
                {
                    var db = new RepositoryFactory().BaseRepository();
                    var areaArray = list[0].PassPost.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    //获取设备
                    var deviceList = (from area in db.IQueryable<DataItemEntity>()
                                      join parent in db.IQueryable<DataItemEntity>() on area.ParentId equals parent.ItemId
                                      join device in db.IQueryable<DataItemDetailEntity>() on area.ItemId equals device.ItemId
                                      where parent.ItemName == "海康门禁设备" && areaArray.Contains(area.ItemName)
                                      select new { device.ItemName, device.ItemValue, device.ItemCode }).ToList();
                    if (deviceList != null && deviceList.Count > 0)
                    {
                        List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//设备信息集合
                        deviceList.ForEach(x =>
                        {

                            List<int> nos = new List<int>();
                            if (x.ItemCode == "1")
                            {//门禁通道
                                nos.Add(1);
                                nos.Add(2);
                            }
                            else
                            {//门禁
                                nos.Add(1);
                            }
                            resourceInfos1 entity1 = new resourceInfos1();
                            entity1.resourceIndexCode = x.ItemValue;//设备唯一编号
                            entity1.resourceType = "acsDevice";
                            entity1.channelNos = nos;
                            resourceInfos.Add(entity1);
                        });

                        string stime = Convert.ToDateTime(list[0].startTime).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);//ISO8601时间格式
                        string etime = Convert.ToDateTime(list[0].EndTime).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);
                        var model = new
                        {
                            personDatas,
                            resourceInfos,
                            startTime = stime,  // "2019-12-01T17:30:08+08:00",
                            endTime = etime     //"2019-12-19T17:30:08+08:00"
                        };
                        string msgs = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                        JurisdictionEntity p1 = JsonConvert.DeserializeObject<JurisdictionEntity>(msgs);
                        //Juentity.taskId = "54654646465465464654";
                        //Juentity.type = "add";
                        //Juentity.resourceInfos = resourceInfos;
                        if (p1 != null && p1.code == "0")
                        {
                            Juentity.taskId = p1.data.taskId;
                            Juentity.resourceInfos = resourceInfos;
                            Juentity.type = "add";
                        }
                    }
                }
                #endregion


                #region 旧代码
                //        string Qres = string.Empty;
                //if (!string.IsNullOrEmpty(list[0].PassPost))
                //{
                //    string[] post = list[0].PassPost.Split(',');
                //    foreach (var item in post)
                //    {
                //        if (string.IsNullOrEmpty(item)) continue;
                //        switch (item)
                //        {
                //            case "一号岗":
                //                Qres += "'equipment1'" + ",";
                //                break;
                //            case "码头岗":
                //                Qres += "'equipmentmt'" + ",";
                //                break;
                //            case "三号岗":
                //                Qres += "'equipment3'" + ",";
                //                break;
                //            //case "生活区岗":
                //            //    break;
                //            default:
                //                break;
                //        }
                //    }
                //}
                //string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode in ({0}) order by t.sortcode asc", Qres.TrimEnd(','));
                //if (!string.IsNullOrEmpty(Qres))
                //{//授权门岗不能为空
                //    DataTable dt = this.BaseRepository().FindTable(sql);
                //    if (dt.Rows.Count > 0)
                //    {
                //        List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//设备信息集合
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            List<int> nos = new List<int>();
                //            if (dt.Rows[i][2].ToString() == "1")
                //            {//门禁通道
                //                nos.Add(1);
                //                nos.Add(2);
                //            }
                //            else
                //            {//门禁
                //                nos.Add(1);
                //            }
                //            resourceInfos1 entity1 = new resourceInfos1();
                //            entity1.resourceIndexCode = dt.Rows[i][1].ToString();//设备唯一编号
                //            entity1.resourceType = "acsDevice";
                //            entity1.channelNos = nos;
                //            resourceInfos.Add(entity1);
                //        }
                //        string stime = Convert.ToDateTime(list[0].startTime).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);//ISO8601时间格式
                //        string etime = Convert.ToDateTime(list[0].EndTime).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);
                //        var model = new
                //        {
                //            personDatas,
                //            resourceInfos,
                //            startTime = stime,  // "2019-12-01T17:30:08+08:00",
                //            endTime = etime     //"2019-12-19T17:30:08+08:00"
                //        };
                //        string msgs = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                //        JurisdictionEntity p1 = JsonConvert.DeserializeObject<JurisdictionEntity>(msgs);
                //        //Juentity.taskId = "54654646465465464654";
                //        //Juentity.type = "add";
                //        //Juentity.resourceInfos = resourceInfos;
                //        if (p1 != null && p1.code == "0")
                //        {
                //            Juentity.taskId = p1.data.taskId;
                //            Juentity.resourceInfos = resourceInfos;
                //            Juentity.type = "add";
                //        }
                //    }
                //}

                #endregion
                #endregion
            }
            return Juentity;
        }


        /// <summary>
        /// 根据出入权限配置快捷下载(IC卡号、人脸)
        /// </summary>
        public void downloadUserlimits(List<resourceInfos1> resourceInfos, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/acps/v1/authDownload/configuration/shortcut";
            var model = new
            {
                taskType = 5,
                resourceInfos
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
        }


        #endregion

        #region 新增

        /// <summary>
        /// 获取所有临时人员列表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<TemporaryUserEntity> GetUserList()
        {
            string sql = "select userid,ModifyUserId,CreateUserId,ModifyDate,CreateDate,CreateUserOrgCode,CreateUserDeptCode,GroupsName,Groupsid,UserName,Gender,Identifyid,ComName,Tel,startTime,EndTime,PassPost,ISDebar,UserImg,Remark,Istemporary,Postname from BIS_TEMPORARYUSER where ISDebar!=1 ";
            Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
            List<TemporaryUserEntity> old = inlogdb.FindList(sql).ToList();
            return old;
        }

        /// <summary>
        /// 批量录入人脸
        /// </summary>
        public bool SaveFace(List<TemporaryUserEntity> insertList, List<TemporaryUserEntity> updateList, string baseUrl, string Key, string Signature)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string url = "/artemis/api/resource/v1/face/single/add";
                //res.Insert<TemporaryUserEntity>(insertList);
                // 批量导入添加人脸信息
                res.Update<TemporaryUserEntity>(updateList);
                res.Commit();
                //循环调用人脸接口增加
                foreach (var insert in updateList)
                {
                    var model = new
                    {
                        personId = insert.USERID,
                        faceData = insert.ImgData
                    };
                    string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                }
                return true;
            }
            catch (Exception e)
            {
                res.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 单个添加人脸
        /// </summary>
        public void SaveSingleFace(TemporaryUserEntity insert)
        {
            DataItemDetailService data = new DataItemDetailService();
            var pitem = data.GetItemValue("Hikappkey");//密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
            string Key = string.Empty;
            string Signature = string.Empty;
            string url = "/artemis/api/resource/v1/face/single/add";
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }

            var model = new
            {
                personId = insert.USERID,
                faceData = insert.ImgData
            };
            string msg = SocketHelper.LoadCameraList(model, baseurl, url, Key, Signature);
        }

        /// <summary>
        /// 电厂用户批量分配卡号
        /// </summary>
        private void SetLoadUserCarNo1(List<TemporaryUserEntity> list, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/cis/v1/card/bindings";
            List<cardList1> cardList = new List<cardList1>();
            if (list.Count > 0 && list.Count < 50)
            {
                foreach (var Item in list)
                {//批量开卡最大支持50张卡
                    cardList1 entity = new cardList1();
                    entity.cardNo = Item.Tel.Trim();
                    entity.personId = Item.USERID;
                    entity.cardType = 1;
                    cardList.Add(entity);
                }
                var model = new
                {
                    startDate = Convert.ToDateTime(list[0].startTime).ToString("yyyy-MM-dd"),
                    endDate = Convert.ToDateTime(list[0].EndTime).ToString("yyyy-MM-dd"),
                    cardList
                };
                string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                //parkList2 pl = JsonConvert.DeserializeObject<parkList2>(msg);
                //if (pl != null && pl.code == "0")
                //{
                //    //UploadUserlimits(list, baseUrl, Key, Signature);
                //}
            }
        }

        /// <summary>
        /// 获取临时人员实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public TemporaryUserEntity HikGetUserEntity(string keyValue)
        {
            Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
            TemporaryUserEntity old = inlogdb.FindEntity(keyValue);
            return old;
        }

        /// <summary>
        /// 用户权限管理
        /// </summary>
        /// <param name="UserEntity"></param>
        /// <param name="userids"></param>
        /// <param name="type"></param>
        public void SaveUserJurisdiction(TemporaryUserEntity UserEntity, string userids, string type)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //List<TemporaryUserEntity> list1 = new List<TemporaryUserEntity>();
                //if (type == "1")
                //{//门岗权限分配
                //    DataItemDetailService data = new DataItemDetailService();
                //    var pitem = data.GetItemValue("Hikappkey");//密钥
                //    var baseurl = data.GetItemValue("HikBaseUrl");//海康地址
                //    string Key = string.Empty;
                //    string Signature = string.Empty;
                //    if (!string.IsNullOrEmpty(pitem))
                //    {
                //        Key = pitem.Split('|')[0];
                //        Signature = pitem.Split('|')[1];
                //    }
                //    Repository<TemporaryUserEntity> inlogdb = new Repository<TemporaryUserEntity>(DbFactory.Base());
                //    List<TemporaryUserEntity> plist = new List<TemporaryUserEntity>();

                //    string[] ids = userids.Split(',');
                //    foreach (var item in ids)
                //    {
                //        if (string.IsNullOrEmpty(item)) continue;
                //        TemporaryUserEntity old = inlogdb.FindEntity(item);
                //        if (old != null && old.EndTime != null)
                //        {//只需要添加权限配置和快捷下载
                //            old.PassPost = UserEntity.PassPost;
                //            list1.Add(old);
                //            if (old.EndTime != null)
                //            {//权限配置及下发结束时间必填
                //                plist.Add(old);
                //            }
                //        }
                //    }

                //    UploadUserlimits(plist, baseurl, Key, Signature);
                //    res.Update<TemporaryUserEntity>(list1);
                //    res.Commit();
                // }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        #endregion
    }
}
