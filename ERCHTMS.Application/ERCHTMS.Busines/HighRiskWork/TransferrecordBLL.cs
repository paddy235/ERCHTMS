using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：转交记录表
    /// </summary>
    public class TransferrecordBLL
    {
        private TransferrecordIService service = new TransferrecordService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TransferrecordEntity> GetList(Expression<Func<TransferrecordEntity, bool>> condition)
        {
            return service.GetList(condition);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TransferrecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
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
        public void SaveForm(string keyValue, TransferrecordEntity entity)
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

        /// <summary>
        /// 保存表单(先将相关业务ID跟流程节点ID的数据设置失效，接着在插入一条整体转交的数据)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveRealForm(string keyValue, TransferrecordEntity entity)
        {
            try
            {
                TransferrecordEntity realentity = service.GetList(t => t.RecId == entity.RecId && t.FlowId == entity.FlowId && t.Disable == 0).FirstOrDefault();
                if (realentity != null)
                {
                    realentity.InTransferUserAccount += (entity.InTransferUserAccount + ",");
                    realentity.InTransferUserId += (entity.InTransferUserId + ",");
                    realentity.InTransferUserName += (entity.InTransferUserName + ",");
                    //当实际转交接收人中存在当前申请人时，转交接收人剔除当前申请转交的账号
                    realentity.InTransferUserAccount = realentity.InTransferUserAccount.Contains(entity.OutTransferUserAccount) ? realentity.InTransferUserAccount.Replace(entity.OutTransferUserAccount + ",", "") : realentity.InTransferUserAccount;
                    realentity.InTransferUserId = realentity.InTransferUserId.Contains(entity.OutTransferUserId) ? realentity.InTransferUserId.Replace(entity.OutTransferUserId + ",", "") : realentity.InTransferUserId;
                    realentity.InTransferUserName = realentity.InTransferUserName.Contains(entity.OutTransferUserName) ? realentity.InTransferUserName.Replace(entity.OutTransferUserName + ",", "") : realentity.InTransferUserName;
                    realentity.OutTransferUserAccount += (entity.OutTransferUserAccount + ",");
                    realentity.OutTransferUserId += (entity.OutTransferUserId + ",");
                    realentity.OutTransferUserName += (entity.OutTransferUserName + ",");

                    //当当前转交接收人存在于实际转交申请人时(及当前当前转交接收人以前做过转交申请),将实际转交申请人中的这个账号剔除
                    string[] InTransferUserAccountList = entity.InTransferUserAccount.Split(',');
                    foreach (var item in InTransferUserAccountList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.OutTransferUserAccount.Contains(item + ","))
                        {
                            realentity.OutTransferUserAccount = realentity.OutTransferUserAccount.Replace(item + ",", "");
                        }
                    }

                    string[] InTransferUserIdList = entity.InTransferUserId.Split(',');
                    foreach (var item in InTransferUserIdList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.OutTransferUserId.Contains(item + ","))
                        {
                            realentity.OutTransferUserId = realentity.OutTransferUserId.Replace(item + ",", "");
                        }
                    }

                    string[] InTransferUserNameList = entity.InTransferUserName.Split(',');
                    foreach (var item in InTransferUserNameList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.OutTransferUserName.Contains(item + ","))
                        {
                            realentity.OutTransferUserName = realentity.OutTransferUserName.Replace(item + ",", "");
                        }
                    }

                    //当转交申请人存在于转交接收人时，将转交申请人中的这个账号剔除
                    string[] OutTransferUserAccountList = realentity.OutTransferUserAccount.Split(',');
                    foreach (var item in OutTransferUserAccountList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.InTransferUserAccount.Contains(item + ","))
                        {
                            realentity.InTransferUserAccount = realentity.InTransferUserAccount.Replace(item + ",", "");
                        }
                    }

                    string[] OutTransferUserIdList = realentity.OutTransferUserId.Split(',');
                    foreach (var item in OutTransferUserIdList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.InTransferUserId.Contains(item + ","))
                        {
                            realentity.InTransferUserId = realentity.InTransferUserId.Replace(item + ",", "");
                        }
                    }

                    string[] OutTransferUserNameList = realentity.OutTransferUserName.Split(',');
                    foreach (var item in OutTransferUserNameList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.InTransferUserName.Contains(item + ","))
                        {
                            realentity.InTransferUserName = realentity.InTransferUserName.Replace(item + ",", "");
                        }
                    }
                    
                    service.SaveForm(realentity.Id, realentity);
                    entity.Disable = 1;
                    service.SaveForm(keyValue, entity);
                }
                else
                {
                    entity.Disable = 1;
                    entity.OutTransferUserAccount += ",";
                    entity.OutTransferUserId += ",";
                    entity.OutTransferUserName += ",";
                    entity.InTransferUserAccount += ",";
                    entity.InTransferUserId += ",";
                    entity.InTransferUserName += ",";
                    service.SaveForm(keyValue, entity);
                    entity.Disable = 0;
                    entity.Id = "";
                    service.SaveForm(keyValue, entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion
    }
}
