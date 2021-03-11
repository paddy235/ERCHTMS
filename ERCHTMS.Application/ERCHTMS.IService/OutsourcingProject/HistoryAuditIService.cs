﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    public interface HistoryAuditIService
    {

        HistoryAudit GetEntity(string keyValue);
        DataTable GetAuditRecList(string keyValue);
        DataTable GetHisAuditRecList(string keyValue);
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
        void SaveForm(string keyValue, HistoryAudit entity);
    }
}
