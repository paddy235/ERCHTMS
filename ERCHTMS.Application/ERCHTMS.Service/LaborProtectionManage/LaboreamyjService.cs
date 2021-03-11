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
    /// 描 述：劳动防护预警表
    /// </summary>
    public class LaboreamyjService : RepositoryFactory<LaboreamyjEntity>, LaboreamyjIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LaboreamyjEntity> GetList(string queryJson)
        {
            string orgcode=OperatorProvider.Provider.Current().OrganizeCode;
            //找到所有目前本机构下的数据
            return this.BaseRepository().IQueryable(it => it.CreateUserOrgCode == orgcode).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LaboreamyjEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
