using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.LllegalManage;
using BSFramework.Util.WebControl;
using System.Data;
using ERCHTMS.Entity.LllegalManage.ViewModel;

namespace ERCHTMS.IService.LllegalManage
{
    /// <summary>
    /// 描 述：违章基本信息
    /// </summary>
    public interface LllegalRegisterIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LllegalRegisterEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LllegalRegisterEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取通用查询分页
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        DataTable GetGeneralQuery(Pagination pagination);

        DataTable GetListByCheckId(string checkId, string checkman, string flowstate);

        /// <summary>
        /// 获取个人(反)违章档案
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable GetLllegalForPersonRecord(string userId);

        #region 获取新编码
        /// <summary>
        /// 获取新编码
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="maxfields"></param>
        /// <param name="seriallen"></param>
        /// <returns></returns>
        string GenerateHidCode(string tablename, string maxfields, int seriallen);
        #endregion

        #region 通过违章编号，来判断是否存在重复现象
        /// <summary>
        /// 通过违章编号，来判断是否存在重复现象
        /// </summary>
        /// <param name="LllegalNumber"></param>
        /// <returns></returns>
        IList<LllegalRegisterEntity> GetListByNumber(string LllegalNumber);
        #endregion

        #region 通过当前用户获取对应违章的违章描述(取前十个)
        /// <summary>
        /// 通过当前用户获取对应违章的违章描述
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable GetLllegalDescribeList(string userId, string lllegaldescribe);
        #endregion


        #region 获取违章积分数据
        /// <summary>
        /// 获取违章积分数据
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="year"></param>
        /// <param name="userids"></param>
        /// <returns></returns>
        DataTable GetLllegalPointData(string basePoint, string year, string userids, string condition);
        #endregion

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, LllegalRegisterEntity entity);

        #region 删除违章相关所有内容
        /// <summary>
        /// 删除违章相关所有内容
        /// </summary>
        /// <param name="keyValue"></param>
        void RemoveFormByBid(string keyValue);
        #endregion


        #endregion

        #region  违章基础信息查询
        /// <summary>
        /// 违章基础信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetLllegalBaseInfo(Pagination pagination, string queryJson);
        #endregion

        #region 违章实体所有元素对象
        /// <summary>
        /// 违章实体所有元素对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetLllegalModel(string keyValue);
        #endregion

        #region 获取违章档案(班组端)
        /// <summary>
        /// 获取违章档案(班组端)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        LllegalRecord GetLllegalRecord(string userid, string year);
        #endregion

        #region 获取违章曝光
        /// <summary>
        /// 获取违章曝光
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        DataTable QueryExposureLllegal(string num);
        #endregion

        #region 通过安全检查id获取对应的违章统计数据
        /// <summary>
        /// 通过安全检查id获取对应的违章统计数据
        /// </summary>
        /// <param name="checkids"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetLllegalBySafetyCheckIds(List<string> checkids, int mode);
        #endregion
    }
}