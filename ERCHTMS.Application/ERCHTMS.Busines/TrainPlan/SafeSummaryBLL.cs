using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.TrainPlan;
using ERCHTMS.Service.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.TrainPlan
{
    public class SafeSummaryBLL
    {
        private ISafeSummaryService service = new SafeSummaryService();
        private ISafeMeasureService safeMeasureService = new SafeMeasureService();

        /// <summary>
        /// 安措计划总结报告列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeSummaryEntity GetFormJson(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 检查是否提交
        /// </summary>
        /// <param name="belongYear"></param>
        /// <param name="quarter"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public bool CheckExists(string keyValue, SafeSummaryEntity entity)
        {
            return service.CheckExists(keyValue, entity);
        }

        /// <summary>
        /// 保存/提交
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="postState">0:保存 1:提交</param>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        public void SaveForm(string keyValue, string postState, SafeSummaryEntity entity, List<SafeMeasureEntity> list)
        {
            foreach (var item in list)
            {
                item.ReportID = keyValue;
                safeMeasureService.ChangeFinishData(postState,item);
            }
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            entity.State = Convert.ToInt32(postState);
            if (postState == "1")
            {
                entity.SubmitTime = DateTime.Now;
            }
            service.SaveForm(keyValue, entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public void DeleteForm(string keyValue)
        {
            //清空保存的实际完成时间，实际费用
            SafeMeasureEntity entity = new SafeMeasureEntity()
            {
                ReportID = keyValue
            };
            safeMeasureService.ChangeFinishData("3", entity);
            service.DeleteForm(keyValue);
        }
    }
}
