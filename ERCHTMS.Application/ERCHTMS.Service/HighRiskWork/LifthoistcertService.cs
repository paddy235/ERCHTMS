using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using ERCHTMS.Code;
using System;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Common;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� �������֤
    /// </summary>
    public class LifthoistcertService : RepositoryFactory<LifthoistcertEntity>, LifthoistcertIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="page">��ҳ����</param>
        /// <param name="search">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination page, LifthoistSearchModel search)
        {
            DatabaseType dataTye = DatabaseType.Oracle;

            #region ���
            page.p_kid = "a.id";
            page.p_fields = @"a.applyuserid,a.applyusername,a.applycompanyname,a.applydate,a.applycode,a.applycodestr,a.toolname as toolvalue,b.itemname as toolname,
                                a.constructionunitid,a.constructionunitname,a.constructionunitcode,a.constructionaddress,a.workstartdate,a.workenddate,a.auditstate,NVL(CHARGEPERSONSIGN,0) AS issign,
                                a.flowid,a.flowname,a.flowroleid,a.flowrolename,a.flowdeptid,a.flowdeptname";
            page.p_tablename = @"bis_lifthoistcert a left join base_dataitemdetail b on a.toolname=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='ToolName')";
            #endregion

            #region  ɸѡ����
            //��ҵ״̬
            if (!string.IsNullOrEmpty(search.auditstate))
            {
                page.conditionJson += " and a.auditstate in (" + search.auditstate + ")";
            }
            //��ҵʱ��
            if (!string.IsNullOrEmpty(search.workstartdate))
            {
                page.conditionJson += string.Format(" and a.WorkStartDate >= to_date('{0}','yyyy-MM-dd')", search.workstartdate);
            }
            if (!string.IsNullOrEmpty(search.workenddate))
            {
                page.conditionJson += string.Format(" and a.WorkEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(search.workenddate).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //������
            if (!string.IsNullOrEmpty(search.applycode))
            {
                page.conditionJson += " and a.applycode like '%" + search.applycode + "%'";
            }
            //��ҵ��λ
            if (!string.IsNullOrEmpty(search.constructionunitid))
            {
                page.conditionJson += " and a.constructionunitid = '" + search.constructionunitid + "'";
            }
            if (!string.IsNullOrEmpty(search.toolname))
            {
                page.conditionJson += " and a.toolname= '" + search.toolname + "'";
            }
            if (!string.IsNullOrEmpty(search.viewrange))
            {
                var user = OperatorProvider.Provider.Current();
                //����
                if (search.viewrange.ToLower() == "self")
                {
                    page.conditionJson += string.Format(" and a.ApplyUserId='{0}'", user.UserId);
                }
                else if (search.viewrange == "selfaudit")
                {
                    string[] roles = user.RoleName.Split(',');
                    int count = 1;
                    string roleWhere = "";
                    foreach (string str in roles)
                    {
                        string strunion = " union";
                        if (roles.Length == count)
                        {
                            strunion = "";
                        }
                        //���״̬Ϊȷ����
                        roleWhere += string.Format(@"   select  distinct t.id from bis_lifthoistcert t  where t.flowdeptid  like'%{0}%' and t.flowrolename like '%{1}%' and  t.AuditState ='{3}' {2}", user.DeptId, str, strunion, search.auditstate);
                        count++;
                    }
                    page.conditionJson += string.Format(" and a.id in ({0})", roleWhere);
                }
            }

            #endregion
            return this.BaseRepository().FindTableByProcPager(page, dataTye);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LifthoistcertEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public string GetMaxCode()
        {
            string orgCode = OperatorProvider.Provider.Current().OrganizeCode;
            string sql = "select max(ApplyCode) from bis_lifthoistcert where to_char(CreateDate,'MM') = @month";
            object o = this.BaseRepository().FindObject(sql, new DbParameter[]{
                DbParameters.CreateDbParameter("@month", DateTime.Now.ToString("MM"))
            });
            if (o == null || o.ToString() == "")
                return DateTime.Now.ToString("yyyyMM") + "001";
            int num = Convert.ToInt32(o.ToString().Substring(6));
            num++;
            return DateTime.Now.ToString("yyyyMM") + num.ToString().PadLeft(3, '0');
        }

        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <summary>
        public void RemoveForm(string keyValue)
        {
            try
            {
                this.BaseRepository().BeginTrans();
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_lifthoistcert where ID = '{0}'", keyValue));
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_lifthoistsafety where LIFTHOISTCERTID = '{0}'", keyValue));
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_lifthoistauditrecord where BUSINESSID = '{0}'", keyValue));
                this.BaseRepository().Commit();
            }
            catch (Exception ex)
            {
                this.BaseRepository().Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LifthoistcertEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<LifthoistsafetyEntity> safetyList = entity.safetys;
                //��Ϊnull,�������ݿ���±���
                entity.safetys = null;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update(entity);
                }
                else
                {
                    entity.Create();
                    entity.APPLYCODE = this.GetMaxCode();
                    entity.APPLYCODESTR = "Q/CRPHZHB 2205.10.02-JL08-" + entity.APPLYCODE;
                    res.Insert(entity);
                }
                SaveSafetys(entity.ID, safetyList, res);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// ��˸���
        /// </summary>
        /// <param name="certEntity">ƾ��֤ʵ��</param>
        /// <param name="auditEntity">���ʵ��</param>
        public void ApplyCheck(LifthoistcertEntity certEntity, LifthoistauditrecordEntity auditEntity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<LifthoistsafetyEntity> safetyList = certEntity.safetys;
                //��Ϊnull,�������ݿ���±���
                certEntity.safetys = null;
                //���ص�װ��ҵֱ�Ӹ���
                res.Update(certEntity);
                //���ʵ�岻Ϊ��ʱ���Ų���
                if (auditEntity != null)
                {
                    auditEntity.Create();
                    auditEntity.BUSINESSID = certEntity.ID;
                    res.Insert(auditEntity);
                }
                SaveSafetys(certEntity.ID, safetyList, res);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        private void SaveSafetys(string certid, List<LifthoistsafetyEntity> safetys, IDatabase res)
        {
            //�жϰ�ȫ��ʩ�Ƿ����
            if (safetys == null || safetys.Count == 0)
            {
                IEnumerable<LifthoistsafetyEntity> dbsafetys = new LifthoistsafetyService().GetList(string.Format("LIFTHOISTCERTID = '{0}'", certid));
                if (dbsafetys == null || dbsafetys.Count() == 0)
                {
                    //��ȫ��ʩ��
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    List<HighProjectSetEntity> baseprojects = new HighProjectSetService().GetList(" and typenum = -2 and  createuserorgcode = " + user.OrganizeCode).ToList();
                    if (baseprojects == null || baseprojects.Count == 0)
                    {
                        baseprojects = new HighProjectSetService().GetList(" and typenum = -2 and  createuserorgcode = '00'").ToList();
                    }
                    if (baseprojects != null && baseprojects.Count() > 0)
                    {
                        safetys = new List<LifthoistsafetyEntity>();

                        foreach (var item in baseprojects)
                        {
                            string[] names = item.MeasureName.Split('|');
                            int order = 0;
                            int.TryParse(item.OrderNumber, out order);
                            safetys.Add(new LifthoistsafetyEntity()
                            {
                                ITEMNAME = names[0],
                                ITEMVALUE = names[1],
                                SORTNUM = order
                            });
                        }
                    }
                }
            }
            if (safetys != null && safetys.Count > 0)
            {
                foreach (var safety in safetys)
                {
                    safety.LIFTHOISTCERTID = certid;
                    if (!string.IsNullOrEmpty(safety.ID))
                    {
                        safety.Modify(safety.ID);
                        res.Update(safety);
                    }
                    else
                    {
                        safety.Create();
                        res.Insert(safety);
                    }
                }
            }
        }

        #endregion
    }
}
