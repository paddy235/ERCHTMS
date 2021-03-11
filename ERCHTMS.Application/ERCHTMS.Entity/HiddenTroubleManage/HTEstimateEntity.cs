using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患评估表
    /// </summary>
    [Table("BIS_HTESTIMATE")]
    public class HTEstimateEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 隐患编号
        /// </summary>
        /// <returns></returns>
        [Column("HIDCODE")]
        public string HIDCODE { get; set; }
        /// <summary>
        /// 评估人
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEPERSON")]
        public string ESTIMATEPERSON { get; set; }
        /// <summary>
        /// 评估人姓名
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEPERSONNAME")]
        public string ESTIMATEPERSONNAME { get; set; }
        /// <summary>
        /// 评估单位编码
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEDEPARTCODE")]
        public string ESTIMATEDEPARTCODE { get; set; }
        /// <summary>
        /// 评估人单位名称
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEDEPARTNAME")]
        public string ESTIMATEDEPARTNAME { get; set; }
        /// <summary>
        /// 评估时间
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEDATE")]
        public DateTime? ESTIMATEDATE { get; set; }
        /// <summary>
        /// 评估单位
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEDEPART")]
        public string ESTIMATEDEPART { get; set; }
        /// <summary>
        /// 评估等级
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATERANK")]
        public string ESTIMATERANK { get; set; }
        /// <summary>
        /// 评估结果
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATERESULT")]
        public string ESTIMATERESULT { get; set; }
        /// <summary>
        /// 自动增量
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int AUTOID { get; set; }
        /// <summary>
        /// 评估相片
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEPHOTO")]
        public string ESTIMATEPHOTO { get; set; }


        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}