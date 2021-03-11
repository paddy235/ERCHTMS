using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：违章责任人处罚信息
    /// </summary>
    public class LllegalPunishBLL
    {
        private LllegalPunishIService service = new LllegalPunishService();
        private ScoreSetBLL scoresetbll = new ScoreSetBLL();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalPunishEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalPunishEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        #region 获取考核记录集合对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public List<LllegalPunishEntity> GetListByLllegalId(string LllegalId, string type)
        {
            return service.GetListByLllegalId(LllegalId, type);
        }
        #endregion

        #region 获取考核记录实体对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public LllegalPunishEntity GetEntityByBid(string LllegalId)
        {
            return service.GetEntityByBid(LllegalId);
        }
        #endregion

        #region 获取考核记录实体对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public LllegalPunishEntity GetEntityByApproveId(string approveId)
        {
            return service.GetEntityByApproveId(approveId);
        }
        #endregion


        #region 获取考核记录集合对象(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// <summary>
        /// 获取集合(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public int DeleteLllegalPunishList(string LllegalId, string type)
        {
            return service.DeleteLllegalPunishList(LllegalId, type);
        }
        #endregion

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LllegalPunishEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 保存用户积分
        /// <summary>
        /// 保存用户积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="lllegallevel"></param>
        public void SaveUserScore(string userid, string lllegallevel)
        {
            // 一般违章 、较严重违章、严重违章
            var item = new DataItemDetailBLL().GetDataItemListByItemCode(" 'LllegalRegister'").ToList();
            var scoreitem = scoresetbll.GetList("").ToList(); //查询标准
            string personID = userid;
            string itemID = string.Empty;
            decimal score = 0;
            //违章级别ID
            if (!string.IsNullOrEmpty(lllegallevel))
            {
                var detail = new DataItemDetailBLL().GetEntity(lllegallevel);

                if (null != detail)
                {
                    if (item.Count() > 0)
                    {
                        string[] levelids = item.FirstOrDefault().ItemValue.ToString().Split('|');
                        //一般违章
                        if (detail.ItemName == "一般违章")
                        {
                            itemID = levelids[0].ToString();
                        }
                        //较严重违章
                        else if (detail.ItemName == "较严重违章")
                        {
                            itemID = levelids[1].ToString();
                        }
                        else  //严重违章
                        {
                            itemID = levelids[2].ToString();
                        }
                        if (!string.IsNullOrEmpty(itemID))
                        {
                            var tempItem = scoreitem.Where(p => p.Id == itemID).FirstOrDefault();
                            if (null != tempItem)
                            {
                                score = tempItem.ItemType == "加分" ? tempItem.Score.Value : -tempItem.Score.Value;
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(personID))
                {
                    UserScoreEntity us = new UserScoreEntity
                    {
                        UserId = personID,
                        ItemId = itemID,
                        Score = score
                    };
                    new UserScoreBLL().SaveForm("", us);
                }
            }
        }
        #endregion
    }

    public class UserScoreOfLevel
    {

    }
}