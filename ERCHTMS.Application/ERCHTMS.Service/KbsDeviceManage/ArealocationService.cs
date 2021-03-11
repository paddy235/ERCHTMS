using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// �� ��������λ��
    /// </summary>
    public class ArealocationService : RepositoryFactory<ArealocationEntity>, ArealocationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ArealocationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡ��������(������)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaColor> GetRiskTable()
        {
            string sql = string.Format(
                "select districtid,parentid,districtcode,districtname,NVL(id,'') id,pointlist,modelids,0 \"Level\",0 HtNum from bis_District dis left join bis_arealocation  area on dis.districtid=area.areaid order by SortCode");
            Repository<KbsAreaColor> inlogdb = new Repository<KbsAreaColor>(DbFactory.Base());
            List<KbsAreaColor> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        public List<AreaHiddenCount> GetHiddenCount()
        {
            string sql = string.Format(
                "select HIDPOINT areacode,HIDPOINTNAME areaname,count(1) htcount from V_BASEHIDDENINFO t  where HIDPOINT is not null  group by HIDPOINT,HIDPOINTNAME");
            Repository<AreaHiddenCount> inlogdb = new Repository<AreaHiddenCount>(DbFactory.Base());
            List<AreaHiddenCount> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }

        /// <summary>
        /// ��ȡ�����������������
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetTable()
        {
            string sql = string.Format(
                @"select districtid,parentid,districtcode,districtname,NVL(id,'') id,pointlist,modelids from bis_District dis
            left join bis_arealocation  area on dis.districtid=area.areaid order by SortCode");
            Repository<KbsAreaLocation> inlogdb = new Repository<KbsAreaLocation>(DbFactory.Base());
            List<KbsAreaLocation> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }
        /// <summary>
        /// ��ȡ�����������������(һ������)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetOneLevelTable()
        {
            string sql = string.Format(
                @"select districtid,parentid,districtcode,districtname,NVL(id,'') id,pointlist,modelids from bis_District dis
            left join bis_arealocation  area on dis.districtid=area.areaid where dis.parentid='0' order by SortCode");
            Repository<KbsAreaLocation> inlogdb = new Repository<KbsAreaLocation>(DbFactory.Base());
            List<KbsAreaLocation> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ArealocationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ArealocationEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
