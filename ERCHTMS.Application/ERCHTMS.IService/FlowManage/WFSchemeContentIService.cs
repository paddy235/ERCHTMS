﻿using ERCHTMS.Entity.FlowManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.FlowManage
{
    /// <summary>
    /// 描 述：工作流模板内容表操作接口
    /// </summary>
    public interface WFSchemeContentIService
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="contentid">关联id</param>
        /// <param name="version">模板版本号</param>
        /// <returns></returns>
        WFSchemeContentEntity GetEntity(string contentid, string version);

        WFSchemeContentEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="wfSchemeInfoId">工作流模板信息表Id</param>
        /// <returns></returns>
        IEnumerable<WFSchemeContentEntity> GetEntityList(string wfSchemeInfoId);
        /// <summary>
        /// 获取对象列表（不包括模板内容）
        /// </summary>
        /// <param name="wfSchemeInfoId">工作流模板信息表Id</param>
        /// <returns></returns>
        DataTable GetTableList(string wfSchemeInfoId);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        int SaveEntity(WFSchemeContentEntity entity, string keyValue);

        int RemoveEntity(string keyValue);
    }
}
