using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;

namespace ERCHTMS.Service.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�����Ԥ����
    /// </summary>
    public class LaboreamyjService : RepositoryFactory<LaboreamyjEntity>, LaboreamyjIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LaboreamyjEntity> GetList(string queryJson)
        {
            string orgcode=OperatorProvider.Provider.Current().OrganizeCode;
            //�ҵ�����Ŀǰ�������µ�����
            return this.BaseRepository().IQueryable(it => it.CreateUserOrgCode == orgcode).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LaboreamyjEntity GetEntity(string keyValue)
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
        public void SaveForm(string json)
        {
            json = json.Replace("&nbsp;", "");
            List<LaboreamyjEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LaboreamyjEntity>>(json);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].ID=list[i].ID.Replace("&nbsp;", "");
                if (list[i].ID == "")
                {
                    list[i].Create();
                    BaseRepository().Insert(list[i]);
                }
                else
                {
                    list[i].Modify(list[i].ID);
                    BaseRepository().Update(list[i]);
                }
            }
        }
        #endregion
    }
}
