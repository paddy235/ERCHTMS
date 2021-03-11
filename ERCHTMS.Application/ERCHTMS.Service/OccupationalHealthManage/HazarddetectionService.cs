using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ��Σ�����ؼ��
    /// </summary>
    public class HazarddetectionService : RepositoryFactory<HazarddetectionEntity>, HazarddetectionIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazarddetectionEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazarddetectionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["riskid"].IsEmpty())//Σ������
            {
                string riskid = queryParam["riskid"].ToString();
                pagination.conditionJson += string.Format(" and RISKID  ='{0}'", riskid.Trim());
                pagination.sidx = "NLSSORT(SUBSTR(AREAVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME";//ɸѡΣ�����غ� �Ȱ�����������ĸ���� �ڰ���ʱ������
            }
            if (!queryParam["areaid"].IsEmpty())//����
            {
                string areaid = queryParam["areaid"].ToString();
                pagination.conditionJson += string.Format(" and AREAID  ='{0}'", areaid.Trim());
                pagination.sidx = "NLSSORT(SUBSTR(RISKVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME";//ɸѡΣ�����غ� �Ȱ�����������ĸ���� �ڰ���ʱ������
            }
            if (!queryParam["starttime"].IsEmpty() && !queryParam["endtime"].IsEmpty())//ʱ�䷶Χ
            {
                string starttime = queryParam["starttime"].ToString();
                string endtime = queryParam["endtime"].ToString();
                pagination.conditionJson += string.Format(" and STARTTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') and ENDTIME  >= TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim(), starttime.Trim());
            }
            else if (!queryParam["starttime"].IsEmpty()) //ֻ�п�ʼʱ��
            {
                string starttime = queryParam["starttime"].ToString();
                pagination.conditionJson += string.Format(" and STARTTIME  >= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime.Trim());
            }
            else if (!queryParam["endtime"].IsEmpty())//ֻ�н���ʱ��
            {
                string endtime = queryParam["endtime"].ToString();
                pagination.conditionJson += string.Format("  and ENDTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim());
            }
            if (!queryParam["isexcessive"].IsEmpty())//�Ƿ񳬱�
            {
                string isexcessive = queryParam["isexcessive"].ToString();
                pagination.conditionJson += string.Format(" and ISEXCESSIVE  ={0}", isexcessive.Trim());
            }
            if (!queryParam["detectionuserid"].IsEmpty())//�����id
            {
                string detectionuserid = queryParam["detectionuserid"].ToString();
                pagination.conditionJson += string.Format(" and DETECTIONUSERID  ='{0}'", detectionuserid.Trim());
            }
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string con = queryParam["condition"].ToString();
                if (con == "month")
                {
                    string month = Convert.ToDateTime(queryParam["keyword"]).ToString("yyyy-MM");
                    pagination.conditionJson += string.Format(" and TO_CHAR(ENDTIME,'yyyy-mm')  = '{0}' and ISEXCESSIVE=1", month.Trim());
                }

            }



            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <param name="riskid">Σ������</param>
        /// <param name="areaid">����</param>
        /// <param name="starttime">ʱ�䷶Χ</param>
        /// <param name="endtime">ʱ�䷶Χ</param>
        /// <param name="isexcessive">�Ƿ񳬱�</param>
        /// <param name="detectionuserid">�����id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string queryJson, string where)
        {
            string order = " order by ENDTIME desc";
            DatabaseType dataType = DbHelper.DbType;
            string sql = "SELECT HID,AREAVALUE,RISKVALUE,LOCATION,STARTTIME,to_char(ENDTIME,'yyyy-mm-dd hh24:mi:ss') as ENDTIME,STANDARD,DETECTIONUSERNAME,ISEXCESSIVE FROM BIS_HAZARDDETECTION WHERE 1=1";

            var queryParam = queryJson.ToJObject();
            string riskid = "";
            string areaid = "";
            string starttime = "";
            string endtime = "";
            string isexcessive = "";
            string detectionuserid = "";
            //��ѯ����
            if (!queryParam["riskid"].IsEmpty())//Σ������
            {
                riskid = queryParam["riskid"].ToString();

            }
            if (!queryParam["areaid"].IsEmpty())//����
            {
                areaid = queryParam["areaid"].ToString();

            }
            if (!queryParam["starttime"].IsEmpty())//ʱ�䷶Χ
            {
                starttime = queryParam["starttime"].ToString();
            }
            if (!queryParam["endtime"].IsEmpty()) 
            {
                endtime = queryParam["endtime"].ToString();
            }
           
            if (!queryParam["isexcessive"].IsEmpty())//�Ƿ񳬱�
            {
                isexcessive = queryParam["isexcessive"].ToString();

            }
            if (!queryParam["detectionuserid"].IsEmpty())//�����id
            {
                detectionuserid = queryParam["detectionuserid"].ToString();
            }

            //��ѯ����
            if (!riskid.IsEmpty())//Σ������
            {
                sql += string.Format(" and RISKID  ='{0}'", riskid.Trim());
                order = " order by NLSSORT(SUBSTR(AREAVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!areaid.IsEmpty())//����
            {
                sql += string.Format(" and AREAID  ='{0}'", areaid.Trim());
                order = " order by NLSSORT(SUBSTR(RISKVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!starttime.IsEmpty() && !endtime.IsEmpty())//ʱ�䷶Χ
            {
                sql += string.Format(" and STARTTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')  and ENDTIME  >= TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim(), starttime.Trim());
            }
            else if (!starttime.IsEmpty()) 
            {
                sql += string.Format(" and STARTTIME  >= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime.Trim());
            }
            else if (!endtime.IsEmpty()) 
            {
                sql += string.Format("  and ENDTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim());
            }
            if (!isexcessive.IsEmpty())//�Ƿ񳬱�
            {
                sql += string.Format(" and ISEXCESSIVE  ={0}", isexcessive.Trim());
            }
            if (!detectionuserid.IsEmpty())//�����id
            {
                sql += string.Format(" and DETECTIONUSERID  ='{0}'", detectionuserid.Trim());
            }

            sql += where;

            sql += order;



            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// ��ȡ����ָ�꼰��׼
        /// </summary>
        /// <param name="RiskId">ְҵ��id</param>
        /// <returns></returns>
        public string GetStandard(string RiskId, string where)
        {
            string Sql = "SELECT STANDARD FROM BIS_HAZARDDETECTION WHERE";

            Sql += string.Format(" RISKID = '{0}'", RiskId);

            Sql += where;

            Sql += string.Format(" order by CREATEDATE desc");

            object obj = this.BaseRepository().FindObject(Sql);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ��ȡΣ�����ؼ��ͳ������
        /// </summary>
        /// <param name="year">��һ������</param>
        /// <param name="risk">ְҵ������</param>
        /// <param name="type">true��ѯȫ�� false��ѯ��������</param>
        /// <returns></returns>
        public DataTable GetStatisticsHazardTable(int year, string risk, bool type, string where)
        {
            string Sql = @"select to_char(HAZ.ENDTIME,'mm') as time,count(1) from BIS_HAZARDDETECTION HAZ where 
                            1=1";

            if (year != null)
            {
                Sql += string.Format(" AND to_char(HAZ.ENDTIME,'yyyy')='{0}'", year);
            }
            if (!type)//�Ƿ��ѯȫ��
            {
                Sql += string.Format("  AND HAZ.ISEXCESSIVE=1");
            }
            if (risk != null && risk != "")
            {
                Sql += string.Format("  AND HAZ.RISKID={0}", risk);
            }
            Sql += where;
            Sql += string.Format("  group by to_char(HAZ.ENDTIME,'mm') order by to_char(HAZ.ENDTIME,'mm') asc");
            return this.BaseRepository().FindTable(Sql);
        }


        #endregion

        #region �ύ����
        /// <summary>
        /// ����id��������ɾ��
        /// </summary>
        /// <param name="Ids"></param>
        public void Remove(string Ids)
        {
            string[] id = Ids.Split(',');
            this.BaseRepository().Delete(it => id.Contains(it.HId));
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
        public void SaveForm(string keyValue, HazarddetectionEntity entity)
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
        #endregion

        #region �ֻ���
        /// <summary>
        /// ����Σ�����ؼ������
        /// </summary>
        /// <param name="assess">ʵ��</param>
        /// <param name="user">��ǰ�û�</param>
        /// <returns></returns>
        public int SaveHazard(HazarddetectionEntity hazard, ERCHTMS.Code.Operator user)
        {
            string sql = "";
            if (string.IsNullOrEmpty(hazard.HId))
            {
                sql = string.Format(@"insert into BIS_HAZARDDETECTION(
                HID,AREAID,AREAVALUE,RISKID,RISKVALUE,LOCATION,STARTTIME,ENDTIME,STANDARD,DETECTIONUSERID,DETECTIONUSERNAME,
                ISEXCESSIVE,CREATEUSERID,CREATEDATE,CREATEUSERDEPTCODE,CREATEUSERORGCODE) values(
                '{0}','{1}','{2}','{3}','{4}','{5}',to_timestamp('{6}','yyyy-mm-dd hh24:mi:ss'),to_timestamp('{7}','yyyy-mm-dd hh24:mi:ss'),'{8}','{9}','{10}',{11},'{12}',to_timestamp('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}')", Guid.NewGuid().ToString(), hazard.AreaId, hazard.AreaValue, hazard.RiskId, hazard.RiskValue, hazard.Location, hazard.StartTime, hazard.EndTime, hazard.Standard, hazard.DetectionUserId, hazard.DetectionUserName, hazard.IsExcessive, user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.DeptCode, user.OrganizeCode);
            }
            else
            {
                sql = string.Format(@"update BIS_HAZARDDETECTION set 
                AREAID='{1}',AREAVALUE='{2}',RISKID='{3}',RISKVALUE='{4}',LOCATION='{5}',
                STARTTIME=to_timestamp('{6}','yyyy-mm-dd hh24:mi:ss'),to_timestamp('{7}','yyyy-mm-dd hh24:mi:ss'),STANDARD='{8}',DETECTIONUSERID='{9}',DETECTIONUSERNAME='{10}',
                ISEXCESSIVE='{11}',MODIFYUSERID={12},MODIFYDATE=to_timestamp('{13}','yyyy-mm-dd hh24:mi:ss') where HID='{0}'", hazard.HId, hazard.AreaId, hazard.AreaValue, hazard.RiskId, hazard.RiskValue, hazard.Location, hazard.StartTime, hazard.EndTime, hazard.Standard, hazard.DetectionUserId, hazard.DetectionUserName, hazard.IsExcessive, user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            return this.BaseRepository().ExecuteBySql(sql);
        }


        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <param name="riskid">Σ������</param>
        /// <param name="areaid">����</param>
        /// <param name="starttime">ʱ�䷶Χ</param>
        /// <param name="endtime">ʱ�䷶Χ</param>
        /// <param name="isexcessive">�Ƿ񳬱�</param>
        /// <param name="detectionuserid">�����id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string riskid, string areaid, string starttime, string endtime, string isexcessive, string detectionuserid, string where)
        {
            string order = " order by ENDTIME desc";
            DatabaseType dataType = DbHelper.DbType;
            string sql = "SELECT HID,AREAVALUE,RISKVALUE,LOCATION,to_char(STARTTIME,'yyyy-mm-dd') as STARTTIME,to_char(ENDTIME,'yyyy-mm-dd') as ENDTIME,STANDARD,DETECTIONUSERNAME,ISEXCESSIVE FROM BIS_HAZARDDETECTION WHERE 1=1";

            //��ѯ����
            if (!riskid.IsEmpty())//Σ������
            {
                sql += string.Format(" and RISKID  ='{0}'", riskid.Trim());
                order = " order by NLSSORT(SUBSTR(AREAVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!areaid.IsEmpty())//����
            {
                sql += string.Format(" and AREAID  ='{0}'", areaid.Trim());
                order = " order by NLSSORT(SUBSTR(RISKVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!starttime.IsEmpty() && !endtime.IsEmpty())//ʱ�䷶Χ
            {
                sql += string.Format(" and STARTTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')  and ENDTIME  >= TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim(), starttime.Trim());
            }
            else if (!starttime.IsEmpty())
            {
                sql += string.Format(" and STARTTIME  >= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime.Trim());
            }
            else if (!endtime.IsEmpty())
            {
                sql += string.Format("  and ENDTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim());
            }

            if (!isexcessive.IsEmpty())//�Ƿ񳬱�
            {
                sql += string.Format(" and ISEXCESSIVE  ={0}", isexcessive.Trim());
            }
            if (!detectionuserid.IsEmpty())//�����id
            {
                sql += string.Format(" and DETECTIONUSERID  ='{0}'", detectionuserid.Trim());
            }

            sql += where;

            sql += order;

            return this.BaseRepository().FindTable(sql);
        }
        #endregion
    }
}
