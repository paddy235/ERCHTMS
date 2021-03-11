using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Service.SystemManage;
using Newtonsoft.Json;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� �����Ž���Ȩ�ޱ�
    /// </summary>
    public class HikaccessaurhorityService : RepositoryFactory<HikaccessaurhorityEntity>, HikaccessaurhorityIService
    {
        #region ��ȡ����
        /// <summary>
        /// Ȩ���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            if (!queryParam["AreaId"].IsEmpty())
            {
                string AreaId = queryParam["AreaId"].ToString();
                pagination.conditionJson += string.Format(" and AreaId = '{0}'", AreaId);
            }

            //�豸����
            if (!queryParam["DeviceName"].IsEmpty())
            {
                string DeviceName = queryParam["DeviceName"].ToString();

                pagination.conditionJson += string.Format(" and DeviceName like '%{0}%'", DeviceName);

            }
            //��������
            if (!queryParam["OutType"].IsEmpty())
            {
                int OutType = Convert.ToInt32(queryParam["OutType"]);

                pagination.conditionJson += string.Format(" and OutType ={0}", OutType);

            }
            //�豸�����Ÿ�
            if (!queryParam["AreaName"].IsEmpty())
            {
                string AreaName = queryParam["AreaName"].ToString();
                pagination.conditionJson += string.Format(" and AreaName like '%{0}%'", AreaName);
            }

            //�豸�����Ÿ�
            if (!queryParam["Type"].IsEmpty())
            {
                string Type = queryParam["Type"].ToString();
                pagination.conditionJson += string.Format(" and Type = {0}", Type);
            }

            //�豸IP
            if (!queryParam["RID"].IsEmpty())
            {
                string RID = queryParam["RID"].ToString();
                pagination.conditionJson += string.Format(" and RID = '{0}'", RID);
            }
            else//�������id���ܲ������
            {
                pagination.conditionJson += string.Format(" and 1=2");
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HikaccessaurhorityEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikaccessaurhorityEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue, string pitem, string url)
        {
            var item = BaseRepository().FindEntity(keyValue);

            this.BaseRepository().Delete(keyValue);
            List<string> userids = new List<string>();
            List<HikAccessNo> AccessIds = new List<HikAccessNo>();
            List<HikAccessNo> parentList = new List<HikAccessNo>();
            userids.Add(item.RID);
            HikAccessNo acc = new HikAccessNo();
            List<int> accno = new List<int>
            {
                item.HikNos
            };
            acc.AccId = item.HikId;
            acc.No = accno;
            AccessIds.Add(acc);
            HikAccessNo Par = new HikAccessNo();
            List<int> Parno = new List<int>
            {
                item.HikNos
            };
            Par.AccId = item.ParentId;
            Par.No = Parno;
            parentList.Add(Par);
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }
            //���ú���ɾ���ӿ�
            DeleteUserlimits(userids, parentList, AccessIds, item.Type, url, key, sign);
        }
        /// <summary>
        /// �����û�ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveUserForm(string keyValue, string pitem, string url)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<string> userids = new List<string>();
                List<HikAccessNo> AccessIds = new List<HikAccessNo>();
                List<HikAccessNo> parentList = new List<HikAccessNo>();
                string searchsql = string.Format("select * from bis_Hikaccessaurhority where rid in ({0})", keyValue);
                //��ѯɾ���û�Ȩ���������ڽӴ�����ƽ̨Ȩ��
                Repository<HikaccessaurhorityEntity> inlogdb = new Repository<HikaccessaurhorityEntity>(DbFactory.Base());
                List<HikaccessaurhorityEntity> deviceList = inlogdb.FindList(searchsql).ToList();
                if (deviceList.Count > 0)
                {
                    foreach (var item in deviceList)
                    {
                        if (!userids.Contains(item.RID))
                        {
                            userids.Add(item.RID);
                        }

                        if (AccessIds.Where(it => it.AccId == item.HikId).Count() == 0)
                        {
                            HikAccessNo hn = new HikAccessNo();
                            List<int> no = new List<int>();
                            no.Add(item.HikNos);
                            hn.AccId = item.HikId;
                            hn.No = no;
                            AccessIds.Add(hn);
                        }
                        var pa = parentList.Where(it => it.AccId == item.ParentId).FirstOrDefault();
                        if (pa == null)
                        {
                            HikAccessNo hn = new HikAccessNo();
                            List<int> no = new List<int>();
                            no.Add(item.HikNos);
                            hn.AccId = item.ParentId;
                            hn.No = no;
                            parentList.Add(hn);
                        }
                        else
                        {
                            pa.No.Add(item.HikNos);
                        }
                    }
                    string key = string.Empty;// "21049470";
                    string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                    if (!string.IsNullOrEmpty(pitem))
                    {
                        key = pitem.Split('|')[0];
                        sign = pitem.Split('|')[1];
                    }
                    //���ú���ɾ���ӿ�
                    DeleteUserlimits(userids, parentList, AccessIds, 1, url, key, sign);
                }

                string delaurhsql = string.Format("delete from bis_Hikaccessaurhority where rid in ({0})", keyValue);
                res.ExecuteBySql(delaurhsql);
                string sql = string.Format("delete from BIS_HIKACCESSUSER where USERID in ({0})", keyValue);
                res.ExecuteBySql(sql);
                //�ύɾ��
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string StartTime, string EndTime, List<Access> DeptList, List<Access> AccessList, int Type, string pitem, string url)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<string> ids = new List<string>();
                List<HikAccessNo> AccList = new List<HikAccessNo>();
                List<HikAccessNo> ParentList = new List<HikAccessNo>();
                //��ѯ�����Ž����豸����
                Repository<HikaccessEntity> inlogdb = new Repository<HikaccessEntity>(DbFactory.Base());
                List<HikaccessEntity> deviceList = inlogdb.FindList("select * from bis_hikaccess").ToList();

                //��ѯ�Ѽ������Ա����
                Repository<HikaccessuserEntity> Hikuserdb = new Repository<HikaccessuserEntity>(DbFactory.Base());
                List<HikaccessuserEntity> HikuserList = Hikuserdb.FindList("select * from bis_hikaccessuser").ToList();

                string rid = "";
                foreach (var dept in DeptList)
                {
                    if (rid == "")
                    {
                        rid += "'" + dept.id + "'";
                    }
                    else
                    {
                        rid += ",'" + dept.id + "'";
                    }
                }

                string delid = "";//�ظ���ӵ��Ž��ڵ�id

                //��ѯ�Ѿ��ӹ����Ž�������
                Repository<HikaccessaurhorityEntity> Hikaurhoritydb = new Repository<HikaccessaurhorityEntity>(DbFactory.Base());
                string sql = string.Format("select * from bis_Hikaccessaurhority where type={0} and RID in({1})", Type,
                    rid);
                List<HikaccessaurhorityEntity> Hikaccessaurhority = Hikaurhoritydb.FindList(sql).ToList();

                List<HikaccessaurhorityEntity> aurhorityList = new List<HikaccessaurhorityEntity>();
                List<HikaccessuserEntity> userList = new List<HikaccessuserEntity>();
                foreach (var dept in DeptList)
                {
                    ids.Add(dept.id);
                    //�������Ա �򽫲������б��е���Ա��ӽ�ȥ
                    if (Type == 1)
                    {
                        if (HikuserList.Where(it => it.UserId == dept.id).FirstOrDefault() == null)
                        {
                            HikaccessuserEntity user = new HikaccessuserEntity();
                            user.UserId = dept.id;
                            user.UserName = dept.text;
                            user.Create();
                            userList.Add(user);
                        }
                    }
                    foreach (var acc in AccessList)
                    {

                        HikaccessEntity device = deviceList.Where(it => it.ID == acc.id).FirstOrDefault();
                        if (device != null)
                        {
                            int hiknos = 1;
                            if (AccList.Where(it => it.AccId == device.HikId).Count() == 0)
                            {
                                HikAccessNo hn = new HikAccessNo();
                                List<int> no = new List<int>();
                                no.Add(device.channelNos);
                                hn.AccId = device.HikId;
                                hn.No = no;
                                hiknos = device.channelNos;
                                AccList.Add(hn);
                            }
                            var pa = ParentList.Where(it => it.AccId == device.ParentId).FirstOrDefault();
                            if (pa == null)
                            {
                                HikAccessNo hn = new HikAccessNo();
                                List<int> no = new List<int>();
                                no.Add(device.channelNos);
                                hn.AccId = device.ParentId;
                                hn.No = no;
                                ParentList.Add(hn);
                            }
                            else
                            {
                                pa.No.Add(device.channelNos);
                            }

                            HikaccessaurhorityEntity aur = Hikaccessaurhority.Where(it => it.RID == dept.id && it.HikId == device.HikId && Type == Type).FirstOrDefault();
                            if (aur != null)
                            {
                                if (delid == "")
                                {
                                    delid = "'" + aur.ID + "'";
                                }
                                else
                                {
                                    delid += ",'" + aur.ID + "'";
                                }
                            }

                            HikaccessaurhorityEntity aurhority = new HikaccessaurhorityEntity();
                            aurhority.AreaId = device.AreaId;
                            aurhority.AreaName = device.AreaName;
                            aurhority.DeviceName = device.DeviceName;
                            aurhority.HikId = device.HikId;
                            aurhority.OutType = device.OutType;
                            aurhority.ParentId = device.ParentId;
                            aurhority.RID = dept.id;
                            aurhority.RName = dept.text;
                            aurhority.Type = Type;
                            aurhority.StartTime = Convert.ToDateTime(StartTime);
                            aurhority.EndTime = Convert.ToDateTime(EndTime);
                            aurhority.HikNos = hiknos;
                            aurhority.Create();
                            aurhorityList.Add(aurhority);
                        }
                    }


                }

                //��ɾ����ǰ��Ȩ��
                if (delid != "")
                {
                    string delsql = string.Format("delete from bis_Hikaccessaurhority where id in ({0})", delid);
                    res.ExecuteBySql(delsql);
                }

                if (userList.Count > 0)
                {
                    res.Insert<HikaccessuserEntity>(userList);
                }

                if (aurhorityList.Count > 0)
                {
                    res.Insert<HikaccessaurhorityEntity>(aurhorityList);
                }

                res.Commit();
                string key = string.Empty;// "21049470";
                string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                if (!string.IsNullOrEmpty(pitem))
                {
                    key = pitem.Split('|')[0];
                    sign = pitem.Split('|')[1];
                }
                //���ú����ӿ��·�Ȩ��
                UploadUserlimits(ids, ParentList, AccList, StartTime, EndTime, Type, url, key, sign);

            }
            catch (Exception e)
            {
                res.Rollback();
            }
        }


        #endregion

        #region ���ú���Ȩ�޽ӿ�


        /// <summary>
        /// ����Ȩ������
        /// </summary>
        /// <param name="codes">�û�����֯id����</param>
        /// <param name="ParentAccess">������id����</param>
        /// <param name="Access">�Ž���id����</param>
        /// <param name="StartTime">��ʼʱ��</param>
        /// <param name="EndTime">����ʱ��</param>
        /// <param name="Type">0��֯ 1��Ա</param>
        /// <param name="baseUrl"></param>
        /// <param name="Key"></param>
        /// <param name="Signature"></param>
        private void UploadUserlimits(List<string> codes, List<HikAccessNo> ParentAccess, List<HikAccessNo> Access, string StartTime, string EndTime, int Type, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/acps/v1/auth_config/add";
            List<personDatas1> personDatas = new List<personDatas1>();
            personDatas1 entity = new personDatas1();
            entity.indexCodes = codes;
            if (Type == 0)
            {
                entity.personDataType = "org";
            }
            else
            {
                entity.personDataType = "person";
            }
            personDatas.Add(entity);

            List<resourceInfos1> AllresourceInfos = new List<resourceInfos1>();//�豸��Ϣ����
            List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//�豸��Ϣ����
            for (int i = 0; i < ParentAccess.Count; i++)
            {

                resourceInfos1 entity1 = new resourceInfos1();
                entity1.resourceIndexCode = ParentAccess[i].AccId;//�豸Ψһ���
                entity1.resourceType = "acsDevice";
                entity1.channelNos = ParentAccess[i].No;
                AllresourceInfos.Add(entity1);
            }
            //�����Ž��������̶�Ϊ�豸�����Ž������Ͳ�ȡ���ݱ�������
            DataItemDetailService itemBll = new DataItemDetailService();
            string doorType = itemBll.GetItemValue("doorType");
            foreach (var item in Access)
            {
                resourceInfos1 entity1 = new resourceInfos1();
                entity1.resourceIndexCode = item.AccId;//�豸Ψһ���
                entity1.resourceType = doorType;
                entity1.channelNos = item.No;
                AllresourceInfos.Add(entity1);
                resourceInfos.Add(entity1);
            }
            string stime = Convert.ToDateTime(StartTime).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);//ISO8601ʱ���ʽ
            string etime = Convert.ToDateTime(EndTime).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);
            var model = new
            {
                personDatas,
                resourceInfos = AllresourceInfos,
                startTime = stime,  // "2019-12-01T17:30:08+08:00",
                endTime = etime     //"2019-12-19T17:30:08+08:00"
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
            var tsk = JsonConvert.DeserializeObject<JurisdictionEntity>(msg);
            bool flag = QuerySpeedofprogress(tsk.data.taskId, baseUrl, Key, Signature);
            if (flag)
            {
                //�Ƚ������·��������豸
                downloadUserlimits(AllresourceInfos, 1, baseUrl, Key, Signature);
                //�ٽ�ָ�Ƶ����·����豸
                downloadUserlimits(resourceInfos, 2, baseUrl, Key, Signature);
            }

        }

        /// <summary>
        /// ���ݳ���Ȩ�����ÿ������
        /// </summary>
        /// <param name="resourceInfos"></param>
        /// <param name="Type">1����2ָ��4����</param>
        /// <param name="baseUrl"></param>
        /// <param name="Key"></param>
        /// <param name="Signature"></param>
        private void downloadUserlimits(List<resourceInfos1> resourceInfos, int Type, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/acps/v1/authDownload/configuration/shortcut";
            var model = new
            {
                taskType = Type,
                resourceInfos
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
        }
        /// <summary>
        /// �ж������Ƿ�������
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="baseUrl"></param>
        /// <param name="Key"></param>
        /// <param name="Signature"></param>
        /// <returns></returns>
        private bool QuerySpeedofprogress(string taskid, string baseUrl, string Key, string Signature)
        {
            int i = 0;
            bool falg = false;
            //���ֻѭ��100��������������ѭ��
            while (i < 100)
            {
                string msg1 = SocketHelper.QuerySpeedofprogress(taskid, baseUrl, Key, Signature);
                progress p1 = JsonConvert.DeserializeObject<progress>(msg1);
                if (p1 != null)
                {
                    if (!p1.data.isFinished || p1.data.percent != 100)
                    {
                        falg = false;
                        i++;
                        continue;
                    }
                    else
                    {
                        falg = true;
                        i = 100;
                        break;
                    }
                }
                i++;
            }

            return falg;

        }

        /// <summary>
        /// ����Ȩ������
        /// </summary>
        /// <param name="codes">�û�����֯id����</param>
        /// <param name="ParentAccess">������id����</param>
        /// <param name="Access">�Ž���id����</param>
        /// <param name="StartTime">��ʼʱ��</param>
        /// <param name="EndTime">����ʱ��</param>
        /// <param name="Type">0��֯ 1��Ա</param>
        /// <param name="baseUrl"></param>
        /// <param name="Key"></param>
        /// <param name="Signature"></param>
        private void DeleteUserlimits(List<string> codes, List<HikAccessNo> ParentAccess, List<HikAccessNo> Access, int Type, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/acps/v1/auth_config/delete";
            List<personDatas1> personDatas = new List<personDatas1>();
            personDatas1 entity = new personDatas1();
            entity.indexCodes = codes;
            if (Type == 0)
            {
                entity.personDataType = "org";
            }
            else
            {
                entity.personDataType = "person";
            }
            personDatas.Add(entity);

            List<resourceInfos1> AllresourceInfos = new List<resourceInfos1>();//�豸��Ϣ����
            for (int i = 0; i < ParentAccess.Count; i++)
            {
                //List<int> nos = new List<int>();
                //nos.Add(1);
                //nos.Add(2);


                resourceInfos1 entity1 = new resourceInfos1();
                entity1.resourceIndexCode = ParentAccess[i].AccId;//�豸Ψһ���
                entity1.resourceType = "acsDevice";
                entity1.channelNos = ParentAccess[i].No;
                AllresourceInfos.Add(entity1);
            }
            //�����Ž��������̶�Ϊ�豸�����Ž������Ͳ�ȡ���ݱ�������
            DataItemDetailService itemBll = new DataItemDetailService();
            string doorType = itemBll.GetItemValue("doorType");
            foreach (var item in Access)
            {
                //List<int> nos = new List<int>();
                //nos.Add(1);
                resourceInfos1 entity1 = new resourceInfos1();
                entity1.resourceIndexCode = item.AccId;//�豸Ψһ���
                entity1.resourceType = doorType;
                entity1.channelNos = item.No;
                AllresourceInfos.Add(entity1);
            }
            var model = new
            {
                personDatas,
                resourceInfos = AllresourceInfos
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
            var tsk = JsonConvert.DeserializeObject<JurisdictionEntity>(msg);
            bool flag = QuerySpeedofprogress(tsk.data.taskId, baseUrl, Key, Signature);
            if (flag)
            {
                //��ɾ�������·��������豸
                downloadUserlimits(AllresourceInfos, 1, baseUrl, Key, Signature);
            }

        }

        #endregion
    }
}
