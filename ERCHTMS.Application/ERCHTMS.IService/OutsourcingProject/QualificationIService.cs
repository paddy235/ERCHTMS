using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质证件
    /// </summary>
    public interface QualificationIService
    {
        /// <summary>
        /// 获取资质证件列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetZzzjPageJson(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取资质证件实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        QualificationEntity GetZzzjFormJson(string keyValue);
        /// <summary>
        /// 保存资质证件表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveZzzjForm(string keyValue, QualificationEntity entity);
        /// <summary>
        /// 删除资质证件信息
        /// </summary>
        /// <param name="keyValue"></param>
        void RemoveZzzjForm(string keyValue);
        IEnumerable<QualificationEntity> GetList();
    }
}
