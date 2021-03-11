using ERCHTMS.Entity.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HseManage;
using System;

namespace ERCHTMS.IService.HseManage
{
    /// <summary>
    /// Ãè Êö£ºÔ¤¾¯Ö¸±ê¿¨
    /// </summary>
    public interface ICheckRecordService
    {
        List<CheckRecordEntity> GetData(string userid, int pagesize, int pageindex, out int total);
        void Save(CheckRecordEntity model);
        void Remove(string id);
        CheckRecordEntity GetDetail(string id);
        List<CheckRecordEntity> GetMine(string userId, int pageSize, int pageIndex, out int total);
        List<CheckRecordEntity> GetList(string[] deptId, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total);
        List<CheckRecordEntity> GetList(string[] deptid, string checkuser, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total);
    }
}
