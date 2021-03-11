﻿using ERCHTMS.Entity.Observerecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.Observerecord
{
    public interface ObsFeedBackFBIService
    {
        ObsFeedBackFBEntity GetEntity(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ObsFeedBackFBEntity entity);
    }
}
