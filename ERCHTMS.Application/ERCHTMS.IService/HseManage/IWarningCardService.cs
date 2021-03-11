using ERCHTMS.Entity.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HseManage;
using System;
using ERCHTMS.Entity.HseManage.ViewModel;

namespace ERCHTMS.IService.HseManage
{
    /// <summary>
    /// Ãè Êö£ºÔ¤¾¯Ö¸±ê¿¨
    /// </summary>
    public interface IWarningCardService
    {
        List<WarningCardEntity> GetData(string key, int pagesize, int pageindex, out int total);
        void Save(WarningCardEntity model);
        void Remove(string id);
        WarningCardEntity GetDetail(string id);
        List<WarningCardEntity> GetMine(string userId, int pageSize, int pageIndex, out int total);
        List<WarningCardEntity> GetList(string[] deptId, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total);
        List<HseKeyValue> GetAQBData(string year, string deptId);
        List<HseKeyValue> GetWarningCardCount(List<string> deptIds, string start, string end);
        List<HseKeyValue> GetCYDData(string year, string deptId);
    }
}
