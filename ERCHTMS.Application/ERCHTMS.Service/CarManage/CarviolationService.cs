using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.MatterManage;
using System;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� ����Υ����Ϣ��
    /// </summary>
    public class CarviolationService : RepositoryFactory<CarviolationEntity>, CarviolationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarviolationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //���������ID
            if (!queryParam["cid"].IsEmpty())
            {
                string cid = queryParam["cid"].ToString();

                pagination.conditionJson += string.Format(" and cid = '{0}'", cid);

            }
            //��ʻ��
            if (!queryParam["Dirver"].IsEmpty())
            {
                string Dirver = queryParam["Dirver"].ToString();
                pagination.conditionJson += string.Format(" and Dirver like '%{0}%'", Dirver);
            }
            //�绰����
            if (!queryParam["Phone"].IsEmpty())
            {
                string Phone = queryParam["Phone"].ToString();
                pagination.conditionJson += string.Format(" and Phone  like '{0}%'", Phone);
            }

            //��������
            if (!queryParam["CarType"].IsEmpty())
            {
                string CarType = queryParam["CarType"].ToString();
                pagination.conditionJson += string.Format(" and violationtype = {0}", CarType);
            }

            if (!queryParam["condition"].IsEmpty())
            {
                //���ƺ�
                if (queryParam["condition"].ToString() == "CarNo")
                {
                    string CarNo = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and cardno like '%{0}%'", CarNo);
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarviolationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// Ԥ����������
        /// </summary>
        /// <returns></returns>
        public List<CarviolationEntity> GetIndexWaring()
        {
            var wcl = BaseRepository().IQueryable(x => x.IsProcess == 0).OrderByDescending(x => x.CreateDate).ToList();
            //�Ѵ���ֻȡ�����
            var startDate = DateTime.Now.Date;
            var endData = startDate.AddDays(1);
            var ycl = BaseRepository().IQueryable(x => x.IsProcess == 1 && x.CreateDate >= startDate && x.CreateDate < endData).ToList();
            wcl.AddRange(ycl);
            return wcl;
        }

        /// <summary>
        /// Ԥ����������ͳ��
        /// </summary>
        /// <returns></returns>
        public object GetIndexWaringCount()
        {
            var startDate = DateTime.Now.Date;
            var endData = startDate.AddDays(1);
            //�Ѵ���ȡ����
            int count1 = BaseRepository().IQueryable(x => x.CreateDate >= startDate && x.CreateDate < endData && x.IsProcess == 1).Count();
            int count2 = BaseRepository().IQueryable(x => x.IsProcess == 0).Count();
            return new { YCL = count1, WCL = count2 };
        }

        /// <summary>
        /// ��ȡ���е�δ����Ķ���
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<CarviolationEntity> GetUntreatedWaringList(Pagination pagination, string queryJson)
        {
            var query = BaseRepository().IQueryable(x => x.IsProcess == 0);

            var data = query.OrderByDescending(x => x.CreateDate).Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
            return data;
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
        public void SaveForm(string keyValue, CarviolationEntity entity)
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
        /// ����һ��Υ�½ӿ�
        /// </summary>
        public void AddViolation(string id, int type, int ViolationType,string ViolationMsg)
        {
            CarviolationEntity entity=new CarviolationEntity();

            //0Ϊ�糧�೵ 1Ϊ˽�ҳ� 2Ϊ���񹫳� 3Ϊ�ݷó��� 4Ϊ���ϳ��� 5ΪΣ��Ʒ����
            switch (type)
            {
                case 0:
                case 1:
                case 2:
                    Repository<CarinfoEntity> inlogdbC = new Repository<CarinfoEntity>(DbFactory.Base());
                    CarinfoEntity car=inlogdbC.FindEntity(id);
                    entity.CardNo = car.CarNo;
                    entity.Dirver = car.Dirver;
                    entity.Phone = car.Phone;
                    break;
                case 3:
                    Repository<VisitcarEntity> inlogdbv = new Repository<VisitcarEntity>(DbFactory.Base());
                    VisitcarEntity oldv = inlogdbv.FindEntity(id);
                    entity.CardNo = oldv.CarNo;
                    entity.Dirver = oldv.Dirver;
                    entity.Phone = oldv.Phone;
                    break;
                case 4:
                    Repository<OperticketmanagerEntity> inlogdbo = new Repository<OperticketmanagerEntity>(DbFactory.Base());
                    OperticketmanagerEntity oldo = inlogdbo.FindEntity(id);
                    entity.CardNo = oldo.Platenumber;
                    entity.Dirver = oldo.DriverName;
                    entity.Phone = oldo.DriverTel;
                    break;
                case 5:
                    Repository<HazardouscarEntity> inlogdb = new Repository<HazardouscarEntity>(DbFactory.Base());
                    HazardouscarEntity old = inlogdb.FindEntity(id);
                   
                    entity.CardNo = old.CarNo;
                    entity.Dirver = old.Dirver;
                    entity.Phone = old.Phone;
                    break;
            }
            entity.CarType = type;
            entity.CID = id;
            entity.ViolationType = ViolationType;
            entity.ViolationMsg = ViolationMsg;
            entity.IsProcess = 0;
            entity.Create();
            this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// ���복��������Ϣ
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CarviolationEntity entity)
        {
            BaseRepository().Insert(entity);
        }

        #endregion
    }
}
